using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1; // amount of money the coin gives the player

    // adds the value of the coin to the players money when they collide
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().money += coinValue;
            Destroy(gameObject);
        }
    }
}
