using System;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Attack : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector2 direction;
    public GameObject Explosion;
    public SpellData spellData;
    // public ShakeData shake;

    public void Initialize(int dmg, float spd, Vector2 dir)
    {
        damage = dmg;
        speed = spd;
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.CompareTag("Enemy"))
        // {
        //     Instantiate(Explosion, this.transform.position, this.transform.rotation);

        //     Destroy(gameObject);
        // }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Instantiate(Explosion, this.transform.position, this.transform.rotation);
            // CameraShakerHandler.Shake(shake);
            enemy.SpellDamage(spellData);
            Destroy(gameObject);
        }


    }

    
}
