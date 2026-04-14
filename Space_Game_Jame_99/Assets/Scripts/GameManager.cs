using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public bool isGameRunning = true; 

    //Récupération des panels de jeu et de game over pour le switch
    [SerializeField] private GameObject panelGameOver, panelJeu; 

    [SerializeField] private AudioSource musiqueJeu, musiqueGameManager; 



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

    public void GamePlay() // A modif avec audio
    {
        
        Time.timeScale = 1; 
        isGameRunning = true; 

        AudioListener.pause = false ; 

        panelGameOver.SetActive(false);
        panelJeu.SetActive(true);

    }

    public void Reset()
    {
        Time.timeScale = 1; 

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); //build index = numéro dans le build settings 
    
        //NE JAMAIS ACTIVER LE JEU DANS LE RESET !!! 
    } 
}
