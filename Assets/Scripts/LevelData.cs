using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelData
{
    public ItemType[,] ItemTypes { get; protected set; }
    public LevelData(LevelSO levelSO)
    {
        //levelSO dam aldığımız item typelara ile grid oluşturlur eğer değer girilmediye random değer atanır
        ItemTypes = new ItemType[levelSO.grid_height, levelSO.grid_width];

        int gridIndex = 0;
        for (int i = levelSO.grid_height - 1; i >= 0; --i)
            for (int j = 0; j < levelSO.grid_width; ++j)
            {
                switch (levelSO.grid[gridIndex++])
                {
                    // Cubes
                    case "b":
                        ItemTypes[i, j] = ItemType.BlueCube;
                        break;
                    case "g":
                        ItemTypes[i, j] = ItemType.GreenCube;
                        break;
                    case "r":
                        ItemTypes[i, j] = ItemType.RedCube;
                        break;
                    case "y":
                        ItemTypes[i, j] = ItemType.YellowCube;
                        break;
                    case "p":
                        ItemTypes[i, j] = ItemType.PinkCube;
                        break;
                    case "pu":
                        ItemTypes[i, j] = ItemType.PurpleCube;
                        break;        
                    default:
                        ItemTypes[i, j] = ((ItemType[])Enum.GetValues(typeof(ItemType)))[Random.Range(1, 5)]; //burası rondom atmasını sağlıyo
                        break;
                }
            }

    }
    public static ItemType GetRandomCubeItemType()
    {
        return ((ItemType[])Enum.GetValues(typeof(ItemType)))[Random.Range(1, 7)]; // 1,5 represents number of blocks
    }

}