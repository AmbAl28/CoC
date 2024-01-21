using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Animator Anim;
    private bool isGrounded;
    private float jumpForce = 2f;

    [SerializeField] private int startHealth; //Здоровье при старте
    [SerializeField] private PlayerUI ui;
    private int health; //Здоровье

    [SerializeField] protected float movementSpeed = 3f;
    protected Vector3 movementVector;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        health = startHealth;
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

    // public void GetDamage(int damage)
    // {
    //     health = health - damage; //Отнимаем домаг от здоровья при домаге
    //     ui.SetHealth(health);

    //     if (health <= 0)
    //     {

    //     }
    // }

    // private void Hit()
    // {

    // }
}