using UnityEngine;
/// <summary>
/// The LevelManager class is responsible for managing the game level. It initializes the game grid, fall and fill manager, moves manager, and goal manager.
/// It also prepares the level by creating items for each cell in the game grid based on the level data.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    //[SerializeField] private FallAndFillManager fallAndFillManager;
    //[SerializeField] private GoalManager goalManager;
    //[SerializeField] private MovesManager movesManager;
    private LevelData levelData;
    
    private void Awake() //önce cell ve item sonra hintmanagerda BFS
    {
        PrepareLevel();
        InitFallAndFills(); 
    }

    private void PrepareLevel()
    {
        levelData = new LevelData(gameGrid.levelInfo);

        for (int i = 0; i < gameGrid.levelInfo.grid_height; ++i)
            for (int j = 0; j < gameGrid.levelInfo.grid_width; ++j)
            {
                var cell = gameGrid.Cells[j, i];

                var itemType = levelData.GridData[gameGrid.levelInfo.grid_height - i-1 , j];
                var item = ItemFactory.Instance.CreateItem(itemType, gameGrid.itemsParent);
                if (item == null) continue;

                cell.item = item;
                item.transform.position = cell.transform.position;

            }
    }

    private void InitFallAndFills()
    {
        //FallAndFillManager.Instance.Init(gameGrid, levelData);
    }
}
