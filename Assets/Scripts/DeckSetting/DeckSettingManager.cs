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

	public Toy toy;

	public TextMeshProUGUI countText;

	private Vector2 cellSize = new Vector2(110, 110);

	private void Awake()
	{
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
				text.fontSize = 40;
				text.color = Color.black;
				text.fontStyle = FontStyles.Bold;
			}

			Destroy(go);
		}
		Instantiate(emptyGo, unitContent);
		Instantiate(emptyGo, unitContent);
		Destroy(emptyGo);
	}

	public void OnValueChange(Vector2 vec)
	{

	}
}
