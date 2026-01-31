using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Key : MonoBehaviour
{
    [SerializeField] private Image keyImage;
    [SerializeField] private CanvasGroup keyGroup;

    [Header("Gain")]
    [SerializeField] private float gainFadeTime = 0.12f;
    [SerializeField] private float gainPopTime = 0.12f;
    [SerializeField] private float gainPopScale = 1.2f;

    [Header("Use")]
    [SerializeField] private float useShakeTime = 0.15f;
    [SerializeField] private float useShakeStrength = 10f;
    [SerializeField] private float useFadeTime = 0.12f;

    private RectTransform rect;
    private Vector2 originalPos;
    private Vector3 originalScale;
    private Coroutine running;

    private void Awake()
    {
        if (keyImage == null) keyImage = GetComponent<Image>();
        if (keyGroup == null) keyGroup = GetComponent<CanvasGroup>();

        rect = keyImage.rectTransform;
        originalPos = rect.anchoredPosition;
        originalScale = rect.localScale;

        SetVisible(false, instant: true);
    }

    public void KeyGained()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(GainRoutine());
    }

    public void KeyUsed()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(UsedRoutine());
    }

    private IEnumerator GainRoutine()
    {
        SetVisible(true, instant: true);

        rect.localScale = originalScale * gainPopScale;
        keyGroup.alpha = 0f;

        float t = 0f;
        while (t < gainFadeTime)
        {
            t += Time.deltaTime;
            keyGroup.alpha = Mathf.Lerp(0f, 1f, t / gainFadeTime);
            yield return null;
        }
        keyGroup.alpha = 1f;

        t = 0f;
        Vector3 start = rect.localScale;
        Vector3 end = originalScale;
        while (t < gainPopTime)
        {
            t += Time.deltaTime;
            float k = t / gainPopTime;
            rect.localScale = Vector3.Lerp(start, end, 1f - Mathf.Pow(1f - k, 3f));
            yield return null;
        }
        rect.localScale = originalScale;

        running = null;
    }

    private IEnumerator UsedRoutine()
    {
        if (keyGroup.alpha <= 0.001f)
        {
            SetVisible(false, instant: true);
            running = null;
            yield break;
        }

        float t = 0f;
        while (t < useShakeTime)
        {
            t += Time.deltaTime;
            Vector2 r = Random.insideUnitCircle * useShakeStrength;
            rect.anchoredPosition = originalPos + r;
            yield return null;
        }
        rect.anchoredPosition = originalPos;

        float startAlpha = keyGroup.alpha;
        t = 0f;
        while (t < useFadeTime)
        {
            t += Time.deltaTime;
            keyGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / useFadeTime);
            yield return null;
        }
        keyGroup.alpha = 0f;

        SetVisible(false, instant: true);
        running = null;
    }

    private void SetVisible(bool visible, bool instant)
    {
        keyImage.enabled = visible;

        if (keyGroup != null)
        {
            if (instant) keyGroup.alpha = visible ? 1f : 0f;
            keyGroup.blocksRaycasts = visible;
            keyGroup.interactable = visible;
        }

        if (!visible)
        {
            rect.anchoredPosition = originalPos;
            rect.localScale = originalScale;
        }
    }
}
