using System.Collections;
using UnityEngine;

public class FallManager : Singleton<FallManager>
{
    public GameGrid grid;

    private void Start()
    {
        grid = FindObjectOfType<GameGrid>();
    }
    

    // Tüm tahtayı tarar ve düşmesi gereken şekerleri aşağıya iter.
    public void ApplyGravity()
    {
        bool hasFallingItems; 

        do
        {
            hasFallingItems = false;

            for (int y = grid.Rows - 1; y >= 0; y--) 
            {
                for (int x = 0; x < grid.Cols; x++)
                {
                    Cell currentCell = grid.GetCell(x, y);
                    Cell belowCell = grid.GetCell(x, y - 1);

                    if (currentCell == null || belowCell == null) continue;

                    // Eğer alttaki hücre boşsa ve üstte bir item varsa, düşme işlemi yap
                    if (belowCell.item == null && currentCell.item != null)
                    {
                        MoveItemDown(currentCell, belowCell);
                        hasFallingItems = true;
                    }
                }
            }
        } while (hasFallingItems); // Eğer hâlâ düşecek bir şey varsa, tekrar et
        
        
    }



    // Belirtilen itemi bir alt hücreye taşı
    private void MoveItemDown(Cell from, Cell to)
    {
        Item fallingItem = from.item;
        if (fallingItem == null) return;

        from.item = null; // Eski hücreyi boşalt
        to.item = fallingItem; // Yeni hücreye item'i yerleştir
        fallingItem.Cell = to; // Item'in hücre bilgisini güncelle

        StartCoroutine(MoveItemSmooth(fallingItem, to.transform.position));
    }


    // Öğe yumuşak bir şekilde aşağı düşürmek için
    private IEnumerator MoveItemSmooth(Item item, Vector3 targetPosition)
    {
        float duration = 0.2f;
        Vector3 startPos = item.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // **Eğer nesne yok edildiyse animasyonu iptal et**
            if (item == null) yield break;

            item.transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (item != null)
            item.transform.position = targetPosition; // Kesin konumda bırak
    }



}