using UnityEngine;
using UnityEngine.UI;

// Declaração da classe ScoreWindow, responsável por gerenciar a interface de usuário relacionada à pontuação do jogo.

public class ScoreWindow : MonoBehaviour
{
    // Referência ao componente de texto para exibir a pontuação máxima.
    private Text highscoreText;

    // Referência ao componente de texto para exibir a pontuação atual.

    private Text scoreText;

    // Método chamado assim que o objeto é inicializado.

    private void Awake()
    {
        // Localiza e atribui o componente de texto para a pontuação atual.

        scoreText = transform.Find("scoreText").GetComponent<Text>();

        // Localiza e atribui o componente de texto para a pontuação máxima.

        highscoreText = transform.Find("highscoreText").GetComponent<Text>();
    }

    // Método chamado quando o jogo começa.
    private void Start()
    {
        // Define o texto da pontuação máxima para o valor atual salvo.

        highscoreText.text = "PONTUAÇÃO MÁXIMA: " + Score.GetHighscore().ToString();

        // Registra métodos para os eventos do Bird (pássaro) quando ele morre e quando o jogo começa.

        Bird.GetInstance().OnDied += ScoreWindow_OnDied;
        Bird.GetInstance().OnStartedPlaying += ScoreWindow_OnStartedPlaying;

        // Esconde a janela de pontuação.

        Hide();
    }

    // Método chamado quando o jogo começa.

    private void ScoreWindow_OnStartedPlaying(object sender, System.EventArgs e)
    {
        // Mostra a janela de pontuação.

        Show();
    }

    // Método chamado quando o pássaro morre.
    private void ScoreWindow_OnDied(object sender, System.EventArgs e)
    {
        // Esconde a janela de pontuação.
        Hide();
    }

    // Método chamado a cada frame do jogo.
    private void Update()
    {
        // Atualiza o texto da pontuação atual com a quantidade de canos que o pássaro passou.
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
    }

    // Método para esconder a janela de pontuação.

    private void Hide()
    {
        // Desativa o objeto da janela de pontuação.
        gameObject.SetActive(false);
    }
    // Método para mostrar a janela de pontuação.
    private void Show()
    {
        // Ativa o objeto da janela de pontuação. // Ativa o objeto da janela de pontuação.
        gameObject.SetActive(true);
    }

}
