using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Discoverable : MonoBehaviour
{
    public bool discardTexture;
    Texture defaultText;
    Material defaultMat;
    bool targeted;

    public ScriptableObject objectScriptableObject; // description, buff, what does it do, etc. 

    void Start()
    {
        defaultMat = GetComponent<MeshRenderer>().sharedMaterial;
        defaultText = defaultMat.mainTexture;
        GetComponent<Outline>().enabled = !discardTexture;
    }

    void Update()
    {
        targeted = UniTool.Instance.currentDisc == this;

        if(!discardTexture)
        {
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
}
