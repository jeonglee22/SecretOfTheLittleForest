using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentPresetPanelData : MonoBehaviour
{
	public Image toyImage;
	public TextMeshProUGUI multipleText;
	public TextMeshProUGUI heartText;
	public TextMeshProUGUI attackText;
	public Image kingImage;
	public TouchManager touchManager;
	private ToyData toyData;
	public ToyData ToyData { get { return toyData; } }

	public int Cost { get; private set; }

	public void SetData(ToyData data, int count, bool isKing)
	{
		toyImage.sprite = GameObjectManager.IconResource.GetToyImage(data.ModelCode);
		kingImage.sprite = isKing ? Resources.Load<Sprite>(Icons.crownPath) : null;
		kingImage.color = isKing ? new Color(1f, 1f, 0f, 1f) : new Color(1f, 1f, 1f, 0f);
		multipleText.text = count <= 1 ? "" : $"x{count}";
		heartText.text = data.HP.ToString();
		attackText.text = data.Attack.ToString();
		toyData = data;
	}
}
