// using UnityEngine;
// using System.Collections.Generic;
// using TMPro;

// public class InteractionController : MonoBehaviour
// {
//     [SerializeField] private GameObject notificationObject; // Ссылка на объект с уведомлением
//     private TextMeshProUGUI notificationText; // Компонент текста уведомления
//     public float interactionDistance = 1f; // Расстояние для взаимодействия
//     public KeyCode interactionKey = KeyCode.E; // Кнопка для взаимодействия
//     public string interactionPrompt = "Нажмите E для взаимодействия"; // Текст уведомления о взаимодействии

//     public List<GameObject> interactableObjects; // Список объектов, с которыми взаимодействует персонаж
//     private GameObject nearestObject; // Ближайший объект для взаимодействия
//     private float collectedObject = 0f;

//     private void Start()
//     {
//         // Получаем компонент TextMeshProUGUI
//         notificationText = notificationObject.GetComponent<TextMeshProUGUI>();
//         // Устанавливаем начальный текст уведомления
//         notificationText.text = interactionPrompt;
//     }

//     private void Update()
//     {
//         nearestObject = GetNearestInteractableObject();

//         if (nearestObject != null)
//         {
//             // Проверяем дистанцию и нажатие кнопки E для ближайшего объекта
//             if (Vector3.Distance(transform.position, nearestObject.transform.position) <= interactionDistance)
//             {
//                 // Отображаем уведомление
//                 notificationObject.SetActive(true);

//                 if (Input.GetKeyDown(interactionKey))
//                 {
//                     // Выполняем действие при взаимодействии
//                     InteractWithObject(nearestObject);
//                 }
//             }
//             else
//             {
//                 // Скрываем уведомление
//                 notificationObject.SetActive(false);
//                 // Устанавливаем начальный текст уведомления
//                 notificationText.text = interactionPrompt;
//             }
//         }
//         else
//         {
//             // Если ближайший объект не найден, скрываем уведомление
//             notificationObject.SetActive(false);
//             // Устанавливаем начальный текст уведомления
//             notificationText.text = interactionPrompt;
//         }
//         if (collectedObject >= 5)
//         {
//             notificationText.text = "Ты собрал все полешки!";
//             // Отображаем уведомление
//             notificationObject.SetActive(true);
//         }
//     }

//     GameObject GetNearestInteractableObject()
//     {
//         float minDistance = Mathf.Infinity;
//         GameObject nearestObj = null;

//         foreach (GameObject obj in interactableObjects)
//         {
//             float distance = Vector3.Distance(transform.position, obj.transform.position);
//             if (distance <= interactionDistance && distance < minDistance)
//             {
//                 minDistance = distance;
//                 nearestObj = obj;
//             }
//         }

//         return nearestObj;
//     }

//     void InteractWithObject(GameObject obj)
//     {
//         // Реализуйте здесь действие, которое должно произойти при взаимодействии с объектом
//         Debug.Log("Взаимодействие с объектом: " + obj.name);

//         // Пример изменения текста уведомления
//         notificationText.text = "Действие произведено: " + obj.name;

//         // Удаляем объект из списка interactableObjects
//         interactableObjects.Remove(obj);
//         Destroy(obj);
//         collectedObject = collectedObject + 1;
//         Debug.Log(collectedObject);
//     }
// }


using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private GameObject notificationObject; // Ссылка на объект с уведомлением
    private TextMeshProUGUI notificationText; // Компонент текста уведомления
    [SerializeField] private TextMeshProUGUI questText; // Компонент текста задания
    public float interactionDistance = 1f; // Расстояние для взаимодействия
    public KeyCode interactionKey = KeyCode.E; // Кнопка для взаимодействия
    private GameObject nearestObject; // Ближайший объект для взаимодействия
    private bool questCompleted = false; // Флаг для отслеживания выполнения задания
    private float collectedObject = 0f;

    private void Start()
    {
        // Получаем компонент TextMeshProUGUI для уведомления
        notificationText = notificationObject.GetComponent<TextMeshProUGUI>();
        // Устанавливаем начальный текст уведомления
        notificationText.text = "Нажмите E для взаимодействия";
        // Скрываем текст задания в начале игры
        questText.gameObject.SetActive(false);
    }

    private void Update()
    {
        nearestObject = GetNearestInteractableObject();

        if (nearestObject != null)
        {
            if (Vector3.Distance(transform.position, nearestObject.transform.position) <= interactionDistance)
            {
                notificationObject.SetActive(true);

                if (Input.GetKeyDown(interactionKey))
                {
                    InteractWithObject(nearestObject);
                }
            }
            else
            {
                notificationObject.SetActive(false);
                notificationText.text = "Нажмите E для взаимодействия";
            }
        }
        else
        {
            notificationObject.SetActive(false);
            notificationText.text = "Нажмите E для взаимодействия";
        }
    }

    GameObject GetNearestInteractableObject()
    {
        // Ищем все объекты с тегами "Interactable" и "Quest"
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject[] questObjects = GameObject.FindGameObjectsWithTag("Quest");

        // Объединяем массивы в один
        List<GameObject> allObjects = new List<GameObject>();
        allObjects.AddRange(interactableObjects);
        allObjects.AddRange(questObjects);

        // Находим ближайший объект для взаимодействия
        float minDistance = Mathf.Infinity;
        GameObject nearestObj = null;

        foreach (GameObject obj in allObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= interactionDistance && distance < minDistance)
            {
                minDistance = distance;
                nearestObj = obj;
            }
        }

        return nearestObj;
    }

    void InteractWithObject(GameObject obj)
    {
        // Debug.Log("Взаимодействие с объектом: " + obj.name);

        // Если объект имеет тег "Quest" и задание не выполнено
        if (obj.CompareTag("Quest") && !questCompleted)
        {
            // Показываем текст задания
            questText.gameObject.SetActive(true);
            // Изменяем текст задания
            questText.text = "Задание: Собери полешки вокруг дома: " + collectedObject + " из 5";
            // Debug.Log("Квест взят " + questCompleted);

            if (collectedObject >= 5)
            {
                // Изменяем текст задания
                questText.text = "Задание: Завершено, спасибо за полешки!";
                // Debug.Log("Квест сдан " + questCompleted);
                // Помечаем задание как активное
                questCompleted = true;

            }
        }
        else
        {
            if (obj.CompareTag("Interactable") && !questCompleted)
            {
                // notificationText.text = "Действие произведено: " + obj.name;
                Destroy(obj);
                collectedObject = collectedObject + 1;
                // Debug.Log(collectedObject);
                // Изменяем текст задания
                questText.text = "Задание: Собери полешки вокруг дома: " + collectedObject + " из 5";
            }
        }
    }
}
