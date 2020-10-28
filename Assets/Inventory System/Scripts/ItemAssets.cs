using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public Transform pfItemWorld;
    public Transform pfUI_Item;

    public Sprite potionSprite;
    public Sprite potionGreenSprite;
    public Sprite fruitSprite;
    public Sprite hatSprite;
    public Sprite shiledSprite;

    private void Awake()
    {
        Instance = this;
    }

    


}
