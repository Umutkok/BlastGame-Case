using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SmurtShuffle : Singleton<SmurtShuffle>
{
    [SerializeField] private GameGrid grid;
    private List<int> matchCounts;
    private bool Shuffled = false;
    Vector2 MoveCenter;
    private float AnimTime = 0.5f;
    
    void Start()
    {
        matchCounts = new List<int>();
        MoveCenter = new Vector2(0, 0);
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
        List<Item> allItems = CollectAllItems();
        List<Vector2Int> emptyPositions = GetAllPositions();
        Dictionary<string, List<Item>> sortedGroups = SortItemsByMatchType(allItems);
        List<List<Item>> selectedGroups = SelectTopGroups(sortedGroups);
        
        PlaceGroupsInGrid(selectedGroups, ref emptyPositions);
        PlaceRemainingItems(allItems, emptyPositions);
        
        IconManager.Instance.Icon();
    }
    
    private List<Item> CollectAllItems()
    {
        List<Item> allItems = new List<Item>();
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (grid.Cells[x, y].item != null)
                {
                    allItems.Add(grid.Cells[x, y].item);
                    grid.Cells[x, y].item = null;
                }
            }
        }
        return allItems;
    }
    
    private Dictionary<string, List<Item>> SortItemsByMatchType(List<Item> items)
    {
        return items.Where(item => item != null) 
                    .GroupBy(item => item.GetMatchType().ToString()) // String olarak grupla
                    .OrderByDescending(group => group.Count()) 
                    .ToDictionary(group => group.Key, group => group.ToList());
    }
    
    private List<List<Item>> SelectTopGroups(Dictionary<string, List<Item>> sortedGroups)
    {
        var selectedGroups = sortedGroups.Take(3)
                                         .Select(group => group.Value.Take(Mathf.CeilToInt(group.Value.Count * 0.75f)).ToList())
                                         .ToList();
        return selectedGroups;
    }
    
    private void PlaceGroupsInGrid(List<List<Item>> groups, ref List<Vector2Int> emptyPositions)
    {
        System.Random rng = new System.Random();
        emptyPositions.ShuffleEmphty(rng);
        
        foreach (var group in groups)
        {
            if (emptyPositions.Count == 0) break;
            
            Vector2Int startPos = emptyPositions[0];
            emptyPositions.RemoveAt(0);
            grid.Cells[startPos.x, startPos.y].item = group[0];
            StartCoroutine(ShuffleAnimation(group[0], group[0].transform.position, MoveCenter, grid.Cells[startPos.x, startPos.y].transform.position, AnimTime));
            
            Vector2Int currentPos = startPos;
            for (int i = 1; i < group.Count; i++)
            {
                Vector2Int neighbor = FindNextEmptyNeighbor(currentPos, emptyPositions);
                if (neighbor == Vector2Int.one * -1) break;
                
                grid.Cells[neighbor.x, neighbor.y].item = group[i];
                emptyPositions.Remove(neighbor);
                StartCoroutine(ShuffleAnimation(group[i], group[i].transform.position, MoveCenter, grid.Cells[neighbor.x, neighbor.y].transform.position, AnimTime));
                
                currentPos = neighbor;
            }
        }
    }
    
    private void PlaceRemainingItems(List<Item> remainingItems, List<Vector2Int> emptyPositions)
    {
        System.Random rng = new System.Random();
        remainingItems.ShuffleEmphty(rng);
        
        for (int i = 0; i < remainingItems.Count && i < emptyPositions.Count; i++)
        {
            Vector2Int pos = emptyPositions[i];
            grid.Cells[pos.x, pos.y].item = remainingItems[i];
            StartCoroutine(ShuffleAnimation(remainingItems[i], remainingItems[i].transform.position, MoveCenter, grid.Cells[pos.x, pos.y].transform.position, AnimTime));
        }
    }
    
    private Vector2Int FindNextEmptyNeighbor(Vector2Int current, List<Vector2Int> emptyPositions)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Vector2Int neighbor = current + dir;
            if (emptyPositions.Contains(neighbor)) return neighbor;
        }
        return Vector2Int.one * -1;
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

public static class ListExtensions
{
    public static void ShuffleEmphty<T>(this List<T> list, System.Random rng)
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


