using UnityEngine;
using UnityEngine.AI;
public class RatBehavior : MonoBehaviour
{
    public GameObject generator;
    public GeneratorScript generatorScript;
    public Animator animator;
    public Transform target;
    public bool inContact = false;
    NavMeshAgent agent;
    public float updateFrequency = .1f;
    public float attackFrequency = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generator = GameObject.Find("generator");
        generatorScript = generator.GetComponent<GeneratorScript>();
        animator = GetComponent<Animator>();
        target = generator.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        attackFrequency -= Time.deltaTime;
        if (attackFrequency < 0 && inContact)
        {
            animator.Play("ShankerAttack");
            attackFrequency = 0.5f;
            generatorScript.TakeDamage(20);
        }
        var vel = agent.velocity;
        vel.z = 0;
        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Generator"))
        {
            inContact = true;
            agent.isStopped = true;
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("Player"))
    //    {
    //        inContact = false;
    //    }
    //}
}
