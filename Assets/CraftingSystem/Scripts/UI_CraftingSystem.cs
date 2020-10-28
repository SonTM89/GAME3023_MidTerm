using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftingSystem : MonoBehaviour
{
    [SerializeField]
    private Transform prefab_UI_Item;

    private Transform[,] slotTransformArray;
    private Transform outputSlotTransform;
    private Transform itemContainer;

    private void Awake()
    {
        Transform gridContainer = transform.Find("gridContainer");
        itemContainer = transform.Find("itemContainer");

        slotTransformArray = new Transform[CraftingSystem.GRID_SIZE, CraftingSystem.GRID_SIZE];

        for(int x = 0; x < CraftingSystem.GRID_SIZE; x++)
        {
            for(int y = 0; y < CraftingSystem.GRID_SIZE; y++)
            {
                slotTransformArray[x,y] = gridContainer.Find("grid_" + x + "_" + y);
                UI_CraftingItemSlot craftingItemSlot = slotTransformArray[x, y].GetComponent<UI_CraftingItemSlot>();
                craftingItemSlot.setXY(x, y);

                craftingItemSlot.OnItemDropped += UI_CraftingSystem_OnItemDropped;
            }
        }

        outputSlotTransform = transform.Find("outputSlot");

        
        
    }

    private void UI_CraftingSystem_OnItemDropped(object sender, UI_CraftingItemSlot.OnItemDroppedEventArgs e)
    {
        Debug.Log(e.item + " " + e.x + " " + e.y);
    }

    private void Start()
    {
        CreateItem(0, 0, new Item { itemType = Item.ItemType.Shield });
        CreateItem(2, 1, new Item { itemType = Item.ItemType.PortionGreen});
        CreateItemOutput(new Item { itemType = Item.ItemType.Potion });
    }

    private void CreateItem(int x, int y, Item item)
    {
        Transform itemTransfrom = Instantiate(prefab_UI_Item, itemContainer);
        RectTransform itemRecTransform = itemTransfrom.GetComponent<RectTransform>();
        itemRecTransform.anchoredPosition = slotTransformArray[x,y].GetComponent<RectTransform>().anchoredPosition;
        itemTransfrom.GetComponent<UI_Item>().SetItem(item);
    }

    private void CreateItemOutput(Item item)
    {
        Transform itemTransfrom = Instantiate(prefab_UI_Item, itemContainer);
        RectTransform itemRecTransform = itemTransfrom.GetComponent<RectTransform>();
        itemRecTransform.anchoredPosition = outputSlotTransform.GetComponent<RectTransform>().anchoredPosition;
        itemTransfrom.GetComponent<UI_Item>().SetItem(item);
    }
}
