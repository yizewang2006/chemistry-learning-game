using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    public bool deleteWithNoChild = true;

    void Update()
    {
        if(transform.childCount == 0 && deleteWithNoChild) Destroy(gameObject);
    }
}
