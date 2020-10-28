using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = new Vector3(Random.Range(1, 2), Random.Range(1, 2), 0);
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir, item);
        //itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 2.0f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = transform.Find("amount").GetComponent<TextMeshPro>();

    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = Item.GetSprite(item.itemType);
        if(item.amount >= 1)
        {
            text.SetText(item.amount.ToString());
        }
        else
        {
            text.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
