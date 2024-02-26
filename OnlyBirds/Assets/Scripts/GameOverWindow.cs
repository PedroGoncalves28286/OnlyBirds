using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{

    private Text scoreText;// Referência ao componente de texto para exibir a pontuação
    [SerializeField] private Text highscoreText;// Referência ao componente de texto para exibir o recorde


    private void Awake()
    {
        // Obtém as referências aos componentes de texto e botões do menu de Game Over
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();

        // Define a função de clique do botão de retry para recarregar a cena do jogo

        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => { UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); };


        //transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.GameScene); };

        // Adiciona sons de botão ao botão de retry

        transform.Find("retryBtn").GetComponent<Button_UI>().AddButtonSounds();

        // Define a função de clique do botão do menu principal para carregar a cena do menu principal

        transform.Find("mainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.MainMenu); };

        // Adiciona sons de botão ao botão do menu principal

        transform.Find("mainMenuBtn").GetComponent<Button_UI>().AddButtonSounds();

        // Define a posição do menu de Game Over para o centro da tela
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Start()
    {
        // Registra o método Bird_OnDied para ser chamado quando o evento OnDied do pássaro for acionado
        Bird.GetInstance().OnDied += Bird_OnDied;
        Hide();
    }

    private void Update()
    {
        // Verifica se a tecla de espaço foi pressionada para recarregar a cena do jogo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Retry
            Loader.Load(Loader.Scene.GameScene);
        }
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();

        // Verifica se a pontuação atual é maior ou igual ao recorde e atualiza o texto do recorde

        if (Level.GetInstance().GetPipesPassedCount() >= Score.GetHighscore())
        {
            // New Highscore!
            highscoreText.text = "PONTUAÇÃO MÁXIMA";
        }
        else
        {
            highscoreText.text = "PONTUAÇÃO MÁXIMA: " + Score.GetHighscore();
        }

        // Exibe o menu de Game Over
        Show();
    }

    // Método para esconder o menu de Game Over
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Método para exibir o menu de Game Over
    private void Show()
    {
        gameObject.SetActive(true);
    }

}
