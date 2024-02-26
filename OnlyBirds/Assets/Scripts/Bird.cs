using CodeMonkey;
using System;
using UnityEngine;


public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 90f;// Quantidade de força aplicada ao pular
    private static Bird instance;// Instância única da classe Bird
    public event EventHandler OnDied;// Evento invocado quando o pássaro morre
    public event EventHandler OnStartedPlaying;// Evento invocado quando o jogo começa
    private Rigidbody2D birdRigidbody2D;// Componente Rigidbody2D do pássaro
    private State state;// Estado atual do pássaro

    public static Bird GetInstance()
    {
        return instance; // Retorna a instância única do pássaro
    }

    // Estados possíveis do pássaro
    private enum State
    {
        WaitingToStart, // Aguardando para começar
        Playing, // Jogando
        Dead// Morto
    }

    private void Awake()
    {
        instance = this; // Define a instância única como esta instância
        birdRigidbody2D = GetComponent<Rigidbody2D>();// Obtém o componente Rigidbody2D
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;// Define o tipo de corpo como estático
        state = State.WaitingToStart;// Inicialmente, o pássaro está esperando para começar
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (TestInput())// Se houver entrada de jogador
                {
                    // Começa a jogar

                    state = State.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic; // Muda o tipo de corpo para dinâmico
                    Jump();// Executa o pulo
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);// Invoca o evento de início de jogo
                }
                break;
            case State.Playing:
                if (TestInput())// Se houver entrada de jogador
                {
                    Jump(); // Executa o pulo
                }

                //Gira o pássaro à medida que salta e cai
                transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);
                break;
            case State.Dead:
                break;
        }
    }


    // Verifica se houve entrada de jogador
    private bool TestInput()
    {
        return
            Input.GetKeyDown(KeyCode.Space) ||  // Tecla de espaço pressionada
            Input.GetMouseButtonDown(0) || // Botão esquerdo do mouse pressionado
            Input.touchCount > 0; // Toque na tela
    }

    // Método para executar um pulo
    private void Jump()
    {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT; // Aplica uma força para cima para simular um pulo
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);  // Reproduz o som do pulo
    }

    // Método chamado quando o pássaro colide com um "colisor"-ponto de embate 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;// Define o tipo de corpo como estático para parar de se mover
        SoundManager.PlaySound(SoundManager.Sound.Lose);// Reproduz o som de perder
        CMDebug.TextPopupMouse("Morreu");
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

}
