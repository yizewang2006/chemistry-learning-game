using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeButton : MonoBehaviour
{
    public AudioClip OnSuccessTrade;
    public AudioClip OnFailTrade;
    List<Discovery> requirements = new List<Discovery>();

    void Start()
    {
        for (int i = 0; i < GetComponent<TradeHolder>().trade.requirements.Count; i++)
        {
            requirements.Add(GetComponent<TradeHolder>().trade.requirements[i]);
        }
    }

    public void Trade()
    {
        if (ProceedTradeCheck())
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
            TradesManager.Instance.GetComponent<AudioSource>().PlayOneShot(OnFailTrade);
            print("Fail Trade");
            return;
        }

        for (int i = 0; i < GetComponent<TradeHolder>().trade.results.Count; i++)
        {
            Inventory.Instance.AddToInventory(GetComponent<TradeHolder>().trade.results[i], 1);
        }
        TradesManager.Instance.GetComponent<AudioSource>().PlayOneShot(OnSuccessTrade);
        print("Success Trade");
    }

    public bool ProceedTradeCheck()
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
