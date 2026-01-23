using TMPro;
using UnityEngine;

public class Configs : MonoBehaviour
{
    public TMP_Text res;
    public GameDatabase db;
    float volMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var data = db.CarregarConfiguracoes();
        if (data.TelaCheia == 1)
        {
            Screen.fullScreen = true;
            res.text = "Sim";
        }
        else if (data.TelaCheia != 1)
        {
            Screen.fullScreen = false;
            res.text = "Não";
        }
        else Debug.Log("Erro tela cheia");

        volMusic = data.VolumeMusica;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResolutionNext()
    {
        if (!Screen.fullScreen)
        {
            Screen.fullScreen = true;
            res.text = "Sim";
            db.SalvarConfiguracoes(volMusic, true);
        }
    }

    public void ResolutionBack()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            res.text = "Não";
            db.SalvarConfiguracoes(volMusic, false);
        }

    }
}
