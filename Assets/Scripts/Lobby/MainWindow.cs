using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : LobbyWindow
{
    public Button start;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Open()
	{
        base.Open();
	}

	public override void Close()
	{
        base.Close();
	}

    public void OnClickStart()
    {
        SceneManager.LoadScene((int)Scenes.DeckSetting);
    }
}
