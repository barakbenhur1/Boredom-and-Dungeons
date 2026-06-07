using UnityEngine;

namespace BoredomAndDungeons
{
    /// <summary>
    /// Deprecated compatibility component. Visible doorway effects are owned by
    /// BDPortalSurfaceEffect and may only add a soft light surface to authored
    /// entrance/exit geometry. No frame cubes or replacement doors are created.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BDDoorwayPortalVisual : MonoBehaviour
    {
        // BD NO GENERATED DOOR FRAME V13
        private void Awake()
        {
            enabled = false;
        }
    }
}
