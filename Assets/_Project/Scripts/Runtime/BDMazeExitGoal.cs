using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMazeExitGoal : MonoBehaviour
    {
        [SerializeField] private bool showDebugOverlay = false;

        private bool reached;
        private string message = "Find the top exit.";

        public bool Reached => reached;

        private void OnTriggerEnter(Collider other)
        {
            if (reached)
                return;

            BDPlayerMarker player = other.GetComponentInParent<BDPlayerMarker>();
            if (player == null)
                return;

            reached = true;
            message = "Maze exit reached.";
            Debug.Log("B&D Maze: exit reached.");
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 285, 120, 270, 72), "Maze Goal");
            GUI.Label(new Rect(Screen.width - 273, 150, 245, 22), message);
            GUI.Label(new Rect(Screen.width - 273, 172, 245, 22), $"Reached: {reached}");
        }
    }
}
