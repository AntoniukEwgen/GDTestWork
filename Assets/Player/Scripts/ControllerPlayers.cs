using UnityEngine;
using UnityEngine.UI;

public class ControllerPlayers : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jogSpeed, walkSpeed, runSpeed, groundDistance, gravity, jumpHeight, rotationSpeed = 5f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Image doubleAttackCooldownImage;

    private CharacterController characterController;
    private Vector3 moveDirection, velocity;
    private Animator animator;
    private GameManager gameManager;


    public GameObject player;

    private float doubleAttackCooldown = 2f; 
    private float lastDoubleAttackTime; 

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        gameManager = FindObjectOfType<GameManager>();

    }

    private void Update()
    {
        if (gameManager != null && gameManager.Panel.activeSelf) return;

        Move();
        if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time - lastDoubleAttackTime >= doubleAttackCooldown && player.GetComponent<PlayerAttack>().AreEnemiesInRadius()) DoubleAttack();
        doubleAttackCooldownImage.fillAmount = (Time.time - lastDoubleAttackTime) / doubleAttackCooldown;
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayerMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveZ;

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftAlt)) Jog();
                else if (Input.GetKey(KeyCode.LeftShift)) Run();
                else Walk();
            }
            else Idle();

            moveDirection *= moveSpeed;
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }

        characterController.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (moveX != 0 || moveZ != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Idle() { animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime); }
    private void Jog() { moveSpeed = jogSpeed; animator.SetFloat("Speed", 0.33f, 0.1f, Time.deltaTime); }
    private void Walk() { moveSpeed = walkSpeed; animator.SetFloat("Speed", 0.66f, 0.1f, Time.deltaTime); }
    private void Run() { moveSpeed = runSpeed; animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime); }
    private void Jump() { velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); animator.SetTrigger("Jump"); }
    private void Attack() { animator.SetTrigger("Attack"); }
    private void DoubleAttack()
    {
        animator.SetTrigger("DoubleAttack");
        lastDoubleAttackTime = Time.time; 
    }
}
