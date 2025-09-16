using System.Collections.Generic;
using UnityEngine;

public class LobbyWindowManager : MonoBehaviour
{
    public List<LobbyWindow> windows;

    public Windows mainWindow;

    public Windows CurrentWindow { get; private set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		foreach (var window in windows)
		{
			window.Init(this);
			window.gameObject.SetActive(false);
		}

		CurrentWindow = mainWindow;
		windows[(int)CurrentWindow].Open();
	}

	public void Open(Windows id)
	{
		windows[(int)CurrentWindow].Close();
		CurrentWindow = id;
		windows[(int)CurrentWindow].Open();
	}
}
