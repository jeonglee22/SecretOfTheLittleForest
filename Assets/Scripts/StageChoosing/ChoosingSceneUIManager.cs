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

	private float unitLimit;
	private float gold;
	private float unitCount;

	private void Awake()
	{
		choosingManager = GetComponent<ChoosingSceneManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		unitLimit = data.unitLimit;
		gold = data.gold;
		unitCount = data.Deck.GetDeckTotalCount();

		SetCoinText();
		SetCountText();
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

	public void SetCountText()
	{
		countText.text = $"  ({unitCount}/{unitLimit})";
	}

	public void SetCoinText()
	{
		coinText.text = $"  ({gold}/{DataTableManger.SettingTable.Get(Settings.goldLimit)})";
	}

	public void SetBottomText(int index)
	{
		bottomText.text = "1대1전투를 하고 골드와 유닛을 얻습니다.\r\n";
	}
}
