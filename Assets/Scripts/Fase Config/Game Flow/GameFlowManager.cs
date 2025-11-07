using UnityEngine;
using System.Collections;

/// <summary>
/// GameFlowManager robusto:
/// - Garante que há um AudioSource configurado (procura automaticamente se nulo).
/// - Toca knock sound depois do delay inicial.
/// - Mostra/oculta hint (seta + texto).
/// - Re-toca knock a cada intervalo até a porta ser aberta.
/// - Contém métodos públicos para debug/controle.
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    [Header("Áudio / Hint")]
    public AudioSource audioSource;      // arraste ou será buscado automaticamente
    public AudioClip knockSound;
    //public GameObject clickHint;         // seta + "Clique para abrir a porta"

    [Header("Timings")]
    public float initialDelay = 2.5f;
    public float knockRepeatDelay = 3f;

    bool doorOpened = false;
    Coroutine introCoroutine;

    void Awake()
    {
       /* if (audioSource == null)
        {
            // tenta encontrar um AudioSource na cena
           // audioSource = FindObjectOfType<AudioSource>();
            if (audioSource == null) Debug.LogWarning("GameFlowManager: AudioSource não encontrado - audio não tocará.");
        }

        //if (clickHint == null) Debug.LogWarning("GameFlowManager: clickHint não atribuído.");*/
    }

    void Start()
    {
        doorOpened = false;
        // if (clickHint != null) clickHint.SetActive(false);
        //introCoroutine = StartCoroutine(IntroSequence());
        ResetFlow();
    }

    IEnumerator IntroSequence()
    {
        //Debug.Log("INICIANDO A COROUTINE"); // OK rodando no respawn
        // delay inicial de silêncio
        yield return new WaitForSeconds(initialDelay);

        // primeira batida
        PlayKnock();

        // mostra hint
        //if (clickHint != null) clickHint.SetActive(true);

        // repete até abrir a porta
        while (!doorOpened)
        {
            yield return new WaitForSeconds(knockRepeatDelay);
            if (!doorOpened) PlayKnock();
        }
    }

    void PlayKnock()
    {
        if (audioSource != null && knockSound != null)
        {
            audioSource.PlayOneShot(knockSound);
            Debug.Log("GameFlowManager: Knock tocado");
        }
        else
        {
            Debug.LogWarning("GameFlowManager: knockSound ou audioSource faltando.");
        }
    }

    // Chamado pela porta quando clicada
    public void PortaAberta()
    {
        if (doorOpened) return;
        doorOpened = true;

        //if (clickHint != null) clickHint.SetActive(false);
        Debug.Log("GameFlowManager: PortaAberta() chamado.");
    }

    // método público para forçar reinício (útil para debug)
    public void ResetFlow()
    {
        Debug.Log("REINICIANDO");
        doorOpened = false;
        //if (clickHint != null) clickHint.SetActive(false);
        if (introCoroutine != null) StopCoroutine(introCoroutine);
        introCoroutine = StartCoroutine(IntroSequence());
    }
}
