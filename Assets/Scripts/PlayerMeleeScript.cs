using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlayerMeleeScript : MonoBehaviour
{
    public GameObject enemy;
    //The list of colliders currently inside the trigger
    public List<Collider2D> TriggerList = new List<Collider2D>();
    bool canKill = false;
    // Update is called once per frame
    private void Update()
    {
        if (canKill)
        {
            for (int i = 0; i < TriggerList.Count; i++)
            {
                Collider2D collider = TriggerList[i];
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log(collider.name);
                }
                TriggerList.Remove(collider);
            }
        }
    }
    public void Kill()
    {
        canKill = true;
        StartCoroutine(DisableKillBox());
    }

    IEnumerator DisableKillBox()
    {
        yield return new WaitForSeconds(.2f);
        canKill = false;
    }
//called when something enters the trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        //if the object is not already in the list
        if (!TriggerList.Contains(collider) && collider.CompareTag("Enemy"))
        {
            //add the object to the list
            TriggerList.Add(collider);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit2D(Collider2D collider)
    {
        //if the object is in the list
        if (TriggerList.Contains(collider) && collider.CompareTag("Enemy"))
        {
            //remove it from the list
            TriggerList.Remove(collider);
        }
    }


}
