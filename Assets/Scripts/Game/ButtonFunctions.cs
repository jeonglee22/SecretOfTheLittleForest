using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
	private Vector3 initCameraPos = new Vector3(-180f, 55f, 130f);
	private Vector3 gameCameraPos = new Vector3(-167f, 66f, 130f);
	public PlayManager playManager;
	public CanvasManager canvasManager;
	public PlayerTurn playerTurn;
	private bool isFromGameScene;
	public ReadyCanvasManager readyCanvasManager;
	public BoardManager boardManager;
	public PlayLogic playLogic;
	public ToyControl toyControl;
	private bool isTeleport;

	public Slider eliteSlider;

	private bool isElite = false;

	private void OnEnable()
	{
		var data = SaveLoadManager.Data;
		var scene = data.Scenes;
		isTeleport = data.isTeleport;
		isElite = data.BattleType == BattleType.Elite;
		isFromGameScene = scene == Scenes.Game;
	}

	private void Start()
	{
		OnClickResetCamera();
		if (toyControl != null)
			toyControl.IsTeleport = isTeleport;
		boardManager.IsEliteBoard = isElite;

		if(eliteSlider != null)
			eliteSlider.value = isElite ? 1 : 0;
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.isTeleport = isTeleport;
		SaveLoadManager.Save();
	}

	public void OnClickStartGame()
    {
		playManager.StartGame();
		canvasManager.Open(GameWindow.MainGame);
		SetCameraToGamePos();
	}

	public void OnClickFinishSetting()
	{
		boardManager.SaveDeckSetting();
		playLogic.ClearNodes();
		SceneManager.LoadScene(isFromGameScene ? (int)Scenes.Game : (int)Scenes.StageChoosing);
	}

	public void OnClickNodeSettingAtGame()
	{
		SaveLoadManager.Data.Scenes = Scenes.Game;
		SaveLoadManager.Data.EnemyFieldID = boardManager.BoardID;
		SaveLoadManager.Save();
		SceneManager.LoadScene((int)Scenes.NodeSetting);
	}

	public void OnClickBackDeck()
	{
		SceneManager.LoadScene((int)Scenes.DeckSetting);
	}

	public void OnClickReLoadGameScene()
	{
		SceneManager.LoadScene((int)Scenes.Game);
	}

	public void OnClickDevelopMode()
	{
		playerTurn.DevelopMode = true;
	}

    public void OnClickResetCamera()
    {
		if (readyCanvasManager == null)
			return;

		if(Camera.main.transform.position == initCameraPos)
		{
			Camera.main.transform.position = gameCameraPos;
			readyCanvasManager.SetCameraText(true);
		}
		else
		{
			Camera.main.transform.position = initCameraPos;
			readyCanvasManager.SetCameraText(false);
		}
    }

	public void SetCameraToGamePos()
	{
		Camera.main.transform.position = gameCameraPos;
	}

	public void OnClickHome()
	{
		SaveLoadManager.Save();
		SceneManager.LoadScene((int)Scenes.Lobby);
	}

	public void OnClickStageChoosing()
	{
		SaveLoadManager.Save();
		SceneManager.LoadScene((int)Scenes.StageChoosing);
	}

	public void OnValueChangedBattleType(float value)
	{
		isElite = value == 1;
		boardManager.IsEliteBoard = isElite;
	}

	public void OnClickSetTeleportMoving(float teleport)
	{
		isTeleport = teleport == 0;
		toyControl.IsTeleport = isTeleport;
	}
}
