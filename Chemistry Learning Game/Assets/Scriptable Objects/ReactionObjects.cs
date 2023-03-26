using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reaction", menuName = "Discoverable/New Reaction")]
public class ReactionObjects : ScriptableObject, IDiscoverable
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

