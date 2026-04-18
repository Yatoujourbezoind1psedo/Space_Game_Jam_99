using UnityEngine;

public class MouseLock : MonoBehaviour
{
    [SerializeField] private bool isMenu = false; // Coche ça UNIQUEMENT dans la scène Menu

    void Start()
    {
        if (!isMenu)
        {
            // Cache le curseur
            Cursor.visible = false;
            // Bloque le curseur au centre de l'écran
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // Dans le menu, on veut voir Katty, pas la flèche Windows
            Cursor.visible = false;
            // On empêche la souris de sortir de la fenêtre
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}