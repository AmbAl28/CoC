using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    Transform player; // Ссылка на трансформ игрока

    // Вызывается при входе в состояние
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Находим игрока и сохраняем ссылку на его трансформ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Вызывается на каждом кадре между OnStateEnter и OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Поворачиваемся к игроку
        animator.transform.LookAt(player);

        // Рассчитываем расстояние до игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);

        // Если игрок находится на расстоянии больше 1, прекращаем атаку
        if (distance > 1)
            animator.SetBool("isAttacking", false);

        // Если игрок находится на расстоянии больше 3, прекращаем преследование
        if (distance > 3)
            animator.SetBool("isChasing", false);
    }

    // Вызывается при выходе из состояния
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}