using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer video;

    void Start()
    {
        // Quando o vídeo termina, chama a função que troca a cena
        video.loopPointReached += EndReached;
    }

    void Update()
    {
        // Se clicar ou apertar qualquer tecla, pula para o menu
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            IrParaMenu();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        IrParaMenu();
    }

    void IrParaMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
