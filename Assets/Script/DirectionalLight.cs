using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayNightDuration = 5000f; // Длительность цикла дня и ночи в секундах (10 минут)
    private float rotationSpeed;
    private Light directionalLight;

    public Gradient dayNightColorGradient;

    void Start()
    {
        rotationSpeed = 180f / dayNightDuration; // Угловая скорость вращения, чтобы завершить цикл за указанное время
        directionalLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        // Изменение угла поворота по оси X в течение времени
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        // Ограничение угла поворота от 0 до 180 градусов
        float currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.x, 360);
        if (currentRotation > 180)
            currentRotation -= 360;

        // Резкое изменение интенсивности при достижении угла 0 или 180 градусов
        if (Mathf.Approximately(currentRotation, 0f) || Mathf.Approximately(currentRotation, 180f) || (currentRotation < 0))
        {
            directionalLight.intensity = 0f;
        }
        else
        {
            if (((currentRotation > 0f) || (currentRotation < 20f)) || ((currentRotation > 160f) || (currentRotation < 180f)))
            {
                // Изменение интенсивности света в зависимости от положения солнца
                directionalLight.intensity = Mathf.Lerp(0f, 1f, Mathf.Abs(currentRotation) / 20f);
            }
        }

        // Изменение цвета света в зависимости от угла поворота
        directionalLight.color = dayNightColorGradient.Evaluate(Mathf.Abs(currentRotation) / 180f);

        // Если нужно выполнить какие-то дополнительные действия в определенные моменты дня/ночи, вы можете добавить соответствующие условия здесь.
        // Например, if (currentRotation < 10) { // Действия для рассвета }

        // Дополнительные действия при достижении положения солнца (0 градусов)
        if (Mathf.Approximately(currentRotation, 0.1f))
        {
            // Действия, которые нужно выполнить, когда солнце находится в определенном положении (например, смена дня на ночь)
        }
    }
}
