using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     *  Esta classe representa uma barra na UI com um plano de fundo e uma barra escaláveis.
     * */
    public class UI_Bar
    {
        // Variáveis para os elementos da barra na UI

        public GameObject gameObject;
        private RectTransform rectTransform;
        private RectTransform background;
        private RectTransform bar;
        private Vector2 size;

        /* 
         * Classe para definir a aparência do contorno da barra.
         * */
        public class Outline
        {
            public float size = 1f;// O tamanho do contorno.
            public Color color = Color.black;// A cor do contorno.

            // Construtor que inicializa o tamanho e a cor do contorno
            public Outline(float size, Color color)
            {
                this.size = size;
                this.color = color;
            }
        }

        // Construtor da classe UI_Bar que cria uma barra na UI com uma cor de barra específica e um tamanho relativo.
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color barColor, float sizeRatio)
        {
            SetupParent(parent, anchoredPosition, size);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }

        // Construtor da classe UI_Bar que cria uma barra na UI com uma cor de barra específica, um tamanho relativo e um contorno.
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color barColor, float sizeRatio, Outline outline)
        {
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }

        // Construtor da classe UI_Bar que cria uma barra na UI com uma cor de barra específica, um tamanho relativo e um contorno.
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color backgroundColor, Color barColor, float sizeRatio)
        {
            SetupParent(parent, anchoredPosition, size);
            SetupBackground(backgroundColor);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }

        // Construtor da classe UI_Bar que cria uma barra na UI com uma cor de plano de fundo e uma cor de barra específicas e um tamanho relativo.
        public UI_Bar(Transform parent, Vector2 anchoredPosition, Vector2 size, Color backgroundColor, Color barColor, float sizeRatio, Outline outline)
        {
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            SetupBackground(backgroundColor);
            SetupBar(barColor);
            SetSize(sizeRatio);
        }
        // Método privado para configurar o objeto pai da barra na UI.
        private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size)
        {
            this.size = size;
            gameObject = new GameObject("UI_Bar", typeof(RectTransform));
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = size;
            rectTransform.anchorMin = new Vector2(0, .5f);
            rectTransform.anchorMax = new Vector2(0, .5f);
            rectTransform.pivot = new Vector2(0, .5f);
            rectTransform.anchoredPosition = anchoredPosition;
        }

        // Método privado para configurar o contorno da barra na UI.
        private RectTransform SetupOutline(Outline outline, Vector2 size)
        {
            return UtilsClass.DrawSprite(outline.color, gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");
        }
        private void SetupBackground(Color backgroundColor)
        {
            background = UtilsClass.DrawSprite(backgroundColor, gameObject.transform, Vector2.zero, Vector2.zero, "Background");
            background.anchorMin = new Vector2(0, 0);
            background.anchorMax = new Vector2(1, 1);
        }

        // Método privado para configurar o contorno da barra na UI.
        private void SetupBar(Color barColor)
        {
            bar = UtilsClass.DrawSprite(barColor, gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
            bar.anchorMin = new Vector2(0, 0);
            bar.anchorMax = new Vector2(0, 1f);
            bar.pivot = new Vector2(0, .5f);
        }

        // Método público para definir o tamanho da barra na UI com base em uma proporção.
        public void SetSize(float sizeRatio)
        {
            bar.sizeDelta = new Vector2(sizeRatio * size.x, 0);
        }

        // Método público para definir a cor da barra na UI.
        public void SetColor(Color color)
        {
            bar.GetComponent<Image>().color = color;
        }

        // Método público para definir se a barra na UI está ativa ou não.
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        // Método público para adicionar um contorno à barra na UI.
        public void AddOutline(Outline outline)
        {
            RectTransform outlineRectTransform = SetupOutline(outline, size);
            outlineRectTransform.transform.SetAsFirstSibling();
        }

        // Método público para definir se a barra na UI é um alvo de interação com o cursor do mouse.
        public void SetRaycastTarget(bool set)
        {
            foreach (Transform trans in gameObject.transform)
            {
                if (trans.GetComponent<Image>() != null)
                {
                    trans.GetComponent<Image>().raycastTarget = set;
                }
            }
        }
        // Método público para destruir a barra na UI.
        public void DestroySelf()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        // Método público para adicionar um botão à barra na UI.
        public Button_UI AddButton()
        {
            return AddButton(null, null, null);
        }

        // Método público para adicionar um botão à barra na UI com funções de clique, mouse sobre e mouse fora especificadas.
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
#if TOOLTIP_UI
        public void AddTooltip(Func<string> getTooltip) {
            Tooltip_UI.AddTip(this, getTooltip);
        }
#endif

    }


}
