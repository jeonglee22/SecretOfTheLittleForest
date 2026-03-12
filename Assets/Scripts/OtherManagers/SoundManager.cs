using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public float masterPitch;
    public float bgmPitch;
    public float sfxPitch;
	private float bgmPos;

    public Slider bgmSlider;
    public Slider sfxSlider;

    [SerializeField] private AudioMixer soundMixer;
    [SerializeField] private AudioSource backgroundMusic;

	private void OnEnable()
	{
        var data = SaveLoadManager.Data;
		bgmPos = data.bgmPos;
        
        masterPitch = data.masterPitch;
        bgmPitch = data.bgmPitch;
        sfxPitch = data.sfxPitch;
	}

	private void OnDisable()
	{
		SaveLoadManager.Data.bgmPos = bgmPos;
		SaveLoadManager.Data.masterPitch = masterPitch;
		SaveLoadManager.Data.bgmPitch = bgmPitch;
		SaveLoadManager.Data.sfxPitch = sfxPitch;
		SaveLoadManager.Save();
	}

	private void Start()
	{
		backgroundMusic.Play();
		if (bgmPos <= 0 || bgmPos >= backgroundMusic.clip.length)
			bgmPos = 0f;
		backgroundMusic.time = bgmPos;

		bgmSlider.onValueChanged.AddListener(SetBgmValue);
		bgmSlider.value = bgmPitch;
		sfxSlider.onValueChanged.AddListener(SetSfxValue);
		sfxSlider.value = sfxPitch;

		soundMixer.SetFloat(SoundGroup.Master, masterPitch);
		soundMixer.SetFloat(SoundGroup.BGM, Mathf.Log10(bgmPitch) * 20f);
		soundMixer.SetFloat(SoundGroup.SFX, Mathf.Log10(sfxPitch) * 20f);
	}

	private void Update()
	{
		bgmPos = backgroundMusic.time;
	}

    private void SetBgmValue(float value)
    {
        value = Mathf.Clamp(value, 0.001f, 1f);
		soundMixer.SetFloat(SoundGroup.BGM, Mathf.Log10(value) * 20f);
        bgmPitch = value;
	}

    private void SetSfxValue(float value)
    {
		value = Mathf.Clamp(value, 0.001f, 1f);
		soundMixer.SetFloat(SoundGroup.SFX, Mathf.Log10(value) * 20f);
        sfxPitch = value;
	}

	public void SaveData()
	{
		SaveLoadManager.Data.bgmPos = bgmPos;
		SaveLoadManager.Data.masterPitch = 1f;
		SaveLoadManager.Data.bgmPitch = bgmPitch;
		SaveLoadManager.Data.sfxPitch = sfxPitch;
	}
}
