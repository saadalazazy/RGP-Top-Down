using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] int roomHeight;
    [SerializeField] int roomWidth;

    [Header("Room Settings")]
    [SerializeField] Vector2 tileSize;
    [SerializeField] Vector2 wallSize;
    [SerializeField] Vector2 tileOffset;
    [SerializeField] int doorPosX;
    [SerializeField] int doorPosY;
    [SerializeField] int floorCount;
    [SerializeField] bool makeFloor;
    [SerializeField] bool makeWall;
    [SerializeField] bool makeFoundation;

    [Header("Prefab Lists (Randomized)")]
    public List<GameObject> tiles;
    public List<GameObject> walls;
    public List<GameObject> firstWalls;
    public List<GameObject> wallCorners;
    public List<GameObject> wallHalves;
    public List<GameObject> doors;

    [Header("Foundation Prefab")]
    public GameObject floorFoundation;

    private Transform floorParent;
    private Transform wallsParent;
    private Transform foundationParent;

    public void GenerateRoom()
    {
        ClearExistingRoom();
        CreateParentContainers();
        if(makeFloor)
            CreateFloor();
        if(makeWall)
            CreateWalls();
        if(makeFoundation)
            CreateFoundation();
    }

    void CreateParentContainers()
    {
        if(makeFloor)
        {
            floorParent = new GameObject("Floor").transform;
            floorParent.SetParent(transform);
            floorParent.localPosition = Vector3.zero;
        }
        if(makeWall) 
        {
            wallsParent = new GameObject("Walls").transform;
            wallsParent.SetParent(transform);
            wallsParent.localPosition = Vector3.zero;
        }
        if (makeFoundation)
        {
            foundationParent = new GameObject("Foundation").transform;
            foundationParent.SetParent(transform);
            foundationParent.localPosition = Vector3.zero;
        }        
    }

    void ClearExistingRoom()
    {
        DestroyIfExists("Floor");
        DestroyIfExists("Walls");
        DestroyIfExists("Foundation");
    }

    void DestroyIfExists(string name)
    {
        Transform child = transform.Find(name);
        if (child != null) DestroyImmediate(child.gameObject);
    }

    void CreateFloor()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                Vector3 pos = new Vector3(x * tileSize.x + tileOffset.x, 0, -y * tileSize.y + tileOffset.y);
                Quaternion rot = GetRandomRotation();
                Instantiate(GetRandomPrefab(tiles), pos, rot, floorParent).name = $"Tile_{x}_{y}";
            }
        }
    }

    private Quaternion GetRandomRotation()
    {
        int i = Random.Range(0, 5);
        if(i == 0)
        {
            return Quaternion.identity;
        }
        else if(i == 1)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else if (i == 2)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        else
        {
            return Quaternion.Euler(0, 270, 0);
        }

        return Quaternion.identity;
    }

    void CreateWalls()
    {
        for (int j = 0; j < floorCount; j++)
        {
            Vector3 pos = new Vector3(0 * wallSize.x, j * wallSize.y, 0);
            Instantiate(GetRandomPrefab(firstWalls), pos, Quaternion.identity, wallsParent);

            for (int i = 1; i < (roomHeight / 2); i++)
            {
                pos = new Vector3(i * wallSize.x, j * wallSize.y, 0);
                GameObject prefab = (doorPosX != 0 && i == doorPosX) ? GetRandomPrefab(doors) : GetRandomPrefab(walls);
                Instantiate(prefab, pos, Quaternion.identity, wallsParent);
            }

            pos = new Vector3((roomHeight / 2) * wallSize.x, j * wallSize.y, 0);
            Quaternion rot = Quaternion.Euler(0, 180, 0);
            Instantiate(GetRandomPrefab(wallCorners), pos, rot, wallsParent);

            for (int i = 1; i < roomWidth / 2; i++)
            {
                pos = new Vector3((roomHeight / 2) * wallSize.x, j * wallSize.y, -i * wallSize.x);
                rot = Quaternion.Euler(0, 90, 0);
                GameObject prefab = (doorPosY != 0 && i == doorPosY) ? GetRandomPrefab(doors) : GetRandomPrefab(walls);
                Instantiate(prefab, pos, rot, wallsParent);
            }

            pos = new Vector3((roomHeight / 2) * wallSize.x, j * wallSize.y, -(roomWidth / 2) * wallSize.y);
            rot = Quaternion.Euler(0, -90, 0);
            Instantiate(GetRandomPrefab(wallHalves), pos, rot, wallsParent);
        }
    }

    void CreateFoundation()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            Vector3 pos = new Vector3(tileOffset.x, -2, -y * tileSize.y + tileOffset.y);
            Instantiate(floorFoundation, pos, Quaternion.identity, foundationParent);
        }

        for (int x = 1; x < roomHeight; x++)
        {
            Vector3 pos = new Vector3(x * tileSize.x + tileOffset.x, -2, -(roomWidth - 1) * tileSize.y + tileOffset.y);
            Instantiate(floorFoundation, pos, Quaternion.identity, foundationParent);
        }
    }

    GameObject GetRandomPrefab(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("Prefab list is empty!");
            return null;
        }
        return list[Random.Range(0, list.Count)];
    }
}
