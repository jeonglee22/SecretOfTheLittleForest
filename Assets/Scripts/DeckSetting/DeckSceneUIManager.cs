using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneUIManager : MonoBehaviour
{
	public ChoosingUnitManager settingManager;

	public TextMeshProUGUI descriptionText;
	public TextMeshProUGUI explainText;

	public SoundManager soundManager;
	public DeckSettingManager deckSettingManager;

	public TextMeshProUGUI diamondLockedText;
	public TextMeshProUGUI diamondHaveText;
	private int presetValue;

	private int costMax;
	private bool isNotCorrectCost = false;
	private bool isNotCorrectCount = false;

	public GameObject popupPanel;

	private Action acceptFunc;
	private int userDiamond;
	private Color origindiamondColor = new Color(0f, 0.69f, 0.94f, 1f);

	private Coroutine textColorCoroute;

	public SettingButtonFunctions buttonFunctions;

	public void OnClickBack()
	{
		descriptionText.text = "로비로 돌아가시겠습니까?";
		acceptFunc = () => SceneManager.LoadScene((int)Scenes.Lobby);
		popupPanel.SetActive(true);
	}

	private void Start()
	{
		popupPanel.SetActive(false);
	}

	private void OnDisable()
	{
		SaveData();
	}

	public void OnClickStart()
	{
		if (settingManager.UnitCount == 0 || isNotCorrectCost || isNotCorrectCount)
		{
			return;
		}
		descriptionText.text = "게임을 시작하시겠습니까?";
		acceptFunc = () => SceneManager.LoadScene((int)Scenes.StageChoosing);
		popupPanel.SetActive(true);
	}

	public void OnClickAccpet()
	{
		acceptFunc();
		popupPanel.SetActive(false);
		SaveData();
	}

	public void OnClickReject()
	{
		popupPanel.SetActive(false);
	}

	public void OnClickDebug()
	{
		SceneManager.LoadScene((int)Scenes.StageChoosing);
	}

	public void OnClickBuyPreset()
	{
		if (textColorCoroute != null)
			StopCoroutine(textColorCoroute);
		textColorCoroute = null;

		if (presetValue > SaveLoadManager.Data.Crystal)
		{
			diamondLockedText.color = Color.red;
			diamondHaveText.color = Color.red;
			if (buttonFunctions.IsVibrate) Handheld.Vibrate();

			textColorCoroute = StartCoroutine(CoColorChange());
			return;
		}
		else
		{
			diamondLockedText.color = origindiamondColor;
			diamondHaveText.color = origindiamondColor;

			descriptionText.text = "보석을 사용해서 프리셋을\n해금하시겠습니까?";
			acceptFunc = () =>
			{
				userDiamond -= presetValue;
				SetHaveDiamondText(userDiamond);
				if (!SaveLoadManager.Data.LockedInfo.Contains(deckSettingManager.ChoosedIndex))
					SaveLoadManager.Data.LockedInfo.Add(deckSettingManager.ChoosedIndex);
				SaveLoadManager.Data.Crystal = userDiamond;
				SaveLoadManager.Save();
				deckSettingManager.ChangeBuyPanel();
			};
			popupPanel.SetActive(true);
		}
	}

	public void SaveData()
	{
		deckSettingManager.SaveData();
		soundManager.SaveData();
		SaveLoadManager.Data.Crystal = userDiamond;
		SaveLoadManager.Data.bgmPos = 0f;
		SaveLoadManager.Data.isTeleport = buttonFunctions.IsTeleport;
		SaveLoadManager.Data.isVibrate = buttonFunctions.IsVibrate;
		SaveLoadManager.Save();
	}

	public void SetLockedText(int value)
	{
		diamondLockedText.text = $" {value}";
		presetValue = value;
	}
	
	public void SetHaveDiamondText(int value)
	{
		diamondHaveText.text = $" {value}";
		userDiamond = value;
	}

	private IEnumerator CoColorChange()
	{
		yield return new WaitForSeconds(0.5f);
		diamondLockedText.color = origindiamondColor;
		diamondHaveText.color = origindiamondColor;
	}

	public void SetExplainText(int index)
	{
		string text = DataTableManger.StageStringTable.GetPresetString(index);
		explainText.text = text;
	}
}
