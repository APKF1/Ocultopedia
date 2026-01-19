using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }
    
    public void SetVolume(float _value)
    {
        if(_value < 0.0001f)
        {
            _value = 0.0001f;
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        float volume = Mathf.Log10(_value) * 20f;
        Debug.Log(volume);
        masterMixer.SetFloat("MasterVolume", volume);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float _value)
    {
        soundSlider.value = _value;
    }
}
