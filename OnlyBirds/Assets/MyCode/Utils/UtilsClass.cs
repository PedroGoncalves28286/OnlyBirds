using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     * Várias funções de utilitários -"utils"
     * */
    public static class UtilsClass
    { // Define uma classe estática para o utililizador 


        // Vetores constantes para reutilização

        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0, -1);

        // Ordenação padrão para objetos renderizados

        private const int sortingOrderDefault = 5000;

        //Obter a ordem de ordenação para definir SpriteRenderer SortingOrder, posição superior = menor SortingOrder
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        //Obter Main Canvas Transform
        private static Transform cachedCanvasTransform;// Cache para transform do Canvas
        public static Transform GetCanvasTransform()
        {
            if (cachedCanvasTransform == null)
            { // Se o cache estiver vazio
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>(); // Encontra o Canvas na cena
                if (canvas != null)
                {
                    cachedCanvasTransform = canvas.transform;// Armazena o transform do Canvas no cache
                }
            }
            return cachedCanvasTransform;
        }

        //Obter a fonte Unity padrão, usada em objetos de texto se nenhuma fonte fôr fornecida
        public static Font GetDefaultFont()
        {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }


        //Criação um Sprite-personagem no Mundo, sem ligação -master
        public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale, int sortingOrder, Color color)
        {
            return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }

        //Criação um Sprite-personagem no Mundo
        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        //Criação um Sprite no Mundo com Button_Sprite, sem "Master"
        public static Button_Sprite CreateWorldSpriteButton(string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
        {
            return CreateWorldSpriteButton(null, name, sprite, localPosition, localScale, sortingOrder, color);
        }

        //Criação de um Sprite no Mundo com Button_Sprite
        public static Button_Sprite CreateWorldSpriteButton(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = CreateWorldSprite(parent, name, sprite, localPosition, localScale, sortingOrder, color);
            gameObject.AddComponent<BoxCollider2D>();
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            return buttonSprite;
        }
        //Criação de uma Malha de Texto-"text mesh" no Mundo e actualizando-a constantemente
        public static FunctionUpdater CreateWorldTextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            TextMesh textMesh = CreateWorldText(GetTextFunc(), parent, localPosition);
            return FunctionUpdater.Create(() =>
            {
                textMesh.text = GetTextFunc();
                return false;
            }, "WorldTextUpdater");
        }

        //Criação de um Texto no Mundo
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        //Criação de um Texto no Mundo
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        //Criação de um Popup de Texto no Mundo, sem Master
        public static void CreateWorldTextPopup(string text, Vector3 localPosition)
        {
            CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, 20), 1f);
        }

        //Criação de um Popup de Texto no Mundo
        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime)
        {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate ()
            {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (popupTime <= 0f)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                }
                else
                {
                    return false;
                }
            }, "WorldTextPopup");
        }

        //Criação de  Actualizador de Texto na UI
        public static FunctionUpdater CreateUITextUpdater(Func<string> GetTextFunc, Vector2 anchoredPosition)
        {
            Text text = DrawTextUI(GetTextFunc(), anchoredPosition, 20, GetDefaultFont());
            return FunctionUpdater.Create(() =>
            {
                text.text = GetTextFunc();
                return false;
            }, "UITextUpdater");
        }



        //Desenhar um UI Sprite
        public static RectTransform DrawSprite(Color color, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            RectTransform rectTransform = DrawSprite(null, color, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            RectTransform rectTransform = DrawSprite(sprite, Color.white, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Color color, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            // Setup icon
            if (name == null || name == "") name = "Sprite";
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.SetParent(parent, false);
            goRectTransform.sizeDelta = size;
            goRectTransform.anchoredPosition = pos;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;

            return goRectTransform;
        }

        // Desenha um texto na UI
        public static Text DrawTextUI(string textString, Vector2 anchoredPosition, int fontSize, Font font)
        {
            return DrawTextUI(textString, GetCanvasTransform(), anchoredPosition, fontSize, font);
        }

        // Desenha um texto na UI com um Transform específico
        public static Text DrawTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize, Font font)
        {
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(parent, false);
            Transform textGoTrans = textGo.transform;
            textGoTrans.SetParent(parent, false);
            textGoTrans.localPosition = Vector3zero;
            textGoTrans.localScale = Vector3one;

            RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
            textGoRectTransform.sizeDelta = new Vector2(0, 0);
            textGoRectTransform.anchoredPosition = anchoredPosition;

            Text text = textGo.GetComponent<Text>();
            text.text = textString;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            return text;
        }


        // Converte uma string em float, retornando um valor padrão se a conversão falhar
        public static float Parse_Float(string txt, float _default)
        {
            float f;
            if (!float.TryParse(txt, out f))
            {
                f = _default;
            }
            return f;
        }

        // Parse um int, devolver predefinição se falhar
        public static int Parse_Int(string txt, int _default)
        {
            int i;
            if (!int.TryParse(txt, out i))
            {
                i = _default;
            }
            return i;
        }
        public static int Parse_Int(string txt)
        {
            return Parse_Int(txt, -1);
        }




        //Posição do Rato no Mundo com Z = 0f
        public static Vector3 GetMouseWorldPositionZeroZ()
        {
            Vector3 vec = GetMouseWorldPosition(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPosition()
        {
            return GetMouseWorldPosition(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPosition(Camera worldCamera)
        {
            return GetMouseWorldPosition(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }


        //Se passar o rato sobre um elemento da interface do utilizador? Usado para ignorar cliques do "world" através da interface do utilizador
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }


        // Desenha uma linha de debug para representar um projétil em movimento
        public static void DebugProjectile(Vector3 from, Vector3 to, float speed, float projectileSize)
        {
            Vector3 dir = (to - from).normalized;
            Vector3 pos = from;
            FunctionUpdater.Create(() =>
            {
                Debug.DrawLine(pos, pos + dir * projectileSize);
                float distanceBefore = Vector3.Distance(pos, to);
                pos += dir * speed * Time.deltaTime;
                float distanceAfter = Vector3.Distance(pos, to);
                if (distanceBefore < distanceAfter)
                {
                    return true;
                }
                return false;
            });
        }


        // Converte um valor decimal em hexadecimal
        public static string Dec_to_Hex(int value)
        {
            //Retorna 00-FF
            return value.ToString("X2");
        }
        public static int Hex_to_Dec(string hex)
        {
            //Retorna 0-255
            return Convert.ToInt32(hex, 16);
        }
        public static string Dec01_to_Hex(float value)
        {
            //Retorna uma string hexadecimal baseada em um número entre 0->1
            return Dec_to_Hex((int)Mathf.Round(value * 255f));
        }
        public static float Hex_to_Dec01(string hex)
        {
            //Retorna um float entre 0->1
            return Hex_to_Dec(hex) / 255f;
        }

        // Converte uma cor em uma string hexadecimal
        public static string GetStringFromColor(Color color)
        {
            string red = Dec01_to_Hex(color.r);
            string green = Dec01_to_Hex(color.g);
            string blue = Dec01_to_Hex(color.b);
            //string alpha = Dec01_to_Hex(color.a);
            return red + green + blue;//+alpha;
        }

        // Converte uma cor em uma string hexadecimal
        public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha)
        {
            red = Dec01_to_Hex(color.r);
            green = Dec01_to_Hex(color.g);
            blue = Dec01_to_Hex(color.b);
            alpha = Dec01_to_Hex(color.a);
        }
        public static string GetStringFromColor(float r, float g, float b)
        {//, float a = 1f) {
            string red = Dec01_to_Hex(r);
            string green = Dec01_to_Hex(g);
            string blue = Dec01_to_Hex(b);
            //string alpha = Dec01_to_Hex(a);
            return red + green + blue;//+alpha;
        }
        public static Color GetColorFromString(string color)
        {
            float red = Hex_to_Dec01(color.Substring(0, 2));
            float green = Hex_to_Dec01(color.Substring(2, 2));
            float blue = Hex_to_Dec01(color.Substring(4, 2));
            return new Color(red, green, blue, 1f);
        }
        public static Color GetColorFromString_Alpha(string color)
        {
            float red = Hex_to_Dec01(color.Substring(0, 2));
            float green = Hex_to_Dec01(color.Substring(2, 2));
            float blue = Hex_to_Dec01(color.Substring(4, 2));
            float alpha = Hex_to_Dec01(color.Substring(6, 2));
            return new Color(red, green, blue, alpha);
        }

    }

}