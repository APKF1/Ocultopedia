using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragAndDropSprite : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cameraPrincipal;

    private bool estaDentro = false;
    private bool estaArrastando = false;
    private Vector3 offset;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (cameraPrincipal == null) cameraPrincipal = Camera.main;

        // Se usar Dynamic por algum motivo, isso evita cair/rodar.
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        // Inicia arraste quando clicar sobre o colisor
        if (estaDentro && Input.GetKeyDown(KeyCode.Mouse0))
        {
            estaArrastando = true;

            // Calcula offset para n�o "teleportar" ao centro
            Vector3 mouseWorld = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = transform.position.z;
            offset = transform.position - mouseWorld;
        }

        // Para ao soltar bot�o
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            estaArrastando = false;
        }
    }

    private void FixedUpdate()
    {
        if (estaArrastando)
        {
            Vector3 mouseWorld = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = transform.position.z; // mant�m o Z do objeto
            rb.MovePosition(mouseWorld + offset);
        }
    }

    private void OnMouseOver() { estaDentro = true; }
    private void OnMouseExit() { estaDentro = false; }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.cyan;
        var b = col.bounds;
        Gizmos.DrawWireCube(b.center, b.size);
    }
#endif
}