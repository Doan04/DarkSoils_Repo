using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //enemyTypes:
    //1 = zombie, 2 = shanker, 3 = mouth
    //4 = miniboss
    public int enemyType;
    public GameObject player;
    public float health;
    public float damage;
    public float movementSpeed;
    private float stunnedTimer;
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
            damage = 4;
            health = 10;
            movementSpeed = 1;
        }
        else if(enemyType == 2)
        {
            damage = 10;
            health = 10;
            movementSpeed = 2;
        }
        else
        {
            damage = 20;
            health = 5;
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
        if(stunnedTimer > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Movement();
        }
        despawnTimer -= Time.deltaTime;
        //If this is the mouth automatically attack since it is ranged enemy
        if(enemyType == 3 && !anim.GetCurrentAnimatorStateInfo(0).IsName("MouthAttack") )
        {
            anim.Play("MouthAttack");          
        }
        if(despawnTimer <= 0 || health <= 0)
        {
            Destroy(gameObject);
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

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Hit by a weapon, reduce its health
        if(collider.gameObject.tag == "MeleeHitbox" && stunnedTimer <= 0)
        {
            Debug.Log("hit");
            health -= 5;
            stunnedTimer = 0.3f;

        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && enemyType == 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GruntAttack"))
        {
            anim.Play("GruntAttack");
        }
        else if(collision.gameObject.tag == "Player" && enemyType == 2 && !anim.GetCurrentAnimatorStateInfo(0).IsName("ShankerAttack"))
        {
            anim.Play("ShankerAttack");
        }
    }

}
