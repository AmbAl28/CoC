using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    private float waterTime = 0f;
    private Vector3 waterPosition = new Vector3(-28.076f, 26.169f, -3.129f);
    private new Rigidbody rigidbody;
    private Animator Anim;
    private bool isGrounded;
    private float jumpForce = 1f;

    [SerializeField] private PlayerUI ui;
    private int _phone;

    [SerializeField] protected float movementSpeed = 1f;
    protected Vector3 movementVector;

    private Coroutine waterTimeCoroutine; // Ссылка на корутину
    private bool canControlCharacter = true; // Флаг, позволяющий управлять персонажем или блокировать управление

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _phone = 0;
    }

    private void FixedUpdate()
    {
        if (canControlCharacter)
        {
            CheckGrounded();
            movementVector = (transform.right * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * transform.forward).normalized;
            // Debug.Log(message: ("Vertical:", Input.GetAxis("Vertical"), " Horizontal:", Input.GetAxis("Horizontal")));
            // Debug.Log(message: ("movementVector:", movementVector));
            rigidbody.MovePosition(transform.position + movementVector * movementSpeed * Time.fixedDeltaTime);

            Anim.SetBool("isWalk", Input.GetAxis("Vertical") > 0);// Код, который будет выполнен, если персонаж двигается вперёд
            Anim.SetBool("isBack", Input.GetAxis("Vertical") < 0);// Код, который будет выполнен, если персонаж двигается назад
            Anim.SetBool("isRight", Input.GetAxis("Horizontal") > 0);// Код, который будет выполнен, если персонаж двигается вправа
            Anim.SetBool("isLeft", Input.GetAxis("Horizontal") < 0);// Код, который будет выполнен, если персонаж двигается влево

            // Проверяем, достиг ли счетчик waterTime значения 10 (время в воде)
            if (waterTime >= 10f)
            {
                // Если достиг, меняем позицию персонажа, запускает запрет на движение, до окончание анимации
                StartCoroutine(WaitAnimationFinish());
            }

            //Ускорение
            if (isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
            {
                movementSpeed = 3f;
                Anim.SetBool("isShiftRun", Input.GetKey(KeyCode.LeftShift));
            }
            else
            {
                movementSpeed = 1f;
                Anim.SetBool("isShiftRun", false);
            }

            // Удар по зажатии левой кнопки мыши
            if (Input.GetMouseButton(0) && isGrounded && !(Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0))
            {
                Anim.SetBool("isCombat", true);
                //     Hit();
            }
            else
            {
                Anim.SetBool("isCombat", false);
            }

            //Падение, подпрыгнуть
            if (isGrounded && Input.GetKey(KeyCode.Space))
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Anim.SetBool("isJump", true);
            }

            // //Проверка что персонаж на тиррейне
            // RaycastHit hit;
            // if (Physics.Raycast(transform.position, Vector3.down, out hit))
            // {
            //     if (hit.collider.CompareTag("Ground"))
            //     {
            //         // Персонаж находится на террейне
            //         isGrounded = true;
            //         Debug.Log("ТЕГ На земле");
            //     }
            // }

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
    }

    private void CheckGrounded()
    {
        // Опускаем луч вниз от нижней части персонажа и проверяем, соприкасается ли он с землей
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.37f))
        {
            isGrounded = true;
            Debug.Log("На земле");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Не на земле");
        }
    }
    //Проверка находится ли персонаж на земле
    private void OnCollisionEnter(Collision collision)
    {
        // Считаем персонажа на земле после любого столкновения
        isGrounded = true;
        Anim.SetBool("isJump", false);
        Debug.Log("На земле");
    }
    private void OnCollisionExit(Collision collision)
    {
        // При отсутствии столкновения считаем, что персонаж не на земле
        isGrounded = false;
        Debug.Log("Не на земле");

    }

    //Плаванье и корутина (счётчик) для увеличения waterTime каждую секунду в воде
    IEnumerator IncreaseWaterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            waterTime += 1f;
            Debug.Log("Water Time: " + waterTime);
        }

    }
    private IEnumerator WaitAnimationFinish()
    {
        Anim.SetBool("isSit", true);
        transform.position = waterPosition;
        canControlCharacter = false; // Запрещаем управление персонажем и камерой при начале анимации
        Debug.Log("Запрещено двигаться");
        yield return new WaitForSeconds(13f); // Сколько ждём (заменить на фактическую длину анимации)
        Debug.Log("Можно двигаться");
        Anim.SetBool("isSit", false);
        canControlCharacter = true; // Разрешаем управление персонажем и камерой после окончания анимации
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            // Запускаем корутину, если она еще не была запущена
            if (waterTimeCoroutine == null)
                waterTimeCoroutine = StartCoroutine(IncreaseWaterTime());
            Anim.SetBool("isSwim", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            // Останавливаем корутину, если она была запущена
            if (waterTimeCoroutine != null)
            {
                StopCoroutine(waterTimeCoroutine);
                waterTimeCoroutine = null;
            }

            Anim.SetBool("isSwim", false);
            waterTime = 0f; // Сбрасываем счетчик времени в воде
        }
    }
}