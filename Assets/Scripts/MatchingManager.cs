using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    public static MatchingManager Instance;
    
    [SerializeField] private GameGrid board; // GameGrid referansını Inspector'dan ata

    private List<int> matchCounts; // Eşleşme boyutlarını saklayacak liste

    private void Awake()
    {
        Instance = this;
        matchCounts = new List<int>();
    }
    public List<Cell> FindMatches(Cell startCell, MatchType matchType)
    {
        List<Cell> matchedCells = new List<Cell>();
        
        // Hücre veya item yoksa boş dön
        if (startCell == null || startCell.item == null || startCell.item.GetMatchType() != matchType)
            return matchedCells;

        // Ziyaret edilen hücreleri takip etmek için
        bool[,] visited = new bool[board.Cols, board.Rows];
        Queue<Cell> queue = new Queue<Cell>();
        
        queue.Enqueue(startCell);
        visited[startCell.X, startCell.Y] = true;

        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            matchedCells.Add(currentCell);

            // Tüm komşu hücre kontrolü(sol, sağ, yukarı, aşağı)
            CheckNeighbor(currentCell.X + 1, currentCell.Y, matchType, visited, queue);
            CheckNeighbor(currentCell.X - 1, currentCell.Y, matchType, visited, queue);
            CheckNeighbor(currentCell.X, currentCell.Y + 1, matchType, visited, queue);
            CheckNeighbor(currentCell.X, currentCell.Y - 1, matchType, visited, queue);
        }
        //matchCounts.
        return matchedCells;
    }


    private void CheckNeighbor(int x, int y, MatchType matchType, bool[,] visited, Queue<Cell> queue)
    {
        // Grid sınırlarını kontrol et
        if (x < 0 || x >= board.Cols || y < 0 || y >= board.Rows) return;

        // Zaten ziyaret edildiyse atla
        if (visited[x, y]) return;

        Cell neighborCell = board.Cells[x, y];
        
        // Komşu hücrede aynı türden item var mı?
        if (neighborCell.item != null && neighborCell.item.GetMatchType() == matchType)
        {
            visited[x, y] = true;
            queue.Enqueue(neighborCell);
        }
    }


    
}
