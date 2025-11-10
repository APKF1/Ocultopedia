using UnityEngine;
using UnityEngine.SceneManagement;


public class GameButtonManager : MonoBehaviour
{
    public void TentarDeNovo()
    {
        SceneManager.LoadScene("Game");
    }

    public void RetornarAoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
