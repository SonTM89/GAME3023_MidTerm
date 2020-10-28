using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    List<int> wew;

    [SerializeField]
    float speed = 5;

    [SerializeField]
    Rigidbody2D rigidBody;

    [SerializeField]
    private UI_Inventory uI_Inventory;

    private Inventory inventory;

    private void Awake()
    {
        //inventory = new Inventory(UseItem);
        inventory = new Inventory(UseItem, 12);
        //uI_Inventory.SetPlayer(this);
        //uI_Inventory.SetInventory(inventory);

        //ItemWorld.SpawnItemWorld(new Vector3(0, 0), new Item { itemType = Item.ItemType.Shield, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(-2, 2), new Item { itemType = Item.ItemType.Hat, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(4, 4), new Item { itemType = Item.ItemType.Fruit, amount = 1 });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Potion:
                inventory.RemoveItem((item.amount > 1) ? new Item { itemType = Item.ItemType.Potion, amount = 1 } : item);
                break;
            case Item.ItemType.PortionGreen:
                inventory.RemoveItem((item.amount > 1) ? new Item { itemType = Item.ItemType.PortionGreen, amount = 1 } : item);
                break;
            case Item.ItemType.Hat:
                inventory.RemoveItem((item.amount > 1) ? new Item { itemType = Item.ItemType.Hat, amount = 1 } : item);
                break;
            case Item.ItemType.Fruit:
                inventory.RemoveItem((item.amount > 1) ? new Item { itemType = Item.ItemType.Fruit, amount = 1 } : item);
                break;
            case Item.ItemType.Shield:
                inventory.RemoveItem((item.amount > 1) ? new Item { itemType = Item.ItemType.Shield, amount = 1 } : item);
                break;
        }
    }


    public Inventory GetInventory()
    {
        return inventory;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementVector *= speed;
        rigidBody.velocity = movementVector;
    }
}