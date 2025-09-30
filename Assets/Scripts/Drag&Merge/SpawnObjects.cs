using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // Lista de objetos que podem ser instanciados
    public List<GameObject> components = new List<GameObject>();

    // Start � chamado uma vez ao iniciar o script
    void Start()
    {
        SpawnSpecificObjects(new int[] {0, 1});
        SpawnObjetos(20); // Spawna 10 objetos ao iniciar
    }

    // Update � chamado a cada frame, mas n�o est� sendo usado agora
    void Update()
    {
        
    }

    // Fun��o para spawnar "n" objetos aleatoriamente posicionados
    void SpawnObjetos(int n)
    {
        for (int i = 0; i < n; i++)
        {
            float x = Random.Range(-9.95f, 9.95f);
            float y = Random.Range(1f, 4.22f);
            float rot = Random.Range(0f, 0f); // Isso sempre ser� 0 (depende da fase)

            int index = Random.Range(0, components.Count);

            // Instancia o objeto escolhido na posi��o e rota��o dadas
            Instantiate(
                components[index],
                new Vector3(x, y, 0),
                Quaternion.Euler(0f, 0f, rot)
            );
        }
    }
    void SpawnSpecificObjects(int[] items)
    {
        foreach (int i in items)
        {
            float x = Random.Range(-9f, 9f);
            float y = Random.Range(-0.7f, 4.22f);
            float rot = Random.Range(0f, 0f); // Isso sempre ser� 0 (depende da fase)

            int index = Random.Range(0, components.Count);

            // Instancia o objeto escolhido na posi��o e rota��o dadas
            Instantiate(
                components[i],
                new Vector3(x, y, 0),
                Quaternion.Euler(0f, 0f, rot)
            );
        }
    }
}


