using UnityEngine;
using UnityEngine.UI;

public class ConfiguracoesUI : MonoBehaviour
{
    [Header("Referências UI")]
    public Slider sliderVolume;
    public Toggle toggleTelaCheia;

    private void Start()
    {
        // Inicializa a UI com os valores atuais do jogo
        CarregarConfiguracoes();
    }

    void CarregarConfiguracoes()
    {
        // Aqui você pode substituir pelas chamadas do seu banco de dados
        sliderVolume.value = volume;
        toggleTelaCheia.isOn = telaCheia == 1;
    }

    public void OnVolumeAlterado(float novoVolume)
    {
        volume = novoVolume;
        // Opcional: aplicar volume imediatamente
        AudioListener.volume = volume;
    }

    public void OnTelaCheiaAlterada(bool isFull)
    {
        telaCheia = isFull ? 1 : 0;
        Screen.fullScreen = isFull; // aplica imediatamente
    }
}
