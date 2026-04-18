using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement & Canaux")]
    [SerializeField] private float distance = 2f; 
    [SerializeField] private int nbCanaux = 4; 
    [SerializeField] private float smoothTime = 0.05f; 

    [Header("Références")]
    [SerializeField] private GameObject visuel; 
    [SerializeField] private GameManager gameManager; 
    [SerializeField] private LaserManager laserManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameObject laser; 

    [Header("Paramètres Laser")]
    [SerializeField] private float intervalNoTarget = 1f;
    [SerializeField] private float tempsDeplacement = 0.3f;

    [Header("Effets d'Impact")]
    [SerializeField] private GameObject explosionObject; 

    private float xPlayer, xOrigine;
    private float noTargetTimer, loseTargetTimer;
    private int ignoreFramesTP = 0;
    private bool isScanning;
    
    private TargetController currentTargetInTrigger; 
    private TargetController lastTarget;
    
    private AudioSource audioSourceDaron;
    private Animator animatorVisu;
    
    private Vector3 velocityHitbox = Vector3.zero; 
    private Vector3 velocityVisuel = Vector3.zero; 

    void Start()
    {
        xOrigine = transform.position.x; 
        xPlayer = xOrigine + distance;
        Vector3 startPos = new Vector3(xPlayer, transform.position.y, transform.position.z);
        transform.position = startPos;
        if (visuel != null) visuel.transform.position = startPos;
        audioSourceDaron = GetComponentInParent<AudioSource>();
        if (visuel != null) animatorVisu = visuel.GetComponent<Animator>();
        if (explosionObject != null) explosionObject.SetActive(false);
    }

    void Update()
    {
        if (gameManager != null && gameManager.isGameRunning)
        {
            HandleMouvement(); 
            HandleLaserLogic(); 
        }
    }

    private void HandleMouvement()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && xPlayer > xOrigine)
        {
            ignoreFramesTP = 6;
            xPlayer -= distance;
            PlayAnim("L");
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && xPlayer < xOrigine + (distance * (nbCanaux - 1)))
        {
            ignoreFramesTP = 6;
            xPlayer += distance; 
            PlayAnim("R");
        }

        Vector3 targetPos = new Vector3(xPlayer, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocityHitbox, smoothTime);
        if (visuel != null) visuel.transform.position = Vector3.SmoothDamp(visuel.transform.position, targetPos, ref velocityVisuel, smoothTime);
    }

    private void HandleLaserLogic()
    {
        if (!Keyboard.current.spaceKey.isPressed)
        {
            if (isScanning)
            {
                isScanning = false;
                laser.SetActive(false);
                if (lastTarget != null) { lastTarget.StopScan(); lastTarget = null; }
            }
            return;
        }

        if (!isScanning) { if(audioSourceDaron) audioSourceDaron.Play(); isScanning = true; }
        laser.SetActive(true);

        TargetController currentTarget = currentTargetInTrigger;
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
                if (loseTargetTimer > tempsDeplacement) { lastTarget.StopScan(); lastTarget = null; }
            }
            else
            {
                noTargetTimer += Time.deltaTime;
                if (noTargetTimer >= intervalNoTarget) { laserManager.DecrementScan(1); noTargetTimer = 0f; }
            }
        }
    }

    public void SetCurrentTarget(TargetController target)
    {
        currentTargetInTrigger = target;
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
        if (other.CompareTag("Obstacle")) 
        {
            if(healthManager != null) healthManager.TakeDamage(1);
            
        }
    }
}