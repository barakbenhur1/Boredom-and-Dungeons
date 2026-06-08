using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    internal struct BDCameraTransitionSample
    {
        public string Mode;
        public string CameraState;
        public Transform Target;
        public BDMinimapRoom CurrentRoom;
        public BDMinimapRoom PreviousRoom;
        public bool RoomHandoffActive;
        public bool HandoffWallCastApplied;
        public Vector3 CameraPositionBeforeFollow;
        public Quaternion CameraRotationBeforeFollow;
        public Vector3 TargetPosition;
        public Vector3 RawDesiredCameraPosition;
        public Vector3 ContainedDesiredCameraPosition;
        public Vector3 BlendedCameraPosition;
        public Vector3 FinalCameraPosition;
        public Vector3 RawDesiredLookPoint;
        public Vector3 ContainedDesiredLookPoint;
        public Vector3 FinalLookPoint;
        public Quaternion DesiredRotation;
        public Quaternion FinalRotation;
        public float FieldOfView;
    }

    [DefaultExecutionOrder(5100)]
    [DisallowMultipleComponent]
    public sealed class BDCameraTransitionDiagnostics : MonoBehaviour
    {
        // BD V23R6 GATED CAMERA TRANSITION DIAGNOSTICS
        private const int InitialBufferCapacity = 256 * 1024;
        private const int MaximumCapturedFrames = 18000;
        private const string PlayerVisualName = "BD_Player_Visual";
        private const string HorseVisualName = "BD_Horse_BodyVisual";

        private readonly CultureInfo invariant = CultureInfo.InvariantCulture;

        private StringBuilder csv;
        private Transform playerRoot;
        private Transform playerVisual;
        private Animator playerAnimator;
        private Transform horseRoot;
        private Transform horseVisual;
        private Animator horseAnimator;
        private Transform previousTarget;
        private BDMinimapRoom previousCurrentRoom;
        private bool previousHandoffActive;
        private Vector3 previousFinalCameraPosition;
        private Quaternion previousFinalCameraRotation;
        private Vector3 previousTargetPosition;
        private Vector3 previousPlayerRootPosition;
        private Vector3 previousPlayerVisualPosition;
        private Vector3 previousHorseRootPosition;
        private Vector3 previousHorseVisualPosition;
        private float previousFieldOfView;
        private float previousFinalDistance;
        private float nextActorResolveAt;
        private int previousCaptureFrame = -1;
        private int capturedFrames;
        private bool recording;
        private bool pendingUserMarker;
        private string latestEvent = "diagnostics idle";
        private string latestExportPath = string.Empty;

        public bool IsRecording => recording;

        private void Update()
        {
            if (ReadTogglePressed())
            {
                if (recording)
                    StopAndExport();
                else
                    StartRecording();
            }

            if (ReadExportPressed())
                Export();

            if (ReadMarkerPressed())
            {
                pendingUserMarker = true;
                latestEvent = "USER_MARKER pending on next camera sample";
                Debug.Log("B&D V23R6 diagnostics: user marker queued.");
            }
        }

        internal void Capture(BDCameraTransitionSample sample)
        {
            if (!recording)
                return;

            if (capturedFrames >= MaximumCapturedFrames)
            {
                StopAndExport();
                Debug.LogWarning(
                    "B&D V23R6 diagnostics reached the 18000-frame safety limit and stopped."
                );
                return;
            }

            ResolveActors();

            int frame = Time.frameCount;
            bool hasPreviousFrame = previousCaptureFrame == frame - 1;
            Vector3 externalCameraDelta = hasPreviousFrame
                ? sample.CameraPositionBeforeFollow - previousFinalCameraPosition
                : Vector3.zero;
            float externalCameraAngle = hasPreviousFrame
                ? Quaternion.Angle(
                    previousFinalCameraRotation,
                    sample.CameraRotationBeforeFollow
                )
                : 0f;

            Vector3 playerRootPosition =
                playerRoot != null ? playerRoot.position : Vector3.zero;
            Vector3 playerVisualPosition =
                playerVisual != null ? playerVisual.position : Vector3.zero;
            Vector3 horseRootPosition =
                horseRoot != null ? horseRoot.position : Vector3.zero;
            Vector3 horseVisualPosition =
                horseVisual != null ? horseVisual.position : Vector3.zero;

            float desiredDistance = Vector3.Distance(
                sample.RawDesiredCameraPosition,
                sample.TargetPosition
            );
            float containedDistance = Vector3.Distance(
                sample.ContainedDesiredCameraPosition,
                sample.TargetPosition
            );
            float finalDistance = Vector3.Distance(
                sample.FinalCameraPosition,
                sample.TargetPosition
            );
            float pitch = NormalizePitch(sample.FinalRotation.eulerAngles.x);
            float yaw = sample.FinalRotation.eulerAngles.y;

            string eventName = ResolveEventName(
                sample,
                hasPreviousFrame,
                externalCameraDelta,
                externalCameraAngle,
                finalDistance
            );

            AppendSample(
                frame,
                eventName,
                sample,
                externalCameraDelta,
                externalCameraAngle,
                desiredDistance,
                containedDistance,
                finalDistance,
                pitch,
                yaw,
                playerRootPosition,
                playerVisualPosition,
                horseRootPosition,
                horseVisualPosition
            );

            previousCaptureFrame = frame;
            previousTarget = sample.Target;
            previousCurrentRoom = sample.CurrentRoom;
            previousHandoffActive = sample.RoomHandoffActive;
            previousFinalCameraPosition = sample.FinalCameraPosition;
            previousFinalCameraRotation = sample.FinalRotation;
            previousTargetPosition = sample.TargetPosition;
            previousPlayerRootPosition = playerRootPosition;
            previousPlayerVisualPosition = playerVisualPosition;
            previousHorseRootPosition = horseRootPosition;
            previousHorseVisualPosition = horseVisualPosition;
            previousFieldOfView = sample.FieldOfView;
            previousFinalDistance = finalDistance;
            capturedFrames++;
            pendingUserMarker = false;

            if (!string.IsNullOrEmpty(eventName))
                latestEvent = "frame " + frame + ": " + eventName;
        }

        private void StartRecording()
        {
            csv = new StringBuilder(InitialBufferCapacity);
            AppendHeader();
            recording = true;
            capturedFrames = 0;
            previousCaptureFrame = -1;
            previousTarget = null;
            previousCurrentRoom = null;
            previousHandoffActive = false;
            pendingUserMarker = false;
            latestEvent = "recording started";
            Debug.Log(
                "B&D V23R6 diagnostics recording started. F8 stops/exports, F9 exports, F10 marks an observed pulse."
            );
        }

        private void StopAndExport()
        {
            recording = false;
            Export();
            latestEvent = "recording stopped";
        }

        private void Export()
        {
            if (csv == null || csv.Length == 0)
            {
                Debug.LogWarning("B&D V23R6 diagnostics: no captured samples to export.");
                return;
            }

            try
            {
                string directory = Path.Combine(
                    Application.persistentDataPath,
                    "BoredomAndDungeons",
                    "Diagnostics"
                );
                Directory.CreateDirectory(directory);
                latestExportPath = Path.Combine(
                    directory,
                    "V23R6_camera_transition_" +
                    DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", invariant) +
                    ".csv"
                );
                File.WriteAllText(latestExportPath, csv.ToString());
                Debug.Log(
                    "B&D V23R6 diagnostics exported " +
                    capturedFrames +
                    " frames to: " +
                    latestExportPath
                );
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    "B&D V23R6 diagnostics export failed: " + exception.Message
                );
            }
        }

        private void ResolveActors()
        {
            if (Time.unscaledTime < nextActorResolveAt &&
                playerRoot != null &&
                horseRoot != null)
            {
                return;
            }

            nextActorResolveAt = Time.unscaledTime + 0.50f;

            if (playerRoot == null)
                playerRoot = BDTargetFinder.FindPlayer();

            if (playerRoot != null)
            {
                playerVisual = FindDescendant(playerRoot, PlayerVisualName);
                playerAnimator = playerRoot.GetComponentInChildren<Animator>();
            }

            if (horseRoot == null)
            {
                BDHorseController horse =
                    FindFirstObjectByType<BDHorseController>();
                horseRoot = horse != null ? horse.transform : null;
            }

            if (horseRoot != null)
            {
                horseVisual = FindDescendant(horseRoot, HorseVisualName);
                horseAnimator = horseRoot.GetComponentInChildren<Animator>();
            }
        }

        private string ResolveEventName(
            BDCameraTransitionSample sample,
            bool hasPreviousFrame,
            Vector3 externalCameraDelta,
            float externalCameraAngle,
            float finalDistance)
        {
            StringBuilder events = new StringBuilder(96);

            if (pendingUserMarker)
                AppendEvent(events, "USER_MARKER");
            if (sample.Target != previousTarget && previousCaptureFrame >= 0)
                AppendEvent(events, "target-switch");
            if (sample.CurrentRoom != previousCurrentRoom && previousCaptureFrame >= 0)
                AppendEvent(events, "room-switch");
            if (sample.RoomHandoffActive != previousHandoffActive)
            {
                AppendEvent(
                    events,
                    sample.RoomHandoffActive
                        ? "handoff-start"
                        : "handoff-complete"
                );
            }
            if (sample.HandoffWallCastApplied)
                AppendEvent(events, "handoff-wall-cast");
            if ((sample.ContainedDesiredCameraPosition -
                 sample.RawDesiredCameraPosition).sqrMagnitude > 0.0025f)
            {
                AppendEvent(events, "desired-containment");
            }
            if ((sample.FinalCameraPosition -
                 sample.BlendedCameraPosition).sqrMagnitude > 0.0025f)
            {
                AppendEvent(events, "final-containment");
            }
            if (hasPreviousFrame &&
                (externalCameraDelta.sqrMagnitude > 0.0004f ||
                 externalCameraAngle > 0.10f))
            {
                AppendEvent(events, "external-camera-writer");
            }
            if (hasPreviousFrame &&
                Mathf.Abs(finalDistance - previousFinalDistance) > 0.05f)
            {
                AppendEvent(events, "distance-delta");
            }
            if (hasPreviousFrame &&
                Mathf.Abs(sample.FieldOfView - previousFieldOfView) > 0.01f)
            {
                AppendEvent(events, "fov-delta");
            }

            return events.ToString();
        }

        private void AppendSample(
            int frame,
            string eventName,
            BDCameraTransitionSample sample,
            Vector3 externalCameraDelta,
            float externalCameraAngle,
            float desiredDistance,
            float containedDistance,
            float finalDistance,
            float pitch,
            float yaw,
            Vector3 playerRootPosition,
            Vector3 playerVisualPosition,
            Vector3 horseRootPosition,
            Vector3 horseVisualPosition)
        {
            csv.Append(frame).Append(',');
            AppendFloat(Time.unscaledTime);
            csv.Append(',').Append(Sanitize(eventName));
            csv.Append(',').Append(Sanitize(sample.Mode));
            csv.Append(',').Append(Sanitize(sample.CameraState));
            csv.Append(',').Append(NameOf(sample.Target));
            csv.Append(',').Append(RoomOf(sample.CurrentRoom));
            csv.Append(',').Append(RoomOf(sample.PreviousRoom));
            csv.Append(',').Append(sample.RoomHandoffActive ? '1' : '0');
            csv.Append(',').Append(sample.HandoffWallCastApplied ? '1' : '0');
            AppendVector(sample.CameraPositionBeforeFollow);
            AppendVector(externalCameraDelta);
            csv.Append(',');
            AppendFloat(externalCameraAngle);
            AppendVector(sample.TargetPosition);
            AppendVector(sample.RawDesiredCameraPosition);
            AppendVector(sample.ContainedDesiredCameraPosition);
            AppendVector(sample.BlendedCameraPosition);
            AppendVector(sample.FinalCameraPosition);
            csv.Append(',');
            AppendFloat(desiredDistance);
            csv.Append(',');
            AppendFloat(containedDistance);
            csv.Append(',');
            AppendFloat(finalDistance);
            AppendVector(sample.RawDesiredLookPoint);
            AppendVector(sample.ContainedDesiredLookPoint);
            AppendVector(sample.FinalLookPoint);
            csv.Append(',');
            AppendFloat(pitch);
            csv.Append(',');
            AppendFloat(yaw);
            csv.Append(',');
            AppendFloat(sample.FieldOfView);
            AppendVector(playerRootPosition);
            AppendVector(playerVisualPosition);
            AppendVector(playerVisualPosition - playerRootPosition);
            AppendVector(playerRootPosition - previousPlayerRootPosition);
            AppendVector(playerVisualPosition - previousPlayerVisualPosition);
            AppendAnimator(playerAnimator);
            AppendVector(horseRootPosition);
            AppendVector(horseVisualPosition);
            AppendVector(horseVisualPosition - horseRootPosition);
            AppendVector(horseRootPosition - previousHorseRootPosition);
            AppendVector(horseVisualPosition - previousHorseVisualPosition);
            AppendAnimator(horseAnimator);
            AppendVector(sample.TargetPosition - previousTargetPosition);
            csv.AppendLine();
        }

        private void AppendHeader()
        {
            csv.AppendLine(
                "frame,unscaled_time,event,mode,camera_state,target,current_room,previous_room," +
                "handoff_active,handoff_wall_cast," +
                "camera_before_x,camera_before_y,camera_before_z," +
                "external_camera_delta_x,external_camera_delta_y,external_camera_delta_z,external_camera_angle," +
                "target_x,target_y,target_z,raw_desired_x,raw_desired_y,raw_desired_z," +
                "contained_desired_x,contained_desired_y,contained_desired_z," +
                "blended_x,blended_y,blended_z,final_x,final_y,final_z," +
                "desired_distance,contained_distance,final_distance," +
                "raw_look_x,raw_look_y,raw_look_z,contained_look_x,contained_look_y,contained_look_z," +
                "final_look_x,final_look_y,final_look_z,pitch,yaw,fov," +
                "player_root_x,player_root_y,player_root_z,player_visual_x,player_visual_y,player_visual_z," +
                "player_visual_offset_x,player_visual_offset_y,player_visual_offset_z," +
                "player_root_delta_x,player_root_delta_y,player_root_delta_z," +
                "player_visual_delta_x,player_visual_delta_y,player_visual_delta_z," +
                "player_animator,player_apply_root_motion,player_anim_delta_x,player_anim_delta_y,player_anim_delta_z," +
                "horse_root_x,horse_root_y,horse_root_z,horse_visual_x,horse_visual_y,horse_visual_z," +
                "horse_visual_offset_x,horse_visual_offset_y,horse_visual_offset_z," +
                "horse_root_delta_x,horse_root_delta_y,horse_root_delta_z," +
                "horse_visual_delta_x,horse_visual_delta_y,horse_visual_delta_z," +
                "horse_animator,horse_apply_root_motion,horse_anim_delta_x,horse_anim_delta_y,horse_anim_delta_z," +
                "target_delta_x,target_delta_y,target_delta_z"
            );
        }

        private void AppendAnimator(Animator animator)
        {
            csv.Append(',').Append(animator != null ? '1' : '0');
            csv.Append(',').Append(animator != null && animator.applyRootMotion ? '1' : '0');
            AppendVector(animator != null ? animator.deltaPosition : Vector3.zero);
        }

        private void AppendVector(Vector3 value)
        {
            csv.Append(',');
            AppendFloat(value.x);
            csv.Append(',');
            AppendFloat(value.y);
            csv.Append(',');
            AppendFloat(value.z);
        }

        private void AppendFloat(float value)
        {
            csv.Append(value.ToString("0.00000", invariant));
        }

        private static void AppendEvent(StringBuilder events, string value)
        {
            if (events.Length > 0)
                events.Append('|');
            events.Append(value);
        }

        private static string NameOf(Transform value)
        {
            return value != null ? Sanitize(value.name) : "none";
        }

        private static string RoomOf(BDMinimapRoom room)
        {
            if (room == null)
                return "none";

            return room.Cell.x + ":" + room.Cell.y + ":" + Sanitize(room.name);
        }

        private static string Sanitize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "none";

            return value.Replace(',', ';').Replace('\n', ' ').Replace('\r', ' ');
        }

        private static Transform FindDescendant(Transform root, string objectName)
        {
            if (root == null)
                return null;

            if (root.name == objectName)
                return root;

            for (int index = 0; index < root.childCount; index++)
            {
                Transform result = FindDescendant(root.GetChild(index), objectName);
                if (result != null)
                    return result;
            }

            return null;
        }

        private static float NormalizePitch(float pitch)
        {
            return pitch > 180f ? pitch - 360f : pitch;
        }

        private static bool ReadTogglePressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.f8Key.wasPressedThisFrame)
                return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F8))
                return true;
#endif
            return false;
        }

        private static bool ReadExportPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame)
                return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F9))
                return true;
#endif
            return false;
        }

        private static bool ReadMarkerPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.f10Key.wasPressedThisFrame)
                return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F10))
                return true;
#endif
            return false;
        }

        private void OnGUI()
        {
            if (!recording)
                return;

            GUI.Box(
                new Rect(12, Screen.height - 112, 560, 100),
                "V23R6 Camera Transition Diagnostics"
            );
            GUI.Label(
                new Rect(24, Screen.height - 84, 530, 22),
                "RECORDING | F8 stop/export | F9 export | F10 mark visible pulse"
            );
            GUI.Label(
                new Rect(24, Screen.height - 62, 530, 22),
                "Frames: " + capturedFrames + " | Latest: " + latestEvent
            );
            GUI.Label(
                new Rect(24, Screen.height - 40, 530, 22),
                string.IsNullOrEmpty(latestExportPath)
                    ? "Export path: pending"
                    : "Last export: " + latestExportPath
            );
        }
    }
}
