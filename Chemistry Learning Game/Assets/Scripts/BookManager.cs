using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public Button startingTab;
    public TMP_Text descriptionText;
    public GameObject compoundPage;
    public GameObject reactionPage;
    public Transform[] compStickyNotes;
    public Transform[] reactStickyNotes;

    public static BookManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OpenTab(0);
        startingTab.Select();
    }

    void Update()
    {
        ResetDiscoveries();
        PopulateDiscoveries();
    }

    void PopulateDiscoveries()
    {
        for (int i = 0; i < Discoveries.Instance.discoveredCompounds.Count; i++)
        {
            // If the # of discovered compounds is greater than the # of sticky notes, break the loop
            if(i+1 > compStickyNotes.Length) break;

            compStickyNotes[i].GetComponent<DiscoverableHolder>().currentDiscoverable = Discoveries.Instance.discoveredCompounds[i];

            // Set sticky notes question marks off and icons/name on
            compStickyNotes[i].GetChild(0).gameObject.SetActive(false);
            compStickyNotes[i].GetChild(1).gameObject.SetActive(true);
            compStickyNotes[i].GetChild(2).gameObject.SetActive(true);

            compStickyNotes[i].GetChild(1).GetComponent<Image>().sprite = Discoveries.Instance.discoveredCompounds[i].displayImage;
            compStickyNotes[i].GetChild(2).GetComponent<TMP_Text>().text = Discoveries.Instance.discoveredCompounds[i].name;
        }

        for (int i = 0; i < Discoveries.Instance.discoveredReactions.Count; i++)
        {
            // If the # of discovered reactions is greater than the # of sticky notes, break the loop
            if(i+1 > reactStickyNotes.Length) break;

            reactStickyNotes[i].GetComponent<DiscoverableHolder>().currentDiscoverable = Discoveries.Instance.discoveredReactions[i];

            // Set sticky notes question marks off and icons/name on
            reactStickyNotes[i].GetChild(0).gameObject.SetActive(false);
            reactStickyNotes[i].GetChild(1).gameObject.SetActive(true);
            reactStickyNotes[i].GetChild(2).gameObject.SetActive(true);

            reactStickyNotes[i].GetChild(1).GetComponent<Image>().sprite = Discoveries.Instance.discoveredReactions[i].displayImage;
            reactStickyNotes[i].GetChild(2).GetComponent<TMP_Text>().text = Discoveries.Instance.discoveredReactions[i].name;
        }
    }

    // Set sticky notes question marks on and icons/name off
    void ResetDiscoveries()
    {
        for (int i = 0; i < compStickyNotes.Length; i++)
        {
            compStickyNotes[i].GetChild(0).gameObject.SetActive(true);
            compStickyNotes[i].GetChild(1).gameObject.SetActive(false);
            compStickyNotes[i].GetChild(2).gameObject.SetActive(false);
        }
        for (int i = 0; i < reactStickyNotes.Length; i++)
        {
            reactStickyNotes[i].GetChild(0).gameObject.SetActive(true);
            reactStickyNotes[i].GetChild(1).gameObject.SetActive(false);
            reactStickyNotes[i].GetChild(2).gameObject.SetActive(false);
        }
    }

    public void OpenTab(int tab)
    {
        compoundPage.SetActive(tab == 0);
        reactionPage.SetActive(tab == 1);
    }

}
