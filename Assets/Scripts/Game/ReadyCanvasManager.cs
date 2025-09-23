using System.Text;
using TMPro;
using UnityEngine;

public class ReadyCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI unitCountText;

	private void Start()
	{
        SetUnitCountText(0);
	}

	public void SetUnitCountText(int count)
    {
        var sb = new StringBuilder();
        SaveLoadManager.Load();
        var maxCount = SaveLoadManager.Data.unitCount;

		sb.Append("배치 가능\n").Append($"({count}/{maxCount})");
        unitCountText.text = sb.ToString();

        if(count == maxCount)
        {
            unitCountText.color = Color.red;
        }
        else
        {
			unitCountText.color = Color.white;
		}
    }
}
