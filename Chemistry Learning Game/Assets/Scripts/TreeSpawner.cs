using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootRaycast();
    }
    void ShootRaycast()
    {
        Vector3 direction;

        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawLine(transform.position, Vector3.down, color: Color.red);

        RaycastHit targetPoint;
        Physics.Raycast(ray, out targetPoint);

        Debug.Log(targetPoint.point);
    }

    void SpawnTree(Vector3 location, Quaternion rotation)
    {
        Instantiate(treePrefab, position: location, rotation: rotation);
    }
}
