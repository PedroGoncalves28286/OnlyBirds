using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeMonkey.Utils
{

    /*
     * Responsável por manipular os eventos dos botões em um Collider 2D no mundo do jogo.
     * */
    public class Button_Sprite : MonoBehaviour
    {
        // Variável estática para obter a câmera do mundo.
        private static Func<Camera> GetWorldCamera;

        // Método estático para definir a função para obter a câmera do mundo.
        public static void SetGetWorldCamera(Func<Camera> GetWorldCamera)
        {
            Button_Sprite.GetWorldCamera = GetWorldCamera;
        }

        // Delegados para eventos de clique e interações do mouse.
        public Action ClickFunc = null;
        public Action MouseRightDownOnceFunc = null;
        public Action MouseRightDownFunc = null;
        public Action MouseRightUpFunc = null;
        public Action MouseDownOnceFunc = null;
        public Action MouseUpOnceFunc = null;
        public Action MouseOverOnceFunc = null;
        public Action MouseOutOnceFunc = null;
        public Action MouseOverOnceTooltipFunc = null;
        public Action MouseOutOnceTooltipFunc = null;

        // Variáveis para arrastar com o botão direito do mouse.

        private bool draggingMouseRight;
        private Vector3 mouseRightDragStart;
        public Action<Vector3, Vector3> MouseRightDragFunc = null;
        public Action<Vector3, Vector3> MouseRightDragUpdateFunc = null;
        public bool triggerMouseRightDragOnEnter = false;

        // Enumeração para o comportamento de "HoverBehavior"
        public enum HoverBehaviour
        {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;

        // Variáveis para comportamento de "hoverBehaviour" com mudança de cor ou imagem.

        public Color hoverBehaviour_Color_Enter = new Color(1, 1, 1, 1), hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        public SpriteRenderer hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move = false;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector3 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        public bool clickThroughUI = false;


        // Delegados internos para eventos de "mouse"-rato

        private Action internalOnMouseDownFunc, internalOnMouseEnterFunc, internalOnMouseExitFunc;

        // Métodos e variáveis condicionais para Sound Manager e Cursor Manager.

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif

        // Método para definir o comportamento de hover com mudança de cor.
        public void SetHoverBehaviourChangeColor(Color colorOver, Color colorOut)
        {
            hoverBehaviourType = HoverBehaviour.Change_Color;
            hoverBehaviour_Color_Enter = colorOver;
            hoverBehaviour_Color_Exit = colorOut;
            if (hoverBehaviour_Image == null) hoverBehaviour_Image = transform.GetComponent<SpriteRenderer>();
            hoverBehaviour_Image.color = hoverBehaviour_Color_Exit;
            SetupHoverBehaviour();
        }

        // Método chamado quando o mouse é clicado sobre o objeto.
        void OnMouseDown()
        {
            if (!clickThroughUI && IsPointerOverUI()) return; // Over UI!

            if (internalOnMouseDownFunc != null) internalOnMouseDownFunc();
            if (ClickFunc != null) ClickFunc();
            if (triggerMouseOutFuncOnClick) OnMouseExit();
        }
        public void Manual_OnMouseExit()
        {
            OnMouseExit();
        }

        // Método chamado quando o mouse é liberado.
        void OnMouseUp()
        {
            if (MouseUpOnceFunc != null) MouseUpOnceFunc();
        }

        // Método chamado quando o mouse entra no objeto.
        void OnMouseEnter()
        {
            if (!clickThroughUI && IsPointerOverUI()) return; // Over UI!

            if (internalOnMouseEnterFunc != null) internalOnMouseEnterFunc();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
            if (MouseOverOnceFunc != null) MouseOverOnceFunc();
            if (MouseOverOnceTooltipFunc != null) MouseOverOnceTooltipFunc();
        }

        // Método chamado quando o mouse sai do objeto.
        void OnMouseExit()
        {
            if (internalOnMouseExitFunc != null) internalOnMouseExitFunc();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
            if (MouseOutOnceFunc != null) MouseOutOnceFunc();
            if (MouseOutOnceTooltipFunc != null) MouseOutOnceTooltipFunc();
        }

        // Método chamado quando o mouse está sobre o objeto.
        void OnMouseOver()
        {
            if (!clickThroughUI && IsPointerOverUI()) return; // Over UI!

            if (Input.GetMouseButton(1))
            {
                if (MouseRightDownFunc != null) MouseRightDownFunc();
                if (!draggingMouseRight && triggerMouseRightDragOnEnter)
                {
                    draggingMouseRight = true;
                    mouseRightDragStart = GetWorldPositionFromUI();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                draggingMouseRight = true;
                mouseRightDragStart = GetWorldPositionFromUI();
                if (MouseRightDownOnceFunc != null) MouseRightDownOnceFunc();
            }
        }

        // Método chamado a cada frame.
        void Update()
        {
            if (draggingMouseRight)
            {
                if (MouseRightDragUpdateFunc != null) MouseRightDragUpdateFunc(mouseRightDragStart, GetWorldPositionFromUI());
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (draggingMouseRight)
                {
                    draggingMouseRight = false;
                    if (MouseRightDragFunc != null) MouseRightDragFunc(mouseRightDragStart, GetWorldPositionFromUI());
                }
                if (MouseRightUpFunc != null) MouseRightUpFunc();
            }
        }

        // Método chamado na inicialização.
        void Awake()
        {
            if (GetWorldCamera == null) SetGetWorldCamera(() => Camera.main); // //Definir Câmara "World Camera"
            posExit = transform.localPosition;
            posEnter = transform.localPosition + (Vector3)hoverBehaviour_Move_Amount;
            SetupHoverBehaviour();

            // Configurações para Sound Manager.

#if SOUND_MANAGER
            // Sound Manager
            internalOnMouseDownFunc += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
            internalOnMouseEnterFunc += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
#endif
            // Configurações para Cursor Manager.

#if CURSOR_MANAGER
            // Cursor Manager
            internalOnMouseExitFunc += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
            internalOnMouseEnterFunc += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
#endif
        }

        // Método para configurar o comportamento deetupHoverBehavio.
        private void SetupHoverBehaviour()
        {
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


        // Método para obter a posição do mundo a partir do ponteiro do mouse.
        private static Vector3 GetWorldPositionFromUI()
        {
            Vector3 worldPosition = GetWorldCamera().ScreenToWorldPoint(Input.mousePosition);
            return worldPosition;
        }

        // Método para verificar se o ponteiro do mouse está sobre uma interface de utilizador.
        private static bool IsPointerOverUI()
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
    }

}