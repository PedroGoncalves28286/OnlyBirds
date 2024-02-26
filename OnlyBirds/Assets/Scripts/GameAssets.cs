using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    private static GameAssets instance;// Instância única da classe GameAssets

    public static GameAssets GetInstance()
    {
        return instance;// Retorna a instância única do GameAssets
    }

    private void Awake()
    {
        instance = this;// Define a instância única como esta instância
    }

    // Referências para os assets do jogo

    public Sprite pipeHeadSprite;// Sprite da cabeça do cano
    public Transform pfPipeHead;// Prefab da cabeça do cano
    public Transform pfPipeBody;// Prefab do corpo do cano
    public Transform pfGround;// Prefab do chão
    public Transform pfCloud_1;// Prefab da nuvem 1
    public Transform pfCloud_2;// Prefab da nuvem 2
    public Transform pfCloud_3;// Prefab da nuvem 3

    public SoundAudioClip[] soundAudioClipArray; // Array de clips de áudio associados aos sons do jogo

    public AudioClip birdJump;// Clip de áudio para o som do salto do pássaro

    // Classe que associa um som a um áudio clip

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;// Tipo de som associado
        public AudioClip audioClip;// Clip de áudio associado
    }


}
