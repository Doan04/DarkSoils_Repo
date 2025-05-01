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
    public int attackDamage;
    // Update is called once per frame
    private void Update()
    {
        if (canHit)
        {
            for (int i = 0; i < TriggerList.Count; i++)
            {
                if(TriggerList[i] == null)
                {
                    Debug.Log("Missing collider");
                }
                Collider2D collider = TriggerList[i];
                if (collider.CompareTag("Enemy"))
                {
                    // if using scythe, damage for 5 or something
                    //Adjusting this to damage based off player's attack
                    if (isScythe)
                    {
                        EnemyHealth enemyScriptRef = collider.gameObject.GetComponent<EnemyHealth>();
                        if (enemyScriptRef) 
                        {
                            GameObject player = transform.parent.gameObject;
                            enemyScriptRef.TakeDamage(player.GetComponent<PlayerScript>().attack);
                        }
                    }
                    // if using hammer, one shot
                    if (isScythe == false) 
                    {
                        EnemyHealth enemyScriptRef = collider.gameObject.GetComponent<EnemyHealth>();
                        ShankerBehavior shankerRef = collider.gameObject.GetComponent<ShankerBehavior>();
                        if (enemyScriptRef)
                        {
                            enemyScriptRef.DeathEvent();
                        }
                        else if (shankerRef != null)
                        {
                            shankerRef.DeathEvent();
                        }
                    }
                }
                else if (collider.CompareTag("Heart"))
                {
                    collider.gameObject.GetComponent<HeartBehavior>().TakeDamage();
                }
                //TriggerList.Remove(collider);
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
        if (!TriggerList.Contains(collider) && (collider.CompareTag("Enemy") || collider.CompareTag("Heart")))
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
