using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BottleController bottlePrefab;     // Prefab do frasco
    public Transform layoutRoot;              // Raiz onde os frascos ficam organizados
    public int columns = 5;                   // Nº de colunas na grade
    public Vector2 spacing = new Vector2(1.6f, 2.6f);

    public int colorsCount = 6;               // Nº de cores diferentes
    public int capacity = 4;                  // Capacidade de cada frasco
    public int extraEmptyBottles = 2;         // Quantos frascos extras vazios
    public float pourSpeed = 0.08f;

    private List<BottleController> bottles = new List<BottleController>();
    private BottleController selected;
    private bool isBusy;

    void Start() => BuildLevel();

    void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider)
            {
                var bottle = hit.collider.GetComponent<BottleController>();
                if (bottle) HandleBottleClick(bottle);
            }
        }
    }

    // Cria todos os frascos e distribui cores
    void BuildLevel()
    {
        foreach (var b in bottles) Destroy(b.gameObject);
        bottles.Clear();

        int total = colorsCount + extraEmptyBottles;
        for (int i = 0; i < total; i++)
        {
            int r = i / columns;
            int c = i % columns;
            var pos = new Vector3(c * spacing.x, -r * spacing.y, 0);
            var b = Instantiate(bottlePrefab, pos, Quaternion.identity, layoutRoot);
            bottles.Add(b);
        }

        var palette = MakePalette(colorsCount);
        for (int i = 0; i < colorsCount; i++)
            bottles[i].FillWithColor(palette[i], capacity);

        Shuffle(100); // embaralha
    }

    // Lógica de clique
    void HandleBottleClick(BottleController bottle)
    {
        if (!selected)
        {
            selected = bottle;
            selected.SetHighlight(true);
        }
        else if (selected == bottle)
        {
            selected.SetHighlight(false);
            selected = null;
        }
        else
        {
            if (bottle.CanReceiveFrom(selected, out int maxPour))
                StartCoroutine(Pour(selected, bottle, maxPour));
            else
            {
                selected.SetHighlight(false);
                selected = bottle;
                selected.SetHighlight(true);
            }
        }
    }

    // Faz o despejo animado
    IEnumerator Pour(BottleController from, BottleController to, int amount)
    {
        isBusy = true;
        yield return StartCoroutine(from.PourTo(to, amount, pourSpeed));
        isBusy = false;

        selected.SetHighlight(false);
        selected = null;

        if (CheckWin())
            Debug.Log("🎉 Vitória!");
    }

    // Checa se o jogo terminou
    bool CheckWin()
    {
        foreach (var b in bottles)
        {
            if (b.IsEmpty) continue;
            if (!b.IsUniformFilled()) return false;
        }
        return true;
    }

    // Embaralha as cores
    void Shuffle(int steps)
    {
        var rnd = new System.Random();
        for (int s = 0; s < steps; s++)
        {
            var from = bottles[rnd.Next(bottles.Count)];
            var to = bottles[rnd.Next(bottles.Count)];
            if (from == to) continue;
            if (to.CanReceiveFrom(from, out int max) && max > 0)
                StartCoroutine(from.PourTo(to, Random.Range(1, max + 1), 0));
        }
    }

    // Cria paleta de cores HSV
    Color32[] MakePalette(int n)
    {
        var arr = new Color32[n];
        for (int i = 0; i < n; i++)
        {
            float h = i / (float)n;
            arr[i] = (Color)Color.HSVToRGB(h, 0.9f, 1f);
        }
        return arr;
    }
}
