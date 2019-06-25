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
    /// Класс модели "Справочник адресов".
    /// </summary>
    public class DirectoryLocationsModel : IBaseModel<Location>
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Справочник адресов";

        //Лог.
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IRepository<Location> _repository;

        #endregion //Private Fields and Consts

        #region Lazy Properties

        /// <summary>
        /// Ленивое свойство для репозитория.
        /// </summary>
        private IRepository<Location> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = CompositionRoot.Resolve<IRepository<Location>>();
                }

                _repository.ConnectionString = Resources.ConnectionStringDomain;

                return _repository;
            }
        }

        #endregion

        #region IBaseModel

        /// <summary>
        /// Сохранение местоположения.
        /// </summary>
        /// <param name="location">Местоположение.</param>
        public int? Save(Location location)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var locationSave = Repository.Save(location);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return locationSave.LocationId;
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
                MessageBox.Show("Ошибка при сохранении адреса.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Обновление местоположения.
        /// </summary>
        /// <param name="location">Местоположение.</param>
        public void Update(Location location)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                Repository.Update(location);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Обновление записи: {1} ({2} с.).", DirectoryName, location.LocationId, implementationTime.TotalSeconds));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Обновление невозможно. Адрес удален другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при обновлении адреса.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление группы местоположений.
        /// </summary>
        /// <param name="locations">Местоположения.</param>
        public void Delete(List<Location> locations)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.Delete(locations);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, locations.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Адрес удален другим пользователем, имеются привязки к другим объектам или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", locations.Count == 1 ? "адреса" : "адресов"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        /// <summary>
        /// Получение всех местоположений.
        /// </summary>
        /// <returns>Местоположения.</returns>
        public List<Location> GetAll()
        {
            var res = new List<Location>();

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
                    MessageBox.Show("Ошибка при получении адресов.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        #endregion //IBaseModel
    }
}
