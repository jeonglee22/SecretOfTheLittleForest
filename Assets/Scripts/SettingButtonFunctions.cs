using UnityEngine;

public class SettingButtonFunctions : MonoBehaviour
{
    public GameObject setting;

    public void OnClickOpenSetting()
    {
        setting.SetActive(true);
    }

    public void OnClickCloseSetting()
    {
        setting.SetActive(false);
    }
}
