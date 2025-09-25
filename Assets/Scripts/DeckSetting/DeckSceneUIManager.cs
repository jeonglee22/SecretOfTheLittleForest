using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneUIManager : MonoBehaviour
{
	public ChoosingUnitManager settingManager;

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
		popupPanel.SetActive(false);
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
	}

	public void OnClickReject()
	{
		popupPanel.SetActive(false);
	}

	public void OnClickDebug()
	{
		SceneManager.LoadScene((int)Scenes.StageChoosing);
	}

	public void OnValueChangeChoosedPreset()
	{

	}
}
