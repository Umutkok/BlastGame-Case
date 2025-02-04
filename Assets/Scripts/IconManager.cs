using System.Collections.Generic;
using UnityEngine;

public class IconManager : Singleton<IconManager>
{
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
                int groupSize = matchedCells.Count;
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
        // Particle kontrolü

       /* if (groupSize > conditionC && item.GetMatchType() == MatchType.Special)
        {
            if (item.Particle == null)
            {
                var particle = ParticleManager.Instance.ComboHintParticle;
                item.Particle = Instantiate(particle, item.transform);
            }
        }
        else if (item.Particle != null)
        {
            Destroy(item.Particle.gameObject);
        }*/

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
