using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalWinUIManager : ResultWindowManager
{
	public TextMeshProUGUI goldButtonText;

	public List<Image> images;
	public List<TextMeshProUGUI> health;
	public List<TextMeshProUGUI> attack;

	private float unitLimit;
	private int stageId;
	private Deck currentDeck;
	private List<int> choosedIds;
	public int centerChoosedIds { get { return choosedIds[1]; } }
	private float blockTouchAlpha = 0.8f;

	protected override void OnEnable()
	{
		base.OnEnable();
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		unitLimit = data.unitLimit;
		if (unitLimit == 0)
			unitLimit = DataTableManger.SettingTable.Get(Settings.unitLimit);
		stageId = data.stageId;
		currentDeck = data.Deck;

		goldButtonText.text = string.Format(DataTableManger.StageStringTable.GetWinString(3),
			DataTableManger.SettingTable.Get(Settings.battleGold));

		choosedIds = DataTableManger.RewardTable.GetRandomUnitIds(stageId,3);
		SetImageAndInfo(choosedIds);

		SetGoldText();
		SetUnitText(data.Deck.GetDeckTotalCount());
		//SetUnitText(12);
		SettingTouchFunction();
	}

	private void OnDisable()
	{
		
	}

	public void SetUnitText(int unitCount)
	{
		unitText.text = $"({unitCount}/{unitLimit})";
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
		for (int i = 0; i < touchManagers.Count; i++)
		{
			if (currentDeck.GetDeckTotalCount() == unitLimit)
			{
				var color = touchManagers[i].gameObject.GetComponent<Image>().color;
				color.a = blockTouchAlpha;
				touchManagers[i].gameObject.GetComponent<Image>().color = color;
			}

			var index = i;
			touchManagers[i].tapFunc = () =>
			{
				if (currentDeck.GetDeckTotalCount() == unitLimit)
					return;
				currentDeck.AddDeckData(DataTableManger.ToyTable.Get(choosedIds[index]));
				SceneManager.LoadScene((int)Scenes.StageChoosing);
				SaveLoadManager.Data.Deck = currentDeck;
				SaveLoadManager.Data.isTeleport = toyControl.IsTeleport;
				SaveLoadManager.Save();
			};
		}
	}
}
