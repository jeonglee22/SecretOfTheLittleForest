using UnityEngine;

public enum PlayTurn
{
    None = -1,
    Enemy,
    Player,
}

public enum MoveType
{
    Pawn,
    King,
    Knight,
    Bishop,
    Rook,
    Queen,
    None,
}

public enum NodeState
{
    None,
    Enemy,
    EnemyMove,
    Player,
    PlayerMove,
    Attack,
    Choose,
}

public class LayerId
{
    public static readonly int node = LayerMask.GetMask("Node");
    public static readonly int ui = LayerMask.GetMask("UI");
    public static readonly int toy = LayerMask.GetMask("Toy");
    public static readonly int ground = LayerMask.GetMask("Ground");
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


