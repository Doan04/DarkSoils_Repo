using UnityEngine;

public class MouthScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public float damage;
    private float attackTimer = 3.5f;
    private float stunnedTimer;
    private float despawnTimer = 5f;
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
        despawnTimer -= Time.deltaTime;
        if(despawnTimer <= 0)
        {
            Destroy(gameObject);
        }
        if(attackTimer <= 0 && (player.transform.position - gameObject.transform.position).magnitude < 15)
        {
            attackTimer = 3.5f;
            anim.Play("MouthAttack");
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody2D projectileRB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = player.transform.position - transform.position;
            projectileRB.AddForce(direction.normalized * 5, ForceMode2D.Impulse);
        }
    }
}
