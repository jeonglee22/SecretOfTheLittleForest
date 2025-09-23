using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EliteWinUIManager : MonoBehaviour
{
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI unitText;
	public TextMeshProUGUI winText;

	public List<TouchManager> touchManagers;

	public float Gold { get; set; }
	private float unitLimit;
	private float unitCount;

	private void OnEnable()
	{
		if (SaveLoadManager.Load())
		{
			SaveLoadManager.Save();
			SaveLoadManager.Load();
		}
		var data = SaveLoadManager.Data;
		unitLimit = data.unitLimit;
		unitCount = data.unitCount;
		Gold = data.gold;

		SetGoldText();
		SetUnitText(data.Deck.GetDeckTotalCount());
		//SetUnitText(12);
		SettingTouchFunction();
	}

	private void OnDisable()
	{
		SaveLoadManager.Save();
	}

	private void SetGoldText()
	{
		var goldLimit = DataTableManger.SettingTable.Get(Settings.goldLimit);
		goldText.text = $"({Gold}/{goldLimit})";
	}

	public void SetUnitText(int unitCount)
	{
		unitText.text = $"({unitCount}/{unitLimit})";
	}

	private void SettingTouchFunction()
	{
		var leftPart = touchManagers[0];
		var rightPart = touchManagers[1];

		if(unitCount == 16)
		{
			var color = leftPart.gameObject.GetComponent<Image>().color;
			color.r *= 0.2f;
			color.g *= 0.2f;
			color.b *= 0.2f;
			leftPart.gameObject.GetComponent<Image>().color = color;
		}
		if(unitLimit == DataTableManger.SettingTable.Get(Settings.unitLimitMax))
		{
			var color = rightPart.gameObject.GetComponent<Image>().color;
			color.r *= 0.2f;
			color.g *= 0.2f;
			color.b *= 0.2f;
			rightPart.gameObject.GetComponent<Image>().color = color;
		}

		leftPart.tapFunc = () =>
		{
			if (unitCount == 16)
				return;

			unitCount++;
			SaveLoadManager.Data.unitCount = unitCount;
			SceneManager.LoadScene((int)Scenes.StageChoosing);
		};
		rightPart.tapFunc = () =>
		{
			if (unitLimit == DataTableManger.SettingTable.Get(Settings.unitLimitMax))
				return;

			unitLimit++;
			SaveLoadManager.Data.unitLimit = unitLimit;
			SceneManager.LoadScene((int)Scenes.StageChoosing);
		};
	}
}
