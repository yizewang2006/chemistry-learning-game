using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu()]
public class Discovery : ScriptableObject
{
    public enum DiscoveryType { ITEM, COMPOUND};
    public enum TagKey { None = 0, Obtainable = 1 << 0, Found = 1 << 1, Sellable = 1 << 2, Created = 1 << 3 }

    [ShowAssetPreview] public Sprite icon;
    public string title;
    public DiscoveryType discoveryType;
    [EnumFlags] public TagKey tagKeys;
    [ResizableTextArea] public string description;
    
    [HorizontalLine(2, EColor.Black)]
    [Expandable] public Recipe recipe;
}
