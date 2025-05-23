using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private float stunnedTimer;
    public GameObject coin;
    public AudioSource enemyAudio;
    public AudioClip enemyHurtSound;
    public int enemyID;
    public GameObject shopBoundary;
    public NPCScript npcScript;
    public bool diedToPlayer;
    void Start()
    {
        shopBoundary = GameObject.Find("ShopBoundary");
        npcScript = shopBoundary.GetComponent<NPCScript>();
    }
    // Update is called once per frame
    void Update()
    {
        stunnedTimer -= Time.deltaTime;
        if (stunnedTimer > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void TakeDamage(int dmg) // Code that gets called when enemy takes Damage
    {
        if(stunnedTimer <= 0)
        {
            health -= dmg;
            stunnedTimer = 0.5f;
            enemyAudio.PlayOneShot(enemyHurtSound);
        }
        if (health <= 0)
        {
            diedToPlayer = true;
            DeathEvent();
        }
    }

    public void DeathEvent()
    {
        enemyAudio.PlayOneShot(enemyHurtSound);
        Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
        GetComponent<SpriteRenderer>().enabled = false;
        GruntScript gscript = GetComponent<GruntScript>();
        EnemyScript enemyScript = GetComponent<EnemyScript>();
        // MouthScript mouthscript = GetComponent<MouthScript>();
        HeartMinionScript heartMinionScript = GetComponent<HeartMinionScript>();
        if (gscript != null)
        {
            gscript.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (enemyScript != null)
        {
            enemyScript.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (heartMinionScript != null)
        {
            heartMinionScript.enabled = false;
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
}
