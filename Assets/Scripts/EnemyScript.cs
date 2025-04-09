using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //enemyTypes:
    //1 = zombie, 2 = shanker, 3 = eye
    //4 = miniboss
    public int enemyType;
    public GameObject player;
    public float health;
    public float damage;
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
            damage = 2;
            health = 10;
        }
        else
        {
            damage = 10;
            health = 10;
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
        //Chasing crops
        else
        {
            transform.up = new Vector3(0, 0, 0) - gameObject.transform.position;
        }
        transform.position += transform.up * Time.deltaTime;
        //Idle pose if needbe
        // else
        // {
        //    changeDirectionTimer -= Time.deltaTime;
        //     if(changeDirectionTimer < 0)
        //     {
        //         changeDirectionTimer = 5;
        //         transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        //     }
        //     transform.position += 0.3f * transform.right * Time.deltaTime;
        // }
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
