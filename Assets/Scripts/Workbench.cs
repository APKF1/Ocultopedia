using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ocultopedia.Gameplay
{
    // Workbench: recebe componentes (Trigger2D) e cria um item quando o jogador aciona Craft().
    [RequireComponent(typeof(Collider2D))]
    public class Workbench : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;                  // ponto onde o resultado aparece
        [SerializeField] private GameObject defaultResultPrefab;        // item padrão caso não exista receita
        [SerializeField] private List<string> combinacoes = new List<string>(); // chaves (ex: "A+B")
        [SerializeField] private List<GameObject> resultados = new List<GameObject>(); // resultados correspondentes

        // lista temporária de componentes presentes (usada para ordenar e formar a chave)
        private readonly List<string> components = new List<string>();

        // quando um componente entra na área
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Component")) return;
            components.Add(CleanName(other.gameObject.name));
        }

        // quando um componente sai da área
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Component")) return;
            // remove apenas uma ocorrência (caso haja duplicatas)
            var name = CleanName(other.gameObject.name);
            components.Remove(name);
        }

        // método público chamado pelo botão de craft
        public void Craft()
        {
            if (components.Count <= 1)
            {
                Debug.Log("Componentes faltando na Workbench!");
                return;
            }

            // cria a chave ordenada e procura a receita
            components.Sort();
            var key = string.Join("+", components);
            Debug.Log($"Workbench: tentando combinar -> {key}");

            var resultPrefab = GetResultFromKey(key);
            if (resultPrefab == null)
            {
                Debug.LogWarning("Workbench: nenhum prefab de resultado disponível.");
                ClearComponentsInWorkbench();
                components.Clear();
                return;
            }

            Instantiate(resultPrefab, spawnPoint != null ? spawnPoint.position : transform.position, Quaternion.identity);
            Debug.Log("Item criado!");

            ClearComponentsInWorkbench();
            components.Clear();
        }

        // constrói a chave limpa a partir de um nome, removendo "(Clone)" se necessário
        private static string CleanName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return name.Replace("(Clone)", "").Trim();
        }

        // retorna o prefab do resultado com segurança
        private GameObject GetResultFromKey(string key)
        {
            if (combinacoes == null || resultados == null)
                return defaultResultPrefab;

            int idx = combinacoes.IndexOf(key);
            if (idx >= 0 && idx < resultados.Count && resultados[idx] != null)
                return resultados[idx];

            return defaultResultPrefab;
        }

        // destrói apenas os objetos Component que estiverem tocando essa workbench
        private void ClearComponentsInWorkbench()
        {
            var myCollider = GetComponent<Collider2D>();
            if (myCollider == null) return;

            var compsInScene = GameObject.FindGameObjectsWithTag("Component");
            foreach (var c in compsInScene)
            {
                var col = c.GetComponent<Collider2D>();
                if (col != null && col.IsTouching(myCollider))
                    Destroy(c);
            }
        }
    }
}