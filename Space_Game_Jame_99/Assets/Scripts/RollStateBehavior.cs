using UnityEngine;

public class RollStateBehavior : StateMachineBehaviour
{
    // OnStateExit est appelé quand la transition vers un autre état commence 
    // ou quand l'animation se termine.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Roll");
        animator.ResetTrigger("L");
        animator.ResetTrigger("R");
        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    
    

    PlayerController player = animator.GetComponentInParent<PlayerController>();
    if (player != null)
    {
        // Si tu veux rajouter une logique de fin de mouvement, c'est ici
        Debug.Log("Animation terminée, triggers nettoyés.");
    }
}
}