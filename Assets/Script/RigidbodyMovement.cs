using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Animator Anim;
    private bool isGrounded;
    private float jumpForce = 2f;

    [SerializeField] private PlayerUI ui;
    private int _phone; //Экран телефона
    [SerializeField] private int Nphone;

    [SerializeField] protected float movementSpeed = 3f;
    protected Vector3 movementVector;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _phone = 0;//Установить экран телефона при старте
    }

    private void FixedUpdate()
    {
        movementVector = (transform.right * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * transform.forward).normalized;

        rigidbody.MovePosition(transform.position + movementVector * movementSpeed * Time.fixedDeltaTime);

        Anim.SetBool("isRun", Input.GetKey(KeyCode.W));// Код, который будет выполнен, если клавиша "W" нажата

        Anim.SetBool("isBack", Input.GetKey(KeyCode.S));// Код, который будет выполнен, если клавиша "S" нажата

        Anim.SetBool("isRight", Input.GetKey(KeyCode.D));// Код, который будет выполнен, если клавиша "D" нажата

        Anim.SetBool("isLeft", Input.GetKey(KeyCode.A));// Код, который будет выполнен, если клавиша "A" нажата

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 5f;
            Anim.SetBool("isShiftRun", Input.GetKey(KeyCode.LeftShift));// Код, который будет выполнен, если клавиша "LeftShift" нажата
        }
        else
        {
            movementSpeed = 3f;
            Anim.SetBool("isShiftRun", Input.GetKey(KeyCode.LeftShift));// Код, который будет выполнен, если клавиша "LeftShift" нажата
        }
        // if (Imput.GetMouseButton(0)) //удар при клике
        // {
        //     Hit();
        // }
        //Anim.SetBool(name: "isWalk", value: movementVector.magnitude > 0.1f);

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            Jump();
            Anim.SetBool("isJump", true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Debug.Log(message: Nphone);
            // Debug.Log(message: _phone);
            _phone = _phone + Nphone; //Переключаем экраны на телефоне
            ui.SetPhone(_phone); //Местод вызывающий включение или выключение экрана

            if (_phone >= 4)
            {
                _phone = 0;
                ui.SetPhone(_phone);
                Debug.Log(message: "Телефон убрал");
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Считаем персонажа на земле после любого столкновения
        isGrounded = true;
        Anim.SetBool(name: "isJump", value: false);
    }

    private void OnCollisionExit(Collision collision)
    {
        // При отсутствии столкновения считаем, что персонаж не на земле
        isGrounded = false;
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}