using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Controla o efeito de fade e dispara eventos durante o processo.
/// </summary>
[RequireComponent(typeof(Image), typeof(AudioSource))]
public class FadeController : MonoBehaviour
{
    [Header("Configurações do Fade")]
    public float fadeDuration = 1.5f;
    public Color fadeColor = Color.black;

    [Header("Som durante o fade")]
    public AudioClip bellSound;

    [Header("Comportamento do Panel")]
    public bool disableAfterFade = true;

    [Header("Eventos do Fade")]
    public UnityEvent OnFadeFullDark; // 🎯 dispara quando a tela escurece completamente

    private Image fadeImage;
    private AudioSource audioSource;

    public bool fadeAconteceu = false;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        Color startColor = fadeColor;
        startColor.a = 0f;
        fadeImage.color = startColor;

        gameObject.SetActive(false);
    }

    public void StartFadeSequence()
    {
        gameObject.SetActive(true);
        fadeImage.raycastTarget = false;
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // 1️⃣ Escurecer
        yield return StartCoroutine(Fade(0f, 1f));

        // 👉 Momento exato da escuridão total
        OnFadeFullDark?.Invoke();

        // 2️⃣ Tocar som
        if (bellSound != null)
            audioSource.PlayOneShot(bellSound);

        // 3️⃣ Esperar antes de clarear
        yield return new WaitForSeconds(0.6f);

        // 4️⃣ Clarear
        yield return StartCoroutine(Fade(1f, 0f));

        if (disableAfterFade)
            gameObject.SetActive(false);
            fadeAconteceu = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = newColor;

            yield return null;
        }
    }
}
