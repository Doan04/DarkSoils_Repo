using UnityEngine;
using System.Collections;
public class RoofScript : MonoBehaviour
{
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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(roofVisual(true));
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
                Debug.Log(color.a);
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
                Debug.Log(color.a);
                color.a -= 0.05f;
                GetComponent<SpriteRenderer>().color = color;
                yield return new WaitForSeconds(timer);
            }
        }
    }
}
