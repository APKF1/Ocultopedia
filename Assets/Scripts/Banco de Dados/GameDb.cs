using UnityEngine;
using System.IO;
using SQLite4Unity3d;

public class GameDatabase : MonoBehaviour
{
    private SQLiteConnection db;

    void Awake()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "savegame.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Banco criado/carregado em: " + dbPath);

        db.CreateTable<Configuracoes>();
        db.CreateTable<Progresso>();

        // Se ainda não existir dados, cria com valores padrão
        if (db.Table<Configuracoes>().Count() == 0)
        {
            SalvarConfiguracoes(1f, true);
        }

        if (db.Table<Progresso>().Count() == 0)
        {
            SalvarProgresso(1, 1, "");
        }
    }

    // ---------------- CONFIGURAÇÕES ----------------
    public void SalvarConfiguracoes(float volumeMusica, bool telaCheia)
    {
        db.DeleteAll<Configuracoes>(); // garante só 1 linha
        db.Insert(new Configuracoes
        {
            VolumeMusica = volumeMusica,
            TelaCheia = telaCheia ? 1 : 0
        });
    }

    public Configuracoes CarregarConfiguracoes()
    {
        return db.Table<Configuracoes>().FirstOrDefault();
    }

    // ---------------- PROGRESSO ----------------
    public void SalvarProgresso(int maiorNivel, int nivelJogado,  string desbloqueaveis)
    {
        db.DeleteAll<Progresso>(); // garante só 1 linha
        if(maiorNivel > nivelJogado)
        {
            db.Insert(new Progresso
            {
                NivelAtual = maiorNivel,
                Desbloqueaveis = desbloqueaveis
            });
        }else{
            db.Insert(new Progresso
            {
                NivelAtual = nivelJogado,
                Desbloqueaveis = desbloqueaveis
            });
        }

    }

    public Progresso CarregarProgresso()
    {
        return db.Table<Progresso>().FirstOrDefault();
    }

    void OnDestroy()
    {
        db?.Close();
    }
}

// ---------------- MODELOS ----------------
public class Configuracoes
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public float VolumeMusica { get; set; }
    public int TelaCheia { get; set; } // 0 ou 1
}

public class Progresso
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int NivelAtual { get; set; }
    public string Desbloqueaveis { get; set; } 
}
