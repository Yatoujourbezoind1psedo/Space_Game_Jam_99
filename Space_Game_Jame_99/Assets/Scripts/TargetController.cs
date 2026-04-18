using UnityEngine;
using UnityEngine.UI; 

public class TargetController : MonoBehaviour
{
    [SerializeField] private LaserManager laserManager;
    [SerializeField] private float vitesseScan = 5f; 
    [SerializeField] private float ptScanMoins = 1f;
    [SerializeField] private float maxScan = 20f;

    private float ptActuel = 0f;
    private bool isBeingScanned = false;

    [Header("Visuel")]
    [SerializeField] private float vitesseRemplissageImage = 10f; // Plus c'est haut, plus c'est rapide
    private Image fillImage;

    void Start()
    {
        fillImage = GetComponentInChildren<Image>();
        if (fillImage != null) fillImage.fillAmount = 0;
    }

    void Update()
    {
        // 1. Calcul du score (logique)
        if (isBeingScanned)
        {
            if (ptActuel < maxScan)
            {
                float pointsCetteFrame = vitesseScan * Time.deltaTime;
                ptActuel += pointsCetteFrame;
                laserManager.IncrementScan(pointsCetteFrame);
            }
            else 
            {
                laserManager.DecrementScan(ptScanMoins * Time.deltaTime); 
            }
        }

        // 2. Mise à jour du visuel (toujours exécuté pour que le Lerp finisse son travail)
        if (fillImage != null)
        {
            float ratioCible = ptActuel / maxScan;
            
            // Option A : Remplissage fluide mais rapide (recommandé)
            fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, ratioCible, vitesseRemplissageImage * Time.deltaTime);

            /* Note : Si tu veux que ce soit INSTANTANÉ sans animation, utilise :
               fillImage.fillAmount = ratioCible; 
            */

            // Changement de couleur quand c'est fini
            if (ptActuel >= maxScan)
            {
                fillImage.color = Color.red;
            }
        }
    }

    public void StartScan() => isBeingScanned = true;
    public void StopScan() => isBeingScanned = false;
}