using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float health;
    private float stunnedTimer;
    public GameObject coin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stunnedTimer -= Time.deltaTime;
        if (stunnedTimer > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void TakeDamage(int dmg) // Code that gets called when enemy takes Damage
    {
        if(stunnedTimer <= 0)
        {
            health -= dmg;
            stunnedTimer = 0.3f;
        }
        if (health <= 0)
        {
            Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
