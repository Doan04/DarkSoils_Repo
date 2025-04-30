using UnityEngine;

public class HeartProjectileScript : MonoBehaviour
{
    public int dmg = 10;
    private float projectileLife = 10f;

    void Update()
    {
        projectileLife -= Time.deltaTime;
        if(projectileLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().DamagePlayer(dmg);
            collidedObject.GetComponent<PlayerScript>().sprayBlood(transform.position);
            Destroy(gameObject);
        }
        else if (collidedObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        
    }
}
