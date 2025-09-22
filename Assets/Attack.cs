using UnityEngine;

public class Attack : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector2 direction;

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
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
