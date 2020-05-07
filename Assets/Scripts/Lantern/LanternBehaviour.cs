using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehaviour
{
    private GhostColor?[] colors;

    public LanternBehaviour()
    {
        colors = new GhostColor?[2];
    }

    public void EmptyLantern()
    {
        colors = new GhostColor?[2];
    }

    public void StoreColor(GhostColor color)
    {
        if (!colors[0].HasValue)
        {
            colors[0] = color;
        }
        else if (colors[0].HasValue)
        {
            colors[1] = color;
        }
    }

    public void ShowColorsIn()
    {
            Debug.Log($"{colors[0]} and {colors[1]}");
    }
}
