using UnityEditor.SceneManagement;
using UnityEngine;

public class ShopLogicManager : MonoBehaviour
{
    private ShopUIManagers uIManagers;

	private int stageId;
	private float unitLimit;
	private float goldLimit;
	private float gold;

	private bool isFree = true;

	private Deck userDeck;

	private void Awake()
	{
		uIManagers = GetComponent<ShopUIManagers>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		stageId = data.stageId;
		unitLimit = data.unitLimit;
		gold = data.gold;
		userDeck = data.Deck;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        uIManagers.SetBuyItems();
		uIManagers.SetGoldText(gold);
		uIManagers.SetCostText();

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
