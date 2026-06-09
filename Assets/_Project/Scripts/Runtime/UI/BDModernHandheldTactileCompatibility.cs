using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDModernHandheldTactileCompatibility
    {
        public static void SetTactileProfile<T>(
            this T control,
            float pressDistance,
            float pressSpeed,
            float scaleCompression)
            where T : Component
        {
            if (control == null)
                return;

            string partName = ResolvePartName(
                control.gameObject.name
            );
            if (string.IsNullOrEmpty(partName))
                return;

            Transform root = control.transform;
            while (root.parent != null &&
                   root.name != "BD Modern Upright Handheld")
            {
                root = root.parent;
            }

            Transform part = Find(root, partName);
            if (part == null)
                return;

            BDModernHandheldPressScaleFeedback feedback =
                part.GetComponent<
                    BDModernHandheldPressScaleFeedback>();
            if (feedback == null)
            {
                feedback = part.gameObject.AddComponent<
                    BDModernHandheldPressScaleFeedback>();
            }

            feedback.Configure(
                pressDistance,
                pressSpeed,
                scaleCompression
            );
        }

        private static string ResolvePartName(string controlName)
        {
            if (controlName.Contains("DPad Up"))
                return "DPad Up Cap";
            if (controlName.Contains("DPad Down"))
                return "DPad Down Cap";
            if (controlName.Contains("DPad Left"))
                return "DPad Left Cap";
            if (controlName.Contains("DPad Right"))
                return "DPad Right Cap";
            if (controlName.Contains("Select"))
                return "Button Select";
            if (controlName.Contains("Exit"))
                return "Button Exit";
            if (controlName.Contains("Button X"))
                return "Button X Root";
            if (controlName.Contains("Button Y"))
                return "Button Y Root";
            if (controlName.Contains("Button A"))
                return "Button A Root";
            if (controlName.Contains("Button B"))
                return "Button B Root";
            return string.Empty;
        }

        private static Transform Find(
            Transform root,
            string name)
        {
            if (root == null)
                return null;
            if (root.name == name)
                return root;

            for (int index = 0; index < root.childCount; index++)
            {
                Transform found = Find(root.GetChild(index), name);
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}
