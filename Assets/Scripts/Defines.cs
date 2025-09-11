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
}
