using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    [Header("Caixa Registradora")]
    public Animator animator;
    public string triggerAbrir = "Abrir";
    public string triggerFechar = "Fechar";

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip somAbrir;
    public AudioClip somFechar;

    [Header("Botão (Opcional)")]
    public GameObject CashierBtn;

    private int objetosDentro = 0;
    private bool caixaAberta = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Artefato"))
            return;

        objetosDentro++;

        // Só abre se estiver fechada
        if (!caixaAberta)
        {
            caixaAberta = true;

            if (animator)
            {
                animator.ResetTrigger(triggerFechar);
                animator.SetTrigger(triggerAbrir);
            }

            if (audioSource && somAbrir)
                audioSource.PlayOneShot(somAbrir);

            if (CashierBtn)
                CashierBtn.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Artefato"))
            return;

        objetosDentro--;

        // Só fecha se saiu o último objeto
        if (objetosDentro <= 0 && caixaAberta)
        {
            caixaAberta = false;

            if (animator)
            {
                animator.ResetTrigger(triggerAbrir);
                animator.SetTrigger(triggerFechar);
            }

            if (audioSource && somFechar)
                audioSource.PlayOneShot(somFechar);

            if (CashierBtn)
                CashierBtn.SetActive(false);
        }
    }
}
