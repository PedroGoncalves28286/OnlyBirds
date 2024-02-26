using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey
{

    /*
     * Chama a função em todas as actualizações até que retorne verdadeiro
     * */
    public class FunctionUpdater
    {

        /*
         * Classe interna para conectar ações ao MonoBehaviour.
         * Isso permite que a ação seja chamada em cada atualização do Unity.
         * */
        private class MonoBehaviourHook : MonoBehaviour
        {

            public Action OnUpdate;

            // Método chamado a cada quadro de atualização do Unity.
            private void Update()
            {
                if (OnUpdate != null) OnUpdate();
            }

        }

        private static List<FunctionUpdater> updaterList; //Contém uma referência a todos os atualizadores ativos
        private static GameObject initGameObject; //Objecto de jogo global utilizado para inicializar a classe, é destruído na mudança de cena

        // Inicializa a lista de atualizadores, se necessário.
        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionUpdater_Global");
                updaterList = new List<FunctionUpdater>();
            }
        }

        // Cria um novo atualizador com uma função de atualização especificada.
        public static FunctionUpdater Create(Action updateFunc)
        {
            return Create(() => { updateFunc(); return false; }, "", true, false);
        }
        public static FunctionUpdater Create(Func<bool> updateFunc)
        {
            return Create(updateFunc, "", true, false);
        }
        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName)
        {
            return Create(updateFunc, functionName, true, false);
        }
        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active)
        {
            return Create(updateFunc, functionName, active, false);
        }
        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName)
        {
            // Inicializa a lista de atualizadores, se necessário.
            InitIfNeeded();

            // Para todos os atualizadores com o mesmo nome, se solicitado.

            if (stopAllWithSameName)
            {
                StopAllUpdatersWithName(functionName);
            }

            // Cria um novo objeto de jogo para associar a função de atualização ao ciclo de vida do MonoBehaviour.
            GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, typeof(MonoBehaviourHook));
            // Cria um novo atualizador.
            FunctionUpdater functionUpdater = new FunctionUpdater(gameObject, updateFunc, functionName, active);
            // Associa o método de atualização do atualizador ao ciclo de vida do MonoBehaviour.
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionUpdater.Update;
            // Adiciona o atualizador à lista de atualizadores ativos.

            updaterList.Add(functionUpdater);
            return functionUpdater;
        }
        private static void RemoveUpdater(FunctionUpdater funcUpdater)
        {
            updaterList.Remove(funcUpdater);
        }
        public static void DestroyUpdater(FunctionUpdater funcUpdater)
        {
            if (funcUpdater != null)
            {
                funcUpdater.DestroySelf();
            }
        }
        public static void StopUpdaterWithName(string functionName)
        {
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    return;
                }
            }
        }
        public static void StopAllUpdatersWithName(string functionName)
        {
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    i--;// Decrementa o índice para garantir que todos os atualizadores com o mesmo nome sejam parados.
                }
            }
        }





        private GameObject gameObject;// Objeto de jogo associado ao atualizador.
        private string functionName;// Nome do atualizador.
        private bool active;// Indica se o atualizador está ativo.
        private Func<bool> updateFunc; // Função de atualização; o atualizador é destruído quando essa função retorna verdadeiro.

        // Construtor da classe FunctionUpdater.
        public FunctionUpdater(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active)
        {
            this.gameObject = gameObject;
            this.updateFunc = updateFunc;
            this.functionName = functionName;
            this.active = active;
        }
        // Pausa o atualizador.
        public void Pause()
        {
            active = false;
        }

        // Retoma o atualizador.
        public void Resume()
        {
            active = true;
        }

        // Método de atualização do atualizador.
        private void Update()
        {
            // Verifica se o atualizador está ativo.
            if (!active) return;
            // Chama a função de atualização.
            if (updateFunc())
            {
                DestroySelf();// Destroi o atualizador se a função de atualização retornar verdadeiro.
            }
        }
        // Destroi o atualizador.
        public void DestroySelf()
        {
            RemoveUpdater(this);
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}