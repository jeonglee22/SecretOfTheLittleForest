using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogicManager : MonoBehaviour
{
    private ShopUIManagers uIManagers;
	private ShopButtonFunctions buttonFunctions;

	private int stageId;
	private float unitLimit;
	private float goldLimit;
	private float gold;
	public float Gold { get { return gold; } set { gold = value; } }

	private Deck userDeck;
	private Deck reGetDeck;

	private ToyData choosedData;
	public ToyData ChooosedData 
	{
		get { return choosedData; }
		set
		{
			choosedData = value;

		}
	}

	public List<TouchManager> touchManagers;
	public List<Image> buyBlockImages;

	private void Awake()
	{
		uIManagers = GetComponent<ShopUIManagers>();
		buttonFunctions = GetComponent<ShopButtonFunctions>();
		reGetDeck = new Deck();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		stageId = data.stageId;
		unitLimit = data.unitLimit;
		gold = data.gold;
		userDeck = data.Deck;
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.gold = gold;
		SaveLoadManager.Data.Deck = userDeck;
		SaveLoadManager.Save();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		goldLimit = DataTableManger.SettingTable.Get(Settings.goldLimit);

        uIManagers.SetBuyItems();
		uIManagers.SetGoldText(gold);
		uIManagers.SetCostText();
		uIManagers.SetUnitText(userDeck.GetDeckTotalCount());

		ReloadDeckInPanel();

		buttonFunctions.OnClickChangeColor(true);
		buttonFunctions.OnClickBuy();

		for (int i = 0; i < touchManagers.Count; i++)
		{
			var manager = touchManagers[i];
			buyBlockImages[i].gameObject.SetActive(false);

			int index = i;
			manager.tapFunc = () =>
			{
				var isLimit = false;

				if (uIManagers.Costs[index] > gold)
				{
					uIManagers.SetGoldColor(true);
					isLimit = true;
				}
				if(userDeck.GetDeckTotalCount() == unitLimit)
				{
					uIManagers.SetUnitColor(true);
					isLimit = true;
				}

				if(!isLimit)
				{
					gold -= uIManagers.Costs[index];
					uIManagers.SetGoldText(gold);
					userDeck.AddDeckData(DataTableManger.ToyTable.Get(uIManagers.ChoosedIds[index]));
					uIManagers.SetUnitText(userDeck.GetDeckTotalCount());
					buyBlockImages[index].gameObject.SetActive(true);
				}

			};
		}
	}

    // Update is called once per frame
    void Update()
    {
    }

	public void MoveData(bool isuserDeck, ContentPanelData data)
	{
		var toy = data.ToyData;

		if (isuserDeck)
		{
			gold += data.Cost;
			if (gold >= goldLimit)
			{
				uIManagers.SetGoldColorAtSellLimit();
				gold = goldLimit;
			}
			userDeck.RemoveDeckData(toy);
			reGetDeck.AddDeckData(toy);
		}
		else
		{
			gold -= data.Cost;
			if(gold < 0)
			{
				uIManagers.SetGoldColor(true);
				gold += data.Cost;
				return;
			}
			userDeck.AddDeckData(toy);
			reGetDeck.RemoveDeckData(toy);
		}

		uIManagers.SetGoldText(gold);
		uIManagers.SetUnitText(userDeck.GetDeckTotalCount());
		ReloadDeckInPanel();
	}

	public void ReloadDeckInPanel()
	{
		uIManagers.SetRect(userDeck, true);
		uIManagers.SetRect(reGetDeck, false);
	}
}
