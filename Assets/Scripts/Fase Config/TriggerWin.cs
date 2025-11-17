using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    [Header("Caixa Registradora")]
    public Animator animator;          // Animator da caixa
    public string abrirTrigger = "Abrir";
    public string fecharTrigger = "Fechar";

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip somAbrir;
    public AudioClip somFechar;

    [Header("Botão da Caixa (opcional)")]
    public GameObject CashierBtn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Artefato")) return;

        // Ativa o botão (se existir)
        if (CashierBtn)
            CashierBtn.SetActive(true);

        // Ativa animação de abrir
        if (animator && !string.IsNullOrEmpty(abrirTrigger))
            animator.SetTrigger(abrirTrigger);

        // Toca som de abrir
        if (audioSource && somAbrir)
            audioSource.PlayOneShot(somAbrir);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Artefato")) return;

        // Desativa o botão (se existir)
        if (CashierBtn)
            CashierBtn.SetActive(false);

        // Ativa animação de fechar
        if (animator && !string.IsNullOrEmpty(fecharTrigger))
            animator.SetTrigger(fecharTrigger);

        // Toca som de fechar (se quiser)
        if (audioSource && somFechar)
            audioSource.PlayOneShot(somFechar);
    }
}
