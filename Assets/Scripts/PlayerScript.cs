using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    private Vector3 mouse_pos;
    Vector3 pos;
    float horizontalInput;
    float verticalInput;
    private Animator animator;
    private Animator legAnimator;
    private GameObject reticle;
    public GameObject meleeHitbox;
    private GameObject Legs;
    public ParticleSystem damageEffect;
    LayerMask mask;
    Rigidbody2D rb;
    private AudioSource playerAudio;
    public AudioClip swingSound;
    public AudioClip PlayerHit;
    public AudioClip dashSound;
    public int money;
    public int fish;
    public int attack;
    bool scytheActive; // true => scythe // false => hammer
    public bool isRepairing; // disable all combat and movement input while F is held.
    public bool firing; // whether or not the player is firing fertilizer
    public bool isRegenStamina; // whether or not the player should be regaining stamina over time
    public bool playerHasControl = true;
    public bool playerInCrops = false;
    public float staminaRegenDelay = 0.5f; // Time after attacking or sprinting until stamina regenerates
    public float playerSpeed = 10f; // Player movement speed
    public float swingCooldown = 0.7f;
    public float swingCooldownTime = 0.7f;
    public float currentHealth = 100f;
    public float invincibleTimer = 0;
    public float currentStamina = 100f;
    public float maxStamina = 100f;
    public float maxHealth = 100f;
    public float currentFert = 100f;
    public float maxFert = 100f;
    public float repairTime = 3f; // Time spent repairing until player send a fix message to machine 
    public float fireRate = 0.2f;
    public float staminaRegen;
    public StaminaBarScript staminaBar;
    public HealthBarScript healthBar;
    public FertilizerBar fertBar;
    public TextMeshProUGUI inventoryText;
    public Vector2 currentMovementDirection;
    public GameObject fertBullet;
    private CropScript cropScript;
    public AudioClip coinNoise;
    public AudioClip fertNoise;
    // public GameObject LosePanel;
    void Start()
    {
        money = 20;
        attack = 3;
        playerSpeed = 7f;
        currentHealth = maxHealth;
        staminaRegen = 10f;
        mask = LayerMask.GetMask("Enemy");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Legs = GameObject.Find("Legs");
        legAnimator = GameObject.Find("Legs").GetComponent<Animator>();
        reticle = GameObject.Find("Reticle");
        scytheActive = true;
        meleeHitbox = GameObject.Find("MeleeHitbox");
        currentStamina = 100f;
        cropScript = GameObject.Find("CropField").GetComponent<CropScript>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        healthBar.updateHealthValue(currentHealth / maxHealth);
        invincibleTimer -= Time.deltaTime;
        swingCooldown -= Time.deltaTime;
        staminaBar.updateStaminaValue(currentStamina/maxStamina);
        fireRate -= Time.deltaTime;
        inventoryText.SetText("Money: " + money + "\nFertilizer: " + currentFert);
        if(currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
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
        if (Input.GetKeyDown(KeyCode.Space)) { 
            scytheActive = !scytheActive;
            animator.SetBool("scytheActive", scytheActive);
        }
        if (Input.GetMouseButtonDown(0) && isRepairing == false) 
        {
            // set animator parameters
            // if enough stamina, drain stamina and attack
            CombatInput();
        }
        else if(Input.GetMouseButtonDown(1) && isRepairing == false)
        {
            if (playerInCrops && currentFert >= 45f && cropScript.canHeal) 
            {
                currentFert -= 45f;
                fertBar.updateFertValue(currentFert / maxFert);
                cropScript.Heal();
            }
        }   
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast to check if player can initiate repair
            Repair();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            // if enough stamina, drain stamina and enable speed, else ignore 
            if(currentStamina > 15f)
            {
                currentStamina -= 15f;
                playerAudio.PlayOneShot(dashSound);
                rb.AddForce(currentMovementDirection * 15f, ForceMode2D.Impulse);
                playerHasControl = false;
                StartCoroutine(returnControlToPlayer());
            }
        }
    }

    public void MovementInput()
    {
        if (playerHasControl)
        {
            // movement code
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            if (isRepairing == true)
            {
                horizontalInput = 0;
                verticalInput = 0;
            }
            if (horizontalInput != 0f || verticalInput != 0f)
            {
                legAnimator.SetBool("isMoving", true);
            }
            else
            {
                legAnimator.SetBool("isMoving", false);
            }
            Vector2 direction = new Vector2(horizontalInput, verticalInput);
            direction.Normalize();
            currentMovementDirection = direction;
            rb.MovePosition(rb.position + direction * playerSpeed * Time.deltaTime);
        }
    }
    public void CombatInput()
    {
        if (swingCooldown <= 0)
        {
            if (scytheActive)
            {
                // if Player is using Scythe
                if (currentStamina >= 10f)
                {
                    animator.SetTrigger("Attack");
                    currentStamina -= 10f;
                    playerAudio.PlayOneShot(swingSound);
                    meleeHitbox.GetComponent<PlayerMeleeScript>().Attack(scytheActive);
                    swingCooldown = swingCooldownTime;
                }
            }
            else
            {
                // if Player is using Hammer
                if (currentStamina >= 30f)
                {
                    animator.SetTrigger("Attack");
                    currentStamina -= 30f;
                    playerAudio.PlayOneShot(swingSound);
                    meleeHitbox.GetComponent<PlayerMeleeScript>().Attack(scytheActive);
                    swingCooldown = swingCooldownTime;
                }
            }
        }
    }
    public void Repair()
    {
        LayerMask mask = LayerMask.GetMask("Machine");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 3f, mask);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            WaterPumpScript pumpScript;
            GeneratorScript genScript;
            // get reference to machine's script and repair them
            if (hit.transform.CompareTag("Generator"))
            {
                genScript = hit.transform.gameObject.GetComponent<GeneratorScript>();
                isRepairing = true;
                animator.SetBool("repairing", true);
                StartCoroutine(FixRoutine(genScript, null));
            }
            else
            {
                pumpScript = hit.transform.gameObject.GetComponent<WaterPumpScript>();
                isRepairing = true;
                animator.SetBool("repairing", true);
                StartCoroutine(FixRoutine(null, pumpScript));
            }
        }
    }

    public void sprayBlood(Vector3 enemyPosition)
    {
        Vector3 targetDir = transform.position - enemyPosition;
        Quaternion rotation = Quaternion.LookRotation(targetDir);
        Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, 5), rotation);
    }

    public void DamagePlayer(int damage)
    {
        if(invincibleTimer <= 0)
        {
            invincibleTimer = 0.5f;
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                SceneManager.LoadScene("LoseScene");
            }
            healthBar.updateHealthValue(currentHealth / maxHealth);
            playerAudio.PlayOneShot(PlayerHit);
        }
    }

    public void updateFertBar()
    {
        fertBar.updateFertValue(currentFert / maxFert);
    }

    IEnumerator FixRoutine(GeneratorScript gs, WaterPumpScript wp)
    {
        yield return new WaitForSeconds(3f);
        if (gs != null) 
        {
            gs.Fix();
        }
        else
        {
            wp.Fix();
        }
        isRepairing = false;
        animator.SetBool("repairing", false);
    }

    public void playcoinNoise()
    {
        playerAudio.PlayOneShot(coinNoise);
    }

    public void playfertNoise()
    {
        playerAudio.PlayOneShot(fertNoise);
    }

    IEnumerator returnControlToPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        playerHasControl = true;
        rb.linearVelocity = Vector2.zero;
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

    //Function passed to Animator cannot have bool parameter
    //Using int instead
    public void SetWeaponHitbox(int activatedNumber)
    {
        bool activated = false;
        if(activatedNumber == 0)
        {
            activated = false;
        }
        else if (activatedNumber == 1)
        {
            activated = true;
        }
        //Asuming that the first child is the hitbox
        GameObject hitboxObject = gameObject.transform.GetChild(0).gameObject;

        hitboxObject.GetComponent<BoxCollider2D>().enabled = activated;
    }
}

