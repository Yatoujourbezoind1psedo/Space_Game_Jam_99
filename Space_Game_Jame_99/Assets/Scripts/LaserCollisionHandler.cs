using UnityEngine;

public class LaserCollisionHandler : MonoBehaviour
{
    private PlayerController player;

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui entre a le tag Target
        if (other.CompareTag("Target"))
        {
            TargetController target = other.GetComponent<TargetController>();
            if (target != null && player != null) 
            {
                Debug.Log("<color=green>COLLISION : Le laser touche " + other.name + "</color>");
                player.SetCurrentTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            if (player != null) 
            {
                Debug.Log("<color=red>SORTIE : Le laser ne touche plus " + other.name + "</color>");
                player.SetCurrentTarget(null);
            }
        }
    }
}