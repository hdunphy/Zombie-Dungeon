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
    public HowToPlayMenu HowToPlayMenu;

    private float outPositionX;
    private float outPositionY;

    private void Start()
    {
        outPositionX = -MenuPanel.rect.width;
        outPositionY = MenuPanel.rect.height;
        MenuPanel.anchoredPosition = new Vector2(outPositionX, 0);
        SettingsMenu.SetAnimationSpeed(animationSpeed);
        HowToPlayMenu.SetAnimationSpeed(animationSpeed);
        AnimateInLeft();
    }

    public void AnimateInLeft()
    {
        LeanTween.moveX(MenuPanel, 0, animationSpeed).setEase(LeanTweenType.easeOutQuad);
    }

    public void AnimateInUp()
    {
        LeanTween.moveY(MenuPanel, 0, animationSpeed).setEase(LeanTweenType.easeOutQuad);
    }

    public void OnPressedPlay()
    {
        AudioManager.Instance.ToggleSound("Title Music", false);
        AudioManager.Instance.PlaySound("Music");
        SceneManager.LoadScene("Game");
    }

    public void OnPressedOptions()
    {
        LeanTween.moveX(MenuPanel, outPositionX, animationSpeed).setEase(LeanTweenType.easeOutQuad);
        SettingsMenu.AnimateIn();
    }

    public void OnPressedHow()
    {
        LeanTween.moveY(MenuPanel, outPositionY, animationSpeed).setEase(LeanTweenType.easeOutQuad);
        HowToPlayMenu.AnimateIn();
    }

    public void OnPressedQuit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
