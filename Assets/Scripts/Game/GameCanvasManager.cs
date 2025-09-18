using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    public List<GameObject> turnImages;
	public PlayManager playManager;
	public Turn playerturn;
	public TextMeshProUGUI turnText;

	private int currentCount;
	private Color initColor;

	private void Start()
	{
		SetTurnText((int)DataTableManger.SettingTable.Get(Settings.battleTurnCount));
		turnText.outlineColor = Color.black;
		turnText.outlineWidth = 0.2f;
		currentCount = (int) DataTableManger.SettingTable.Get(Settings.moveCount);
		initColor = turnImages[0].GetComponent<Image>().color;
	}

	public void OnClickTurnEnd()
    {
		playerturn.EndTurn();
		ResetTurnImage();
	}

	public void SetTurnText(int turnCount)
	{
		turnText.text = $"≥≤¿∫ ≈œ\n{turnCount}";
	}

	public void OnValuePlayerChanged(bool b)
	{
		playManager.ShowPlayerStats(b);
	}

	public void OnValueEnemyChanged(bool b)
	{
		playManager.ShowEnemyStats(b);
	}

	public void ResetTurnImage()
	{
		foreach (GameObject turnImage in turnImages)
		{
			turnImage.GetComponent<Image>().color = initColor;
		}
		currentCount = (int)DataTableManger.SettingTable.Get(Settings.moveCount);
	}

	public void SetTurnImageColor(int index)
	{
		turnImages[playerturn.MoveCount].GetComponent<Image>().color = Color.gray;
	}
}
