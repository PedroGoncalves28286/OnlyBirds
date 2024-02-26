using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey
{

    /*
     * Esta classe permite acionar uma ação após um certo tempo.
     * */
    public class FunctionTimer
    {

        /*
         *  Classe interna utilizada para associar uma ação ao ciclo de vida do MonoBehaviour.
         *  Isso permite que a ação seja acionada periodicamente
         * */
        private class MonoBehaviourHook : MonoBehaviour
        {

            // Método chamado a cada quadro de atualização do Unity

            public Action OnUpdate;

            private void Update()
            {
                if (OnUpdate != null) OnUpdate();
            }

        }


        private static List<FunctionTimer> timerList; // Lista de todos os temporizadores ativos
        private static GameObject initGameObject;// Objeto de jogo global usado para inicializar a classe; é destruído durante a troca de cena

        // Inicializa a lista de temporizadores, se necessário
        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionTimer_Global");
                timerList = new List<FunctionTimer>();
            }
        }

        // Cria um novo temporizador com uma ação a ser acionada após um determinado tempo
        public static FunctionTimer Create(Action action, float timer)
        {
            return Create(action, timer, "", false, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName)
        {
            return Create(action, timer, functionName, false, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime)
        {
            return Create(action, timer, functionName, useUnscaledDeltaTime, false);
        }
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime, bool stopAllWithSameName)
        {

            // Inicializa a lista de temporizadores, se necessário

            InitIfNeeded();

            // Para todos os temporizadores com o mesmo nome, se solicitado

            if (stopAllWithSameName)
            {
                StopAllTimersWithName(functionName);
            }

            // Cria um novo objeto de jogo para associar a ação ao ciclo de vida do MonoBehaviour

            GameObject obj = new GameObject("FunctionTimer Object " + functionName, typeof(MonoBehaviourHook));

            // Cria um novo temporizador

            FunctionTimer funcTimer = new FunctionTimer(obj, action, timer, functionName, useUnscaledDeltaTime);
            // Associa o método de atualização do temporizador ao ciclo de vida do MonoBehaviour
            obj.GetComponent<MonoBehaviourHook>().OnUpdate = funcTimer.Update;


            // Adiciona o temporizador à lista de temporizadores ativos

            timerList.Add(funcTimer);

            return funcTimer;
        }


        // Remove um temporizador da lista
        public static void RemoveTimer(FunctionTimer funcTimer)
        {
            timerList.Remove(funcTimer);
        }

        // Para todos os temporizadores com um determinado nome
        public static void StopAllTimersWithName(string functionName)
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                if (timerList[i].functionName == functionName)
                {
                    timerList[i].DestroySelf();
                    i--;// Decrementa o índice para garantir que todos os temporizadores com o mesmo nome sejam parados
                }
            }
        }
        // Para o primeiro temporizador com um determinado nome
        public static void StopFirstTimerWithName(string functionName)
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                if (timerList[i].functionName == functionName)
                {
                    timerList[i].DestroySelf();
                    return;
                }
            }
        }





        private GameObject gameObject;// Objeto de jogo associado ao temporizador
        private float timer;// Tempo restante no temporizador
        private string functionName; // Nome do temporizador
        private bool active;
        private bool useUnscaledDeltaTime; // Flag indicando se o tempo deve ser baseado no tempo desacelerado
        private Action action;// Ação a ser acionada quando o temporizador expirar


        // Construtor da classe FunctionTimer
        public FunctionTimer(GameObject gameObject, Action action, float timer, string functionName, bool useUnscaledDeltaTime)
        {
            this.gameObject = gameObject;
            this.action = action;
            this.timer = timer;
            this.functionName = functionName;
            this.useUnscaledDeltaTime = useUnscaledDeltaTime;
        }
        // Método de atualização do temporizador
        private void Update()
        {
            if (useUnscaledDeltaTime)
            {
                timer -= Time.unscaledDeltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }

            // Verifica se o temporizador expirou
            if (timer <= 0)
            {
                // Timer complete, trigger Action
                action();
                DestroySelf();
            }
        }
        // Remove este temporizador da lista de temporizadores e destrói o objeto de jogo associado, se existir.
        private void DestroySelf()
        {
            RemoveTimer(this);// Remove este temporizador da lista de temporizadores ativos
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);// Destroi o objeto de jogo associado, se existir
            }
        }




        /*
         *Classe que permite acionar ações manualmente sem criar um GameObject associado.
         * Útil quando não é necessário associar a ação a um objeto específico no cenário.
         * */
        public class FunctionTimerObject
        {

            private float timer;// Tempo restante para acionar a ação
            private Action callback;// Ação a ser acionada quando o temporizador expirar

            // Construtor da classe FunctionTimerObject
            public FunctionTimerObject(Action callback, float timer)
            {
                this.callback = callback;
                this.timer = timer;
            }

            // Método de atualização do temporizador, usando o tempo delta fornecido pelo Unity
            public void Update()
            {
                Update(Time.deltaTime);
            }
            public void Update(float deltaTime)
            {
                timer -= deltaTime;
                if (timer <= 0)
                {
                    callback();
                }
            }
        }

        // Método estático para criar um objeto FunctionTimerObject com uma ação e um tempo especificados
        // Útil para criar temporizadores independentes que não estão associados a um GameObject
        public static FunctionTimerObject CreateObject(Action callback, float timer)
        {
            return new FunctionTimerObject(callback, timer);
        }
    }
}