using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace Client.BusinessTrip.IoC
{
    /// <summary>
    /// Внедрение связей.
    /// </summary>
    public class CompositionRoot
    {
        private static IKernel _ninjectKernel;

        /// <summary>
        /// Инициализация связей.
        /// </summary>
        /// <param name="module"></param>
        public static void Wire(INinjectModule module)
        {
            _ninjectKernel = new StandardKernel(module);
        }

        /// <summary>
        /// Получение связанного экземпляра. 
        /// </summary>
        /// <typeparam name="T">Экземпляр.</typeparam>
        /// <returns>Экземпляр со связями.</returns>
        public static T Resolve<T>()
        {
            return _ninjectKernel.Get<T>();
        }

        /// <summary>
        /// Получение связанного экземпляра с заданием параметров.
        /// </summary>
        /// <typeparam name="T">Экземпляр.</typeparam>
        /// <param name="parameters">Параметры.</param>
        /// <returns>Экземпляр со связями.</returns>
        public static T Resolve<T>(IParameter[] parameters)
        {
            return _ninjectKernel.Get<T>(parameters);
        }
    }
}
