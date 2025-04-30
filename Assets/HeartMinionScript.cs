using UnityEngine;

public class HeartMinionScript : MonoBehaviour
{
    public GameObject player;
    public float damage;
    public float movementSpeed;
    public float attackTimer;
    public float despawnTimer;
    public Animator anim;
    public bool inContactWithPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 1f;
        player = GameObject.Find("Player");
        despawnTimer = 20f;
        damage = 5;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0) 
        {
            Destroy(gameObject);
        }
        Movement();
        if (attackTimer <= 0)
        {
            if(inContactWithPlayer)
            {
                anim.Play("GruntAttack");
                player.GetComponent<PlayerScript>().DamagePlayer(5);
                attackTimer = 2f;
            }
        }
    }
    public void Movement()
    {
        //Only face player if within a certain distance otherwise
        if ((player.transform.position - gameObject.transform.position).magnitude < 100)
        {
            transform.up = player.transform.position - gameObject.transform.position;
        }
        else
        {
            transform.up = new Vector3(0, 0, 0) - gameObject.transform.position;
        }
        transform.position += movementSpeed * transform.up * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            inContactWithPlayer = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            inContactWithPlayer = false;
        }
    }
}
