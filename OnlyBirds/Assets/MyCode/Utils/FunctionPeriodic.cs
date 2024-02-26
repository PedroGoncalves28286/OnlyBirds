using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey
{

    /*
     * Esta classe permite a execução de uma função periodicamente com opções de personalização.
     * */
    public class FunctionPeriodic
    {

        /*
         * Classe interna utilizada para associar uma ação ao ciclo de vida do MonoBehaviour.
         * Isso permite que a ação seja acionada periodicamente.
         * */
        private class MonoBehaviourHook : MonoBehaviour
        {

            public Action OnUpdate;

            // Método chamado a cada quadro de atualização do Unity
            private void Update()
            {
                if (OnUpdate != null) OnUpdate();
            }

        }


        private static List<FunctionPeriodic> funcList; //Mantém uma referência a todos os temporizadores activos
        private static GameObject initGameObject; //Objecto de jogo global utilizado para inicializar a classe, é destruído na mudança de cena

        // Inicializa a lista de temporizadores, se necessário
        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionPeriodic_Global");
                funcList = new List<FunctionPeriodic>();
            }
        }




        // Cria um temporizador global que persiste nas mudanças de cena
        public static FunctionPeriodic Create_Global(Action action, Func<bool> testDestroy, float timer)
        {
            FunctionPeriodic functionPeriodic = Create(action, testDestroy, timer, "", false, false, false);
            MonoBehaviour.DontDestroyOnLoad(functionPeriodic.gameObject);
            return functionPeriodic;
        }


        // Cria um temporizador que executa uma ação periodicamente
        public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer)
        {
            return Create(action, testDestroy, timer, "", false);
        }

        public static FunctionPeriodic Create(Action action, float timer)
        {
            return Create(action, null, timer, "", false, false, false);
        }

        public static FunctionPeriodic Create(Action action, float timer, string functionName)
        {
            return Create(action, null, timer, functionName, false, false, false);
        }

        public static FunctionPeriodic Create(Action callback, Func<bool> testDestroy, float timer, string functionName, bool stopAllWithSameName)
        {
            return Create(callback, testDestroy, timer, functionName, false, false, stopAllWithSameName);
        }

        public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer, string functionName, bool useUnscaledDeltaTime, bool triggerImmediately, bool stopAllWithSameName)
        {
            InitIfNeeded();

            if (stopAllWithSameName)
            {
                StopAllFunc(functionName);
            }

            GameObject gameObject = new GameObject("FunctionPeriodic Object " + functionName, typeof(MonoBehaviourHook));
            FunctionPeriodic functionPeriodic = new FunctionPeriodic(gameObject, action, timer, testDestroy, functionName, useUnscaledDeltaTime);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionPeriodic.Update;

            funcList.Add(functionPeriodic);

            if (triggerImmediately) action();

            return functionPeriodic;
        }




        // Remove um temporizador da lista
        public static void RemoveTimer(FunctionPeriodic funcTimer)
        {
            funcList.Remove(funcTimer);
        }

        // Para um temporizador com um nome específico
        public static void StopTimer(string _name)
        {
            for (int i = 0; i < funcList.Count; i++)
            {
                if (funcList[i].functionName == _name)
                {
                    funcList[i].DestroySelf();
                    return;
                }
            }
        }

        // Para todos os temporizadores com o mesmo nome
        public static void StopAllFunc(string _name)
        {
            for (int i = 0; i < funcList.Count; i++)
            {
                if (funcList[i].functionName == _name)
                {
                    funcList[i].DestroySelf();
                    i--;
                }
            }
        }

        // Verifica se um temporizador com o nome fornecido está ativo
        public static bool IsFuncActive(string name)
        {
            for (int i = 0; i < funcList.Count; i++)
            {
                if (funcList[i].functionName == name)
                {
                    return true;
                }
            }
            return false;
        }




        private GameObject gameObject;
        private float timer;
        private float baseTimer;
        private bool useUnscaledDeltaTime;
        private string functionName;
        public Action action;
        public Func<bool> testDestroy;


        private FunctionPeriodic(GameObject gameObject, Action action, float timer, Func<bool> testDestroy, string functionName, bool useUnscaledDeltaTime)
        {
            this.gameObject = gameObject;
            this.action = action;
            this.timer = timer;
            this.testDestroy = testDestroy;
            this.functionName = functionName;
            this.useUnscaledDeltaTime = useUnscaledDeltaTime;
            baseTimer = timer;
        }

        // Pula o temporizador para um determinado valor
        public void SkipTimerTo(float timer)
        {
            this.timer = timer;
        }

        // Método de atualização para verificar se a ação deve ser executada novamente
        void Update()
        {
            if (useUnscaledDeltaTime)
            {
                timer -= Time.unscaledDeltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                action();
                if (testDestroy != null && testDestroy())
                {
                    //Destroy
                    DestroySelf();
                }
                else
                {
                    //Repeat
                    timer += baseTimer;
                }
            }
        }

        // Destruir o temporizador e remover da lista
        public void DestroySelf()
        {
            RemoveTimer(this);
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}