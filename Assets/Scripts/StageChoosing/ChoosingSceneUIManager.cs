using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingSceneUIManager : MonoBehaviour
{
    public Image leftImage;
    public TextMeshProUGUI leftText;
    public Image middleImage;
	public TextMeshProUGUI middleText;
	public Image rightImage;
	public TextMeshProUGUI rightText;

    public TextMeshProUGUI bottomText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI coinText;

    private ChoosingSceneManager choosingManager;

	private void Awake()
	{
		choosingManager = GetComponent<ChoosingSceneManager>();
	}

	public void SetImage(int pos, Sprite image)
	{
		switch (pos)
		{
			case 0: leftImage.sprite = image; break;
			case 1: middleImage.sprite = image; break;
			case 2: rightImage.sprite = image; break;
			default: throw new System.Exception("Wrong input");
		};
	}

	public void SetText(int pos, string text)
	{
		switch (pos)
		{
			case 0: leftText.text = text; break;
			case 1: middleText.text = text; break;
			case 2: rightText.text = text; break;
			default: throw new System.Exception("Wrong input");
		};
	}

	public void SetCountText(int count)
	{
		countText.text = $"  ({count}/{DataTableManger.SettingTable.Get(Settings.unitLimit)})";
	}

	public void SetCoinText(int coin)
	{
		coinText.text = $"  ({coin}/{DataTableManger.SettingTable.Get(Settings.goldLimit)})";
	}

	public void SetBottomText(int index)
	{
		bottomText.text = "1대1전투를 하고 골드와 유닛을 얻습니다.\r\n";
	}
}
