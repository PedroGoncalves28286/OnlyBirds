using UnityEngine;// Importa o namespace UnityEngine para acessar classes Unity.

// Classe responsável por animar um sprite exibindo uma sequência de frames.
public class SpriteAnimator : MonoBehaviour
{
    // Array de sprites representando os frames da animação.
    public Sprite[] frames;
    // Número de frames por segundo para a animação.
    public int framesPerSecond = 30;
    // Indica se a animação deve se repetir em um loop.
    public bool loop = true;
    // Delegado para executar ações quando a animação termina um loop.
    public delegate void OnLoopDel();
    public OnLoopDel onLoop;
    // Indica se o tempo deve ser contado usando o tempo de jogo não escalonado.
    public bool useUnscaledDeltaTime;

    // Indica se a animação está ativa.
    private bool isActive = true;
    // Contador de tempo para controlar a animação.
    private float timer;
    // O tempo máximo entre cada frame da animação.
    private float timerMax;
    // Índice do frame atual na animação.
    private int currentFrame;
    // Componente SpriteRenderer do objeto para exibir os frames da animação.
    private SpriteRenderer spriteRenderer;

    // Método chamado assim que o objeto é inicializado.
    private void Awake()
    {
        // Calcula o tempo máximo entre cada frame com base na taxa de frames por segundo.
        timerMax = 1f / framesPerSecond;
        // Obtém o componente SpriteRenderer do objeto.
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        // Verifica se há frames na animação.
        if (frames != null)
        {
            // Define o primeiro frame da animação.
            spriteRenderer.sprite = frames[0];
        }
        else
        {
            // Se não houver frames na animação, desativa a animação.
            isActive = false;
        }
    }

    // Método chamado a cada frame do jogo.
    private void Update()
    {
        // Se a animação não estiver ativa, retorna sem fazer nada.
        if (!isActive) return;
        // Incrementa o timer com base no tempo passado desde o último frame.
        timer += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        bool newFrame = false;
        // Verifica se é hora de atualizar para o próximo frame.
        while (timer >= timerMax)
        {
            timer -= timerMax;
            // Avança para o próximo frame.
            currentFrame = (currentFrame + 1) % frames.Length;
            newFrame = true;
            // Se a animação terminar um loop.
            if (currentFrame == 0)
            {
                // Se a animação não deve fazer loop, desativa a animação.
                if (!loop)
                {
                    isActive = false;
                    newFrame = false;
                }
                // Executa a ação de loop, se houver.
                if (onLoop != null)
                {
                    onLoop();
                }
            }
        }
        // Se houver um novo frame, atualiza o SpriteRenderer para exibir o frame atual.
        if (newFrame)
        {
            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    // Método para configurar a animação com novos frames e taxa de frames por segundo.
    public void Setup(Sprite[] frames, int framesPerSecond)
    {
        // Define os novos frames e taxa de frames por segundo.
        this.frames = frames;
        this.framesPerSecond = framesPerSecond;
        // Calcula o novo tempo máximo entre cada frame.
        timerMax = 1f / framesPerSecond;
        // Define o primeiro frame da animação.
        spriteRenderer.sprite = frames[0];
        timer = 0f;
        // Inicia a animação.
        PlayStart();
    }

    // Método para iniciar a reprodução da animação a partir do primeiro frame.
    public void SetTimerMax(float timerMax)
    {
        this.timerMax = timerMax;
    }

    // Método para iniciar a reprodução da animação a partir do primeiro frame.
    public void PlayStart()
    {
        // Reinicia o timer e o índice do frame.
        timer = 0;
        currentFrame = 0;
        // Define o primeiro frame da animação.
        spriteRenderer.sprite = frames[currentFrame];
        // Ativa a animação.
        isActive = true;
    }

}
