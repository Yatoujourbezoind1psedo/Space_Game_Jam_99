using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [Header("Réglages Animation")]
    [SerializeField] private float scaleMax = 1.5f;    // Taille maximale atteinte
    [SerializeField] private float vitesseVentre = 10f; // Vitesse de gonflement
    [SerializeField] private float vitesseRetrait = 5f; // Vitesse de disparition

    private bool estEnTrainDeGrandir = true;

    void OnEnable()
    {
        // Reset de la taille à presque 0 quand on l'active
        transform.localScale = Vector3.one * 0.01f;
        estEnTrainDeGrandir = true;
    }

    void Update()
    {
        if (estEnTrainDeGrandir)
        {
            // Phase 1 : Gonflement rapide
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleMax, Time.deltaTime * vitesseVentre);

            // Si on est assez proche de la taille max, on commence à dégonfler
            if (transform.localScale.x >= scaleMax * 0.9f)
            {
                estEnTrainDeGrandir = false;
            }
        }
        else
        {
            // Phase 2 : Rétractation et disparition
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * vitesseRetrait);

            // Si c'est devenu minuscule, on désactive l'objet
            if (transform.localScale.x <= 0.05f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}