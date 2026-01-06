using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthStaminaManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;
    public statController stat;

    [Header("Health Regen")]
    public float healthRegenRate = 1f;
    public float healthRegenDelay = 5f;
    private float lastHealthDamageTime;

    // [Header("Level settings")]
    // public float baseLevel = 100;
    // public float currentLevel = 0;
    // public float currentXp = 0;
    // public float xpGrowth = 15;
    // public float xpExponential = 10;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    public float staminaRegenDelay = 2f;
    
    [Header("UI References")]
    public Slider healthBar;
    public Slider staminaBar;
    public TMP_Text healthText;  // Changed from Text to TMP_Text
    public TMP_Text staminaText; // Changed from Text to TMP_Text
    
    [Header("Low Stamina Effects")]
    public bool enableStaminaEffects = true;
    public float lowStaminaThreshold = 25f;
    public AudioSource heavyBreathingAudio;
    
    private float lastStaminaUseTime;
    private FirstPersonController playerController;
    
    void Start()
    {
        // Initialize values
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        playerController = GetComponent<FirstPersonController>();
        
        // Initialize UI with proper values
        InitializeUI();
    }
    
    void InitializeUI()
    {
        // Set slider max values
        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        
        if (staminaBar != null)
        {
            staminaBar.minValue = 0;
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
        
        UpdateHealthUI();
        UpdateStaminaUI();
    }
    
    void Update()
    {
        maxHealth = stat.getHealth();
        HandleStamina();
        HandleHealthRegen();

    }

    // void HandleXp()
    // {
    //     if (currentXp >= baseLevel)
    //     {
    //         //level up function shows a canvas for leveling up
    //         //int overflow = currentxp - baselevel
    //         //reset the currentxp
    //         //currentxp += overflow
    //         //increase currentlevel
    //         //increase baselevel with 
    //         //xpGrowth + xpExponential*currentLevel
    //         //maxhealth + healthlevelincrease (5)
    //         //maxstamina + staminalevelincrease (5)
    //         //update the xp bar


    //     }
    // }

    void HandleStamina()
    {
        bool isUsingStamina = false;
        
        if (playerController != null)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) && 
                              (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
            
            if (isSprinting && currentStamina > 0)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;
                isUsingStamina = true;
                lastStaminaUseTime = Time.time;
            }
        }
        
        if (!isUsingStamina && Time.time - lastStaminaUseTime > staminaRegenDelay)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
        
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        
        UpdateStaminaUI();
        HandleLowStaminaEffects();
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        damage -= stat.getDefense();

        if (damage < 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastHealthDamageTime = Time.time; // start regen delay

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void HandleHealthRegen()
    {
        if (isDead) return;
        if (currentHealth >= maxHealth) return;

        if (Time.time - lastHealthDamageTime > healthRegenDelay)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
        }
    }

    
    public bool UseStamina(float staminaCost)
    {
        if (currentStamina >= staminaCost)
        {
            currentStamina -= staminaCost;
            lastStaminaUseTime = Time.time;
            UpdateStaminaUI();
            return true;
        }
        return false;
    }
    
    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {Mathf.RoundToInt(maxHealth)}";
        }
    }
    
    void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }
        
        if (staminaText != null)
        {
            staminaText.text = $"{Mathf.RoundToInt(currentStamina)} / {Mathf.RoundToInt(maxStamina)}";
        }
    }
    
    void HandleLowStaminaEffects()
    {
        if (!enableStaminaEffects) return;
        
        bool isLowStamina = currentStamina <= lowStaminaThreshold;
        
        if (heavyBreathingAudio != null)
        {
            if (isLowStamina && !heavyBreathingAudio.isPlaying)
            {
                heavyBreathingAudio.Play();
            }
            else if (!isLowStamina && heavyBreathingAudio.isPlaying)
            {
                heavyBreathingAudio.Stop();
            }
        }
    }
    
    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");

        GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<PlayerDeathUI>().Show();

    }

    public void ResetHealth()
    {
        isDead = false;
        currentHealth = maxHealth;
    }
    
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public float GetStaminaPercentage() => currentStamina / maxStamina;
    public bool HasLowStamina() => currentStamina <= lowStaminaThreshold;
}