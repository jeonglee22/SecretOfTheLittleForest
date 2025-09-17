using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatShowManager : MonoBehaviour
{
    public TextMeshProUGUI shield;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI heart;
    public TextMeshProUGUI attack;

    public GameObject imageShowGrid;
    private GameObject currentShown;

	private void Start()
	{
		shield.text = string.Empty;
		coin.text = string.Empty;
		heart.text = string.Empty;
		attack.text = string.Empty;
	}

	public void SetToyText(ToyData data)
    {
        shield.text = data.Rating.ToString();
        coin.text = data.Price.ToString();
        heart.text = data.HP.ToString();
        attack.text = data.Attack.ToString();
    }

    public void SetGridImage(Sprite sprite)
    {
        if (currentShown != null)
            Destroy(currentShown);

        var go = new GameObject();
        var image = go.AddComponent<Image>();
        image.sprite = sprite;
        currentShown = Instantiate(go, imageShowGrid.transform).gameObject;
        Destroy(go);
    }
}
