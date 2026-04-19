using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Indispensable pour Keyboard.current

public class VideoCreditManager : MonoBehaviour
{
    [Header("Configuration")]
    public VideoPlayer videoPlayer;
    public string sceneToLoad = "MenuPrincipal"; 

    [Header("Options")]
    public bool canSkip = true;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void Update()
    {
        // Utilisation du New Input System au lieu de l'ancien Input.GetKeyDown
        if (canSkip && Keyboard.current != null)
        {
            // Vérifie si la touche Espace ou Entrée est pressée ce frame
            if (Keyboard.current.spaceKey.wasPressedThisFrame || 
                Keyboard.current.enterKey.wasPressedThisFrame)
            {
                LoadNextScene();
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
        
        Debug.Log("Changement de scène vers : " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}