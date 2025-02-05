using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public TextMesh labelText;

    [HideInInspector] public int X;
    [HideInInspector] public int Y;

    public Item _item;
    public GameGrid gameGrid { get; private set; }

    private void OnMouseDown()
    {
        if (item == null || item.GetMatchType() == MatchType.None) return;

        var matchedCells = MatchingManager.Instance.FindMatches(this, item.GetMatchType());

        if (matchedCells.Count >= 2)
        {
            List<Item> itemsToDestroy = new List<Item>();

            for (int i = 0; i < matchedCells.Count; i++)
            {
                if (matchedCells[i].item != null)
                {
                    itemsToDestroy.Add(matchedCells[i].item);
                }
            }

            StartCoroutine(DestroyItems(itemsToDestroy));


        }
        
        StartCoroutine(FillGrid());
    }

    private IEnumerator FillGrid()//gridin dolmasını bekle
    {
        yield return new WaitForSeconds(2f);
        SmurtShuffle.Instance.ResetMatchCounts();
        SmurtShuffle.Instance.LookInt();
    }

    private IEnumerator DestroyItems(List<Item> items)
    {
        yield return new WaitForEndOfFrame(); // Küplerin düşmesini bekleyebiliriz

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null) items[i].TryExecute();
            //NewBehaviourScript.Instance.SpawnNewCubesAtTop();
            //FallManager.Instance.ApplyGravity();               // bunlar burada mı yoksa ıtem de mi çalışsın bilemedim
        }
    }

    public Item item 
    {
        get
        {
            return _item;
        }
        set
        {
            if (_item == value) return;

            var oldItem = _item;
            _item = value;

            if (oldItem != null && Equals(oldItem.Cell, this))
                oldItem.Cell = null;
            
            if (value != null)
                value.Cell = this;
        }
    }
    public void Prepare(int x, int y,GameGrid board)
    {
        gameGrid = board;
        X = x;
        Y = y;
        transform.localPosition = new Vector3 (x, y);
    }


}
