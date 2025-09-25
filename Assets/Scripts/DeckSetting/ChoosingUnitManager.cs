using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingUnitManager : MonoBehaviour
{
	public ScrollRect chooseRect;
	public int UnitCount { get { return chooseRect.content.childCount; } }
	private RectTransform chooseContent;

	public TextMeshProUGUI coinText;
	public TextMeshProUGUI countText;

	public Image coin;

	private Deck chooseDeck;

	private DeckSettingManager deckSettingManager;
	private DeckSceneUIManager deckSceneUIManager;

	private void Awake()
	{
		chooseDeck = new Deck();
		deckSettingManager = GetComponent<DeckSettingManager>();
		deckSceneUIManager = GetComponent<DeckSceneUIManager>();
	}

	private void Start()
	{
		chooseContent = chooseRect.content;
	}

	public void AddToyOnChoosedDeck(GameObject go)
	{
		var newGo = Instantiate(go, chooseContent);
		var toy = newGo.GetComponent<Toy>();
		toy.Data = go.GetComponent<Toy>().Data;
		toy.SetData();

		chooseDeck.AddDeckData(newGo.GetComponent<Toy>().Data);

		Destroy(newGo);
	}

	public void RemoveAllToysInDeck()
	{
		while(chooseDeck.Toys.Count > 0)
		{
			var first = chooseDeck.Toys[0].data;
			chooseDeck.RemoveDeckData(first);
		}
	}
}
