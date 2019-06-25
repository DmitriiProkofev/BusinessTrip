using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IoC;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Client.BusinessTrip.Properties;
using Core.BusinessTrip.Domain;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Справочник направлений".
    /// </summary>
    public partial class DirectoryDirectionsView : Form, IDirectoryDirectionsView
    {
        #region Private Fields 

        private IDirectoryDirectionsPresenter _presenter;

        private IBaseModel<Direction> _model;

        private Direction _direction;

        private Direction _currentDirection;

        private int? _locationId;

        private int? _organizationId;

        private int? _saveIndex = 0;

        //режим редактирования.
        private bool _isEdit = false;

        private int _businessTripId;

        private DialogResult _resultDialog = DialogResult.No;

        #endregion //Private Fields

        #region Constructors

        public DirectoryDirectionsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник направлений".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryDirectionsView(IDirectoryDirectionsPresenter presenter, IBaseModel<Direction> model, int businessTripId)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewDirections)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewDirections, Settings.Default.WindowHeightViewDirections);
            }

            CurrentDirection = new Direction();

            _businessTripId = businessTripId;
            _isEdit = false;
        }

        public DirectoryDirectionsView(IDirectoryDirectionsPresenter presenter, IBaseModel<Direction> model, Direction direction)
            : this()
        {
            _presenter = presenter;
            _model = model;

            _presenter.Init(this, _model);

            SetValues(direction);

            _isEdit = true;
        }

        #endregion //Constructors

        #region IBaseView, IDirectoryDirectionsView

        public int? SaveIndex
        {
            get
            {
                return _saveIndex;
            }
            set
            {
                _saveIndex = value;
            }
        }

        /// <summary>
        /// Направление.
        /// </summary>
        public Direction Direction
        {
            get
            {
                if (_isEdit)
                {
                    _direction = new Direction
                    {
                        DirectionId = CurrentDirection.DirectionId,
                        DateBegin = dateTimePickerDateBegin.Value,
                        DateEnd = dateTimePickerDateEnd.Value,
                        Reason = textBoxReason.Text,
                        LocationId = LocationId,
                        OrganizationId = OrganizationId,
                        BusinessTripId = CurrentDirection.BusinessTripId
                        
                    };
                }
                else
                {
                    _direction = new Direction
                    {
                        DateBegin = dateTimePickerDateBegin.Value,
                        DateEnd = dateTimePickerDateEnd.Value,
                        Reason = textBoxReason.Text,
                        LocationId = LocationId,
                        OrganizationId = OrganizationId,
                        BusinessTripId = _businessTripId
                    };
                }
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        public int? LocationId
        {
            get
            {
                return _locationId;
            }
            set
            {
                _locationId = value;
            }
        }

        /// <summary>
        /// Ид организации
        /// </summary>
        public int? OrganizationId
        {
            get
            {
                return _organizationId;
            }
            set
            {
                _organizationId = value;
            }
        }

        /// <summary>
        /// Выбранное направление.
        /// </summary>
        public Direction CurrentDirection
        {
            get
            {
                return _currentDirection;
            }
            set
            {
                _currentDirection = value;
            }
        }

        /// <summary>
        /// Событие запускает сохранение направления.
        /// </summary>
        public event Action<Direction> Save;

        /// <summary>
        /// Событие запускает обновление направления.
        /// </summary>
        public event Action<Direction> Update;

        /// <summary>
        /// Событие запускает удаление направления.
        /// </summary>
        public event Action<List<Direction>> Delete;

        /// <summary>
        /// Событие запускает получение направлений.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;


        #endregion //IBaseView, IDirectoryDirectionsView

        #region EventHandlers

        private void btnLoc_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryLocationsView>(new IParameter[] { new ConstructorArgument("isStandard", false) });

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentLocation != null)
                {
                    this.LocationId = view.CurrentLocation.LocationId;
                    this.textBoxLocation.Text = view.CurrentLocation.ShortAddress;
                }
                else
                {
                    this.LocationId = null;
                    this.textBoxLocation.Text = "";
                }
            }
        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryOrganizationsView>(new IParameter[] { new ConstructorArgument("isStandard", false) });

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentOrganization != null)
                {
                    this.OrganizationId = view.CurrentOrganization.OrganizationId;
                    this.textBoxOrganization.Text = view.CurrentOrganization.ShortName;
                }
                else
                {
                    this.OrganizationId = null;
                    this.textBoxOrganization.Text = "";
                }
            }
        }

        private void DirectoryDirectionsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        //private void btnDel_Click(object sender, EventArgs e)
        //{
        //    var directions = new List<Direction>();

        //    foreach (DataGridViewRow row in dataGridViewDirections.SelectedRows)
        //    {
        //        var direction = row.DataBoundItem as Direction;
        //        if (direction != null)
        //        {
        //            directions.Add(new Direction
        //            {
        //                DirectionId = direction.DirectionId,
        //                Location = direction.Location,
        //                Organization = direction.Organization,
        //                LocationId = direction.LocationId,
        //                OrganizationId = direction.OrganizationId
        //            });
        //        }
        //    }

        //    if (!directions.Any())
        //        return;

        //    if (MessageBox.Show(directions.Count > 1 ? string.Format("Вы действительно хотите удалить следующие направления: {0}?", string.Join("; ", directions.Select(d => d.ToString()))) :
        //        string.Format("Вы действительно хотите удалить направление: {0}?", string.Join("", directions.Select(d => d.ToString()))),
        //        "Справочник направлений", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {
        //        if (Delete != null)
        //            Delete(directions);

        //        GetAllDirections();
        //    }
        //}

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    if (IsCheck())
        //    {
        //        SaveChange();
        //    }
        //}

        private void btnOK_ButtonClick(object sender, EventArgs e)
        {
            if (IsCheck())
            {
                SaveChange();
                _resultDialog = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_ButtonClick(object sender, EventArgs e)
        {
            _resultDialog = DialogResult.Cancel;
            Close();
        }

        private void dateTimePickerDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerDateBegin.Value > dateTimePickerDateEnd.Value)
            {
                dateTimePickerDateEnd.Value = dateTimePickerDateBegin.Value;
            }

            textBoxDayCount.Text = Math.Round((dateTimePickerDateEnd.Value - dateTimePickerDateBegin.Value).TotalDays + 1).ToString();
        }

        private void dateTimePickerDateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerDateEnd.Value.Date < dateTimePickerDateBegin.Value.Date)
            {
                MessageBox.Show("Дата окончания не может быть меньше даты начала.", "Справочник направлений", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dateTimePickerDateEnd.Value = dateTimePickerDateBegin.Value;
            }

            textBoxDayCount.Text = Math.Round((dateTimePickerDateEnd.Value - dateTimePickerDateBegin.Value).TotalDays + 1).ToString();
        }

        private void btnLocClear_Click(object sender, EventArgs e)
        {
            LocationId = null;
            textBoxLocation.Clear();
        }

        private void btnOrgClear_Click(object sender, EventArgs e)
        {
            OrganizationId = null;
            textBoxOrganization.Clear();
        }

        private void DirectoryDirectionsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsCheck())
                {
                    _resultDialog = DialogResult.OK;

                    Close();
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (IsCheck())
                {
                    _resultDialog = DialogResult.No;

                    Close();
                }
            }
        }

        private void DirectoryDirectionsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewDirections = true;
            else
                Settings.Default.WindowStateMaximizedViewDirections = false;

            Settings.Default.WindowWidthViewDirections = this.Size.Width;
            Settings.Default.WindowHeightViewDirections = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        /// <summary>
        /// Установка полей направления.
        /// </summary>
        /// <param name="direction">Направление.</param>
        private void SetValues(Direction direction)
        {
            CurrentDirection = direction;

            if (direction != null)
            {
                LocationId = direction.LocationId;
                OrganizationId = direction.OrganizationId;

                textBoxLocation.Text = direction.Location != null ? direction.Location.ShortAddress : "";
                textBoxOrganization.Text = direction.Organization != null ? direction.Organization.ShortName : "";

                dateTimePickerDateBegin.Value = direction.DateBegin != null ? (DateTime)direction.DateBegin : DateTime.Now;
                dateTimePickerDateEnd.Value = direction.DateEnd != null ? (DateTime)direction.DateEnd : DateTime.Now;

                textBoxDayCount.Text = Math.Round((dateTimePickerDateEnd.Value - dateTimePickerDateBegin.Value).TotalDays + 1).ToString();

                textBoxReason.Text = direction.Reason;
            }
        }

        /// <summary>
        /// Сохранение изменений.
        /// </summary>
        private void SaveChange()
        {
            if (_isEdit)
            {
                if (Update != null)
                    Update(Direction);
            }
            else
            {
                if (Save != null)
                    Save(Direction);
            }
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        /// <returns>True - успех. False - не пройдена.</returns>
        private bool IsCheck()
        {
            errorProvider.Clear();
            var ok = true;
            //if (string.IsNullOrWhiteSpace(textBoxNumber.Text))
            //{
            //    errorProvider.SetError(textBoxName, "Не задан номер командировки");
            //    ok = false;
            //}
            return ok;
        }



        #endregion

    }
}
