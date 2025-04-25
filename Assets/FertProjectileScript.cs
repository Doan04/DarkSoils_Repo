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
            EnemyScript ems = collidedObject.GetComponent<EnemyScript>();
            MouthScript mouth = collidedObject.GetComponent<MouthScript>();
            if (ems != null) 
            {
                ems.TakeDamage(dmg);
            }
            else if(mouth != null)
            {
                mouth.TakeDamage(dmg);
            }
            Destroy(gameObject);
        }
    }
}
