using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    public float health = 100f;
    public float shootTimer = 5f;
    public float shootInterval = 5f;
    public float projectileForce = 20f;
    public float rotateSpeed = 5f; // controls the speed of the firepoint that rotates towards the player
    public GameObject bossDrop;
    public AudioSource heartAudio;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public GameObject player;
    public GameObject projectile;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
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
                Shoot(firePoint1);// going to add more, possibly make them ove so thery are harder to dodge
                Shoot(firePoint2);
                Shoot(firePoint3);
                Shoot(firePoint4);
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

    void Shoot(Transform firePoint)
    {
        GameObject liveProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = liveProjectile.GetComponent<Rigidbody2D>();
        projectileRB.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    }
}
