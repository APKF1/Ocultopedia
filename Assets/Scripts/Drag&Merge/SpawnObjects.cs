using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de spawn otimizado e controlado — 
/// Gera os componentes/ingredientes da fase após o início do jogo.
/// </summary>
public class SpawnObjects : MonoBehaviour
{
    [Header("Objetos disponíveis para spawn")]
    [Tooltip("Lista de prefabs que podem ser instanciados na cena.")]
    public List<GameObject> components = new List<GameObject>();

    [Header("Pontos de spawn")]
    [Tooltip("Pontos fixos onde os objetos podem aparecer.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Configurações")]
    [Tooltip("Se verdadeiro, o spawn será aleatório (ordem e posição).")]
    public bool spawnAleatorio = true;

    [Tooltip("Som opcional reproduzido ao spawnar os objetos.")]
    public AudioClip spawnSound;

    private AudioSource audioSource;
    int counter = 0;

    private void Awake()
    {
        // Configura o áudio, se existir
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Método chamado no início da fase (após o jogador clicar em "Ok!").
    /// </summary>
    public void SpawnarFase(int n, List<int> items)
    {
        if (components.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnObjects: Nenhum prefab ou ponto de spawn definido!");
            return;
        }

        SpawnSpecificObjects(n, items);
        SpawnRandomObjects(spawnPoints.Count - n);

        // Reproduz som (se houver)
        if (spawnSound != null && audioSource != null)
            audioSource.PlayOneShot(spawnSound);

        Debug.Log("🧩 Objetos da fase spawnados com sucesso!");
    }

    /// <summary>
    /// Spawna todos os objetos da lista em pontos fixos (um por ponto).
    /// </summary>
    private void SpawnSpecificObjects(int n, List<int> items)
    {
        // int count = Mathf.Min(components.Count, spawnPoints.Count);

        
       // for (int i = 0; i < n; i++)
        foreach (int i in items)
        {
            GameObject prefab = components[i];
            Transform point = spawnPoints[counter];
            counter++;

            Instantiate(prefab, point.position, point.rotation);
        }
    }

    /// <summary>
    /// Spawna objetos em ordem aleatória, em pontos de spawn diferentes.
    /// </summary>
    private void SpawnRandomObjects(int n)
    {
        // int spawnCount = Mathf.Min(n, spawnPoints.Count);

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < n; i++)
        {
            //int pointIndex = Random.Range(0, availablePoints.Count);
            // Transform point = availablePoints[pointIndex];
            // availablePoints.RemoveAt(pointIndex);
            Transform point = spawnPoints[counter];
            counter++;

            int prefabIndex = Random.Range(0, components.Count);
            GameObject prefab = components[prefabIndex];

            Instantiate(prefab, point.position, point.rotation);
        }
    }

    /// <summary>
    /// Visualiza os pontos de spawn no Editor (em verde).
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.green;
        foreach (var p in spawnPoints)
        {
            if (p != null)
                Gizmos.DrawWireSphere(p.position, 0.3f);
        }
    }
}
