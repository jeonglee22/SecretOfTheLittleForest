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

	public GameObject shownGrid;

	public Toy toy;

	public TextMeshProUGUI countText;

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
		foreach (var toyData in unitDeck.Toys)
		{
			toy.Data = toyData.data;
			toy.SetData();
			var go = new GameObject();
			var image = go.AddComponent<Image>();
			image.sprite = toy.Toy2D;
			var obj = Instantiate(go, unitContent);

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
	}
}
