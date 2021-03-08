using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] private int MinDrop;
    [SerializeField] private int MaxDrop;

    private int ammo;
    private AmmoType AmmoType;

    // Start is called before the first frame update
    void Start()
    {
        ammo = Random.Range(MinDrop, MaxDrop);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().AddAmmo(ammo);
            Destroy(gameObject);
        }
    }
}
