using System;
using UnityEngine;

namespace CodeMonkey.Utils
{

    /*
     * Esta classe ComponentActions fornece métodos para associar ações (delegates) a eventos do ciclo de vida de um componente MonoBehaviour.
     * */
    public class ComponentActions : MonoBehaviour
    {

        // Ações associadas a eventos do MonoBehaviour

        public Action OnDestroyFunc;
        public Action OnEnableFunc;
        public Action OnDisableFunc;
        public Action OnUpdate;

        // Método chamado quando o componente é destruído
        void OnDestroy()
        {
            if (OnDestroyFunc != null) OnDestroyFunc();
        }

        // Método chamado quando o componente é ativado
        void OnEnable()
        {
            if (OnEnableFunc != null) OnEnableFunc();
        }
        void OnDisable()
        {
            if (OnDisableFunc != null) OnDisableFunc();
        }
        // Método chamado a cada quadro de atualização
        void Update()
        {
            if (OnUpdate != null) OnUpdate();
        }

        // Método estático para criar um novo GameObject com um ComponentActions associado
        public static void CreateComponent(Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null)
        {
            GameObject gameObject = new GameObject("ComponentActions");
            AddComponent(gameObject, OnDestroyFunc, OnEnableFunc, OnDisableFunc, OnUpdate);
        }

        // Método estático para adicionar um ComponentActions a um GameObject existente
        public static void AddComponent(GameObject gameObject, Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null)
        {
            ComponentActions componentFuncs = gameObject.AddComponent<ComponentActions>();
            componentFuncs.OnDestroyFunc = OnDestroyFunc;
            componentFuncs.OnEnableFunc = OnEnableFunc;
            componentFuncs.OnDisableFunc = OnDisableFunc;
            componentFuncs.OnUpdate = OnUpdate;
        }
    }

}