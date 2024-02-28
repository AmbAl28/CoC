using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource audioSource;
    Transform player; // Позиция игрока

    public float maxDistance = 20f;

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
        PlayMusic();
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
}
