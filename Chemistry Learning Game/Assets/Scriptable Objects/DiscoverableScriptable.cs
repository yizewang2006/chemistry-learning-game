using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Discoverable", menuName = "Discoverable")]
public class DiscoverableScriptable : ScriptableObject
{
    // Description
    public new string name;
    [TextArea(10,10)] public string description;
    public Sprite displayImage;

    public List<Reactions> canBeInvolvedIn;

    public GameObject modelPrefab;
}

public enum Reactions
{
    acidBase, oxidationReduction, carbonatedDrink, ammoniaSynthesis, ammoniumNitrateSynthesis
}
