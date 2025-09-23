using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // Lista de objetos que podem ser instanciados
    public List<GameObject> components = new List<GameObject>();

    // Start é chamado uma vez ao iniciar o script
    void Start()
    {
        SpawnObjetos(10); // Spawna 10 objetos ao iniciar
    }

    // Update é chamado a cada frame, mas não está sendo usado agora
    void Update()
    {
        
    }

    // Função para spawnar "n" objetos aleatoriamente posicionados
    void SpawnObjetos(int n)
    {
        for (int i = 0; i < n; i++)
        {
            float x = Random.Range(-9f, 9f);
            float y = Random.Range(-0.7f, 4.22f);
            float rot = Random.Range(0f, 0f); // Isso sempre será 0 (depende da fase)

            int index = Random.Range(0, components.Count);

            // Instancia o objeto escolhido na posição e rotação dadas
            Instantiate(
                components[index],
                new Vector3(x, y, 0),
                Quaternion.Euler(0f, 0f, rot)
            );
        }
    }
}
