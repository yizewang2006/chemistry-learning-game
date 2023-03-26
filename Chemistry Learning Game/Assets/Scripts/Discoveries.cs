using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discoveries : MonoBehaviour
{
    public List<CompoundObjects> discoveredCompounds = new List<CompoundObjects>();
    public List<ReactionObjects> discoveredReactions = new List<ReactionObjects>();

    public static Discoveries Instance;

    void Awake()
    {
        Instance = this;
    }
}
