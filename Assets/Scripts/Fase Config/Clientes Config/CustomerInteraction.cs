using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla a intera��o com o cliente (fala e pedido).
/// </summary>
public class CustomerInteraction : MonoBehaviour
{
    [Header("Refer�ncias da UI do cliente")]
    public GameObject speechBubble;  // bal�o de fala
    public Text speechText;          // texto dentro do bal�o
    public Image requestedItemIcon;  // �cone do item desejado
    public Sprite itemDesejado;      // qual item ele quer

    private bool falou = false;

    private void Start()
    {
        speechBubble.SetActive(false);
        requestedItemIcon.enabled = false;
    }

    private void OnMouseDown()
    {
        MostrarFala();
    }

    private void MostrarFala()
    {
        if (!falou)
        {
            speechBubble.SetActive(true);
            speechText.text = "Ol�! Tudo bem? Eu gostaria de um item especial!";
            falou = true;

            // Ap�s 1.5s mostra o item desejado
            Invoke(nameof(MostrarPedido), 1.5f);
        }
    }

    private void MostrarPedido()
    {
        requestedItemIcon.enabled = true;
        requestedItemIcon.sprite = itemDesejado;
        speechText.text = "Voc� tem esse item?";
    }
}
