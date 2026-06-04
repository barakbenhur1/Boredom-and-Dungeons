using UnityEngine;

namespace BoredomAndDungeons
{
    public interface IBDBossAttackTelegraph
    {
        bool IsTelegraphActive { get; }
        void BeginTelegraph(Vector3 origin, Vector3 direction, float duration);
        void CancelTelegraph();
    }
}
