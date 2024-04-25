using UnityEngine;
using System.Collections;

public class MusicControllerCercov : MonoBehaviour
{
    AudioSource audioSource;
    Transform player; // Позиция игрока

    public float maxDistance = 150f;
    private float waitToMusic = 100f;

    private Coroutine musicTimeCoroutine; // Ссылка на корутину

    void Start()
    {
        // Проверяем, есть ли компонент AudioSource
        audioSource = GetComponent<AudioSource>();

        // Если компонент не найден, добавляем его
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            // Тут можно также установить другие параметры AudioSource, если необходимо
        }

        player = Camera.main.transform; // Предполагаем, что камера используется как слушатель
        // Запускаем корутину, если она еще не была запущена
        if (musicTimeCoroutine == null)
        {
            musicTimeCoroutine = StartCoroutine(IncreaseMusicTime());
        }
    }

    void Update()
    {
        // Обновляем расстояние между музыкой и игроком
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Применяем затухание на основе расстояния
        audioSource.volume = 1 - Mathf.Clamp01(distanceToPlayer / maxDistance);
    }

    void PlayMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    IEnumerator IncreaseMusicTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(60f, 180f));
            Debug.Log("Колокол!");
            PlayMusic();
        }
    }
}
