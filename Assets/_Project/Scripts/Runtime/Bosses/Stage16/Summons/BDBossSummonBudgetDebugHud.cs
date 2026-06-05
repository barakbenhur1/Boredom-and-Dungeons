using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossSummonBudgetDebugHud : MonoBehaviour
    {
        [SerializeField] private BDBossSharedSummonBudget budget;
        [SerializeField] private bool visibleInDevelopmentBuild = true;
        [SerializeField] private Vector2 position = new Vector2(18f, 180f);

        private GUIStyle style;

        private void Awake()
        {
            ResolveBudget();
        }

        private void OnGUI()
        {
            if (!ShouldShow())
                return;

            ResolveBudget();

            if (budget == null)
                return;

            EnsureStyle();

            Rect rect = new Rect(
                position.x,
                position.y,
                250f,
                30f
            );

            GUI.Label(
                rect,
                $"Summons: {budget.AliveSummons}/" +
                $"{budget.MaximumAliveSummons}",
                style
            );
        }

        private bool ShouldShow()
        {
#if UNITY_EDITOR
            return visibleInDevelopmentBuild;
#else
            return visibleInDevelopmentBuild &&
                   Debug.isDebugBuild;
#endif
        }

        private void ResolveBudget()
        {
            if (budget == null)
            {
                budget =
                    GetComponent<BDBossSharedSummonBudget>();
            }
        }

        private void EnsureStyle()
        {
            if (style != null)
                return;

            style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold
            };

            style.normal.textColor = Color.white;
        }
    }
}
