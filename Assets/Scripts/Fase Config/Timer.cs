using UnityEngine;
using UnityEngine.UI; // Necessário se for mostrar o tempo em um Text (UI antiga)
using TMPro;          // Necessário se for usar TextMeshPro

public class Timer : MonoBehaviour
{
    [Header("Configurações")]
    public float tempoInicial = 60f;   // Valor inicial (60 segundos, por exemplo)
    public bool contarParaCima = false; // Define se o timer conta pra cima ou para baixo
    public bool iniciarAutomatico = true;

    [Header("Referências de UI (opcional)")]
    public TextMeshProUGUI textoTMP; // Arraste aqui um TextMeshProUGUI
    public Text textoUI;             // Ou um Text normal da UI antiga

    private float tempoAtual;
    private bool ativo = false;

    void Start()
    {
        tempoAtual = tempoInicial;

        if (iniciarAutomatico)
            ativo = true;
    }

    void Update()
    {
        if (!ativo) return;

        // Atualiza o tempo
        if (contarParaCima)
            tempoAtual += Time.deltaTime;
        else
            tempoAtual -= Time.deltaTime;

        // Se for regressivo, trava no zero
        if (!contarParaCima && tempoAtual <= 0)
        {
            tempoAtual = 0;
            ativo = false;
            Debug.Log("Tempo acabou!");
        }

        // Atualiza a UI, se existir
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        // Converte para minutos:segundos
        int minutos = Mathf.FloorToInt(tempoAtual / 60);
        int segundos = Mathf.FloorToInt(tempoAtual % 60);

        string tempoFormatado = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (textoTMP != null)
            textoTMP.text = tempoFormatado;

        if (textoUI != null)
            textoUI.text = tempoFormatado;
    }

    // Métodos extras para controlar o timer
    public void IniciarTimer() => ativo = true;
    public void PausarTimer() => ativo = false;
    public void ResetarTimer() => tempoAtual = tempoInicial;
}
