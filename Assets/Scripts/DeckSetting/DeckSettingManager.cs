using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeckSettingManager : MonoBehaviour
{
	public ScrollRect unitRect;
	private RectTransform unitContent;
	public ScrollRect chooseRect;

	private Deck unitDeck;
	private Deck choosedDeck;
	private StatShowManager statShowManager;

	public Toy toy;

	public TextMeshProUGUI countText;

	private Vector2 cellSize = new Vector2(110, 110);

	private float lastChangedTime;
	private bool isValueChange;
	private float currentValue;
	private float clampTimeInterval = 0.5f;
	private float speed = 0.5f;
	private float offset = 0.01f;

	private void Awake()
	{
		statShowManager = GetComponent<StatShowManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		unitDeck = SaveLoadManager.Data.Deck;
		unitDeck.LoadDeckData();
	}

	private void OnDisable()
	{
		SaveLoadManager.Save();
		SaveLoadManager.Data.Deck = unitDeck;
	}

	private void Start()
	{
		unitContent = unitRect.content;

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

		SetFirstImageOnView();
	}

	private void Update()
	{
		SetCellSize();

		if (isValueChange && Time.time - lastChangedTime > clampTimeInterval)
		{
			int minIndex = -1;
			float minDis = float.MaxValue;

			var count = unitDeck.Toys.Count;
			for (int i = 0; i < count; i++)
			{
				var interval = 1f / (count - 1) * (float)i;
				if (Mathf.Abs(interval - currentValue) < minDis) 
				{ 
					minDis = Mathf.Abs(interval - currentValue);
					minIndex = i;
				}
			}

			StartCoroutine(CoMove(count, minIndex));

		}
	}

	private void SetCellSize()
	{
		var content = unitRect.content;
		var gridgroup = content.GetComponent<GridLayoutGroup>();
		var cellsize = content.parent.GetComponent<RectTransform>().rect.height;
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

	private IEnumerator CoMove(int count, int index)
	{
		float ratio = 0f;
		float pos = 1f / (count - 1) * (float)index;
		while (Mathf.Abs(pos - unitRect.horizontalNormalizedPosition) > offset)
		{
			var movePos = Mathf.Lerp(currentValue, pos, ratio);
			yield return new WaitForSeconds(0.001f);
			unitRect.horizontalNormalizedPosition = movePos;
			ratio += Time.deltaTime * speed;
		}
		unitRect.horizontalNormalizedPosition = pos;
		isValueChange = false;
	}
}
