using UnityEngine;

/// <summary>
/// 
/// CubeItem is a class that represents a cube item in the game. It inherits from the Item class.
/// 
/// </summary>
/*public class CubeItem : Item
{
    private MatchType matchType;
    
    public void PrepareCubeItem(ItemBase itemBase,MatchType matchType)
    {
        //SoundID = SoundID.Cube;
        this.matchType = matchType;
        itemBase.Clickable = true;
        Prepare(itemBase, GetSpritesForMatchType());
    }
    private Sprite GetSpritesForMatchType()
    {
        var imageLibrary = SpriteHolder.Instance; //dikkat
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
                return imageLibrary.PinkCubeSprite; //Sonradan eklendi başlangıçta spritelara ataması için normalde 4 adet vardı biraz altta update sprite yerinde update atıyo
            case MatchType.Purple:
                return imageLibrary.PurpleCubeSprite;   
        }
        return null;
    }
    public override MatchType GetMatchType()
    {
        return matchType;
    }
    public override void HintUpdateToSprite(ItemType itemType)
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
    private void ConditionASprite(SpriteHolder imageLibrary)
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
    private void ConditionBSprite(SpriteHolder imageLibrary)
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
    private void ConditionCSprite(SpriteHolder imageLibrary)
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
    
    public override void TryExecute()
    {
        //ParticleManager.Instance.PlayParticle(this);
        //AudioManager.Instance.PlayEffect(SoundID);
        base.TryExecute();
    }
}*/
