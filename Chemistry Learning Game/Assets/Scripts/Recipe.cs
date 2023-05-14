using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public Sprite craftingMachineIcon;
    public List<Discovery> recipeRequirements = new List<Discovery>();
    public List<Discovery> recipeResults = new List<Discovery>();
}
