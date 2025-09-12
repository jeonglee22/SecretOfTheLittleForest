using UnityEngine;

public enum MoveType
{
    Pawn,
    King,
    Knight,
    Bishop,
    Rook,
    Queen,
}

public enum NodeState
{
    None,
    Enemy,
    EnemyMove,
    Player,
    PlayerMove,
    Attack,
}

public class LayerId
{
    public static readonly int node = LayerMask.GetMask("Node");
    public static readonly int ui = LayerMask.GetMask("UI");
}

public enum Languages
{
	Korean,
	English,
	Japanese,
}

public static class DataTableIds
{
	public static readonly string[] StringTableIds =
	{
		"StringTableKr",
		"StringTableEn",
		"StringTableJp",
	};
	public static string String => StringTableIds[(int)Variables.Language];

	public static readonly string Toy = "ToyTable";
}


public static class Variables
{
	public static Languages Language = Languages.Korean;
}


