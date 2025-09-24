using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButtonFunctions : MonoBehaviour
{
	private float gold;
	private float unitCount;

	public GameObject buyPanel;
	public GameObject sellPanel;

	private ShopUIManagers shopUIManagers;

	private void Awake()
	{
		shopUIManagers = GetComponent<ShopUIManagers>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;

		gold = data.gold;
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
		shopUIManagers.SetBuyItems();

		if (shopUIManagers.IsFree)
		{
			shopUIManagers.IsFree = false;
		}
		else
		{
			gold -= 4;
			SaveLoadManager.Data.gold -= 4;
		}
		shopUIManagers.SetGoldText(gold);
	}
}
