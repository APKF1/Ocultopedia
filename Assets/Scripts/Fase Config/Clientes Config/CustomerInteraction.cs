using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Sistema de diálogo modular para clientes.
/// Mostra falas e inicia a fase ao clicar em "Ok".
/// </summary>
public class CustomerInteraction : MonoBehaviour, IPointerClickHandler
{
    [Header("Referências de UI")]
    public GameObject speechBubble;      // Balão de fala
    public TMP_Text speechText;          // Texto exibido
    public Button okButton;              // Botão "Ok!"

    [Header("Diálogo do cliente")]
    [TextArea]
    public string[] falas;               // Falas do cliente
    public Sprite[] itensPedidos;        // Ícones opcionais (não usados no momento)

    [Header("Referências externas")]
    public Timer timer;                  // Controle do tempo
    public SpawnObjects spawn;           // Sistema de spawn
    public int specificObjects;          // Número da fase (mantido, mas não usado aqui)
    public List<int> items = new List<int>(); // IDs dos itens a spawnar

    private int etapaConversa = 0;

    private void Start()
    {
        // Desativa elementos no início
        if (speechBubble != null) speechBubble.SetActive(false);
        if (okButton != null) okButton.gameObject.SetActive(false);
        if (speechText != null) speechText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Reservado para uso futuro com UI
    }

    private void OnMouseDown()
    {
        AvancarConversa();
    }

    /// <summary>
    /// Avança a fala do cliente e mostra o botão "Ok" ao final.
    /// </summary>
    private void AvancarConversa()
    {
        if (etapaConversa < falas.Length)
        {
            // Exibe a fala atual
            if (speechBubble != null) speechBubble.SetActive(true);
            if (speechText != null)
            {
                speechText.gameObject.SetActive(true);
                speechText.text = falas[etapaConversa];
            }

            etapaConversa++;
        }
        else
        {
            // Fim da conversa → exibe o botão "Ok!"
            if (okButton != null)
            {
                okButton.gameObject.SetActive(true);
                okButton.onClick.RemoveAllListeners();
                okButton.onClick.AddListener(OnOkClicked);
            }
        }
    }

    /// <summary>
    /// Ação ao clicar em "Ok": inicia a fase e o timer.
    /// </summary>
    public void OnOkClicked()
    {
        if (okButton != null) okButton.gameObject.SetActive(false);
        if (speechBubble != null) speechBubble.SetActive(false);
        if (speechText != null) speechText.gameObject.SetActive(false);

        // ✅ Corrigido: agora chama o método correto com apenas 1 argumento
        if (spawn != null)
        {
            spawn.SpawnarFase(items);
        }

        // Reinicia e inicia o timer
        if (timer != null)
        {
            timer.ResetarTimer();
            timer.IniciarTimer();
        }

        Debug.Log("🎯 Fase iniciada! Itens spawnados: " + items.Count);
    }
}
