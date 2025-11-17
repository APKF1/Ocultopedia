using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Workbench : MonoBehaviour
{
    [Header("Configurações de Craft")]
    public Transform spawnPoint;
    public GameObject defaultResultPrefab;
    public List<string> combinacoes = new();
    public GameObject[] resultados;
    public float tempoDeCraft = 5f;

    [Header("Animação e Som do Caldeirão")]
    public Animator animCaldeirao;
    [Range(1f, 3f)] public float velocidadeInicial = 1f;
    [Range(1f, 10f)] public float velocidadeFinal = 3f;
    public AudioSource audioCaldeirao;
    public AudioClip somBorbulhando;
    [Range(0f, 1f)] public float volumeInicial = 0.4f;
    [Range(0f, 1f)] public float volumeFinal = 1f;
    [Range(0.5f, 2f)] public float pitchInicial = 1f;
    [Range(0.5f, 3f)] public float pitchFinal = 2f;

    [Header("Sons do Resultado Final")]
    public AudioSource audioResultado;
    public AudioClip somResultadoPadrao;   // toca quando gerar defaultResultPrefab
    public AudioClip somResultadoCorreto;  // toca quando gerar qualquer item válido

    // dados salvos por componente usado
    private List<string> nomesComps = new();
    private List<Vector3> posicoesOriginais = new();
    private List<Vector3> escalasOriginais = new();

    private Coroutine craftRoutine;
    private bool craftAtivo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Component")) return;

        GameObject comp = other.gameObject;
        string nome = LimparNome(comp.name);

        Redimensionamento redim = comp.GetComponent<Redimensionamento>();

        Vector3 posOriginal = redim ? redim.posicaoOriginalSpawn : comp.transform.position;
        Vector3 escalaOriginal = redim ? redim.escalaOriginalSpawn : comp.transform.localScale;

        nomesComps.Add(nome);
        posicoesOriginais.Add(posOriginal);
        escalasOriginais.Add(escalaOriginal);

        Destroy(comp);

        if (!craftAtivo)
        {
            craftAtivo = true;

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

            craftRoutine = StartCoroutine(ProcessoDeCraft());
        }
    }

    private string LimparNome(string original) =>
        original.Replace("(Clone)", "").Trim();

    private IEnumerator ProcessoDeCraft()
    {
        float tempo = tempoDeCraft;

        while (tempo > 0f)
        {
            tempo -= Time.deltaTime;
            float progresso = 1f - (tempo / tempoDeCraft);

            if (animCaldeirao)
                animCaldeirao.speed = Mathf.Lerp(velocidadeInicial, velocidadeFinal, progresso);

            if (audioCaldeirao)
            {
                audioCaldeirao.pitch = Mathf.Lerp(pitchInicial, pitchFinal, progresso);
                audioCaldeirao.volume = Mathf.Lerp(volumeInicial, volumeFinal, progresso);
            }

            yield return null;
        }

        // --- Determinar resultado ---
        List<string> copiaNomes = new(nomesComps);
        copiaNomes.Sort();
        string combinacaoFinal = string.Join("", copiaNomes);

        int idx = combinacoes.IndexOf(combinacaoFinal);

        bool resultadoCorreto = (idx >= 0 && idx + 1 < resultados.Length);
        GameObject resultado = resultadoCorreto ? resultados[idx + 1] : defaultResultPrefab;

        Instantiate(resultado, spawnPoint.position, Quaternion.identity);

        // --- Tocar som adequado ---
        if (audioResultado)
        {
            if (resultadoCorreto && somResultadoCorreto)
                audioResultado.PlayOneShot(somResultadoCorreto);
            else if (!resultadoCorreto && somResultadoPadrao)
                audioResultado.PlayOneShot(somResultadoPadrao);
        }

        // --- Respawnar os componentes usados ---
        RespawnarComponentesUsados();

        // --- Finalizar efeitos ---
        if (animCaldeirao)
        {
            animCaldeirao.speed = 1f;
            animCaldeirao.SetBool("Ativo", false);
        }

        if (audioCaldeirao)
            StartCoroutine(EncerrarSomGradualmente());

        nomesComps.Clear();
        posicoesOriginais.Clear();
        escalasOriginais.Clear();
        craftAtivo = false;
    }

    private IEnumerator EncerrarSomGradualmente()
    {
        if (audioCaldeirao == null) yield break;

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

    private void RespawnarComponentesUsados()
    {
        SpawnObjects spawner = FindObjectOfType<SpawnObjects>();
        if (spawner == null)
        {
            Debug.LogWarning("SpawnObjects não encontrado — não é possível respawnar componentes.");
            return;
        }

        for (int i = 0; i < nomesComps.Count; i++)
        {
            string nome = nomesComps[i];
            Vector3 pos = posicoesOriginais[i];
            Vector3 escala = escalasOriginais[i];

            GameObject prefab = BuscarPrefabPorNome(spawner, nome);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab não encontrado para '{nome}'");
                continue;
            }

            GameObject novo = Instantiate(prefab, pos, Quaternion.identity);
            novo.transform.localScale = escala;

            Redimensionamento redim = novo.GetComponent<Redimensionamento>();
            redim?.RegistrarEscalaEPosicaoOriginais(pos, escala);
        }
    }

    private GameObject BuscarPrefabPorNome(SpawnObjects spawner, string nomeLimpo)
    {
        foreach (GameObject prefab in spawner.componentes)
        {
            string p = LimparNome(prefab.name);
            if (string.Equals(p, nomeLimpo, System.StringComparison.OrdinalIgnoreCase))
                return prefab;
        }
        return null;
    }
}
