using UnityEngine;

public class FertilizerScript : MonoBehaviour
{
    public float fertValue = 10f; // amount of fertilizer it gives the player

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            // the player will not pick it up if they have the max amount already
            if(collidedObject.GetComponent<PlayerScript>().currentFert < collidedObject.GetComponent<PlayerScript>().maxFert)
            {
                collidedObject.GetComponent<PlayerScript>().currentFert += fertValue;
                // ensures the player doesn't go above the max amount
                if(collidedObject.GetComponent<PlayerScript>().currentFert > collidedObject.GetComponent<PlayerScript>().maxFert)
                {
                    collidedObject.GetComponent<PlayerScript>().currentFert = collidedObject.GetComponent<PlayerScript>().maxFert;
                }
                Destroy(gameObject);
            }
        }
    }
}
