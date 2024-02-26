using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     * * Classe que exibe texto com ícones entre o texto.
     * */
    public class UI_TextComplex
    {

        // Método estático para obter o Transform do Canvas
        private static Transform GetCanvasTransform()
        {
            return UtilsClass.GetCanvasTransform();
        }

        // Estrutura para representar um ícone
        public struct Icon
        {
            public Sprite sprite;// Sprite do ícone
            public Vector2 size; // Tamanho do ícone
            public Color color;// Cor do ícone
            public Icon(Sprite sprite, Vector2 size, Color? color = null)
            {
                this.sprite = sprite;
                this.size = size;
                if (color == null)
                {
                    this.color = Color.white; // Se a cor não for especificada, usa-se branco como padrão
                }
                else
                {
                    this.color = (Color)color;
                }
            }
        }
        // Variáveis da classe

        public GameObject gameObject;
        private Transform transform;
        private RectTransform rectTransform;


        // Construtor da classe UI_TextComplex
        public UI_TextComplex(Transform parent, Vector2 anchoredPosition, int fontSize, char iconChar, string text, Icon[] iconArr, Font font)
        {
            SetupParent(parent, anchoredPosition);
            string tmp = text;
            float textPosition = 0f;
            while (tmp.IndexOf(iconChar) != -1)
            {

                // Ainda tem mais espaço após o número do ícone
                string untilTmp = tmp.Substring(0, tmp.IndexOf(iconChar));
                string iconNumber = tmp.Substring(tmp.IndexOf(iconChar) + 1);
                int indexOfSpaceAfterIconNumber = iconNumber.IndexOf(" ");
                if (indexOfSpaceAfterIconNumber != -1)
                {
                    //Ainda tem mais espaço após IconNumber
                    iconNumber = iconNumber.Substring(0, indexOfSpaceAfterIconNumber);
                }
                else
                {
                    //Não existe mais espaço depois do iconNumber
                }
                tmp = tmp.Substring(tmp.IndexOf(iconChar + iconNumber) + (iconChar + iconNumber).Length);
                if (untilTmp.Trim() != "")
                {
                    Text uiText = UtilsClass.DrawTextUI(untilTmp, transform, new Vector2(textPosition, 0), fontSize, font);
                    textPosition += uiText.preferredWidth;
                }
                // Icon de desenho
                int iconIndex = UtilsClass.Parse_Int(iconNumber, 0);
                Icon icon = iconArr[iconIndex];
                UtilsClass.DrawSprite(icon.sprite, transform, new Vector2(textPosition + icon.size.x / 2f, 0), icon.size);
                textPosition += icon.size.x;
            }
            if (tmp.Trim() != "")
            {
                UtilsClass.DrawTextUI(tmp, transform, new Vector2(textPosition, 0), fontSize, font);
            }
        }
        // Configura o master e a posição ancorada do objeto de texto
        private void SetupParent(Transform parent, Vector2 anchoredPosition)
        {
            gameObject = new GameObject("UI_TextComplex", typeof(RectTransform));
            transform = gameObject.transform;
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = new Vector2(0, 0);
            rectTransform.anchorMin = new Vector2(0, .5f);
            rectTransform.anchorMax = new Vector2(0, .5f);
            rectTransform.pivot = new Vector2(0, .5f);
            rectTransform.anchoredPosition = anchoredPosition;
        }
        // Define a cor do texto
        public void SetTextColor(Color color)
        {
            foreach (Transform trans in transform)
            {
                Text text = trans.GetComponent<Text>();
                if (text != null)
                {
                    text.color = color;
                }
            }
        }
        // Obtém a largura total do texto
        public float GetTotalWidth()
        {
            float textPosition = 0f;
            foreach (Transform trans in transform)
            {
                Text text = trans.GetComponent<Text>();
                if (text != null)
                {
                    textPosition += text.preferredWidth;
                }
                Image image = trans.GetComponent<Image>();
                if (image != null)
                {
                    textPosition += image.GetComponent<RectTransform>().sizeDelta.x;
                }
            }
            return textPosition;
        }

        // Obtém a altura total do texto
        public float GetTotalHeight()
        {
            foreach (Transform trans in transform)
            {
                Text text = trans.GetComponent<Text>();
                if (text != null)
                {
                    return text.preferredHeight;
                }
            }
            return 0f;
        }
        // Adiciona um contorno ao texto
        public void AddTextOutline(Color color, float size)
        {
            foreach (Transform textComplexTrans in transform)
            {
                if (textComplexTrans.GetComponent<Text>() != null)
                {
                    Outline outline = textComplexTrans.gameObject.AddComponent<Outline>();
                    outline.effectColor = color;
                    outline.effectDistance = new Vector2(size, size);
                }
            }
        }
        // Define a âncora do objeto de texto como o meio
        public void SetAnchorMiddle()
        {
            rectTransform.anchorMin = new Vector2(.5f, .5f);
            rectTransform.anchorMax = new Vector2(.5f, .5f);
        }

        // Centraliza o objeto de texto em uma posição específica
        public void CenterOnPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position + new Vector2(-GetTotalWidth() / 2f, 0);
        }


        // Destroi o objeto de texto
        public void DestroySelf()
        {
            Object.Destroy(gameObject);
        }
    }
}
