using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool hasUIOpen;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        hasUIOpen = DiscoveryManager.Instance.bookOpen || Inventory.Instance.inventoryOpen || CraftingManager.Instance.craftingUIOpen;
    }

    public void CloseAllUI()
    {
        DiscoveryManager.Instance.OpenBook(false);
        Inventory.Instance.OpenInventory(false);
        CraftingManager.Instance.OpenCrafting(false);
    }
}
