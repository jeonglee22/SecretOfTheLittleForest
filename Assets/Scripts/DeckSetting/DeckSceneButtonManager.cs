using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneButtonManager : MonoBehaviour
{
	public void OnClickBack()
	{
		SceneManager.LoadScene((int)Scenes.Lobby);
	}

	public void OnClickStart()
	{
		SceneManager.LoadScene((int)Scenes.Game);
	}

	public void OnClickReset()
	{
		SceneManager.LoadScene((int)Scenes.DeckSetting);
	}
}
