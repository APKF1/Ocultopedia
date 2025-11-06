using UnityEngine;

/// <summary>
/// Permite arrastar o objeto e alterar sua escala enquanto segurado.
/// Volta à posição e escala originais se solto fora da área permitida.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Redimensionamento : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cameraPrincipal;
    private bool estaArrastando = false;
    private Vector3 offset;

    [Header("Escala Durante Arrasto")]
    public Vector3 escalaDuranteArrasto = new Vector3(1.2f, 1.2f, 1f);

    [Header("Área Permitida (via Layer)")]
    public LayerMask areaPermitidaLayer;

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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                estaArrastando = true;
                offset = transform.position - (Vector3)mousePos;
                transform.localScale = escalaDuranteArrasto;
            }
        }

        if (Input.GetMouseButtonUp(0) && estaArrastando)
        {
            estaArrastando = false;

            Collider2D areaValida = Physics2D.OverlapPoint(transform.position, areaPermitidaLayer);
            if (areaValida == null)
            {
                Vector3 posVoltar = (posicaoOriginalSpawn != default(Vector3)) ? posicaoOriginalSpawn : posicaoInicial;
                Vector3 escalaVoltar = (escalaOriginalSpawn != default(Vector3)) ? escalaOriginalSpawn : escalaInicial;

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

    /// <summary>
    /// Recebe a posição e escala originais do objeto ao ser spawnado.
    /// Chamado automaticamente pelo SpawnObjects.
    /// </summary>
    public void RegistrarEscalaEPosicaoOriginais(Vector3 pos, Vector3 escala)
    {
        posicaoOriginalSpawn = pos;
        escalaOriginalSpawn = escala;
    }
}
