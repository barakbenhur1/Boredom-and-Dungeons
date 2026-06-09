namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void ApplySharedTactileProfile()
        {
            if (tactileReady)
                return;

            BDModernHandheldControlTarget[] controls =
                deviceRoot.GetComponentsInChildren<
                    BDModernHandheldControlTarget>(true);

            for (int index = 0; index < controls.Length; index++)
            {
                BDModernHandheldControlTarget control = controls[index];
                if (control == null ||
                    control.Action ==
                    BDModernHandheldControlTarget.ControlAction.ScreenItem)
                {
                    continue;
                }

                control.SetTactileProfile(0.12f, 8.5f, 0.075f);
            }

            tactileReady = true;
        }
    }
}
