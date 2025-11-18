using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Configuração do Tempo")]
    public float tempoTotal = 60f;
    public float tempoRestante;
    public bool ativo = false;

    [Header("Relógio Visual (Animação Sincronizada)")]
    public Animator relogioAnimator;   // Animator do timer
    private int totalFrames;           // Frames totais da animação

    [Header("UI")]
    public GameObject timerUI;         // Objeto do timer para ativar/desativar
    public GameObject TelaGameOver;    // Tela de Game Over

    [Header("Eventos")]
    public UnityEvent OnTempoAcabou;   // Evento chamado quando o tempo termina

    private void Start()
    {
        if (timerUI != null)
            timerUI.SetActive(false);

        if (TelaGameOver != null)
            TelaGameOver.SetActive(false);

        // Pegar a quantidade real de frames da animação
        if (relogioAnimator != null)
        {
            AnimationClip clip = relogioAnimator.runtimeAnimatorController.animationClips[0];
            totalFrames = Mathf.RoundToInt(clip.length * clip.frameRate);

            // Congelar a animação para ser controlada pelo script
            relogioAnimator.speed = 0f;
        }
    }

    private void Update()
    {
        if (!ativo) return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            ativo = false;

            Debug.Log("⏰ Tempo esgotado!");

            if (TelaGameOver != null)
                TelaGameOver.SetActive(true);

            OnTempoAcabou?.Invoke();

            // Parar tudo
            Time.timeScale = 0f;

            return;
        }

        AtualizarAnimacao();
    }

    public void IniciarTimer()
    {
        tempoRestante = tempoTotal;
        ativo = true;

        if (timerUI != null)
            timerUI.SetActive(true);

        AtualizarAnimacao();
    }

    public void PararTimer()
    {
        ativo = false;
    }

    public void ResetarTimer()
    {
        tempoRestante = tempoTotal;
        AtualizarAnimacao();
    }

    private void AtualizarAnimacao()
    {
        if (relogioAnimator == null) return;

        // 0 → 1 (proporção do tempo passado)
        float progresso = 1f - (tempoRestante / tempoTotal);

        // ir para o ponto exato da animação
        relogioAnimator.Play(0, 0, progresso);
    }

    public void MostrarTimer(bool mostrar)
    {
        if (timerUI != null)
            timerUI.SetActive(mostrar);
    }
}
