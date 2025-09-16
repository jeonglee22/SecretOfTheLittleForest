using TMPro;
using UnityEngine;

public class TextSetting : MonoBehaviour
{
	private TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		text.outlineColor = Color.black;
		text.outlineWidth = 0.2f;
	}
}
