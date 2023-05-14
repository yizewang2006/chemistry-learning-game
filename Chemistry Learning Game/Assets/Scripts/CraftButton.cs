using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftButton : MonoBehaviour
{
    public AudioClip OnSuccessCraft;
    public AudioClip OnFailCraft;
    List<Discovery> requirements = new List<Discovery>();

    void Start()
    {
        for (int i = 0; i < GetComponent<DiscoveryHolder>().discovery.recipe.recipeRequirements.Count; i++)
        {
            requirements.Add(GetComponent<DiscoveryHolder>().discovery.recipe.recipeRequirements[i]);
        }
    }

    public void Craft()
    {
        if (ProceedCraftCheck())
        {
            foreach (var req in requirements)
            {
                foreach (var slots in Inventory.Instance.slots)
                {
                    if (slots.discovery == req)
                    {
                        Inventory.Instance.DeleteItem(slots);
                        break;
                    }
                }
                continue;
            }

        }
        else
        {
            CraftingManager.Instance.GetComponent<AudioSource>().PlayOneShot(OnFailCraft);
            print("Fail Craft");
            return;
        }

        for (int i = 0; i < GetComponent<DiscoveryHolder>().discovery.recipe.recipeResults.Count; i++)
        {

            Inventory.Instance.AddToInventory(GetComponent<DiscoveryHolder>().discovery.recipe.recipeResults[i], 1);
        }
        CraftingManager.Instance.GetComponent<AudioSource>().PlayOneShot(OnSuccessCraft);
        print("Sucess Craft");
    }

    public bool ProceedCraftCheck()
    {
        foreach (var req in requirements)
        {

            if (!Inventory.Instance._items.Contains(req))
                return false;
        }
        return true;
    }
}
