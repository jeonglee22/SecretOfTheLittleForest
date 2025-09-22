using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosingSceneManager : MonoBehaviour
{
    private float emptyProb;
    private float normalProb;
    private float eliteProb;
    private float shopProb;
    private float randomProb;
    private float bossProb;
    //private int bossCount;

    public List<GameObject> buttons;

    private ChoosingSceneUIManager uIManager;

	private void Awake()
	{
		uIManager = GetComponent<ChoosingSceneUIManager>();
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

        uIManager.SetImage(0, Resources.Load<Sprite>("Icons/sword"));
        uIManager.SetImage(1, Resources.Load<Sprite>("Icons/banner"));
        uIManager.SetImage(2, Resources.Load<Sprite>("Icons/boss"));

        uIManager.SetText(0, "일반 전투");
        uIManager.SetText(1, "엘리트 전투");
        uIManager.SetText(2, "보스 전투");

        for( int i = 0; i < buttons.Count; i++)
        {
            var index = i;
            buttons[i].GetComponent<TouchManager>().tapFunc = () => TapFunc(index);
            buttons[i].GetComponent<TouchManager>().longPressFunc = HoldingFunc;
        }

        //bossCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TapFunc(int x)
    {
        SaveLoadManager.Data.BattleType = (BattleType)x;
        SaveLoadManager.Save((int)JsonFileNum.StageInfo);
        SceneManager.LoadScene((int)Scenes.Game);
    }
    private void HoldingFunc()
    {
        uIManager.SetBottomText(0);
    }
}
