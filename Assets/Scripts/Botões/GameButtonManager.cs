using UnityEngine;
using UnityEngine.SceneManagement;


public class GameButtonManager : MonoBehaviour
{
    public void TentarDeNovo()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void RetornarAoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
