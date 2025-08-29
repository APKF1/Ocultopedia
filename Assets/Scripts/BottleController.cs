using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [Range(1, 8)] public int capacity = 4;             // Capacidade máxima do frasco
    public Transform[] slots;                          // Posições onde o líquido fica (de baixo pra cima)
    public GameObject liquidPiecePrefab;               // Prefab do "LiquidPiece"
    public SpriteRenderer glassRenderer;               // SpriteRenderer do vidro

    private readonly List<Color32> layers = new List<Color32>();   // Cores atuais no frasco
    private readonly List<SpriteRenderer> pieces = new List<SpriteRenderer>(); // Instâncias visuais dos líquidos

    public int Count => layers.Count;
    public bool IsEmpty => Count == 0;
    public int FreeSpace => capacity - Count;
    public Color32 TopColor => IsEmpty ? new Color32(0, 0, 0, 0) : layers[layers.Count - 1];

    // Enche o frasco com uma cor (usado na geração inicial)
    public void FillWithColor(Color32 c, int amount)
    {
        for (int i = 0; i < amount && Count < capacity; i++)
            layers.Add(c);
        RefreshVisuals();
    }

    // Verifica se o frasco está cheio e só com uma cor
    public bool IsUniformFilled()
    {
        if (Count != capacity) return false;
        var c = layers[0];
        foreach (var col in layers)
            if (!SameColor(c, col)) return false;
        return true;
    }

    // Quantas peças consecutivas da mesma cor estão no topo
    public int TopRunLength()
    {
        if (IsEmpty) return 0;
        var top = TopColor;
        int run = 1;
        for (int i = Count - 2; i >= 0; i--)
        {
            if (SameColor(layers[i], top)) run++;
            else break;
        }
        return run;
    }

    // Checa se esse frasco pode receber líquido de outro
    public bool CanReceiveFrom(BottleController source, out int maxPour)
    {
        maxPour = 0;
        if (source == null || source == this || source.IsEmpty || FreeSpace == 0) return false;

        var movingColor = source.TopColor;
        int block = source.TopRunLength();

        if (IsEmpty)
        {
            maxPour = Mathf.Min(block, FreeSpace);
            return true;
        }

        if (SameColor(TopColor, movingColor))
        {
            maxPour = Mathf.Min(block, FreeSpace);
            return true;
        }

        return false;
    }

    // Faz a animação de despejar líquido em outro frasco
    public IEnumerator PourTo(BottleController target, int amount, float speed = 0.08f)
    {
        if (!target.CanReceiveFrom(this, out int maxPour)) yield break;
        amount = Mathf.Min(amount, maxPour);

        for (int i = 0; i < amount; i++)
        {
            var color = layers[layers.Count - 1];
            layers.RemoveAt(layers.Count - 1);

            // Cria uma peça temporária para animar o movimento
            var temp = Instantiate(liquidPiecePrefab, slots[Mathf.Max(0, Count)].position, Quaternion.identity);
            var sr = temp.GetComponent<SpriteRenderer>();
            sr.color = color;
            Vector3 targetPos = target.slots[target.Count].position;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 5f;
                temp.transform.position = Vector3.Lerp(temp.transform.position, targetPos, t);
                yield return null;
            }
            Destroy(temp);

            target.layers.Add(color);
            RefreshVisuals();
            target.RefreshVisuals();
            yield return new WaitForSeconds(speed);
        }
    }

    // Atualiza a aparência do frasco
    public void RefreshVisuals()
    {
        while (pieces.Count < layers.Count)
            pieces.Add(Instantiate(liquidPiecePrefab, transform).GetComponent<SpriteRenderer>());
        while (pieces.Count > layers.Count)
        {
            Destroy(pieces[pieces.Count - 1].gameObject);
            pieces.RemoveAt(pieces.Count - 1);
        }

        for (int i = 0; i < layers.Count; i++)
        {
            pieces[i].color = layers[i];
            pieces[i].transform.position = slots[i].position;
            pieces[i].sortingOrder = i;
        }
    }

    // Destaca o frasco quando selecionado
    public void SetHighlight(bool on)
    {
        if (glassRenderer)
            glassRenderer.color = on ? new Color(1, 1, 1, 0.6f) : new Color(1, 1, 1, 0.2f);
    }

    // Compara duas cores (ignora alpha)
    private static bool SameColor(Color32 a, Color32 b)
        => a.r == b.r && a.g == b.g && a.b == b.b;
}
