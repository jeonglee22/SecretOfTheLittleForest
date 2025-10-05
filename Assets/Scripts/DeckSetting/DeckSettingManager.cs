using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SaveDataVC = SaveDataV1;

public class DeckSettingManager : MonoBehaviour
{
	public ScrollRect presetNameRect;
	public ScrollRect presetContentRect;

	private RectTransform unitContent;
	private RectTransform presetContent;

	public GameObject presetPanel;
	public GameObject presetContentPanelData;

	private Deck unitDeck;
	private ChoosingUnitManager choosingUnitManager;
	public DeckSceneUIManager sceneUIManager;

	public Toy toy;

	private int diamondCount;
	public GameObject lockedPanel;
	public GameObject startButton;

	public int ChoosedIndex {  get; private set; }
	private List<int> lockedInfo;

	private int presetDataIDStart = 3000;
	private float cellSizeOffset = 0.8f;
	private float cellYSize = 100f;

	private void Awake()
	{
		choosingUnitManager = GetComponent<ChoosingUnitManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		unitDeck = SaveLoadManager.Data.Deck;
		diamondCount = SaveLoadManager.Data.Crystal;
		lockedInfo = SaveLoadManager.Data.LockedInfo;
		unitDeck.LoadDeckData();
	}

	private void Start()
	{
		unitContent = presetNameRect.content;
		presetContent = presetContentRect.content;
		sceneUIManager.SetHaveDiamondText(diamondCount);

		Canvas.ForceUpdateCanvases();

		float xSize = Mathf.FloorToInt(Mathf.Abs(unitContent.parent.gameObject.GetComponent<RectTransform>().rect.width));
		unitContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(xSize * cellSizeOffset, cellYSize);

		SetInitPresetList();

		if(TutorialManager.IsTutorial)
		{
            startButton.SetActive(false);
            lockedPanel.SetActive(false);
            return;
		}

        startButton.SetActive(true);
        lockedPanel.SetActive(false);
    }

	private void SetInitPresetList()
	{
		var presetTable = DataTableManger.PresetTable;
		var count = presetTable.Count;
		for (int i = 0; i < count; i++)
		{
			var index = i;
			var data = DataTableManger.PresetTable.Get((int)IDOffset.Preset + i);
			var preset = Instantiate(presetPanel, unitContent);
			preset.GetComponent<PresetPanelData>().SetData(data.Name);
			preset.GetComponent<Toggle>().onValueChanged.AddListener((bool b) => { if (b) OpenPreset(data); ChoosedIndex = index; });
			preset.GetComponent<Toggle>().group = unitContent.gameObject.GetComponent<ToggleGroup>();

			if (i == 0)
			{
				preset.GetComponent<Toggle>().isOn = true;
			}
		}
	}

	private void OpenPreset(PresetData data)
	{
		var pos = data.Pos;
		if(!lockedInfo.Contains(data.ID % presetDataIDStart))
		{
			lockedPanel.SetActive(true);
			startButton.SetActive(false);
			sceneUIManager.SetLockedText(data.Price);
		}
		else
		{
			lockedPanel.SetActive(false);
			startButton.SetActive(true);
		}

		unitDeck = new Deck();
		unitDeck.AddPosSetting(pos.ToList());
		unitDeck.KingId = pos[data.BossPos];
		unitDeck.KingPos = data.BossPos;
		for (int i = 0; i < pos.Length; i++)
		{
			if (pos[i] == 0)
				continue;

			unitDeck.AddDeckData(DataTableManger.ToyTable.Get(pos[i]));
		}

		sceneUIManager.SetExplainText(data.ID % presetDataIDStart);
		SetDeckInfos();
	}

	public void ChangeBuyPanel()
	{
		lockedPanel.SetActive(false);
		startButton.SetActive(true);
	}

	public void SetDeckInfos()
	{
		var presetContent = presetContentRect.content;
		for (int i = 0; i < presetContent.childCount; i++)
			Destroy(presetContent.GetChild(i).gameObject);

		foreach (var toyGroup in unitDeck.Toys)
		{
			var count = toyGroup.count;
			var toyData = toyGroup.data;

			var content = Instantiate(presetContentPanelData, presetContent);
			content.GetComponent<ContentPresetPanelData>().SetData(toyData, count, toyData.UnitID == unitDeck.KingId);
		}
	}

	public void SaveData()
	{
		SaveLoadManager.Data = new SaveDataVC();
		SaveLoadManager.Data.Deck = unitDeck;
		if(!lockedInfo.Contains(0))
			lockedInfo.Add(0);
		SaveLoadManager.Data.LockedInfo = lockedInfo;
	}

	private void Update()
	{
		//SetCellSize();

		//if (isValueChange && Time.time - lastChangedTime > clampTimeInterval)
		//{
		//	int minIndex = -1;
		//	float minDis = float.MaxValue;

		//	var count = unitDeck.Toys.Count;

		//	if (count == 1)
		//	{
		//		presetNameRect.horizontalNormalizedPosition = 0;
		//		isValueChange = false;
		//		return;
		//	}

		//	for (int i = 0; i < count; i++)
		//	{
		//		var interval = 1f / (count - 1) * (float)i;
		//		if (Mathf.Abs(interval - currentValue) < minDis) 
		//		{ 
		//			minDis = Mathf.Abs(interval - currentValue);
		//			minIndex = i;
		//		}
		//	}

		//	presetNameRect.horizontalNormalizedPosition = 1f / (count - 1) * (float)minIndex;
		//	isValueChange = false;
		//}
	}

	//private void SetCellSize()
	//{
	//	var content = presetNameRect.content;
	//	var gridgroup = content.GetComponent<GridLayoutGroup>();
	//	var cellsize = (presetNameRect.gameObject.GetComponent<RectTransform>().rect.width - 40f) / 5f;
	//	cellSize = new Vector2(cellsize, cellsize);
	//	gridgroup.cellSize = cellSize;

	//	foreach (var collider in presetNameRect.content.GetComponentsInChildren<BoxCollider2D>())
	//	{
	//		collider.size = cellSize;
	//	}
	//}
}
