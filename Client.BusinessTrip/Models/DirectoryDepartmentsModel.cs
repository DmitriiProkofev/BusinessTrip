using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IoC;
using Client.BusinessTrip.Properties;
using Core.BusinessTrip.DataInterfaces;
using Core.BusinessTrip.Domain;
using Core.BusinessTrip.ProjectBase.Utils.AsyncWorking;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Windows.Forms;

namespace Client.BusinessTrip.Models
{
    /// <summary>
    /// Класс модели "Справочник отделов".
    /// </summary>
    public class DirectoryDepartmentsModel : IBaseModel<Department>
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Справочник подразделений";

        //Лог.
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IRepository<Department> _repository;

        #endregion //Private Fields and Consts

        #region Lazy Properties

        /// <summary>
        /// Ленивое свойство для репозитория.
        /// </summary>
        private IRepository<Department> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = CompositionRoot.Resolve<IRepository<Department>>();
                }

                _repository.ConnectionString = Resources.ConnectionStringDomain;

                return _repository;
            }
        }

        #endregion

        #region IBaseModel

        /// <summary>
        /// Сохранение отдела.
        /// </summary>
        /// <param name="department">Отдел.</param>
        public int? Save(Department department)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var departmentSave = Repository.Save(department);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return departmentSave.DepartmentId;
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Сохранение невозможно. Отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при сохранении отдела.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Обновление отдела.
        /// </summary>
        /// <param name="department">Отдел.</param>
        public void Update(Department department)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                Repository.Update(department);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Обновление записи: {1} ({2} с.).", DirectoryName, department.DepartmentId, implementationTime.TotalSeconds));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Обновление невозможно. Отдел удален другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при обновлении отдела.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление группы отделов.
        /// </summary>
        /// <param name="departments">Отделы.</param>
        public void Delete(List<Department> departments)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.Delete(departments);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, departments.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Отдел удален другим пользователем, имеются привязки к другим объектам или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", departments.Count == 1 ? "отдела" : "отделов"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        /// <summary>
        /// Получение всех отделов.
        /// </summary>
        /// <returns>Отделы.</returns>
        public List<Department> GetAll()
        {
            var res = new List<Department>();

            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    res = Repository.GetAll();

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Получение данных ({1} с.).", DirectoryName, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Ошибка при получении отделов.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        #endregion //IBaseModel
    }
}
