using System.Collections.Generic;
using UnityEngine;

public class IconManager : Singleton<IconManager>
{
    //IconManager matching managerdan aldığı groupSize bilgisi ile item conditionlarını ayarlar
        
    [SerializeField] private GameGrid board;
    [SerializeField] private int conditionA = 4, conditionB = 6, conditionC = 8;

    private Cell cell;

    private void Start()
    {
        Icon();
    }

    private void ResetAllIconsToDefault()
    {
        for (int y = 0; y < board.Rows; y++)
        {
            for (int x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];
                if (cell.item != null)
                {
                    cell.item.HintUpdateToSprite(cell.item.ItemType);
                }
            }
        }
    }

    //Matching manager ile komşuları bulup ıcon update condition nına göre ayarla
    public void Icon()
    {
        ResetAllIconsToDefault();
        var visitedCells = new List<Cell>();

        for (var y = 0; y < board.Rows; y++)
        {
            for(var x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];
                
                if(cell.item == null || visitedCells.Contains(cell)) continue;
                var matchedCells = MatchingManager.Instance.FindMatches(cell, cell.item.GetMatchType());
                int groupSize = matchedCells.Count; //grup büyüklüğü Matching manager ile ayarlansın
                visitedCells.AddRange(matchedCells);

                foreach (var matchedCell in matchedCells)
                {
                    var currentItem = matchedCell.item;
                    if(currentItem == null) continue;

                    UpdateItemVisuals(currentItem, groupSize);
                    SetStatic(currentItem, groupSize);
                }
            }
        }
    }



    private void UpdateItemVisuals(Item item, int groupSize)
    {
        // Icon güncelleme
        if (groupSize > conditionC)
        {
            item.HintUpdateToSprite(ItemType.ConditionC);
        }
        else if (groupSize > conditionB)
        {
            item.HintUpdateToSprite(ItemType.ConditionB);
        }
        else if (groupSize > conditionA)
        {
            item.HintUpdateToSprite(ItemType.ConditionA);
        }
    }

    //eğer komşusu yoksa static yap
    private void SetStatic(Item item, int groupSize)
    {
        if(groupSize == 1)
        {
            item.gameObject.isStatic = true;
        }
        else{
            item.gameObject.isStatic = false;
        }
    }
}
