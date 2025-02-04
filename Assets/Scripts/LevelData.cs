using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// LevelData is a class that represents the data of a level in the game.
/// Constructor takes a LevelInfo object and creates the level data from it.
/// 
/// </summary>
public class LevelData
{
    public ItemType[,] GridData { get; protected set; }
    public LevelData(LevelSO levelInfo)
    {
        // Set the grid data
        GridData = new ItemType[levelInfo.grid_height, levelInfo.grid_width];

        int gridIndex = 0;
        for (int i = levelInfo.grid_height - 1; i >= 0; --i)
            for (int j = 0; j < levelInfo.grid_width; ++j)
            {
                switch (levelInfo.grid[gridIndex++])
                {
                    // Cubes
                    case "b":
                        GridData[i, j] = ItemType.BlueCube;
                        break;
                    case "g":
                        GridData[i, j] = ItemType.GreenCube;
                        break;
                    case "r":
                        GridData[i, j] = ItemType.RedCube;
                        break;
                    case "y":
                        GridData[i, j] = ItemType.YellowCube;
                        break;
                    case "p":
                        GridData[i, j] = ItemType.PinkCube;
                        break;
                    case "pu":
                        GridData[i, j] = ItemType.PurpleCube;
                        break;        
                    default:
                        GridData[i, j] = ((ItemType[])Enum.GetValues(typeof(ItemType)))[Random.Range(1, 5)]; //burası rondom atmasını sağlıyo
                        break;
                }
            }

    }
    public static ItemType GetRandomCubeItemType()
    {
        return ((ItemType[])Enum.GetValues(typeof(ItemType)))[Random.Range(1, 7)]; // 1,5 represents number of blocks
    }

}