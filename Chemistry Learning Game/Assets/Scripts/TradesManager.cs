using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TradesManager : MonoBehaviour
{
    public List<Trade> _currentTrades = new List<Trade>();
    public List<Trade> _allTrades = new List<Trade>();
    public UnityEvent OnTradeOpen;

    [Header("UI")]
    public GameObject tradePanel;
    public List<TradeHolder> tradeUIs = new List<TradeHolder>();
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
    public Transform tradeUIPrefab;
    public Transform tradeUIContainer;
    public TMP_Text pageNumberText;

    [Header("Debug")]
    [ReadOnly] public bool tradeUIOpen;
    [ReadOnly] Discovery lastDiscovery;
    [ReadOnly] Discovery _currentDiscovery;
    [ReadOnly] int prevItemAmt;
    [SerializeField][ReadOnly] List<TradeHolder> tradesToDisplay = new List<TradeHolder>();

    public static TradesManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OpenTrade(false);
    }

    public bool AddNewTrade(Trade newTrade)
    {
        if (newTrade == null) return false;
        if(_currentTrades.Contains(newTrade)) return false;
        _currentTrades.Add(newTrade);
        return true;
    }

    void Update()
    {
        _currentDiscovery = DiscoveryManager.Instance._currentDiscovery;
        UpdateInformationUI();
        UpdateRecipeUI();

        tradeUIOpen = tradePanel.activeSelf;

        _currentTrades = _allTrades.Where(trade => trade.requirements.All(requirement => DiscoveryManager.Instance._discoveries.Contains(requirement))).ToList();
    }

    void UpdateRecipeUI()
    {
        PopulateTradeUIs();

        int startIndex = currentPage * slotsPerPage;
        tradesToDisplay = tradeUIs.Skip(startIndex).Take(slotsPerPage).ToList();

        int totalPages = Mathf.CeilToInt((float)tradeUIs.Count / slotsPerPage);
        string pageText = (currentPage + 1) + "/" + totalPages;
        if(pageText == "1/0") pageText = "0/0";
        pageNumberText.text = pageText;

        // Clamp pages
        if (currentPage + 1 >= totalPages) currentPage = totalPages - 1;
        if (currentPage <= 0) currentPage = 0;

        for (int i = 0; i < tradeUIs.Count; i++)
            tradeUIs[i].gameObject.SetActive(false);

        for (int i = 0; i < tradesToDisplay.Count; i++)
        {
            tradesToDisplay[i].gameObject.SetActive(true);
        }
    }
    // Rework for perfomance
    void PopulateTradeUIs()
    {
        if (prevItemAmt != _currentTrades.Count)
        {
            prevItemAmt = _currentTrades.Count;

            tradeUIs.Clear();
            for (int i = tradeUIContainer.childCount - 1; i >= 0; i--)
                Destroy(tradeUIContainer.GetChild(i).gameObject);

            foreach (Trade item in _currentTrades)
            {
                Transform itemObj = Instantiate(tradeUIPrefab, tradeUIContainer);

                if (item.requirements.Count > 0 && item.results.Count > 0)
                {
                    itemObj.GetComponent<TradeHolder>().trade = item;
                    for (int i = 0; i < item.requirements.Count; i++)
                    {
                        itemObj.GetChild(1).GetChild(i).gameObject.SetActive(true);
                        itemObj.GetChild(1).GetChild(i).GetComponent<DiscoveryHolder>().discovery = item.requirements[i];
                        itemObj.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.requirements[i].icon;
                    }
                    for (int i = 0; i < item.results.Count; i++)
                    {
                        itemObj.GetChild(3).GetChild(i).gameObject.SetActive(true);
                        itemObj.GetChild(3).GetChild(i).GetComponent<DiscoveryHolder>().discovery = item.results[i];
                        itemObj.GetChild(3).GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.results[i].icon;
                    }
                    tradeUIs.Add(itemObj.GetComponent<TradeHolder>());
                }
            }
        }
    }

    void UpdateInformationUI()
    {
        informationPanel.SetActive(_currentDiscovery != null && DiscoveryManager.Instance._discoveries.Contains(_currentDiscovery));
        if (_currentDiscovery != null && _currentDiscovery != lastDiscovery)
        {
            lastDiscovery = _currentDiscovery;
            if (!DiscoveryManager.Instance._discoveries.Contains(_currentDiscovery))
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

    public void OpenTrade(bool active)
    {
        if (active)
            OnTradeOpen?.Invoke();
        tradePanel.SetActive(active);
    }

    public bool IsUIOpen() => tradeUIOpen;
}
