using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Deafult Trees")]
    public Vector2 spawnSize;
    public float spawnHeightMin;
    public Vector2 spacingRangeDefault;

    [Space(20)]
    [Header("Dense Trees")]
    public Vector2 denseAreaCount;
    public Vector2 spacingRangeDense;
    public Vector2 denseSize;

    [Space(20)]
    [Header("Others")]
    public GameObject[] treePrefab;
    public LayerMask terrainLayer;
    public Transform treeSpawnParent;
    
    List<GameObject> spawnedTrees = new List<GameObject>();
    RaycastHit hit;

    public bool allSpawned;
    bool defaultSpawned;
    bool denseSpawned;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDefaultDense(new Vector2(0,0), spawnSize, spacingRangeDefault);

        for (int i = 0; i < Random.Range(denseAreaCount.x, denseAreaCount.y); i++) // spawn 1-3 dense areas
        {
            float x = Random.Range(spawnSize.x * 0.1f, spawnSize.x * 0.9f);
            float y = Random.Range(spawnSize.y * 0.1f, spawnSize.y * 0.9f);
            float sizeX = Random.Range(denseSize.x, denseSize.x);
            float sizeY = Random.Range(denseSize.y, denseSize.y);
            Vector2 pos = new Vector2(x, y);
            Vector2 size = new Vector2(sizeX, sizeY);
            SpawnDenseArea(pos, size, spacingRangeDense);
        }
    }

    void Update()
    {
        allSpawned = defaultSpawned && denseSpawned;
    }

    void SpawnDefaultDense(Vector2 startingPos, Vector2 endingPos, Vector2 spacing)
    {
        float randSpace = 1;
        for (float row = startingPos.x; row < endingPos.x; row+=randSpace)
        {
            for (float col = startingPos.y; col < endingPos.y; col+=randSpace)
            {
                randSpace = Random.Range(spacing.x, spacing.y);
                if(Physics.Raycast(new Vector3(row, 100, col), Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    if(hit.point.y >= spawnHeightMin)
                    {
                        if(Random.Range(0,2) == 0) continue;
                        GameObject tree = SpawnTree(hit.point, Quaternion.Euler(hit.normal));
                        spawnedTrees.Add(tree);
                    }
                }
                
            }
            randSpace = Random.Range(spacingRangeDefault.x, spacingRangeDefault.y);
        }
        
        defaultSpawned = true;
    }

    void SpawnDenseArea(Vector2 center, Vector2 size, Vector2 spacing)
    {
        for (float row = center.x - size.x/2; row < center.x + size.x/2; row+=spacing.x)
        {
            for (float col = center.y - size.y/2; col < center.y + size.y/2; col+=spacing.y)
            {
                if(Physics.Raycast(new Vector3(row, 100, col), Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    if(hit.point.y >= spawnHeightMin)
                    {
                        if(Random.Range(0,2) == 0) continue;
                        GameObject tree = SpawnTree(hit.point, Quaternion.Euler(hit.normal));
                        spawnedTrees.Add(tree);
                    }
                }
            }
        }

        denseSpawned = true;
    }

    GameObject  SpawnTree(Vector3 location, Quaternion rotation)
    {
        GameObject tree =  Instantiate(treePrefab[Random.Range(0, treePrefab.Length)], location, rotation, treeSpawnParent);
        tree.GetComponentInChildren<MeshRenderer>().gameObject.AddComponent(typeof(MeshCollider));
        tree.GetComponentInChildren<MeshRenderer>().gameObject.AddComponent(typeof(Discoverable));
        return tree;
    }

    public List<GameObject> GetSpawnedTrees()
    {
        return spawnedTrees;
    }
}
