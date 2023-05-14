using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Discovery> _items = new List<Discovery>();

    public UnityEvent OnInventoryOpen;
    public UnityEvent OnItemAdd;

    [Header("UI")]
    public GameObject inventoryPanel;
    public List<DiscoveryHolder> slots = new List<DiscoveryHolder>();
    public Transform pickupNotifierContainer;
    public GameObject pickupNotifierPrefab;
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
    public Transform slotsPrefab;
    public Transform slotsContainer;
    public TMP_Text pageNumberText;

    [Header("Debug")]
    [ReadOnly] public bool inventoryOpen;
    [ReadOnly] Discovery lastDiscovery;
    [ReadOnly] Discovery _currentDiscovery;
    [ReadOnly] int prevItemAmt;
    [SerializeField][ReadOnly] List<DiscoveryHolder> slotsToDisplay = new List<DiscoveryHolder>();

    public static Inventory Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OpenInventory(false);
    }

    public bool AddToInventory(Discovery newItem, int amount)
    {
        if(newItem == null) return false;

        _items.Add(newItem);
        OnItemAdd?.Invoke();
        AddToPickupNotifier(newItem);
        DiscoveryManager.Instance.AddNewDiscovery(newItem);
        return true;
    }

    public void DeleteItem(DiscoveryHolder item)
    {
        foreach (var i in _items)
        {
            if(i == item.discovery){
                _items.RemoveAt(_items.IndexOf(i));
                return;
            }
        }
        //_items.RemoveAt(slots.IndexOf(item));
    }
        
    public void AddToPickupNotifier(Discovery discovery)
    {
        Transform newNotifier = Instantiate(pickupNotifierPrefab, pickupNotifierContainer).transform;
        newNotifier.transform.GetChild(0).GetComponent<Image>().sprite = discovery.icon;
        newNotifier.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = discovery.title;
    }



    void Update()
    {
        _currentDiscovery = DiscoveryManager.Instance._currentDiscovery;
        UpdateInformationUI();
        UpdateSlotsUI();

        inventoryOpen = inventoryPanel.activeSelf;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenInventory(!inventoryPanel.activeSelf);
        }
    }

    void UpdateSlotsUI()
    {
        PopulateSlots();

        int startIndex = currentPage * slotsPerPage;
        slotsToDisplay = slots.Skip(startIndex).Take(slotsPerPage).ToList();

        int totalPages = Mathf.CeilToInt((float)slots.Count / slotsPerPage);
        string pageText = (currentPage + 1) + "/" + totalPages;
        pageNumberText.text = pageText;

        // Clamp pages
        if (currentPage + 1 >= totalPages) currentPage = totalPages - 1;
        if (currentPage <= 0) currentPage = 0;

        for (int i = 0; i < slots.Count; i++)
            slots[i].gameObject.SetActive(false);

        for (int i = 0; i < slotsToDisplay.Count; i++)
        {
            slotsToDisplay[i].gameObject.SetActive(true);
        }
    }
    // Rework for perfomance
    void PopulateSlots()
    {
        if (prevItemAmt != _items.Count)
        {
            prevItemAmt = _items.Count;

            slots.Clear();
            for (int i = slotsContainer.childCount - 1; i >= 0; i--)
                Destroy(slotsContainer.GetChild(i).gameObject);

            foreach (Discovery item in _items)
            {
                Transform itemObj = Instantiate(slotsPrefab, slotsContainer);

                if (!string.IsNullOrEmpty(item.title))
                {
                    itemObj.GetChild(0).GetComponent<Image>().sprite = item.icon;
                    itemObj.GetComponent<DiscoveryHolder>().discovery = item;
                    slots.Add(itemObj.GetComponent<DiscoveryHolder>());
                }
            }
        }
    }

    void UpdateInformationUI()
    {
        informationPanel.SetActive(_currentDiscovery != null && _items.Contains(_currentDiscovery));
        if (_currentDiscovery != null && _currentDiscovery != lastDiscovery)
        {
            lastDiscovery = _currentDiscovery;

            iconUI.sprite = _currentDiscovery.icon;
            titleUI.text = _currentDiscovery.title;
            typeUI.text = _currentDiscovery.discoveryType.ToString();
            descUI.text = _currentDiscovery.description;

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

    public void OpenInventory(bool active)
    {
        if (active)
            OnInventoryOpen?.Invoke();
        inventoryPanel.SetActive(active);
    }

    public void ChangePage(int amt)
    {
        currentPage += amt;
    }

    public bool IsUIOpen() => inventoryOpen;
}
