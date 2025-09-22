
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalWinUIManager : MonoBehaviour
{
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI unitText;
	public TextMeshProUGUI winText;

	public List<Image> images;
	public List<TextMeshProUGUI> health;
	public List<TextMeshProUGUI> attack;
	public List<TouchManager> touchManagers;

	public float Gold {  get; set; }
	private float unitLimit;
	private int stageId;
	private Deck currentDeck;
	private List<int> choosedIds;

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		unitLimit = data.unitLimit;
		if (unitLimit == 0)
			unitLimit = DataTableManger.SettingTable.Get(Settings.unitLimit);
		stageId = data.stageId;
		Gold = data.gold;
		currentDeck = data.Deck;

		choosedIds = DataTableManger.RewardTable.GetRandomUnitIds(stageId,3);
		SetImageAndInfo(choosedIds);

		SetGoldText();
		SetUnitText(data.Deck.GetDeckTotalCount());
		//SetUnitText(12);
		SettingTouchFunction();
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.unitLimit = unitLimit;
		SaveLoadManager.Data.stageId = stageId;
		//SaveLoadManager.Data.gold = Gold;
		SaveLoadManager.Data.gold = Gold;
		SaveLoadManager.Data.Deck = currentDeck;
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
	}

	private void SetImageAndInfo(List<int> ids)
	{
		for (int i = 0; i < ids.Count; i++)
		{
			var toyData = DataTableManger.ToyTable.Get(ids[i]);
			var sprite = Resources.Load<Sprite>(string.Format(Variables.SpritePath, toyData.ModelCode));

			images[i].sprite = sprite;
			health[i].text = toyData.HP.ToString();
			attack[i].text = toyData.Attack.ToString();
		}
	}

	private void SettingTouchFunction()
	{
		for(int i =0; i < touchManagers.Count; i++)
		{
			if (currentDeck.GetDeckTotalCount() == unitLimit)
				images[i].color *= 0.5f;
			var index = i;
			touchManagers[i].tapFunc = () =>
			{
				if (currentDeck.GetDeckTotalCount() == unitLimit)
					return;
				currentDeck.AddDeckData(DataTableManger.ToyTable.Get(choosedIds[index]));
				SceneManager.LoadScene((int)Scenes.StageChoosing);
			};
		}
	}
}
