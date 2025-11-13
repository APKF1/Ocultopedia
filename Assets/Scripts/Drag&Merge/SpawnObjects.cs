using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de spawn de objetos.
/// Lê dados dos spawnpoints, aplica a escala definida neles e
/// registra as informações originais no Redimensionamento.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SpawnObjects : MonoBehaviour
{
    [Header("Prefabs disponíveis")]
    public List<GameObject> componentes = new List<GameObject>();

    [Header("Pontos de spawn")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Configurações")]
    public bool spawnAleatorio = true;
    public AudioClip somSpawn;

    private AudioSource audioSrc;
    private int contador = 0;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gera os objetos da fase.
    /// </summary>
    public void SpawnarFase(int n, List<int> indicesFixos)
    {
        if (componentes.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnObjects: Lista de componentes ou pontos vazia.");
            return;
        }

        SpawnFixos(n, indicesFixos);
        SpawnAleatorios(spawnPoints.Count - n);

        if (somSpawn && audioSrc)
            audioSrc.PlayOneShot(somSpawn);

        Debug.Log("🧩 Objetos da fase spawnados com sucesso!");
    }

    private void SpawnFixos(int n, List<int> indices)
    {
        foreach (int i in indices)
        {
            if (contador >= spawnPoints.Count) break;
            CriarObjeto(componentes[i], spawnPoints[contador]);
            contador++;
        }
    }

    private void SpawnAleatorios(int n)
    {
        for (int i = 0; i < n && contador < spawnPoints.Count; i++)
        {
            int prefabIndex = Random.Range(0, componentes.Count);
            CriarObjeto(componentes[prefabIndex], spawnPoints[contador]);
            contador++;
        }
    }

    /// <summary>
    /// Cria o objeto no ponto especificado e aplica escala e dados originais.
    /// </summary>
    private void CriarObjeto(GameObject prefab, Transform spawnPoint)
    {
        GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Aplica escala definida pelo ScaleSet do spawnpoint
        ScaleSet scaleSet = spawnPoint.GetComponent<ScaleSet>();
        if (scaleSet)
            obj.transform.localScale *= scaleSet.scale;

        // Registra posição e escala originais (funciona para Redimensionamento ou CompAnimation)
        Redimensionamento redim = obj.GetComponent<Redimensionamento>();
        CompAnimation compAnim = obj.GetComponent<CompAnimation>();

        if (redim)
            redim.RegistrarEscalaEPosicaoOriginais(spawnPoint.position, obj.transform.localScale);
        else if (compAnim)
            compAnim.RegistrarEscalaEPosicaoOriginais(spawnPoint.position, obj.transform.localScale);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var p in spawnPoints)
            if (p) Gizmos.DrawWireSphere(p.position, 0.3f);
    }
}
