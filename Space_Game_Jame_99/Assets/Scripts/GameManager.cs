using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public bool isGameRunning = true; 

    //Récupération des panels de jeu et de game over pour le switch
    [SerializeField] private GameObject panelGameOver, panelJeu, panelWin; 

    [SerializeField] private AudioSource musiqueJeu, musiqueGameManager; 
    [SerializeField] private float delaWin;
    public bool isGameWinned = false; 

    private void Update()
    {
        if (isGameWinned)
        {
            delaWin -= Time.deltaTime; 
        }

        if(delaWin <= 0) //Donc on a gagné
        {
            GameWin(); 
        }
    }

    private void GameWin() //pour l'activer avec un délai je pas par isGameWinned 
    {
        Debug.Log("GameWin"); 

        panelWin.SetActive(true); 
        panelJeu.SetActive(false); 

        //ZA WARUDO !!!!
        Time.timeScale = 0;
        isGameRunning = false;

        
        
        musiqueJeu.Stop(); //Juste au cas où 
        //musiqueGameManager.Play(); //Générique win (faudra mettre le bon audio clip)
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

    }

    public void GamePlay() 
    {
        
        Time.timeScale = 1; 
        isGameRunning = true; 

        musiqueJeu.Play(); 

        panelGameOver.SetActive(false);
        panelJeu.SetActive(true);

    }

    

    public void Reset()
    {
        Time.timeScale = 1; 

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); //build index = numéro dans le build settings 
    
        //NE JAMAIS ACTIVER LE JEU DANS LE RESET !!! PAS DE isGameRunning = true !!!!! 
    } 
}
