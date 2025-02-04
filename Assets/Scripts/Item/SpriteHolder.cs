using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : Singleton<SpriteHolder>
{
    [Header("Red")]
    public Sprite RedCubeSprite;
    public Sprite RedCubeSpriteA;
    public Sprite RedCubeSpriteB;
    public Sprite RedCubeSpriteC;

    [Header("Green")]
    public Sprite GreenCubeSprite;
    public Sprite GreenCubeSpriteA;
    public Sprite GreenCubeSpriteB;
    public Sprite GreenCubeSpriteC;

    [Header("Yellow")]
    public Sprite YellowCubeSprite;
    public Sprite YellowCubeSpriteA;
    public Sprite YellowCubeSpriteB;
    public Sprite YellowCubeSpriteC;

    [Header("Blue")]
    public Sprite BlueCubeSprite;
    public Sprite BlueCubeSpriteA;
    public Sprite BlueCubeSpriteB;
    public Sprite BlueCubeSpriteC;

    [Header("Pink")]
    public Sprite PinkCubeSprite;
    public Sprite PinkCubeSpriteA;
    public Sprite PinkCubeSpriteB;
    public Sprite PinkCubeSpriteC;

    [Header("Purple")]
    public Sprite PurpleCubeSprite;
    public Sprite PurpleCubeSpriteA;
    public Sprite PurpleCubeSpriteB;
    public Sprite PurpleCubeSpriteC;


     public Sprite GetSpriteForItemType(ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.GreenCube: return GreenCubeSprite;
            case ItemType.YellowCube: return YellowCubeSprite;
            case ItemType.BlueCube: return BlueCubeSprite;
            case ItemType.RedCube: return RedCubeSprite;
            case ItemType.PinkCube: return PinkCubeSprite;
            case ItemType.PurpleCube: return PurpleCubeSprite;
 
            default: return null;
        }
    }



}
