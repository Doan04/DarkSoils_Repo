using UnityEngine;
using UnityEngine.AI;
public class ShankerBehavior : MonoBehaviour
{
    public GameObject Player;
    public GameObject coin;
    public PlayerScript playerScript;
    public Animator animator;
    public Transform target;
    bool inContact = false;
    NavMeshAgent agent;
    int attacksAvailable = 3;
    public float updateFrequency = .1f;
    public float attackFrequency = 1f;
    public AudioSource shankerAudio;
    public AudioClip shankerHitSound;
    public GameObject shopBoundary;
    public NPCScript npcScript;
    public bool diedToPlayer;
    public int enemyID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyID = 2;
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        shopBoundary = GameObject.Find("ShopBoundary");
        npcScript = shopBoundary.GetComponent<NPCScript>();
    }

    // Update is called once per frame
    void Update()
    {
        updateFrequency -= Time.deltaTime;
        attackFrequency -= Time.deltaTime;
        if (updateFrequency < 0)
        {
            agent.SetDestination(target.position);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            float distance = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y).magnitude;
        }
        if (attackFrequency < 0 && inContact) 
        {
            animator.Play("ShankerAttack");
            //Destroy(Player.gameObject);
            playerScript.DamagePlayer(10);
            playerScript.sprayBlood(transform.position);
            attacksAvailable--;
            if(attacksAvailable <= 0)
            {
                Destroy(gameObject);
            }
            attackFrequency = 1f;
        }
        var vel = agent.velocity;
        vel.z = 0;
        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
        }
    }

    public void DeathEvent()
    {
        Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
        GetComponent<SpriteRenderer>().enabled = false;
        shankerAudio.PlayOneShot(shankerHitSound);
        ShankerBehavior script = GetComponent<ShankerBehavior>();
        if (script != null)
        {
            script.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        Invoke("EnemyDie", 0.5f);
    }

    public void EnemyDie()
    {
        if(diedToPlayer)
        {
            npcScript.questUpdate(enemyID);
            Debug.Log("died to player");
        }
        else
        {
            Debug.Log("died naturally");
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) {
            inContact = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            inContact = false;
        }
    }
}
