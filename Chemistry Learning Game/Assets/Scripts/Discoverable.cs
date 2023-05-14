using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline)), RequireComponent(typeof(DiscoveryHolder))]
public class Discoverable : MonoBehaviour
{
    public bool discardTexture;
    public bool collectable;
    [ShowIf("collectable")] public bool infiniteHealth;
    [ShowIf("NotInifiniteHP")] public float health = 100;
    [ShowIf("collectable")] public int itemAmount;
    [ShowIf("collectable")] public bool destroyable;
    [ShowIf("collectable")] public UnityEvent OnDestroy;
    Texture defaultText;
    Material defaultMat;
    bool targeted;
    float startingHealth;

    bool NotInifiniteHP() => !infiniteHealth;

    void Start()
    {
        defaultMat = GetComponent<MeshRenderer>().sharedMaterial;
        defaultText = defaultMat.mainTexture;
        GetComponent<Outline>().enabled = !discardTexture;

        startingHealth = health;
    }

    void Update()
    {
        targeted = UniTool.Instance.currentDisc == this;

        MaterialUpdate();
    }

    void MaterialUpdate()
    {
        if (!discardTexture && UniTool.Instance.mode == UniTool.Mode.SCAN)
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
        else if (!discardTexture && UniTool.Instance.mode == UniTool.Mode.LASER)
        {
            GetComponent<Outline>().enabled = false;
            if (targeted && UniTool.Instance.shootingLaser)
            {
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_HoloDistance", Mathf.InverseLerp(startingHealth, 0, health));
                GetComponent<MeshRenderer>().sharedMaterial = UniTool.Instance.collectMat;
                GetComponent<MeshRenderer>().sharedMaterial.mainTexture = defaultText;
            }
            else
            {
                GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
            }
        }
        else
        {
            GetComponent<Outline>().enabled = false;
            GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
        }
    }

    public void ApplyDamage(float amount)
    {
        if (collectable)
        {
            if (NotInifiniteHP())
            {
                health -= amount;
                if (health <= 0)
                {
                    health = 0;
                    Inventory.Instance.AddToInventory(GetComponent<DiscoveryHolder>().discovery, itemAmount);

                    if (destroyable)
                    {
                        OnDestroy?.Invoke();
                        Destroy(gameObject);
                    }
                }
            } else
            {
                Inventory.Instance.AddToInventory(GetComponent<DiscoveryHolder>().discovery, 1);
            }
        }
    }
}
