using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Sistema de diálogo modular para clientes.
/// Permite qualquer número de falas, mostrando o botão "OK!" apenas no final.
/// </summary>
public class CustomerInteraction : MonoBehaviour
{
    [Header("Referências de UI")]
    public GameObject speechBubble;      // Balão de fala
    public TMP_Text speechText;          // Texto do balão
    // public SpriteRenderer requestedItemIcon;      // Ícone do item desejado
    public Button okButton;              // Botão "Ok!"

    [Header("Diálogo do cliente")]
    [TextArea]
    public string[] falas1;               // Array de falas do cliente
    public string[] falas2;
    public Sprite[] itensPedidos;        // Ícones opcionais para cada fala (-1 se nenhum)

    [Header("Referências externas")]
    public Timer timer;                         //Script do Timer
    public SpawnObjects spawn;                  //Script do Spawn
    public FadeController fade;                 //Script do Fade
    public CustomerManager customerManager;     //Script do customerManager
    public int specificObjects;                 //variavel dos ingredientes
    public GameObject panel;                    //variavel do painel que tem o fade                          

    public List<int> items = new List<int>();

    private int etapaConversa = 0;

    private int etapaConversa2 = 0;

    public int estadoDaFase;
    /*
    0 = introducao
    1 = gameplay
    2 = final da fase
    */

    private void Start()
    {
        speechBubble.SetActive(false);
        okButton.gameObject.SetActive(false);
        speechText.gameObject.SetActive(false);
        fade = panel.GetComponent<FadeController>();
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        AvancarConversa();
    }
    */

    /*private void OnMouseDown()
    {
        AvancarConversa();
    }
    */

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            verificarConversa();
        }
    }
    private void verificarConversa()
    {
        if (estadoDaFase == 0 & fade.fadeAconteceu == true)
        {
            AvancarConversa();
        }
        else if (estadoDaFase == 2)
        {
            AvancarConversa2();
        }

    }

    private void AvancarConversa()
    {
        if (etapaConversa < falas1.Length)
        {
            // Mostra o balão e o texto atual
            speechBubble.SetActive(true);
            speechText.gameObject.SetActive(true);
            speechText.text = falas1[etapaConversa];

            // Mostra ícone do item, se houver
            /* if (itensPedidos != null && itensPedidos.Length > etapaConversa && itensPedidos[etapaConversa] != null)
            {
                requestedItemIcon.enabled = true;
                requestedItemIcon.sprite = itensPedidos[etapaConversa];
            }
            /*else
            {
                requestedItemIcon.enabled = false;
            } */

            etapaConversa++;
        }
        else
        {
            // Conversa terminou -> mostra botão OK
            okButton.gameObject.SetActive(true);
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked1);
            estadoDaFase = 1;
        }
    }

    public void OnOkClicked1()
    {
        okButton.gameObject.SetActive(false);
        speechBubble.SetActive(false);
        speechText.gameObject.SetActive(false);
        // requestedItemIcon.enabled = false;

        spawn.SpawnarFase(specificObjects, items);

        if (timer != null)
        {
            timer.ResetarTimer();
            timer.IniciarTimer();
        }

        Debug.Log("🎯 Timer iniciado, fase começou!");
    }

    public void AvancarConversa2()
    {
        estadoDaFase = 2;
        if (etapaConversa2 < falas2.Length)
        {
            // Mostra o balão e o texto atual
            speechBubble.SetActive(true);
            speechText.gameObject.SetActive(true);
            speechText.text = falas2[etapaConversa2];



            // Mostra ícone do item, se houver
            /* if (itensPedidos != null && itensPedidos.Length > etapaConversa && itensPedidos[etapaConversa] != null)
            {
                requestedItemIcon.enabled = true;
                requestedItemIcon.sprite = itensPedidos[etapaConversa];
            }
            /*else
            {
                requestedItemIcon.enabled = false;
            } */

            etapaConversa2++;
        }
        else
        {
            // Conversa terminou -> mostra botão OK
            okButton.gameObject.SetActive(true);
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked2);
        }
    }
    public void OnOkClicked2()
    {
        okButton.gameObject.SetActive(false);
        speechBubble.SetActive(false);
        speechText.gameObject.SetActive(false);
        fade.StartFadeSequence();
    }

    public void DestruirCliente()
    {
        if (estadoDaFase == 2)
        {
            Destroy(gameObject);
            Debug.Log("Cliente Destruido");
        }
    }

}
