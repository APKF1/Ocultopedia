using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla diálogo, estados da fase e spawn dos itens.
/// Mostra nome do cliente em um TMP separado e a fala no balão.
/// </summary>
public class CustomerInteraction : MonoBehaviour
{
    [Header("UI – Balão de Fala")]
    public GameObject speechBubble;
    public TMP_Text speechText;
    public Button okButton;

    [Header("UI – Nome do Cliente (Prefab Separado)")]
    public TMP_Text customerNameText;
    public string nomeDoCliente = "Cliente";

    [Header("Fal falas do cliente")]
    [TextArea] public string[] falas1; // Introdução
    [TextArea] public string[] falas2; // Final

    public Sprite[] itensPedidos;

    [Header("Referências Externas")]
    public Timer timer;
    public SpawnObjects spawn;
    public FadeController fade;
    public CustomerManager customerManager;

    public GameObject panel;

    [Header("Objetos do Spawn")]
    public int specificObjects;
    public List<int> items = new List<int>();

    /*
        0 = conversando (introdução)
        1 = gameplay acontecendo
        2 = conversando final
    */
    public int estadoDaFase = 0;

    private int etapaConversa = 0;
    private int etapaConversa2 = 0;

    private void Start()
    {
        speechBubble.SetActive(false);
        okButton.gameObject.SetActive(false);
        speechText.gameObject.SetActive(false);

        if (customerNameText)
            customerNameText.gameObject.SetActive(false);

        fade = panel.GetComponent<FadeController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            VerificarConversa();
    }

    private void VerificarConversa()
    {
        if (estadoDaFase == 0 && fade.fadeAconteceu)
        {
            AvancarConversa();
        }
        else if (estadoDaFase == 2)
        {
            AvancarConversa2();
        }
    }

    // -----------------------------
    // INTRODUÇÃO (falas1)
    // -----------------------------
    private void AvancarConversa()
    {
        if (etapaConversa < falas1.Length)
        {
            MostrarNomeEFala(falas1[etapaConversa]);
            etapaConversa++;
        }
        else
        {
            okButton.gameObject.SetActive(true);
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked1);

            estadoDaFase = 1;
        }
    }

    // -----------------------------
    // FINAL (falas2)
    // -----------------------------
    public void AvancarConversa2()
    {
        if (etapaConversa2 < falas2.Length)
        {
            MostrarNomeEFala(falas2[etapaConversa2]);
            etapaConversa2++;
        }
        else
        {
            okButton.gameObject.SetActive(true);
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked2);
        }
    }

    // -----------------------------
    // Mostra nome + fala
    // -----------------------------
    private void MostrarNomeEFala(string textoFala)
    {
        speechBubble.SetActive(true);
        speechText.gameObject.SetActive(true);

        if (customerNameText)
        {
            customerNameText.gameObject.SetActive(true);
            customerNameText.text = nomeDoCliente;
        }

        speechText.text = textoFala;
    }

    // -----------------------------
    // OK da introdução
    // -----------------------------
    private void OnOkClicked1()
    {
        FecharCaixaDeTexto();

        spawn.SpawnarFase(specificObjects, items);

        if (timer != null)
        {
            timer.ResetarTimer();
            timer.IniciarTimer();
        }
    }

    // -----------------------------
    // OK do final da fase
    // -----------------------------
    private void OnOkClicked2()
    {
        FecharCaixaDeTexto();
        fade.StartFadeSequence();
    }

    // -----------------------------
    // Esconde o balão e o nome
    // -----------------------------
    private void FecharCaixaDeTexto()
    {
        okButton.gameObject.SetActive(false);
        speechBubble.SetActive(false);
        speechText.gameObject.SetActive(false);

        if (customerNameText)
            customerNameText.gameObject.SetActive(false);
    }

    // -----------------------------
    // Chamado por outro script
    // -----------------------------
    public void DestruirCliente()
    {
        if (estadoDaFase == 2)
            Destroy(gameObject);
    }
}
