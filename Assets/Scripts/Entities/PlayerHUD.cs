﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private TMP_Text AmmoAmmountText;
    [SerializeField] private Image AmmoRenderer;
    [SerializeField] private RectTransform ReloadBar;

    [Header("Guns")]
    [SerializeField] private Image SelectedGun;

    [Header("Health")]
    [SerializeField] private RectTransform HealthBarTransform;

    [Header("Info")]
    [SerializeField] private TMP_Text WaveText;
    [SerializeField] private TMP_Text ScoreText;


    public static PlayerHUD Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAmmoAmount(int currentClip, int totalAmmo)
    {
        AmmoAmmountText.text = $"{currentClip} / {totalAmmo}";
    }

    public void UpdateAmmoType(AmmoData ammoType)
    {
        AmmoRenderer.sprite = ammoType.Icon;
    }

    public void SetHealthPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0, 1);
        LeanTween.scale(HealthBarTransform, new Vector3(percent, 1), .5f);
    }

    public void SetSelectedGun(Sprite sprite)
    {
        SelectedGun.sprite = sprite;
        ReloadBar.gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    public void SetWaveNumber(int waveNumber)
    {
        WaveText.text = $"Wave {waveNumber}";
    }

    public void Reload(float seconds)
    {
        ReloadBar.localScale = new Vector3(0, 1, 1);
        ReloadBar.gameObject.SetActive(true);
        LeanTween.scale(ReloadBar, Vector3.one, seconds).
            setOnComplete(() => { ReloadBar.gameObject.SetActive(false); });
    }
}
