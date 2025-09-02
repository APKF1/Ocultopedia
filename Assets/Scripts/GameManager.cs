using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BottleController bottlePrefab;
    public Transform layoutRoot;
    public int rows = 2;
    public int columns = 4;
    public List<Color> colors = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow };

    private BottleController selectedBottle;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateBottles();
    }

    void GenerateBottles()
    {
        List<Color> allColors = new List<Color>();
        for (int i = 0; i < colors.Count; i++)
        {
            for (int j = 0; j < 4; j++) // 4 líquidos por cor
            {
                allColors.Add(colors[i]);
            }
        }

        // Embaralha
        for (int i = 0; i < allColors.Count; i++)
        {
            Color temp = allColors[i];
            int randomIndex = Random.Range(i, allColors.Count);
            allColors[i] = allColors[randomIndex];
            allColors[randomIndex] = temp;
        }

        int bottleCount = rows * columns;
        for (int i = 0; i < bottleCount; i++)
        {
            BottleController bottle = Instantiate(bottlePrefab, layoutRoot);
            bottle.transform.localPosition = new Vector3((i % columns) * 2f, -(i / columns) * 3f, 0); // ajuste do espaçamento
            bottle.liquids = new List<Color>();

            for (int j = 0; j < 4; j++)
            {
                if (allColors.Count > 0)
                {
                    bottle.liquids.Add(allColors[0]);
                    allColors.RemoveAt(0);
                }
            }

            bottle.RenderLiquids();
        }
    }

    public void SelectBottle(BottleController bottle)
    {
        if (selectedBottle == null)
        {
            selectedBottle = bottle;
        }
        else
        {
            if (bottle != selectedBottle)
            {
                TransferLiquid(selectedBottle, bottle);
            }
            DeselectBottle();
        }
    }

    public void DeselectBottle()
    {
        selectedBottle = null;
    }

    void TransferLiquid(BottleController from, BottleController to)
    {
        if (from.IsEmpty()) return;
        Color topColor = from.liquids[from.liquids.Count - 1];
        if (to.CanReceive(topColor))
        {
            from.PourLiquid();
            to.ReceiveLiquid(topColor);
        }
    }
}
