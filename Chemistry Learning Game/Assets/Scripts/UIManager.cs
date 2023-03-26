using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject bookUI;
    private bool uiActive;

    void ToggleUI()
    {
        uiActive = !uiActive;
        bookUI.SetActive(uiActive);
        FPS.allowLook = !uiActive;
    }

    private void Update()
    {
        uiActive = bookUI.activeInHierarchy;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
    }


}
