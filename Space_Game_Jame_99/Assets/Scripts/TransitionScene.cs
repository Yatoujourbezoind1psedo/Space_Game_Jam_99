using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{ 
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;//Je le rajoute parce que sinon ça reste à 0 en faisant pause (juste au cas où)
    }
    public void QuitApp() { //attention quit marche qeu en build
        Application.Quit();
        Debug.Log("Application has quit");
    }
}
