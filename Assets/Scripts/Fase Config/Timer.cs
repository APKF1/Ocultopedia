using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float tempoTotal = 60f;
    public float tempoRestante { get; private set; }
    public float tempoCritico = 10f;
    private bool ativo = false;

    [Header("UI")]
    public GameObject timerTextObject; // AGORA É GAMEOBJECT

    [Header("Animator (sincronizado)")]
    public Animator animTimer;
    public string nomeDaAnimacao = "TimerAnimation";

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip somTempoCritico;
    public AudioClip somTempoAcabou;

    [Header("Eventos & GameOver")]
    public UnityEvent OnTempoAcabou;
    public GameObject TelaGameOver;

    private bool somCriticoTocado = false;
    private bool somFinalTocado = false;

    private void Update()
    {
        if (!ativo) return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            ativo = false;

            if (!somFinalTocado)
            {
                somFinalTocado = true;
                if (audioSource && somTempoAcabou)
                    audioSource.PlayOneShot(somTempoAcabou);
            }

            OnTempoAcabou?.Invoke();

            if (TelaGameOver)
                TelaGameOver.SetActive(true);

            Time.timeScale = 0f;
        }

        AtualizarAnimacao();
        VerificarSomCritico();
    }

    // ---------------------
    // CONTROLES DO TIMER
    // ---------------------
    public void IniciarTimer()
    {
        tempoRestante = tempoTotal;
        ativo = true;

        somCriticoTocado = false;
        somFinalTocado = false;

        if (animTimer)
            animTimer.speed = 0;

        AtualizarAnimacao();
    }

    public void ResetarTimer()
    {
        tempoRestante = tempoTotal;

        somCriticoTocado = false;
        somFinalTocado = false;

        if (animTimer)
            animTimer.speed = 0;

        AtualizarAnimacao();
    }

    public void PararTimer()
    {
        ativo = false;
    }

    // ---------------------
    // ANIMAÇÃO SINCRONIZADA
    // ---------------------
    private void AtualizarAnimacao()
    {
        if (!animTimer) return;

        float progresso = 1f - (tempoRestante / tempoTotal);
        progresso = Mathf.Clamp01(progresso);

        animTimer.Play(nomeDaAnimacao, 0, progresso);
        animTimer.speed = 0;
    }

    // ---------------------
    // SOM CRÍTICO
    // ---------------------
    private void VerificarSomCritico()
    {
        if (somCriticoTocado || tempoRestante <= 0) return;

        if (tempoRestante <= tempoCritico)
        {
            somCriticoTocado = true;

            if (audioSource && somTempoCritico)
                audioSource.PlayOneShot(somTempoCritico);
        }
    }
}
