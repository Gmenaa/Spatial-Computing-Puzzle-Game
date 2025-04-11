using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitializer : MonoBehaviour
{
    public GameObject[] gridObjects;  // array of 9 game objects in the grid

    void Start()
    {
        // initialize each grid object with position in the grid
        int index = 0;
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                ColorChanger colorChanger = gridObjects[index].GetComponent<ColorChanger>();
                colorChanger.Initialize(x, y);
                index++;
            }
        }
    }
}

