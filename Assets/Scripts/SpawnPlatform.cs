// Importando bibliotecas necessárias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definindo a classe SpawnPlatform que herda de MonoBehaviour
public class SpawnPlatform : MonoBehaviour
{
    // Listas para armazenar plataformas e instâncias atuais de plataformas
    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();

    // Offset inicial e índice da plataforma atual
    public int offset;
    private int platformIndex;

    // Referências ao jogador e ao ponto atual da plataforma
    private Transform player;
    private Transform currentPlatformPoint;

    // Função chamada no início da execução do script
    void Start()
    {
        // Encontrando a transformação do jogador
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Instanciando as plataformas iniciais e armazenando suas transformações
        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 44), transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 44; // Ajustando o offset para a próxima plataforma
        }

        // Configurando o ponto atual da plataforma
        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    // Função chamada a cada quadro
    void Update()
    {
        // Calculando a distância entre o jogador e o ponto atual da plataforma
        float distance = player.position.z - currentPlatformPoint.position.z;

        // Verificando se é necessário reciclar plataformas
        if (distance >= 5)
        {
            // Reciclando a plataforma atual
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex++;

            // Voltando ao início da lista de plataformas se necessário
            if (platformIndex > currentPlatforms.Count - 1)
            {
                platformIndex = 0;
            }

            // Atualizando o ponto atual da plataforma
            currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    // Função para reciclar uma plataforma
    public void Recycle(GameObject platform)
    {
        // Reposicionando a plataforma no topo da lista
        platform.transform.position = new Vector3(0, 0, offset);
        offset += 44; // Ajustando o offset para a próxima plataforma
    }
}