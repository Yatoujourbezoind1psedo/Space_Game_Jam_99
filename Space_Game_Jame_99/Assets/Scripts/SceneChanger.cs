using UnityEngine;
using UnityEngine.SceneManagement; // Obligatoire pour gérer les scènes

public class SceneChanger : MonoBehaviour
{
    public void ChargerScene(string Sanatest)
    {
        SceneManager.LoadScene(Sanatest);
    }
}