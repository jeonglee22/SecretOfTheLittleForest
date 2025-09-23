using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckSettingManager : MonoBehaviour, IPointerClickHandler
{
	public ScrollRect unitRect;
	private RectTransform unitContent;

	private Deck unitDeck;
	private StatShowManager statShowManager;
	private ChoosingUnitManager choosingUnitManager;

	public Toy toy;

	private GameObject centerToy;

	public TextMeshProUGUI countText;

	private Vector2 cellSize = new Vector2(110, 110);

	private float lastChangedTime;
	private bool isValueChange;
	private float currentValue;
	private float clampTimeInterval = 0.5f;
	//private bool isClicking = false;

	private void Awake()
	{
		statShowManager = GetComponent<StatShowManager>();
		choosingUnitManager = GetComponent<ChoosingUnitManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		unitDeck = SaveLoadManager.Data.Deck;
		unitDeck.LoadDeckData();
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.Deck = unitDeck;
		SaveLoadManager.Save();
	}

	private void Start()
	{
		unitContent = unitRect.content;

		SetDeckInfos();

		SetFirstImageOnView();
	}

	public void SetDeckInfos()
	{
		for(int i = 0; i < unitContent.childCount; i++)
			Destroy(unitContent.GetChild(i).gameObject);

		var emptyGo = new GameObject();
		var emptyImage = emptyGo.AddComponent<Image>();
		emptyImage.rectTransform.sizeDelta = cellSize;
		emptyImage.color = new Color(0, 0, 0, 0);
		Instantiate(emptyGo, unitContent);
		Instantiate(emptyGo, unitContent);

		foreach (var toyData in unitDeck.Toys)
		{
			toy.Data = toyData.data;
			toy.SetData();
			var go = new GameObject();
			var toyComp = go.AddComponent<Toy>();
			var image = go.AddComponent<Image>();
			var collider = go.AddComponent<BoxCollider2D>();
			image.sprite = toy.Toy2D;
			collider.size = cellSize;
			collider.isTrigger = true;

			var obj = Instantiate(go, unitContent);
			obj.GetComponent<Toy>().Data = toyData.data;
			obj.GetComponent<Toy>().SetData();

			if (toyData.count != 1)
			{
				var text = Instantiate(countText, obj.transform);
				text.alignment = TextAlignmentOptions.BottomRight;
				text.text = $"x{toyData.count}";
				text.color = Color.black;
				text.fontStyle = FontStyles.Bold;
				text.enableAutoSizing = true;
				text.rectTransform.anchorMax = new Vector2(1, 0.5f);
				text.rectTransform.anchorMin = new Vector2(0.5f, 0);
			}

			Destroy(go);
		}

		Instantiate(emptyGo, unitContent);
		Instantiate(emptyGo, unitContent);
		Destroy(emptyGo);
	}

	private void Update()
	{
		SetCellSize();

		if (isValueChange && Time.time - lastChangedTime > clampTimeInterval)
		{
			int minIndex = -1;
			float minDis = float.MaxValue;

			var count = unitDeck.Toys.Count;

			if (count == 1)
			{
				unitRect.horizontalNormalizedPosition = 0;
				isValueChange = false;
				return;
			}

			for (int i = 0; i < count; i++)
			{
				var interval = 1f / (count - 1) * (float)i;
				if (Mathf.Abs(interval - currentValue) < minDis) 
				{ 
					minDis = Mathf.Abs(interval - currentValue);
					minIndex = i;
				}
			}

			unitRect.horizontalNormalizedPosition = 1f / (count - 1) * (float)minIndex;
			isValueChange = false;
		}
	}

	private void SetCellSize()
	{
		var content = unitRect.content;
		var gridgroup = content.GetComponent<GridLayoutGroup>();
		var cellsize = (unitRect.gameObject.GetComponent<RectTransform>().rect.width - 40f) / 5f;
		cellSize = new Vector2(cellsize, cellsize);
		gridgroup.cellSize = cellSize;

		foreach (var collider in unitRect.content.GetComponentsInChildren<BoxCollider2D>())
		{
			collider.size = cellSize;
		}
	}

	private void SetFirstImageOnView()
	{
		var firstData = unitDeck.Toys[0].data;
		toy.Data = firstData;
		toy.SetData();
		statShowManager.SetGridImage(toy.Toy2D);
	}

	public void OnValueChange(Vector2 vec)
	{
		lastChangedTime = Time.time;
		isValueChange = true;
		currentValue = vec.x;
	}

	public void AddChoossedToy(ToyData data)
	{
		unitDeck.AddDeckData(data);
		SetDeckInfos();
	}

	public void ReduceChoosedToy(ToyData data)
	{
		unitDeck.RemoveDeckData(data);
		SetDeckInfos();

		if(unitDeck.Toys.Count == 0)
		{
			statShowManager.RemoveGridImage();
			centerToy = null;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (centerToy != null && eventData.pointerEnter == centerToy)
		{
			choosingUnitManager.AddToyOnChoosedDeck(centerToy);
		}
		else if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Toy>() != null &&
			Input.GetTouch(0).position.x < Screen.width * 0.5f)
		{
			choosingUnitManager.AddToyOnChoosedDeck(eventData.pointerEnter);
		}
		else if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Toy>() != null &&
			Input.GetTouch(0).position.x > Screen.width * 0.5f)
		{
			choosingUnitManager.RemoveToyInChoosedDeck(eventData.pointerEnter);
		}
	}
}
