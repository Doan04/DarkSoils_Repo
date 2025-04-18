using UnityEngine;

public class HeartProjectileScript : MonoBehaviour
{
    public int dmg = 10;
    private float projectileLife = 15f;

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
        if(collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().DamagePlayer(dmg);
            Destroy(gameObject);
        }
        
    }
}
