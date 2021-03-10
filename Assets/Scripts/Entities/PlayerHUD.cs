using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private TMP_Text AmmoAmmountText;
    [SerializeField] private Image AmmoRenderer;
    [SerializeField] private List<Sprite> AmmoSprites;

    [Header("Health")]
    [SerializeField] private RectTransform HealthBarTransform;


    public static PlayerHUD Instance;

    private void Awake()
    {
        if(Instance == null)
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

    public void UpdateAmmoType(AmmoType ammoType)
    {
        AmmoRenderer.sprite = GetSpriteFromAmmoType(ammoType);
    }

    public Sprite GetSpriteFromAmmoType(AmmoType ammoType)
    {
        return AmmoSprites[(int)ammoType];
    }

    public void SetHealthPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0, 1);
        HealthBarTransform.localScale = new Vector3(percent, 1);
    }
}
