using UnityEngine;

public class Toy : MonoBehaviour
{
	private ToyData data;
	private MoveType moveType;
	public MoveType MoveType { get { return moveType; } set { moveType = value; } }

	public bool IsEnemy { get; set; }

	public bool IsMove { get; set; }

	public Vector3 startPosition;
	public Vector3 endPosition;

	private void Start()
	{
		data = DataTableManger.ToyTable.GetRandom();
		moveType = (MoveType)data.Movement;
	}
}
