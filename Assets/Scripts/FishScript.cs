using UnityEngine;

public class FishScript : MonoBehaviour
{
    public int type;
    public Sprite smallFish;
    public Sprite mediumFish;
    public Sprite largeFish;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(type == 1)
        {
            sr.sprite = smallFish;
        }
        else if(type == 2)
        {
            sr.sprite = mediumFish;
        }
        else if(type == 3)
        {
            sr.sprite = largeFish;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            collidedObject.GetComponent<PlayerScript>().fish += type;
            collidedObject.GetComponent<PlayerScript>().playcoinNoise();
            Destroy(gameObject);
        }
    }
}
