using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniTool : MonoBehaviour
{
    public Discoverable currentDisc;
    public Camera cam;
    public float maxDistance;
    public float scanningSpeed;
    public LayerMask layerMask;
    public Material hologramMat;
    public GameObject scanningUI;
    public Image loadingBar;

    RaycastHit hit;

    public static UniTool Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, layerMask))
        {
            if(hit.transform.GetComponent<Discoverable>() != null)
            {
                currentDisc = hit.transform.GetComponent<Discoverable>();
                if(Input.GetMouseButton(0))
                {
                    scanningUI.SetActive(true);
                    loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1, Time.deltaTime * scanningSpeed);

                } else
                {
                    scanningUI.SetActive(false);
                    loadingBar.fillAmount = 0;
                }
            } else
            {
                scanningUI.SetActive(false);
                currentDisc = null;
                loadingBar.fillAmount = 0;
            }
        } else
        {
            scanningUI.SetActive(false);
            currentDisc = null;
            loadingBar.fillAmount = 0;
        }
    }
}
