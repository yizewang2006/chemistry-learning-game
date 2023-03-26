using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Compound", menuName = "Discoverable/New Compound")]
public class CompoundObjects : ScriptableObject, IDiscoverable
{
    // Description
    public string displayName;
    [TextArea(10,10)] public string description;
    public Sprite displayImage;

    public List<Reactions> canBeInvolvedIn;

    public GameObject modelPrefab;

    public string GetDescription()
    {
        return description;
    }
}

public enum Reactions
{
    acidBase, oxidationReduction, carbonatedDrink, ammoniaSynthesis, ammoniumNitrateSynthesis
}
