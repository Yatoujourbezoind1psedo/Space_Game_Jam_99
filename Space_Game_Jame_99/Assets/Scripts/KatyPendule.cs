using UnityEngine;

public class KatyPendule : MonoBehaviour
{
    [Header("Références (Scène Menu)")]
    [SerializeField] private Camera menuCamera;
    
    [Header("Points d'Attache (Hiérarchie)")]
    [SerializeField] private Transform emptyBras; // L'Empty DANS le bras, point de pivot.
    [SerializeField] private Transform corpsKaty;  // L'objet visuel ENFANT de emptyBras.

    [Header("Réglages de Flottaison")]
    [SerializeField, Range(1f, 10f)] private float distanceCam = 5f;
    [SerializeField, Range(0f, 1f)] private float tempsSuiviSourie = 0.05f; // Suivi de la souris (plus petit = plus réactif)
    
    [Header("Réglages de Balancement")]
    [SerializeField, Range(10f, 100f)] private float forceBalancement = 50f; // Intensité du balancement
    [SerializeField, Range(1f, 20f)] private float amortiBalancement = 5f;   // Vitesse de retour à la verticale

    // Variables internes pour les calculs
    private Vector3 veloSuivi = Vector3.zero;
    private Vector3 positionSourisPrecedente;
    private float angleBalancementActuel;
    private float velociteBalancement;

    void Start()
    {
        positionSourisPrecedente = Input.mousePosition;
        
        // Sécurité : Vérifie la hiérarchie
        if (corpsKaty != null && emptyBras != null && !corpsKaty.IsChildOf(emptyBras))
        {
            Debug.LogError("ATTENTION : 'corpsKaty' doit être un ENFANT de 'emptyBras' pour que l'effet de pendule fonctionne !");
        }
    }

    void Update()
    {
        if (menuCamera == null || emptyBras == null || corpsKaty == null) return;

        HandleSuiviSouris();
        HandleBalancementPendule();
    }

    // --- 1. LE BRAS SUIT LA SOURIS FLUIDEMENT ---
    private void HandleSuiviSouris()
    {
        // Conversion position souris (pixels) -> monde 3D
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceCam;
        Vector3 targetWorldPos = menuCamera.ScreenToWorldPoint(mousePos);

        // Mouvement fluide de l'Empty du bras vers la souris (SmoothDamp)
        emptyBras.position = Vector3.SmoothDamp(
            emptyBras.position, 
            targetWorldPos, 
            ref veloSuivi, 
            tempsSuiviSourie
        );
    }

    // --- 2. LE CORPS BALANCE SOUS LE BRAS (EFFEET PENDULE) ---
    private void HandleBalancementPendule()
    {
        // Calcul du déplacement de la souris cette frame (Vitesse)
        Vector3 positionSourisActuelle = Input.mousePosition;
        Vector3 deplacementSouris = positionSourisActuelle - positionSourisPrecedente;
        positionSourisPrecedente = positionSourisActuelle;

        // On s'intéresse surtout au mouvement horizontal (X) pour le balancement
        // On normalise un peu pour pas que ça balance trop violemment
        float vitesseX = deplacementSouris.x * 0.1f; 

        // Calcul de la cible de balancement (inertie)
        // L'angle cible est opposé au mouvement (forceBalancement détermine l'amplitude)
        float angleCible = -vitesseX * forceBalancement;

        // Limitation de l'angle pour éviter qu'elle fasse un tour complet (ex: max 60 degrés)
        angleCible = Mathf.Clamp(angleCible, -60f, 60f);

        // Amorti du balancement (pour revenir à 0 quand la souris s'arrête)
        // On utilise SmoothDamp pour un amorti physique doux
        angleBalancementActuel = Mathf.SmoothDamp(
            angleBalancementActuel, 
            angleCible, 
            ref velociteBalancement, 
            1f / amortiBalancement
        );

        // Application de la rotation sur l'axe Z du corps (enfant du bras)
        corpsKaty.localRotation = Quaternion.Euler(0, 0, angleBalancementActuel);
    }
}