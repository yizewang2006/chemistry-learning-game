using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu()]
public class Trade : ScriptableObject
{
    public List<Discovery> requirements = new List<Discovery>();
    public List<Discovery> results = new List<Discovery>();
}