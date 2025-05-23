using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;


public class NewItem : Singleton<NewItem>
{
    /*Bu class yeni item spawnlamamız için son satırı kontrol ediyor eğer null dönerse
    sırasıyla satırın ait olduğu kolona rastgele item spawnlıyor*/


    public GameGrid grid;
    ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));
    public Cell[,] Cells { get; private set; }
    
    public void SpawnNewCubesAtTop()
    {
        for (int x = 0; x < grid.Cols; x++)
        {
            // Sütundaki boşluk sayısını hesapla (EN ÜSTTEN BAŞLAYARAK)
            int missingCubeCount = 0;
            for (int y = grid.Rows -1; y >= 0; y--) // ÜST SATIR = grid.Rows - 1
            {
                Cell currentCell = grid.GetCell(x, y);

                
                if (currentCell.item == null)
                {
                    missingCubeCount++;
                }
                else
                {
                    break; // Üstte boşluk yoksa döngüyü durdur
                }
            }

            // Yeni küpleri spawnla
            if (missingCubeCount > 0)
            {
                
                for(int i = 0; i < missingCubeCount; i++)
                {
                ItemType randomType = itemTypes[Random.Range(1, 7)];

                Item newItem = ItemFactory.Instance.CreateItem(randomType, grid.itemsParent);

                var cell = grid.Cells[x,grid.Rows -1 -i];
                cell.item = newItem;
                
                //anim
                Vector2 startPos = cell.transform.position + new Vector3(0, 5); //Ne kadar yukarıdan düşsün
                Vector2 targetPos = newItem.Cell.transform.position; // asıl cell noktasını belirle

                StartCoroutine(MoveItemToPosition(newItem, startPos, targetPos, 0.2f)); 

                }
            }
            

            
        }
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


}
