using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Controla o diálogo do cliente e inicia o timer ao clicar em "Ok".
/// </summary>
public class CustomerInteraction : MonoBehaviour, IPointerClickHandler
{
    [Header("Referências de UI")]
    public GameObject speechBubble;      // Balão de fala
    public TMP_Text speechText;          // Texto do balão
    public Image requestedItemIcon;      // Ícone do item desejado
    public Sprite itemDesejado;          // Sprite do item
    public Button okButton;              // Botão "Ok!"

    [Header("Referências externas")]
    public Timer timer;                  // Script do Timer

    private int etapaConversa = 0;       // controla qual etapa da conversa

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
        switch (etapaConversa)
        {
            case 0:
                // Primeira fala
                speechBubble.SetActive(true);
                speechText.text = "Olá! Tudo bem? Eu gostaria de um artefato!";
                etapaConversa++;
                break;

            case 1:
                // Pedido do item
                requestedItemIcon.enabled = true;
                requestedItemIcon.sprite = itemDesejado;
                speechText.text = "Você pode criar esse item para mim?";
                etapaConversa++;
                break;

            case 2:
                // Fim da conversa -> mostra botão OK
                okButton.gameObject.SetActive(true);
                okButton.onClick.RemoveAllListeners();
                okButton.onClick.AddListener(OnOkClicked);
                etapaConversa++;
                break;
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
