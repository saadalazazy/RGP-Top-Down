using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationGeneration : MonoBehaviour
{
    private int roomHeight;
    private int roomWidth;

    private Vector2 tileSize;
    private Vector2 wallSize;
    private Vector2 tileOffset;
    public GameObject floorFoundation;

    void CreatFoundation()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            Vector3 pos = new Vector3(tileOffset.x, -2, -y * tileSize.y + tileOffset.y);
            Instantiate(floorFoundation, pos, Quaternion.identity, transform);
        }
        for (int x = 1; x < roomHeight; x++)
        {
            Vector3 pos = new Vector3(x * tileSize.x + tileOffset.x, -2, -(roomWidth - 1) * tileSize.y + tileOffset.y);
            Instantiate(floorFoundation, pos, Quaternion.identity, transform);
        }
    }
}
