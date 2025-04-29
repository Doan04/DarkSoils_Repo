using UnityEngine;
using TMPro;
using System.Collections;

public class NPCScript : MonoBehaviour
{
    //-1, anything purely readable, 0 = Shopkeeper, 1 = Fishing Area
    public int npcID;
    public string initialText;
    public string[] dialoguePool;
    public bool currentlyOnNPCPlatform;
    public bool talk;
    public bool sell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject popup = gameObject.transform.GetChild(0).gameObject; 
        initialText = popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        currentlyOnNPCPlatform = false;
        talk = false;
    }

    // Update is called once per frame
    void Update()
    {
         if(currentlyOnNPCPlatform)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                talk = true;
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                sell = true;
            }
        }       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        GameObject popup = gameObject.transform.GetChild(0).gameObject; 
        string text = "";
        if(npcID == -1)
        {
            text = initialText;
        }
        else if(npcID == 0)
        {
            text = "Press E to talk\nPress Q to sell fish";
        }
        else if(npcID == 1)
        {
            text = "Press E to fish";
        }
        if (collidedObject.CompareTag("Player"))
        {
            StartCoroutine(changeDialogue(popup, text));
            popup.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        //The player in this case
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            currentlyOnNPCPlatform = true;
            //Assuming that this first child is the gameobject with the popup text
            if(talk)
            {
                if(npcID == -1)
                {
                    GameObject popup = gameObject.transform.GetChild(0).gameObject; 
                    popup.SetActive(false);
                    talk = false;
                }
                else if(npcID == 0)
                {
                    GameObject popup = gameObject.transform.GetChild(0).gameObject; 
                    StopAllCoroutines();
                    int dialogueChoice = Random.Range(0, dialoguePool.Length);
                    StartCoroutine(changeDialogue(popup, dialoguePool[dialogueChoice]));
                }
                else if(npcID == 1)
                {
                    GetComponent<FishingScript>().fishingActivated = true;
                }
                talk = false;
            }
            if(sell)
            {
                if(npcID == 0)
                {
                    GameObject popup = gameObject.transform.GetChild(0).gameObject; 
                    StopAllCoroutines();
                    int numberOfFish = collision.gameObject.GetComponent<PlayerScript>().fish;
                    collision.gameObject.GetComponent<PlayerScript>().fish = 0;
                    collision.gameObject.GetComponent<PlayerScript>().money += 2 * numberOfFish;
                    StartCoroutine(changeDialogue(popup, (numberOfFish + " fish sold for " +  2 * numberOfFish)));
                    sell = false;
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
            StopAllCoroutines();
            currentlyOnNPCPlatform = false;
            if(npcID == 1)
            {
                GetComponent<FishingScript>().fishingActivated = false;
            }
        }
    }

    IEnumerator changeDialogue(GameObject popup, string text)
    {
        float timer = 0.005f;
        for(int i = 0; i < text.Length; i++)
        {
            popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(text.Substring(0, i+1));
            yield return new WaitForSeconds(timer);
        }   
    }

}
