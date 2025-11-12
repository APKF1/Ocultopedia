using UnityEngine;

public class CaixaBtnScript : MonoBehaviour
{
    public CustomerInteraction dialogo;

    GameObject[] gos;
    void Update()
    {
        gos = GameObject.FindGameObjectsWithTag("Artefato");
    }

    private void OnMouseDown()
    {
        dialogo.AvancarConversa2();

        foreach (GameObject go in gos)
            Destroy(go);

    }
}
