using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void LowerDeviceAndShadows()
        {
            if (offsetRoot == null)
            {
                offsetRoot = Find(
                    presentationRoot,
                    "V6 Product Shot Lowering Offset"
                );

                if (offsetRoot == null)
                {
                    offsetRoot = new GameObject(
                        "V6 Product Shot Lowering Offset"
                    ).transform;
                    offsetRoot.SetParent(presentationRoot, false);
                    offsetRoot.localPosition =
                        new Vector3(0f, DeviceYOffset, 0f);
                }
            }

            ReparentKeepingLocal(deviceRoot, offsetRoot);

            Transform shadows = Find(
                presentationRoot,
                "Modern Handheld Table Shadows"
            );
            if (shadows != null)
                ReparentKeepingLocal(shadows, offsetRoot);
        }

        private static void ReparentKeepingLocal(
            Transform child,
            Transform parent)
        {
            if (child == null || parent == null || child.parent == parent)
                return;

            Vector3 position = child.localPosition;
            Quaternion rotation = child.localRotation;
            Vector3 scale = child.localScale;
            child.SetParent(parent, false);
            child.localPosition = position;
            child.localRotation = rotation;
            child.localScale = scale;
        }

        private void PolishPhysicalControls()
        {
            // The original control target owns press travel on local Z and was
            // configured before V6 moved these controls. Keep only the approved
            // X/Y layout pinned every LateUpdate so its tactile Z animation and
            // return timing remain untouched.
            EnforcePlanarPosition(
                "Button Select",
                -0.66f,
                -3.82f
            );
            EnforcePlanarPosition(
                "Button Exit",
                0.66f,
                -3.82f
            );

            if (controlsReady)
                return;

            MovePart(
                "Button Select " + "Hit Target",
                new Vector3(-0.66f, -3.82f, -0.58f)
            );
            MovePart(
                "Button Exit " + "Hit Target",
                new Vector3(0.66f, -3.82f, -0.58f)
            );

            TextMesh[] labels =
                deviceRoot.GetComponentsInChildren<TextMesh>(true);
            for (int index = 0; index < labels.Length; index++)
            {
                TextMesh label = labels[index];
                if (label == null)
                    continue;

                Vector3 position = label.transform.localPosition;
                if (label.text == "SELECT")
                {
                    position.x = -0.66f;
                    position.y = -4.28f;
                    label.transform.localPosition = position;
                }
                else if (label.text == "EXIT")
                {
                    position.x = 0.66f;
                    position.y = -4.28f;
                    label.transform.localPosition = position;
                }
            }

            controlsReady = true;
        }

        private void EnforcePlanarPosition(
            string name,
            float x,
            float y)
        {
            Transform part = Find(deviceRoot, name);
            if (part == null)
                return;

            Vector3 position = part.localPosition;
            position.x = x;
            position.y = y;
            part.localPosition = position;
        }

        private void MovePart(string name, Vector3 position)
        {
            Transform part = Find(deviceRoot, name);
            if (part != null)
                part.localPosition = position;
        }
    }
}
