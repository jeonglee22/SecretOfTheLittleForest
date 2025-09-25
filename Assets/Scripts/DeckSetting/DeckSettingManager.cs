using System;
using System.Linq;
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

	public Toy toy;

	private Vector2 cellSize = new Vector2(110, 110);

	private float lastChangedTime;
	private bool isValueChange;
	private float currentValue;
	private float clampTimeInterval = 0.5f;
	//private bool isClicking = false;

	private void Awake()
	{
		choosingUnitManager = GetComponent<ChoosingUnitManager>();
	}

	private void OnEnable()
	{
		SaveLoadManager.Load();
		unitDeck = SaveLoadManager.Data.Deck;
		unitDeck.LoadDeckData();
	}

	private void OnDisable()
	{
		SaveLoadManager.Data = new SaveDataVC();
		SaveLoadManager.Data.Deck = unitDeck;
		SaveLoadManager.Save();
	}

	private void Start()
	{
		unitContent = presetNameRect.content;
		presetContent = presetContentRect.content;

		SetInitPresetList();
	}

	private void SetInitPresetList()
	{
		var presetTable = DataTableManger.PresetTable;
		var count = presetTable.Count;
		for(int i = 0; i < count; i++)
		{
			var data = DataTableManger.PresetTable.Get((int)IDOffset.Preset + i);
			var preset = Instantiate(presetPanel, unitContent);
			preset.GetComponent<PresetPanelData>().SetData(data.Name);
			preset.GetComponent<Toggle>().onValueChanged.AddListener((bool b) => { if (b) OpenPreset(data); });
			preset.GetComponent<Toggle>().group = unitContent.gameObject.GetComponent<ToggleGroup>();
		}
	}

	private void OpenPreset(PresetData data)
	{
		var pos = data.Pos;
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

		SetDeckInfos();
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
