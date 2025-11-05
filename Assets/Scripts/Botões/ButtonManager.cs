using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	public GameObject seletorDeFases;
	public GameObject settingsMenu;
	public GameObject colecao;

	public void BtnPlay()
	{
		SceneManager.LoadScene("Game");
	}

	public void BtnQuit()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
	public void BtnSeletorDeFases()
	{
		seletorDeFases.SetActive(true);
	}
	public void BtnBackToMenu()
	{
		colecao.SetActive(false);
		seletorDeFases.SetActive(false);
	}
	public void BtnSettings()
	{
		settingsMenu.SetActive(true);
	}
	public void BtnCollection()
	{
		colecao.SetActive(true);
	}

	public void BtnFase1()
	{
        SceneManager.LoadScene("Game");
    }
}
