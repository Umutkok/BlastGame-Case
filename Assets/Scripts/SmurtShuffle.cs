using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmurtShuffle : Singleton<SmurtShuffle>
{
    
    /*Shuffle yapması griddeki cell sayısı ile gridde ki komşusu olmayan item sayısı eşit olmalı matchCounts ile bunu ölçüyoruz
    bu sayede grid üzerinde hamle yapamayacağımızı anlamış oluyoruz ve shuffle fonksiyonunu çalıştırıyoruz
    daha sonra tahtadaki tüm cell leri temizliyoruz ve groupedItems Dictionary sinde matchType larına göre listeliyoruz
    en uzun 3 listeyi rastgele cell noktalarında başlatıp komşularını BFS ile bulup spawnlatıyoruz
    kalan itemleri ise rastgele boş kalan cell lere yerleştiriyoruz bu sayede kesin olarak eşleşme sağlarken bu eşleşmenin oranını da kontrol edebiliyoruz

    */


    [SerializeField] private GameGrid grid;
    private List<int> matchCounts;
    public bool shuffling = false;

    private Vector2 AnimCenter = new Vector3(0,0,0);
    private float AnimTime = 0.2f;
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
        
        if(matchCounts.Count == grid.Rows*grid.Cols && shuffling == false) // grid boyutu ile eşit olursa
        {
            shuffling = true;
            Shuffle();
            Debug.Log(matchCounts.Count);
        }
        
    }



    public void ResetMatchCounts()
    {
        matchCounts.Clear();
    }


    public void Shuffle()
    {
        // 1. Itemları grupla, azalt ve kalanları karıştır
        var processedItems = ProcessItemsWithReduction();
        List<List<Item>> reducedTopThree = processedItems.reducedTopThree;
        List<Item> finalCombined = processedItems.finalCombinedRemaining
                                                .OrderBy(x => Guid.NewGuid())
                                                .ToList();

        // 2. Grid'in tüm hücrelerini boşalt
        ClearGrid();
        Debug.Log(reducedTopThree[0].Count);

        // 3. Rastgele başlangıç noktalarından reducedTopThree'leri dağıt
        foreach (var list in reducedTopThree)
        {
            if (list.Count == 0) continue;

            // Rastgele başlangıç hücresi
            Vector2Int startPos = GetRandomEmptyCell();
            if (startPos.x == -1) break; // Boş hücre kalmadıysa

            // BFS ile komşu hücrelere yerleştir
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(startPos);
            int itemIndex = 0;

            while (queue.Count > 0 && itemIndex < list.Count)
            {
                Vector2Int pos = queue.Dequeue();
                
                // Hücre doluysa atla
                if (grid.Cells[pos.x, pos.y].item != null) continue;

                // Item'ı yerleştir
                PlaceItem(list[itemIndex++], pos.x, pos.y);
                
                // Komşuları kuyruğa ekle
                AddNeighborsToQueue(pos, queue);
            }
        }

        // 4. Kalan tüm boş hücrelere finalCombined'den elemanları yerleştir
        int remainingIndex = 0;
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (grid.Cells[x, y].item == null && remainingIndex < finalCombined.Count)
                {
                    PlaceItem(finalCombined[remainingIndex++], x, y);
                }
            }
        }

        IconManager.Instance.Icon();
    }

    
    private void ClearGrid()
    {
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                grid.Cells[x, y].item = null;
            }
        }
    }
    //reducedTopThree için rondom cell seçiyoruz seçilen noktadan başlayarak komşulara dağılacak
    private Vector2Int GetRandomEmptyCell()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if (grid.Cells[x, y].item == null)
                {
                    emptyCells.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyCells.Count > 0 ? emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)] : new Vector2Int(-1, -1);
    }
    
    private void AddNeighborsToQueue(Vector2Int pos, Queue<Vector2Int> queue)
    {
        // Sağ
        if (pos.x + 1 < grid.Cols) queue.Enqueue(new Vector2Int(pos.x + 1, pos.y));
        // Sol
        if (pos.x - 1 >= 0) queue.Enqueue(new Vector2Int(pos.x - 1, pos.y));
        // Yukarı
        if (pos.y + 1 < grid.Rows) queue.Enqueue(new Vector2Int(pos.x, pos.y + 1));
        // Aşağı
        if (pos.y - 1 >= 0) queue.Enqueue(new Vector2Int(pos.x, pos.y - 1));
    }

    private void PlaceItem(Item item, int x, int y)
    {
        grid.Cells[x, y].item = item;
        StartCoroutine(ShuffleAnimation(
            item, 
            item.transform.position, 
            AnimCenter, 
            grid.Cells[x, y].transform.position, 
            AnimTime
        ));
        item.UpdateSortingOrder(y);
    }



 // matchtype a göre listeler yap 
    private Dictionary<MatchType, List<Item>> CollectAndGroupItems()
    {
        Dictionary<MatchType, List<Item>> groupedItems = new Dictionary<MatchType, List<Item>>();
        
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                Item item = grid.Cells[x, y].item;
                if (item != null)
                {
                    // Item'ın tipini al
                    MatchType type = item.GetMatchType();
                    
                    // Dictionary'de bu tip yoksa yeni liste oluştur
                    if (!groupedItems.ContainsKey(type))
                    {
                        groupedItems[type] = new List<Item>();
                    }
                    
                    // Listeye ekle ve hücreyi temizle
                    groupedItems[type].Add(item);
                    grid.Cells[x, y].item = null;
                }
            }
        }
        
        return groupedItems;
    }


    private (List<List<Item>> topThreeLists, List<Item> combinedRemaining) ProcessItems()
    {
        // Itemları MatchType'a göre grupla
        Dictionary<MatchType, List<Item>> groupedItems = CollectAndGroupItems();
        
        // Grupları eleman sayısına göre azalan şekilde sırala
        var orderedGroups = groupedItems.Values
                                        .OrderByDescending(list => list.Count)
                                        .ToList();

        // En fazla elemanı olan ilk 3 listeyi al
        List<List<Item>> topThreeLists = orderedGroups.Take(3).ToList();

        // Geri kalanları tek bir listede birleştir
        List<Item> combinedRemaining = orderedGroups.Skip(3)
                                                    .SelectMany(list => list)
                                                    .ToList();

        return (topThreeLists, combinedRemaining);
    }

    private (List<List<Item>> reducedTopThree, List<Item> finalCombinedRemaining) ProcessItemsWithReduction()
    {
        //gruplandırma ve sıralaması yap
        var groupedItems = CollectAndGroupItems();
        var orderedGroups = groupedItems.Values
                                        .OrderByDescending(list => list.Count)
                                        .ToList();
        List<List<Item>> topThreeLists = orderedGroups.Take(3).ToList();
        List<Item> combinedRemaining = orderedGroups.Skip(3)
                                                    .SelectMany(list => list)
                                                    .ToList();

        // Her bir topThree listesini %75-%25 böl
        List<List<Item>> reducedTopThree = new List<List<Item>>();
        List<Item> allTwentyFivePercent = new List<Item>();

        foreach (var list in topThreeLists)
        {
            int totalItems = list.Count;
            float listPercent;

            if(list.Count <4 )listPercent = 1; //eğer komşu sayısı 4 den az ise listeyi bölme
            else listPercent = 0.75f;   //eğer 4den fazla ise yüzde 75 ini al

            int seventyFiveCount = (int)(totalItems * listPercent); // yüzde hesapla

            List<Item> seventyFiveList = list.Take(seventyFiveCount).ToList();
            List<Item> twentyFiveList = list.Skip(seventyFiveCount).ToList();

            reducedTopThree.Add(seventyFiveList);
            allTwentyFivePercent.AddRange(twentyFiveList);
        }

        //Kalanları combinedRemaining ile birleştir
        List<Item> finalCombinedRemaining = combinedRemaining
                                            .Concat(allTwentyFivePercent)
                                            .ToList();

        return (reducedTopThree, finalCombinedRemaining);
    }


    //SmurtShuffle animasyounu için önce merkeze ardından yeni pozisyonunlara Coroutine ile götür
    private IEnumerator ShuffleAnimation(Item item, Vector3 start, Vector2 Center, Vector3 target, float duration)
    {
        yield return StartCoroutine(MoveItemToPosition(item, start, Center, duration));
        yield return StartCoroutine(MoveItemToPosition(item, Center, target, duration));
        shuffling = false;
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