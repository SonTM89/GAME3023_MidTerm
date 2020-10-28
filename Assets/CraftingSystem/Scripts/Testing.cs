using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterController player;
    [SerializeField]
    private UI_Inventory uI_Inventory;

    private void Start()
    {
        uI_Inventory.SetPlayer(player);
        uI_Inventory.SetInventory(player.GetInventory());

        CraftingSystem craftingSystem = new CraftingSystem();
        Item item = new Item { itemType = Item.ItemType.Hat, amount = 1 };
        craftingSystem.SetItem(item, 0, 0);
        Debug.Log(craftingSystem.GetItem(0, 0));
    }
}
