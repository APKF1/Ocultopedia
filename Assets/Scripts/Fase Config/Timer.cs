using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Controla o tempo da fase (contagem regressiva).
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool ativo = false;

    [Header("UI")]
    public TMP_Text textoTimer;           // Referência ao texto "Timer"

    [Header("Evento de Fim")]
    public UnityEvent OnTempoAcabou;

    private void Update()
    {
        if (!ativo) return;

        tempoRestante -= Time.deltaTime;
        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            ativo = false;
            OnTempoAcabou?.Invoke();
            Debug.Log("⏰ Tempo esgotado!");
        }

        AtualizarTexto();
    }

    public void IniciarTimer()
    {
        tempoRestante = tempoTotal;
        ativo = true;
        AtualizarTexto();
    }

    public void ResetarTimer()
    {
        tempoRestante = tempoTotal;
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tempoRestante % 60f);
        textoTimer.text = $"{minutos:00}:{segundos:00}";
    }
}
