using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneUIManager : MonoBehaviour
{
	public ChoosingUnitManager settingManager;
	public TextMeshProUGUI costText;
	public TextMeshProUGUI countText;

	private int costMax;
	private bool isNotCorrectCost = false;
	private bool isNotCorrectCount = false;

	public void OnClickBack()
	{
		SceneManager.LoadScene((int)Scenes.Lobby);
	}

	private void Start()
	{
		SetCostLevel(0);
		SetCostText(0);
		SetCountText(0);
	}

	public void OnClickStart()
	{
		if (settingManager.UnitCount == 0 || isNotCorrectCost || isNotCorrectCount)
		{
			return;
		}
		SceneManager.LoadScene((int)Scenes.Game);
	}

	public void OnClickReset()
	{
		SceneManager.LoadScene((int)Scenes.DeckSetting);
	}

	public void SetCostText(int cost)
	{
		costText.text = $"{cost}/{costMax}";
		if (cost > costMax)
			costText.color = Color.red;
		else
			costText.color = Color.white;
		isNotCorrectCost = cost > costMax;
	}

	public void SetCostLevel(int level)
	{
		var cost = level switch
		{
			0 => DataTableManger.SettingTable.Get(Settings.costLimit0),
			1 => DataTableManger.SettingTable.Get(Settings.costLimit1),
			2 => DataTableManger.SettingTable.Get(Settings.costLimit2),
			3 => DataTableManger.SettingTable.Get(Settings.costLimit3),
			_ => throw new System.Exception("Wrong Input")
		};
		costMax = Mathf.FloorToInt(cost);
	}

	public void SetCountText(int count)
	{
		int limitCount = Mathf.FloorToInt(DataTableManger.SettingTable.Get(Settings.unitLimit));
		countText.text = $"{count}/{limitCount}";
		if (count > limitCount)
			countText.color = Color.red;
		else
			countText.color = Color.white;
		isNotCorrectCount = count > limitCount;
	}
}
