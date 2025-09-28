using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButtonFunctions : MonoBehaviour
{
	private float unitCount;

	public GameObject buyPanel;
	public GameObject sellPanel;

	public GameObject decisionPanel;
	public TextMeshProUGUI decisionText;
	private Action acceptFunc;

	private int reloadCost;

	private ShopUIManagers shopUIManagers;
	private ShopLogicManager logicManager;
	public GameObject reloadButton;
	public GameObject costText;

	private void Awake()
	{
		shopUIManagers = GetComponent<ShopUIManagers>();
		logicManager = GetComponent<ShopLogicManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		unitCount = data.unitCount;
	}

	private void Start()
	{
		reloadCost = Mathf.FloorToInt(DataTableManger.SettingTable.Get(Settings.battleGold));
	}

	public void OnClickBuy()
	{
		buyPanel.SetActive(true);
		sellPanel.SetActive(false);
		reloadButton.SetActive(true);
		costText.SetActive(true);
	}

	public void OnClickExitShop()
	{
		decisionText.text = "상점에서 나가시겠습니까?";
		acceptFunc = () => SceneManager.LoadScene((int)Scenes.StageChoosing);
		decisionPanel.SetActive(true);
	}

	public void OnClickAccpet()
	{
		decisionPanel.SetActive(false);
		SaveLoadManager.Save();
		acceptFunc();
	}

	public void OnClickReject()
	{
		decisionPanel.SetActive(false);
	}

	public void OnClickSell()
	{
		buyPanel.SetActive(false);
		sellPanel.SetActive(true);
		reloadButton.SetActive(false);
		costText.SetActive(false);
		logicManager.ReloadDeckInPanel();
	}

	public void OnClickChangeColor(bool isBuyButton)
	{
		shopUIManagers.SetButtonColor(isBuyButton);
	}

	public void OnClickReloadElements()
	{		
		if (shopUIManagers.IsFree)
		{
			shopUIManagers.IsFree = false;
		}
		else
		{
			logicManager.Gold -= reloadCost;

			if (logicManager.Gold < 0 && !shopUIManagers.IsFree)
			{
				shopUIManagers.SetGoldColor(true);
				logicManager.Gold += reloadCost;
				return;
			}

			SaveLoadManager.Data.gold -= reloadCost;
		}

		logicManager.buyBlockImages.ForEach(x => x.gameObject.SetActive(false));

		shopUIManagers.SetBuyItems();
		shopUIManagers.SetGoldText(logicManager.Gold);
		shopUIManagers.SetCostText();
	}
}
