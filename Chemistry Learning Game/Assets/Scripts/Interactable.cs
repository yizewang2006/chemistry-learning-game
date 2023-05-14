using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    public string interactPrompt = "Use";
    public UnityEvent OnInteract;

    void Update()
    {
        GetComponent<Outline>().enabled = Interactor.Instance.currentInteractable == this;
    }
    
    public void Interact()
    {
        OnInteract?.Invoke();
    }
}
