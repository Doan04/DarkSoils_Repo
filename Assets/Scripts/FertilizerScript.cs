using UnityEngine;

public class PickupsScript : MonoBehaviour
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

    // //Essentially if it touches the player then disappear and increment fertilizer count
    // private void OnTriggerStay2D(Collider2D collider)
    // {
    //     if(collider.gameObject.tag == "Player")
    //     {
    //         //If maxfert is 100 then if less than 90 just add 10
    //         //Else add the difference between current and the cap
    //         if(player.GetComponent<PlayerScript>().currentFert < player.GetComponent<PlayerScript>().maxFert - 10)
    //         {
    //             player.GetComponent<PlayerScript>().currentFert += 10;
    //         }
    //         else
    //         {
    //             player.GetComponent<PlayerScript>().currentFert += (player.GetComponent<PlayerScript>().maxFert - player.GetComponent<PlayerScript>().currentFert);
    //         }
    //         Destroy(gameObject);
    //     }
    // }
}
