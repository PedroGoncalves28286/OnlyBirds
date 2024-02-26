using UnityEngine;// Importa o namespace UnityEngine para acessar classes Unity.

// Classe responsável por controlar a janela de espera para iniciar o jogo.
public class WaitingToStartWindow : MonoBehaviour
{

    // Método chamado quando o objeto é inicializado.
    private void Start()
    {
        // Registra o método WaitingToStartWindow_OnStartedPlaying para ser chamado quando o jogo começa.

        Bird.GetInstance().OnStartedPlaying += WaitingToStartWindow_OnStartedPlaying;
    }

    // Método chamado quando o jogo começa.
    private void WaitingToStartWindow_OnStartedPlaying(object sender, System.EventArgs e)
    {
        // Esconde a janela de espera para iniciar o jogo.
        Hide();
    }

    // Método para esconder a janela de espera.
    private void Hide()
    {
        // Desativa o objeto da janela de espera.
        gameObject.SetActive(false);
    }

    // Método para mostrar a janela de espera.
    private void Show()
    {
        // Ativa o objeto da janela de espera.
        gameObject.SetActive(true);
    }

}
