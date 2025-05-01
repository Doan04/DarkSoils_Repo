using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public int enemyType;
    public GameObject player;
    public PlayerScript playerScript;
    public GameObject coin;
    public float damage;
    public float movementSpeed;
    public float attackTimer;
    public float changeDirectionTimer;
    //Just using this for testing purposes
    public float despawnTimer;
    public Animator anim;
    public bool reachedCrop = false;
    public CropScript cropField;
    public bool inContactWithPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 1f;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        despawnTimer = 7f;
        damage = 5;
        anim = GetComponent<Animator>();
        cropField = GameObject.Find("CropField").GetComponent<CropScript>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        despawnTimer -= Time.deltaTime;
        if(despawnTimer <= 0)
        {
            GetComponent<EnemyHealth>().EnemyDie();
        }
        if (reachedCrop == false)
        {
            Movement();
        }
        else
        {
            if (attackTimer <= 0)
            {
                cropField.TakeDamage(damage);
                anim.Play("GruntAttack");
                attackTimer = 2f;
            }
        }
        if (attackTimer <= 0)
        {
            if(inContactWithPlayer)
            {
                anim.Play("GruntAttack");
                player.GetComponent<PlayerScript>().DamagePlayer(5);
                playerScript.sprayBlood(transform.position);
                attackTimer = 2f;
            }
        }
    }

    public void Movement()
    {
        //Only face player if within a certain distance otherwise
        if ((player.transform.position - gameObject.transform.position).magnitude < 2)
        {
            transform.up = player.transform.position - gameObject.transform.position;
        }
        else
        {
            transform.up = new Vector3(0, 0, 0) - gameObject.transform.position;
        }
        transform.position += movementSpeed * transform.up * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidedObject = collider.gameObject;
        //If crop then attack
        if (collidedObject.CompareTag("Crop"))
        {
            reachedCrop = true;
            Debug.Log("trigger crop");
        }
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