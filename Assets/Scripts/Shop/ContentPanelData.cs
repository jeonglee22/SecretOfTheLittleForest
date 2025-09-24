using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentPanelData : MonoBehaviour
{
    public Image toyImage;
    public TextMeshProUGUI multipleText;
    public TextMeshProUGUI costText;
    public TouchManager touchManager;
    private ToyData toyData;

	public void SetData(ToyData data, int count, int cost)
    {
        toyImage.sprite = GameObjectManager.IconResource.GetToyImage(data.ModelCode);
        multipleText.text = count <= 1 ? "" : $"x{count}";
        costText.text = cost.ToString();
        toyData = data;
    }

    public void SetTapFunc(Action func)
    {
        touchManager.tapFunc = func;
    }
}
