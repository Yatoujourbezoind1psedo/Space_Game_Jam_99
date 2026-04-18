using UnityEngine;
using UnityEngine.InputSystem; // OBLIGATOIRE pour le nouveau système

public class KatyPendule : MonoBehaviour
{
    [Header("Références (Scène Menu)")]
    [SerializeField] private Camera menuCamera;
    
    [Header("Points d'Attache (Hiérarchie)")]
    [SerializeField] private Transform emptyBras; 
    [SerializeField] private Transform corpsKaty;  

    [Header("Réglages")]
    [SerializeField] private float distanceCam = 5f;
    [SerializeField] private float tempsSuiviSourie = 0.05f; 
    [SerializeField] private float forceBalancement = 50f; 
    [SerializeField] private float amortiBalancement = 5f;

    private Vector3 veloSuivi = Vector3.zero;
    private Vector2 positionSourisPrecedente;
    private float angleBalancementActuel;
    private float velociteBalancement;

    void Start()
    {
        Debug.Log("<color=green>KATY : Script démarré avec le New Input System !</color>");
        
        if (Mouse.current != null)
            positionSourisPrecedente = Mouse.current.position.ReadValue();
    }

    void Update()
    {
        if (menuCamera == null || emptyBras == null || Mouse.current == null) return;

        HandleSuiviSouris();
        HandleBalancementPendule();
    }

    private void HandleSuiviSouris()
    {
        // Lecture de la position de la souris (Nouveau Système)
        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        
        // Conversion en Vector3 pour le calcul monde
        Vector3 mousePos3D = new Vector3(mousePos2D.x, mousePos2D.y, distanceCam);
        Vector3 targetWorldPos = menuCamera.ScreenToWorldPoint(mousePos3D);

        // Suivi fluide
        emptyBras.position = Vector3.SmoothDamp(
            emptyBras.position, 
            targetWorldPos, 
            ref veloSuivi, 
            tempsSuiviSourie
        );
    }

    private void HandleBalancementPendule()
    {
        Vector2 positionSourisActuelle = Mouse.current.position.ReadValue();
        Vector2 deplacementSouris = positionSourisActuelle - positionSourisPrecedente;
        positionSourisPrecedente = positionSourisActuelle;

        float vitesseX = deplacementSouris.x * 0.1f; 
        float angleCible = -vitesseX * forceBalancement;
        angleCible = Mathf.Clamp(angleCible, -60f, 60f);

        angleBalancementActuel = Mathf.SmoothDamp(
            angleBalancementActuel, 
            angleCible, 
            ref velociteBalancement, 
            1f / amortiBalancement
        );

        if (corpsKaty != null)
            corpsKaty.localRotation = Quaternion.Euler(0, 0, angleBalancementActuel);
    }
}