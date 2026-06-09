using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void AlignScreenArea(
            int index,
            float x,
            float y,
            float width,
            float height)
        {
            BDModernHandheldControlTarget[] areas =
                deviceRoot.GetComponentsInChildren<
                    BDModernHandheldControlTarget>(true);

            for (int areaIndex = 0;
                 areaIndex < areas.Length;
                 areaIndex++)
            {
                BDModernHandheldControlTarget area = areas[areaIndex];
                if (area == null ||
                    area.Action !=
                    BDModernHandheldControlTarget.ControlAction.ScreenItem ||
                    area.Index != index)
                {
                    continue;
                }

                area.transform.localPosition = new Vector3(
                    x / CanvasSize.x * ScreenWidth,
                    ScreenCenterY +
                    y / CanvasSize.y * ScreenHeight,
                    -0.27f
                );
                area.transform.localScale = new Vector3(
                    width / CanvasSize.x * ScreenWidth,
                    height / CanvasSize.y * ScreenHeight,
                    0.08f
                );
                return;
            }
        }
    }
}
