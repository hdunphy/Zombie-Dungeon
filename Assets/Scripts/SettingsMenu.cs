using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public TMPro.TMP_Dropdown ResolutionDropdown;
    public Toggle FullScreenToggle;
    public TMPro.TMP_Dropdown QualityDropdown;
    public Slider MusicVolSlider;
    public Slider SoundVolSlider;

    private int qualitySettings;
    private int height;
    private int width;
    private bool isFullScreen;
    private float musicVolume;
    private float soundVolume;

    List<Resolution> resolutions;

    private void Start()
    {
        LoadSettings();

        resolutions = Screen.resolutions.Distinct().ToList();

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentindex = 0;

        Debug.Log($"Screen size: {Screen.width} x {Screen.height}");
        Debug.Log($"Screen resolution {Screen.currentResolution}");

        for(int i = 0; i< resolutions.Count; i++)
        {
            int _width = resolutions[i].width;
            int _height = resolutions[i].height;
            string option = _width + " x " + _height;
            options.Add(option);

            if (_width == width && _height == height)
                currentindex = i;
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentindex;
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    public void SetMusicVolume(float _volume)
    {
        AudioMixer.SetFloat("MusicVol", Mathf.Log10(_volume) * 20);
        musicVolume = _volume;
    }

    public void SetSoundVolume(float _volume)
    {
        AudioMixer.SetFloat("SoundVol", Mathf.Log10(_volume) * 20);
        soundVolume = _volume;
    }

    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
        qualitySettings = _qualityIndex;
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool _isFullscreen)
    {
        Screen.fullScreen = _isFullscreen;
        isFullScreen = _isFullscreen;
    }

    public void OnBackPressed()
    {

    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetting", qualitySettings);
        PlayerPrefs.SetInt("IsFullScreen", Convert.ToInt32(isFullScreen));
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetInt("ScreenHeight", height);
        PlayerPrefs.SetInt("ScreenWidth", width);
    }

    private void LoadSettings()
    {
        if(PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight"))
        {
            height = PlayerPrefs.GetInt("ScreenWidth");
            width = PlayerPrefs.GetInt("ScreenWidth");
        }
        else
        {
            height = Screen.currentResolution.height;
            width = Screen.currentResolution.width;
        }

        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            qualitySettings = PlayerPrefs.GetInt("QualitySetting");
        }
        else
        {
            qualitySettings = 3;
        }

        if (PlayerPrefs.HasKey("IsFullScreen"))
        {
            isFullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("IsFullScreen"));
        }
        else
        {
            isFullScreen = true;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        else
            musicVolume = 1;
        if (PlayerPrefs.HasKey("SoundVolume"))
            soundVolume = PlayerPrefs.GetFloat("SoundVolume");
        else
            soundVolume = 1;

        //SetFullscreen(isFullScreen);
        //SetMusicVolume(musicVolume);
        //SetSoundVolume(soundVolume);
        //SetQuality(qualitySettings);
        FullScreenToggle.isOn = isFullScreen;
        MusicVolSlider.value = musicVolume;
        SoundVolSlider.value = soundVolume;
        QualityDropdown.value = qualitySettings;
    }
}
