using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     * Sprite na interface do utilizador
     * */
    public class UI_Sprite
    {

        // Método estático para obter o Transform do Canvas
        private static Transform GetCanvasTransform()
        {
            return UtilsClass.GetCanvasTransform();
        }

        // Cria um botão de depuração com uma cor específica
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc)
        {
            return CreateDebugButton(anchoredPosition, size, ClickFunc, Color.green);
        }

        // Cria um botão de depuração com um texto e uma cor específicos
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color)
        {
            UI_Sprite uiSprite = new UI_Sprite(GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, null, null);
            return uiSprite;
        }

        // Cria um botão de depuração com um texto e uma cor específicos e um preenchimento
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc)
        {
            return CreateDebugButton(anchoredPosition, text, ClickFunc, Color.green);
        }

        // Cria um botão de depuração com um texto e uma cor específicos
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc, Color color)
        {
            return CreateDebugButton(anchoredPosition, text, ClickFunc, color, new Vector2(30, 20));
        }

        // Cria um botão de depuração com um texto e uma cor específicos e um preenchimento
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc, Color color, Vector2 padding)
        {
            UI_TextComplex uiTextComplex;
            UI_Sprite uiSprite = CreateDebugButton(anchoredPosition, Vector2.zero, ClickFunc, color, text, out uiTextComplex);
            uiSprite.SetSize(new Vector2(uiTextComplex.GetTotalWidth(), uiTextComplex.GetTotalHeight()) + padding);
            return uiSprite;
        }

        // Cria um botão de depuração com um texto e uma cor específicos
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text)
        {
            UI_TextComplex uiTextComplex;
            return CreateDebugButton(anchoredPosition, size, ClickFunc, color, text, out uiTextComplex);
        }

        // Cria um botão de depuração com um texto, uma cor específicos e um objeto UI_TextComplex associado
        public static UI_Sprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text, out UI_TextComplex uiTextComplex)
        {
            if (color.r >= 1f) color.r = .9f;
            if (color.g >= 1f) color.g = .9f;
            if (color.b >= 1f) color.b = .9f;
            Color colorOver = color * 1.1f; // button over color lighter
            UI_Sprite uiSprite = new UI_Sprite(GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, () => uiSprite.SetColor(colorOver), () => uiSprite.SetColor(color));
            uiTextComplex = new UI_TextComplex(uiSprite.gameObject.transform, Vector2.zero, 12, '#', text, null, null);
            uiTextComplex.SetTextColor(Color.black);
            uiTextComplex.SetAnchorMiddle();
            uiTextComplex.CenterOnPosition(Vector2.zero);
            return uiSprite;
        }

        // Variáveis da classe

        public GameObject gameObject;
        public Image image;
        public RectTransform rectTransform;

        // Construtor da classe UI_Sprite
        public UI_Sprite(Transform parent, Sprite sprite, Vector2 anchoredPosition, Vector2 size, Color color)
        {
            rectTransform = UtilsClass.DrawSprite(sprite, parent, anchoredPosition, size, "UI_Sprite");
            gameObject = rectTransform.gameObject;
            image = gameObject.GetComponent<Image>();
            image.color = color;
        }
        // Define a cor do sprite
        public void SetColor(Color color)
        {
            image.color = color;
        }

        // Define o sprite
        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        // Define o tamanho do sprite
        public void SetSize(Vector2 size)
        {
            rectTransform.sizeDelta = size;
        }
        // Define a posição ancorada do sprite
        public void SetAnchoredPosition(Vector2 anchoredPosition)
        {
            rectTransform.anchoredPosition = anchoredPosition;
        }

        // Adiciona um botão ao sprite
        public Button_UI AddButton(Action ClickFunc, Action MouseOverOnceFunc, Action MouseOutOnceFunc)
        {
            Button_UI buttonUI = gameObject.AddComponent<Button_UI>();
            if (ClickFunc != null)
                buttonUI.ClickFunc = ClickFunc;
            if (MouseOverOnceFunc != null)
                buttonUI.MouseOverOnceFunc = MouseOverOnceFunc;
            if (MouseOutOnceFunc != null)
                buttonUI.MouseOutOnceFunc = MouseOutOnceFunc;
            return buttonUI;
        }
        // Destroi o sprite
        public void DestroySelf()
        {
            UnityEngine.Object.Destroy(gameObject);
        }

    }
}