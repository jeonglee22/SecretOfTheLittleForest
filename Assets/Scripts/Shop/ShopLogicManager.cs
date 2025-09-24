using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogicManager : MonoBehaviour
{
    private ShopUIManagers uIManagers;
	private ShopButtonFunctions buttonFunctions;

	private int stageId;
	private float unitLimit;
	private float goldLimit;
	public float Gold { get; set; }

	private Deck userDeck;
	private Deck reGetDeck;

	public ToyData ChooosedData { get; set; }

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
		Gold = data.gold;
		userDeck = data.Deck;
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.gold = Gold;
		SaveLoadManager.Data.Deck = userDeck;
		SaveLoadManager.Save();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        uIManagers.SetBuyItems();
		uIManagers.SetGoldText(Gold);
		uIManagers.SetCostText();
		uIManagers.SetUnitText(userDeck.GetDeckTotalCount());

		uIManagers.SetRect(userDeck, true);
		uIManagers.SetRect(reGetDeck, false);

		buttonFunctions.OnClickChangeColor(true);

		for (int i = 0; i < touchManagers.Count; i++)
		{
			var manager = touchManagers[i];
			buyBlockImages[i].gameObject.SetActive(false);

			int index = i;
			manager.tapFunc = () =>
			{
				if (uIManagers.Costs[index] > Gold)
				{
					uIManagers.SetGoldColor(true);
				}
				else if(userDeck.GetDeckTotalCount() == unitLimit)
				{
					uIManagers.SetUnitColor(true);
				}
				else
				{
					Gold -= uIManagers.Costs[index];
					uIManagers.SetGoldText(Gold);
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

	public void MoveDataTo(bool isuserDeck)
	{
		if (isuserDeck)
		{

		}
		else
		{

		}
	}
}
