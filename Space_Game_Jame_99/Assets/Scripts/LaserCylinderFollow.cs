using UnityEngine;

public class LaserCylinderFollow : MonoBehaviour
{
    [Header("Points d'ancrage (N'importe où dans la hiérarchie)")]
    public Transform anchorSatellite; 
    public Transform anchorCible;     

    [Header("Paramètres")]
    public float epaisseur = 0.1f;
    [Tooltip("1 si le pivot est au bord, 0.5 si le pivot est au centre du cylindre")]
    public float multiplicateurLongueur = 0.5f; 

    void Update()
    {
        if (anchorSatellite == null || anchorCible == null) return;

        // --- 1. POSITION ABSOLUE ---
        // On place le laser au milieu des deux points en coordonnées monde
        Vector3 positionMondeA = anchorSatellite.position;
        Vector3 positionMondeB = anchorCible.position;
        transform.position = (positionMondeA + positionMondeB) / 2f;

        // --- 2. ROTATION ABSOLUE ---
        // On oriente l'axe Z vers la cible
        transform.LookAt(positionMondeB);
        // On tourne de 90° sur Y pour aligner l'axe X (rouge) de ton cylindre vers la cible
        transform.Rotate(0, 90, 0);

        // --- 3. SCALE ABSOLU ---
        float distanceAbsolue = Vector3.Distance(positionMondeA, positionMondeB);
        
        // On récupère le scale du parent pour l'annuler (pour que le laser ignore le scale de Hitbox)
        Vector3 scaleParent = transform.parent != null ? transform.parent.lossyScale : Vector3.one;

        // Calcul du scale final pour compenser les déformations des parents
        float finalX = (distanceAbsolue * multiplicateurLongueur) / scaleParent.x;
        float finalY = epaisseur / scaleParent.y;
        float finalZ = epaisseur / scaleParent.z;

        transform.localScale = new Vector3(finalX, finalY, finalZ);
    }
}