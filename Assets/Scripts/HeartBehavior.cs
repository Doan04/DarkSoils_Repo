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
    private bool audioPlaying = false;
    private bool dead = false;
    public Animator animator;
    private float hitTimer = 0f;
    private bool hit = false;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        circleShootTimer -= Time.deltaTime;
        hitTimer -= Time.deltaTime;
        if((player.transform.position - gameObject.transform.position).magnitude <= 10)
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
        health -= 5;
        heartAudio.PlayOneShot(hitSound);

        if(!hit)
        {
            animator.SetBool("hit", true);
            hit = true;
            hitTimer = 0.3f;   
        }
        
        if (health <= 0)
        {
            dead = true;
            Instantiate(bossDrop, transform.position, Quaternion.Euler(0, 0, 0));
            heartAudio.PlayOneShot(deathSound);
            Destroy(gameObject, 7f);
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
        for(int i = 0; i < 15; i++)
        {
            firePoint.Rotate(0, 0, 24);
            GameObject liveProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            Rigidbody2D projectileRB = liveProjectile.GetComponent<Rigidbody2D>();
            projectileRB.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
        }
    }
}
