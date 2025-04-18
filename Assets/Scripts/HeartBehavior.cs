using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    public float health = 100f;
    public float shootTimer = 5f;
    public float shootInterval = 5f;
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

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0 && !dead)
        {
            if((player.transform.position - gameObject.transform.position).magnitude <= 10)
            {
                Shoot(firePoint1, rotation);// going to add more, possibly make them move so they are harder to dodge
                Shoot(firePoint2, rotation);
                Shoot(firePoint3, rotation);
                Shoot(firePoint4, rotation);
                Shoot(firePoint5, rotation);
            }
            shootTimer = shootInterval;
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
            
    }

    public void TakeDamage() // Code that gets called when enemy takes Damage
    {
        health -= 5;
        heartAudio.PlayOneShot(hitSound);
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
}
