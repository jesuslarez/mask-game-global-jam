using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MaskEquipped : MonoBehaviour
{
    [SerializeField] private Image maskUiSprite;
    [SerializeField] private PlayerController player;
    [SerializeField] private PowerUpDatabaseSO database;
    [SerializeField] private TMP_Text maskText;
    [SerializeField] private CanvasGroup maskCanvasGroup;


    [Header("Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength = 0.08f;


    private Vector3 originalLocalPos;
    private bool locked;
    private PowerUpType? lastType;

    private void Awake()
    {
        if (maskUiSprite == null) maskUiSprite = GetComponent<Image>();
        if (player == null) player = FindAnyObjectByType<PlayerController>();

        originalLocalPos = maskUiSprite.transform.localPosition;
        RefreshFromPlayer();
    }

    private void Update()
    {
        if (locked || player == null) return;

        if (player.currentPowerUpType != lastType)
            RefreshFromPlayer();
    }

    public void RefreshFromPlayer()
    {
        if (player == null || maskUiSprite == null) return;

        lastType = player.currentPowerUpType;

        if (player.currentPowerUpType == null)
        {
            maskUiSprite.sprite = null;
            Color c0 = maskUiSprite.color;
            c0.a = 0f;
            maskUiSprite.color = c0;
            maskUiSprite.enabled = false;
            maskText.text = "Use mask";
            maskCanvasGroup.alpha = 0.25f;

            return;
        }

        if (database == null)
        {
            maskUiSprite.sprite = null;
            maskUiSprite.enabled = false;
            maskText.text = "Use mask";
            maskCanvasGroup.alpha = 0.25f;
            return;
        }

        Color c = maskUiSprite.color;
        c.a = 1f;
        maskUiSprite.color = c;
        var def = database.Get(player.currentPowerUpType.Value);
        maskUiSprite.sprite = def != null ? def.sprite : null;
        maskUiSprite.enabled = (maskUiSprite.sprite != null);
        maskText.text = def.powerName + " mask";
        maskCanvasGroup.alpha = 1f;

    }

    public void PlayUseShake()
{
    if (maskUiSprite == null || player == null || database == null) return;
    if (player.currentPowerUpType == null) return;

    var def = database.Get(player.currentPowerUpType.Value);
    if (def == null) return;

    shakeDuration = def.duration;

    StopAllCoroutines();
    StartCoroutine(UseShakeRoutine());
}

    private IEnumerator UseShakeRoutine()
    {
        locked = true;

        if (!maskUiSprite.enabled || maskUiSprite.sprite == null)
        {
            locked = false;
            yield break;
        }

        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            Vector2 r = Random.insideUnitCircle * shakeStrength;
            maskUiSprite.transform.localPosition =
                originalLocalPos + new Vector3(r.x, r.y, 0f);
            yield return null;
        }

        maskUiSprite.transform.localPosition = originalLocalPos;

        locked = false;
        RefreshFromPlayer();
    }
}
