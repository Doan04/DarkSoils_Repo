using UnityEngine;
using TMPro;
public class ShopPlatformScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //0 = damage
    //1 = health
    //2 = speed
    public int wares;
    public int timesBought;
    public int price;
    void Start()
    {
        timesBought = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //The player in this case
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            //Assuming that this first child is the gameobject with the popup text
            //And the second child is the gameobject having the gameobject child of
            //the description of item being bought
            GameObject popup = gameObject.transform.GetChild(0).gameObject;    
            popup.SetActive(true);
            GameObject description = gameObject.transform.GetChild(1).gameObject;
            description.SetActive(true);
            //Every time an item is bought increase price by 1
            //Each ware has a custom multiplier
            if(wares == 0)
            {
                price = 5 * (timesBought + 1);
                description.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Damage\n Price: " + price);
            }
            else if(wares == 1)
            {
                price = 3 * (timesBought + 1);
                description.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Max Health\n Price: " + price);
            }
            else if(wares == 2)
            {
                price = 7 * (timesBought + 1);
                description.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Health\n Price: " + price);
            }
            if(Input.GetKeyDown(KeyCode.E) && collidedObject.GetComponent<PlayerScript>().money >= price)
            {
                timesBought += 1;
                collidedObject.GetComponent<PlayerScript>().money -= price;
                if(wares == 0)
                {
                  collidedObject.GetComponent<PlayerScript>().attack += 2;
                }
                else if(wares == 1)
                {
                    collidedObject.GetComponent<PlayerScript>().currentHealth += 15;
                    collidedObject.GetComponent<PlayerScript>().maxHealth += 15;
                }
                else if(wares == 2)
                {
                    collidedObject.GetComponent<PlayerScript>().playerSpeed += 1;                   
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            GameObject popup = gameObject.transform.GetChild(0).gameObject;    
            popup.SetActive(false);
            GameObject description = gameObject.transform.GetChild(1).gameObject;
            description.SetActive(false);
        }
    }
}
