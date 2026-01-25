using UnityEngine;
using System.IO;
using SQLite4Unity3d;
using System.Linq;

public class GameDatabase : MonoBehaviour
{
    private SQLiteConnection db;

    void Awake()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "savegame.db");
        if(!File.Exists(dbPath))
        {
            string origemDb = Application.dataPath + "/Banco/savegame.db";
            string destinoDb = dbPath;
            File.Copy(origemDb, destinoDb);
        }

        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Banco criado/carregado em: " + dbPath);

        //db.CreateTable<Configuracoes>();
        //db.CreateTable<Progresso>();
        //db.CreateTable<Colecao>();

        // Se ainda não existir dados, cria com valores padrão
        //if (db.Table<Configuracoes>().Count() == 0)
        //{
        //    SalvarConfiguracoes(1f, true);
        //}

        //if (db.Table<Progresso>().Count() == 0)
        //{
        //    db.Insert(new Progresso { NivelProgresso = 1, NivelAtual =  1 });
        //}
    }

    // ---------------- CONFIGURAÇÕES ----------------
    public void SalvarConfiguracoes(float volumeMusica, bool telaCheia) // mudar para update
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
    public void SalvarProgresso(int maiorNivel, int nivelEscolhido,  int id)
    {
        db.Execute("UPDATE Progresso SET NivelProgresso = ?, NivelAtual = ? WHERE Id = ?", maiorNivel, nivelEscolhido, id);

    }

    public Progresso CarregarProgresso()
    {
        return db.Table<Progresso>().FirstOrDefault();
    }

    /*public Colecao CarregarColecao(string desbloq, int id)
    {
        if (desbloq == null) return db.Table<Colecao>().FirstOrDefault();
        else return db.Table<Colecao>().Where(c => c.Coletado == desbloq && c.Id == id).FirstOrDefault();
    }*/

    public void SalvarColecao(int id, bool coletado, string colecionavel)
    {
        db.Execute("UPDATE Colecao SET Coletado = ? WHERE Desbloqueavel = '?' AND Id = ?", coletado ? 1: 0, colecionavel, id);
    }

    /*public void InserirColecao(string desb)
    {
        db.Insert(new Colecao
        {
            Coletado = false ? 1:0,
            Desbloqueavel = desb
        });
    }*/

    
    void OnDestroy() //Passar para obj dontDestroy
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

public class Fases
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Cliente { get; set; } 
    public int Colecionavel { get; set; } 
    public int Artefato { get; set; } 
}

public class Clientes
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
}

public class Colecao
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public int Coletado { get; set; } // 0 ou 1
}

public class Progresso
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int Fase { get; set; }
}

public class Receitas
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Item { get; set; }
    public string Comp { get; set; }
    public string Comp_2 { get; set; }
    public string Comp_3 { get; set; }
}


