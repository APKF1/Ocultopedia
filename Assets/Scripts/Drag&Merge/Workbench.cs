using UnityEngine;
using System.Collections.Generic;

public class Workbench : MonoBehaviour
{
    public Transform spawnPoint;   // Onde o item final vai nascer
    public GameObject defaultResultPrefab; // Prefab do item padrão (caso não haja combinação)

    public List<string> components = new List<string>(); // Lista utilizada para ordenar em ordem alfabética
    public List<string> combinacao = new List<string>() {{"Objeto1Objeto2"}};
    public GameObject[] resultados ;

    public string comps;

    // Quando um componente entra na área da Workbench
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string AWOOOBA = other.gameObject.name;
            components.Add(AWOOOBA);
            Debug.Log($"Componente adicionado: {AWOOOBA}");
        }
        Debug.Log(components[0]);
    }

    // Quando um componente sai da área
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string AWOOOBA = other.gameObject.name;
            components.Remove(AWOOOBA);
            Debug.Log($"Componente removido: {AWOOOBA}");
        }
    }

    // Método chamado pelo botão
    public void Craft()
    {
        if (components.Count <= 1)
        {
            Debug.Log("Componentes faltando na Workbench!");
            return;
        }


        components.Sort();


        GameObject resultPrefab = GetResultFromComponents(); // escolhe o resultado
        Instantiate(resultPrefab, spawnPoint.position, Quaternion.identity);

        Debug.Log("Item criado!");

        // Aqui destruímos os componentes usados (opcional)
        GameObject[] compsInWorkbench = GameObject.FindGameObjectsWithTag("Component");
        foreach (var c in compsInWorkbench)
        {
            if (c.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
                Destroy(c);
        }

        components.Clear(); // limpa a lista
    }

    // Decide o que criar dependendo dos componentes
    private GameObject GetResultFromComponents()
    {
        for (int i = 0; i < 2; i++)
        {
            string compName = components[i];
            if (compName.Contains("(Clone)"))
            {
                compName =  compName.Replace("(Clone)", "");
                components[i] = compName;
            }
        }
        comps = components[0] + components[1];
        Debug.Log(comps);
        Debug.Log(components[0]);
        Debug.Log(components[1]);
        int indexResult = combinacao.IndexOf(comps);
        Debug.Log(indexResult);

        // Se não bate com nenhuma receita, retorna o item padrão
        return resultados[indexResult + 1]; // Sempre index + 1 pq index 0 é o item faltando
    }
}
