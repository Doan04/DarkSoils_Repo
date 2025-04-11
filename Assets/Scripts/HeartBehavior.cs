using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int health = 100;
    public GameObject coin;
    public AudioSource heartAudio;
    public AudioClip deathSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage() // Code that gets called when enemy takes Damage
    {
        health -= 10;
        if (health <= 0)
        {
            Instantiate(coin, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }

    }
}
