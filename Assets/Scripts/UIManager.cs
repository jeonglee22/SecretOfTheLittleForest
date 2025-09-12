using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayLogic playLogic;
    public ToyControl toyControl;

    public TMP_Dropdown dropdown;

    public Toy toy;

    public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.MoveType = moveType;
        playLogic.ClearNodes();
    }

    public void OnClickSetToy()
    {
        toyControl.Toy = toy;
    }
}
