using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public float masterPitch;
    public float bgmPitch;
    public float sfxPitch;

    [SerializeField] private AudioMixer soundMixer;

	private void OnEnable()
	{
        var data = SaveLoadManager.Data;
        masterPitch = data.masterPitch;
        bgmPitch = data.bgmPitch;
        sfxPitch = data.sfxPitch;

        soundMixer.SetFloat(SoundGroup.Master, Mathf.Log10(masterPitch) * 20);
        soundMixer.SetFloat(SoundGroup.BGM, bgmPitch);
        soundMixer.SetFloat(SoundGroup.SFX, sfxPitch);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    soundMixer.SetFloat(SoundGroup.Master, Mathf.Log10(masterPitch) * 20);
        soundMixer.SetFloat(SoundGroup.BGM, bgmPitch);
        soundMixer.SetFloat(SoundGroup.SFX, sfxPitch);
    }
}
