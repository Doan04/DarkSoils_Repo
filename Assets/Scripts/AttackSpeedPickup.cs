using UnityEngine;

public class AttackSpeedPickup : MonoBehaviour
{
    public float attackRate = 0.5f; // how much the pickup affects the attack speed

    // increases the players attack speed when they collide
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().swingCooldownTime = (collidedObject.GetComponent<PlayerScript>().swingCooldownTime) * attackRate;
            Destroy(gameObject);
        }
    }
}
