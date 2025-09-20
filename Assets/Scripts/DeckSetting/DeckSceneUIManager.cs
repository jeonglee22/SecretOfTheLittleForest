using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneUIManager : MonoBehaviour
{
	public ChoosingUnitManager settingManager;
	public TextMeshProUGUI costText;
	public TextMeshProUGUI countText;
	public TextMeshProUGUI descriptionText;

	private int costMax;
	private bool isNotCorrectCost = false;
	private bool isNotCorrectCount = false;

	public GameObject popupPanel;

	private Action acceptFunc;

	public void OnClickBack()
	{
		descriptionText.text = "로비로 돌아가시겠습니까?";
		acceptFunc = () => SceneManager.LoadScene((int)Scenes.Lobby);
		popupPanel.SetActive(true);
	}

	private void Start()
	{
		SetCostLevel(0);
		SetCostText(0);
		SetCountText(0);
		popupPanel.SetActive(false);
	}

	public void OnClickStart()
	{
		if (settingManager.UnitCount == 0 || isNotCorrectCost || isNotCorrectCount)
		{
			return;
		}
		descriptionText.text = "게임을 시작하시겠습니까?";
		acceptFunc = () => SceneManager.LoadScene((int)Scenes.Game);
		popupPanel.SetActive(true);
	}

	public void OnClickReset()
	{
		descriptionText.text = "선택한 목록을\n비우시겠습니까?";
		acceptFunc = () => settingManager.RemoveAllToysInDeck();
		popupPanel.SetActive(true);
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

	public void OnClickAccpet()
	{
		acceptFunc();
		popupPanel.SetActive(false);
	}

	public void OnClickReject()
	{
		popupPanel.SetActive(false);
	}
}
