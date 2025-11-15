using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    public GameObject CashierBtn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Artefato"))
        {
            CashierBtn.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Artefato"))
        {
            CashierBtn.SetActive(false);
        }
    }

}
