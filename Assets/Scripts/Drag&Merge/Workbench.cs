using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sistema de craft da Workbench.
/// Recebe componentes (via collider), inicia o processo,
/// define o resultado e respawna os itens usados.
/// </summary>
public class Workbench : MonoBehaviour
{
    // ---------------------------
    // CONFIGURAÇÃO DO CRAFT
    // ---------------------------

    [Header("Configurações de Craft")]
    public Transform spawnPoint;                     // Onde o resultado aparece
    public GameObject defaultResultPrefab;           // Resultado quando a combinação não existe
    public List<string> combinacoes = new();         // Lista de combinações válidas
    public GameObject[] resultados;                  // Resultados na mesma ordem da lista acima
    public float tempoDeCraft = 5f;                  // Tempo total do craft

    // ---------------------------
    // EFEITOS DO CALDEIRÃO
    // ---------------------------
    [Header("Animação e Som do Caldeirão")]
    public Animator animCaldeirao;
    public float velocidadeInicial = 1f;
    public float velocidadeFinal = 3f;

    public AudioSource audioCaldeirao;               // Fonte do áudio borbulhante
    public AudioClip somBorbulhando;
    public float volumeInicial = 0.4f;
    public float volumeFinal = 1f;
    public float pitchInicial = 1f;
    public float pitchFinal = 2f;

    // ---------------------------
    // SONS DO RESULTADO FINAL
    // ---------------------------
    [Header("Sons do Resultado Final")]
    public AudioSource audioResultado;               // Toca após o craft
    public AudioClip somResultadoPadrao;             // Toca no resultado errado
    public AudioClip somResultadoCorreto;            // Toca no resultado correto

    // ---------------------------
    // ARMAZENAMENTO DE COMPONENTES
    // ---------------------------
    private List<string> nomesComps = new();
    private List<Vector3> posicoesOriginais = new();
    private List<Vector3> escalasOriginais = new();

    private Coroutine craftRoutine;
    private bool craftAtivo = false;

    // ---------------------------------------------------
    // INTERAÇÕES — CHAMADO PELO SISTEMA DE FÍSICA DA UNITY
    // ---------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Component")) return;

        // Limpa o nome para comparação
        GameObject comp = other.gameObject;
        string nome = LimparNome(comp.name);

        // Dados de spawn vêm do script:
        // 👉 Redimensionamento.cs (registro de posição e escala)
        Redimensionamento redim = comp.GetComponent<Redimensionamento>();

        Vector3 posOriginal = redim ? redim.posicaoOriginalSpawn : comp.transform.position;
        Vector3 escalaOriginal = redim ? redim.escalaOriginalSpawn : comp.transform.localScale;

        nomesComps.Add(nome);
        posicoesOriginais.Add(posOriginal);
        escalasOriginais.Add(escalaOriginal);

        Destroy(comp);

        // Primeiro item ativa o processo
        if (!craftAtivo)
        {
            craftAtivo = true;
            IniciarEfeitosDoCaldeirao();
            craftRoutine = StartCoroutine(ProcessoDeCraft());
        }
    }

    // ---------------------------
    // FUNÇÕES DE CONTROLE
    // ---------------------------

    private void IniciarEfeitosDoCaldeirao()
    {
        if (animCaldeirao)
        {
            animCaldeirao.SetBool("Ativo", true);
            animCaldeirao.speed = velocidadeInicial;
        }

        if (audioCaldeirao && somBorbulhando)
        {
            audioCaldeirao.clip = somBorbulhando;
            audioCaldeirao.loop = true;
            audioCaldeirao.volume = volumeInicial;
            audioCaldeirao.pitch = pitchInicial;
            audioCaldeirao.Play();
        }
    }

    private string LimparNome(string original) =>
        original.Replace("(Clone)", "").Trim();

    // ---------------------------
    // PROCESSO DO CRAFT
    // ---------------------------

    private IEnumerator ProcessoDeCraft()
    {
        float tempo = tempoDeCraft;

        // Aceleração do caldeirão + áudio
        while (tempo > 0f)
        {
            tempo -= Time.deltaTime;
            float p = 1f - (tempo / tempoDeCraft);

            if (animCaldeirao)
                animCaldeirao.speed = Mathf.Lerp(velocidadeInicial, velocidadeFinal, p);

            if (audioCaldeirao)
            {
                audioCaldeirao.pitch = Mathf.Lerp(pitchInicial, pitchFinal, p);
                audioCaldeirao.volume = Mathf.Lerp(volumeInicial, volumeFinal, p);
            }

            yield return null;
        }

        // Define resultado final
        GameObject resultado = DefinirResultadoFinal(out bool ehCorreto);

        Instantiate(resultado, spawnPoint.position, Quaternion.identity);

        // Toca áudio específico
        TocarSomDoResultado(ehCorreto);

        // Respawna componentes usados
        RespawnarComponentesUsados();

        // Finaliza efeitos
        FinalizarEfeitosDoCaldeirao();

        nomesComps.Clear();
        posicoesOriginais.Clear();
        escalasOriginais.Clear();
        craftAtivo = false;
    }

    // ---------------------------
    // RESULTADO DO CRAFT
    // ---------------------------

    private GameObject DefinirResultadoFinal(out bool ehCorreto)
    {
        List<string> copia = new(nomesComps);
        copia.Sort();
        string combinacaoFinal = string.Join("", copia);

        // Ligação direta com:
        // 👉 Lista "combinacoes" configurada na Unity
        int idx = combinacoes.IndexOf(combinacaoFinal);

        ehCorreto = idx >= 0 && idx + 1 < resultados.Length;

        return ehCorreto ? resultados[idx + 1] : defaultResultPrefab;
    }

    private void TocarSomDoResultado(bool correto)
    {
        if (!audioResultado) return;

        if (correto && somResultadoCorreto)
            audioResultado.PlayOneShot(somResultadoCorreto);

        else if (!correto && somResultadoPadrao)
            audioResultado.PlayOneShot(somResultadoPadrao);
    }

    // ---------------------------
    // FINALIZAÇÃO DO CALDEIRÃO
    // ---------------------------

    private void FinalizarEfeitosDoCaldeirao()
    {
        if (animCaldeirao)
        {
            animCaldeirao.speed = 1f;
            animCaldeirao.SetBool("Ativo", false);
        }

        if (audioCaldeirao)
            StartCoroutine(EncerrarSomGradualmente());
    }

    private IEnumerator EncerrarSomGradualmente()
    {
        float dur = 0.8f;
        float start = audioCaldeirao.volume;
        float t = 0f;

        while (t < dur)
        {
            t += Time.deltaTime;
            audioCaldeirao.volume = Mathf.Lerp(start, 0f, t / dur);
            yield return null;
        }

        audioCaldeirao.Stop();
        audioCaldeirao.volume = volumeInicial;
        audioCaldeirao.pitch = pitchInicial;
    }

    // ---------------------------
    // RESPAWN DOS COMPONENTES
    // ---------------------------

    private void RespawnarComponentesUsados()
    {
        // Busca automática pelo script:
        // 👉 SpawnObjects.cs
        SpawnObjects spawner = FindObjectOfType<SpawnObjects>();

        if (spawner == null)
        {
            Debug.LogWarning("SpawnObjects não encontrado para respawn.");
            return;
        }

        for (int i = 0; i < nomesComps.Count; i++)
        {
            string nome = nomesComps[i];
            Vector3 pos = posicoesOriginais[i];
            Vector3 escala = escalasOriginais[i];

            // Puxa prefab da lista do SpawnObjects
            GameObject prefab = BuscarPrefabPorNome(spawner, nome);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab não encontrado para '{nome}'.");
                continue;
            }

            GameObject novo = Instantiate(prefab, pos, Quaternion.identity);
            novo.transform.localScale = escala;

            // Registra posição/escala via:
            // 👉 Redimensionamento.cs
            Redimensionamento redim = novo.GetComponent<Redimensionamento>();
            redim?.RegistrarEscalaEPosicaoOriginais(pos, escala);
        }
    }

    private GameObject BuscarPrefabPorNome(SpawnObjects spawner, string nomeLimpo)
    {
        // Faz correspondência com:
        // 👉 SpawnObjects.componentes
        foreach (GameObject prefab in spawner.componentes)
        {
            string p = LimparNome(prefab.name);
            if (string.Equals(p, nomeLimpo, System.StringComparison.OrdinalIgnoreCase))
                return prefab;
        }
        return null;
    }
}
