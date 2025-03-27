using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class InnkeeperInventory : MonoBehaviour
{
    public static InnkeeperInventory instance { get; private set; }

    [Header("UI Displaying")]
    [SerializeField] private TextMeshProUGUI goldCountDisplaying;
    [SerializeField] private TextMeshProUGUI soulPowerDisplaying;
    [SerializeField] private GameObject inventoryUIPanel;

    [Header("Start Objects")]
    public int startGold;
    public int startSoulPower;
    public List<Dish> startDishes;

    public int gold { get; private set; }
    public int soulPower { get; private set; }
    public List<Dish> dishes { get; private set; }

    private List<InventorySlot> slots;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find mort than one Innkeeper Inventory in scene");
        }
        instance = this;
    }

    private void Start()
    {
        gold = startGold;
        soulPower = startSoulPower;

        dishes = new List<Dish>();

        foreach(var dish in startDishes)
        {
            if(dish != null) dishes.Add(dish);
        }

        CreateSlotsList();

        GameEventsManager.instance.innkeeperInventoryEvents.onGoldGained += GoldGained;
        GameEventsManager.instance.innkeeperInventoryEvents.onSoulPowerGained += SoulPowerGained;

        GameEventsManager.instance.inputEvents.onOpenInventoryPressed += OnOffInventoryUI;

       inventoryUIPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GameEventsManager.instance.innkeeperInventoryEvents.onGoldGained -= GoldGained;
        GameEventsManager.instance.innkeeperInventoryEvents.onSoulPowerGained -= SoulPowerGained;

        GameEventsManager.instance.inputEvents.onOpenInventoryPressed -= OnOffInventoryUI;
    }

    private void Update()
    {
        DisplayUI();
        SetInventorySlots();
    }

    private void DisplayUI()
    {
        goldCountDisplaying.text = Convert.ToString(gold);
        soulPowerDisplaying.text = Convert.ToString(soulPower);
    }

    private void OnOffInventoryUI()
    {
        if (inventoryUIPanel.activeSelf == false)
        {
            inventoryUIPanel.SetActive(true);
        }
        else if (inventoryUIPanel.activeSelf == true)
        {
            inventoryUIPanel.SetActive(false);
        }
    }

    private void GoldGained(int gold)
    {
        this.gold += gold;
    }

    private void SoulPowerGained(int soulPower)
    {
        this.soulPower += soulPower;
    }

    private void CreateSlotsList()
    {
        slots = new List<InventorySlot>();

        for (int i = 0; i < inventoryUIPanel.transform.childCount; i++)
        {
            InventorySlot slot = inventoryUIPanel.transform.GetChild(i).GetComponent<InventorySlot>();

            if (slot != null)
            {
                slots.Add(slot);
            }
        }
    }

    private void SetInventorySlots()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            try
            {
                slots[i].SetItem(dishes[i]);
            }
            catch (ArgumentOutOfRangeException)
            {
                slots[i].ClearSlot();
            }
            
        }
    }
}
