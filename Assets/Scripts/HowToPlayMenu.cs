using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    public RectTransform HowToPlayPanel;

    private float animationSpeed;
    private float offscreenPosition;
    // Start is called before the first frame update
    void Start()
    {
        offscreenPosition = -HowToPlayPanel.rect.height;
        HowToPlayPanel.anchoredPosition = new Vector2(0, offscreenPosition);
    }

    public void SetAnimationSpeed(float _animationSpeed)
    {
        animationSpeed = _animationSpeed;
    }

    public void AnimateIn()
    {
        LeanTween.moveY(HowToPlayPanel, 0, animationSpeed).setEase(LeanTweenType.easeOutQuad);
    }

    public void OnBackPressed()
    {
        LeanTween.moveY(HowToPlayPanel, offscreenPosition, animationSpeed).setEase(LeanTweenType.easeOutQuad);
        FindObjectOfType<MainMenu>().AnimateInUp();
    }
}
