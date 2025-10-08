using UnityEngine;

/// <summary>
/// Controla a gera��o de clientes e o fluxo de entrada.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    [Header("Refer�ncias")]
    public FadeController fadeController;
    public Transform spawnPoint; // onde o cliente aparece
    public GameObject[] customerPrefabs; // lista de clientes poss�veis

    private GameObject clienteAtual;

    private void Start()
    {
        // Liga o evento do Fade ao m�todo de spawn
        fadeController.OnFadeFullDark.AddListener(SpawnarNovoCliente);
    }

    /// <summary>
    /// Chama o fade e inicia o processo de entrada do cliente.
    /// </summary>
    public void NovoCliente()
    {
        fadeController.StartFadeSequence();
    }

    /// <summary>
    /// Gera o cliente no momento em que a tela est� escura.
    /// </summary>
    private void SpawnarNovoCliente()
    {
        if (clienteAtual != null)
            Destroy(clienteAtual);

        int index = Random.Range(0, customerPrefabs.Length);
        clienteAtual = Instantiate(customerPrefabs[index], spawnPoint.position, Quaternion.identity);

        Debug.Log($"Novo cliente gerado: {clienteAtual.name}");
    }
}
