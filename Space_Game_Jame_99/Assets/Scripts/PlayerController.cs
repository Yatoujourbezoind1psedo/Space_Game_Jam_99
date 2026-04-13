using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float distance = 2; 
    [SerializeField] private int nbCanaux = 4; 
    private float xPlayer, xOrigine, loseTargetTimer;
    public float tempsDeplacement = 0.2f; //Va permettre de faire verrou temporel (ou grace period) pour que dès que perso bouge alors on continue sur le même laser (sinon couperait le startscan())

    [SerializeField] private HealthManager healthManager; 
    [SerializeField] private float ptScan; 
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private GameObject laser; 

    [SerializeField] private GameManager gameManager; 

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
        if (gameManager.isGameRunning)
        {
            HandleMouvement(); 
            HandleLaser();   
        }
        
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
            if (lastTarget != null) //si la dernière cible n'est pas présente et donc que j'avais scan une target
            {
                lastTarget.StopScan(); //stop scan de l'ancienne cible
                lastTarget = null; //et l'ancienne cible maintenant est nulle puisqeu on relâche le bouton
            }
            return; //retour de la fonction HandleLaser qaund pas clic poru pas déclencher le reste
        }

        //Si espace a été pressé
        
        RaycastHit hit; //info du raycast 
        laser.SetActive(true); //fait apparaitre le visuel du laser
        TargetController currentTarget = null;  //On intialise à vide la référence de la cible au cas où raycast touche R 

        Debug.DrawRay(transform.position, Vector3.left * rayDistance, Color.red); //Pour afficher dans scène (attention à Vector3.left, ici tire rayon à gauche)

        //détection cible actuelle 
        if (Physics.Raycast(transform.position, Vector3.left, out hit, rayDistance)) //Le rayon part à partir de transform.position, dans la direction gauche, à une distance rayDistance (hit est le rayon qu'on envoie)
        {
            if (hit.collider.CompareTag("Target"))
            {
                currentTarget = hit.collider.GetComponent<TargetController>(); //si on touche une target on dit que la cible (current) = TargetController de l'impact
            }
        }
        
        if (currentTarget != null) //si le current est pas nul, donc si on touche un target
        {
            loseTargetTimer = 0f; //Reset du timer car cible toujours détectée

            if(currentTarget != lastTarget) //Si c'est pas la même cible que la précédente
            {
                if (lastTarget != null) //Et que l'ancienne est pas nulle
                    lastTarget.StopScan(); //Alors arrêt du scan

                lastTarget = currentTarget; //Ancienne cible deivent la nouvelle
                lastTarget.StartScan(); //et le scan commence 

            }
        }
        else //Donc si le l'objet touché par le raycast n'est pas une target
        {
            loseTargetTimer += Time.deltaTime; //Le timer se remplit petit à petit tant que pas de cible détectée (le fait qu'il s'incrémente n'est pas dérangeant sur la durée puisque c'est chronométré le niveau)

            if (loseTargetTimer > tempsDeplacement) //et s'il dépasse le cooldown donné
            {
                if (lastTarget != null) lastTarget.StopScan(); //Alors on a stoppé de suivre la target (avec un délai), du coup on arrête son scan

                lastTarget = null; //et on dit qu'on capte R (et current target est null puisque cette fonction s'active toutes les frames)
            }
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
