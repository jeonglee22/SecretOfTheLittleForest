using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultWindowManager : MonoBehaviour
{
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI unitText;
	public TextMeshProUGUI explainText;

	public List<TouchManager> touchManagers;
    public PlayManager playManager;
	public ToyControl toyControl;

	public float Gold { get; set; }

	protected virtual void OnEnable()
	{
        if (playManager.WinType != WinType.None)
        {
            SetExplainText(DataTableManger.StageStringTable.GetWinString((int)playManager.WinType));
        }
        else if (playManager.LoseType != LoseType.None)
        {
            SetExplainText(DataTableManger.StageStringTable.GetLoseString((int)playManager.LoseType));
        }

		var data = SaveLoadManager.Data;
		Gold = data.gold;
	}

    public void SetExplainText(string text)
    {
        explainText.text = text;
    }

	public virtual void OnClickGetGold()
	{
		var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

		var battleType = boardManager.BattleType;
		var gold = battleType switch
		{
			BattleType.Normal => DataTableManger.SettingTable.Get(Settings.battleGold),
			BattleType.Elite => DataTableManger.SettingTable.Get(Settings.eliteGold),
			BattleType.Boss => DataTableManger.SettingTable.Get(Settings.bossGold),
			_ => throw new System.InvalidOperationException(),
		};
		Gold += gold;
		Gold = Mathf.Clamp(Gold, 0, DataTableManger.SettingTable.Get(Settings.goldLimit));

		SetGoldText();
		SceneManager.LoadScene((int)Scenes.StageChoosing);
		SaveLoadManager.Data.gold = Gold;
		SaveLoadManager.Data.isTeleport = toyControl.IsTeleport;
		SaveLoadManager.Save();
	}

	protected void SetGoldText()
	{
		var goldLimit = DataTableManger.SettingTable.Get(Settings.goldLimit);
		if(goldText != null) goldText.text = $"({Gold}/{goldLimit})";
	}
}
