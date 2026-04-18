using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement & Canaux")]
    [SerializeField] private float distance = 2f; 
    [SerializeField] private int nbCanaux = 4; 
    [SerializeField] private float smoothTime = 0.05f; // Temps pour atteindre la cible (Ease-out)

    [Header("Références")]
    [SerializeField] private GameObject visuel; // L'objet frère
    [SerializeField] private GameManager gameManager; 
    [SerializeField] private LaserManager laserManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameObject laser; 

    [Header("Paramètres Laser")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float intervalNoTarget = 1f;
    [SerializeField] private float tempsDeplacement = 0.3f;

    // Variables internes
    private float xPlayer, xOrigine;
    private float noTargetTimer, loseTargetTimer;
    private int ignoreFramesTP = 0;
    private bool isScanning;
    private TargetController lastTarget;
    private AudioSource audioSourceDaron;
    private Animator animatorVisu;
    
    // On a besoin de deux vélocités séparées pour que les deux objets soient fluides
    private Vector3 velocityHitbox = Vector3.zero; 
    private Vector3 velocityVisuel = Vector3.zero; 

    void Start()
    {
        // 1. Initialisation
        xOrigine = transform.position.x; 
        float xPositionDepart = xOrigine + distance; 
        xPlayer = xPositionDepart;

        // 2. Positionnement initial (sans glissade au démarrage)
        Vector3 startPos = new Vector3(xPositionDepart, transform.position.y, transform.position.z);
        transform.position = startPos;
        if (visuel != null) visuel.transform.position = startPos;

        // 3. Composants
        audioSourceDaron = GetComponentInParent<AudioSource>();
        if (visuel != null) animatorVisu = visuel.GetComponent<Animator>();
    }

    void Update()
    {
        if (gameManager != null && gameManager.isGameRunning)
        {
            HandleMouvement(); 
            HandleLaser();   
        }
    }

    private void HandleMouvement()
    {
        // --- 1. DETECTION DES TOUCHES ---
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && xPlayer > xOrigine)
        {
            ignoreFramesTP = 6; // On augmente un peu car le mouvement est plus lent que la TP
            xPlayer -= distance;
            PlayAnim("L");
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && xPlayer < xOrigine + (distance * (nbCanaux - 1)))
        {
            ignoreFramesTP = 6;
            xPlayer += distance; 
            PlayAnim("R");
        }

        // --- 2. MOUVEMENT FLUIDE (HITBOX + VISUEL) ---
        Vector3 targetPos = new Vector3(xPlayer, transform.position.y, transform.position.z);

        // La Hitbox glisse maintenant au lieu de se TP
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPos, 
            ref velocityHitbox, 
            smoothTime
        );

        // Le Visuel suit la même cible avec sa propre vélocité
        if (visuel != null)
        {
            visuel.transform.position = Vector3.SmoothDamp(
                visuel.transform.position, 
                targetPos, 
                ref velocityVisuel, 
                smoothTime
            );
        }
    }

    private void HandleLaser()
    {
        if (!Keyboard.current.spaceKey.isPressed)
        {
            isScanning = false; 
            laser.SetActive(false);
            if (lastTarget != null) { lastTarget.StopScan(); lastTarget = null; }
            return;
        }

        if (!isScanning) { audioSourceDaron.Play(); isScanning = true; }

        laser.SetActive(true);
        RaycastHit hit;
        TargetController currentTarget = null;

        // On tire le rayon DEPUIS LA HITBOX (qui est en train de glisser)
        if (Physics.Raycast(transform.position, Vector3.left * rayDistance, out hit))
        {
            if (hit.collider.CompareTag("Target"))
            {
                currentTarget = hit.collider.GetComponent<TargetController>();
                noTargetTimer = 0f;
            }
        }

        // Pendant que la hitbox glisse, on force le maintien de la cible pour éviter les coupures
        if (ignoreFramesTP > 0) { ignoreFramesTP--; currentTarget = lastTarget; }

        if (currentTarget != null)
        {
            loseTargetTimer = 0f;
            if (currentTarget != lastTarget)
            {
                if (lastTarget != null) lastTarget.StopScan();
                lastTarget = currentTarget;
                lastTarget.StartScan();
            }
        }
        else
        {
            if (lastTarget != null)
            {
                loseTargetTimer += Mathf.Min(Time.deltaTime, 0.05f);
                if (loseTargetTimer > tempsDeplacement)
                {
                    lastTarget.StopScan();
                    lastTarget = null;
                }
            }
            else
            {
                noTargetTimer += Time.deltaTime;
                if (noTargetTimer >= intervalNoTarget)
                {
                    laserManager.DecrementScan(1);
                    noTargetTimer = 0f;
                }
            }
        }
    }

    private void PlayAnim(string side)
    {
        if (animatorVisu == null) return;
        animatorVisu.SetTrigger("Roll");
        if (side == "L") { animatorVisu.ResetTrigger("R"); animatorVisu.SetTrigger("L"); }
        else { animatorVisu.ResetTrigger("L"); animatorVisu.SetTrigger("R"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == transform.root) return;
        if (other.CompareTag("Obstacle")) healthManager.TakeDamage(1);
    }
}