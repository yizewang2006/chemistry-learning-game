using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public void Delete()
    {
        Inventory.Instance.DeleteItem(GetComponent<DiscoveryHolder>());
    }
}
