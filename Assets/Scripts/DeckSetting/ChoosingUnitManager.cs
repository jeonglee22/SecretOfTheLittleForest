using System;
using TMPro;
using Unity.VisualScripting;
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

	private void OnDisable()
	{
		SaveLoadManager.Data.Deck = chooseDeck;
		SaveLoadManager.Save(1);
	}

	public void AddToyOnChoosedDeck(GameObject go)
	{
		var newGo = Instantiate(go, chooseContent);
		var toy = newGo.GetComponent<Toy>();
		toy.Data = go.GetComponent<Toy>().Data;
		toy.SetData();

		chooseDeck.AddDeckData(newGo.GetComponent<Toy>().Data);

		Destroy(newGo);

		ReloadDeckImages();
		deckSettingManager.ReduceChoosedToy(toy.Data);

		deckSceneUIManager.SetCostText(chooseDeck.GetDeckTotalCost());
		deckSceneUIManager.SetCountText(chooseDeck.GetDeckTotalCount());
	}

	private bool ReloadDeckImages()
	{
		ClearField();

		if (chooseDeck.Toys.Count == 0)
			return false;

		foreach (var toy in chooseDeck.Toys)
		{
			var count = toy.count;
			var data = toy.data;

			var empty = new GameObject();
			var go = Instantiate(empty, chooseContent);
			Destroy(empty);

			var goToy = go.AddComponent<Toy>();
			var goImage = go.AddComponent<Image>();
			goToy.Data = data;
			goToy.SetData();
			goImage.sprite = goToy.Toy2D;
			if(count > 1)
			{
				var text = Instantiate(countText, go.transform);
				text.alignment = TextAlignmentOptions.BottomRight;
				text.text = $"x{count}";
				text.color = Color.black;
				text.fontStyle = FontStyles.Bold;
				text.enableAutoSizing = true;
				text.rectTransform.anchorMax = new Vector2(1, 0.5f);
				text.rectTransform.anchorMin = new Vector2(0.5f, 0);
			}

			var coinImage = Instantiate(coin, go.transform);
			var image = coinImage.GetComponent<Image>();
			image.sprite = Resources.Load<Sprite>("Icons/coin");
			image.color = Color.yellow;
			image.rectTransform.sizeDelta = Vector2.zero;
			coinImage.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
			coinImage.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);

			var coinTextGo = Instantiate(coinText, coinImage.transform);
			coinTextGo.text = data.Price.ToString();
		}

		return true;
	}

	private void ClearField()
	{
		for(int i = 0; i < chooseContent.childCount; i++)
			Destroy(chooseContent.GetChild(i).gameObject);
	}

	public void RemoveToyInChoosedDeck(GameObject pointerEnter)
	{
		if (pointerEnter != null && pointerEnter.GetComponent<Toy>() != null)
		{
			var toy = pointerEnter.GetComponent<Toy>();
			toy.Data = pointerEnter.GetComponent<Toy>().Data;
			toy.SetData();

			chooseDeck.RemoveDeckData(toy.Data);

			ReloadDeckImages();
			deckSettingManager.AddChoossedToy(toy.Data);

			deckSceneUIManager.SetCostText(chooseDeck.GetDeckTotalCost());
			deckSceneUIManager.SetCountText(chooseDeck.GetDeckTotalCount());
		}
	}

	public void RemoveAllToysInDeck()
	{
		while(chooseDeck.Toys.Count > 0)
		{
			var first = chooseDeck.Toys[0].data;
			chooseDeck.RemoveDeckData(first);
			deckSettingManager.AddChoossedToy(first);
		}

		ReloadDeckImages();
		deckSceneUIManager.SetCostText(chooseDeck.GetDeckTotalCost());
		deckSceneUIManager.SetCountText(chooseDeck.GetDeckTotalCount());
	}
}
