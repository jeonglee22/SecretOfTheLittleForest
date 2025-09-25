using TMPro;
using UnityEngine;

public class PresetPanelData : MonoBehaviour
{
	public TextMeshProUGUI presetName;

	public void SetData(string name)
	{
		presetName.text = name;
	}
}
