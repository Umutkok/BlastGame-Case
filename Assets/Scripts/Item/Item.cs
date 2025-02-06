using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Item oyundaki şekerlerimizin ana unsuru diğer script ler ile beraber çalışarak itemlerin spritelarının doğru bir şekilde oluşturuyor.
    // Eğer bulunduğu cell input alırsa kendisini destroylayıp fallManager,NewItem,IconManager ı tetikliyor.
    // Bu sayede oyunda eğer herhangi bir item yok olursa grid mekaniklerini devreye sokmuş oluyoruz.



    public ItemType ItemType;
    private Cell cell;
    private MatchType matchType;
    public SpriteRenderer SpriteRenderer;
    public float SpriteSize = 0.44f;

    public Cell Cell
    {
        get { return cell; }
        set
        {
            if(cell == value) return;

            var oldCell = cell;
            cell = value;

            if (oldCell != null && oldCell.item == this)
                oldCell.item = null;
    
            if(value != null)
            {
                value.item = this;
            }
        }
    }

    public void Prepare(ItemPrefab ItemPrefab, Sprite sprite)
    {
        SpriteRenderer = AddSprite(sprite);
        ItemType = ItemPrefab.ItemType;
    }

    public SpriteRenderer AddSprite(Sprite sprite)
    {
        var spriteRenderer = new GameObject("Sprite_" + childSpriteOrder).AddComponent<SpriteRenderer>();


        spriteRenderer.transform.SetParent(transform);
        spriteRenderer.transform.localPosition = Vector3.zero;
        spriteRenderer.transform.localScale = new Vector2(SpriteSize, SpriteSize);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Cell");
        spriteRenderer.sortingOrder = BaseSortingOrder + childSpriteOrder++;

        return spriteRenderer;
    }
    private const int BaseSortingOrder = 10;
    public static int childSpriteOrder;
    public void UpdateSortingOrder(int cellY)
    {
        if (SpriteRenderer == null) return;
        SpriteRenderer.sortingOrder = cellY * 10; // Y değeri arttıkça öne çıkar Shuffle için
    }

    public void RemoveItem()
    {
        Cell.item = null;
        Cell = null;
        Destroy(gameObject); //object pooling eklenebir
        
        // Şekerler patladığında tahtada değişiklik olduğunu bildir
        FallManager.Instance.ApplyGravity();
        NewItem.Instance.SpawnNewCubesAtTop();
        IconManager.Instance.Icon();

    }
    
    public void UpdateSprite(Sprite sprite)
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprite; 
    }

    public void PrepareItem(ItemPrefab itemBase,MatchType matchType)
    {
        this.matchType = matchType;
        Prepare(itemBase, GetSpritesForMatchType());
    }
    private Sprite GetSpritesForMatchType()
    {
        var imageLibrary = SpriteHolder.Instance; 
        switch(matchType)
        {
            case MatchType.Green:
                return imageLibrary.GreenCubeSprite;
            case MatchType.Yellow:
                return imageLibrary.YellowCubeSprite;
            case MatchType.Blue:
                return imageLibrary.BlueCubeSprite;
            case MatchType.Red:
                return imageLibrary.RedCubeSprite;
            case MatchType.Pink:
                return imageLibrary.PinkCubeSprite;
            case MatchType.Purple:
                return imageLibrary.PurpleCubeSprite;   
        }
        return null;
    }
    public MatchType GetMatchType()
    {
        return matchType;
    }

    public  void HintUpdateToSprite(ItemType itemType)
    {
        var imageLibrary = SpriteHolder.Instance;

        switch(itemType)
        {
            case ItemType.ConditionA:
                ConditionASprite(imageLibrary);
                break;
            case ItemType.ConditionB:
                ConditionBSprite(imageLibrary);
                break;
            case ItemType.ConditionC:
                ConditionCSprite(imageLibrary);
                break;    
            default:
                UpdateSprite(GetSpritesForMatchType());
                break;
        }
    }
    private void ConditionASprite(SpriteHolder imageLibrary) //A sprite
    {
        Sprite newSprite;
        switch (matchType)
        {
            case MatchType.Green:
                newSprite = imageLibrary.GreenCubeSpriteA;
                break;
            case MatchType.Yellow:
                newSprite = imageLibrary.YellowCubeSpriteA;
                break;
            case MatchType.Blue:
                newSprite = imageLibrary.BlueCubeSpriteA;
                break;
            case MatchType.Red:
                newSprite = imageLibrary.RedCubeSpriteA;
                break;
            case MatchType.Pink:
                newSprite = imageLibrary.PinkCubeSpriteA;
                break;
            case MatchType.Purple:
                newSprite = imageLibrary.PurpleCubeSpriteA;
                break;
            default:
                return;
        }
        UpdateSprite(newSprite);
    }
    private void ConditionBSprite(SpriteHolder imageLibrary) //B sprite
    {
        Sprite newSprite;
        switch (matchType)
        {
            case MatchType.Green:
                newSprite = imageLibrary.GreenCubeSpriteB;
                break;
            case MatchType.Yellow:
                newSprite = imageLibrary.YellowCubeSpriteB;
                break;
            case MatchType.Blue:
                newSprite = imageLibrary.BlueCubeSpriteB;
                break;
            case MatchType.Red:
                newSprite = imageLibrary.RedCubeSpriteB;
                break;
            case MatchType.Pink:
                newSprite = imageLibrary.PinkCubeSpriteB;
                break;
            case MatchType.Purple:
                newSprite = imageLibrary.PurpleCubeSpriteB;
                break;
            default:
                return;
        }
        UpdateSprite(newSprite);
    }
    private void ConditionCSprite(SpriteHolder imageLibrary) //C sprite
    {
        Sprite newSprite;
        switch (matchType)
        {
            case MatchType.Green:
                newSprite = imageLibrary.GreenCubeSpriteC;
                break;
            case MatchType.Yellow:
                newSprite = imageLibrary.YellowCubeSpriteC;
                break;
            case MatchType.Blue:
                newSprite = imageLibrary.BlueCubeSpriteC;
                break;
            case MatchType.Red:
                newSprite = imageLibrary.RedCubeSpriteC;
                break;
            case MatchType.Pink:
                newSprite = imageLibrary.PinkCubeSpriteC;
                break;
            case MatchType.Purple:
                newSprite = imageLibrary.PurpleCubeSpriteC;
                break;
            default:
                return;
        }
        UpdateSprite(newSprite);
    }
    
    public  void TryExecute()
    {
        RemoveItem();   
    }
}
