using System.Text;
using TMPro;
using UnityEngine;

public class ReadyCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI cameraText;

    private void OnEnable()
    {

    }

    private void Start()
    {
        SetUnitCountText(GetCountInBoard());
        SetCameraText(false);
    }

    private int GetCountInBoard()
    {
        var count = 0;
        var fieldToys = SaveLoadManager.Data.Deck.Pos;
        foreach (var item in fieldToys)
        {
            if (item != 0)
                count++;
        }

        return count;
    }

    public void SetUnitCountText(int count)
    {
        var sb = new StringBuilder();
        var maxCount = SaveLoadManager.Data.unitCount;

        sb.Append("��ġ ����\n").Append($"({count}/{maxCount})");
        unitCountText.text = sb.ToString();

        if (count == maxCount)
        {
            unitCountText.color = Color.red;
        }
        else
        {
            unitCountText.color = Color.white;
        }
    }

    public void SetCameraText(bool isGameView)
    {
        if (isGameView)
        {
            cameraText.text = "��ü ȭ��";
        }
        else
        {
            cameraText.text = "��ġ ȭ��";
        }
    }
}
