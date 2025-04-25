using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //enemyTypes:
    //1 = zombie, 2 = shanker, 3 = mouth
    //4 = miniboss
    public int enemyType;
    public GameObject player;
    public GameObject coin;
    public float health;
    public float damage;
    public float movementSpeed;
    private float stunnedTimer;
    public float attackTimer;
    private bool attacking;
    public float changeDirectionTimer;
    //Just using this for testing purposes
    public float despawnTimer;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(enemyType == 1)
        {
            health = 5;
            damage = 4;
            movementSpeed = 1;
        }
        else if(enemyType == 2)
        {
            health = 10;
            damage = 10;
            movementSpeed = 2;
        }
        else
        {
            health = 5;
            damage = 20;
            movementSpeed = 0.5f;
        }
        player = GameObject.Find("Player");
        despawnTimer = 10;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        stunnedTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        despawnTimer -= Time.deltaTime;
        if(stunnedTimer > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(stunnedTimer < 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Movement();
        }
        //If this is the mouth automatically attack since it is ranged enemy
        if(enemyType == 3 && !anim.GetCurrentAnimatorStateInfo(0).IsName("MouthAttack") )
        {
            anim.Play("MouthAttack");          
        }
    }

    public void Movement()
    {
        //Only face player if within a certain distance otherwise 
        //If within a certain distance of the crops face the crops otherwise
        //Set a random direction every 5 seconds and move slower
        //Chasing player
        if((player.transform.position - gameObject.transform.position).magnitude < 4)
        {
            transform.up = player.transform.position - gameObject.transform.position;
        }
        //Chasing crops if melee
        else if(enemyType != 3)
        {
            transform.up = new Vector3(0, 0, 0) - gameObject.transform.position;
        }
        //Random slow walk for ranged enemies
        else
        {
           changeDirectionTimer -= Time.deltaTime;
            if(changeDirectionTimer < 0)
            {
                changeDirectionTimer = 3;
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
        }
        transform.position += movementSpeed * transform.up * Time.deltaTime;
    }

    public void TakeDamage(int dmg) // Code that gets called when enemy takes Damage
    {
        health -= dmg;
        stunnedTimer = 0.3f;
        if(health <= 0)
        {
            Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //If player then attack
        if(collision.gameObject.tag == "Player" && enemyType == 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GruntAttack"))
        {
            if (attackTimer < 0) 
            {
                anim.Play("GruntAttack");
                player.GetComponent<PlayerScript>().DamagePlayer(5);
                player.GetComponent<PlayerScript>().sprayBlood(transform.position);
            }
        }
        else if(collision.gameObject.tag == "Player" && enemyType == 2 && !anim.GetCurrentAnimatorStateInfo(0).IsName("ShankerAttack"))
        {
            anim.Play("ShankerAttack");
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        GameObject collidedObject = collider.gameObject;
        //If crop then attack
        if(collidedObject.CompareTag("Crop"))
        {
            if(attackTimer <= 0)
            {
                collider.gameObject.GetComponent<CropScript>().currentHealth -= damage;
            }
        }
        if(attackTimer <= 0)
        {
            Debug.Log("TOUCHING CROP");
            if(collidedObject.CompareTag("Crop") && enemyType == 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GruntAttack"))
            {
                anim.Play("GruntAttack");
            }
            else if(collidedObject.CompareTag("Crop") && enemyType == 2 && !anim.GetCurrentAnimatorStateInfo(0).IsName("ShankerAttack"))
            {
                anim.Play("ShankerAttack");
            }
            attackTimer = 2f;
        }
    }
}
