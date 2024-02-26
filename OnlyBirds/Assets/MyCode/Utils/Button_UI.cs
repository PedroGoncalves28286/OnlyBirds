using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{

    /*
     * 
        Classe responsável por criar botões na interface do utlizador.
     * */
    public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        // "actions" para eventos de clique e interações do mouse.

        public Action ClickFunc = null;
        public Action MouseRightClickFunc = null;
        public Action MouseMiddleClickFunc = null;
        public Action MouseDownOnceFunc = null;
        public Action MouseUpFunc = null;
        public Action MouseOverOnceTooltipFunc = null;
        public Action MouseOutOnceTooltipFunc = null;
        public Action MouseOverOnceFunc = null;
        public Action MouseOutOnceFunc = null;
        public Action MouseOverFunc = null;
        public Action MouseOverPerSecFunc = null; //Aciona a cada segundo do "MouseOver"
        public Action MouseUpdate = null;
        public Action<PointerEventData> OnPointerClickFunc;

        // Enumeração para o comportamento de hover.
        public enum HoverBehaviour
        {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;

        // Variáveis para comportamento de hover com mudança de cor ou imagem.

        public Color hoverBehaviour_Color_Enter, hoverBehaviour_Color_Exit;
        public Image hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move = false;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector2 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        private bool mouseOver;
        private float mouseOverPerSecFuncTimer;

        // "Action" internos para eventos de mouse.
        private Action internalOnPointerEnterFunc, internalOnPointerExitFunc, internalOnPointerClickFunc;

        // Configurações opcionais para Sound Manager e Cursor Manager.
#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif

        // Método chamado quando o ponteiro do mouse entra no objeto.
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (internalOnPointerEnterFunc != null) internalOnPointerEnterFunc();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
            if (MouseOverOnceFunc != null) MouseOverOnceFunc();
            if (MouseOverOnceTooltipFunc != null) MouseOverOnceTooltipFunc();
            mouseOver = true;
            mouseOverPerSecFuncTimer = 0f;
        }

        // Método chamado quando o ponteiro do mouse sai do objeto.
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (internalOnPointerExitFunc != null) internalOnPointerExitFunc();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
            if (MouseOutOnceFunc != null) MouseOutOnceFunc();
            if (MouseOutOnceTooltipFunc != null) MouseOutOnceTooltipFunc();
            mouseOver = false;
        }

        // Método chamado quando o ponteiro do mouse clica no objeto.
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (internalOnPointerClickFunc != null) internalOnPointerClickFunc();
            if (OnPointerClickFunc != null) OnPointerClickFunc(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (triggerMouseOutFuncOnClick)
                {
                    OnPointerExit(eventData);
                }
                if (ClickFunc != null) ClickFunc();
            }
            if (eventData.button == PointerEventData.InputButton.Right)
                if (MouseRightClickFunc != null) MouseRightClickFunc();
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) MouseMiddleClickFunc();
        }
        public void Manual_OnPointerExit()
        {
            OnPointerExit(null);
        }
        public bool IsMouseOver()
        {
            return mouseOver;
        }

        // Método chamado quando o ponteiro do mouse é pressionado sobre o objeto.
        public void OnPointerDown(PointerEventData eventData)
        {
            if (MouseDownOnceFunc != null) MouseDownOnceFunc();
        }

        // Método chamado quando o ponteiro do mouse é liberado sobre o objeto.
        public void OnPointerUp(PointerEventData eventData)
        {
            if (MouseUpFunc != null) MouseUpFunc();
        }
        // Método chamado a cada frame.
        void Update()
        {
            if (mouseOver)
            {
                if (MouseOverFunc != null) MouseOverFunc();
                mouseOverPerSecFuncTimer -= Time.unscaledDeltaTime;
                if (mouseOverPerSecFuncTimer <= 0)
                {
                    mouseOverPerSecFuncTimer += 1f;
                    if (MouseOverPerSecFunc != null) MouseOverPerSecFunc();
                }
            }
            if (MouseUpdate != null) MouseUpdate();

        }

        // Método chamado na inicialização.
        void Awake()
        {
            posExit = transform.localPosition;
            posEnter = (Vector2)transform.localPosition + hoverBehaviour_Move_Amount;
            SetHoverBehaviourType(hoverBehaviourType);

            // Configurações opcionais para Sound Manager.
#if SOUND_MANAGER
            // Sound Manager
            internalOnPointerEnterFunc += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
            internalOnPointerClickFunc += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
#endif
            // Configurações opcionais para Cursor Manager.
#if CURSOR_MANAGER
            // Cursor Manager
            internalOnPointerEnterFunc += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
            internalOnPointerExitFunc += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
#endif
        }

        // Método para definir o comportamento de hover.
        public void SetHoverBehaviourType(HoverBehaviour hoverBehaviourType)
        {
            this.hoverBehaviourType = hoverBehaviourType;
            switch (hoverBehaviourType)
            {
                case HoverBehaviour.Change_Color:
                    hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.color = hoverBehaviour_Color_Enter; };
                    hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.color = hoverBehaviour_Color_Exit; };
                    break;
                case HoverBehaviour.Change_Image:
                    hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Enter; };
                    hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Exit; };
                    break;
                case HoverBehaviour.Change_SetActive:
                    hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.gameObject.SetActive(true); };
                    hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.gameObject.SetActive(false); };
                    break;
            }
        }









        /*
         * 
          Classe para interceptar temporariamente uma ação de botão * Útil para o Tutorial desabilitar botões específicos
         * */
        public class InterceptActionHandler
        {

            private Action removeInterceptFunc;

            // Construtor da classe, recebe uma função para remover a interceptação.
            public InterceptActionHandler(Action removeInterceptFunc)
            {
                this.removeInterceptFunc = removeInterceptFunc;
            }

            // Método para remover a interceptação.
            public void RemoveIntercept()
            {
                removeInterceptFunc();
            }
        }

        // Método para interceptar a ação de clique.
        public InterceptActionHandler InterceptActionClick(Func<bool> testPassthroughFunc)
        {
            return InterceptAction("ClickFunc", testPassthroughFunc);
        }

        // Método genérico para interceptar uma ação com base no nome do campo.
        public InterceptActionHandler InterceptAction(string fieldName, Func<bool> testPassthroughFunc)
        {
            return InterceptAction(GetType().GetField(fieldName), testPassthroughFunc);
        }
        // Método genérico para interceptar uma ação com base em uma informação de campo.
        public InterceptActionHandler InterceptAction(System.Reflection.FieldInfo fieldInfo, Func<bool> testPassthroughFunc)
        {
            Action backFunc = fieldInfo.GetValue(this) as Action;
            // Criar um manipulador de interceptação.
            InterceptActionHandler interceptActionHandler = new InterceptActionHandler(() => fieldInfo.SetValue(this, backFunc));
            // Substituir a função de ação original por uma nova que passa pelo teste de interceptação.
            fieldInfo.SetValue(this, (Action)delegate ()
            {
                if (testPassthroughFunc())
                {
                    // Ação passa pelo teste, remover interceptação e executar a ação original.
                    interceptActionHandler.RemoveIntercept();
                    backFunc();
                }
            });

            return interceptActionHandler;
        }
    }
}