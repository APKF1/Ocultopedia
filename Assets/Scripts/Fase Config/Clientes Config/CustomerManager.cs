using UnityEngine;

/// <summary>
/// Controla a geração dos clientes e o fluxo de entrada.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    [Header("Referências")]
    public FadeController fadeController;
    public Transform spawnPoint;               // Onde o cliente aparece
    public GameObject[] customerPrefabs;       // Lista de clientes

    private GameObject clienteAtual;

    private void Start()
    {
        // Escuta o evento do Fade para spawnar o cliente
        fadeController.OnFadeFullDark.AddListener(SpawnarNovoCliente);
    }

    /*public void NovoCliente()
    {
        fadeController.StartFadeSequence();
    }*/

    private void SpawnarNovoCliente()
    {
        if (clienteAtual != null)
            Destroy(clienteAtual);

        int index = Random.Range(0, customerPrefabs.Length);
        clienteAtual = Instantiate(customerPrefabs[index], spawnPoint.position, Quaternion.identity);
        //Debug.Log($"🧍 Novo cliente gerado: {clienteAtual.name}");
    }
}
