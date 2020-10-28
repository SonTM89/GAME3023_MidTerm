using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform pfUI_Item;

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private PlayerCharacterController player;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }

    public void SetPlayer(PlayerCharacterController player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            if(child == itemSlotTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 64.0f;

        foreach (Inventory.InventorySlot inventorySlot in inventory.GetInventorySlotArray())
        {
            Item item = inventorySlot.GetItem();

            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use item
                //inventory.UseItem(item);
            };

            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop Item
                //Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                //inventory.RemoveItem(item);
                //ItemWorld.DropItem(player.transform.position, duplicateItem);

                // Split item
                if (item.IsStackable())
                {
                    // Is Stackable
                    if (item.amount > 2)
                    {
                        // Can split
                        int splitAmount = Mathf.FloorToInt(item.amount / 2f);
                        item.amount -= splitAmount;
                        Item duplicateItem = new Item { itemType = item.itemType, amount = splitAmount };
                        inventory.AddItem(duplicateItem);
                    }
                }
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * (itemSlotCellSize + 8), y* (itemSlotCellSize + 8));


            //Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            //image.sprite = Item.GetSprite(item.itemType);
            //TextMeshProUGUI uiText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();
            //if(item.amount >= 1)
            //{
            //    uiText.SetText(item.amount.ToString());
            //}
            //else
            //{
            //    uiText.SetText("");
            //}

            if (!inventorySlot.IsEmpty())
            {
                // Not Empty, has Item
                Transform uiItemTransform = Instantiate(pfUI_Item, itemSlotContainer);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(item);
            }

            Inventory.InventorySlot tmpInventorySlot = inventorySlot;

            UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
            uiItemSlot.SetOnDropAction(() => {
                // Dropped on this UI Item Slot
                Item draggedItem = UI_ItemDrag.Instance.GetItem();
                draggedItem.RemoveFromItemHolder();
                inventory.AddItem(draggedItem, tmpInventorySlot);
            });


            x++;
            if(x>2)
            {
                x = 0;
                y--;
            }
        }
    }
}
