using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using System.Linq; // For finding all gameObjects with name
using System;

//public class Inventory : MonoBehaviour, ISaveHandler
//{
//    [Tooltip("Reference to the master item table")]
//    [SerializeField]
//    private ItemTable masterItemTable;

//    [Tooltip("The object which will hold Item Slots as its direct children")]
//    [SerializeField]
//    private GameObject inventoryPanel;

//    [Tooltip("List size determines how many slots there will be. Contents will replaced by copies of the first element")]
//    [SerializeField]
//    private List<ItemSlot> itemSlots;

//    [Tooltip("Items to add on Start for testing purposes")]
//    [SerializeField]
//    private List<Item> startingItems;

//    /// <summary>
//    /// Private key used for saving with playerprefs
//    /// </summary>
//    private string saveKey = "";

//    // Start is called before the first frame update
//    void Start()
//    {
//        InitItemSlots();
//        InitSaveInfo();

//        // init starting items for testing
//        for (int i = 0; i < startingItems.Count && i < itemSlots.Count; i++)
//        {
//            itemSlots[i].SetContents(startingItems[i], 16);
//        }
//    }

//    private void InitItemSlots()
//    {
//        Assert.IsTrue(itemSlots.Count > 0, "itemSlots was empty");
//        Assert.IsNotNull(itemSlots[0], "Inventory is missing a prefab for itemSlots. Add it as the first element of its itemSlot list");

//        // init item slots
//        for (int i = 1; i < itemSlots.Count; i++)
//        {
//            GameObject newObject = Instantiate(itemSlots[0].gameObject, inventoryPanel.transform);
//            ItemSlot newSlot = newObject.GetComponent<ItemSlot>();
//            itemSlots[i] = newSlot;
//        }

//        foreach (ItemSlot item in itemSlots)
//        {
//            item.onItemUse.AddListener(OnItemUsed);
//        }
//    }
//    private void InitSaveInfo()
//    {
//        // init save info
//        //assert only one object with the same name, or else we can have key collisions on PlayerPrefs
//        Assert.AreEqual(
//            Resources.FindObjectsOfTypeAll(typeof(GameObject)).Where(gameObArg => gameObArg.name == gameObject.name).Count(),
//            1,
//            "More than one gameObject have the same name, therefore there may be save key collisions in PlayerPrefs"
//            );

//        // set a key to use for saving/loading
//        saveKey = gameObject.name + this.GetType().Name;

//        //Subscribe to save events on start so we are listening
//        GameSaver.OnLoad.AddListener(OnLoad);
//        GameSaver.OnSave.AddListener(OnSave);
//    }

//    private void OnDestroy()
//    {
//        // Remove listeners on destroy
//        GameSaver.OnLoad.RemoveListener(OnLoad);
//        GameSaver.OnSave.RemoveListener(OnSave);

//        foreach (ItemSlot item in itemSlots)
//        {
//            item.onItemUse.RemoveListener(OnItemUsed);
//        }
//    }

//    //////// Event callbacks ////////

//    void OnItemUsed(Item itemUsed)
//    {
//       // Debug.Log("Inventory: item used of category " + itemUsed.category);
//    }

//    public void OnSave()
//    {
//        //Make empty string
//        //For each item slot
//        //Get its current item
//        //If there is an item, write its id, and its count to the end of the string
//        //If there is not an item, write -1 and 0 

//        //File format:
//        //ID,Count,ID,Count,ID,Count

//        string saveStr = "";

//        foreach(ItemSlot itemSlot in itemSlots)
//        {
//            int id = -1;
//            int count = 0;

//            if(itemSlot.HasItem())
//            {
//                id = itemSlot.ItemInSlot.ItemID;
//                count = itemSlot.ItemCount;
//            }

//            saveStr += id.ToString() + ',' + count.ToString() + ',';
//        }

//        PlayerPrefs.SetString(saveKey, saveStr);
//    }

//    public void OnLoad()
//    {
//        //Get save string
//        //Split save string
//        //For each itemSlot, grab a pair of entried (ID, count) and parse them to int
//        //If ID is -1, replace itemSlot's item with null
//        //Otherwise, replace itemSlot with the corresponding item from the itemTable, and set its count to the parsed count

//        string loadedData = PlayerPrefs.GetString(saveKey, "");

//        Debug.Log(loadedData);

//        char[] delimiters = new char[] { ',' };
//        string[] splitData = loadedData.Split(delimiters);

//        for(int i = 0; i < itemSlots.Count; i++)
//        {
//            int dataIdx = i * 2;

//            int id = int.Parse(splitData[dataIdx]);
//            int count = int.Parse(splitData[dataIdx + 1]);

//            if(id < 0)
//            {
//                itemSlots[i].ClearSlot();
//            } else
//            {
//                itemSlots[i].SetContents(masterItemTable.GetItem(id), count);
//            }
//        }
//    }
//}

public class Inventory : IItemHolder
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    private Action<Item> useItemAction;
    public InventorySlot[] inventorySlotArray;

    public Inventory(Action<Item> useItemAction, int inventorySlotCount)
    {
        this.useItemAction = useItemAction;

        itemList = new List<Item>();

        inventorySlotArray = new InventorySlot[inventorySlotCount];
        for (int i = 0; i < inventorySlotCount; i++)
        {
            inventorySlotArray[i] = new InventorySlot(i);
        }


        AddItem(new Item { itemType = Item.ItemType.Shield, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Potion, amount = 3 });
        AddItem(new Item { itemType = Item.ItemType.Hat, amount = 3 });
        AddItem(new Item { itemType = Item.ItemType.Fruit, amount = 3 });
        //Debug.Log(itemList.Count);
    }


    public InventorySlot GetEmptyInventorySlot()
    {
        foreach (InventorySlot inventorySlot in inventorySlotArray)
        {
            if (inventorySlot.IsEmpty())
            {
                return inventorySlot;
            }
        }
        Debug.LogError("Cannot find an empty InventorySlot!");
        return null;
    }

    public InventorySlot GetInventorySlotWithItem(Item item)
    {
        foreach (InventorySlot inventorySlot in inventorySlotArray)
        {
            if (inventorySlot.GetItem() == item)
            {
                return inventorySlot;
            }
        }
        Debug.LogError("Cannot find Item " + item + " in a InventorySlot!");
        return null;
    }


    //public void AddItem(Item item)
    //{
    //    if(item.IsStackable())
    //    {
    //        bool hasItem = false;
    //        foreach (Item inventoryItem in itemList)
    //        {
    //            if(inventoryItem.itemType == item.itemType)
    //            {
    //                inventoryItem.amount += item.amount;
    //                hasItem = true;
    //            }
    //        }
    //        if(!hasItem)
    //        {
    //            itemList.Add(item);
    //        }
    //    }
    //    else
    //    {
    //        itemList.Add(item);
    //    }

    //    OnItemListChanged?.Invoke(this, EventArgs.Empty);
    //}

    public void AddItem(Item item)
    {
        itemList.Add(item);
        item.SetItemHolder(this);
        GetEmptyInventorySlot().SetItem(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddItemMergeAmount(Item item)
    {
        // Adds an Item and increases amount if same ItemType already present
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
                item.SetItemHolder(this);
                GetEmptyInventorySlot().SetItem(item);
            }
        }
        else
        {
            itemList.Add(item);
            item.SetItemHolder(this);
            GetEmptyInventorySlot().SetItem(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public void RemoveItem(Item item)
    {
        GetInventorySlotWithItem(item).RemoveItem();
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItemAmount(Item.ItemType itemType, int amount)
    {
        RemoveItemRemoveAmount(new Item { itemType = itemType, amount = amount });
    }


    public void RemoveItemRemoveAmount(Item item)
    {
        // Removes item but tries to remove amount if possible
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                GetInventorySlotWithItem(itemInInventory).RemoveItem();
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            GetInventorySlotWithItem(item).RemoveItem();
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddItem(Item item, InventorySlot inventorySlot)
    {
        // Add Item to a specific Inventory slot
        itemList.Add(item);
        item.SetItemHolder(this);
        inventorySlot.SetItem(item);

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }



    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public InventorySlot[] GetInventorySlotArray()
    {
        return inventorySlotArray;
    }

    public bool CanAddItem()
    {
        return GetEmptyInventorySlot() != null;
    }

    //public void RemoveItem(Item item)
    //{
    //    if (item.IsStackable())
    //    {
    //        Item itemInventory = null;
    //        foreach (Item inventoryItem in itemList)
    //        {
    //            if (inventoryItem.itemType == item.itemType)
    //            {
    //                inventoryItem.amount -= item.amount;
    //                itemInventory = inventoryItem;
    //            }
    //        }
    //        if (itemInventory != null && itemInventory.amount <= 0)
    //        {

    //            itemList.Remove(item);
    //        }
    //    }
    //    else
    //    {
    //        itemList.Remove(item);
    //    }

    //    OnItemListChanged?.Invoke(this, EventArgs.Empty);
    //}

    //public List<Item> GetItemList()
    //{
    //    return itemList;
    //}


    /*
     * Represents a single Inventory Slot
     * */
    public class InventorySlot
    {

        private int index;
        private Item item;

        public InventorySlot(int index)
        {
            this.index = index;
        }

        public Item GetItem()
        {
            return item;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void RemoveItem()
        {
            item = null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }

    }
}
