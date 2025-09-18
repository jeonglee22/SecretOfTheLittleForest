using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

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

	public Toy toy;
	public Image icon;
	public Image outline;
	public TextMeshProUGUI valueText;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		unitContent = unitRect.content;

		heartSprite = Resources.Load<Sprite>(heartPath);
		attackSprite = Resources.Load<Sprite>(attackPath);

		UnitSettingOnBoard();
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

		foreach (var toyData in unitDeck.Toys)
		{
			toy.Data = toyData.data;
			toy.SetData();
			var go = new GameObject();
			var toyComp = go.AddComponent<Toy>();
			var image = go.AddComponent<Image>();
			image.sprite = outline.sprite;
			image.type = Image.Type.Sliced;
			image.fillCenter = true;

			var obj = Instantiate(go, unitContent);
			obj.GetComponent<Toy>().Data = toyData.data;
			obj.GetComponent<Toy>().SetData();

			var imageGO = new GameObject();
			var img = imageGO.AddComponent<Image>();
			img.sprite = toy.Toy2D;

			Instantiate(imageGO, obj.transform);
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

			Destroy(go);
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
		text.text = go.GetComponent<Toy>().HP.ToString();
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
		text.text = go.GetComponent<Toy>().Attack.ToString();
	}
}
