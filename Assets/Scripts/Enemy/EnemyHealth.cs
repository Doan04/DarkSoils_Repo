using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private float stunnedTimer;
    public GameObject coin;
    public AudioSource enemyAudio;
    public AudioClip enemyHurtSound;

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
        stunnedTimer = 0.5f;
        if (health <= 0)
        {
            DeathEvent();
        }
    }

    public void DeathEvent()
    {
        Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
        GetComponent<SpriteRenderer>().enabled = false;
        GruntScript gscript = GetComponent<GruntScript>();
        MouthScript mouthscript = GetComponent<MouthScript>();
        HeartMinionScript heartMinionScript = GetComponent<HeartMinionScript>();
        if (gscript != null)
        {
            gscript.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (mouthscript != null)
        {
            mouthscript.enabled = false;
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
        Destroy(gameObject);
    }
}
