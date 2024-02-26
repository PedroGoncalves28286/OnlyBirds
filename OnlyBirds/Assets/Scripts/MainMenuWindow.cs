using CodeMonkey.Utils;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{


    // Este método é chamado quando o GameObject é ativado pela primeira vez

    private void Awake()
    {
        // Configura o botão "playBtn" para carregar a cena do jogo quando clicado

        transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.GameScene); };

        // Adiciona sons de botão ao botão "playBtn"
        transform.Find("playBtn").GetComponent<Button_UI>().AddButtonSounds();

        // Configura o botão "quitBtn" para sair do jogo quando clicado

        transform.Find("quitBtn").GetComponent<Button_UI>().ClickFunc = () => { Application.Quit(); };

        // Adiciona sons de botão ao botão "quitBtn"
        transform.Find("quitBtn").GetComponent<Button_UI>().AddButtonSounds();
    }

}
