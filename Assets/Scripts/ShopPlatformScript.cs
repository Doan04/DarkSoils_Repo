using UnityEngine;
using TMPro;
using System.Collections;

public class ShopPlatformScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //0 = damage
    //1 = health
    //2 = speed
    public int wares;
    public int timesBought;
    public int price;
    public bool isSpeaking;
    public bool isSpeaking2;
    public GameObject popup;
    public GameObject description;
    void Start()
    {
        popup = gameObject.transform.GetChild(0).gameObject;    
        description = gameObject.transform.GetChild(1).gameObject;
        timesBought = 0;
        isSpeaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            //Assuming that this first child is the gameobject with the popup text
            //And the second child is the gameobject having the gameobject child of
            //the description of item being bought
            popup.SetActive(true);
            description.SetActive(true);
            if(!isSpeaking)
            {
                StartCoroutine(changeDialogue(popup, "Press E to Buy"));
                if(wares == 0)
                {
                    price = 5 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Damage\n Price: " + price));
                }
                else if(wares == 1)
                {
                    price = 3 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Max Health\n Price: " + price));
                }
                else if(wares == 2)
                {
                    price = 7 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Max Speed\n Price: " + price));
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        //The player in this case
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            //Every time an item is bought increase price by 1
            //Each ware has a custom multiplier
            if(Input.GetKeyDown(KeyCode.E) && collidedObject.GetComponent<PlayerScript>().money >= price)
            {
                timesBought += 1;
                if(!isSpeaking)
                {
                    StartCoroutine(changeDialogue(popup, "Press E to Buy"));
                    if(wares == 0)
                    {
                        price = 5 * (timesBought + 1);
                        StartCoroutine(changeDialogue(description, "Damage\n Price: " + price));
                    }
                    else if(wares == 1)
                    {
                        price = 3 * (timesBought + 1);
                        StartCoroutine(changeDialogue(description, "Max Health\n Price: " + price));
                    }
                    else if(wares == 2)
                    {
                        price = 7 * (timesBought + 1);
                        StartCoroutine(changeDialogue(description, "Max Speed\n Price: " + price));
                    }
                }                
                collidedObject.GetComponent<PlayerScript>().money -= price;
                if(wares == 0)
                {
                  collidedObject.GetComponent<PlayerScript>().attack += 2;
                }
                else if(wares == 1)
                {
                    collidedObject.GetComponent<PlayerScript>().currentHealth += 15;
                    collidedObject.GetComponent<PlayerScript>().maxHealth += 15;
                    Debug.Log("Curren health is " + collidedObject.GetComponent<PlayerScript>().currentHealth);
                    Debug.Log("Max health is" + collidedObject.GetComponent<PlayerScript>().currentHealth);
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
            popup.SetActive(false);
            description.SetActive(false);
        }
    }

    IEnumerator changeDialogue(GameObject popup, string text)
    {
        isSpeaking = true;
        float timer = 0.005f;
        for(int i = 0; i < text.Length; i++)
        {
            popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(text.Substring(0, i+1));
            yield return new WaitForSeconds(timer);
            Debug.Log(i);
        }   
        isSpeaking = false;
    }
}
