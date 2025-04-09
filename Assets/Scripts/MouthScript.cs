using UnityEngine;

public class MouthScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public float health;
    public float damage;
    private bool attackTimer;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int dmg) // Code that gets called when enemy takes Damage
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
