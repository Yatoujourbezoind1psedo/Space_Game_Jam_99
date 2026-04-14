using UnityEngine;

public class next_novel : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // On récupère le composant Animator attaché à l'objet
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Si on appuie sur la touche
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // On active le trigger défini dans l'Animator
            animator.SetTrigger("next");
        }
    }
}