using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Discoverable : MonoBehaviour
{
    Texture defaultText;
    Material defaultMat;
    bool targeted;

    void Start()
    {
        defaultMat = GetComponent<MeshRenderer>().sharedMaterial;
        defaultText = defaultMat.mainTexture;
    }

    void Update()
    {
        targeted = UniTool.Instance.currentDisc == this;
        GetComponent<Outline>().enabled = targeted;
        if (targeted)
        {
            GetComponent<MeshRenderer>().sharedMaterial = UniTool.Instance.hologramMat;
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = defaultText;
        }
        else
        {
            GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
        }
    }
}
