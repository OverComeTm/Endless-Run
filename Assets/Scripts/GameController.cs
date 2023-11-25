// Importando bibliotecas necessárias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Definindo a classe GameController que herda de MonoBehaviour
public class GameController : MonoBehaviour
{
    // Referências a objetos no Unity Editor
    public GameObject gameOver;     // Referência ao objeto de tela de Game Over
    public float score;             // Pontuação geral
    public int scoreCoin;           // Número de moedas coletadas

    // Textos para exibição de pontuações no Unity Editor
    public Text scoreText;          // Texto para exibir a pontuação geral
    public Text scoreCoinText;      // Texto para exibir o número de moedas coletadas

    private Player player;          // Referência ao componente Player

    // Função chamada no início da execução do script
    void Start()
    {
        // Encontra o objeto do jogador e obtém sua referência
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Função chamada a cada quadro
    void Update()
    {
        // Verifica se o jogador não está morto para continuar atualizando a pontuação
        if (!player.isDead)
        {
            // Incrementa a pontuação com o tempo decorrido
            score += Time.deltaTime * 10f;
            // Atualiza o texto de pontuação no Unity Editor
            scoreText.text = Mathf.Round(score).ToString() + "m";
        }
    }

    // Função para exibir a tela de Game Over
    public void ShowGameOver()
    {
        // Ativa o objeto da tela de Game Over
        gameOver.SetActive(true);
    }

    // Função para adicionar uma moeda à pontuação
    public void AddCoin()
    {
        // Incrementa o número de moedas coletadas
        scoreCoin++;
        // Atualiza o texto de moedas no Unity Editor
        scoreCoinText.text = scoreCoin.ToString();
    }
}