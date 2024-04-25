using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpWeapon : MonoBehaviour
{
    public float interactionDistance = 1f; // Расстояние для взаимодействия
    public KeyCode interactionKey = KeyCode.E; // Кнопка для взаимодействия
    public GameObject notificationObject; // Объект для отображения уведомления
    private TextMeshProUGUI notificationText; // Текст уведомления
    private GameObject nearestObject; // Ближайший предмет для взаимодействия
    private GameObject currentWeapon; // Текущий подобранный предмет
    private bool canPickUp = false; // Можно ли подобрать предмет

    private void Start()
    {
        // // Получаем компонент текста уведомления
        // notificationText = notificationObject.GetComponent<TextMeshProUGUI>();
        // // Устанавливаем начальный текст уведомления
        // notificationText.text = "Нажмите E для взаимодействия";
        // // Скрываем уведомление в начале игры
        // notificationObject.SetActive(false);
    }

    private void Update()
    {
        // Находим ближайший предмет для взаимодействия
        nearestObject = GetNearestInteractableObject();

        // Если ближайший предмет есть
        if (nearestObject != null)
        {
            // Рассчитываем расстояние до ближайшего предмета
            float distance = Vector3.Distance(transform.position, nearestObject.transform.position);

            // Если расстояние меньше или равно дистанции взаимодействия
            if (distance <= interactionDistance)
            {
                // Отображаем уведомление
                // notificationObject.SetActive(true);

                // Если нажата кнопка взаимодействия
                if (Input.GetKeyDown(interactionKey))
                {
                    InteractWithObject(nearestObject);
                }
            }
            else
            {
                // Прячем уведомление
                // notificationObject.SetActive(false);
            }
        }
        else
        {
            // Прячем уведомление
            // notificationObject.SetActive(false);
        }

        // Если нажата кнопка E и можно подобрать предмет
        if (Input.GetKeyDown(interactionKey) && canPickUp && nearestObject != currentWeapon)
        {
            Drop(); // Вызываем метод отпускания предмета
        }
    }

    // Функция для нахождения ближайшего предмета для взаимодействия
    GameObject GetNearestInteractableObject()
    {
        GameObject[] weaponObjects = GameObject.FindGameObjectsWithTag("Weapon");

        float minDistance = Mathf.Infinity;
        GameObject nearestObj = null;

        foreach (GameObject obj in weaponObjects)
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

    // Функция для взаимодействия с объектом
    void InteractWithObject(GameObject obj)
    {
        // Поднимаем текущий предмет
        Debug.Log("Поднял");
        canPickUp = true;
        currentWeapon = obj;
        currentWeapon.transform.parent = transform;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localEulerAngles = Vector3.zero;

        // Отключаем физическое тело объекта
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    // Функция для отпускания предмета
    void Drop()
    {
        Debug.Log("Бросаю");
        // Включаем физическое тело объекта перед отпусканием
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Отсоединяем предмет от игрока
        currentWeapon.transform.parent = null;
        // Сбрасываем флаг подбора
        canPickUp = false;
    }
}
