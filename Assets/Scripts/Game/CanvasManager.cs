using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public List<GameObject> canvases;

    public GameWindow defaultWindow;
    private GameWindow currentWindow;

    private void Start()
    {
        currentWindow = defaultWindow;
		for (int i = 0; i < canvases.Count; i++)
		{
			canvases[i].SetActive(false);
		}

		canvases[(int)defaultWindow].SetActive(true);
	}

    internal void Open(GameWindow windowId)
    {
        canvases[(int)currentWindow].SetActive(false);
        currentWindow = windowId;
        canvases[(int)currentWindow].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
