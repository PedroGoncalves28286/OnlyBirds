using UnityEngine;

namespace CodeMonkey.Utils
{

    /*
     * Barra "in the World"
     * */
    public class World_Bar
    {

        // Variáveis para armazenar referências ao objeto da barra e suas partes
        private GameObject gameObject;
        private Transform transform;
        private Transform background;
        private Transform bar;

        // Método estático para calcular a ordem de classificação com base na posição
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        // Classe interna para armazenar informações sobre o contorno da barra
        public class Outline
        {
            public float size = 1f;
            public Color color = Color.black;
        }

        // Construtor da classe World_Bar para inicializar a barra no mundo
        public World_Bar(Transform parent, Vector3 localPosition, Vector3 localScale, Color? backgroundColor, Color barColor, float sizeRatio, int sortingOrder, Outline outline = null)
        {
            // Configura o master da barra e sua posição local
            SetupParent(parent, localPosition);
            // Configura o contorno se fornecido
            if (outline != null) SetupOutline(outline, localScale, sortingOrder - 1);
            // Configura o fundo se fornecido
            if (backgroundColor != null) SetupBackground((Color)backgroundColor, localScale, sortingOrder);
            SetupBar(barColor, localScale, sortingOrder + 1);
            // Define o tamanho da barra com base na proporção fornecida
            SetSize(sizeRatio);
        }
        // Método privado para configurar o pai da barra
        private void SetupParent(Transform parent, Vector3 localPosition)
        {
            // Cria um novo GameObject para a barra
            gameObject = new GameObject("World_Bar");
            transform = gameObject.transform;
            // Define o pai da barra e sua posição local
            transform.SetParent(parent);
            transform.localPosition = localPosition;
        }
        // Método privado para configurar o contorno da barra
        private void SetupOutline(Outline outline, Vector3 localScale, int sortingOrder)
        {
            UtilsClass.CreateWorldSprite(transform, "Outline", Assets.i.s_White, new Vector3(0, 0), localScale + new Vector3(outline.size, outline.size), sortingOrder, outline.color);
        }
        // Cria um sprite de fundo para a barra
        private void SetupBackground(Color backgroundColor, Vector3 localScale, int sortingOrder)
        {
            // Cria um sprite de fundo para a barra
            background = UtilsClass.CreateWorldSprite(transform, "Background", Assets.i.s_White, new Vector3(0, 0), localScale, sortingOrder, backgroundColor).transform;
        }
        // Método privado para configurar a barra principal
        private void SetupBar(Color barColor, Vector3 localScale, int sortingOrder)
        {
            // Cria um GameObject para a barra principal
            GameObject barGO = new GameObject("Bar");
            bar = barGO.transform;
            bar.SetParent(transform);
            // Define a posição local da barra e sua escala
            bar.localPosition = new Vector3(-localScale.x / 2f, 0, 0);
            bar.localScale = new Vector3(1, 1, 1);
            // Cria um sprite para a parte interna da barra
            Transform barIn = UtilsClass.CreateWorldSprite(bar, "BarIn", Assets.i.s_White, new Vector3(localScale.x / 2f, 0), localScale, sortingOrder, barColor).transform;
        }
        // Método para definir o tamanho da barra
        public void SetRotation(float rotation)
        {
            transform.localEulerAngles = new Vector3(0, 0, rotation);
        }

        // Método para definir o tamanho da barra
        public void SetSize(float sizeRatio)
        {
            bar.localScale = new Vector3(sizeRatio, 1, 1);
        }

        // Método para definir a cor da barra
        public void SetColor(Color color)
        {
            bar.Find("BarIn").GetComponent<SpriteRenderer>().color = color;
        }

        // Método para mostrar a barra
        public void Show()
        {
            gameObject.SetActive(true);
        }

        // Método para ocultar a barra
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        // Método para adicionar um botão à barra
        public Button_Sprite AddButton(System.Action ClickFunc, System.Action MouseOverOnceFunc, System.Action MouseOutOnceFunc)
        {
            // Adiciona um componente Button_Sprite ao GameObject da barra
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            // Define as funções de clique, mouseover e mouseout, se fornecidas
            if (ClickFunc != null)
                buttonSprite.ClickFunc = ClickFunc;
            if (MouseOverOnceFunc != null)
                buttonSprite.MouseOverOnceFunc = MouseOverOnceFunc;
            if (MouseOutOnceFunc != null)
                buttonSprite.MouseOutOnceFunc = MouseOutOnceFunc;
            return buttonSprite;
        }
        // Método para destruir a barra
        public void DestroySelf()
        {
            // Destroi o GameObject da barra, se existir
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }

    }

}
