using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public float health;
    public float damage;
    
    public float changeDirectionTimer;
    //Just using this for testing purposes
    public float despawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        despawnTimer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        despawnTimer -= Time.deltaTime;
        if(despawnTimer <= 0)
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
            transform.right = player.transform.position - gameObject.transform.position;
        }
        //Chasing crops
        else
        {
            transform.right = new Vector3(0, 0, 0) - gameObject.transform.position;
        }
        transform.position += transform.right * Time.deltaTime;
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
}
