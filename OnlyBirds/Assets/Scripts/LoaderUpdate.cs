using UnityEngine;

public class LoaderUpdate : MonoBehaviour
{

    // Este método é chamado uma vez por frame
    private void Update()
    {
        // Chamando o método LoadTargetScene() da classe Loader
        // Isso permite que a cena de destino seja carregada continuamente enquanto este script estiver ativo
        Loader.LoadTargetScene();
    }

}
