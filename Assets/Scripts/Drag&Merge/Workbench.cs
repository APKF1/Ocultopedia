// Updated Workbench.cs with Trigger "ItemEntrou" added
// Only the requested modification was made.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    public float velocidadeInicial = 1f;
    public float velocidadeFinal = 3f;

    public AudioSource audioCaldeirao;
    public AudioClip somBorbulhando;
    public float volumeInicial = 0.4f;
    public float volumeFinal = 1f;
    public float pitchInicial = 1f;
    public float pitchFinal = 2f;

    [Header("Sons do Resultado Final")]
    public AudioSource audioResultado;
    public AudioClip somResultadoPadrao;
    public AudioClip somResultadoCorreto;

    public List<string> nomesComps = new();
    public List<Vector3> posicoesOriginais = new();
    public List<Vector3> escalasOriginais = new();

    private Coroutine craftRoutine;
    private bool craftAtivo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Component")) return;

        GameObject comp = other.gameObject;
        string nome = LimparNome(comp.name);

        Redimensionamento redim = comp.GetComponent<Redimensionamento>();

        CompAnimation compAnimation = comp.GetComponent<CompAnimation>();

        Vector3 posOriginal;
        Vector3 escalaOriginal;

        if (redim != null)
        {
            posOriginal = redim ? redim.posicaoOriginalSpawn : comp.transform.position;
            escalaOriginal = redim ? redim.escalaOriginalSpawn : comp.transform.localScale;
        }
        else if (compAnimation != null)
        {
            posOriginal = compAnimation ? compAnimation.posicaoOriginalSpawn : comp.transform.position;
            escalaOriginal = compAnimation ? compAnimation.escalaOriginalSpawn : comp.transform.localScale;
        }
        else
        {
            return;
        }

        nomesComps.Add(nome);
        posicoesOriginais.Add(posOriginal);
        Debug.Log(posOriginal);
        escalasOriginais.Add(escalaOriginal);

        Destroy(comp);

        // -----------------
        // Trigger adicionado
        // -----------------
        if (animCaldeirao)
            animCaldeirao.SetTrigger("ItemEntrou");

        if (!craftAtivo)
        {
            craftAtivo = true;
            IniciarEfeitosDoCaldeirao();
            craftRoutine = StartCoroutine(ProcessoDeCraft());
        }
    }

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

    private string LimparNome(string original) => original.Replace("(Clone)", "").Trim();

    private IEnumerator ProcessoDeCraft()
    {
        float tempo = tempoDeCraft;

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

        GameObject resultado = DefinirResultadoFinal(out bool ehCorreto);

        Instantiate(resultado, spawnPoint.position, Quaternion.identity);

        // Trigger chamado após gerar o artefato/resultado
        if (animCaldeirao)
            animCaldeirao.SetTrigger("ResultadoGerado");

        TocarSomDoResultado(ehCorreto);

        RespawnarComponentesUsados();

        FinalizarEfeitosDoCaldeirao();

        nomesComps.Clear();
        posicoesOriginais.Clear();
        escalasOriginais.Clear();
        craftAtivo = false;
    }

    private GameObject DefinirResultadoFinal(out bool ehCorreto)
    {
        List<string> copia = new(nomesComps);
        copia.Sort();
        string combinacaoFinal = string.Join("", copia);

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

    private void RespawnarComponentesUsados()
    {
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

            GameObject prefab = BuscarPrefabPorNome(spawner, nome);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab não encontrado para '{nome}'.");
                continue;
            }

            Debug.Log(prefab);
            Debug.Log(pos);
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
