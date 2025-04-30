using UnityEngine;

public class AttackSpeedPickup : MonoBehaviour
{
    public float attackRate = 0.35f; // the new attack speed

    // increases the players attack speed when they collide
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().swingCooldownTime = attackRate;
            Destroy(gameObject);
        }
    }
}
