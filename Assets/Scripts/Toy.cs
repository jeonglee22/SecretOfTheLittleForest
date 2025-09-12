using UnityEngine;

public class Toy : MonoBehaviour
{
	private ToyData data;
	private MoveType moveType;
	public MoveType MoveType { get { return moveType; } }

	public bool IsEnemy { get; set; }

	public bool IsMove { get; set; }

	public Vector3 startPosition;
	public Vector3 endPosition;

	private void Start()
	{
		do
		{
			data = DataTableManger.ToyTable.GetRandom();
		} while (data.Movement == 0);

		moveType = (MoveType)data.Movement;
		Debug.Log(moveType);
	}
}
