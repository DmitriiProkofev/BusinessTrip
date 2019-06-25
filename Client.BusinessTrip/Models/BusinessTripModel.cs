using Client.BusinessTrip.IModels;
using System;
using System.Collections.Generic;
using NLog;
using System.Windows.Forms;
using System.Data.Entity.Infrastructure;
using Core.BusinessTrip.DataInterfaces;
using Client.BusinessTrip.IoC;
using Core.BusinessTrip.Domain;
using Client.BusinessTrip.Properties;
using Core.BusinessTrip.ProjectBase.Utils.AsyncWorking;

namespace Client.BusinessTrip.Models
{
    /// <summary>
    /// Класс модели "Командировка".
    /// </summary>
    public class BusinessTripModel : IBusinessTripModel
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Командировки";

        //Лог.
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IExtendedRepositoryBusinessTrip _repository;

        #endregion //Private Fields and Consts

        #region Lazy Properties

        /// <summary>
        /// Ленивое свойство для репозитория.
        /// </summary>
        private IExtendedRepositoryBusinessTrip Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = CompositionRoot.Resolve<IExtendedRepositoryBusinessTrip>();
                }

                _repository.ConnectionString = Resources.ConnectionStringDomain;

                return _repository;
            }
        }

        #endregion

        #region IBusinessTripModel

        /// <summary>
        /// Получение направлений по Ид командировки.
        /// </summary>
        /// <param name="businessTripId">Ид командировки.</param>
        /// <returns>Список направлений.</returns>
        public List<Direction> GetDirectionsByBusinessTripId(int businessTripId)
        {
            var res = new List<Direction>();

            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    res = Repository.GetDirectionsByBusinessTripId(businessTripId);

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
                    MessageBox.Show("Ошибка при получении направлений.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");
            return res;
        }

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        public int? SaveBusinessTrip(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var businessTripSave = Repository.SaveBusinessTrip(businessTrip);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return businessTripSave.BusinessTripId;
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
                MessageBox.Show("Ошибка при сохранении командировки.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Удаление направлений по Ид.
        /// </summary>
        /// <param name="directionIds">Ид направлений.</param>
        public void DeleteDirectionByIds(List<int> directionIds)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.DeleteDirectionByIds(directionIds);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, directionIds.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Командировка удалена другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", directionIds.Count == 1 ? "направления" : "направлений"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        public Dictionary<string, string> GetTargetsByReasons(List<string> reasons)
        {
            var res = new Dictionary<string, string>();

            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    res = Repository.GetTargetsByReasons(reasons);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Получение данных о задачах ПИР ({1} с.).", DirectoryName, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Ошибка при получении задач по контрактам ПИР", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        public List<PartyPerson> GetPartyPersonsByPartyId(int partyId)
        {
            var res = new List<PartyPerson>();

            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    res = Repository.GetPartyPersonsByPartyId(partyId);

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
                    MessageBox.Show("Ошибка при получении сотрудников партии.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        ///// <summary>
        ///// Обновление командировок и связанных данных.
        ///// </summary>
        ///// <param name="businessTrip">Командировка.</param>
        ///// <param name="AddDirectionIds">Ид новых направлений.</param>
        ///// <param name="DeleteDirectionIds">Ид удаленных направлений.</param>
        //public void UpdateBusinessTrip(Core.BusinessTrip.Domain.BusinessTrip businessTrip, List<int> AddDirectionIds, List<int> DeleteDirectionIds)
        //{
        //    try
        //    {
        //        Repository.UpdateBusinessTrip(businessTrip, AddDirectionIds, DeleteDirectionIds);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        _logger.Error(ex);
        //        MessageBox.Show("Командировка была удалена другим пользователем.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);
        //        MessageBox.Show("Ошибка при обновлении командировки.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        #endregion //IBusinessTripModel

        #region IBaseModel

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        public int? Save(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                var businessTripSave = Repository.Save(businessTrip);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Добавление записи ({1} с.).", DirectoryName, implementationTime.TotalSeconds));

                return businessTripSave.BusinessTripId;
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
                MessageBox.Show("Ошибка при сохранении командировки.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Обновление командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        public void Update(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;

                Repository.Update(businessTrip);

                var timeNew = DateTime.Now.TimeOfDay;
                var implementationTime = timeNew - time;

                _logger.Info(string.Format("{0} | Обновление записи: {1} ({2} с.).", DirectoryName, businessTrip.BusinessTripId, implementationTime.TotalSeconds));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DbUpdateException ex)
            {
                _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Обновление невозможно. Командировка удалена другим пользователем или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при обновлении командировки.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление группы командировок.
        /// </summary>
        /// <param name="businessTrips">Командировки.</param>
        public void Delete(List<Core.BusinessTrip.Domain.BusinessTrip> businessTrips)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                try
                {
                    var time = DateTime.Now.TimeOfDay;

                    Repository.Delete(businessTrips);

                    var timeNew = DateTime.Now.TimeOfDay;
                    var implementationTime = timeNew - time;

                    _logger.Info(string.Format("{0} | Удаление записей, кол-во: {1} ({2} с.).", DirectoryName, businessTrips.Count, implementationTime.TotalSeconds));
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Недостаточный объем памяти.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (DbUpdateException ex)
                {
                    _logger.Warn(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show("Удаление невозможно. Командировка удалена другим пользователем, имеются привязки к другим объектам или отсутствуют права доступа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                    MessageBox.Show(string.Format("Ошибка при удалении {0}.", businessTrips.Count == 1 ? "командировки" : "командировок"), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания удаления...");
        }

        /// <summary>
        /// Получение всех командировок.
        /// </summary>
        /// <returns>Командировки.</returns>
        public List<Core.BusinessTrip.Domain.BusinessTrip> GetAll()
        {
            var res = new List<Core.BusinessTrip.Domain.BusinessTrip>();

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
                    MessageBox.Show("Ошибка при получении командировок.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        public List<Core.BusinessTrip.Domain.BusinessTrip> GetBusinessTripFullByIds(List<int> businessTripIds)
        {
            var res = new List<Core.BusinessTrip.Domain.BusinessTrip>();

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
                    MessageBox.Show("Ошибка при получении командировок.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");

            return res;
        }

        #endregion //IBaseModel
    }
}
