using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossWinUIManager : MonoBehaviour
{
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI unitText;
	public TextMeshProUGUI winText;
	public TextMeshProUGUI bossRewardCrystalText;
	public TextMeshProUGUI goldButtonText;

	public List<Image> images;
	public List<TextMeshProUGUI> health;
	public List<TextMeshProUGUI> attack;
	public List<TouchManager> touchManagers;

	public GameObject crystalPanel;
	public TouchManager crystalPanelmanager;

	public float Gold { get; set; }
	private float unitLimit;
	private int stageId;
	private Deck currentDeck;
	private List<int> choosedIds;

	private float blockTouchAlpha = 0.8f;

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

		crystalPanel.SetActive(true);

		choosedIds = DataTableManger.RewardTable.GetRandomInNextList(stageId, 3);
		SetImageAndInfo(choosedIds);

		bossRewardCrystalText.text = $"+{DataTableManger.SettingTable.Get(Settings.bossReward)}";
		goldButtonText.text = string.Format(DataTableManger.StageStringTable.GetWinString(3),
			DataTableManger.SettingTable.Get(Settings.bossGold));

		SetGoldText();
		SetUnitText(data.Deck.GetDeckTotalCount());
		//SetUnitText(12);
		SettingTouchFunction();
		var corout = StartCoroutine(CoDestroy());
		crystalPanelmanager.tapFunc = () =>
		{
			crystalPanel.SetActive(false);
			StopCoroutine(corout);
			SaveLoadManager.Data.Crystal += 4;
			corout = null;
		};
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

	public void SetWinText(WinType ty)
	{
		winText.text = "text";
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
			};
		}
	}

	private IEnumerator CoDestroy()
	{
		var currTime = 2f;
		while (currTime > 0) 
		{ 
			currTime -= Time.deltaTime;
			yield return null;
		}
		crystalPanel.SetActive(false);
		SaveLoadManager.Data.Crystal += 4;
	}
}
