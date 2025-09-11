using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayLogic playLogic;

    public TMP_Dropdown dropdown;

    public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.MoveType = moveType;
    }
}
