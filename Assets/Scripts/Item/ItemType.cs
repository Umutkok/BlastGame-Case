[System.Serializable]
public enum ItemType
{
    //itemlerin türleri ve conditionları bazı noktalarda rastgelelik durumu olduğu için default itemtype ların sırası önemli
    //conditionlar item yan yana geldiğinde gözükecek sprite için
    None,
    RedCube,
    GreenCube,
    YellowCube,
    BlueCube,
    PinkCube,
    PurpleCube,

    //leveldata içinde random atıldığı için sırası önemli unutma
    ConditionA,
    ConditionB,
    ConditionC,

}
