using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiscoveryManager : MonoBehaviour
{
    [Header("Main")]
    public UnityEvent OnBookOpen;
    public Discovery _currentDiscovery;
    public List<Discovery> _discoveries = new List<Discovery>();
    public List<GameObject> discoveryObjs = new List<GameObject>();
    public UnityEvent OnNewDiscovery;

    [Header("Popup UI")]
    public CanvasGroup popUpGroup;
    public Image popUpIcon;
    public TMP_Text popUpText;

    [Header("Page UI")]
    public int currentPage;
    public int discoveryPerPage;
    public Transform stickNotePrefab;
    public Transform discoveryContainer;
    public TMP_Text pageNumberText;
    [Header("Information UI")]
    public GameObject bookUI;
    public GameObject informationPage;
    public Image iconUI;
    public TMP_Text titleUI;
    public TMP_Text descUI;
    public TMP_Text typeUI;

    [Space(10)]
    public GameObject obtainableTag;
    public GameObject foundTag;
    public GameObject sellableTag;
    public GameObject createdTag;

    [Space(10)]
    public GameObject craftingUI;
    public Image machineIconUI;
    public Transform requireContainerUI;
    public Transform resultContainerUI;
    public Transform recipeButtonPrefab;
    public List<GameObject> recipeRequireButtons = new List<GameObject>();
    public List<GameObject> recipeResultsButtons = new List<GameObject>();

    [Header("Debug")]
    [ReadOnly] public bool bookOpen;
    [ReadOnly] Discovery lastDiscovery;
    [ReadOnly] int prevDiscoveredAmt;
    [SerializeField][ReadOnly] List<GameObject> itemsToDisplay = new List<GameObject>();

    public static DiscoveryManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        popUpGroup.alpha = 0;
        OpenBook(false);
    }

    void Update()
    {
        UpdateInformationUI();
        UpdatePageUI();

        bookOpen = bookUI.activeSelf;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenBook(!bookUI.activeSelf);
        }


        if (_currentDiscovery == null && _discoveries.Count > 0) _currentDiscovery = _discoveries[0];
    }

    public void AddNewDiscovery(Discovery disc)
    {
        if (!_discoveries.Contains(disc))
        {
            _discoveries.Add(disc);
            popUpIcon.sprite = disc.icon;
            popUpText.text = "Discovered " + disc.title + "!";
            CraftingManager.Instance.AddNewRecipe(disc);
            OnNewDiscovery?.Invoke();
            StartCoroutine(ShowPopUp());
        }
        else
            print("Already discovered " + disc.title + "!");
    }

    IEnumerator ShowPopUp()
    {
        while (popUpGroup.alpha != 1)
        {
            popUpGroup.alpha = Mathf.MoveTowards(popUpGroup.alpha, 1, Time.deltaTime * 1);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        while (popUpGroup.alpha != 0)
        {
            popUpGroup.alpha = Mathf.MoveTowards(popUpGroup.alpha, 0, Time.deltaTime * 1);
            yield return null;
        }
    }

    public void OpenBook(bool active)
    {
        if (active)
            OnBookOpen?.Invoke();
        bookUI.SetActive(active);
    }

    void UpdateInformationUI()
    {
        informationPage.SetActive(_currentDiscovery != null);
        if (_currentDiscovery != null && (_currentDiscovery != lastDiscovery || prevDiscoveredAmt != _discoveries.Count))
        {
            lastDiscovery = _currentDiscovery;  // Quick trick to make sure the UI updates only when the _currentDiscovery changes
            // Set basic UIs
            if (_discoveries.Contains(_currentDiscovery))
            {
                iconUI.sprite = _currentDiscovery.icon;
                titleUI.text = _currentDiscovery.title;
                typeUI.text = _currentDiscovery.discoveryType.ToString();
                descUI.text = _currentDiscovery.description;
                descUI.alignment = TextAlignmentOptions.MidlineLeft;
            }
            else
            {
                iconUI.sprite = _currentDiscovery.icon;
                titleUI.text = _currentDiscovery.title;
                typeUI.text = _currentDiscovery.discoveryType.ToString();
                descUI.text = "??? UNDISCOVERED ???";
                descUI.alignment = TextAlignmentOptions.Midline;
            }

            // TAGS
            if (_currentDiscovery.tagKeys != Discovery.TagKey.None)
            {
                obtainableTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Obtainable) != 0);
                foundTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Found) != 0);
                sellableTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Sellable) != 0);
                createdTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Created) != 0);
            }

            // CRAFTING
            // Set crafting preview active
            craftingUI.SetActive(_currentDiscovery.recipe != null && _discoveries.Contains(_currentDiscovery));
            if (_currentDiscovery.recipe != null)
            {
                machineIconUI.sprite = _currentDiscovery.recipe.craftingMachineIcon;

                // Disable all recipe buttons first
                for (int i = 0; i < 4; i++)
                {
                    recipeRequireButtons[i].SetActive(false);
                    recipeResultsButtons[i].SetActive(false);
                }

                // Then activate based on the amount of recipe
                for (int i = 0; i < _currentDiscovery.recipe.recipeRequirements.Count; i++)
                {
                    recipeRequireButtons[i].SetActive(true);
                    recipeRequireButtons[i].transform.GetChild(0).GetComponent<Image>().sprite
                    = _currentDiscovery.recipe.recipeRequirements[i].icon;
                    recipeRequireButtons[i].GetComponent<DiscoveryHolder>().discovery = _currentDiscovery.recipe.recipeRequirements[i];

                    if (_discoveries.Contains(_currentDiscovery.recipe.recipeRequirements[i]))
                        recipeRequireButtons[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    else
                        recipeRequireButtons[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
                }
                for (int i = 0; i < _currentDiscovery.recipe.recipeResults.Count; i++)
                {
                    recipeResultsButtons[i].SetActive(true);
                    recipeResultsButtons[i].transform.GetChild(0).GetComponent<Image>().sprite
                    = _currentDiscovery.recipe.recipeResults[i].icon;
                    recipeResultsButtons[i].GetComponent<DiscoveryHolder>().discovery = _currentDiscovery.recipe.recipeResults[i];

                    if (_discoveries.Contains(_currentDiscovery.recipe.recipeResults[i]))
                        recipeResultsButtons[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    else
                        recipeResultsButtons[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
                }
            }
        }
    }

    void UpdatePageUI()
    {
        PopulateDiscoveries();

        int startIndex = currentPage * discoveryPerPage;
        itemsToDisplay = discoveryObjs.Skip(startIndex).Take(discoveryPerPage).ToList();

        int totalPages = Mathf.CeilToInt((float)discoveryObjs.Count / discoveryPerPage);
        string pageText = "Page " + (currentPage + 1) + "/" + totalPages;
        if(pageText == "Page 1/0") pageText = "Page 0/0";
        pageNumberText.text = pageText;

        // Clamp pages
        if (currentPage + 1 >= totalPages) currentPage = totalPages - 1;
        if (currentPage <= 0) currentPage = 0;

        for (int i = 0; i < discoveryObjs.Count; i++)
            discoveryObjs[i].SetActive(false);

        for (int i = 0; i < itemsToDisplay.Count; i++)
        {
            itemsToDisplay[i].SetActive(true);
        }
    }

    // Rework for perfomance
    void PopulateDiscoveries()
    {
        if (prevDiscoveredAmt != _discoveries.Count)
        {
            prevDiscoveredAmt = _discoveries.Count;

            discoveryObjs.Clear();
            for (int i = discoveryContainer.childCount - 1; i >= 0; i--)
                Destroy(discoveryContainer.GetChild(i).gameObject);

            foreach (Discovery item in _discoveries)
            {
                Transform itemObj = Instantiate(stickNotePrefab, discoveryContainer);

                if (!string.IsNullOrEmpty(item.title))
                {
                    itemObj.GetChild(0).GetComponent<TMP_Text>().text = item.title;
                    itemObj.GetChild(1).GetComponent<Image>().sprite = item.icon;

                    itemObj.GetComponent<DiscoveryHolder>().discovery = item;
                    discoveryObjs.Add(itemObj.gameObject);
                }
            }
        }
    }

    public void ChangePage(int amt)
    {
        currentPage += amt;
    }

    public bool IsUIOpen() => bookOpen;
}
