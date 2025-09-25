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
	public TextMeshProUGUI goldButtonText;

	public List<TouchManager> touchManagers;

	public float Gold { get; set; }
	private float unitLimit;
	private float unitCount;

	private float blockTouchAlpha = 0.8f;

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

		goldButtonText.text = string.Format(DataTableManger.StageStringTable.GetWinString(3),
			DataTableManger.SettingTable.Get(Settings.eliteGold));

		SetGoldText();
		SetUnitText(data.Deck.GetDeckTotalCount());
		//SetUnitText(12);
		SettingTouchFunction();
	}

	private void OnDisable()
	{
		SaveLoadManager.Save();
	}

	public void OnClickGetGold()
	{
		var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

		var battleType = boardManager.BattleType;
		var gold = battleType switch
		{
			BattleType.Normal => DataTableManger.SettingTable.Get(Settings.battleGold),
			BattleType.Elite => DataTableManger.SettingTable.Get(Settings.eliteGold),
			BattleType.Boss => DataTableManger.SettingTable.Get(Settings.bossGold),
			_ => throw new System.InvalidOperationException(),
		};
		Gold += gold;
		Gold = Mathf.Clamp(Gold, 0, DataTableManger.SettingTable.Get(Settings.goldLimit));

		SetGoldText();
		SceneManager.LoadScene((int)Scenes.StageChoosing);
		SaveLoadManager.Data.gold = Gold;
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
			color.a = blockTouchAlpha;
			leftPart.gameObject.GetComponent<Image>().color = color;
		}
		if(unitLimit == DataTableManger.SettingTable.Get(Settings.unitLimitMax))
		{
			var color = rightPart.gameObject.GetComponent<Image>().color;
			color.a = blockTouchAlpha;
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
