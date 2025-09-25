using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContentPanelData : MonoBehaviour
{
    public Image toyImage;
    public TextMeshProUGUI multipleText;
    public TextMeshProUGUI costText;
    public TouchManager touchManager;
    private ToyData toyData;
    public ToyData ToyData {  get { return toyData; } }

    public int Cost {  get; private set; }

	public void SetData(ToyData data, int count, int cost, bool isuserDeck)
    {
        toyImage.sprite = GameObjectManager.IconResource.GetToyImage(data.ModelCode);
        multipleText.text = count <= 1 ? "" : $"x{count}";
        costText.text = cost.ToString();
        Cost = cost;
        toyData = data;
    }
}
