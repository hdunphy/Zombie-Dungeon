using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public RectTransform MenuPanel;
    public float animationSpeed;
    public SettingsMenu SettingsMenu;

    private float outPosition;

    private void Start()
    {
        outPosition = -MenuPanel.rect.width;
        MenuPanel.anchoredPosition = new Vector2(outPosition, 0);
        SettingsMenu.SetAnimationSpeed(animationSpeed);
        AnimateIn();
    }

    public void AnimateIn()
    {
        LeanTween.moveX(MenuPanel, 0, animationSpeed).setEase(LeanTweenType.easeOutQuad);
    }

    public void OnPressedPlay()
    {
        AudioManager.Instance.ToggleSound("Title Music", false);
        AudioManager.Instance.PlaySound("Music");
        SceneManager.LoadScene("Game");
    }

    public void OnPressedOptions()
    {
        LeanTween.moveX(MenuPanel, outPosition, animationSpeed).setEase(LeanTweenType.easeOutQuad);
        SettingsMenu.AnimateIn();
    }

    public void OnPressedQuit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
