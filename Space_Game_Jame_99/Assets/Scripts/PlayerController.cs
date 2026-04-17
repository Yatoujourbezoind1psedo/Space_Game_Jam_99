using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float distance = 2; 
    [SerializeField] private int nbCanaux = 4; 
    private float xPlayer, xOrigine, loseTargetTimer;
    public float tempsDeplacement = 0.3f; //Va permettre de faire verrou temporel (ou grace period) pour que dès que perso bouge alors on continue sur le même laser (sinon couperait le startscan())

    [SerializeField] private HealthManager healthManager; 
    [SerializeField] private float ptScan, vitesseVisu; 
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private GameObject visuel, laser; 

    [SerializeField] private GameManager gameManager; 

    private TargetController lastTarget; //Permet d'avoir un target controller null au démarage
    private int ignoreFramesTP = 0; //Va s'incrémenter au mouvement pour skip quelques frames
    [SerializeField] private LaserManager laserManager; 

    private float noTargetTimer = 0f;
    [SerializeField] private float intervalNoTarget = 1f;

    private AudioSource audioSourceDaron;
    private bool isScanning; 

    private Animator animatorVisu; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xPlayer = transform.position.x; 
        xOrigine = transform.position.x;

        //récupération audiosource parent
        audioSourceDaron = GetComponentInParent<AudioSource>();

        //Récupération animator du visu
        animatorVisu = visuel.GetComponent<Animator>(); 
        
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
        //visuel déplacement (séparation visuel et hitbox pour que pas punitif avec tp et à la fois visuel)
        visuel.transform.position = Vector3.Lerp( //On va aller vers à l'aide d'un vector 3
            visuel.transform.position, //on part de la position de départ
            new Vector3(xPlayer, visuel.transform.position.y, visuel.transform.position.z), //L'arrivé c'est un truc du genre Nouveau x, même y, même z
            Time.deltaTime * vitesseVisu //en temps de vitesse visu en seconde 
        ); 

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && xPlayer > xOrigine) //gauche + peut pas aller en dessous de point de départ
        {
            ignoreFramesTP = 4; //Skip de 4 frame pour le laser pour capter
            transform.position -= new Vector3(distance, 0f, 0f);
            xPlayer -= distance;

            //Visu roll
            animatorVisu.SetTrigger("Roll"); 
            
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && xPlayer < xOrigine * (distance * (nbCanaux - 1))) //droite + paramétrer pour aller que dans 4 canaux XORIGINE PAS ZERO
        {
            ignoreFramesTP = 4; //Skip de 4 frame pour le laser pour capter
            transform.position += new Vector3(distance, 0f, 0f);
            xPlayer += distance; 

            animatorVisu.SetTrigger("Roll"); 
        }

        //Pour qu'anim joue une fois 
        AnimatorStateInfo infoAnim = animatorVisu.GetCurrentAnimatorStateInfo(0); 

        if(infoAnim.IsName("Rolling") && infoAnim.normalizedTime >= 0.9f) //Si Rolling est arrivé à 1 (donc joué entièrement) (mis un peu avant fin pour éviter souci)
        {
            animatorVisu.ResetTrigger("Roll"); //Alors j'annule le roll
        }
        
    }

    //gestion du laser du joueur 
    private void HandleLaser()
    {
        if (!Keyboard.current.spaceKey.isPressed) //Si espace est pas pressé on (ca veut dire que la dernière cible doit cessé son décompte et que l'on vise rien maintenant)
        {
            isScanning = false; 

            laser.SetActive(false); //Fait disparaitre visuel du laser
            if (lastTarget != null) //si la dernière cible n'est pas présente et donc que j'avais scan une target
            {
                lastTarget.StopScan(); //stop scan de l'ancienne cible
                lastTarget = null; //et l'ancienne cible maintenant est nulle puisqeu on relâche le bouton
            }
            return; //retour de la fonction HandleLaser qaund pas clic poru pas déclencher le reste
        }

        //Si espace a été pressé

        //gerstion de l'audio
        if (!isScanning)
        {  
            audioSourceDaron.Play(); 
        }

        isScanning = true; 
        
        RaycastHit hit; //info du raycast 
        laser.SetActive(true); //fait apparaitre le visuel du laser
        TargetController currentTarget = null;  //On intialise à vide la référence de la cible au cas où raycast touche R 

        Debug.DrawRay(transform.position, Vector3.left * rayDistance, Color.red); //Pour afficher dans scène (attention à Vector3.left, ici tire rayon à gauche)

        
        //détection cible actuelle 
        if (Physics.Raycast(transform.position, Vector3.left * rayDistance, out hit)) //Le rayon part à partir de transform.position, dans la direction gauche, à une distance rayDistance (hit est le rayon qu'on envoie)
        {
            

            if (hit.collider.CompareTag("Target"))
            {
                currentTarget = hit.collider.GetComponent<TargetController>(); //si on touche une target on dit que la cible (current) = TargetController de l'impact
                noTargetTimer = 0f; //Reset le fait de pas toucher cible dès qu'on détecte une target 
            }
        }

        /*
        if (currentTarget == null && lastTarget != null) //Permet de se coller sur une target, comme ça permet téléportation
        {
            currentTarget = lastTarget; 
        }*/
        if (ignoreFramesTP > 0) //La même chose qu'au dessus mais jsute pour deux frame (sécurité bonus en déplacement)
        {
            ignoreFramesTP --; 
            currentTarget = lastTarget; 
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
            if (lastTarget != null) //permet de mettre un petit cooldown 
            {
                loseTargetTimer += Mathf.Min(Time.deltaTime, 0.05f); //Le timer se remplit petit à petit tant que pas de cible détectée (le fait qu'il s'incrémente n'est pas dérangeant sur la durée puisque c'est chronométré le niveau)

                if (loseTargetTimer > tempsDeplacement) //et s'il dépasse le cooldown donné
                {
                    Debug.Log("Perdu Cible"); 
                    lastTarget.StopScan(); //Alors on a stoppé de suivre la target (avec un délai), du coup on arrête son scan

                    lastTarget = null; //et on dit qu'on capte R (et current target est null puisque cette fonction s'active toutes les frames)
                }
            }else if (lastTarget == null) //Donc que toutes les targets sont nulles 
            {
                noTargetTimer += Time.deltaTime; 

                if (noTargetTimer >= intervalNoTarget)
                {
                    laserManager.DecrementScan(1); //Perd un point dès que joueur tire à coté 
                    noTargetTimer = 0f; 
                }
                
            }
            
        }
    }

    //DETECTION OBSTACLE
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == transform.root) return; // Si l'autre objet rentre en collison avec un enfant alors return

        if (other.CompareTag("Obstacle")) //Si le joueur se fait toucher
        {
            //Debug.Log("ouch"); 
            healthManager.TakeDamage(1); 
        }
    }

    /*
    Pour éviter que laser active collision avec obstacle : 
    Laser a layer laser, player a layer player et obstacle a layer obtacle 
    Dans edit -> Project Settings -> Physics -> Settings > Layer collision matrix : disable interaction entre laser et obstacle
    */
}
