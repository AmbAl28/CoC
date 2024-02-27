using UnityEngine;

public class BaseCharacterRotation : MonoBehaviour
{
    [SerializeField] protected float sensitivity = 1.5f;
    [SerializeField] protected float smooth = 10;
    [SerializeField] protected Transform character;
    protected float yRotation;
    protected float xRotation;

    [SerializeField] private Transform fpsTransformView;
    [SerializeField] private Transform thirdPersonTransformView_1;
    [SerializeField] private Transform thirdPersonTransformView_2;
    [SerializeField] private Transform thirdPersonTransformView_3;
    [SerializeField] private float isFpsView = 0;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * sensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * sensitivity;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(xRotation, yRotation, 0), Time.deltaTime * smooth);
        character.rotation = Quaternion.Lerp(character.rotation, Quaternion.Euler(0, yRotation, 0), Time.deltaTime * smooth);

        // Приближение и отдаление камеры с помощью Tab
        if (Input.GetKeyDown(KeyCode.Tab))
            isFpsView += 1;

        // Приближение и отдаление камеры с помощью скролла колесика мыши
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            isFpsView += 1;
        }

        if (isFpsView < 1)
        {
            transform.position = fpsTransformView.position;
            xRotation = Mathf.Clamp(xRotation, -90f, 55f);
        }
        if (isFpsView == 1)
        {
            xRotation = Mathf.Clamp(xRotation, 5f, 35f);
            transform.position = thirdPersonTransformView_1.position;
        }
        if (isFpsView == 2)
        {
            xRotation = Mathf.Clamp(xRotation, 5f, 35f);
            transform.position = thirdPersonTransformView_2.position;
        }
        if (isFpsView == 3)
        {
            xRotation = Mathf.Clamp(xRotation, 5f, 35f);
            transform.position = thirdPersonTransformView_3.position;
        }
        if (isFpsView > 3)
        {
            isFpsView = 0;
        }
    }
}
