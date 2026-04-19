using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement; // Obligatoire pour gérer les scènes

public class SceneChanger : MonoBehaviour
{
    private string test; 
    public void ChargerScene(string Sana test)
    {
        SceneManager.LoadScene(Sana test);
    }
}