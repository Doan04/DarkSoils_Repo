using UnityEngine;

public class FertProjectileScript : MonoBehaviour
{
    public int dmg = 4;
    private float projectileLife = 10f;
    public Rigidbody2D rb;
    void Update()
    {
        projectileLife -= Time.deltaTime;
        if (projectileLife <= 0)
        {
            Destroy(gameObject);
        }
        rb.AddForce(transform.up * 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Enemy"))
        {
            EnemyHealth ems = collidedObject.GetComponent<EnemyHealth>();
            if(ems != null)
            {
                ems.TakeDamage(dmg);
            }
            Destroy(gameObject);
        }
    }
}
