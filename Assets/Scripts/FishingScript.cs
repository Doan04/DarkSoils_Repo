using UnityEngine;
using System.Collections;
using TMPro;
public class FishingScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject fish;
    public float timer;
    public float timeForFish;
    public float textReset;
    public bool fishingActivated;
    public bool fishingText;
    public bool textResetBoolean;
    public bool fishObtained;
    public bool tooEarly;
    public int temp;
    public GameObject popup;
    void Start()
    {
        tooEarly = false;
        fishObtained = false;
        popup = gameObject.transform.GetChild(0).gameObject;
        timer = 1.25f;
        timeForFish = Random.Range(1.5f, 2.25f);
        fishingActivated = false;
        fishingText = false;
        //NPCScript already prints out the initial press e to fish so can skip the first iteration
        textResetBoolean = true;
        textReset = 0.5f;
        temp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(fishingActivated)
        {
            textResetBoolean = false;
            textReset = 1f;
            timeForFish -= Time.deltaTime;
            if(timeForFish > 0)
            {
                if(!fishingText)
                {
                    fishingText = true;
                    StopAllCoroutines();
                    StartCoroutine(changeDialogue(popup, "Fishing..."));
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    tooEarly = true;
                    timeForFish = 0;
                    timer = 0;
                }
            }
            else if(timeForFish <= 0 && timer > 0)
            {
                timer -= Time.deltaTime;
                if((temp / 10) % 2 == 0)
                {
                    popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("FISH HOOKED\nPRESS E!!!!!!");
                }
                else if((temp / 10) % 2 == 1)
                {
                    popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("");
                }
                temp++;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    fishObtained = true;
                    timeForFish = 0;
                    timer = 0;
                }
            }
            else if(timer <= 0)
            {
                StopAllCoroutines();
                if(fishObtained)
                {
                    StartCoroutine(changeDialogue(popup, "Fish Caught!"));
                    //1 = small fish (x1), 2 = medium fish (x2), 3 = large fish (x3)
                    int type = Random.Range(1, 4);
                    GameObject theFish = Instantiate(fish, transform.position + new Vector3(4, Random.Range(-3f, 4f), 0), Quaternion.Euler(0, 0, 0));
                    theFish.GetComponent<FishScript>().type = type;
                }
                else if(tooEarly)
                {
                    StartCoroutine(changeDialogue(popup, "Too early!"));
                }
                else if(!fishObtained){
                    StartCoroutine(changeDialogue(popup, "Unable to catch fish..."));
                }
                fishingActivated = false;
            }
        }
        else
        {
            tooEarly = false;
            timeForFish = Random.Range(1.5f, 2.25f);
            timer = 0.75f;
            fishingText = false;
            textReset -= Time.deltaTime;
            fishObtained = false;
            if(textReset <= 0)
            {
                if(!textResetBoolean)
                {
                    StopAllCoroutines();
                    StartCoroutine(changeDialogue(popup, "Press E to fish"));
                    textResetBoolean=true;
                }
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
