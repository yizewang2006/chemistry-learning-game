using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public List<Discovery> _discoveries = new List<Discovery>();
    public UnityEvent OnCraftingOpen;

    [Header("UI")]
    public GameObject craftingPanel;
    public List<DiscoveryHolder> recipes = new List<DiscoveryHolder>();
    [Header("Information UI")]
    public GameObject informationPanel;
    public Image iconUI;
    public TMP_Text titleUI;
    public TMP_Text descUI;
    public TMP_Text typeUI;
    public GameObject obtainableTag;
    public GameObject foundTag;
    public GameObject sellableTag;
    public GameObject createdTag;
    [Header("Page UI")]
    public int currentPage;
    public int slotsPerPage;
    public Transform recipePrefab;
    public Transform recipeContainer;
    public TMP_Text pageNumberText;

    [Header("Debug")]
    [ReadOnly] public bool craftingUIOpen;
    [ReadOnly] Discovery lastDiscovery;
    [ReadOnly] Discovery _currentDiscovery;
    [ReadOnly] int prevItemAmt;
    [SerializeField][ReadOnly] List<DiscoveryHolder> recipesToDisplay = new List<DiscoveryHolder>();

    public static CraftingManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OpenCrafting(false);
    }

    public bool AddNewRecipe(Discovery newDiscovery)
    {
        if (newDiscovery == null) return false;
        foreach (var item in _discoveries)
        {
            if (item.recipe == newDiscovery.recipe) return false;
        }
        if (newDiscovery.recipe == null) return false;
        _discoveries.Add(newDiscovery);
        return true;
    }

    void Update()
    {
        _currentDiscovery = DiscoveryManager.Instance._currentDiscovery;
        UpdateInformationUI();
        UpdateRecipeUI();


        craftingUIOpen = craftingPanel.activeSelf;
    }

    void UpdateRecipeUI()
    {
        PopulateRecipe();

        int startIndex = currentPage * slotsPerPage;
        recipesToDisplay = recipes.Skip(startIndex).Take(slotsPerPage).ToList();

        int totalPages = Mathf.CeilToInt((float)recipes.Count / slotsPerPage);
        string pageText = (currentPage + 1) + "/" + totalPages;
        if(pageText == "1/0") pageText = "0/0";
        pageNumberText.text = pageText;

        // Clamp pages
        if (currentPage + 1 >= totalPages) currentPage = totalPages - 1;
        if (currentPage <= 0) currentPage = 0;

        for (int i = 0; i < recipes.Count; i++)
            recipes[i].gameObject.SetActive(false);

        for (int i = 0; i < recipesToDisplay.Count; i++)
        {
            recipesToDisplay[i].gameObject.SetActive(true);
        }
    }
    // Rework for perfomance
    void PopulateRecipe()
    {
        if (prevItemAmt != _discoveries.Count)
        {
            prevItemAmt = _discoveries.Count;

            recipes.Clear();
            for (int i = recipeContainer.childCount - 1; i >= 0; i--)
                Destroy(recipeContainer.GetChild(i).gameObject);

            foreach (Discovery item in _discoveries)
            {
                Transform itemObj = Instantiate(recipePrefab, recipeContainer);

                if (!string.IsNullOrEmpty(item.title))
                {
                    itemObj.GetComponent<DiscoveryHolder>().discovery = item;
                    for (int i = 0; i < item.recipe.recipeRequirements.Count; i++)
                    {
                        itemObj.GetChild(1).GetChild(i).gameObject.SetActive(true);
                        itemObj.GetChild(1).GetChild(i).GetComponent<DiscoveryHolder>().discovery = item.recipe.recipeRequirements[i];
                        itemObj.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.recipe.recipeRequirements[i].icon;
                    }
                    for (int i = 0; i < item.recipe.recipeResults.Count; i++)
                    {
                        itemObj.GetChild(3).GetChild(i).gameObject.SetActive(true);
                        itemObj.GetChild(3).GetChild(i).GetComponent<DiscoveryHolder>().discovery = item.recipe.recipeResults[i];
                        itemObj.GetChild(3).GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.recipe.recipeResults[i].icon;
                    }
                    recipes.Add(itemObj.GetComponent<DiscoveryHolder>());
                }
            }
        }
    }

    void UpdateInformationUI()
    {
        informationPanel.SetActive(_currentDiscovery != null && _discoveries.Contains(_currentDiscovery));
        if (_currentDiscovery != null && _currentDiscovery != lastDiscovery)
        {
            lastDiscovery = _currentDiscovery;
            if (!_discoveries.Contains  (_currentDiscovery))
            {
                iconUI.sprite = _currentDiscovery.icon;
                titleUI.text = _currentDiscovery.title;
                typeUI.text = _currentDiscovery.discoveryType.ToString();
                descUI.text = "??? UNDISCOVERED ???";
                descUI.alignment = TextAlignmentOptions.Midline;
            }
            else
            {

                iconUI.sprite = _currentDiscovery.icon;
                titleUI.text = _currentDiscovery.title;
                typeUI.text = _currentDiscovery.discoveryType.ToString();
                descUI.text = _currentDiscovery.description;
                descUI.alignment = TextAlignmentOptions.MidlineLeft;
            }
            // TAGS
            if (_currentDiscovery.tagKeys != Discovery.TagKey.None)
            {
                obtainableTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Obtainable) != 0);
                foundTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Found) != 0);
                sellableTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Sellable) != 0);
                createdTag.SetActive((_currentDiscovery.tagKeys & Discovery.TagKey.Created) != 0);
            }
        }
    }

    public void OpenCrafting(bool active)
    {
        if (active)
            OnCraftingOpen?.Invoke();
        craftingPanel.SetActive(active);
    }

    public bool IsUIOpen() => craftingUIOpen;
}
