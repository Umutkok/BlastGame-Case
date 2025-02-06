using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : Singleton<FallManager>
{
    public GameGrid grid;

    private void Start()
    {
        grid = FindObjectOfType<GameGrid>();
    }

    public void ApplyGravity()
    {
        for (int x = 0; x < grid.Cols; x++)  // Her sütunu ayrı işle
        {
            Stack<Cell> emptyCells = new Stack<Cell>(); // Boş hücreleri tut

            for (int y = 0; y < grid.Rows; y++) // Yukarıdan aşağıya işle
            {
                Cell currentCell = grid.GetCell(x, y);
                if (currentCell == null) continue;

                if (currentCell.item == null)
                {
                    emptyCells.Push(currentCell); // Boş hücreleri stack'e ekle
                }
                else if (emptyCells.Count > 0) // Eğer boş hücre varsa, yukarıdan aşağı taşı
                {
                    Cell emptyCell = emptyCells.Pop();
                    MoveItemDown(currentCell, emptyCell);
                    emptyCells.Push(currentCell); // Boş hücre yukarı kaydı
                }
            }
        }
    }

    private void MoveItemDown(Cell from, Cell to)
    {
        Item fallingItem = from.item;
        if (fallingItem == null) return;

        from.item = null;
        to.item = fallingItem;
        fallingItem.Cell = to;

        StartCoroutine(MoveItemSmooth(fallingItem, to.transform.position));
    }

    private IEnumerator MoveItemSmooth(Item item, Vector3 targetPosition)
    {
        float duration = 0.2f;
        Vector3 startPos = item.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (item == null) yield break;

            item.transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (item != null)
            item.transform.position = targetPosition;
    }
}