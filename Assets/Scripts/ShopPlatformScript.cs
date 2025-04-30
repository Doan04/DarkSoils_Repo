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
    public bool currentlyOnShopPlatform;
    public bool purchasing;
    public GameObject popup;
    public GameObject description;
    public AudioSource[] audioText;
    void Start()
    {
        popup = gameObject.transform.GetChild(0).gameObject;    
        description = gameObject.transform.GetChild(1).gameObject;
        timesBought = 0;
        currentlyOnShopPlatform = false;
        purchasing = false;
        audioText = GameObject.Find("AudioManager").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentlyOnShopPlatform)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                purchasing = true;
                audioText[1].Play();
            }
        }
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
            StopAllCoroutines();
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
            else if(wares == 3)
            {
                price = 10;
                StartCoroutine(changeDialogue(description, "Full Restore\n Price: " + price));
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        //The player in this case
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            currentlyOnShopPlatform = true;
            //Every time an item is bought increase price by 1
            //Each ware has a custom multiplier
            //Note do not do input on physics functions its buggy
            if(purchasing && collidedObject.GetComponent<PlayerScript>().money >= price)
            {
                //Special case for health restore don't buy if at max health
                if(wares != 3 || collidedObject.GetComponent<PlayerScript>().currentHealth != collidedObject.GetComponent<PlayerScript>().maxHealth)
                {
                    StopAllCoroutines();
                    StartCoroutine(changeDialogue(popup, "Press E to Buy"));
                    collidedObject.GetComponent<PlayerScript>().money -= price;
                    timesBought += 1;
                }
                if(wares == 0)
                {
                    price = 3 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Damage\n Price: " + price));
                    collidedObject.GetComponent<PlayerScript>().attack += 1;

                }
                else if(wares == 1)
                {
                    price = 5 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Max Health\n Price: " + price));
                    collidedObject.GetComponent<PlayerScript>().currentHealth += 15;
                    collidedObject.GetComponent<PlayerScript>().maxHealth += 15;                
                }
                else if(wares == 2)
                {
                    price = 6 * (timesBought + 1);
                    StartCoroutine(changeDialogue(description, "Max Speed\n Price: " + price));
                    collidedObject.GetComponent<PlayerScript>().playerSpeed += 1;       
                }
                //Can only buy 25% restore if not at full health
                else if(wares == 3 && collidedObject.GetComponent<PlayerScript>().currentHealth != collidedObject.GetComponent<PlayerScript>().maxHealth)
                {
                    StartCoroutine(changeDialogue(description, "25% Restore\n Price: " + price));
                    collidedObject.GetComponent<PlayerScript>().currentHealth += 0.25f * collidedObject.GetComponent<PlayerScript>().maxHealth;  
                    if(collidedObject.GetComponent<PlayerScript>().currentHealth > collidedObject.GetComponent<PlayerScript>().maxHealth)
                    {
                        collidedObject.GetComponent<PlayerScript>().currentHealth = collidedObject.GetComponent<PlayerScript>().maxHealth;
                    }
                    audioText[2].Play();
                }      
                purchasing = false;   
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            popup.SetActive(false);
            description.SetActive(false);
            audioText[0].Stop();
        }
        currentlyOnShopPlatform = false;
    }

    IEnumerator changeDialogue(GameObject popup, string text)
    {
        float timer = 0.005f;
        audioText[0].Play();
        for(int i = 0; i < text.Length; i++)
        {
            popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(text.Substring(0, i+1));
            yield return new WaitForSeconds(timer);
        }   
        audioText[0].Stop();
    }
}
