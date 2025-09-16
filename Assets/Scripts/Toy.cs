using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Toy : MonoBehaviour
{
	private ToyData data;
	public ToyData Data { get { return data; } set { data = value; } }
	private MoveType moveType;
	public MoveType MoveType { get { return moveType; } }

	public Transform healthTransform;
	public Transform attackTransform;
	public Transform canvas;

	private List<Image> hearts;
	private List<Image> attacks;

	public Image heart;
	public Image attack;

	private Color damagedColor = new Color(0.5f, 0.5f, 0.5f);

	public int HP { get; private set; }
	public int Attack { get;private set; }

	public bool IsEnemy { get; set; }

	public bool IsMove { get; set; }

	private void Awake()
	{
	}

	public void Init()
	{
		Instantiate(GameObjectManager.ToyResource.Get(data.ModelCode), transform);
		moveType = (MoveType)data.Movement;
		HP = data.HP;
		Attack = data.Attack;

		hearts = new List<Image>();
		attacks = new List<Image>();

		for (int i = 0; i < HP; i++)
		{
			hearts.Add(Instantiate(heart, healthTransform));
		}
		for (int i = 0; i < Attack; i++)
		{
			attacks.Add(Instantiate(attack, attackTransform));
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

	public bool GetDamageAndAlive(int damage)
	{
		for (int i = HP-1; i >= Mathf.Max(HP - damage,0); i--)
			hearts[i].gameObject.GetComponent<Image>().color = damagedColor;
		
		HP -= damage;

		if (HP <= 0)
		{
			Die();
			return false;
		}
		return true;
	}

	public void Die()
	{
		Destroy(gameObject);
	}

	public void LookCamera()
	{
		var rotation = transform.rotation;
		canvas.LookAt(Camera.main.transform);
		rotation.x = canvas.rotation.x;
		rotation.z = canvas.rotation.z;
		canvas.rotation = rotation;
	}
}