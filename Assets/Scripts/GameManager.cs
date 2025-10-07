using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameDatabase db;
    void Start()
    {
        db = GetComponent<GameDatabase>();

        // Teste: salvar dados
        db.SalvarConfiguracoes(0.7f, 0.9f, "1280x720", false);
        db.SalvarProgresso(2, "Adaga", "", "");

        // Teste: carregar dados
        var cfg = db.CarregarConfiguracoes();
        Debug.Log($"Config -> Música: {cfg.VolumeMusica}, Efeitos: {cfg.VolumeEfeitos}, Res: {cfg.Resolucao}, Tela Cheia: {cfg.TelaCheia}");

        var prog = db.CarregarProgresso();
        //Debug.Log($"Progresso -> Nível: {prog.NivelAtual}, Pontos: {prog.Pontos}, Itens: {prog.Itens}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
