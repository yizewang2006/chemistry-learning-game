using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Vector2 spawnSize;
    public float spawnHeightMin;
    public Vector2 spacingRangeDense;
    public Vector2 spacingRangeDefault;
    public Vector2 minDenseSize;
    public Vector2 maxDenseSize;
    public GameObject[] treePrefab;
    public LayerMask terrainLayer;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDefaultDense(new Vector2(0,0), spawnSize, spacingRangeDefault);
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
                        SpawnTree(hit.point, Quaternion.Euler(hit.normal));
                    }
                }
                
            }
            randSpace = Random.Range(spacingRangeDefault.x, spacingRangeDefault.y);
        }
        
    }

    void SpawnTree(Vector3 location, Quaternion rotation)
    {
        Instantiate(treePrefab[Random.Range(0, treePrefab.Length)], location, rotation);
    }
}
