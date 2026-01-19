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
    public GameObject clienteAtual;
    private float posX;
    private float posY;

    private void Start()
    {
        // Escuta o evento do Fade para spawnar o cliente
        fadeController.OnFadeFullDark.AddListener(SpawnarNovoCliente);
    }

    public void NovoCliente()
    {
        fadeController.StartFadeSequence();
    }

    private void SpawnarNovoCliente()
    {
        string nomeCliente = clienteAtual.name;

        switch (nomeCliente)
        {
            case "Cliente 1":
                posX = -2.5f;
                posY = -1.35f;
                break;

            default:
                Debug.Log("Erro ao definir cliente!");
                break;

        }




        /*if (clienteAtual != null)
            Destroy(clienteAtual);
        */
        //int index = Random.Range(0, customerPrefabs.Length);
        //clienteAtual = Instantiate(customerPrefabs[index], spawnPoint.position, Quaternion.identity);
        clienteAtual.transform.position = new Vector3(posX, posY, 0);
        //Debug.Log($"🧍 Novo cliente gerado: {clienteAtual.name}");
    }
}
