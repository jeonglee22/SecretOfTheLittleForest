using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatShowManager : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI shield;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI heart;
    public TextMeshProUGUI attack;

    public GameObject imageShowGrid;
    private GameObject currentShown;

    private ChoosingUnitManager choosingUnitManager;

	private void Awake()
	{
		choosingUnitManager = GetComponent<ChoosingUnitManager>();
	}

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

        if (currentShown != null)
        {
            var toy = currentShown.AddComponent<Toy>();
            toy.Data = data;
            toy.SetData();
        }
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

    public void RemoveGridImage()
    {
        if(currentShown != null) Destroy(currentShown);
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(eventData.pointerEnter == currentShown)
        {
            choosingUnitManager.AddToyOnChoosedDeck(currentShown);
        }
	}
}
