using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de spawn otimizado e flex�vel para objetos em posi��es fixas.
/// Permite configurar pontos de spawn no Inspector, spawnar objetos espec�ficos ou aleat�rios.
/// </summary>
public class SpawnObjects : MonoBehaviour
{
    [Header("Objetos dispon�veis para spawn")]
    [Tooltip("Lista de prefabs que podem ser instanciados na cena.")]
    public List<GameObject> components = new List<GameObject>();

    [Header("Pontos de spawn")]
    [Tooltip("Pontos fixos onde os objetos podem aparecer.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Configura��o de spawn inicial")]
    [Tooltip("N�mero de objetos aleat�rios para spawnar no in�cio.")]
    [Range(0, 50)]
    public int initialSpawnCount = 10;

    [Tooltip("Se verdadeiro, spawna objetos aleat�rios automaticamente no Start.")]
    public bool spawnOnStart = true;

    void Start()
    {
        // Spawna objetos espec�ficos (exemplo: �ndice 0 e 1)
        SpawnSpecificObjects(new int[] { 0, 1 });

        // Spawna um n�mero configurado de objetos aleat�rios
        if (spawnOnStart)
            SpawnRandomObjects(initialSpawnCount);
    }

    /// <summary>
    /// Spawna 'n' objetos aleat�rios em pontos de spawn diferentes.
    /// </summary>
    /// <param name="n">N�mero de objetos a spawnar.</param>
    public void SpawnRandomObjects(int n)
    {
        if (components.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnObjects: Nenhum prefab ou ponto de spawn definido!");
            return;
        }

        // Garantir que n�o spawne mais objetos do que pontos dispon�veis
        int spawnCount = Mathf.Min(n, spawnPoints.Count);

        // Lista tempor�ria para n�o repetir spawnPoints
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < spawnCount; i++)
        {
            // Escolhe um ponto aleat�rio e remove da lista
            int pointIndex = Random.Range(0, availablePoints.Count);
            Transform point = availablePoints[pointIndex];
            availablePoints.RemoveAt(pointIndex);

            // Escolhe um prefab aleat�rio
            int prefabIndex = Random.Range(0, components.Count);
            GameObject prefab = components[prefabIndex];

            // Instancia o objeto
            Instantiate(prefab, point.position, point.rotation);
        }
    }

    /// <summary>
    /// Spawna objetos espec�ficos (por �ndice) em pontos de spawn fixos.
    /// </summary>
    /// <param name="objectIndices">�ndices dos objetos na lista 'components'.</param>
    public void SpawnSpecificObjects(int[] objectIndices)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnObjects: Nenhum ponto de spawn definido!");
            return;
        }

        // Evita index fora do limite
        int count = Mathf.Min(objectIndices.Length, spawnPoints.Count);

        for (int i = 0; i < count; i++)
        {
            int objIndex = objectIndices[i];
            if (objIndex < 0 || objIndex >= components.Count)
            {
                Debug.LogWarning($"SpawnObjects: �ndice {objIndex} fora do limite da lista de objetos!");
                continue;
            }

            GameObject prefab = components[objIndex];
            Transform point = spawnPoints[i];

            Instantiate(prefab, point.position, point.rotation);
        }
    }

    /// <summary>
    /// Gizmos no Editor � desenha esferas para visualizar os pontos de spawn.
    /// </summary>
    void OnDrawGizmosSelected()
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
