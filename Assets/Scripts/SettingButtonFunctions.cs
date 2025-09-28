using UnityEngine;
using UnityEngine.UI;

public class SettingButtonFunctions : MonoBehaviour
{
    public GameObject setting;
    public bool IsTeleport {  get; private set; }
	public Slider teleport;
	public bool IsVibrate { get; private set; }
	public Slider vibration;

	private void OnEnable()
	{
		var data = SaveLoadManager.Data;
		IsTeleport = data.isTeleport;
		IsVibrate = data.isVibrate;
	}
	private void OnDisable()
	{
		
	}

	private void Start()
	{
		teleport.value = IsTeleport ? 1 : 0;
		vibration.value = IsVibrate ? 1 : 0;
	}

	public void OnClickOpenSetting()
    {
        setting.SetActive(true);
    }

    public void OnClickCloseSetting()
    {
        setting.SetActive(false);
    }

	public void OnValueTeleportChange(float value)
	{
		IsTeleport = value == 1f;
		SaveLoadManager.Data.isTeleport = IsTeleport;
		SaveLoadManager.Save();
	}

	public void OnValueVibrationChange(float value)
	{
		IsVibrate = value == 1f;
		SaveLoadManager.Data.isVibrate = IsVibrate;
		SaveLoadManager.Save();
	}
}
