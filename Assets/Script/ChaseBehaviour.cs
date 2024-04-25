using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent; // Ссылка на агента навигации
    Transform player; // Ссылка на игрока
    float attackRange = 1; // Радиус атаки
    float chaseRange = 5; // Радиус преследования

    // Вызывается при входе в состояние
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Получаем ссылку на агента навигации и устанавливаем скорость
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1;

        // Находим игрока и сохраняем ссылку на его трансформ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Вызывается на каждом кадре между OnStateEnter и OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Устанавливаем игрока в качестве цели для преследования
        agent.SetDestination(player.position);
        // Расстояние до игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);
        // Если игрок в радиусе атаки, начинаем атаку
        if (distance < attackRange)
            animator.SetBool("isAttacking", true);
        // Если игрок слишком далеко, прекращаем преследование
        if (distance > 3)
            animator.SetBool("isChasing", false);
    }

    // Вызывается при выходе из состояния
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Останавливаем агента навигации
        agent.SetDestination(agent.transform.position);
        // Возвращаем стандартную скорость агента
        agent.speed = 1;
    }
}
