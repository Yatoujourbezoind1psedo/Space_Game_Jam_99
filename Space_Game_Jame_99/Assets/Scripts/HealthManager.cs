using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;

public class HealthManager : MonoBehaviour
{
    public Image healthBar; 
    public float healthAmount = 5f; //Nombre de PV
    private float healtAmountOrigin; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healtAmountOrigin = healthAmount; 
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0) // Donc Game over
        {
            Debug.Log("GAME OVER"); 
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
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / healtAmountOrigin; // ON prend la barre et on lui donne le nombre de pv divisé par le nombre de pv de base 
    }

public void Heal(float healingAmount) //Pt bloquer au max de pv d'origine, à voir
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healtAmountOrigin); //Permet d'éviter d'avoir plus de pv que le max et moins de PV que le min (zéro)
        healthBar.fillAmount = healthAmount / healtAmountOrigin; 
    }
}
