using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmurtShuffle : Singleton<SmurtShuffle>
{
    [SerializeField] private GameGrid grid;
    private List<int> matchCounts;
    private bool Shuffled = false;
    void Start()
    {
        matchCounts = new List<int>();
    }

    public void LookInt()
    {
        var visitedCells = new List<Cell>();

        for (var y = 0; y < grid.Rows; y++)
        {
            for(var x = 0; x < grid.Cols; x++)
            {
                var cell = grid.Cells[x, y];
                
                if(cell.item == null || visitedCells.Contains(cell)) continue;

                var matchedCells = MatchingManager.Instance.FindMatches(cell, cell.item.GetMatchType());
                
                matchCounts.Add(matchedCells.Count);
                
                visitedCells.AddRange(matchedCells);



            }
        }
        
        if(matchCounts.Count == 25 && Shuffled == false)
        {
            Shuffle();
            Shuffled = true;
            Debug.Log(matchCounts.Count);
        }
        
    }

    public void ResetMatchCounts()
    {
        matchCounts.Clear();
    }


    public void Shuffle()
    {
        List<Item> allItems = CollectAllItems();
        allItems.Sort((a, b) => a.GetMatchType().CompareTo(b.GetMatchType()));
        RedistributeItems(allItems);
        IconManager.Instance.Icon();


    }
        private List<Item> CollectAllItems()
    {
        List<Item> items = new List<Item>();
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (grid.Cells[x, y].item != null)
                {
                    items.Add(grid.Cells[x, y].item);
                    grid.Cells[x, y].item = null; // Hücreyi boşalt
                }
            }
        }
        return items;
    }

    private void RedistributeItems(List<Item> items)
    {
        int index = 0;
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (index >= items.Count) return;

                // Item'ı hücreye yerleştir
                Item item = items[index++];
                grid.Cells[x, y].item = item;
                item.transform.position = grid.Cells[x, y].transform.position;

                item.UpdateSortingOrder(y);
                
            }
        }
    }

}


