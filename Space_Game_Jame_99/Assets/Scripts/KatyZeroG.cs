using UnityEngine;
using UnityEngine.InputSystem;

public class KatyZeroG : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Camera menuCamera;
    [SerializeField] private Transform emptyBras; // Le point qui suit la souris
    [SerializeField] private Transform corpsKaty;  // Le visuel qui "flotte"

    [Header("Réglages Espace")]
    [SerializeField] private float distanceCam = 5f;
    [SerializeField] private float suiviVitesse = 0.1f;    // Fluidité du bras
    [SerializeField] private float flottementAmorti = 0.5f; // Plus c'est bas, plus elle "glisse" longtemps
    [SerializeField] private float sensibiliteRotation = 2f; // Force du balancement

    private Vector3 veloBras = Vector3.zero;
    private Vector3 veloCorps = Vector3.zero;
    private Vector3 rotationVelocity = Vector3.zero;
    
    // Pour calculer l'inertie
    private Vector3 dernierePosBras;

    void Start()
    {
        if (emptyBras != null) dernierePosBras = emptyBras.position;
    }

    void Update()
    {
        if (menuCamera == null || emptyBras == null || Mouse.current == null) return;

        // 1. LE BRAS SUIT LA SOURIS (Très fluide)
        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 mousePos3D = new Vector3(mousePos2D.x, mousePos2D.y, distanceCam);
        Vector3 cibleMonde = menuCamera.ScreenToWorldPoint(mousePos3D);

        emptyBras.position = Vector3.SmoothDamp(emptyBras.position, cibleMonde, ref veloBras, suiviVitesse);

        // 2. LE CORPS TRAINE DERRIÈRE (Effet Zéro-G)
        if (corpsKaty != null)
        {
            // On calcule le décalage : le corps veut être à la position du bras
            // mais avec un gros retard physique (SmoothDamp avec une valeur plus haute)
            corpsKaty.position = Vector3.SmoothDamp(corpsKaty.position, emptyBras.position, ref veloCorps, flottementAmorti);

            // 3. ROTATION PAR INERTIE
            // On regarde de combien le bras a bougé cette frame
            Vector3 mouvementBras = emptyBras.position - dernierePosBras;
            dernierePosBras = emptyBras.position;

            // On crée une rotation basée sur la direction du mouvement
            // Plus on va vite, plus elle s'incline
            float cibleAngleZ = -mouvementBras.x * sensibiliteRotation * 100f;
            float cibleAngleX = mouvementBras.y * sensibiliteRotation * 100f;

            Quaternion rotationCible = Quaternion.Euler(cibleAngleX, 0, cibleAngleZ);
            
            // Applique la rotation de façon très douce pour l'effet "flottaison"
            corpsKaty.localRotation = Quaternion.Slerp(corpsKaty.localRotation, rotationCible, Time.deltaTime * 2f);
        }
    }
}