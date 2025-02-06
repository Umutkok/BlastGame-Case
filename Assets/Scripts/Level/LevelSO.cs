using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="LevelSO", menuName ="newLevel")]
//LevelInfo is a ScriptableObject that is used to store the information of a level.
public class LevelSO : ScriptableObject
{
    /* Level oluşturmamız için ScriptableObject*/
    public int level_number;
    public int grid_width;
    public int grid_height;
    public string[] grid;
}

