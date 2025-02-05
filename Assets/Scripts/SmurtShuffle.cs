using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmurtShuffle : Singleton<SmurtShuffle>
{
    
    /*Shuffle yapması griddeki cell sayısı ile gridde ki komşusu olmayan item sayısı eşit olmalı
    bu sayede grid üzerinde hamle yapamıyacağımızı anlamış oluyoruz ve shuffle fonksiyonunu çalıştırıyoruz */


    [SerializeField] private GameGrid grid;
    private List<int> matchCounts;
    private bool Shuffled = false;

    private Vector2 AnimCenter = new Vector3(0,0,0);
    private float AnimTime = 0.5f;
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
                //item.transform.position = grid.Cells[x, y].transform.position;
                StartCoroutine(ShuffleAnimation(item,item.transform.position,AnimCenter,grid.Cells[x, y].transform.position,AnimTime));

                item.UpdateSortingOrder(y);

                
            }
        }
    }

    private IEnumerator ShuffleAnimation(Item item, Vector3 start, Vector2 Center, Vector3 target, float duration)
    {
        yield return StartCoroutine(MoveItemToPosition(item, start, Center, duration));
        yield return StartCoroutine(MoveItemToPosition(item, Center, target, duration));
    }
    
    private IEnumerator MoveItemToPosition(Item item, Vector3 start, Vector3 target, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            item.transform.position = Vector3.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.transform.position = target;
    }

}


