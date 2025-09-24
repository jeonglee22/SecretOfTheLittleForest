using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButtonFunctions : MonoBehaviour
{
	private float unitCount;

	public GameObject buyPanel;
	public GameObject sellPanel;

	private int reloadCost = 4;

	private ShopUIManagers shopUIManagers;

	private void Awake()
	{
		shopUIManagers = GetComponent<ShopUIManagers>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		unitCount = data.unitCount;
	}

	public void OnClickBuy()
	{
		buyPanel.SetActive(true);
		sellPanel.SetActive(false);
	}

	public void OnClickExitShop()
	{
		SaveLoadManager.Save();
		SceneManager.LoadScene((int)Scenes.StageChoosing);
	}

	public void OnClickSell()
	{
		buyPanel.SetActive(false);
		sellPanel.SetActive(true);
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
			shopUIManagers.Gold -= reloadCost;

			if (shopUIManagers.Gold < reloadCost && !shopUIManagers.IsFree)
			{
				shopUIManagers.SetGoldColor(true);
				shopUIManagers.Gold += reloadCost;
				return;
			}

			SaveLoadManager.Data.gold -= 4;
		}

		shopUIManagers.SetBuyItems();
		shopUIManagers.SetGoldText(shopUIManagers.Gold);
		shopUIManagers.SetCostText();
	}
}
