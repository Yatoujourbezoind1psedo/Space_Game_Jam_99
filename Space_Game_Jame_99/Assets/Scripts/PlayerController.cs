using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float distance = 2; 
    [SerializeField] private int nbCanaux = 4; 
    private float xPlayer, xOrigine;

    [SerializeField] private HealthManager healthManager; 
    [SerializeField] private float ptScan; 
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private GameObject laser; 

    private TargetController lastTarget; //Permet d'avoir un target controller null au démarage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xPlayer = transform.position.x; 
        xOrigine = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouvement(); 
        HandleLaser(); 
        
    }

    //Logique de déplacement
    private void HandleMouvement()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && xPlayer > xOrigine) //gauche + peut pas aller en dessous de point de départ
        {
            transform.position -= new Vector3(distance, 0f, 0f);
            xPlayer -= distance;
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && xPlayer < xOrigine * (distance * (nbCanaux - 1))) //droite + paramétrer pour aller que dans 5 canaux XORIGINE PAS ZERO
        {
            transform.position += new Vector3(distance, 0f, 0f);
            xPlayer += distance; 
        }
    }

    //gestion du laser du joueur 
    private void HandleLaser()
    {
        if (!Keyboard.current.spaceKey.isPressed) //Si espace est pas pressé on (ca veut dire que la dernière cible doit cessé son décompte et que l'on vise rien maintenant)
        {
            laser.SetActive(false); //Fait disparaitre visuel du laser
            if (lastTarget != null) //si la dernière cible n'est pas présente
            {
                lastTarget.StopScan(); //stop scan de l'ancienne cible
                lastTarget = null; //et l'ancienne cible maintenant est nulle puisqeu on relâche le bouton
            }
            return; //retour de la fonction HandleLaser poru pas déclencher le reste
        }

        //Si espace a été pressé
        TargetController current = null; //On intialise à vide la référence de la cible au cas où raycast touche R 
        RaycastHit hit; //info du raycast 
        laser.SetActive(true); //fait apparaitre le visuel du laser

        Debug.DrawRay(transform.position, Vector3.left * rayDistance, Color.red); //Pour afficher dans scène (attention à Vector3.left, ici tire rayon à gauche)

        if (Physics.Raycast(transform.position, Vector3.left, out hit, rayDistance)) //Le rayon part à partir de transform.position, dans la direction gauche, à une distance rayDistance (hit est le rayon qu'on envoie)
        {
            if (hit.collider.CompareTag("Target"))
            {
                current = hit.collider.GetComponent<TargetController>(); //si on touche une target on dit que la cible (current) = TargetController de l'impact
            }
        }

        if (current != lastTarget) //Si la cible actuelle est différente de l'ancienne (donc supprime qaudn touche même cible)
        {
            if (lastTarget != null) //permet d'éviter bug dans le cas où rien n'était touché avant 
                lastTarget.StopScan(); //On arrête le scan de l'ancienne cible

            if (current != null) //permet d'éviter bug dans le cas où le rayon loupe cible
                current.StartScan(); //et on démarre le nombre de point de celle scannée

            lastTarget = current; //et l'ancienne cible devient la nouvelle (vu que TargetController current = null pose pas de soucis pour prochaine frame)
        }
    }

    //DETECTION OBSTACLE
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == transform.root) return; // Si l'autre objet rentre en collison avec un enfant alors return

        if (other.CompareTag("Obstacle")) //Si le joueur se fait toucher
        {
            Debug.Log("ouch"); 
            healthManager.TakeDamage(1); 
        }
    }

    /*
    Pour éviter que laser active collision avec obstacle : 
    Laser a layer laser, player a layer player et obstacle a layer obtacle 
    Dans edit -> Project Settings -> Physics -> Settings > Layer collision matrix : disable interaction entre laser et obstacle
    */
}
