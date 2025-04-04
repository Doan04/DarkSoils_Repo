using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    public Vector3 mouse_pos;
    Vector3 pos;
    float horizontalInput;
    float verticalInput;
    public Animator animator;
    public Animator legAnimator;
    public GameObject reticle;
    public GameObject meleeHitbox;
    public GameObject Legs;
    LayerMask mask;
    Rigidbody2D rb;
    public AudioSource playerAudio;
    public AudioClip swingSound;

    bool scytheActive;
    public bool isRepairing; // disable all combat and movement input while F is held.
    public bool sprinting; // whether or not the player is sprinting
    public bool isRegenStamina; // whether or not the player should be regaining stamina over time
    public float staminaRegenDelay = 2f;
    float playerSpeed = 5f;
    public float swingCooldown = 0.7f;
    public float angle;
    public float currentHealth = 100f;
    public float invincibleTimer = 0;
    public float currentStamina = 100f;
    public float maxStamina = 100f;
    public float maxHealth = 100f;
    public float repairTime = 3f;
    //public MakeCorpse playerCorpseScript;
    void Start()
    {
        mask = LayerMask.GetMask("Enemy");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Legs = GameObject.Find("Legs");
        legAnimator = GameObject.Find("Legs").GetComponent<Animator>();
        reticle = GameObject.Find("Reticle");
        scytheActive = true;
        meleeHitbox = GameObject.Find("MeleeHitbox");
    }

    // Update is called once per frame
    void Update()
    {
        invincibleTimer -= Time.deltaTime;
        swingCooldown -= Time.deltaTime;
        if (sprinting) 
        {
            currentStamina -= Time.deltaTime * 1;
            if (currentStamina > 0)
            {
                playerSpeed = 7f;
            }
            else
            {
                playerSpeed = 5f;
            }
        }
        else
        {
            // tick down the timer until stamina is allowed to regen
            staminaRegenDelay -= Time.deltaTime;
            if(staminaRegenDelay <= 0)
            {
                isRegenStamina = true;
            }
        }
        if (isRegenStamina)
        {
            currentStamina += Time.deltaTime * 1;
        }
        // calculating player and mouse positions
        pos = transform.position;
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        reticle.transform.position = new Vector3(mouse_pos.x, mouse_pos.y, 0f);
        Vector2 direction = new Vector2(mouse_pos.x - pos.x, mouse_pos.y - pos.y);
        if (isRepairing == false)
        {
            transform.up = direction;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // enable stamina drain and reset the timer for regen
            sprinting = true;
            staminaRegenDelay = 2f;
            isRegenStamina = false;
            // modify speed
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.Space)) { 
            scytheActive = !scytheActive;
            animator.SetBool("scytheActive", scytheActive);    
        }
        if (Input.GetMouseButtonDown(0) && isRepairing == false) 
        {
            // set animator parameters
            animator.SetTrigger("Attack");
            if(swingCooldown <= 0)
            {
                if (scytheActive) 
                {
                    // if Player is using Scythe
                    // code for melee attack
                    playerAudio.PlayOneShot(swingSound);
                    //meleeHitbox.GetComponent<PlayerMeleeScript>().Kill();
                    swingCooldown = 0.7f;
                }
                else
                {
                    // if Player is using Hammer
                    // code for melee attack
                    playerAudio.PlayOneShot(swingSound);
                    swingCooldown = 0.7f;
                }
            }
        }
        else if(Input.GetMouseButtonDown(1) && isRepairing == false)
        {
            // Fertilizer Spray
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Do a check that player is in interaction range

            // Repair or Interact
            isRepairing = true;
            animator.SetBool("repairing", true);
            
        }
        if (Input.GetKeyUp(KeyCode.F)) 
        {
            // Stop repair or interact
            isRepairing = false;
            animator.SetBool("repairing", false);
        }
    }

    private void FixedUpdate()
    {
        // movement code
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (isRepairing == true) 
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
        if(horizontalInput != 0f || verticalInput != 0f)
        {
            legAnimator.SetBool("isMoving", true);
        }
        else
        {
            legAnimator.SetBool("isMoving", false);
        }
        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        direction.Normalize();
        rb.MovePosition(rb.position + direction * playerSpeed * Time.deltaTime);
        
    }

    public void Die()
    {

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //Hit by an enemy, set invincibility state
        if(collision.gameObject.tag == "Enemy" && invincibleTimer <= 0)
        {
            invincibleTimer = 3;
            currentHealth -= collision.gameObject.GetComponent<EnemyScript>().damage;
        }
    }

    //Animation event parameter cannot be a bool, must be something else
    public void SetWeaponHitbox(int choice)
    {
        if(choice == 0)
        {
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        else if(choice == 1)
        {
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
    }

    IEnumerator ShowPanel(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        panel.SetActive(true);
    }
    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
