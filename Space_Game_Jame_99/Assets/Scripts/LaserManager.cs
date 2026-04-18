using UnityEngine;
using TMPro;

public class LaserManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score; 
    private float tauxScan = 0f; 
    public float tauxScanMax = 100f; 

    public void IncrementScan(float ptScan)
    {
        tauxScan = Mathf.Min(tauxScan + ptScan, tauxScanMax); // Bloque au max
        UpdateUI();
    }

    public void DecrementScan(float ptScanMoins)
    {
        tauxScan = Mathf.Max(tauxScan - ptScanMoins, 0); // Bloque à 0
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (score != null)
        {
            // On affiche l'entier (Mathf.FloorToInt) pour que ce soit propre
            score.text = Mathf.FloorToInt(tauxScan).ToString() + "%";
        }
    }

    public float GetTauxScanPourCent()
    {
        // Ton tauxScan étant déjà sur 100, on retourne juste la valeur
        return (tauxScan / tauxScanMax) * 100f;
    }
}