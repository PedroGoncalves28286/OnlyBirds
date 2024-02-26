using UnityEngine.SceneManagement;

public static class Loader
{
    // Enumeração para representar as cenas disponíveis no jogo
    public enum Scene
    {
        GameScene,// Cena do jogo
        Loading,// Cena de carregamento
        MainMenu,// Menu principal
    }

    // Variável privada para armazenar a cena de destino a ser carregada
    private static Scene targetScene;


    // Método para iniciar o carregamento de uma cena
    public static void Load(Scene scene)
    {



        // Carrega a cena de carregamento para exibir durante o processo
        SceneManager.LoadScene(Scene.Loading.ToString());

        // Define a cena de destino
        targetScene = scene;




    }

    // Método para carregar a cena de destino previamente definida
    public static void LoadTargetScene()
    {
        // Carrega a cena de destino

        SceneManager.LoadScene(targetScene.ToString());
    }

}
