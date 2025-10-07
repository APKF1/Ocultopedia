using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private GameDatabase db;
    void Start()
    {
        db = GetComponent<GameDatabase>();
        // Teste: salvar dados
        //db.SalvarConfiguracoes(0.7f, 0.9f, "1280x720", false);
        //db.SalvarProgresso(2, "Adaga", "Banana");

        // Teste: carregar dados
        var cfg = db.CarregarConfiguracoes();
        Debug.Log($"Config -> Música: {cfg.VolumeMusica}, Efeitos: {cfg.VolumeEfeitos}, Res: {cfg.Resolucao}, Tela Cheia: {cfg.TelaCheia}");

        var prog = db.CarregarProgresso();
        List<string> result = prog.IngredientesPerdidos.Split(new char[] { ',' }).ToList();
        foreach (var item in result)
        {
            Debug.Log("Oi");
            Debug.Log(item);
        }
        //Testando Parse
        //Debug.Log($"Progresso -> Nível: {prog.NivelAtual}, Pontos: {prog.Pontos}, Itens: {prog.Itens}");
    }
    void Update()
    {
        
    }
}
