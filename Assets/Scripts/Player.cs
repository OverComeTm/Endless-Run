using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public float score;
    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    public float rayRadius;
    public LayerMask layer;
    public LayerMask coinLayer;
    public float horizontalSpeed;
    private bool isMovingLeft;
    private bool isMovingRight;

    public Animator anim;
    public bool isDead;

    private GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if(controller.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 5f && !isMovingRight)
            {
                isMovingRight = true;
                StartCoroutine(RightMove());
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -5f && !isMovingLeft)
            {
                isMovingLeft = true;
                StartCoroutine(LeftMove());
            }
        }
        else
        {
            jumpVelocity -= gravity;
        }

        OnCollision();

        direction.y = jumpVelocity;

        controller.Move(direction * Time.deltaTime);

    }

    IEnumerator LeftMove()
    {
        for(float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.left * Time.deltaTime * horizontalSpeed);
            yield return null;
        }
        isMovingLeft = false;

    }

    IEnumerator RightMove()
    {
        for(float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.right * Time.deltaTime * horizontalSpeed);
            yield return null;
        }

        isMovingRight = false;
    }

    void OnCollision()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRadius, layer) && !isDead)
        {
            // Chama o Game Over
            anim.SetTrigger("die");
            speed = 0;
            jumpHeight = 0;
            horizontalSpeed = 0;
            Invoke("GameOver", 3f);


            isDead = true;
        }


        RaycastHit coinHit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0,1f,0)), out coinHit, rayRadius, coinLayer))
        {
            // Colisão com a moeda
            gc.AddCoin();
            Destroy(coinHit.transform.gameObject);
            
        }
    }


    void GameOver()
    {
        gc.ShowGameOver();
    }
}