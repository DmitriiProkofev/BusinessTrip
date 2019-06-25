using System;
using System.Threading;

namespace Core.BusinessTrip.ProjectBase.Utils.AsyncWorking
{
    /// <summary>
    /// Класс асинхронного выполнение метода с получением результата.
    /// </summary>
    /// <typeparam name="TResult">Тип результата работы метода (напр. object).</typeparam>
    public class AsyncLoader<TResult>
    {
        #region Fields

        /// <summary>
        /// Метод, вызываемый после завершения работы.
        /// </summary>
        protected readonly Action<TResult> CallbackMethod;

        /// <summary>
        /// Метод, выполняющий работу.
        /// </summary>
        protected readonly OutEventHandler<TResult> ExecuteMethod;

        /// <summary>
        /// Результат работы асинхронного метода.
        /// </summary>
        protected TResult Result;

        #endregion //Fields

        #region Methods

        /// <summary>
        /// Запуск асинхронного выполнения метода.
        /// </summary>
        public virtual void Start()
        {
            var thread = new Thread(Run) {IsBackground = true};
            thread.Start();
        }

        private void Run()
        {
            ExecuteMethod(out Result);

            CallbackMethod(Result);

            OnFinished();
        }

        /// <summary>
        /// Зажигание события с проверкой на null.
        /// </summary>
        protected void OnFinished()
        {
            if (Finished != null) Finished();
        }

        #endregion //Methods

        #region Delegates

        /// <summary>
        /// Делегат, описывающий асинхронный метод.
        /// </summary>
        /// <typeparam name="TOut">Тип выходного параметра.</typeparam>
        /// <param name="outValue">Выходной параметр.</param>
        public delegate void OutEventHandler<TOut>(out TOut outValue);

        #endregion //Delegates

        /// <summary>
        /// Создание нового экземпляра асинхронного загрузчика.
        /// </summary>
        /// <param name="executeMethod">Метод, выполняемый асинхронно, возвращающий результат TResult</param>
        /// <param name="callbackMethod">Метод - точка возврата асинхронной работы.</param>
        public AsyncLoader(
            OutEventHandler<TResult> executeMethod,
            Action<TResult> callbackMethod)
        {
            ExecuteMethod = executeMethod;
            CallbackMethod = callbackMethod;
        }

        /// <summary>
        /// Возникает при окончании загрузки методов.
        /// </summary>
        public event Action Finished;

        /// <summary>
        /// Статический метод асинхронного выполнения метода без получения результата.
        /// </summary>
        /// <param name="method">Исполняемый метод.</param>
        public static void Execute(ThreadStart method)
        {
            var thread = new Thread(method) {IsBackground = true};
            thread.Start();
        }
    }
}