using UnityEngine;

public class CoinScript : MonoBehaviour
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

    //Essentially if it touches the player then disappear and increment coin count
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerScript>().money += 1;
            Destroy(gameObject);
        }
    }
}
