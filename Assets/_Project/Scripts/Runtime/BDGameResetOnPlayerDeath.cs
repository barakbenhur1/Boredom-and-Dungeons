using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDGameResetOnPlayerDeath : MonoBehaviour
    {
        [SerializeField] private float resetDelay = 2.25f;
        [SerializeField] private bool resetSceneOnDeath = true;

        private BDHealth health;
        private bool resetStarted;
        private string message = "";

        private void Awake()
        {
            health = GetComponent<BDHealth>();
            if (health != null)
                health.Died += OnDied;
        }

        private void OnDied(BDHealth dead)
        {
            if (resetStarted || !resetSceneOnDeath)
                return;

            resetStarted = true;
            StartCoroutine(ResetAfterDelay());
        }

        private IEnumerator ResetAfterDelay()
        {
            float remaining = resetDelay;

            while (remaining > 0f)
            {
                message = $"Restarting in {remaining:0.0}s";
                remaining -= Time.deltaTime;
                yield return null;
            }

            Scene active = SceneManager.GetActiveScene();

            if (!string.IsNullOrEmpty(active.name))
                SceneManager.LoadScene(active.name);
            else
                SceneManager.LoadScene(0);
        }

        private void OnGUI()
        {
            if (!resetStarted)
                return;

            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 22,
                fontStyle = FontStyle.Bold
            };
            style.normal.textColor = Color.white;

            GUI.Label(new Rect(0f, Screen.height * 0.34f + 82f, Screen.width, 32f), message, style);
        }

        private void OnDestroy()
        {
            if (health != null)
                health.Died -= OnDied;
        }
    }
}
