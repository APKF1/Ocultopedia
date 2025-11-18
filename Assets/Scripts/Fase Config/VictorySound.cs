using UnityEngine;

public class VictorySound : MonoBehaviour
{
    [Header("Som de Vitória")]
    public AudioSource audioSource;
    public AudioClip victoryClip;

    [Header("Referências")]
    public GameObject telaVitoria;   // A tela que o Fade ativa

    private bool jaTocou = false;

    private void Update()
    {
        // Quando a tela de vitória ativar pela primeira vez → toca o som
        if (telaVitoria != null && telaVitoria.activeSelf && !jaTocou)
        {
            jaTocou = true;

            if (audioSource != null && victoryClip != null)
                audioSource.PlayOneShot(victoryClip);
        }
    }
}
