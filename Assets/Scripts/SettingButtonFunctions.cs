using UnityEngine;
using UnityEngine.UI;

public class SettingButtonFunctions : MonoBehaviour
{
    public GameObject setting;
    public bool IsTeleport {  get; private set; }
	public Toggle teleport;

	private void OnEnable()
	{
		var data = SaveLoadManager.Data;
		IsTeleport = data.isTeleport;
	}
	private void OnDisable()
	{
		SaveLoadManager.Data.isTeleport = IsTeleport;
		SaveLoadManager.Save();
	}

	private void Start()
	{
		teleport.isOn = IsTeleport;
	}

	public void OnClickOpenSetting()
    {
        setting.SetActive(true);
    }

    public void OnClickCloseSetting()
    {
        setting.SetActive(false);
    }

	public void OnValueTeleportChange(bool b)
	{
		IsTeleport = b;
	}
}
