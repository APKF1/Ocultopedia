using UnityEngine;
using UnityEngine.UI;

public class ConfiguracoesUI : MonoBehaviour
{
    GameManager gameManager;
    [Header("Referências UI")]
    public Slider sliderVolume;
    public Toggle toggleTelaCheia;
    public float volume;
    public bool telaCheia;
    public GameObject configuracoes;

    private GameDatabase db;

    private void Start()
    {
        gameManager = GameObject.Find("-----Menu Principal-----").GetComponent<GameManager>();
        sliderVolume.value = gameManager.volume;
        if(gameManager.telaCheia == 1)
        {
            toggleTelaCheia.isOn = true;
        }
        else
        {
            toggleTelaCheia.isOn = false;
        }
    }

    public void OnVolumeAlterado(float novoVolume)
    {
        volume = novoVolume;
        // Opcional: aplicar volume imediatamente
        AudioListener.volume = volume;
    }

    public void OnTelaCheiaAlterada(bool isFull)
    {
        //telaCheia = isFull ? 1 : 0;
        Screen.fullScreen = isFull; // aplica imediatamente
    }

    public void SairConfiguracoes()
    {
        db = GetComponent<GameDatabase>();
        Debug.Log("Aqui foi");
        db.SalvarConfiguracoes(sliderVolume.value, toggleTelaCheia.isOn);
        configuracoes.SetActive(false);
    }
}