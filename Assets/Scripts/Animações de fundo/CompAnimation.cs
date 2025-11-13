using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CompAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cameraPrincipal;
    private bool estaArrastando = false;
    private Vector3 offset;

    [Header("Escala Durante Arrasto")]
    public Vector3 escalaDuranteArrasto = new Vector3(1.2f, 1.2f, 1f);

    [Header("Área Permitida (via Layer)")]
    public LayerMask areaPermitidaLayer;

    [Header("Animação Enquanto Segurado")]
    public Animator animator;
    public string triggerAnimacao = "Ativar";
    public float tempoMin = 1.5f;
    public float tempoMax = 4f;

    private Coroutine animacaoCoroutine;

    [HideInInspector] public Vector3 escalaOriginalSpawn;
    [HideInInspector] public Vector3 posicaoOriginalSpawn;

    private Vector3 escalaInicial;
    private Vector3 posicaoInicial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraPrincipal = Camera.main;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        posicaoInicial = transform.position;
        escalaInicial = transform.localScale;

        if (!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Início do arrasto
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                estaArrastando = true;
                offset = transform.position - (Vector3)mousePos;
                transform.localScale = escalaDuranteArrasto;

                // Inicia animação aleatória
                if (animacaoCoroutine == null)
                    animacaoCoroutine = StartCoroutine(TocarAnimacoesAleatorias());
            }
        }

        // Soltar o objeto
        if (Input.GetMouseButtonUp(0) && estaArrastando)
        {
            estaArrastando = false;

            // Para animações
            if (animacaoCoroutine != null)
            {
                StopCoroutine(animacaoCoroutine);
                animacaoCoroutine = null;
            }

            // Verifica se soltou fora da área permitida
            Collider2D areaValida = Physics2D.OverlapPoint(transform.position, areaPermitidaLayer);
            if (areaValida == null)
            {
                // Volta ao ponto e escala originais (prioriza dados de spawn)
                Vector3 posVoltar = (posicaoOriginalSpawn != Vector3.zero) ? posicaoOriginalSpawn : posicaoInicial;
                Vector3 escalaVoltar = (escalaOriginalSpawn != Vector3.zero) ? escalaOriginalSpawn : escalaInicial;

                transform.position = posVoltar;
                transform.localScale = escalaVoltar;
            }
        }
    }

    private IEnumerator TocarAnimacoesAleatorias()
    {
        while (estaArrastando)
        {
            yield return new WaitForSeconds(Random.Range(tempoMin, tempoMax));
            if (!estaArrastando) break;

            if (animator && !string.IsNullOrEmpty(triggerAnimacao))
                animator.SetTrigger(triggerAnimacao);
        }
    }

    private void FixedUpdate()
    {
        if (estaArrastando)
        {
            Vector2 mousePos = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos + (Vector2)offset);
        }
    }

    public void RegistrarEscalaEPosicaoOriginais(Vector3 pos, Vector3 escala)
    {
        posicaoOriginalSpawn = pos;
        escalaOriginalSpawn = escala;
    }
}
