using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Toy : MonoBehaviour
{
	private ToyData data;
	public ToyData Data { get { return data; } }
	private MoveType moveType;
	public MoveType MoveType { get { return moveType; } }

	public Transform healthTransform;
	public Transform attackTransform;
	public Transform canvas;

	private List<Image> hearts;
	private List<Image> attacks;

	public Image heart;
	public Image attack;

	private int HP { get; set; }
	private int Attack { get; set; }

	public bool IsEnemy { get; set; }

	public bool IsMove { get; set; }

	private void Awake()
	{
		do
		{
			data = DataTableManger.ToyTable.GetRandom();
		} while (data.Movement == 0);

		moveType = (MoveType)data.Movement;
		HP = data.HP;
		Attack = data.Attack;

		hearts = new List<Image>();
		attacks = new List<Image>();

		for(int i = 0; i < HP; i++)
		{
			hearts.Add(Instantiate(heart, healthTransform));
		}
		for(int i = 0; i < Attack; i++)
		{
			attacks.Add(Instantiate(attack,attackTransform));
		}

		canvas.rotation = Camera.main.transform.rotation;
		SetActiveInfoCanvas(false);
	}

	private void Start()
	{ 
	}

	public void SetActiveInfoCanvas(bool active)
	{
		for (int i = 0; i < HP; i++)
		{
			hearts[i].gameObject.SetActive(active);
		}
		for (int i = 0; i < Attack; i++)
		{
			attacks[i].gameObject.SetActive(active);
		}
	}
}