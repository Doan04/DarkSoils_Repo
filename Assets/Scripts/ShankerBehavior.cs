using UnityEngine;
using UnityEngine.AI;
public class ShankerBehavior : MonoBehaviour
{
    public GameObject Player;
    public PlayerScript playerScript;
    public Animator animator;
    public Transform target;
    bool inContact = false;
    NavMeshAgent agent;
    public float updateFrequency = .1f;
    public float attackFrequency = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
            attackFrequency = 0.5f;
        }
        var vel = agent.velocity;
        vel.z = 0;
        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            inContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inContact = false;
        }
    }
}
