using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameRunning = true; 

    //Récupération des panels de jeu et de game over pour le switch
    [SerializeField] private GameObject panelGameOver, panelJeu, panelWin, panelGameRetry; 

    [SerializeField] private AudioSource musiqueJeu, musiqueGameManager; 
    [SerializeField] private float delaWin;
    public bool isMusicFinished = false; 

    [SerializeField] private LaserManager laserManager; 
    private bool panelAffiche = false;

    private void Awake()
    {
        isMusicFinished = false; 
    }

    private void Update()
    {
        if (isMusicFinished)
        {
            delaWin -= Time.deltaTime; 
        }

        if(!panelAffiche && delaWin <= 0 && (laserManager.GetTauxScanPourCent() == 100)) //Donc on a gagné
        {
            panelAffiche = true;
            GameWin(); 
        }else if (!panelAffiche && delaWin <= 0 && (laserManager.GetTauxScanPourCent() != 100)) //don on a terminé la musique mais pas tout scan (la condition est pas nécessaire mais bon)
        {
            panelAffiche = true;
            GameRetry(); 
        }
    }

    private void GameWin() //pour l'activer avec un délai je pas par isMusicFinished 
    {
        //Debug.Log("GameWin"); 

        panelWin.SetActive(true); 
        panelJeu.SetActive(false); 

        //ZA WARUDO !!!!
        Time.timeScale = 0;
        isGameRunning = false;

        
        
        musiqueJeu.Stop(); //Juste au cas où 
        //musiqueGameManager.Play(); //Générique win (faudra mettre le bon audio clip)
        
        this.GetComponent<MouseLock>().Afficher(); //affiche souris
    }

    public void GameRetry()
    {
        //Debug.Log("GAME OVER");
        panelGameRetry.GetComponentInChildren<TextMeshProUGUI>().text = "FAIL : " + laserManager.GetTauxScanPourCent() + "% sur " + laserManager.tauxScanMax + "%"; 

        panelGameRetry.SetActive(true); 
        panelJeu.SetActive(false); 

        //ZA WARUDO !!!!
        Time.timeScale = 0;
        isGameRunning = false;

        
        
        musiqueJeu.Stop(); //ZA WARUDO AUDIO
        musiqueGameManager.Play(); //Générique fin

        this.GetComponent<MouseLock>().Afficher(); 
    }

    public void GameOver()
    {
        //Debug.Log("GAME OVER");

        panelGameOver.SetActive(true); 
        panelJeu.SetActive(false); 

        //ZA WARUDO !!!!
        Time.timeScale = 0;
        isGameRunning = false;

        
        
        musiqueJeu.Stop(); //ZA WARUDO AUDIO
        musiqueGameManager.Play(); //Générique fin

        this.GetComponent<MouseLock>().Afficher(); 
    }

    public void GamePlay() 
    {
        this.GetComponent<MouseLock>().Desafficher(); 
        
        Time.timeScale = 1; 
        isGameRunning = true; 

        musiqueJeu.Play(); 

        panelGameOver.SetActive(false);
        panelJeu.SetActive(true);

    }

    

    public void Reset()
    {
        this.GetComponent<MouseLock>().Desafficher(); 

        Time.timeScale = 1; 

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); //build index = numéro dans le build settings 
    
        //NE JAMAIS ACTIVER LE JEU DANS LE RESET !!! PAS DE isGameRunning = true !!!!! 
    } 
}
