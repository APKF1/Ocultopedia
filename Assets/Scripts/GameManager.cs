using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BottleController bottlePrefab;
    public Transform layoutRoot;
    public int columns = 5;
    public Vector2 spacing = new Vector2(1.6f, 2.6f);

    public int colorsCount = 6;
    public int capacity = 4;
    public int extraEmptyBottles = 2;
    public float pourSpeed = 0.08f;

    private readonly List<BottleController> bottles = new();
    private BottleController selected;
    private bool isBusy;

    void Start() => BuildLevel();

    void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Physics2D.Raycast(worldPoint, Vector2.zero).collider?.GetComponent<BottleController>() is BottleController bottle)
                HandleBottleClick(bottle);
        }
    }

    void BuildLevel()
    {
        bottles.ForEach(b => Destroy(b.gameObject));
        bottles.Clear();

        int total = colorsCount + extraEmptyBottles;

        for (int i = 0; i < total; i++)
        {
            int r = i / columns;
            int c = i % columns;
            var pos = new Vector3(c * spacing.x, -r * spacing.y, 0);
            bottles.Add(Instantiate(bottlePrefab, pos, Quaternion.identity, layoutRoot));
        }

        var palette = MakePalette(colorsCount);

        for (int i = 0; i < colorsCount; i++)
            bottles[i].FillWithColor(palette[i], capacity);

        StartCoroutine(ShuffleRoutine(100));
    }

    void HandleBottleClick(BottleController bottle)
    {
        if (!selected) SelectBottle(bottle);
        else if (selected == bottle) DeselectBottle();
        else
        {
            if (bottle.CanReceiveFrom(selected, out int maxPour))
                StartCoroutine(Pour(selected, bottle, maxPour));
            else
            {
                DeselectBottle();
                SelectBottle(bottle);
            }
        }
    }

    void SelectBottle(BottleController bottle)
    {
        selected = bottle;
        selected.SetHighlight(true);
    }

    void DeselectBottle()
    {
        selected?.SetHighlight(false);
        selected = null;
    }

    IEnumerator Pour(BottleController from, BottleController to, int amount)
    {
        isBusy = true;
        yield return StartCoroutine(from.PourTo(to, amount, pourSpeed));
        isBusy = false;

        DeselectBottle();

        if (CheckWin()) Debug.Log("🎉 Vitória!");
    }

    bool CheckWin() => bottles.TrueForAll(b => b.IsEmpty || b.IsUniformFilled());

    IEnumerator ShuffleRoutine(int steps)
    {
        var rnd = new System.Random();

        for (int s = 0; s < steps; s++)
        {
            var from = bottles[rnd.Next(bottles.Count)];
            var to = bottles[rnd.Next(bottles.Count)];
            if (from == to) continue;

            if (to.CanReceiveFrom(from, out int max) && max > 0)
            {
                int amount = Random.Range(1, max + 1);
                yield return StartCoroutine(from.PourTo(to, amount, 0f));
            }
        }
    }

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
