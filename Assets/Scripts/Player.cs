// Importando bibliotecas necessárias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definindo a classe Player que herda de MonoBehaviour
public class Player : MonoBehaviour
{
    // Variáveis para controle do personagem
    private CharacterController controller;

    public float score;           // Pontuação do jogador
    public float speed;           // Velocidade de movimentação
    public float jumpHeight;      // Altura do salto
    private float jumpVelocity;   // Velocidade vertical do salto
    public float gravity;         // Gravidade aplicada durante o pulo

    // Variáveis para detecção de colisões
    public float rayRadius;       // Raio para os raycasts
    public LayerMask layer;       // Layer para raycasts
    public LayerMask coinLayer;   // Layer específica para detectar moedas
    public float horizontalSpeed; // Velocidade horizontal para movimentação lateral
    private bool isMovingLeft;    // Flag para verificar se está movendo para a esquerda
    private bool isMovingRight;   // Flag para verificar se está movendo para a direita

    // Variáveis para animação e controle de morte
    public Animator anim;  // Referência ao componente Animator
    public bool isDead;     // Flag para indicar se o jogador está morto

    private GameController gc;  // Referência ao GameController

    // Função chamada no início da execução do script
    void Start()
    {
        // Obtém uma referência ao CharacterController do objeto
        controller = GetComponent<CharacterController>();
        // Encontra uma instância do GameController na cena
        gc = FindObjectOfType<GameController>();
    }

    // Função chamada a cada quadro
    void Update()
    {
        // Vetor de direção para movimentação
        Vector3 direction = Vector3.forward * speed;

        // Verifica se o personagem está no chão
        if (controller.isGrounded)
        {
            // Verifica se a tecla de espaço foi pressionada para pular
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;  // Configura a velocidade de pulo
            }
            
            // Verifica se a tecla de seta direcional para a direita foi pressionada e movimenta para a direita
            if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 5f && !isMovingRight)
            {
                isMovingRight = true;
                StartCoroutine(RightMove());  // Inicia uma coroutine para movimentação lateral para a direita
            }
            
            // Verifica se a tecla de seta direcional para a esquerda foi pressionada e movimenta para a esquerda
            if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -5f && !isMovingLeft)
            {
                isMovingLeft = true;
                StartCoroutine(LeftMove());  // Inicia uma coroutine para movimentação lateral para a esquerda
            }
        }
        else
        {
            jumpVelocity -= gravity;  // Aplica a gravidade durante o pulo
        }

        OnCollision();  // Verifica colisões com raycasts

        direction.y = jumpVelocity;  // Adiciona a componente vertical à direção

        controller.Move(direction * Time.deltaTime);  // Move o personagem

    }

    // Coroutine para movimentação lateral para a esquerda
    IEnumerator LeftMove()
    {
        for (float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.left * Time.deltaTime * horizontalSpeed);  // Move para a esquerda
            yield return null;  // Aguarda o próximo quadro
        }
        isMovingLeft = false;  // Reseta a flag de movimento para a esquerda
    }

    // Coroutine para movimentação lateral para a direita
    IEnumerator RightMove()
    {
        for (float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.right * Time.deltaTime * horizontalSpeed);  // Move para a direita
            yield return null;  // Aguarda o próximo quadro
        }

        isMovingRight = false;  // Reseta a flag de movimento para a direita
    }

    // Função para detecção de colisões com raycasts
    void OnCollision()
    {
        RaycastHit hit;

        // Verifica se há colisão com um obstáculo à frente
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRadius, layer) && !isDead)
        {
            // Chama a animação de morte e desabilita movimentos
            anim.SetTrigger("die");
            speed = 0;
            jumpHeight = 0;
            horizontalSpeed = 0;
            Invoke("GameOver", 3f);  // Chama a função GameOver após 3 segundos

            isDead = true;  // Configura a flag de morte
        }

        RaycastHit coinHit;

        // Verifica se há colisão com uma moeda à frente e acima
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0, 1f, 0)), out coinHit, rayRadius, coinLayer))
        {
            // Colisão com a moeda - incrementa a pontuação e destrói a moeda
            gc.AddCoin();
            Destroy(coinHit.transform.gameObject);

        }
    }

    // Função chamada quando o jogo termina
    void GameOver()
    {
        gc.ShowGameOver();  // Exibe a tela de Game Over
    }
}