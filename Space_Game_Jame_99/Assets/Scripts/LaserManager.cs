using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;
using TMPro;

public class LaserManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score; 
    private float tauxScan; 

    public void IncrementScan(float ptScan)
    {
        tauxScan += ptScan; 
        Debug.Log(tauxScan); 
        score.text = tauxScan.ToString()+"%";
    }
}
