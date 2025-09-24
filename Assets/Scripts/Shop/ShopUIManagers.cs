using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManagers : MonoBehaviour
{
    public ScrollRect unitHavingRect;
    public ScrollRect unitRegetRect;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI goldText2;
    public TextMeshProUGUI unitText;
    public TextMeshProUGUI unitText2;
    public TextMeshProUGUI reloadText;

    public List<TextMeshProUGUI> nameTexts;
    public List<TextMeshProUGUI> heartTexts;
    public List<TextMeshProUGUI> attackTexts;
    public List<TextMeshProUGUI> coinTexts;
    public List<Image> toyImages;

    public List<Image> buttonImages;

    private Coroutine colorCorout;

    public List<TouchManager> touchManagers;
    public List<Image> buyBlockImages;

    private List<int> choosedIds;
    private List<int> costs;
    private int stageId;
	private float unitLimit;
	private float goldLimit;

    private Color selectedColor = new Color(0, 0, 0, 0.98f);
    private Color notSelectedColor = new Color(0.462f, 0.462f, 0.462f, 0.165f);
    private Color orangeColor = new Color(1f, 0.7529f, 0f, 1f);

	public bool IsFree { get; set; } = true;

    private Deck userDeck;
    public float Gold;

	private void OnEnable()
	{
        SaveLoadManager.Load();
        var data = SaveLoadManager.Data;
        stageId = data.stageId;
        unitLimit = data.unitLimit;
        userDeck = data.Deck;
        Gold = data.gold;
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < touchManagers.Count; i++)
        {
            var manager = touchManagers[i];

            if (!manager.IsTap)
                return;

            manager.gameObject.SetActive(false);
            manager.tapFunc = () =>
            {
                if (costs[i] > gold)
            };
        }
    }

    public void SetBuyItems()
    {
		choosedIds = DataTableManger.RewardTable.GetRandomUnitIds(stageId, 3);
		goldLimit = DataTableManger.SettingTable.Get(Settings.goldLimit);
		for (int i = 0; i < 3; i++)
        {
            SetImageAndInfo(DataTableManger.ToyTable.Get(choosedIds[i]), i);
        }
    }

    public void SetImageAndInfo(ToyData data, int index)
    {
        nameTexts[index].text = data.Name;
        nameTexts[index].color = ConvertRating(data.Rating);
        heartTexts[index].text = data.HP.ToString();
        attackTexts[index].text = data.Attack.ToString();
        var value = GetBuyCost(data.Price);

		coinTexts[index].text = value.ToString();
        costs.Add(Mathf.FloorToInt(value));
        toyImages[index].sprite = GameObjectManager.IconResource.GetToyImage(data.ModelCode);
    }

    private Color ConvertRating(char s)
    {
        return s switch
        {
            'S' => Color.yellow,
            'A' => Color.magenta,
            'B' => new Color(0f, 0.7f, 0.94f, 1f),
            'C' => Color.green,
            _ => throw new System.Exception("Wrong Rating")
        };
    }

    private float GetBuyCost(int cost)
    {
        float realCost = cost;
        realCost += stageId;
        realCost *= goldLimit;

        var maxCost = DataTableManger.ToyTable.Table[0].Price;
        var table = DataTableManger.ToyTable.Table;

		foreach (var data in table)
        {
            if(maxCost < data.Price)
                maxCost = data.Price;
        }

        realCost /= maxCost;

        return Mathf.FloorToInt(realCost) + 1;
    }

    public void SetGoldText(float gold)
    {
        goldText.text = $"({gold}/{goldLimit})";
    }

    public void SetUnitText(float unitCount)
    {
        unitText.text = $"({userDeck.GetDeckTotalCount()}/{unitLimit})";
    }

    public void SetCostText()
    {
        var text = IsFree ? "무료" : "4";
        reloadText.text = $"비용 : {text}";
    }

    public void SetButtonColor(bool b)
    {
        if(b)
        {
            buttonImages[0].color = selectedColor;
            buttonImages[1].color = notSelectedColor;
        }
        else
        {
            buttonImages[0].color = notSelectedColor;
            buttonImages[1].color = selectedColor;
		}
    }

    public void SetGoldColor(bool isLimit)
    {
        if(isLimit)
        {
			if (colorCorout != null)
				StopCoroutine(colorCorout);
			colorCorout = null;

			goldText.color = Color.red;
            goldText2.color = Color.red;

            colorCorout = StartCoroutine(CoTextColorChange());
        }
        else
        {
            goldText.color = Color.white;
            goldText2.color = orangeColor;
		}
    }

    private IEnumerator CoTextColorChange()
    {
        yield return new WaitForSeconds(1);
        SetGoldColor(false);
	}

    public void SetUnitColor(bool isLimit)
    {
        if(isLimit)
        {
            unitText.color = Color.red;
            unitText2.color = Color.red;
        }
        else
        {
			unitText.color = Color.white;
			unitText2.color = orangeColor;
		}
    }
}
