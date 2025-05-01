using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    public float health = 100f;
    public float shootTimer = 5f;
    public float circleShootTimer = 5f;
    public float shootInterval = 5f;
    public float circleShootInterval = 5f;
    public float projectileForce = 20f;
    public float rotation = 0f;
    public GameObject bossDrop;
    public AudioSource heartAudio;
    public AudioClip hitSound;
    public AudioClip deathSound;
    private GameObject player;
    public GameObject projectile;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
    public Transform firePoint5;
    public GameObject minion;
    private bool audioPlaying = false;
    private bool dead = false;
    public Animator animator;
    private float hitTimer = 0f;
    private bool hit = false;
    private bool spawnedDrop = false;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        circleShootTimer -= Time.deltaTime;
        hitTimer -= Time.deltaTime;
        if((player.transform.position - gameObject.transform.position).magnitude <= 15)
        {
            if(shootTimer <= 0 && !dead)
            {
                Shoot(firePoint1, rotation);
                Shoot(firePoint2, rotation);
                Shoot(firePoint3, rotation);
                Shoot(firePoint4, rotation);
                shootTimer = shootInterval;
            }

            if(circleShootTimer <= 0 && !dead)
            {
                ShootCircle(firePoint5, rotation);
                circleShootTimer = circleShootInterval;
            }
        }

        if((player.transform.position - gameObject.transform.position).magnitude <= 10 && !audioPlaying)
        {
            audioPlaying = true;
            heartAudio.Play();
        }
        else if((player.transform.position - gameObject.transform.position).magnitude > 10 && audioPlaying)
        {
            audioPlaying = false;
            heartAudio.Stop();
        }

        if(hitTimer < 0 && hit)
        {
            animator.SetBool("hit", false);
            hit = false;
        }
            
    }

    public void TakeDamage() // Code that gets called when the heart takes Damage
    {
        if(!hit)
        {
            health -= 5;
            heartAudio.PlayOneShot(hitSound);
            animator.SetBool("hit", true);
            hit = true;
            hitTimer = 0.3f;   
        }
        
        if (health <= 0)
        {
            dead = true;
            if(!spawnedDrop)
            {
                Instantiate(bossDrop, transform.position, Quaternion.Euler(0, 0, 0));
                spawnedDrop = true;
            }
            heartAudio.volume = 0.1f;
            heartAudio.PlayOneShot(deathSound);
            Invoke("setHeartInactive", 7f);
        }

    }

    void Shoot(Transform firePoint, float rotation)
    {
        firePoint.Rotate(0, 0, rotation);
        GameObject liveProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = liveProjectile.GetComponent<Rigidbody2D>();
        projectileRB.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    }

    void ShootCircle(Transform firePoint, float rotation)
    {
        firePoint.Rotate(0, 0, rotation);
        Instantiate(minion, firePoint.position + new Vector3(2, 0, 0), firePoint.rotation);
        Instantiate(minion, firePoint.position + new Vector3(-2, 0, 0), firePoint.rotation);
        for(int i = 0; i < 15; i++)
        {
            firePoint.Rotate(0, 0, 24);
            GameObject liveProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            Rigidbody2D projectileRB = liveProjectile.GetComponent<Rigidbody2D>();
            projectileRB.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
        }
    }

    void setHeartInactive()
    {
        gameObject.SetActive(false);
    }
}
