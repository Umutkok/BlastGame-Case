using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //Cell içinde item tutar ve eğer yeni bir item gelirse eskisi ile değiştirir
    //collision ekli olduğu için OnMouseDown ile ınput alınır. Oyunda tek tür item olduğu için Input System kullanmadım

    [HideInInspector] public int X;
    [HideInInspector] public int Y;

    public Item _item;
    public GameGrid gameGrid { get; private set; }


    private void OnMouseDown()
    {
        if (item == null || item.GetMatchType() == MatchType.None || SmurtShuffle.Instance.shuffling ==true) return; //shuffle halinde ise veya null ise return

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

            for (int i = 0; i < itemsToDestroy.Count; i++)
            {
                if (itemsToDestroy[i] != null) itemsToDestroy[i].TryExecute();
            }


        }
        
        StartCoroutine(FillGrid());
    }

    private IEnumerator FillGrid()//gridin dolmasını bekle
    {
        yield return new WaitForSeconds(2f);
        SmurtShuffle.Instance.ResetMatchCounts();
        SmurtShuffle.Instance.LookInt();
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
