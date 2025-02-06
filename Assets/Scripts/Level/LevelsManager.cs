using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    private LevelData levelData;
    
    private void Awake() //önce cell ve item sonra hintmanagerda BFS
    {
        PrepareLevel();
    }

    private void PrepareLevel()
    {
        levelData = new LevelData(gameGrid.levelSO);

        for (int i = 0; i < gameGrid.levelSO.grid_height; ++i)
            for (int j = 0; j < gameGrid.levelSO.grid_width; ++j)
            {
                var cell = gameGrid.Cells[j, i];

                var itemType = levelData.ItemTypes[gameGrid.levelSO.grid_height - i-1 , j];
                var item = ItemFactory.Instance.CreateItem(itemType, gameGrid.itemsParent);
                if (item == null) continue;

                cell.item = item;
                item.transform.position = cell.transform.position;

            }
    }

}
