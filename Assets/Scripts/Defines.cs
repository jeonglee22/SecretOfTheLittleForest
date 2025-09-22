using UnityEngine;

public enum JsonFileNum
{
    UserTotalDeck,
    StageDeck,
    StageInfo,
}

public enum PlayTurn
{
    None = -1,
    Enemy,
    Player,
    EliteEnemy,
}

public enum BattleType
{
    Normal,
    Elite,
    Boss,
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
    ReadyMove,
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
    public static readonly string BossStage = "BossStageCombinations";
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
    public const string unitCount = "Unit_count";
    public const string battleTurnCount = "Battle_Turn";
    public const string costLimit0 = "Cost_lim";
    public const string costLimit1 = "Cost_lim_1";
    public const string costLimit2 = "Cost_lim_2";
    public const string costLimit3 = "Cost_lim_3";
    public const string unitLimit = "Unit_lim";
    public const string eliteBattle = "Elite_spon";
    public const string bossCount = "Boss_count";
    public const string bossWeight = "Boss_weight";
    public const string shopRoom = "Shop_spon";
    public const string randomRoom = "Random_spon";
    public const string emptyRoom = "Empty_spon";
    public const string normalBattle = "Battle_spon";
    public const string goldLimit = "Gold_lim";
}

public static class Tags
{
    public static readonly string BillBoard = "BillBoard";
    public static readonly string Trigger = "Trigger";
    public static readonly string BoardManager = "BoardManager";
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

public enum GameWindow
{
    MainGame,
    DeckSetting,

}
