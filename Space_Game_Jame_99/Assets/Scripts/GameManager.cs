using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public bool isGameRunning = true; 

    //Récupération des panels de jeu et de game over pour le switch
    [SerializeField] private GameObject panelGameOver, panelJeu; 


    public void GameOver()
    {
        panelGameOver.SetActive(true); 
        panelJeu.SetActive(false); 

        //ZA WARUDO !!!!
        Time.timeScale = 0;
        isGameRunning = false;

        //ZA WARUDO AUDIO
        AudioListener.pause = true; 
    }

    public void GamePlay()
    {
        panelGameOver.SetActive(false);
        panelJeu.SetActive(true);

        Time.timeScale = 1; 
        isGameRunning = true; 

        AudioListener.pause = false ; 
    }

    public void Reset()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); //build index = numéro dans le build settings 

        Time.timeScale = 1; 
        isGameRunning = true;

        AudioListener.pause = false; 
    } 
}
