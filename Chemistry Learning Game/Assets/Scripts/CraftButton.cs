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
        print("Success Craft");
    }

    public bool ProceedCraftCheck()
    {
        Dictionary<Discovery, int> requirementCounts = new Dictionary<Discovery, int>();

        // Count the occurrences of each requirement in the requirements list
        foreach (var req in requirements)
        {
            if (requirementCounts.ContainsKey(req))
                requirementCounts[req]++;
            else
                requirementCounts[req] = 1;
        }

        // Check if the requirements are met in the _items list
        foreach (var kvp in requirementCounts)
        {
            int requiredCount = kvp.Value;
            int itemCount = Inventory.Instance.GetItemCount(kvp.Key);

            if (requiredCount > itemCount)
                return false;
        }

        return true;
    }
}
