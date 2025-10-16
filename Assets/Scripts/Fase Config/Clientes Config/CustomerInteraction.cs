using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Sistema de diálogo modular para clientes.
/// Permite qualquer número de falas, mostrando o botão "OK!" apenas no final.
/// </summary>
public class CustomerInteraction : MonoBehaviour, IPointerClickHandler
{
    [Header("Referências de UI")]
    public GameObject speechBubble;      // Balão de fala
    public TMP_Text speechText;          // Texto do balão
    public SpriteRenderer requestedItemIcon;      // Ícone do item desejado
    public Button okButton;              // Botão "Ok!"

    [Header("Diálogo do cliente")]
    [TextArea]
    public string[] falas;               // Array de falas do cliente
    public Sprite[] itensPedidos;        // Ícones opcionais para cada fala (-1 se nenhum)

    [Header("Referências externas")]
    public Timer timer;                  // Script do Timer

    private int etapaConversa = 0;

    private void Start()
    {
        speechBubble.SetActive(false);
        requestedItemIcon.enabled = false;
        okButton.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AvancarConversa();
    }

    private void AvancarConversa()
    {
        if (etapaConversa < falas.Length)
        {
            // Mostra o balão e o texto atual
            speechBubble.SetActive(true);
            speechText.text = falas[etapaConversa];

            // Mostra ícone do item, se houver
            if (itensPedidos != null && itensPedidos.Length > etapaConversa && itensPedidos[etapaConversa] != null)
            {
                requestedItemIcon.enabled = true;
                requestedItemIcon.sprite = itensPedidos[etapaConversa];
            }
            else
            {
                requestedItemIcon.enabled = false;
            }

            etapaConversa++;
        }
        else
        {
            // Conversa terminou -> mostra botão OK
            okButton.gameObject.SetActive(true);
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked);
        }
    }

    private void OnOkClicked()
    {
        okButton.gameObject.SetActive(false);
        speechBubble.SetActive(false);
        requestedItemIcon.enabled = false;

        if (timer != null)
        {
            timer.ResetarTimer();
            timer.IniciarTimer();
        }

        Debug.Log("🎯 Timer iniciado, fase começou!");
    }
}
