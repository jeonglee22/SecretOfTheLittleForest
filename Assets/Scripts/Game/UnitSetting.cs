using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSetting : MonoBehaviour
{
	public ScrollRect unitRect;
	private RectTransform unitContent;

	public TextMeshProUGUI countText;

	private Deck unitDeck;

	private Sprite heartSprite;
	private string heartPath = "Icons/card-hearts";
	private Sprite attackSprite;
	private string attackPath = "Icons/battle";

	public GameObject toy2D;
	public GameObject toy;
	public Image icon;
	public Image outline;
	public TextMeshProUGUI valueText;

	public List<Node> playerStartNodes;

	public CameraManager cameraManager;
	public ReadyCanvasManager readyCanvasManager;

	private int count = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		unitContent = unitRect.content;

		heartSprite = Resources.Load<Sprite>(heartPath);
		attackSprite = Resources.Load<Sprite>(attackPath);

		UnitSettingOnBoard();

		cameraManager.SetCameraToSettingPos();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load(1);
		unitDeck = SaveLoadManager.Data.Deck;
	}

	// Update is called once per frame
	void Update()
    {
    }

	private void UnitSettingOnBoard()
	{
		for (int i = 0; i < unitContent.childCount; i++)
			Destroy(unitContent.GetChild(i).gameObject);

		var toyComp = toy2D.GetComponent<Toy>();
		foreach (var toyData in unitDeck.Toys)
		{
			toyComp.Data = toyData.data;
			toyComp.SetData();

			var obj = Instantiate(toy2D, unitContent);
			var objToy = obj.GetComponent<Toy>();
			objToy.Data = toyComp.Data;
			objToy.SetData();
			objToy.IsEnemy = false;

			var imageGO = new GameObject();
			var img = imageGO.AddComponent<Image>();
			img.sprite = toyComp.Toy2D;
			var drag = imageGO.AddComponent<DragObject>();
			drag.playerStartNodes = playerStartNodes;
			drag.spawnObj = toy;

			var oj = Instantiate(imageGO, obj.transform);
			oj.GetComponent<DragObject>().dragSucessFunc =
				(toyData) =>
				{
					ReduceCount(toyData);
					UnitSettingOnBoard();
				};

			Destroy(imageGO);

			if (toyData.count != 1)
			{
				var text = Instantiate(countText, obj.transform);
				text.alignment = TextAlignmentOptions.BottomRight;
				text.text = $"x{toyData.count}";
				text.color = Color.black;
				text.fontStyle = FontStyles.Bold;
				text.enableAutoSizing = true;
				text.rectTransform.anchorMax = new Vector2(1, 0.4f);
				text.rectTransform.anchorMin = new Vector2(0.6f, 0);
			}

			SetHeartImage(obj);
			SetAttackImage(obj);
		}
	}

	private void SetHeartImage(GameObject go)
	{
		var heartIcon = Instantiate(icon, go.transform);
		var image = heartIcon.GetComponent<Image>();
		image.sprite = heartSprite;
		image.color = Color.red;
		image.rectTransform.sizeDelta = Vector2.zero;
		heartIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.7f);
		heartIcon.GetComponent<RectTransform>().anchorMax = new Vector2(0.3f, 1f);

		var text = Instantiate(valueText, heartIcon.transform);
		text.text = go.GetComponent<Toy>().Data.HP.ToString();
	}

	private void SetAttackImage(GameObject go)
	{
		var attackIcon = Instantiate(icon, go.transform);
		var image = attackIcon.GetComponent<Image>();
		image.sprite = attackSprite;
		image.color = Color.blue;
		image.rectTransform.sizeDelta = Vector2.zero;
		attackIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
		attackIcon.GetComponent<RectTransform>().anchorMax = new Vector2(0.3f, 0.3f);

		var text = Instantiate(valueText, attackIcon.transform);
		text.text = go.GetComponent<Toy>().Data.Attack.ToString();
	}

	private void ReduceCount(ToyData data)
	{
		var toys = unitDeck.Toys;
		var datas = toys.ConvertAll(x => x.data);
		if (datas.Contains(data))
		{
			var pair = toys[datas.IndexOf(data)];
			pair.count = pair.count - 1;
			if (pair.count > 0)
			{
				unitDeck.Toys[datas.IndexOf(data)] = pair;
			}
			else
			{
				unitDeck.Toys.RemoveAt(datas.IndexOf(data));
			}
		}

		readyCanvasManager.SetUnitCountText(++count);
	}
}
