using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     *UI Container para barras múltiplas, útil para exibir várias barras internas, como hipóteses de sucesso e chance de falha
     * */
    public class UI_BarMultiple
    {

        private GameObject gameObject;// GameObject para armazenar a barra múltipla
        private RectTransform rectTransform;// RectTransform para posicionar e dimensionar a barra múltipla
        private RectTransform[] barArr;// Array de RectTransforms para as barras internas
        private Image[] barImageArr;// Array de Image para as barras internas
        private Vector2 size; // Tamanho da barra múltipla

        // Classe para definir o contorno da barra múltipla
        public class Outline
        {
            public float size = 1f;// Tamanho do contorno
            public Color color = Color.black;// Cor do contorno
            public Outline(float size, Color color)// Construtor para inicializar o contorno
            {
                this.size = size;
                this.color = color;
            }
        }


        // Construtor para inicializar a barra múltipla na interface do usuário
        public UI_BarMultiple(Transform parent, Vector2 anchoredPosition, Vector2 size, Color[] barColorArr, Outline outline)
        {
            this.size = size;
            SetupParent(parent, anchoredPosition, size);// Configura o pai e a posição ancorada da barra múltipla
            if (outline != null) SetupOutline(outline, size);// Configura o contorno, se fornecido
            List<RectTransform> barList = new List<RectTransform>(); // Lista para armazenar as barras internas
            List<Image> barImageList = new List<Image>();// Lista para armazenar as imagens das barras internas
            List<float> defaultSizeList = new List<float>();// Lista para armazenar os tamanhos padrão das barras internas
            foreach (Color color in barColorArr)// Para cada cor na matriz de cores
            {
                barList.Add(SetupBar(color));// Configura uma barra interna com a cor correspondente
                defaultSizeList.Add(1f / barColorArr.Length);// Adiciona o tamanho padrão da barra interna à lista
            }
            barArr = barList.ToArray(); // Converte a lista de RectTransforms em um array
            barImageArr = barImageList.ToArray();// Converte a lista de Images em um array
            SetSizes(defaultSizeList.ToArray());// Define os tamanhos das barras internas com base nos tamanhos padrão
        }

        // Configura o master da barra múltipla na interface do usuário
        private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size)
        {
            gameObject = new GameObject("UI_BarMultiple", typeof(RectTransform));// Cria um novo GameObject para a barra múltipla
            rectTransform = gameObject.GetComponent<RectTransform>(); // Obtém o RectTransform do GameObject
            rectTransform.SetParent(parent, false);// Define o master do RectTransform e mantém sua escala original
            rectTransform.sizeDelta = size;// Define o tamanho do RectTransform
            rectTransform.anchorMin = new Vector2(0, .5f);// Define a âncora mínima do RectTransform
            rectTransform.anchorMax = new Vector2(0, .5f);// Define a âncora máxima do RectTransform
            rectTransform.pivot = new Vector2(0, .5f);// Define o pivô do RectTransform
            rectTransform.anchoredPosition = anchoredPosition; // Define a posição ancorada do RectTransform
        }

        // Configura o contorno das barras múltiplas
        private void SetupOutline(Outline outline, Vector2 size)
        {
            UtilsClass.DrawSprite(outline.color, gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");
        }

        // Configura uma barra interna com a cor fornecida
        private RectTransform SetupBar(Color barColor)
        {
            RectTransform bar = UtilsClass.DrawSprite(barColor, gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
            bar.anchorMin = new Vector2(0, 0);
            bar.anchorMax = new Vector2(0, 1f);
            bar.pivot = new Vector2(0, .5f);
            return bar;
        }

        // Set the sizes of the inner bars
        public void SetSizes(float[] sizeArr)
        {
            if (sizeArr.Length != barArr.Length)
            {
                throw new System.Exception("Length doesn't match!");// Throw an exception if the lengths don't match
            }
            Vector2 pos = Vector2.zero;

            // Para cada barra interna
            for (int i = 0; i < sizeArr.Length; i++)
            {
                float scaledSize = sizeArr[i] * size.x; // Dimensiona o tamanho com base na matriz de tamanhos fornecida
                barArr[i].anchoredPosition = pos;// Define a posição ancorada da barra interna
                barArr[i].sizeDelta = new Vector2(scaledSize, 0f);// Define o tamanho da barra interna
                pos.x += scaledSize;// Incrementa a posição x para a próxima barra interna
            }
        }

        // Obtém o tamanho das barras múltiplas
        public Vector2 GetSize()
        {
            return size;
        }

        // Destroi o GameObject das barras múltiplas
        public void DestroySelf()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
