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
    Moved,
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
    public static readonly string AI = "AITable";
    public static readonly string Setting = "Settings";
    public static readonly string Stage = "StageCombinations";
}

public static class ResourceObjectIds
{
	public static readonly string Toy = "Toys";
}

public static class Variables
{
	public static Languages Language = Languages.Korean;
}

public static class AINames
{
    public static readonly string[] AINameList =
    {
        runAI, atkSaveAI, moveSaveAI , atkAI, moveAI, defMove, randomMove, hateMove
    };

	public const string runAI = "run_ai";
    public const string atkSaveAI = "atk_save_ai";
    public const string moveSaveAI = "move_save_ai";
    public const string atkAI = "atk_ai";
    public const string moveAI = "move_ai";
    public const string defMove = "def_move";
    public const string randomMove = "random_move";
    public const string hateMove = "hate_move";
}

public static class Settings
{
    public const string moveCount = "Move_count";
}

public static class Tags
{
    public static readonly string BillBoard = "BillBoard";
}

public enum Windows
{
    LobbyMain,
}

public enum Scenes
{
	Lobby,
	DeckSetting,
	Game,

    Test,
}
