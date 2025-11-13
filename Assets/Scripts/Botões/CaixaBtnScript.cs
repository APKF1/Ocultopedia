using UnityEngine;

public class CaixaBtnScript : MonoBehaviour
{
    public CustomerInteraction dialogo;
    public Timer timer;

    GameObject[] gos;
    void Update()
    {
        gos = GameObject.FindGameObjectsWithTag("Artefato");
    }

    public void OnMouseDown()
    {
        dialogo.AvancarConversa2();

        foreach (GameObject go in gos)
            Destroy(go);
        timer.PararTimer();
    }
}
