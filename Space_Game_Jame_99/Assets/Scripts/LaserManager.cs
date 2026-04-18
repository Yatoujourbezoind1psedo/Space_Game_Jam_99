using System;
using UnityEngine;
using TMPro;

public class LaserManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score; 
    private float tauxScan; 
    public float tauxScanMax = 100f; 
    public void IncrementScan(float ptScan)
    {
        if (tauxScan < tauxScanMax)
        {
            tauxScan += ptScan; 
            //Debug.Log(tauxScan); 
            score.text = tauxScan.ToString()+"%";
        }

    }

    public void DecrementScan(float ptScanMoins)
    {
        if (tauxScan > 0)
        {
            tauxScan -= ptScanMoins; 
            score.text = tauxScan.ToString()+"%";           
        }

    }

    public float GetTauxScanPourCent() //si c'est égal à 100 alors le max a été atteint 
    {
        
        return (tauxScan / tauxScanMax) * 100; //Je passe par des entiers pour pas avoir de souci 
    }

}
