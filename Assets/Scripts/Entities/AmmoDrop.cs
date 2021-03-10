using System;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] private int MinDrop;
    [SerializeField] private int MaxDrop;
    [SerializeField] private SpriteRenderer Bullet_L;
    [SerializeField] private SpriteRenderer Bullet_M;
    [SerializeField] private SpriteRenderer Bullet_R;

    private int ammo;
    private AmmoType ammoType;

    // Start is called before the first frame update
    void Start()
    {
        AmmoType[] ammoTypes = (AmmoType[])Enum.GetValues(typeof(AmmoType));
        ammo = UnityEngine.Random.Range(MinDrop, MaxDrop);
        ammoType = ammoTypes[UnityEngine.Random.Range(0, ammoTypes.Length)];

        Sprite _ammoSprite = PlayerHUD.Instance.GetSpriteFromAmmoType(ammoType);
        Bullet_L.sprite = _ammoSprite;
        Bullet_M.sprite = _ammoSprite;
        Bullet_R.sprite = _ammoSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().AddAmmo(ammo, ammoType);
            Destroy(gameObject);
        }
    }
}
