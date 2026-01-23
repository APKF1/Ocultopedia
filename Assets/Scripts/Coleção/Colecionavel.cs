using System;
using UnityEngine;

public class Colecionavel : MonoBehaviour
{
    [Header("Banco de dados")]
    [SerializeField] GameDatabase db;

    [Header("Colecionavel")]
    [SerializeField] string[] arrNames;
    [SerializeField] Sprite[] arrImage;
    private string nameColec;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VerificarColecao();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Progresso prog = db.CarregarProgresso();
            Debug.Log("Coletei");
            db.SalvarColecao(prog.Id, true, nameColec);
            gameObject.gameObject.SetActive(false);
        }
    }

    public void VerificarColecao()
    {
        // Verifica qual fase está rodando
        Progresso prog = db.CarregarProgresso();
        nameColec = arrNames[prog.NivelAtual - 1];
        Debug.Log(nameColec);
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = arrImage[prog.NivelAtual - 1];

        // Olha o banco de dados, e instancia o colecionavel caso necessário.
        Colecao colec = db.CarregarColecao(nameColec, prog.Id);
        if (colec == null)
        {
            db.InserirColecao(nameColec);
            gameObject.transform.position = new Vector3(0,0,0);
            gameObject.gameObject.SetActive(true);
        }
        else if (colec.Coletado == 0)
        {
            gameObject.transform.position = new Vector3(0, 0, 0);
            gameObject.gameObject.SetActive(true);
        }
        else return;
    }
}
