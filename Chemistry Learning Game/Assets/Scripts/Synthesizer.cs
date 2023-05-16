using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    public void UseSynthesizer()
    {
        UIManager.Instance.CloseAllUI();
        CraftingManager.Instance.OpenCrafting(true);
    }
}
