using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverableHolder : MonoBehaviour
{
    public ScriptableObject currentDiscoverable;
    IDiscoverable currDiscoverable;

    public void ShowDescription()
    {
        if(currentDiscoverable != null)
        {
            currDiscoverable = (IDiscoverable)currentDiscoverable;
            BookManager.Instance.descriptionText.text = currDiscoverable.GetDescription();
            if(currDiscoverable.GetDescription() == "" || currDiscoverable.GetDescription() == null)
                BookManager.Instance.descriptionText.text = "???";
        }
    }
}

interface IDiscoverable
{
    string GetDescription();
}
