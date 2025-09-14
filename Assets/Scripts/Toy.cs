using UnityEngine;

public class Toy : MonoBehaviour
{
	private ToyData data;
	public ToyData Data { get { return data; } }
	private MoveType moveType;
	public MoveType MoveType { get { return moveType; } }

	public bool IsEnemy { get; set; }

	public bool IsMove { get; set; }

	private void Awake()
	{
		do
		{
			data = DataTableManger.ToyTable.GetRandom();
		} while (data.Movement == 0);

		moveType = (MoveType)data.Movement;
	}

	private void Start()
	{ 
	}
}