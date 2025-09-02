using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [Header("Configurações do Frasco")]
    public int capacity = 4;                     // Quantos líquidos cabem no frasco
    public float pourSpeed = 2f;                 // Velocidade de esvaziamento
    public float fillSpeed = 2f;                 // Velocidade de preenchimento
    public Transform liquidContainer;            // Onde os líquidos ficarão

    [Header("Prefabs e Sprites")]
    public GameObject liquidPiecePrefab;         // Prefab do pedaço de líquido

    private List<GameObject> liquids = new List<GameObject>();
    private bool isSelected = false;

    // Para controle interno
    private Vector3 liquidPieceScale;

    void Start()
    {
        // Define a escala padrão dos pedaços de líquido com base no container
        if (liquidContainer != null)
            liquidPieceScale = new Vector3(1f, 1f / capacity, 1f);
    }

    // Inicializa o frasco com uma lista de cores
    public void Initialize(List<Color> colors)
    {
        foreach (Color c in colors)
        {
            AddLiquid(c, instant: true);
        }
    }

    // Adiciona um líquido no frasco
    public void AddLiquid(Color color, bool instant = false)
    {
        if (liquids.Count >= capacity) return;

        GameObject newLiquid = Instantiate(liquidPiecePrefab, liquidContainer);
        newLiquid.transform.localScale = liquidPieceScale;
        newLiquid.transform.localPosition = new Vector3(0, liquids.Count * liquidPieceScale.y, 0);

        SpriteRenderer sr = newLiquid.GetComponent<SpriteRenderer>();
        sr.color = color;

        liquids.Add(newLiquid);

        if (!instant)
            StartCoroutine(AnimateFill(newLiquid));
    }

    // Remove o líquido do topo do frasco
    public void RemoveLiquid()
    {
        if (liquids.Count == 0) return;

        GameObject topLiquid = liquids[liquids.Count - 1];
        liquids.RemoveAt(liquids.Count - 1);
        StartCoroutine(AnimatePour(topLiquid));
    }

    // Pega a cor do líquido do topo
    public Color GetTopColor()
    {
        if (liquids.Count == 0) return Color.clear;
        return liquids[liquids.Count - 1].GetComponent<SpriteRenderer>().color;
    }

    // Verifica se o frasco pode receber uma cor
    public bool CanReceive(Color color)
    {
        if (liquids.Count >= capacity) return false;
        if (liquids.Count == 0) return true;
        return GetTopColor() == color;
    }

    // Animação de preenchimento
    IEnumerator AnimateFill(GameObject liquid)
    {
        float targetY = liquids.Count * liquidPieceScale.y - liquidPieceScale.y;
        Vector3 startPos = liquid.transform.localPosition;
        Vector3 endPos = new Vector3(0, targetY, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fillSpeed;
            liquid.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        liquid.transform.localPosition = endPos;
    }

    // Animação de esvaziamento
    IEnumerator AnimatePour(GameObject liquid)
    {
        Vector3 startPos = liquid.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 1f, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * pourSpeed;
            liquid.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        Destroy(liquid);
    }

    // Seleção visual do frasco
    public void Select(bool select)
    {
        isSelected = select;
        // Pode implementar efeito visual, ex: mudar borda ou cor do frasco
    }

    public bool IsEmpty() => liquids.Count == 0;
    public bool IsFull() => liquids.Count >= capacity;
    public int Count() => liquids.Count;
}
