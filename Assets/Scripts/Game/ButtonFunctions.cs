using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
	private Vector3 initCameraPos;
	public PlayManager playManager;
	public CanvasManager canvasManager;

	private void Start()
	{
		initCameraPos = Camera.main.transform.position;
	}

	public void OnClickFinishPut()
    {
		playManager.StartGame();
		canvasManager.Open(GameWindow.MainGame);
    }

    public void OnClickResetCamera()
    {
		Camera.main.transform.position = initCameraPos;
    }
}
