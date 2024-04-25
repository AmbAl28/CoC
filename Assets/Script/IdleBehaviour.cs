using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    float timer; // Таймер для отслеживания времени пребывания в состоянии покоя
    Transform player; // Ссылка на трансформ игрока
    float chaseRange = 3; // Радиус преследования игрока

    // Вызывается при входе в состояние покоя
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Сброс таймера
        timer = 0;
        // Находим игрока и сохраняем ссылку на его трансформ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Вызывается на каждом кадре между OnStateEnter и OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Увеличиваем таймер
        timer += Time.deltaTime;
        // Если прошло больше 5 секунд, начинаем патрулировать
        if (timer > 5)
            animator.SetBool("isPatrolling", true);

        // Рассчитываем расстояние до игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);
        // Если игрок в радиусе преследования, начинаем преследование
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    // Вызывается при выходе из состояния покоя
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Здесь можно добавить дополнительную логику, если необходимо
    }
}
