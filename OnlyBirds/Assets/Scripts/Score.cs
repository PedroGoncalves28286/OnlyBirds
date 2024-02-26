using UnityEngine;

public static class Score
{
    
    
    // Método para iniciar o sistema de pontuação
    public static void Start()
    {
        // Adiciona um evento para ser chamado quando o pássaro morrer
        ResetHighscore();
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    private static void Bird_OnDied(object sender, System.EventArgs e)
    {
        // Tenta definir um novo recorde com base no número de tubos que o jogador passou

        TrySetNewHighscore(Level.GetInstance().GetPipesPassedCount());
    }

    // Método para obter o recorde atual do jogo
    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscore" );
    }

    // Método para tentar definir um novo recorde
    public static bool TrySetNewHighscore(int score)
    {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore)
        {
            // Define um novo recorde se a pontuação atual for maior do que o recorde anterior
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Método para redefinir o recorde para 0
    public static void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
