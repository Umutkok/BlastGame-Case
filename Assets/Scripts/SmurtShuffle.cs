using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SmurtShuffle : Singleton<SmurtShuffle>
{
    public enum ShuffleDensity { Dense, Medium, Sparse }
    
    [SerializeField] private GameGrid grid;
    [SerializeField] private ShuffleDensity density = ShuffleDensity.Medium;
    
    private List<int> matchCounts;
    private bool Shuffled = false;
    Vector2 MoveCenter;

    private float AnimTime = 0.5f;
    
    void Start()
    {
        matchCounts = new List<int>();
        Vector2 MoveCenter = new Vector2(0,0);
    }

    public void LookInt()
    {
        var visitedCells = new List<Cell>();
        
        for (var y = 0; y < grid.Rows; y++)
        {
            for (var x = 0; x < grid.Cols; x++)
            {
                var cell = grid.Cells[x, y];
                if (cell.item == null || visitedCells.Contains(cell)) continue;

                var matchedCells = MatchingManager.Instance.FindMatches(cell, cell.item.GetMatchType());
                matchCounts.Add(matchedCells.Count);
                visitedCells.AddRange(matchedCells);
            }
        }
        
        if (matchCounts.Count == grid.Rows * grid.Cols && !Shuffled)
        {
            Shuffle();
            Shuffled = true;
        }
    }

    public void ResetMatchCounts()
    {
        matchCounts.Clear();
    }
    
    public void Shuffle()
    {
        List<List<Item>> groupedItems = GroupItemsByMatchType();
        List<Vector2Int> emptyPositions = GetAllPositions();
        System.Random rng = new System.Random();
        emptyPositions.Shuffle(rng);
        
        foreach (var group in groupedItems)
        {
            int groupSize = AdjustGroupSize(group.Count);
            List<Item> selectedGroup = group.Take(groupSize).ToList();
            
            foreach (var item in selectedGroup)
            {
                if (emptyPositions.Count > 0)
                {
                    Vector2Int newPos = emptyPositions[0];
                    emptyPositions.RemoveAt(0);
                    grid.Cells[newPos.x, newPos.y].item = item;
                    //item.transform.position = grid.Cells[newPos.x, newPos.y].transform.position;

                    //MoveItemtoPostion corotineları birleştiren shuffle anim item, 3 transform noktası: itemlerin eski konumu, merkez, itemlerin yeni konumu ve animasyon süresini içeriyor
                    StartCoroutine(ShuffleAnimation(item, item.transform.position, MoveCenter, grid.Cells[newPos.x, newPos.y].transform.position, AnimTime)); 
                    
                    item.UpdateSortingOrder(newPos.y);
                }
            }
        }
        IconManager.Instance.Icon();
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
        item.transform.position = target; // Tam olarak yerine otursun
    }

    private IEnumerator ShuffleAnimation(Item item, Vector3 start, Vector2 Center, Vector3 target, float duration)
    {
        yield return StartCoroutine(MoveItemToPosition(item,start,Center,duration));
        yield return StartCoroutine(MoveItemToPosition(item,Center,target,duration));
    }

    
    private int AdjustGroupSize(int maxSize)
    {
        switch (density)
        {
            case ShuffleDensity.Dense:
                return maxSize;
            case ShuffleDensity.Medium:
                return Mathf.Max(2, maxSize / 2);
            case ShuffleDensity.Sparse:
                return Mathf.Min(3, maxSize);
            default:
                return maxSize;
        }
    }
    
    private List<List<Item>> GroupItemsByMatchType()
    {
        List<List<Item>> groups = new List<List<Item>>();
        bool[,] visited = new bool[grid.Cols, grid.Rows];

        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (!visited[x, y] && grid.Cells[x, y].item != null)
                {
                    List<Cell> matchedCells = MatchingManager.Instance.FindMatches(grid.Cells[x, y], grid.Cells[x, y].item.GetMatchType());
                    if (matchedCells.Count > 0)
                    {
                        List<Item> group = new List<Item>();
                        foreach (var cell in matchedCells)
                        {
                            group.Add(cell.item);
                            visited[cell.X, cell.Y] = true;
                            cell.item = null;
                        }
                        groups.Add(group);
                    }
                }
            }
        }
        return groups;
    }
    
    private List<Vector2Int> GetAllPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }
        return positions;
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list, System.Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}


