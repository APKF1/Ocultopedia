using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Exemplo de string que define quais bot�es est�o vis�veis
    private string botoesVisiveis;
    private GameDatabase db;

    void Start()
    {
        db = GetComponent<GameDatabase>();
        // Teste: salvar dados
        db.SalvarConfiguracoes(0.7f, 0.9f, "1280x720", false);
        db.SalvarProgresso(2, 3, "Chifre, Gorila, Pedras");

        // Teste: carregar dados
        var cfg = db.CarregarConfiguracoes();
        //Debug.Log($"Config -> M�sica: {cfg.VolumeMusica}, Efeitos: {cfg.VolumeEfeitos}, Res: {cfg.Resolucao}, Tela Cheia: {cfg.TelaCheia}");

        var prog = db.CarregarProgresso();
        botoesVisiveis = prog.Desbloqueaveis;
        //Testando Parse
        //Debug.Log($"Progresso -> N�vel: {prog.NivelAtual}, Pontos: {prog.Pontos}, Itens: {prog.Itens}");
    }
    
    // M�todo p�blico para recuperar isso como array
    public string[] GetBotoesVisiveis()
    {
        return botoesVisiveis
            .Split(',') // separa por v�rgula
            .Select(b => b.Trim()) // remove espa�os
            .ToArray();
    }
    
    void Update()
    {
        
    }
}
