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
    /// Класс модели "Справочник видов работ".
    /// </summary>
    public class DirectoryTypeWorksModel : IBaseModel<TypeWork>
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Справочник видов работ";

        //Лог.
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IRepository<TypeWork> _repository;

        #endregion //Private Fields and Consts

        #region Lazy Properties

        /// <summary>
        /// Ленивое свойство для репозитория.
        /// </summary>
        private IRepository<TypeWork> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = CompositionRoot.Resolve<IRepository<TypeWork>>();
                }

                _repository.ConnectionString = Resources.ConnectionStringDomain;

                return _repository;
            }
        }

        #endregion

        #region IBaseModel

        /// <summary>
        /// Сохранение вида работ.
        /// </summary>
        /// <param name="typeWork">Вид работ.</param>
        public int? Save(TypeWork typeWork)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var typeWorkSave = Repository.Save(typeWork);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return typeWorkSave.TypeWorkId;
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
                MessageBox.Show("Ошибка при сохранении вида работ.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Обновление вида работ.
        /// </summary>
        /// <param name="typeWork">Вид работ.</param>
        public void Update(TypeWork typeWork)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                Repository.Update(typeWork);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Обновление записи: {1} ({2} с.).", DirectoryName, typeWork.TypeWorkId, implementationTime.TotalSeconds));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Обновление невозможно. Вид работ удален другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при обновлении вида работ.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление группы видов работ.
        /// </summary>
        /// <param name="typeWorks">Виды работ.</param>
        public void Delete(List<TypeWork> typeWorks)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.Delete(typeWorks);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, typeWorks.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Вид работ удален другим пользователем, имеются привязки к другим объектам или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", typeWorks.Count == 1 ? "вида работ" : "видов работ"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        /// <summary>
        /// Получение всех видов работ.
        /// </summary>
        /// <returns>Виды работ.</returns>
        public List<TypeWork> GetAll()
        {
            var res = new List<TypeWork>();

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
                    MessageBox.Show("Ошибка при получении видов работ.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        #endregion //IBaseModel
    }
}
