using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;

public class HealthManager : MonoBehaviour
{
    public Image healthBar; 
    [SerializeField] private float maxHealth = 5;
    private float healthAmount ; //Nombre de PV
    private float healtAmountOrigin;

    [SerializeField] private GameManager gameManager; 
    [SerializeField] private float tempsRemplissage = 2f; 
    private float tauxRemplissage; 

    [SerializeField] private float cooldownSeuil = 0f; 
    private float coolDownTimer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        healthAmount = maxHealth; 
        healtAmountOrigin = maxHealth; 
       
        coolDownTimer = cooldownSeuil; 

        //Debug.Log("Start appelé, health = " + healthAmount);


    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0 && gameManager.isGameRunning) // Donc GAME OVER
        {
            //Debug.Log("GAME OVER"); 
            gameManager.GameOver(); 
        }

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthAmount / healtAmountOrigin, Time.deltaTime * tempsRemplissage);

        if (coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;
        }

        /* TEST
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TakeDamage(20); 
        }
        
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Heal(10); 
        }*/
    }

    public void TakeDamage(float damage)
    {
        if (coolDownTimer <= 0f)
        {
            healthAmount -= damage;
            tauxRemplissage = healthAmount / healtAmountOrigin; 
            coolDownTimer = cooldownSeuil; 
        }

    }

public void Heal(float healingAmount) //Pt bloquer au max de pv d'origine, à voir
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healtAmountOrigin); //Permet d'éviter d'avoir plus de pv que le max et moins de PV que le min (zéro)
        tauxRemplissage = healthAmount / healtAmountOrigin; 
    }
    
}
