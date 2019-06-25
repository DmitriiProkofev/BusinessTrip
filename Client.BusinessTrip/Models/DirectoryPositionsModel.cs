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
    /// Класс модели "Справочник должностей".
    /// </summary>
    public class DirectoryPositionsModel : IBaseModel<Position>
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Справочник должностей";

        //Лог.
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IRepository<Position> _repository;

        #endregion //Private Fields and Consts

        #region Lazy Properties

        /// <summary>
        /// Ленивое свойство для репозитория.
        /// </summary>
        private IRepository<Position> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = CompositionRoot.Resolve<IRepository<Position>>();
                }

                _repository.ConnectionString = Resources.ConnectionStringDomain;

                return _repository;
            }
        }

        #endregion

        #region IBaseModel

        /// <summary>
        /// Сохранение должности.
        /// </summary>
        /// <param name="position">Должность.</param>
        public int? Save(Position position)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var positionSave = Repository.Save(position);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return positionSave.PositionId;
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
                MessageBox.Show("Ошибка при сохранении должности.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Обновление должности.
        /// </summary>
        /// <param name="position">Должность.</param>
        public void Update(Position position)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                Repository.Update(position);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Обновление записи: {1} ({2} с.).", DirectoryName, position.PositionId, implementationTime.TotalSeconds));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Обновление невозможно. Должность удалена другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при обновлении должности.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление группы должностей.
        /// </summary>
        /// <param name="position">Должности.</param>
        public void Delete(List<Position> positions)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.Delete(positions);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, positions.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Должность удалена другим пользователем, имеются привязки к другим объектам или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", positions.Count == 1 ? "должности" : "должностей"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        /// <summary>
        /// Получение всех должностей.
        /// </summary>
        /// <returns>Должности.</returns>
        public List<Position> GetAll()
        {
            var res = new List<Position>();

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
                    MessageBox.Show("Ошибка при получении должностей.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        #endregion //IBaseModel
    }
}
