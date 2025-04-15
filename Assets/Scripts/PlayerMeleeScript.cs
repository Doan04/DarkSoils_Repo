using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlayerMeleeScript : MonoBehaviour
{
    //public GameObject enemy;
    //The list of colliders currently inside the trigger
    public List<Collider2D> TriggerList = new List<Collider2D>();
    public bool canHit = false;
    public bool isScythe = false;
    // Update is called once per frame
    private void Update()
    {
        if (canHit)
        {
            for (int i = 0; i < TriggerList.Count; i++)
            {
                Collider2D collider = TriggerList[i];
                if (collider.CompareTag("Enemy"))
                {
                    // if using scythe, damage for 5 or something
                    if (isScythe)
                    {
                        EnemyScript enemyScriptRef = collider.gameObject.GetComponent<EnemyScript>();
                        if (enemyScriptRef) 
                        {
                            enemyScriptRef.TakeDamage(5);
                        }
                    }
                    // if using hammer, one shot
                    if (isScythe == false) 
                    {
                        Destroy(collider.gameObject);
                    }
                    Debug.Log(collider.name);
                }
                else if (collider.CompareTag("Heart"))
                {
                    collider.gameObject.GetComponent<HeartBehavior>().TakeDamage();
                }
                TriggerList.Remove(collider);
            }
        }
    }
    public void Attack(bool scytheActive)
    {
        canHit = true;
        isScythe = scytheActive;
        StartCoroutine(DisableKillBox());
    }

    IEnumerator DisableKillBox()
    {
        yield return new WaitForSeconds(.2f);
        canHit = false;
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
