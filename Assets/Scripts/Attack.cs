using System;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public enum Caster { Player, Enemy }

public class Attack : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector2 direction;
    public GameObject Explosion;
    public SpellData spellData;
    public Caster caster;
    // public ShakeData shake;

    public void Initialize(int dmg, float spd, Vector2 dir, Caster cstr)
    {
        damage = dmg;
        speed = spd;
        direction = dir.normalized;
        caster = cstr;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (caster == Caster.Player)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Instantiate(Explosion, this.transform.position, this.transform.rotation);
                enemy.SpellDamage(spellData);
                Destroy(gameObject);
            }
           
            // CameraShakerHandler.Shake(shake);
        }
        else if (caster == Caster.Enemy)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                Instantiate(Explosion, this.transform.position, this.transform.rotation);
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }


    }


}
