using UnityEngine;

public class GameHandler : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("GameHandler.Start");

        // Cria um novo objeto GameObject chamado "Pipe" com um componente SpriteRenderer

        GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));

        // Define o sprite do componente SpriteRenderer para a cabeça do cano usando os assets do jogo

        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;

        // Define e salva um novo valor para a chave "highscore" nas preferências do jogador
        //PlayerPrefs.SetInt("highscore", 10);
        //PlayerPrefs.Save();

        // Obtém e imprime o valor da chave "highscore" das preferências do jogador

        Debug.Log(PlayerPrefs.GetInt("hightscore"));

        // Tenta definir um novo recorde de pontuação e o salva se for bem-sucedido
        //Score.TrySetNewHighscore(20);
    }

}
