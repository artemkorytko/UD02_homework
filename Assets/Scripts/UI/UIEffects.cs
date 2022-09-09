using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class UIEffects
{
    private const float FADE_EFFECT_DURATION = 1f;
    private const float TEXT_EFFECT_DURATION = 4f;
    private const float MAX_ALPHA = 1f;
    private const float MIN_ALPHA = 0f;
    
    private async UniTask PanelFadeEffect(GameObject panel, float alphaStartValue,
                                          float alphaEndValue, float duration)
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = alphaStartValue;
        await canvasGroup.DOFade(alphaEndValue, duration);
    }

    public async UniTask PanelFadeInEffect(GameObject panel)
    {
        await PanelFadeEffect(panel, MIN_ALPHA, MAX_ALPHA, FADE_EFFECT_DURATION);
    }
    
    public async UniTask PanelFadeOutEffect(GameObject panel)
    {
        await PanelFadeEffect(panel, MAX_ALPHA, MIN_ALPHA, FADE_EFFECT_DURATION);
    }

    public async UniTask TextEffect(Text textObject, string text)
    {
        await textObject.DOText(text, TEXT_EFFECT_DURATION);
    }
}
