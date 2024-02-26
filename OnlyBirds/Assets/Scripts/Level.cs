using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Constantes para valores padrão no jogo

    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = +100f;
    private const float GROUND_DESTROY_X_POSITION = -200f;
    private const float CLOUD_DESTROY_X_POSITION = -160f;
    private const float CLOUD_SPAWN_X_POSITION = +160f;
    private const float CLOUD_SPAWN_Y_POSITION = +30f;
    private const float BIRD_X_POSITION = 0f;

    // Instância estática da classe nível

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }
    // Listas para armazenar elementos do jogo

    private List<Transform> groundList;
    private List<Transform> cloudList;
    private float cloudSpawnTimer;
    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private State state;

    // Enum para representar a dificuldade do jogo
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    // Método chamado a cada frame
    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    // Método chamado no início do jogo
    private void Awake()
    {
        instance = this;
        SpawnInitialGround();
        SpawnInitialClouds();
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 5f;
        SetDifficulty(Difficulty.Easy);
        state = State.Playing;
        gapSize = 50f;
        groundList = new List<Transform>();
    }

    // Método chamado no início do jogo
    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
        // CreatePipe(40f, 20f, false);
        // CreatePipe(40f, 20f, true);


    }

    // Manipula o evento quando o pássaro inicia o jogo
    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;

    }

    // Manipula o evento quando o pássaro morre
    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        //cmdebug.textPopupMouse (“Morto!”) ;
        state = State.BirdDead;
    }

    // Método chamado a cada frame
    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }
    }

    // Método para criar nuvens iniciais
    private void SpawnInitialClouds()
    {
        cloudList = new List<Transform>();
        Transform cloudTransform;
        //float cloudY = -47.5f;
        //float groudwith = 192f;
        cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(0, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
        groundList = new List<Transform>();
        cloudList.Add(cloudTransform);
    }

    // Método para obter um prefab de nuvem aleatório
    private Transform GetCloudPrefabTransform()
    {
        switch (Random.Range(0, 3))
        {
            default:
            case 0: return GameAssets.GetInstance().pfCloud_1;
            case 1: return GameAssets.GetInstance().pfCloud_2;
            case 2: return GameAssets.GetInstance().pfCloud_3;
        }
    }

    // Manipula o movimento e a criação de nuvens
    private void HandleClouds()
    {

        //Lidar com a Cloud Spawning

        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0)
        {
            //Hora de criar outra nuvem
            float cloudSpawnTimerMax = 6f;
            cloudSpawnTimer = cloudSpawnTimerMax;
            //float cloudY = +30f
            Transform cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(CLOUD_SPAWN_X_POSITION, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
            cloudList.Add(cloudTransform);
            //float cloudY = +47.5f;
        }


        //Lidar com o clound Moving

        for (int i = 0; i < cloudList.Count; i++)
        {
            Transform cloudTransform = cloudList[i];
            //Movimentar a nuvem a menos velocidade do que os tubos para Parallax
            cloudTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime * .7f;

            if (cloudTransform.position.x < CLOUD_DESTROY_X_POSITION)
            {

                //Nuvem-clound além do ponto de destruição, destrua a si mesmo
                Destroy(cloudTransform.gameObject);
                cloudList.RemoveAt(i);
                i--;
            }
        }
    }

    // Método para criar solo inicial
    private void SpawnInitialGround()
    {
        groundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -47.5f;
        float groundWidth = 192f;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth * 2f, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
    }

    // Manipula o movimento do solo
    private void HandleGround()
    {
        foreach (Transform groundTransform in groundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (groundTransform.position.x < GROUND_DESTROY_X_POSITION)
            {

                // O solo passou pelo lado esquerdo, realoque pelo lado direito
                //Encontre a posição X mais à direita

                float rightMostXPosition = -100f;
                for (int i = 0; i < groundList.Count; i++)
                {
                    if (groundList[i].position.x > rightMostXPosition)
                    {
                        rightMostXPosition = groundList[i].position.x;
                    }
                }


                //Coloca o solo na posição mais à direita

                float groundWidth = 192f;
                groundTransform.position = new Vector3(rightMostXPosition + groundWidth, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }

    // Manipula a criação de tubos
    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            // Hora de gerar outro Tubo
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    // Manipula o movimento dos tubos
    private void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];

            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                //Tubo passou por pássaro
                pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                // Destrua o tubo
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }


    // Define a dificuldade do jogo com base no número de tubos gerados
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.4f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.3f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.0f;
                break;
        }
    }

    // Obtém a dificuldade com base no número de tubos gerados
    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 24) return Difficulty.Impossible;
        if (pipesSpawned >= 12) return Difficulty.Hard;
        if (pipesSpawned >= 5) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    // Cria dois tubos com um espaço entre eles
    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    // Cria um tubo com cabeça e corpo
    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        //Configura a cabeça do tubo
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);


        //Configura o corpo do tubo

        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        // Ajusta a altura do corpo e o collider

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        // Cria o objeto Pipe e o adiciona à lista

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }

    // Retorna o número de tubos que passaram pelo pássaro
    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }

    /*
     * 
          Representa um único tubo
     * */
    private class Pipe
    {

        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        // Move o tubo para a esquerda
        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        // Retorna a posição X da cabeça do tubo
        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        // Verifica se é o tubo inferior
        public bool IsBottom()
        {
            return isBottom;
        }

        // Destrói o tubo
        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

    }

}

