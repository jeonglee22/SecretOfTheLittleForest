using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickObject : MonoBehaviour, IPointerClickHandler
{
	private Image connectedImage;
	private bool isClicked;
	private Color initColor;
	private SetObjectControl objectControl;

	private void Start()
	{
		connectedImage = GetComponent<Image>();
		isClicked = false;
		if(connectedImage != null )
			initColor = connectedImage.color;
		objectControl = transform.root.GetComponentInChildren<SetObjectControl>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		var go = eventData.pointerClick;
		isClicked = !isClicked;

		if (connectedImage != null && isClicked)
		{
			var color = initColor;
			color = color * 0.5f;
			color.a = initColor.a;
			connectedImage.color = color;
		}
		else if (connectedImage != null && !isClicked) 
		{
			connectedImage.color = initColor;
		}
	}
}
