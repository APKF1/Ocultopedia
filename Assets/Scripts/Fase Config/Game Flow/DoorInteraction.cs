using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Detector "universal" de clique na porta:
/// - Suporta UI (IPointerClickHandler) quando a porta for parte do Canvas (GraphicRaycaster).
/// - Suporta OnMouseDown (3D/2D collider) para GameObjects na cena.
/// - Suporta toque/mobile via raycast manual (Update).
/// - Evita múltiplos cliques/ativação dupla.
/// - Reproduz som opcional e chama FadeController / CustomerManager / GameFlowManager.
/// </summary>
[RequireComponent(typeof(Collider))] // requer um Collider 3D; se for 2D, remova e coloque Collider2D em outra versão
public class DoorInteraction : MonoBehaviour, IPointerClickHandler
{
    [Header("Referências (arraste no Inspector)")]
    public FadeController fade;
    public CustomerManager customerManager;
    public GameFlowManager flowManager;
   // public AudioSource clickAudioSource;    // som da porta (opcional)
    //public AudioClip clickClip;             // clip para PlayOneShot (opcional)

    [Header("Raycast (para toque)")]
    public LayerMask clickableLayers = ~0;  // quais layers respondem ao clique (por padrão, todos)

    bool opened = false;

    void Start()
    {
        /* Small sanity checks with friendly logs
        if (fade == null) Debug.LogWarning("DoorInteraction: FadeController não atribuído.");
        if (customerManager == null) Debug.LogWarning("DoorInteraction: CustomerManager não atribuído.");
        if (flowManager == null) Debug.LogWarning("DoorInteraction: GameFlowManager não atribuído.");
        if (clickAudioSource == null && clickClip != null)
            Debug.LogWarning("DoorInteraction: clickClip definido mas clickAudioSource está nulo."); */
    }

    // IPointerClickHandler -> captura cliques via EventSystem / UI (se porta estiver sob Canvas como UI element)
    public void OnPointerClick(PointerEventData eventData)
    {
        //TryOpenDoor("UI PointerClick");
    }

    // OnMouseDown -> captura clicks em GameObjects com Collider (desktop/browser)
    private void OnMouseDown()
    {
        TryOpenDoor("OnMouseDown");
    }

    // Touch fallback: checa toques e faz um Physics.Raycast para este objeto
    void Update()
    {
        if (opened) return;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, clickableLayers))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        TryOpenDoor("Touch Raycast");
                    }
                }
            }
        }

        // Optional: also support mouse clicks via raycast (useful if object is behind UI)
        if (Input.GetMouseButtonDown(0))
        {
            // ignore if pointer over UI (so clickable UI elements still work)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit2, 100f, clickableLayers))
            {
                if (hit2.collider != null && hit2.collider.gameObject == gameObject)
                {
                    TryOpenDoor("Mouse Raycast");
                }
            }
        }
    }

    void TryOpenDoor(string source)
    {
        if (opened) return;
        opened = true;

        // Notifica flow manager
        if (flowManager != null) flowManager.PortaAberta();

        // Toca som
        /*if (clickAudioSource != null)
        {
            if (clickClip != null) clickAudioSource.PlayOneShot(clickClip);
            else clickAudioSource.Play();
        } */

        // Inicia fade e cliente (se atribuídos)
        if (fade != null) fade.StartFadeSequence();
        if (customerManager != null) customerManager.NovoCliente();
    }

    // Método público para (re)resetar a porta se quiser reutilizar o objeto entre fases
    public void ResetDoor()
    {
        opened = false;
    }
}
