using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : Singleton<ItemFactory>
{
    public ItemPrefab ItemPrefab;

    //Factory design pattern ile ItemType a göre farklı item çeşitleri oluşturulur.
    private Dictionary<ItemType, Func<ItemPrefab, Item>> itemCreators = new Dictionary<ItemType, Func<ItemPrefab, Item>>
    {
        { ItemType.GreenCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Green) },
        { ItemType.BlueCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Blue) },
        { ItemType.RedCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Red) },
        { ItemType.YellowCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Yellow) },
        { ItemType.PinkCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Pink) },
        { ItemType.PurpleCube, (itemprefab) => CreateCubeItem(itemprefab, MatchType.Purple) },
    };

    public Item CreateItem(ItemType itemType, Transform parent)
    {
        if(itemType == ItemType.None) return null;

        var itemprefab = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, parent);
        itemprefab.ItemType = itemType;

        if (!itemCreators.TryGetValue(itemType, out var createItem))
        {
            Debug.LogWarning("Can not create item: " + itemType);
            return null;
        }

        return createItem(itemprefab);
    }

    private static Item CreateCubeItem(ItemPrefab itemprefab, MatchType matchType)
    {
        var cubeItem = itemprefab.gameObject.AddComponent<Item>(); //Birleştirme için <CubeItem> dan <Item>a değştirildi
        cubeItem.PrepareItem(itemprefab, matchType);
        return cubeItem;
    }

}
