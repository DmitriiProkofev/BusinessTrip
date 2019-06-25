using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.BusinessTrip.ProjectBase.Utils.AsyncWorking
{
    /// <summary>
    /// Форма с ProgressBar'ом. Используется во время длительных операций.
    /// </summary>
    public partial class AsyncLoaderForm : Form
    {
        #region static

        /// <summary>
        /// Показывает ход выполнения путем увеличения размера ровной, непрерывной полосы
        /// в System.Windows.Forms.ProgressBar.
        /// </summary>
        /// <param name="workMethod">Метод, выполняемый асинхронно.</param>
        /// <param name="max">Общее количество итераций.</param>
        /// <param name="name">Название операции, используемое для отображения.</param>
        public static void ShowContinuous(DoWorkEventHandler workMethod, int max, string name = "")
        {
            using (var waitForm = new AsyncLoaderForm(workMethod, name, ProgressBarStyle.Continuous, max))
            {
                waitForm.ShowDialog();
            }
        }

        /// <summary>
        /// Показывает ход выполнения путем непрерывной прокрутки блока в объекте System.Windows.Forms.ProgressBar,
        /// используя форму "отмеченной области" (marquee).
        /// </summary>
        /// <param name="workMethod">Метод, выполняемый асинхронно.</param>
        /// <param name="name">Название операции, используемое для отображения.</param>
        public static void ShowMarquee(DoWorkEventHandler workMethod, string name = "")
        {
            using (var waitForm = new AsyncLoaderForm(workMethod, name, ProgressBarStyle.Marquee))
            {
                waitForm.ShowDialog();
            }
        }

        /// <summary>
        /// Показывает ход выполнения путем непрерывной прокрутки блока в объекте System.Windows.Forms.ProgressBar,
        /// используя форму "отмеченной области" (marquee).
        /// </summary>
        /// <param name="workMethod">Метод, выполняемый асинхронно.</param>
        /// <param name="name">Название операции, используемое для отображения.</param>
        /// <returns>Возвращает DoWorkEventArgs.Result.</returns>
        public static object ShowResultMarquee(DoWorkEventHandler workMethod, string name = "")
        {
            object result = null;
            using (var waitForm = new AsyncLoaderForm(workMethod, name, ProgressBarStyle.Marquee))
            {
                waitForm.BackgroundWorker.RunWorkerCompleted +=
                    delegate (object sender, RunWorkerCompletedEventArgs args) { result = args.Result; };

                waitForm.ShowDialog();
            }

            return result;
        }

        #endregion //static

        #region Fields

        private readonly bool _isAutoClose = true;

        #endregion //Fields

        #region Constructors

        /// <summary>
        /// Создает форму, показывающую прогресс выполнения операции.
        /// </summary>
        /// <param name="workMethod">Метод, выполняемый асинхронно.</param>
        /// <param name="name">Название операции, используемое для отображения.</param>
        /// <param name="style">Стиль ProgressBar.</param>
        /// <param name="max">Общее количество итераций (для ProgressBarStyle.Continuous).</param>
        /// <param name="isAutoClose">Закроется ли форма с после завершения итерации.</param>
        public AsyncLoaderForm(DoWorkEventHandler workMethod, string name,
                               ProgressBarStyle style, int max = 100, bool isAutoClose = true)
            : this()
        {
            BackgroundWorker.DoWork += workMethod;

            label_name.Text = name;
            progressBarWork.Style = style;
            progressBarWork.Value = progressBarWork.Minimum = 0;
            progressBarWork.Maximum = max;
            _isAutoClose = isAutoClose;
        }

        /// <summary>
        /// Создает форму, показывающую прогресс выполнения операции.
        /// </summary>
        /// <param name="workMethod">Метод, выполняемый асинхронно.</param>
        /// <param name="callBackMethod">Метод, выполняемый после завершения операции.</param>
        /// <param name="name">Название операции, используемое для отображения.</param>
        /// <param name="style">Стиль ProgressBar.</param>
        /// <param name="max">Общее количество итераций (для ProgressBarStyle.Continuous).</param>
        /// <param name="isAutoClose">Закроется ли форма с после завершения итерации.</param>
        public AsyncLoaderForm(DoWorkEventHandler workMethod, RunWorkerCompletedEventHandler callBackMethod,
                               string name, ProgressBarStyle style, int max = 100, bool isAutoClose = true)
            : this(workMethod, name, style, max, isAutoClose)
        {
            backgroundWorkerMaster.RunWorkerCompleted += callBackMethod;
        }

        /// <summary>
        /// Создает форму, показывающую прогресс выполнения операции.
        /// </summary>
        public AsyncLoaderForm()
        {
            InitializeComponent();

            progressBarWork.Maximum = 100;
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Элемент, выполняющий какой-либо процесс в отдельном потоке. 
        /// Необходимо задать выполняемый им процесс, используя BackgroundWorker.DoWork.
        /// </summary>
        public BackgroundWorker BackgroundWorker
        {
            get { return backgroundWorkerMaster; }
        }

        /// <summary>
        /// Стиль ProgressBar.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get { return progressBarWork.Style; }
            set { progressBarWork.Style = value; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Смена операции.
        /// </summary>
        /// <param name="name">Новое название метода.</param>
        /// <param name="max">Количество итераций в операции.</param>
        /// <param name="style">Стиль панели статуса.</param>
        public void OperationChanged(string name, int max, ProgressBarStyle style)
        {
            if (!InvokeRequired)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    label_name.Text = name;
                    progressBarWork.Style = style;
                    progressBarWork.Maximum = max;
                    progressBarWork.Value = progressBarWork.Minimum;
                }
            }
            else
            {
                Invoke(new Action<string, int, ProgressBarStyle>(OperationChanged), name, max, style);
            }
        }


        /// <summary>
        /// Форма закрывается.
        /// </summary>
        public void RunWorkerClose()
        {
            if (!InvokeRequired)
                Close();
            else
                Invoke(new Action(RunWorkerClose));
        }

        #region Private 

        /// <summary>
        /// После завершения процесса, форма закрывается.
        /// </summary>
        private void BackgroundWorkerMasterRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, реагирующий на изменение прогресса.
        /// Изменение прогресса вызывается BackgroundWorker.ReportProgress(значение);.
        /// </summary>
        private void BackgroundWorkerMasterProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarWork.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// При запуске формы начинает выполняться процесс, заданный в BackgroundWorker.DoWork.
        /// </summary>
        private void WaitFormLoad(object sender, EventArgs e)
        {
            backgroundWorkerMaster.RunWorkerAsync();
        }

        private void AsyncLoaderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //BackgroundWorker.DoWork += workMethod;
        }

        #endregion //Private

        #endregion //Methods


    }
}
