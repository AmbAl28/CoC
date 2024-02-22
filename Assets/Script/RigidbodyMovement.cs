using System;
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

    [SerializeField] protected float movementSpeed = 2f;
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

        Anim.SetBool("isWalk", Input.GetKey(KeyCode.W));// Код, который будет выполнен, если клавиша "W" нажата

        Anim.SetBool("isBack", Input.GetKey(KeyCode.S));// Код, который будет выполнен, если клавиша "S" нажата

        Anim.SetBool("isRight", Input.GetKey(KeyCode.D));// Код, который будет выполнен, если клавиша "D" нажата

        Anim.SetBool("isLeft", Input.GetKey(KeyCode.A));// Код, который будет выполнен, если клавиша "A" нажата

        //Ускорение
        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            movementSpeed = 4f;
            Anim.SetBool("isShiftRun", Input.GetKey(KeyCode.LeftShift));
        }
        else
        {
            movementSpeed = 2f;
            Anim.SetBool("isShiftRun", false);
        }

        // Удар по зажатии левой кнопки мыши
        if (Input.GetMouseButton(0) && isGrounded && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            Anim.SetBool("isCombat", true);
            //     Hit();
        }
        else
        {
            Anim.SetBool("isCombat", false);
        }

        //Anim.SetBool(name: "isWalk", value: movementVector.magnitude > 0.1f);

        //Перекат 
        // if (isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        // {
        //     Anim.SetBool("isRoll", Input.GetKey(KeyCode.LeftControl));
        // }
        // else Anim.SetBool("isRoll", false);

        //Падение, подрыгнуть
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Anim.SetBool("isJump", true);
        }

        //Достать инвентарь
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Debug.Log(message: Nphone);
            // Debug.Log(message: _phone);
            _phone = _phone + 1; //Переключаем экраны на телефоне
            ui.SetPhone(_phone); //Местод вызывающий включение или выключение экрана

            if (_phone >= 4)
            {
                _phone = 0;
                ui.SetPhone(_phone);
                Debug.Log(message: "Телефон убрал");
            }
        }

    }

    //Проверка находится ли персонаж на земле
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
}