using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float time = 5;
    void Start()
    {
        Invoke("DestroySelf", time);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
