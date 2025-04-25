using UnityEngine;

public class MouthScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject coinPrefab;
    public GameObject player;
    public float health;
    public float damage;
    private float attackTimer = 3.5f;
    private float stunnedTimer;
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
        attackTimer -= Time.deltaTime;
        if(attackTimer < 0)
        {
            attackTimer = 2f;
            anim.Play("MouthAttack");
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody2D projectileRB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = player.transform.position - transform.position;
            projectileRB.AddForce(direction.normalized * 5, ForceMode2D.Impulse);
        }
    }
    public void TakeDamage(int dmg) // Code that gets called when enemy takes Damage
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(coinPrefab, transform.position, transform.rotation);
        }
    }
}
