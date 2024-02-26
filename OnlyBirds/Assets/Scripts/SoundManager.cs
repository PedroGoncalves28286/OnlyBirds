using CodeMonkey.Utils;// Importa o namespace CodeMonkey.Utils para acessar a classe Button_UI.
using UnityEngine;// Importa o namespace UnityEngine para acessar classes Unity.

// Classe estática responsável por gerenciar sons no jogo.
public static class SoundManager
{

    // Enumeração para identificar os diferentes tipos de sons no jogo.
    public enum Sound
    {
        BirdJump,// Som do pássaro voando.
        Score,// Som do pássaro voando.
        Lose,// Som de perder o jogo.
        ButtonOver,// Som de hover sobre um botão.
        ButtonClick,// Som de clique em um botão.
    }

    // Método estático para reproduzir um som específico.
    public static void PlaySound(Sound sound)
    {
        // Cria um novo objeto de jogo para reproduzir o som.
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        // Obtém o componente AudioSource do objeto de som criado.
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        // Reproduz o som especificado.
        audioSource.PlayOneShot(GetAudioClip(sound));
        // Reproduz o som de pulo do pássaro independentemente do som especificado.
        audioSource.PlayOneShot(GameAssets.GetInstance().birdJump);
    }

    // Método privado estático para obter o AudioClip correspondente a um Sound.
    private static AudioClip GetAudioClip(Sound sound)
    {
        // Itera sobre todos os SoundAudioClips armazenados em GameAssets para encontrar o correspondente ao som especificado.

        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {

            // Verifica se o SoundAudioClip atual corresponde ao som especificado.
            if (soundAudioClip.sound == sound)
            {
                // Retorna o AudioClip correspondente.

                return soundAudioClip.audioClip;
            }
        }

        // Se nenhum AudioClip correspondente for encontrado, exibe um erro no console.

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    // Método de extensão para adicionar sons a um botão.
    public static void AddButtonSounds(this Button_UI buttonUI)
    {
        // Adiciona uma função que reproduz o som de hover ao passar o mouse sobre o botão.
        buttonUI.MouseOverOnceFunc += () => PlaySound(Sound.ButtonOver);

        // Adiciona uma função que reproduz o som de clique ao clicar no botão.
        buttonUI.ClickFunc += () => PlaySound(Sound.ButtonClick);
    }

}
