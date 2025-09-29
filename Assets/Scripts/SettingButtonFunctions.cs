using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveDataVC = SaveDataV1;

public class SettingButtonFunctions : MonoBehaviour
{
    public GameObject setting;
    public bool IsTeleport {  get; private set; }
	public Slider teleport;
	public bool IsVibrate { get; private set; }
	public Slider vibration;

	public GameObject popupPanel;
	public TextMeshProUGUI decisionText;
	private Action acceptFunc;

	private void OnEnable()
	{
		var data = SaveLoadManager.Data;
		IsTeleport = data.isTeleport;
		IsVibrate = data.isVibrate;
	}
	private void OnDisable()
	{
		
	}

	private void Start()
	{
		teleport.value = !IsTeleport ? 1 : 0;
		vibration.value = IsVibrate ? 1 : 0;
	}

	public void OnClickOpenSetting()
    {
        setting.SetActive(true);
    }

    public void OnClickCloseSetting()
    {
        setting.SetActive(false);
    }

	public void OnValueTeleportChange(float value)
	{
		IsTeleport = value == 0;
		SaveLoadManager.Data.isTeleport = IsTeleport;
		SaveLoadManager.Save();
	}

	public void OnValueVibrationChange(float value)
	{
		IsVibrate = value == 1f;
		SaveLoadManager.Data.isVibrate = IsVibrate;
		SaveLoadManager.Save();
	}

	public void OnClickGoTitle()
	{
		popupPanel.SetActive(true);
		decisionText.text = "포기하고 타이틀 화면으로\n돌아갈까요?";
		acceptFunc = () =>
		{
			SaveLoadManager.Data.stageId = 1;
			SaveLoadManager.Data.StageCount = 1;
			SaveLoadManager.Data.gold = 0f;
			SaveLoadManager.Data.unitLimit = 12;
			SaveLoadManager.Data.unitCount = 8;
			SaveLoadManager.Data.Deck = new Deck();
			SaveLoadManager.Data.isSave = false;
			SaveLoadManager.Save();
			SceneManager.LoadScene((int)Scenes.Lobby);
		};
	}

	public void OnClickSaveGame()
	{
		popupPanel.SetActive(true);
		decisionText.text = "진행중인 내용을 저장하고\n종료하시겠습니까?";
		acceptFunc = () =>
		{
			SaveLoadManager.Data.isSave = true;
			SaveLoadManager.Save();
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		};	
	}

	public void OnClickAccpet()
	{
		popupPanel.SetActive(false);
		SaveLoadManager.Save();
		acceptFunc();
	}

	public void OnClickReject()
	{
		popupPanel.SetActive(false);
	}
}
