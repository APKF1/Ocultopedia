using UnityEngine;
using System.Collections;

public class AnimacaoAleatoria : MonoBehaviour
{
    [Header("🎞️ Configurações de Animação")]
    public Animator animator;                  // Animator do objeto
    public string triggerName = "Ativar";      // Nome do trigger usado na animação

    [Tooltip("Velocidade base da animação.")]
    [Range(0.1f, 3f)] public float velocidadeAnimacao = 1f;

    [Tooltip("Se verdadeiro, a velocidade varia aleatoriamente entre min e max.")]
    public bool usarVelocidadeAleatoria = false;
    [Range(0.1f, 3f)] public float velocidadeMin = 0.8f;
    [Range(0.1f, 3f)] public float velocidadeMax = 1.2f;

    [Header("⏱️ Intervalo entre ativações")]
    public float tempoMin = 3f;
    public float tempoMax = 8f;

    [Header("🔊 Som da Animação")]
    public AudioSource audioSource;
    public AudioClip somDaAnimacao;
    [Range(0f, 1f)] public float volumeSom = 1f;

    private bool rodando = false;

    private void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();

        StartCoroutine(TocarAnimacoesAleatorias());
    }

    private IEnumerator TocarAnimacoesAleatorias()
    {
        rodando = true;

        while (rodando)
        {
            // Aguarda um tempo aleatório antes da próxima animação
            float espera = Random.Range(tempoMin, tempoMax);
            yield return new WaitForSeconds(espera);

            // Define a velocidade da animação
            if (animator)
            {
                float velocidade = usarVelocidadeAleatoria
                    ? Random.Range(velocidadeMin, velocidadeMax)
                    : velocidadeAnimacao;

                animator.speed = velocidade;
                animator.SetTrigger(triggerName);
            }

            // Toca o som da animação (se configurado)
            if (audioSource && somDaAnimacao)
                audioSource.PlayOneShot(somDaAnimacao, volumeSom);
        }
    }

    public void PararAnimacoes()
    {
        rodando = false;
        StopAllCoroutines();
    }
}
