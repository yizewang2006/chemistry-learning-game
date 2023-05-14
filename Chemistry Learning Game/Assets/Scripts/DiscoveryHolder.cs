using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryHolder : MonoBehaviour
{
    public Discovery discovery;
    public void ChangeToThisDiscovery()
    {
        if(discovery != null && DiscoveryManager.Instance != null)
        {
            DiscoveryManager.Instance._currentDiscovery = discovery;
        }
    }
}
