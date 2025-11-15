using UnityEngine;
using UnityEngine.UI;

public class SelecaoFases : MonoBehaviour
{
    GameManager gameManager;
    public int nivelDoJogador; // Recebe do outro script
    public Button[] botoesDeFase; // Arraste todos os botões no inspetor

    void Start()
    {  
        gameManager = FindFirstObjectByType<GameManager>();
        nivelDoJogador = gameManager.nivelAtual;
        Debug.Log(nivelDoJogador);
        AtualizarFases();
    }

    public void AtualizarFases()
    {
        for (int i = 0; i < botoesDeFase.Length; i++)
        {
            if (i < nivelDoJogador)
            {
                botoesDeFase[i].interactable = true; // Desbloqueado
            }
            else
            {
                botoesDeFase[i].interactable = false; // Bloqueado
            }
        }
    }
}