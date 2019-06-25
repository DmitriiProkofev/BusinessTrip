using System;
using System.Reflection;
using System.Windows.Forms;

namespace Client.BusinessTrip.Helpers
{
    /// <summary>
    /// Дополнительные методы.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Двойная буферизация для сглаживания таблицы.
        /// </summary>
        /// <param name="dgv">Таблица.</param>
        /// <param name="setting">Включение\выключение буферизации.</param>
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        /// <summary>
        /// Двойная буферизация для сглаживания отображения формы.
        /// </summary>
        /// <param name="form">Форма.</param>
        /// <param name="setting">Включение\выключение буферизации.</param>
        public static void DoubleBuffered(this Form form, bool setting)
        {
            Type formType = form.GetType();
            PropertyInfo pi = formType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(form, setting, null);
        }

        /// <summary>
        /// Сравнение строк с учетом настроек.
        /// </summary>
        /// <param name="source">Исходная строка.</param>
        /// <param name="toCheck">Строка с которой сравнивается исходная.</param>
        /// <param name="comp">Настройка.</param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

    }
}
