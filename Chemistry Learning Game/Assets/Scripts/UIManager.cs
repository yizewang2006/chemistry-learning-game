using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        uiActive = bookUI.activeInHierarchy;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
    }
}
