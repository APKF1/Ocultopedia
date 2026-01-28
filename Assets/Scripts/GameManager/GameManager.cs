using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Exemplo de string que define quais botões estão visíveis
    public float volume;
    public int telaCheia;
    public int nivelAtual;
    private string botoesVisiveis;
    public GameDatabase db;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // destrói o NOVO
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        db = GetComponent<GameDatabase>();

        var prog = db.CarregarProgresso();
        nivelAtual = prog.Fase;

    }
    
    // Método público para recuperar isso como array
    public string[] GetBotoesVisiveis()
    {
        return botoesVisiveis
            .Split(',') // separa por vírgula
            .Select(b => b.Trim()) // remove espaços
            .ToArray();
    }
    
    void Update()
    {
        
    }
}
