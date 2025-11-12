using UnityEngine;

public class ButtonCollectables : MonoBehaviour
{
    public bool estaDesbloqueado = false;
    public GameObject[] popups;

    public void FuiClicado()
    {
        if(estaDesbloqueado)
        {
            popups[0].SetActive(true);
        }
        else
        {
            popups[1].SetActive(true);
        }
    }

    public void Bloqueia()
    {
        estaDesbloqueado = false;
    }

    public void Desbloqueia()
    {
        estaDesbloqueado = true;
    }
}