using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColectionManager : MonoBehaviour
{
    [SerializeField] private Transform parentEmpty; // arraste o Empty com os botões
    [SerializeField] private GameManager gameManager; // referência ao GameManager

    bool deveEstarVisivel;

    void Start()
    {
        AtualizarVisibilidade();
    }

    public void AtualizarVisibilidade()
    {
        // Obtém a lista de nomes de botões ativos do GameManager
        string[] botoesAtivos = gameManager.GetBotoesVisiveis(); 
        // Exemplo de retorno: new string[] { "BotaoA", "BotaoC" };

        foreach (Transform child in parentEmpty)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
            {
                Image img = btn.GetComponent<Image>();
                if (img != null)
                {
                    // Se o nome do botão estiver na lista, mostra a imagem
                    deveEstarVisivel = botoesAtivos.Contains(child.name);
                    img.enabled = deveEstarVisivel; // imagem visível ou invisível
                }

                ButtonCollectables bc = btn.GetComponent<ButtonCollectables>();
                if(bc != null && deveEstarVisivel)
                {
                    //Debug.Log("Desbloqueia " + btn.gameObject.name);
                    bc.Desbloqueia();
                }

                // O botão continua habilitado em qualquer caso
                btn.interactable = true;
            }
        }
    }
}