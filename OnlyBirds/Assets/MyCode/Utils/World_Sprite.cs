using UnityEngine;

namespace CodeMonkey.Utils
{

    /*
     * Sprite no mundo
     * */
    public class World_Sprite
    {

        // Ordem de classificação padrão

        private const int sortingOrderDefault = 5000;

        // Variáveis para armazenar referências ao GameObject, ao Transform e ao SpriteRenderer do sprite no mundo

        public GameObject gameObject;
        public Transform transform;
        private SpriteRenderer spriteRenderer;

        // Método estático para criar um botão de depuração com base em uma posição no mundo e uma função de clique
        public static World_Sprite CreateDebugButton(Vector3 position, System.Action ClickFunc)
        {
            // Cria um World_Sprite com uma cor verde e adiciona um botão com a função de clique
            World_Sprite worldSprite = new World_Sprite(null, position, new Vector3(10, 10), Assets.i.s_White, Color.green, sortingOrderDefault);
            worldSprite.AddButton(ClickFunc, null, null);
            return worldSprite;
        }

        // Método estático para criar um botão de depuração com base em um transformador pai, posição local e função de clique
        public static World_Sprite CreateDebugButton(Transform parent, Vector3 localPosition, System.Action ClickFunc)
        {

            // Cria um World_Sprite com uma cor verde e adiciona um botão com a função de clique
            World_Sprite worldSprite = new World_Sprite(parent, localPosition, new Vector3(10, 10), Assets.i.s_White, Color.green, sortingOrderDefault);
            worldSprite.AddButton(ClickFunc, null, null);
            return worldSprite;
        }

        // Método estático para criar um botão de depuração com base em um transformador pai, posição local, texto e função de clique
        public static World_Sprite CreateDebugButton(Transform parent, Vector3 localPosition, string text, System.Action ClickFunc, int fontSize = 30, float paddingX = 5, float paddingY = 5)
        {

            // Cria um botão de depuração com um texto e adiciona um botão com a função de clique
            GameObject gameObject = new GameObject("DebugButton");
            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = localPosition;
            TextMesh textMesh = UtilsClass.CreateWorldText(text, gameObject.transform, Vector3.zero, fontSize, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 20000);
            Bounds rendererBounds = textMesh.GetComponent<MeshRenderer>().bounds;

            // Cor do botão e cor sobreposta (hover)

            Color color = UtilsClass.GetColorFromString("00BA00FF");
            if (color.r >= 1f) color.r = .9f;
            if (color.g >= 1f) color.g = .9f;
            if (color.b >= 1f) color.b = .9f;
            Color colorOver = color * 1.1f; // botão sobre a cor mais clara

            // Cria um World_Sprite para o botão e adiciona um botão com funções de clique e hover

            World_Sprite worldSprite = new World_Sprite(gameObject.transform, Vector3.zero, rendererBounds.size + new Vector3(paddingX, paddingY), Assets.i.s_White, color, sortingOrderDefault);
            worldSprite.AddButton(ClickFunc, () => worldSprite.SetColor(colorOver), () => worldSprite.SetColor(color));
            return worldSprite;
        }
        // Métodos estáticos para criar um World_Sprite com várias configurações diferentes
        public static World_Sprite Create(Transform parent, Vector3 localPosition, Vector3 localScale, Sprite sprite, Color color, int sortingOrderOffset)
        {
            return new World_Sprite(parent, localPosition, localScale, sprite, color, sortingOrderOffset);
        }
        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale, Sprite sprite, Color color, int sortingOrderOffset)
        {
            return new World_Sprite(null, worldPosition, localScale, sprite, color, sortingOrderOffset);
        }
        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale, Sprite sprite, Color color)
        {
            return new World_Sprite(null, worldPosition, localScale, sprite, color, 0);
        }
        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale, Color color)
        {
            return new World_Sprite(null, worldPosition, localScale, Assets.i.s_White, color, 0);
        }
        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale)
        {
            return new World_Sprite(null, worldPosition, localScale, Assets.i.s_White, Color.white, 0);
        }
        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale, int sortingOrderOffset)
        {
            return new World_Sprite(null, worldPosition, localScale, Assets.i.s_White, Color.white, sortingOrderOffset);
        }

        // Método estático para calcular a ordem de classificação com base na posição no mundo

        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        // Construtor para inicializar um World_Sprite com um transformador pai, posição local, escala local, sprite, cor e deslocamento de ordem de classificação
        public World_Sprite(Transform parent, Vector3 localPosition, Vector3 localScale, Sprite sprite, Color color, int sortingOrderOffset)
        {

            // Calcula a ordem de classificação
            int sortingOrder = GetSortingOrder(localPosition, sortingOrderOffset);

            // Cria o sprite no mundo
            gameObject = UtilsClass.CreateWorldSprite(parent, "Sprite", sprite, localPosition, localScale, sortingOrder, color);

            // Obtém a referência ao transformador e ao spriteRenderer do sprite
            transform = gameObject.transform;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        // Método para definir um deslocamento na ordem de classificação
        public void SetSortingOrderOffset(int sortingOrderOffset)
        {
            SetSortingOrder(GetSortingOrder(gameObject.transform.position, sortingOrderOffset));
        }

        // Método para definir a ordem de classificação
        public void SetSortingOrder(int sortingOrder)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        }

        // Método para definir a escala local
        public void SetLocalScale(Vector3 localScale)
        {
            transform.localScale = localScale;
        }

        // Método para definir a posição local
        public void SetPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }

        // Método para definir a cor do sprite
        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }

        // Método para definir o sprite
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        // Método para mostrar o sprite
        public void Show()
        {
            gameObject.SetActive(true);
        }

        // Método para ocultar o sprite
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // Método para adicionar um botão ao sprite
        public Button_Sprite AddButton(System.Action ClickFunc, System.Action MouseOverOnceFunc, System.Action MouseOutOnceFunc)
        {
            gameObject.AddComponent<BoxCollider2D>();
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            if (ClickFunc != null)
                buttonSprite.ClickFunc = ClickFunc;
            if (MouseOverOnceFunc != null)
                buttonSprite.MouseOverOnceFunc = MouseOverOnceFunc;
            if (MouseOutOnceFunc != null)
                buttonSprite.MouseOutOnceFunc = MouseOutOnceFunc;
            return buttonSprite;
        }

        // Método para destruir o sprite
        public void DestroySelf()
        {
            Object.Destroy(gameObject);
        }

    }
}