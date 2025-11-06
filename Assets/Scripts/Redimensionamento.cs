using UnityEngine;

/// <summary>
/// Permite arrastar o objeto e alterar sua escala enquanto segurado.
/// Volta à posição e escala originais se solto fora da área permitida.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Redimensionamento : MonoBehaviour
{
    [Header("Arrasto")]
    private Rigidbody2D rb;
    private Camera cameraPrincipal;
    private bool estaArrastando = false;
    private Vector3 offset;

    [Header("Escala Durante Arrasto")]
    public Vector3 escalaDuranteArrasto = new Vector3(1.2f, 1.2f, 1f);

    [Header("Área Permitida (via Layer)")]
    public LayerMask areaPermitidaLayer;

    // Dados originais (preenchidos pelo SpawnObjects)
    [HideInInspector] public Vector3 escalaOriginalSpawn;
    [HideInInspector] public Vector3 posicaoOriginalSpawn;

    // Backup local (em caso de spawn sem referência)
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
    }

    private void Update()
    {
        // Início do arrasto (ao clicar sobre o objeto)
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                estaArrastando = true;
                offset = transform.position - (Vector3)mousePos;

                // Aplica escala configurada durante o arrasto
                transform.localScale = escalaDuranteArrasto;
            }
        }

        // Solta o objeto
        if (Input.GetMouseButtonUp(0) && estaArrastando)
        {
            estaArrastando = false;

            // Verifica se está em uma área válida
            Collider2D areaValida = Physics2D.OverlapPoint(transform.position, areaPermitidaLayer);
            if (areaValida == null)
            {
                // Volta ao ponto e escala originais
                Vector3 posVoltar = posicaoOriginalSpawn != Vector3.zero ? posicaoOriginalSpawn : posicaoInicial;
                Vector3 escalaVoltar = escalaOriginalSpawn != Vector3.zero ? escalaOriginalSpawn : escalaInicial;

                transform.position = posVoltar;
                transform.localScale = escalaVoltar;
            }
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
}