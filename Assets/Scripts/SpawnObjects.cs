using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsável por spawnar objetos nos pontos definidos.
/// Cada spawnpoint define a escala via o script ScaleSet.
/// Corrige o problema garantindo que o valor de escala original do spawn
/// seja gravado exatamente após aplicar a escala do spawnpoint.
/// </summary>
public class SpawnObjects : MonoBehaviour
{
    [Header("Prefabs dos Componentes")]
    [SerializeField] private List<GameObject> components = new List<GameObject>();

    [Header("Pontos de Spawn")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Som de Spawn (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;

    private int counter = 0;

    /// <summary>
    /// Spawna os objetos principais da fase.
    /// items: lista de índices em 'components' a serem instanciados na ordem dos spawnPoints.
    /// </summary>
    public void SpawnarFase(List<int> items)
    {
        counter = 0;

        if (components == null || components.Count == 0 || spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnObjects: faltam prefabs ou pontos de spawn!");
            return;
        }

        // Usamos um índice simples para percorrer spawnPoints (mantendo a ordem)
        for (int i = 0; i < items.Count && counter < spawnPoints.Count; i++)
        {
            int prefabIndex = items[i];
            if (prefabIndex < 0 || prefabIndex >= components.Count)
            {
                counter++;
                continue;
            }

            Transform point = spawnPoints[counter++];
            GameObject prefab = components[prefabIndex];
            if (prefab == null || point == null) continue;

            GameObject obj = Instantiate(prefab, point.position, point.rotation);

            // Aplica escala do spawnpoint (se houver ScaleSet). Usar atribuição direta para evitar erros acumulados.
            var scaleSet = point.GetComponent<ScaleSet>();
            float scaleValue = scaleSet != null ? scaleSet.scale : 1f;

            // Garante que aplicamos a escala correta (escala final = prefab.localScale * scaleValue)
            Vector3 finalScale = new Vector3(prefab.transform.localScale.x * scaleValue,
                                             prefab.transform.localScale.y * scaleValue,
                                             prefab.transform.localScale.z);
            obj.transform.localScale = finalScale;

            // Passa dados originais para o script Redimensionamento (salvando a escala final aplicada)
            var redim = obj.GetComponent<Redimensionamento>();
            if (redim != null)
            {
                redim.escalaOriginalSpawn = obj.transform.localScale; // agora corresponde à escala aplicada
                redim.posicaoOriginalSpawn = point.position;
            }
        }

        // Toca som (opcional)
        if (audioSource != null && spawnSound != null)
            audioSource.PlayOneShot(spawnSound);
    }

    // Mostra os pontos no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var p in spawnPoints)
        {
            if (p != null)
                Gizmos.DrawWireSphere(p.position, 0.25f);
        }
    }
}