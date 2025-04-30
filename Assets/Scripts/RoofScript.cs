using UnityEngine;
using System.Collections;
public class RoofScript : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public GameObject audioManager;
    public bool shop;
    public bool boss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            //If visual already running stop it and play the other one
            StopAllCoroutines();
            StartCoroutine(roofVisual(false));
            if(shop)
            {
                StartCoroutine(audioManager.GetComponent<MainAudio>().Shop(true));
                if(collidedObject.CompareTag("Enemy"))
                {
                    Destroy(collidedObject);
                }
            }
            else if(boss)
            {
                StartCoroutine(audioManager.GetComponent<MainAudio>().Boss(true));
            }
        }
        //Doubling as a role to terminate any enemies who enter the shop

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(roofVisual(true));
            if(shop)
            {
                StartCoroutine(audioManager.GetComponent<MainAudio>().Shop(false));
            }
            else if(boss)
            {
                StartCoroutine(audioManager.GetComponent<MainAudio>().Boss(false));
            }        
        }
    }

    IEnumerator roofVisual(bool show)
    {
        float timer = 0.005f;
        if(show)
        {
            while(GetComponent<SpriteRenderer>().color.a < 1)
            {
                //This color is a temp copy of the original color
                Color color = GetComponent<SpriteRenderer>().color;
                color.a += 0.05f;
                GetComponent<SpriteRenderer>().color = color;
                yield return new WaitForSeconds(timer);
            }
        }
        else
        {
            while(GetComponent<SpriteRenderer>().color.a > 0)
            {
                Color color = GetComponent<SpriteRenderer>().color;
                color.a -= 0.05f;
                GetComponent<SpriteRenderer>().color = color;
                yield return new WaitForSeconds(timer);
            }
        }
    }
}
