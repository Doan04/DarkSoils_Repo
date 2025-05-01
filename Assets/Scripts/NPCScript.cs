using UnityEngine;
using TMPro;
using System.Collections;

public class NPCScript : MonoBehaviour
{
    //-1, anything purely readable, 0 = Shopkeeper, 1 = Fishing Area
    public int npcID;
    public string initialText;
    public string[] dialoguePool;
    public string[] questDialoguePool;
    //Only applicable at shopkeeper
    public ObjectiveManager questManager;
    public int questIndex;
    public bool currentlyOnNPCPlatform;
    public bool talk;
    public bool sell;
    public bool quest;
    //When this reaches 0, quest completed
    public float questValue;
    public AudioSource[] audioText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject popup = gameObject.transform.GetChild(0).gameObject; 
        initialText = popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        currentlyOnNPCPlatform = false;
        talk = false;
        sell = false;
        quest = false;
        questIndex = -1;
        questValue = -1;
        audioText = GameObject.Find("AudioManager").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // questValue -= 0.25f * Time.deltaTime;
        if(currentlyOnNPCPlatform)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                talk = true;
                audioText[1].Play();
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                sell = true;
                audioText[1].Play();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                quest = true;
                audioText[1].Play();

            }
        }    
        //Updating quest values live if shopkeeper
        if(npcID == 0 && questManager.npcText.enabled == true)
        {
            if(questIndex % 3 == 0)
            {
                questManager.EnableNPCQuest("Kill " + questValue + " grunts");
            }
            else if(questIndex % 3 == 1)
            {
                questManager.EnableNPCQuest("Kill " + questValue + " shooters");
            }
            else if(questIndex % 3 == 2)
            {
                questManager.EnableNPCQuest("Kill" + questValue + " shankers");
            }  
            //No matter what quest if quest is completed display completed quest
            if(questValue <= 0)
            {
                questManager.EnableNPCQuest("Quest Completed, return to shopkeeper for reward");
                questManager.npcText.color = Color.yellow;
            }
        }

    }

    public void questUpdate(int enemyID)
    {
        //Are we killing the matching enemy?
        if(enemyID == (questIndex % 3))
        {
            questValue--;
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
            text = "Press E to talk\nPress Q to sell fish\nPress R to quest";
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
                    if(GetComponent<FishingScript>().fishingActivated == false)
                    {
                        Debug.Log("FISHING");
                        GetComponent<FishingScript>().fishingActivated = true;
                    }
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
                }
                sell = false;
            }
            if(quest)
            {
                if(npcID == 0)
                {
                    GameObject popup = gameObject.transform.GetChild(0).gameObject; 
                    //Initializing quests only if completed
                    //If completed then increment quest
                    if(questValue <= 0)
                    {
                        questManager.npcText.color = Color.white;
                        questIndex++;
                        if(questIndex % 3 == 0)
                        {
                            if(questIndex != 0)
                            {
                                collidedObject.GetComponent<PlayerScript>().money += 75;
                            }
                            questValue = 10;
                            questManager.EnableNPCQuest("Kill " + questValue + " grunts");
                        }
                        else if(questIndex % 3 == 1)
                        {
                            collidedObject.GetComponent<PlayerScript>().money += 75;
                            questValue = 7;
                            questManager.EnableNPCQuest("Kill " + questValue + " shooters");
                        }
                        else if(questIndex % 3 == 2)
                        {
                            collidedObject.GetComponent<PlayerScript>().money += 75;
                            questValue = 5;
                            questManager.EnableNPCQuest("Kill" + questValue + " shankers");
                        }
                        if(questIndex != 0)
                        {
                            string[] stringArray = {"Thanks for the help. Here's 75 gold.", questDialoguePool[questIndex]};
                            StopAllCoroutines();
                            StartCoroutine(changeDialogueSeries(popup, stringArray));
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(changeDialogue(popup, questDialoguePool[questIndex]));
                        }
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(changeDialogue(popup, questDialoguePool[questIndex]));
                    }
                }
                quest = false;
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
            audioText[0].Stop();
        }
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
    IEnumerator changeDialogueSeries(GameObject popup, string[] texts)
    {
        float timer = 0.005f;
        float timer2 = 3;
        for(int i = 0; i < texts.Length; i++)
        {
            for(int j = 0; j < texts[i].Length; j++)
            {
                popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(texts[i].Substring(0, j+1));
                yield return new WaitForSeconds(timer);
            }   
            yield return new WaitForSeconds(timer2);
        }
    }

}
