using UnityEngine;
using TMPro;
using System.Collections;

public class NPCScript : MonoBehaviour
{
    public int npcID;
    public string[] dialoguePool;
    public bool currentlyOnNPCPlatform;
    public bool talk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        }       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        GameObject popup = gameObject.transform.GetChild(0).gameObject; 
        string text = "Press E to talk";
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
                GameObject popup = gameObject.transform.GetChild(0).gameObject; 
                StopAllCoroutines();
                int dialogueChoice = Random.Range(0, dialoguePool.Length);
                StartCoroutine(changeDialogue(popup, dialoguePool[dialogueChoice]));
                talk = false;
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
        }
    }

    IEnumerator changeDialogue(GameObject popup, string text)
    {
        float timer = 0.005f;
        for(int i = 0; i < text.Length; i++)
        {
            popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(text.Substring(0, i+1));
            yield return new WaitForSeconds(timer);
            Debug.Log(i);
        }   
    }

}
