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
                posX = -2.97f;
                posY = -1.307f;
                break;

            case "Cliente 2":
                posX = -3.208139f;
                posY = -1.8f;
                break;

            case "Cliente 3":
                posX = -3.21f;
                posY = -1.13f;
                break;

            case "Cliente 4":
                posX = -3.11f;
                posY = -2.29f;
                break;

            case "Cliente 5":
                posX = -2.38f;
                posY = -1.92f;
                break;

            case "Cliente 6":
                posX = -2.98f;
                posY = -2.09f;
                break;

            case "Cliente 7":
                posX = -2.51f;
                posY = -1.7942f;
                break;

            case "Cliente 8":
                posX = -3.45f;
                posY = -0.62f;
                break;

            case "Cliente 9":
                posX = -2.61f;
                posY = -1.84f;
                break;

            case "Cliente 10":
                posX = -3.8591f;
                posY = -0.53f;
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
