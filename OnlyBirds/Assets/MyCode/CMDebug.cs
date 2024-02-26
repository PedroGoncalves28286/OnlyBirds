using CodeMonkey.Utils;
using System;
using UnityEngine;

namespace CodeMonkey
{

    /*
         Classe de debug com várias funções auxiliares para criar rapidamente botões, texto, etc.
     * */
    public static class CMDebug
    {

        // Cria um botão no mundo
        public static World_Sprite Button(Transform parent, Vector3 localPosition, string text, System.Action ClickFunc, int fontSize = 30, float paddingX = 5, float paddingY = 5)
        {
            return World_Sprite.CreateDebugButton(parent, localPosition, text, ClickFunc, fontSize, paddingX, paddingY);
        }

        // Cria um botão na UI
        public static UI_Sprite ButtonUI(Vector2 anchoredPosition, string text, Action ClickFunc)
        {
            return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc);
        }


        // O texto do mundo aparece na posição do mouse
        public static void TextPopupMouse(string text)
        {
            UtilsClass.CreateWorldTextPopup(text, UtilsClass.GetMouseWorldPositionZeroZ());
        }

        // Cria um pop-up de texto na posição mundial
        public static void TextPopup(string text, Vector3 position)
        {
            UtilsClass.CreateWorldTextPopup(text, position);
        }

        //Atualizador de texto no mundo, (parent == null) = posição no Mundo
        public static FunctionUpdater TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            return UtilsClass.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }

        //Atualizador de texto na UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 anchoredPosition)
        {
            return UtilsClass.CreateUITextUpdater(GetTextFunc, anchoredPosition);
        }

        //Atualizador de texto sempre seguindo o mouse
        public static void MouseTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset)
        {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() =>
            {
                gameObject.transform.position = UtilsClass.GetMouseWorldPositionZeroZ() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }

    }

}