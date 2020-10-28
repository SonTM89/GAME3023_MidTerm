using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class ItemException : System.Exception
//{
//    public ItemException(string message) : base(message)
//    {

//    }
//}


//[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
//public class Item : ScriptableObject
//{
//    [SerializeField]
//    private int itemID;

//    public int ItemID
//    {
//        get { return itemID; }
//        set {
//            itemID = value;
//            throw new ItemException("You never should have come here!");
//        }
//    }

//    [SerializeField]
//    private new string name = "item";
//    public string Name
//    {
//        get { return name; }
//        private set { }
//    }

//    [SerializeField]
//    [TextArea]
//    private string description = "this is an item";
//    public string Description
//    {
//        get { return description; }
//        private set { }
//    }

//    [SerializeField]
//    private string category = "misc";
//    public string Category
//    {
//        get { return category; }
//        private set { }
//    }


//    [SerializeField]
//    private Sprite icon;
//    public Sprite Icon
//    {
//        get { return icon; }
//        private set { }
//    }


//    public void Use()
//    {
//        //Debug.Log("Used item " + name);
//    }
//}

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Potion,
        PortionGreen,
        Fruit,
        Hat,
        Shield,
    }

    public ItemType itemType;
    public int amount = 1;
    private IItemHolder itemHolder;

    public void SetItemHolder(IItemHolder itemHolder)
    {
        this.itemHolder = itemHolder;
    }

    public IItemHolder GetItemHolder()
    {
        return itemHolder;
    }

    public void RemoveFromItemHolder()
    {
        if (itemHolder != null)
        {
            // Remove from current Item Holder
            itemHolder.RemoveItem(this);
        }
    }

    public void MoveToAnotherItemHolder(IItemHolder newItemHolder)
    {
        RemoveFromItemHolder();
        // Add to new Item Holder
        newItemHolder.AddItem(this);
    }


    public Sprite GetSprite()
    {
        return GetSprite(itemType);
    }
    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Potion:
                return ItemAssets.Instance.potionSprite;
            case ItemType.PortionGreen:
                return ItemAssets.Instance.potionGreenSprite;
            case ItemType.Fruit:
                return ItemAssets.Instance.fruitSprite;
            case ItemType.Hat:
                return ItemAssets.Instance.hatSprite;
            case ItemType.Shield:
                return ItemAssets.Instance.shiledSprite;
        }
    }

    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Potion:
                return true;
            case ItemType.PortionGreen:
                return true;
            case ItemType.Fruit:
                return true;
            case ItemType.Hat:
                return true;
            case ItemType.Shield:
                return true;
        }
    }
}