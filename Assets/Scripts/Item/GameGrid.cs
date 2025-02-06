using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Transform cellsParent;
    public Transform itemsParent;
    [SerializeField] private Cell cellPrefab;


    public LevelSO levelSO;

    public int Rows { get; private set; }
    public int Cols { get; private set; }

    public Cell[,] Cells { get; private set; }

    public Vector3 Center;

    public static GameGrid Instance;
    private void Awake()
    {
        Instance = this;
        LoadLevelInfo();
        InitializeCells();
        PrepareCells();
        Center = new Vector3((float)levelSO.grid_height/2, (float)levelSO.grid_width/2, 0);
        
    }

    private void LoadLevelInfo()
    {
        Rows = levelSO.grid_height;
        Cols = levelSO.grid_width;
    }

    private void InitializeCells()
    {
        Cells = new Cell[Cols, Rows];
        ResizeBoard(Rows, Cols);
        CreateCells();
    }

    private void CreateCells()
    {
        for(int y = 0; y < Rows; y++)
            for (int x = 0; x < Cols; x++)
                Cells[x, y] = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, cellsParent);
    }

    private void PrepareCells()
    {
        for (int y = 0; y < Rows; y++)
            for(int x= 0; x < Cols; x++)
                Cells[x, y].Prepare(x, y, this);
    }


    private void ResizeBoard(int rows, int cols)
    {
        Transform currTrans = this.transform;

        float newX = (9 - cols) * 0.5f;
        float newY = (9 - rows) * 0.5f;


        this.transform.position = new Vector3(newX, newY, currTrans.position.z);

    }

    public Cell GetCell(int x, int y)
{
    if (x < 0 || x >= Cols || y < 0 || y >= Rows) return null;
    return Cells[x, y]; // cells, tüm hücreleri tutan 2D array
}
}