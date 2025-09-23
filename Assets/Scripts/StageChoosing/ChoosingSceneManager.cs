using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosingSceneManager : MonoBehaviour
{
    private float emptyProb;
    private float normalProb;
    private float eliteProb;
    private float shopProb;
    private float randomProb;
    private float bossProb;
    private float bossCount;

    private int stageCount = 0;

    private List<Room> rooms;

    public List<GameObject> buttons;

    private ChoosingSceneUIManager uIManager;
    private List<float> probList;

    public Image background;

	private void Awake()
	{
		uIManager = GetComponent<ChoosingSceneUIManager>();
	}

	private void OnEnable()
	{
        SaveLoadManager.Load();
        var data = SaveLoadManager.Data;
        stageCount = data.StageCount;
		
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        emptyProb = DataTableManger.SettingTable.Get(Settings.emptyRoom);
        normalProb = DataTableManger.SettingTable.Get(Settings.normalBattle);
        eliteProb = DataTableManger.SettingTable.Get(Settings.eliteBattle);
        shopProb = DataTableManger.SettingTable.Get(Settings.shopRoom);
        randomProb = DataTableManger.SettingTable.Get(Settings.randomRoom);
        bossProb = DataTableManger.SettingTable.Get(Settings.bossWeight);
        bossCount = DataTableManger.SettingTable.Get(Settings.bossCount);
        probList = new List<float> { normalProb, eliteProb, shopProb, randomProb,emptyProb };

        rooms = GetRandomRoom(3);

		for (int i = 0; i < rooms.Count; i++)
        {
            int index = i;
            uIManager.SetImage(index, Resources.Load<Sprite>(Icons.roomIcons[(int)rooms[index]]));
			uIManager.SetText(index, DataTableManger.StageStringTable.GetExplainString((int)Room.Count + (int)rooms[index] + 1));
            buttons[index].GetComponent<TouchManager>().tapTimeLimit = 0.4f;
			buttons[index].GetComponent<TouchManager>().tapFunc = () => TapFunc(rooms[index]);
			buttons[index].GetComponent<TouchManager>().longPressEnterFunc = () => HoldingFunc((int)rooms[index] + 1);
		}

		uIManager.SetBottomText(0);
		//bossCount = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var pos = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(pos);

        if(RectTransformUtility.RectangleContainsScreenPoint(background.rectTransform, Input.GetTouch(0).position, null) &&
            !(RectTransformUtility.RectangleContainsScreenPoint(buttons[0].GetComponent<Image>().rectTransform, Input.GetTouch(0).position, null) ||
            RectTransformUtility.RectangleContainsScreenPoint(buttons[1].GetComponent<Image>().rectTransform, Input.GetTouch(0).position, null) ||
            RectTransformUtility.RectangleContainsScreenPoint(buttons[2].GetComponent<Image>().rectTransform, Input.GetTouch(0).position, null)))
        {
			uIManager.SetBottomText(0);
		}
    }

	private void TapFunc(Room x)
    {
        SaveLoadManager.Data.StageCount = ++stageCount;
		switch (x)
		{
			case Room.Normal:
			case Room.Elite:
			case Room.Boss:
				SaveLoadManager.Data.BattleType = x == Room.Boss ? BattleType.Boss : (BattleType)(int)x;
				SaveLoadManager.Save();
				SceneManager.LoadScene((int)Scenes.Game);
				break;
			case Room.Shop:
			case Room.Unknown:
			case Room.Empty:
				SaveLoadManager.Save();
				SceneManager.LoadScene((int)Scenes.StageChoosing);
				break;
            default:
                return;
		}
	}
	private void HoldingFunc(int index)
    {
        uIManager.SetBottomText(index);
    }

    private List<Room> GetRandomRoom(int count = 1)
    {
        var result = new List<Room>();
        while (result.Count < count)
        {
            Room room;

            if (stageCount < DataTableManger.SettingTable.Get(Settings.bossCount))
            {
                room = GetRandomWithoutBoss();
            }
            else
            {
                room = GetRandomWithBoss();
            }

            if (stageCount > DataTableManger.SettingTable.Get(Settings.bossCount) + 20)
            {
                result.Add(Room.Boss);
                continue;
            }
            if (room == Room.Init)
                continue;

            if (result.Contains(room))
            {
                if (result.Count == 2 && result[0] == room && result[1] == room)
                    continue;

                if (room == Room.Boss)
                    continue;
            }
            
            result.Add(room);
        }

        return result;
    }

	private Room GetRandomWithoutBoss()
	{
		float prob = UnityEngine.Random.value;
		int index = -1;
		while (prob > 0f)
		{
            if(index == probList.Count)
                return Room.Init;

			prob -= probList[++index];
		}

		return (Room)index;
	}

	private Room GetRandomWithBoss()
    {
		float prob = UnityEngine.Random.value;

        if(prob < bossProb * (stageCount - bossCount))
            return Room.Boss;

        return GetRandomWithoutBoss();
	}

}
