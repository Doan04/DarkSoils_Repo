using UnityEngine;

public class FertilizerScript : MonoBehaviour
{
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Essentially if it touches the player then disappear and increment fertilizer count
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //If maxfert is 100 then if less than 90 just add 10
            //Else add the difference between current and the cap
            if(player.GetComponent<PlayerScript>().currentFert < player.GetComponent<PlayerScript>().maxFert - 10)
            {
                player.GetComponent<PlayerScript>().currentFert += 10;
            }
            else
            {
                player.GetComponent<PlayerScript>().currentFert += (player.GetComponent<PlayerScript>().maxFert - player.GetComponent<PlayerScript>().currentFert);
            }
            Destroy(gameObject);
        }
    }
}
