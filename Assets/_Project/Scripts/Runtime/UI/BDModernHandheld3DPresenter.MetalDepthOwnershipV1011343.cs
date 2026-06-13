using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD METAL DEPTH OWNER REPAIR V10.11.30.43
        //
        // Two different transient depth owners could remain while the product
        // presentation was visible:
        // 1. scene Game cameras still targeting the same backbuffer;
        // 2. the product key light allocating a Metal shadow-map surface.
        //
        // The handheld already owns the full visible frame and already ships
        // authored device/furniture contact shadows. Keep one backbuffer camera
        // owner and avoid the redundant Metal shadow-map depth pass.

        private readonly Dictionary<Camera, bool>
            competingGameCameraStatesV1011343 =
                new Dictionary<Camera, bool>();

        private bool handheldRenderOwnershipActiveV1011343;

        private void SetHandheldRenderOwnershipV1011343(bool active)
        {
            if (active)
            {
                handheldRenderOwnershipActiveV1011343 = true;
                MaintainHandheldRenderOwnershipV1011343();
                return;
            }

            RestoreCompetingGameCamerasV1011343();
        }

        private void MaintainHandheldRenderOwnershipV1011343()
        {
            if (!handheldRenderOwnershipActiveV1011343)
                return;

            Camera[] cameras = Camera.allCameras;
            for (int index = 0; index < cameras.Length; index++)
            {
                Camera candidate = cameras[index];
                if (!ShouldSuspendCompetingCameraV1011343(candidate))
                    continue;

                if (!competingGameCameraStatesV1011343.ContainsKey(candidate))
                {
                    competingGameCameraStatesV1011343.Add(
                        candidate,
                        candidate.enabled
                    );
                }

                if (candidate.enabled)
                    candidate.enabled = false;
            }

            RemoveDestroyedCompetingCamerasV1011343();
        }

        private bool ShouldSuspendCompetingCameraV1011343(Camera candidate)
        {
            if (candidate == null ||
                candidate == deviceCamera ||
                candidate == screenCamera)
            {
                return false;
            }

            // Never touch SceneView, Preview, Reflection or VR helper cameras.
            if (candidate.cameraType != CameraType.Game)
                return false;

            // Offscreen cameras do not share the visible backbuffer depth surface.
            if (candidate.targetTexture != null)
                return false;

            return true;
        }

        private void RestoreCompetingGameCamerasV1011343()
        {
            foreach (
                KeyValuePair<Camera, bool> entry
                in competingGameCameraStatesV1011343
            )
            {
                Camera camera = entry.Key;
                if (camera != null)
                    camera.enabled = entry.Value;
            }

            competingGameCameraStatesV1011343.Clear();
            handheldRenderOwnershipActiveV1011343 = false;
        }

        private void RemoveDestroyedCompetingCamerasV1011343()
        {
            if (competingGameCameraStatesV1011343.Count == 0)
                return;

            List<Camera> destroyed = null;
            foreach (
                KeyValuePair<Camera, bool> entry
                in competingGameCameraStatesV1011343
            )
            {
                if (entry.Key != null)
                    continue;

                if (destroyed == null)
                    destroyed = new List<Camera>();
                destroyed.Add(entry.Key);
            }

            if (destroyed == null)
                return;

            for (int index = 0; index < destroyed.Count; index++)
                competingGameCameraStatesV1011343.Remove(destroyed[index]);
        }

        private void ConfigureMetalDepthSurfaceOwnersV1011343()
        {
            if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Metal ||
                presentationRoot == null)
            {
                return;
            }

            Light[] lights =
                presentationRoot.GetComponentsInChildren<Light>(true);

            for (int index = 0; index < lights.Length; index++)
            {
                Light light = lights[index];
                if (light == null ||
                    light.name != "Cinematic Key Light")
                {
                    continue;
                }

                // Hand-authored horizontal device/furniture shadows remain.
                // This only removes the redundant runtime shadow-map depth pass
                // that Metal was allocating as a memoryless surface.
                light.shadows = LightShadows.None;
                light.shadowStrength = 0f;
                break;
            }
        }
    }
}
