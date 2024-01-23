using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayNightDuration = 600f; // Длительность цикла дня и ночи в секундах (10 минут)
    private float rotationSpeed;

    void Start()
    {
        rotationSpeed = 180f / dayNightDuration; // Угловая скорость вращения, чтобы завершить цикл за указанное время
    }

    void Update()
    {
        // Изменение угла поворота по оси X в течение времени
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        // Ограничение угла поворота от 0 до 180 градусов
        float currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.x, 360);
        if (currentRotation > 180)
            currentRotation -= 360;

        // Изменение интенсивности света в зависимости от положения солнца
        Light directionalLight = GetComponentInChildren<Light>();
        if (directionalLight != null)
        {
            directionalLight.intensity = Mathf.Clamp01(1 - Mathf.Abs(currentRotation / 180f));
        }

        // Если нужно выполнить какие-то дополнительные действия в определенные моменты дня/ночи, вы можете добавить соответствующие условия здесь.
        // Например, if (currentRotation < 10) { // Действия для рассвета }

        // Дополнительные действия при достижении положения солнца (0 градусов)
        if (Mathf.Abs(currentRotation) < 0.1f)
        {
            // Действия, которые нужно выполнить, когда солнце находится в определенном положении (например, смена дня на ночь)
        }
    }
}
