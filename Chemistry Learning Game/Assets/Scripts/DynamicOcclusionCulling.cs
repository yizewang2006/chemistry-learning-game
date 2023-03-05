using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOcclusionCulling : MonoBehaviour
{
    public float cullDistance = 50f;
    public float enableRadius;
    public TreeSpawner treeSpawner;

    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("No camera found in the scene!");
        }
    }

    void Update()
    {
        // Test each spawned tree against the camera's view frustum
        foreach (var tree in treeSpawner.GetSpawnedTrees())
        {
            Renderer renderer = tree.GetComponent<Renderer>();
            if (renderer != null)
            {
                // If the tree is within the cull distance and visible to the camera, enable its renderer
                if (Vector3.Distance(renderer.bounds.center, mainCamera.transform.position) <= cullDistance && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), renderer.bounds))
                {
                    renderer.enabled = true;
                }
                // Otherwise, disable its renderer
                else
                {
                    if(Vector3.Distance(renderer.bounds.center, mainCamera.transform.position) <= enableRadius)
                        renderer.enabled = true;
                    else
                        renderer.enabled = false;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mainCamera.transform.position, cullDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mainCamera.transform.position, enableRadius);
    }
}
