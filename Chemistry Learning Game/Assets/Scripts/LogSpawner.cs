using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public Transform prefab;

    public void SpawnLogs(Transform tree)
    {
        Instantiate(prefab, tree.position, Quaternion.identity);
    }
}
