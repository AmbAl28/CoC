using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    float timer; // Таймер для смены точек патрулирования
    List<Transform> points = new List<Transform>(); // Список точек патрулирования
    NavMeshAgent agent; // Ссылка на агента навигации

    Transform player; // Ссылка на игрока
    float chaseRange = 3; // Радиус преследования

    // Вызывается при входе в состояние
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        // Находим и сохраняем точки патрулирования
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in pointsObject)
            points.Add(t);

        // Получаем ссылку на агента навигации и устанавливаем первую точку патрулирования
        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);

        // Находим игрока и сохраняем ссылку на его трансформ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Вызывается на каждом кадре между OnStateEnter и OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Если агент достиг цели, выбираем новую случайную точку патрулирования
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(points[Random.Range(0, points.Count)].position);

        // Увеличиваем таймер
        timer += Time.deltaTime;
        // Если прошло больше 10 секунд, перестаем патрулировать
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        // Если игрок в радиусе преследования, начинаем преследование
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    // Вызывается при выходе из состояния
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Останавливаем агента навигации
        agent.SetDestination(agent.transform.position);
    }
}
