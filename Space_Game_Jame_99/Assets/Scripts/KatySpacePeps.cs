using UnityEngine;
using UnityEngine.InputSystem;

public class KatySpacePeps : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Camera menuCamera;
    [SerializeField] private Transform emptyBras; 
    [SerializeField] private Transform corpsKaty;  

    [Header("Distance Dynamique (Z)")]
    [SerializeField] private float distMinBords = 3f;  // Très proche de la caméra sur les côtés
    [SerializeField] private float distMaxCentre = 9f; // S'enfonce dans le décor au centre
    [SerializeField] private float suiviVitesseBras = 0.05f;

    [Header("Physique Zéro-G (Le Peps)")]
    [SerializeField] private float elasticite = 0.15f; 
    [SerializeField] private float amortiInertie = 0.05f; 
    [SerializeField] private float puissanceRotation = 150f;

    private Vector3 veloBras = Vector3.zero;
    private Vector3 veloCorps = Vector3.zero;
    private Vector3 dernierePosBras;
    private float rotationZActuelle;
    private float veloRotationZ;

    void Start()
    {
        if (menuCamera == null) menuCamera = Camera.main;
        if (emptyBras != null) dernierePosBras = emptyBras.position;
    }

    void Update()
    {
        if (menuCamera == null || emptyBras == null || Mouse.current == null) return;

        // 1. CALCUL DE LA DISTANCE DYNAMIQUE
        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        
        // On normalise la position : -1 à 1 (0 = centre)
        float xNorm = (mousePos2D.x / Screen.width) * 2f - 1f;
        float yNorm = (mousePos2D.y / Screen.height) * 2f - 1f;

        // Magnitude de 0 (centre) à ~1.4 (coins)
        float distanceAuCentre = new Vector2(xNorm, yNorm).magnitude;
        // On sature à 1 pour que les coins ne soient pas plus proches que les bords
        distanceAuCentre = Mathf.Clamp01(distanceAuCentre);

        // Au centre (0), on va vers distMaxCentre. Aux bords (1), vers distMinBords.
        float distanceZ = Mathf.Lerp(distMaxCentre, distMinBords, distanceAuCentre);

        // 2. LE PIVOT (Le Bras) suit la souris avec la profondeur Z calculée
        Vector3 mousePos3D = new Vector3(mousePos2D.x, mousePos2D.y, distanceZ);
        Vector3 cibleMonde = menuCamera.ScreenToWorldPoint(mousePos3D);
        emptyBras.position = Vector3.SmoothDamp(emptyBras.position, cibleMonde, ref veloBras, suiviVitesseBras);

        // 3. LE CORPS (Inertie & Ressort)
        if (corpsKaty != null)
        {
            corpsKaty.position = Vector3.SmoothDamp(corpsKaty.position, emptyBras.position, ref veloCorps, elasticite, Mathf.Infinity, Time.deltaTime);

            // 4. ORIENTATION FACE CAMÉRA
            Vector3 directionVersCam = (menuCamera.transform.position - corpsKaty.position).normalized;
            // Sécurité pour éviter les erreurs si Katy est pile sur la caméra
            if (directionVersCam == Vector3.zero) directionVersCam = Vector3.forward;
            Quaternion lookRotation = Quaternion.LookRotation(directionVersCam);
            
            // 5. INERTIE DE ROTATION
            Vector3 vitesseMouvement = (emptyBras.position - dernierePosBras) / Time.deltaTime;
            dernierePosBras = emptyBras.position;

            float cibleRotationZ = -vitesseMouvement.x * (puissanceRotation / 100f);
            cibleRotationZ = Mathf.Clamp(cibleRotationZ, -45f, 45f);

            rotationZActuelle = Mathf.SmoothDamp(rotationZActuelle, cibleRotationZ, ref veloRotationZ, amortiInertie);

            corpsKaty.rotation = lookRotation * Quaternion.Euler(0, 0, rotationZActuelle);
        }
    }
}