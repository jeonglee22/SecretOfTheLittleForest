using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
	private Vector3 initCameraPos = new Vector3(-180f, 55f,130f);
	public PlayManager playManager;
	public CanvasManager canvasManager;
	public PlayerTurn playerTurn;

	public void OnClickFinishPut()
    {
		playManager.StartGame();
		canvasManager.Open(GameWindow.MainGame);
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
		Camera.main.transform.position = initCameraPos;
    }
}
