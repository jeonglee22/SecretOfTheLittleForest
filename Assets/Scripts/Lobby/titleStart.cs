using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV1;

public class titleStart : MonoBehaviour
{
	SaveDataVC data;

	private void OnEnable()
	{
		data = SaveLoadManager.Data;
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.touchCount == 0)
            return;

		if(!data.isSave)
		{
			SaveLoadManager.Data.Crystal = 30;
			SaveLoadManager.Save();
			SceneManager.LoadScene((int)Scenes.DeckSetting);
		}
		else
		{
			SaveLoadManager.Data.Rooms = new List<Room>();
			SaveLoadManager.Data.Rooms.AddRange(data.CurrentStageRooms);
			SaveLoadManager.Data.CurrentStageRooms?.Clear();
			SaveLoadManager.Data.StageCount = data.CurrentStageCount;
			SaveLoadManager.Data.CurrentStageCount = 0;
			SaveLoadManager.Save();
			SceneManager.LoadScene((int)Scenes.StageChoosing);
		}
    }
}
