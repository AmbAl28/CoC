using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // [SerializeField] private GameObject[] infoText;
    [SerializeField] private GameObject[] phones; //массив с изображениями экранов
    public void SetPhone(int phone) //Публичный метод принимающий в себя какой экран показать
    {
        if (phone > phones.Length) return; //Проверка на общюю длину массива экранов, если закончилась длинна, то останавливаем функцию SetPhone и всё что в ней
        for (int i = 0; i < phones.Length; i++)
        {
            phones[i].SetActive(phone > i); //SetActive включает и выключает видимость объекта на UI экране
        }
    }

}
