using Client.BusinessTrip.IoC;
using Client.BusinessTrip.IoC.Ninject;
using Client.BusinessTrip.Views;
using Data.BusinessTrip.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.BusinessTrip
{
    static class BusinessTrips
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CompositionRoot.Wire(new NinjectBindings());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(CompositionRoot.Resolve<BusinessTripView>());
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", "Оформление командировок", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show("Ошибка приложения.", "Оформление командировок", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
