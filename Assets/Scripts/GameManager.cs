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

    void Start()
    {
        db = GetComponent<GameDatabase>();
        // Teste: salvar dados
        db.SalvarConfiguracoes(0.7f, false);
        //db.SalvarProgresso(2, 3, 1);

        // Teste: carregar dados
        var cfg = db.CarregarConfiguracoes();
        volume = cfg.VolumeMusica;
        telaCheia = cfg.TelaCheia;

        //Debug.Log($"Config -> Música: {cfg.VolumeMusica}, Efeitos: {cfg.VolumeEfeitos}, Res: {cfg.Resolucao}, Tela Cheia: {cfg.TelaCheia}");

        var prog = db.CarregarProgresso();
        nivelAtual = prog.Fase;

        //Testando Parse
        //Debug.Log($"Progresso -> Nível: {prog.NivelAtual}, Pontos: {prog.Pontos}, Itens: {prog.Itens}");
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
