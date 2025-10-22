using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] int gridX;
    [SerializeField] int GridY;

    [SerializeField] float tileSize = 1f;
    [SerializeField] float foundationSize = 2f; 
    [SerializeField] Vector2 wallSize = new Vector2(1f, 1f);
    [SerializeField] Vector2 tileOffset;
    [SerializeField] Vector2 wallOffset;
    [SerializeField] Vector2Int doorPos;
    [SerializeField] int floorCount = 1;

    [SerializeField] bool makeFloor;
    [SerializeField] bool makeWall;
    [SerializeField] bool makeFoundation;
    [SerializeField] bool notRotation;

    public List<GameObject> tiles;
    public List<GameObject> walls;
    public List<GameObject> wallCorners;
    public List<GameObject> wallHalves;
    public List<GameObject> doors;
    public GameObject floorFoundation;

    private Transform floorParent;
    private Transform wallsParent;
    private Transform foundationParent;


    public void GenerateRoom()
    {
        ClearExistingRoom();
        CreateParentContainers();
        ClampDoorPosition();
        if (makeFloor) CreateFloor();
        if (makeWall) CreateWalls();
        if (makeFoundation) CreateFoundation();
    }

    void ClampDoorPosition()
    {
        doorPos.x = Mathf.Clamp(doorPos.x, 0, Mathf.FloorToInt(gridX / 2f) - 1);
        doorPos.y = Mathf.Clamp(doorPos.y, 0, Mathf.FloorToInt(GridY / 2f) - 1);
    }

    void CreateParentContainers()
    {
        if (makeFloor)
        {
            floorParent = new GameObject("Floor").transform;
            floorParent.SetParent(transform);
            floorParent.localPosition = Vector3.zero;
        }
        if (makeWall)
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
        for (int y = 0; y < GridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 localPos = new Vector3(x * tileSize + tileOffset.x, 0, -y * tileSize + tileOffset.y);
                Vector3 pos = transform.TransformPoint(localPos);
                Quaternion rot = !notRotation ? transform.rotation * GetRandomRotation() : Quaternion.identity;
                GameObject prefab = GetRandomPrefab(tiles);
                if (prefab != null)
                    Instantiate(prefab, pos, rot, floorParent).name = $"Tile_{x}_{y}";
            }
        }
    }

    private Quaternion GetRandomRotation()
    {
        int i = Random.Range(0, 4);
        return Quaternion.Euler(0, i * 90, 0);
    }

    void CreateWalls()
    {
        if (walls == null || walls.Count == 0)
        {
            Debug.LogError("No wall prefabs assigned");
            return;
        }

        if (wallsParent == null)
        {
            Debug.LogWarning("No walls parent assigned, using scene root");
        }

        var gridDimensions = new Vector2(gridX * tileSize, GridY * tileSize);
        var wallCounts = CalculateWallCounts(gridDimensions);

        if (wallCounts.x <= 0 || wallCounts.y <= 0)
        {
            Debug.LogError("Invalid wall counts calculated");
            return;
        }

        CreateWallStructure(wallCounts);
    }

    Vector2Int CalculateWallCounts(Vector2 gridDimensions)
    {
        int xWalls = Mathf.FloorToInt(gridDimensions.x / wallSize.x);
        int yWalls = Mathf.FloorToInt(gridDimensions.y / wallSize.y);

        return new Vector2Int(xWalls, yWalls);
    }

    void CreateWallStructure(Vector2Int wallCounts)
    {
        Transform cachedTransform = transform;
        Quaternion baseRotation = cachedTransform.rotation;

        for (int floor = 0; floor < floorCount; floor++)
        {
            float y = floor * wallSize.y;
            Vector3 position = new Vector3(wallOffset.x, y, 0);
            SpawnWall(cachedTransform.TransformPoint(position), baseRotation, wallHalves);
            for (int x = 1; x < wallCounts.x; x++)
            {
                position = new Vector3(x * wallSize.x + wallOffset.x, y, 0);
                if(doorPos.x == x && floor == 0)
                {
                    SpawnWall(cachedTransform.TransformPoint(position), baseRotation, doors);
                }
                else
                {
                    SpawnWall(cachedTransform.TransformPoint(position), baseRotation, walls);
                }
            }
            position = new Vector3(wallCounts.x * wallSize.x + wallOffset.x, y, 0);
            SpawnWall(cachedTransform.TransformPoint(position), baseRotation*Quaternion.Euler(0 , 180 , 0), wallCorners);
            for (int z = 1; z < wallCounts.y; z++)
            {
                position = new Vector3(wallCounts.x * wallSize.x, y, -z * wallSize.y + wallOffset.y);
                SpawnWall(cachedTransform.TransformPoint(position), baseRotation * Quaternion.Euler(0, 90, 0) , walls);
                if (doorPos.y == y && floor == 0)
                {
                    SpawnWall(cachedTransform.TransformPoint(position), baseRotation * Quaternion.Euler(0, 90, 0), doors);
                }
                else
                {
                    SpawnWall(cachedTransform.TransformPoint(position), baseRotation * Quaternion.Euler(0, 90, 0), walls);
                }
            }
            position = new Vector3(wallCounts.x * wallSize.x, y, -wallCounts.y * wallSize.y + wallOffset.y);
            SpawnWall(cachedTransform.TransformPoint(position), baseRotation * Quaternion.Euler(0, -90, 0), wallHalves);
        }
    }
    
    void SpawnWall(Vector3 position, Quaternion rotation , List<GameObject> wall)
    {
        GameObject prefab = GetRandomPrefab(wall);
        if (prefab != null)
        {
            Instantiate(prefab, position, rotation, wallsParent);
        }
    }


    void CreateFoundation()
    { 
        for (int y = 0; y < GridY; y++)
        {
            Vector3 localPos = new Vector3(tileOffset.x, -2f, -y * tileSize + tileOffset.y);
            Vector3 pos = transform.TransformPoint(localPos);
            if (floorFoundation != null)
                Instantiate(floorFoundation, pos, transform.rotation, foundationParent);
        }

        for (int x = 1; x < gridX; x++)
        {
            Vector3 localPos = new Vector3(x * tileSize + tileOffset.x, -2f, -(GridY - 1) * tileSize + tileOffset.y);
            Vector3 pos = transform.TransformPoint(localPos);
            if (floorFoundation != null)
                Instantiate(floorFoundation, pos, transform.rotation, foundationParent);
        }
    }

    GameObject GetRandomPrefab(List<GameObject> list)
    {
        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }
}
