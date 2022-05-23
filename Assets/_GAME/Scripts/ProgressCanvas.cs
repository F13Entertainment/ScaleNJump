using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class ProgressCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup CG;
    [SerializeField] private RectTransform bar;
    [SerializeField] private float fullWidth;

    private void OnEnable()
    {
        GameController.Instance.OnGameplayEnter.AddListener(Show);
        GameController.Instance.OnGameplayExit.AddListener(Hide);
    }

    private void OnDisable()
    {
        GameController.Instance?.OnGameplayEnter.RemoveListener(Show);
        GameController.Instance?.OnGameplayExit.RemoveListener(Hide);
    }

    private void Update()
    {
        if (GameController.Instance.IsInGame)
            bar.anchoredPosition = new Vector2((Ball.Instance.transform.position.x / LevelContentInfo.Instance.LevelWidth) * fullWidth, bar.anchoredPosition.y);
    }

    private void Show()
    {
        CG.alpha = 1;
    }

    private void Hide()
    {
        CG.alpha = 0;
    }
}
