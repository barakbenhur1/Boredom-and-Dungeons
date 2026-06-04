using UnityEngine;

namespace BoredomAndDungeons.Environment.NaturalMap
{
    [DisallowMultipleComponent]
    public sealed class BDNaturalMapVisualMarker : MonoBehaviour
    {
        [SerializeField] private bool decorativeOnly = true;
        [SerializeField] private string visualType = "natural-map-shape";

        public bool DecorativeOnly => decorativeOnly;
        public string VisualType => visualType;
    }
}
