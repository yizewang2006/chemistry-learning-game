using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public UnityEvent OnInteract;
    [ReadOnly] public Interactable currentInteractable;
    public Camera cam;
    public float maxDistance;
    public KeyCode interactKey;
    public TMP_Text interactText;

    RaycastHit hit;


    public static Interactor Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance))
        {
            if (hit.transform.GetComponent<Interactable>() != null)
                currentInteractable = hit.transform.GetComponent<Interactable>();
            else
                currentInteractable = null;
        }
        else currentInteractable = null;

        interactText.gameObject.SetActive(currentInteractable != null);
        if (currentInteractable != null)
        {
            interactText.text = interactKey.ToString() + " - " + currentInteractable.interactPrompt;

            if (Input.GetKeyDown(interactKey))
            {
                OnInteract?.Invoke();
                currentInteractable.Interact();
            }
        }
    }
}
