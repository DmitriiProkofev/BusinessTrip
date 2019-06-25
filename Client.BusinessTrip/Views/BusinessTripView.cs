using Client.BusinessTrip.IViews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IoC;
using Ninject.Parameters;
using Core.BusinessTrip.Domain;
using Client.BusinessTrip.Reports.Words;
using Core.BusinessTrip.ProjectBase.Utils.AsyncWorking;
using Cyriller;
using Cyriller.Model;
using Client.BusinessTrip.Helpers;
using Client.BusinessTrip.Helpers.Sortable;
using Client.BusinessTrip.Reports.Excels;
using Core.BusinessTrip.Helpers.DomainHelpers;
using Client.BusinessTrip.Properties;
using System.ComponentModel;
using System.Drawing;
using NLog;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Командировка".
    /// </summary>
    public partial class BusinessTripView : Form, IBusinessTripView
    {
        #region Private Fields 

        private const string DirectoryName = "Оформление командировок";

        private List<string> _listSearch = new List<string> { "Номер", /*"Ид",*/ "Дата", "Автор" };

        private IBusinessTripPresenter _presenter;

        private IBusinessTripModel _model;

        private List<Core.BusinessTrip.Domain.BusinessTrip> _businessTrips;

        private Core.BusinessTrip.Domain.BusinessTrip _businessTrip;

        private Core.BusinessTrip.Domain.BusinessTrip _currentBusinessTrip;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private int? _headStructuralDivisionId;
        private int? _headOrganizationId;
        private int? _authoredId;
        private int? _typeWorkId;
        private int? _partyId;

        private int? _equipmentId;
        private int? _dataId;
        private int? _communicationId;
        private int? _technicalSecurityId;
        private int? _fireSecurityId;
        private int? _industrialSecurityId;
        private int? _rigInspectionId;
        private int? _dealerId;
        private int? _receivingId;
        private int? _monitoringId;
        private int? _informationId;

        private int? _projectManagerId;
        private int? _transportCustomerId;

        private int? _markId;
        private int? _driverNameId;
        private int? _addressId;

        private int _businessTripId;

        private List<Direction> _directions;

        private List<int> _addDirections;

        private List<int> _deleteDirections;

        //режим редактирования.
        private bool _isEdit = false;

        //private bool _saveCheckBoxMileageValue, _saveCheckBoxTimeHour, _saveCheckBoxTimeWork;

        private int? _saveIndex = 0;

        private int _col = -1;
        private int _row = -1;

        private int _colDVGPersons = -1;
        private int _rowDVGPersons = -1;

        private int _colDVGDirections = -1;
        private int _rowDVGDirections = -1;

        private Dictionary<string, string> _targetResons;

        private bool _oneShow = true;

        private List<PartyPerson> _partyPersons;

        #endregion //Private Fields

        #region Constructors

        public BusinessTripView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Командировки".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        public BusinessTripView(IBusinessTripPresenter presenter, IBusinessTripModel model)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewBusinessTrips)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewBusinessTrips, Settings.Default.WindowHeightViewBusinessTrips);
            }

            #region dataGridViewBusinessTrip settings

            //dataGridViewBusinessTrip.Columns.Add("BusinessTripIdColumn", "Ид");
            //dataGridViewBusinessTrip.Columns["BusinessTripIdColumn"].DataPropertyName = "BusinessTripId";
            //dataGridViewBusinessTrip.Columns["BusinessTripIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewBusinessTrip.Columns["BusinessTripIdColumn"].Width = 50;
            //dataGridViewBusinessTrip.Columns["BusinessTripIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridViewBusinessTrip.Columns.Add("NumberDocumentColumn", "Номер");
            dataGridViewBusinessTrip.Columns["NumberDocumentColumn"].DataPropertyName = "NumberDocument";
            dataGridViewBusinessTrip.Columns["NumberDocumentColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewBusinessTrip.Columns["NumberDocumentColumn"].FillWeight = 100;

            dataGridViewBusinessTrip.Columns.Add("DateFormulationToStringColumn", "Дата");
            dataGridViewBusinessTrip.Columns["DateFormulationToStringColumn"].DataPropertyName = "DateFormulationToString";
            dataGridViewBusinessTrip.Columns["DateFormulationToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewBusinessTrip.Columns["DateFormulationToStringColumn"].FillWeight = 100;

            dataGridViewBusinessTrip.Columns.Add("AuthoredToStringColumn", "Автор");
            dataGridViewBusinessTrip.Columns["AuthoredToStringColumn"].DataPropertyName = "AuthoredToString";
            dataGridViewBusinessTrip.Columns["AuthoredToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewBusinessTrip.Columns["AuthoredToStringColumn"].FillWeight = 100;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewBusinessTrip.ContextMenuStrip = contextMenu;

            ContextMenuStrip contextMenuDGVPersons = new ContextMenuStrip();
            ToolStripMenuItem itemDGVPersons = new ToolStripMenuItem("Копировать");
            itemDGVPersons.Click += new EventHandler(mnuCopyDGVPersons_Click);
            contextMenuDGVPersons.Items.AddRange(new ToolStripItem[] { itemDGVPersons });
            dataGridViewPersons.ContextMenuStrip = contextMenuDGVPersons;

            ContextMenuStrip contextMenuDGVDirections = new ContextMenuStrip();
            ToolStripMenuItem itemDGVDirections = new ToolStripMenuItem("Копировать");
            itemDGVDirections.Click += new EventHandler(mnuCopyDGVDirections_Click);
            contextMenuDGVDirections.Items.AddRange(new ToolStripItem[] { itemDGVDirections });
            dataGridViewDirections.ContextMenuStrip = contextMenuDGVDirections;

            dataGridViewDirections.DoubleBuffered(true);
            dataGridViewPersons.DoubleBuffered(true);
            dataGridViewBusinessTrip.DoubleBuffered(true);

            #endregion //dataGridViewBusinessTrip settings

            //dataGridViewPersons.Columns["isResponsibleDataGridViewCheckBoxColumn"].E

            this.DoubleBuffered(true);

            btnSave.Enabled = false;
            TabMain.SelectedTab = TabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (BusinessTrips != null)
            {
                if (BusinessTrips.Count > 0)
                {
                    CurrentBusinessTrip = BusinessTrips[0];
                }
            }

            //// Создаем коллекцию всех существительных.
            //CyrNounCollection nouns = new CyrNounCollection();

            //// Создаем коллекцию всех прилагательных.
            //CyrAdjectiveCollection adjectives = new CyrAdjectiveCollection();

            //// Создаем фразу с использование созданных коллекций.
            //CyrPhrase phrase = new CyrPhrase(nouns, adjectives);

            //// Склоняем словосочетание "быстрый компьютер" в единственном числе используя точное совпадение при поиске слов.
            //CyrResult singular = phrase.Decline("быстрый компьютер", GetConditionsEnum.Strict);

            //txtSearch.Text = singular.Родительный;
        }

        #endregion //Constructors

        #region IBaseView, IBusinessTripView

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
        /// Список командировок.
        /// </summary>
        public List<Core.BusinessTrip.Domain.BusinessTrip> BusinessTrips
        {
            get
            {
                return _businessTrips;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewBusinessTrip.SelectionChanged -= dataGridViewBusinessTrip_SelectionChanged;

                    dataGridViewBusinessTrip.AutoGenerateColumns = false;
                    SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip> businessTripBindingList = new SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip>(value);
                    dataGridViewBusinessTrip.DataSource = businessTripBindingList;

                    if (dataGridViewBusinessTrip.ColumnCount > Settings.Default.SortIndexBusinessTrips)
                    {
                        dataGridViewBusinessTrip.Sort(dataGridViewBusinessTrip.Columns[Settings.Default.SortIndexBusinessTrips],
                            Settings.Default.SortOrderBusinessTrips ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (businessTripBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _businessTrips = value;

                    dataGridViewBusinessTrip.SelectionChanged += dataGridViewBusinessTrip_SelectionChanged;

                    //dataGridViewBusinessTrip.SelectionChanged -= dataGridViewBusinessTrip_SelectionChanged;

                    //dataGridViewBusinessTrip.AutoGenerateColumns = false;
                    //SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip> businessTripBindingList = new SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip>(value);
                    //dataGridViewBusinessTrip.DataSource = businessTripBindingSource;

                    //_businessTrips = value;

                    //dataGridViewBusinessTrip.SelectionChanged += dataGridViewBusinessTrip_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Ид руководителя структурного подразделения.
        /// </summary>
        public int? HeadStructuralDivisionId
        {
            get { return _headStructuralDivisionId; }
            set { _headStructuralDivisionId = value; }
        }

        /// <summary>
        /// Ид главы организации.
        /// </summary>
        public int? HeadOrganizationId
        {
            get { return _headOrganizationId; }
            set { _headOrganizationId = value; }
        }

        /// <summary>
        /// Ид автора отчета.
        /// </summary>
        public int? AuthoredId
        {
            get { return _authoredId; }
            set { _authoredId = value; }
        }

        /// <summary>
        /// Ид вида работ.
        /// </summary>
        public int? TypeWorkId
        {
            get { return _typeWorkId; }
            set { _typeWorkId = value; }
        }

        public int? PartyId
        {
            get { return _partyId; }
            set { _partyId = value; }
        }

        /// <summary>
        /// Ид транспорта.
        /// </summary>
        public int? MarkId
        {
            get { return _markId; }
            set { _markId = value; }
        }

        /// <summary>
        /// Ид водителя.
        /// </summary>
        public int? DriverNameId
        {
            get { return _driverNameId; }
            set { _driverNameId = value; }
        }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        public int? AddressId
        {
            get { return _addressId; }
            set { _addressId = value; }
        }

        //public List<TypeWork> TypeWorks
        //{
        //    get
        //    {
        //        return _typeWorks;
        //    }
        //    set
        //    {
        //        if (value!=null)
        //        {
        //            bindingSourceTypeWorks.DataSource = value;
        //            bindingSourceTypeWorks.ResetBindings(true);
        //            _typeWorks = value;
        //        }
        //        else
        //        {
        //            _typeWorks = new List<TypeWork>();
        //        }
        //    }
        //}

        public List<Direction> Directions
        {
            get
            {
                if (_directions != null)
                {
                    lblNumPer.Text = string.Format("{0} напр.", _directions.Count.ToString());
                }

                return _directions;
            }
            set
            {
                if (value != null)
                {
                    DirectionComparer comparer = new DirectionComparer();
                    value.Sort(comparer);
                    bindingSourceDirections.DataSource = value;
                    bindingSourceDirections.ResetBindings(true);
                    _directions = value;
                }
                else
                {
                    bindingSourceDirections.DataSource = null;
                    bindingSourceDirections.ResetBindings(true);
                    _directions = new List<Direction>();
                }
            }
        }

        public List<PartyPerson> PartyPersons
        {
            get
            {
                return _partyPersons;
            }
            set
            {
                if (value != null)
                {
                    PartyPersonComparer comparer = new PartyPersonComparer();
                    value.Sort(comparer);
                    bindingSourcePersons.DataSource = value;
                    bindingSourcePersons.ResetBindings(true);

                    //if (dataGridViewPersons.ColumnCount > 0 && dataGridViewPersons != null)
                    //{
                    //    dataGridViewPersons.Sort(dataGridViewPersons.Columns[0], ListSortDirection.Ascending);
                    //}
                    _partyPersons = value;
                }
                else
                {
                    bindingSourcePersons.DataSource = null;
                    bindingSourcePersons.ResetBindings(true);
                    _partyPersons = new List<PartyPerson>();
                }
            }
        }

        //public List<int> AddDirections
        //{
        //    get
        //    {
        //        return _addDirections;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _addDirections = value;
        //        }
        //        else
        //        {
        //            _addDirections = new List<int>();
        //        }
        //    }
        //}

        //public List<int> DeleteDirections
        //{
        //    get
        //    {
        //        return _deleteDirections;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _deleteDirections = value;
        //        }
        //        else
        //        {
        //            _deleteDirections = new List<int>();
        //        }
        //    }
        //}

        public Core.BusinessTrip.Domain.BusinessTrip BusinessTrip
        {
            get
            {
                if (_isEdit)
                {
                    _businessTrip = new Core.BusinessTrip.Domain.BusinessTrip
                    {
                        //TODO: не все поля.
                        BusinessTripId = CurrentBusinessTrip.BusinessTripId,
                        NumberDocument = textBoxNumber.Text,
                        DateFormulation = dateTimePickerDate.Value,
                        Target = textBoxTarget.Text,
                        HeadStructuralDivisionId = HeadStructuralDivisionId,
                        HeadOrganizationId = HeadOrganizationId,
                        AuthoredId = AuthoredId,
                        TypeWorkId = TypeWorkId,
                        PartyId = PartyId,
                        IsHoliday = checkBoxIsHoliday.Checked,
                        IsOrenburgWork = checkBoxIsOrenburgWork.Checked,
                        ProxyHeadSD = textBoxProxyHeadSD.Text,
                        ProxyHeadO = textBoxProxyHeadO.Text,
                        Command = new Command
                        {
                            ResponsibleForEquipmentId = EquipmentId,
                            ResponsibleForDataId = DataId,
                            ResponsibleForCommunicationId = CommunicationId,
                            ResponsibleForTechnicalSecurityId = TechnicalSecurityId,
                            ResponsibleForFireSecurityId = FireSecurityId,
                            ResponsibleForIndustrialSecurityId = IndustrialSecurityId,
                            ResponsibleForRigInspectionId = RigInspectionId,
                            ResponsibleForDealerId = DealerId,
                            ResponsibleForReceivingId = ReceivingId,
                            ResponsibleForMonitoringId = MonitoringId,
                            ResponsibleForInformationId = InformationId
                            //DateBegin = dateTimePickerDateBegin.Value,
                            //DateEnd = dateTimePickerDateEnd.Value
                        },
                        RequestTransport = new RequestTransport
                        {
                            ProjectManagerID = ProjectManagerId,
                            //TransportCustomerID = TransportCustomerId,
                            TransportId = MarkId,
                            DriverId = DriverNameId,
                            AddressId = AddressId,
                            Mileage = checkBoxMileage.Checked  ? (double?)numericUpDownMileage.Value : null,
                            Date = dateTimePickerDateRT.Value,
                            TimeWork = checkBoxTimeWork.Checked ? (double?)numericUpDownTimeWork.Value : null,
                            TimeHour = checkBoxTimeHour.Checked ? (double?)numericUpDownTimeHour.Value : null,
                            //ContactInformation = string.Format("{0}|{1}",textBoxNameContact.Text,textBoxPhoneContact.Text),
                            DateFormulation = dateTimePickerDateFormulation.Value
                        }
                        
                    };
                }
                else
                {
                    _businessTrip = new Core.BusinessTrip.Domain.BusinessTrip
                    {
                        //TODO: не все поля.
                        NumberDocument = textBoxNumber.Text,
                        DateFormulation = dateTimePickerDate.Value,
                        Target = textBoxTarget.Text,
                        HeadStructuralDivisionId = HeadStructuralDivisionId,
                        HeadOrganizationId = HeadOrganizationId,
                        AuthoredId = AuthoredId,
                        TypeWorkId = TypeWorkId,
                        PartyId = PartyId,
                        IsHoliday = checkBoxIsHoliday.Checked,
                        IsOrenburgWork = checkBoxIsOrenburgWork.Checked,
                        ProxyHeadSD = textBoxProxyHeadSD.Text,
                        ProxyHeadO = textBoxProxyHeadO.Text,
                        Command = new Command
                        {
                            ResponsibleForEquipmentId = EquipmentId,
                            ResponsibleForDataId = DataId,
                            ResponsibleForCommunicationId = CommunicationId,
                            ResponsibleForTechnicalSecurityId = TechnicalSecurityId,
                            ResponsibleForFireSecurityId = FireSecurityId,
                            ResponsibleForIndustrialSecurityId = IndustrialSecurityId,
                            ResponsibleForRigInspectionId = RigInspectionId,
                            ResponsibleForDealerId = DealerId,
                            ResponsibleForReceivingId = ReceivingId,
                            ResponsibleForMonitoringId = MonitoringId,
                            ResponsibleForInformationId = InformationId,
                            //DateBegin = dateTimePickerDateBegin.Value,
                            //DateEnd = dateTimePickerDateEnd.Value
                        },
                        RequestTransport = new RequestTransport
                        {
                            ProjectManagerID = ProjectManagerId,
                            //TransportCustomerID = TransportCustomerId,
                            TransportId = MarkId,
                            DriverId = DriverNameId,
                            AddressId = AddressId,
                            Mileage = checkBoxMileage.Checked ? (double?)numericUpDownMileage.Value : null,
                            Date = dateTimePickerDateRT.Value,
                            TimeWork = checkBoxTimeWork.Checked ? (double?)numericUpDownTimeWork.Value : null,
                            TimeHour = checkBoxTimeHour.Checked ? (double?)numericUpDownTimeHour.Value : null,
                            //ContactInformation = string.Format("{0}|{1}", textBoxNameContact.Text, textBoxPhoneContact.Text),
                            DateFormulation = dateTimePickerDateFormulation.Value
                        }

                    };
                }

                return _businessTrip;
            }
            set
            {
                _businessTrip = value;
            }
        }

        public Core.BusinessTrip.Domain.BusinessTrip CurrentBusinessTrip
        {
            get { return _currentBusinessTrip; }
            set { _currentBusinessTrip = value; }
        }

        /// <summary>
        /// Ид ответственного за оборудование.
        /// </summary>
        public int? EquipmentId
        {
            get { return _equipmentId; }
            set { _equipmentId = value; }
        }

        /// <summary>
        /// Ид ответственного за данные. 
        /// </summary>
        public int? DataId
        {
            get { return _dataId; }
            set { _dataId = value; }
        }

        /// <summary>
        /// Ид ответственного за связь.
        /// </summary>
        public int? CommunicationId
        {
            get { return _communicationId; }
            set { _communicationId = value; }
        }

        /// <summary>
        /// Ид ответственного за техническую безопасность.
        /// </summary>
        public int? TechnicalSecurityId
        {
            get { return _technicalSecurityId; }
            set { _technicalSecurityId = value; }
        }

        /// <summary>
        /// Ид ответственного за пожарную безопасность.
        /// </summary>
        public int? FireSecurityId
        {
            get { return _fireSecurityId; }
            set { _fireSecurityId = value; }
        }

        /// <summary>
        /// Ид ответственного за промышленную безопасность.
        /// </summary>
        public int? IndustrialSecurityId
        {
            get { return _industrialSecurityId; }
            set { _industrialSecurityId = value; }
        }

        /// <summary>
        /// Ид ответственного за осмотр буровой.
        /// </summary>
        public int? RigInspectionId
        {
            get { return _rigInspectionId; }
            set { _rigInspectionId = value; }
        }

        /// <summary>
        /// Ид ответственного за сдачу материала.
        /// </summary>
        public int? DealerId
        {
            get { return _dealerId; }
            set { _dealerId = value; }
        }

        /// <summary>
        /// Ид ответственного за прием материала.
        /// </summary>
        public int? ReceivingId
        {
            get { return _receivingId; }
            set { _receivingId = value; }
        }

        /// <summary>
        /// Ид ответственного за контроль выполнения приказа.
        /// </summary>
        public int? MonitoringId
        {
            get { return _monitoringId; }
            set { _monitoringId = value; }
        }

        /// <summary>
        /// Ид ответственного за оповещение.
        /// </summary>
        public int? InformationId
        {
            get { return _informationId; }
            set { _informationId = value; }
        }

        public int? ProjectManagerId
        {
            get { return _projectManagerId; }
            set { _projectManagerId = value; }
        }

        public int? TransportCustomerId
        {
            get { return _transportCustomerId; }
            set { _transportCustomerId = value; }
        }

        public Dictionary<string, string> TargetReasons
        {
            get
            {
                return _targetResons;
            }
            set
            {
                _targetResons = value;
            }
        }

        /// <summary>
        /// Событие запускает удаление направление
        /// </summary>
        public event Action<List<int>> DeleteDirectionByIds;

        /// <summary>
        /// Событие запускает сохранение командировки.
        /// </summary>
        public event Action<Core.BusinessTrip.Domain.BusinessTrip> SaveBusinessTrip;

        /// <summary>
        /// Событие запускает получение направлений по ид командировки.
        /// </summary>
        public event Action<int> GetDirectionsByBusinessTripId;

        /// <summary>
        /// Событие запускает сохранение командировки.
        /// </summary>
        public event Action<Core.BusinessTrip.Domain.BusinessTrip> Save;

        /// <summary>
        /// Событие запускает обновление коандировки.
        /// </summary>
        public event Action<Core.BusinessTrip.Domain.BusinessTrip> Update;

        /// <summary>
        /// Событие запускает удаление командировки.
        /// </summary>
        public event Action<List<Core.BusinessTrip.Domain.BusinessTrip>> Delete;

        /// <summary>
        /// Событие запускает получение Командировок.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        public event Action<List<int>> ExportToDoc;

        public event Action<List<string>> GetTargetsByReasons;

        public event Action<int> GetPartyPersonsByPartyId;

        #endregion //IBaseView, IBusinessTripView

        #region EventHandlers

        private void btnTypeWork_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryTypeWorksView>(new IParameter[] { new ConstructorArgument("isStandard", false) });

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentTypeWork != null)
                {
                    TypeWorkId = view.CurrentTypeWork.TypeWorkId;
                    textBoxTypeWork.Text = view.CurrentTypeWork.Name;
                }
                else
                {
                    TypeWorkId = null;
                    textBoxTypeWork.Text = null;
                }
            }
        }

        private void btnHeadStr_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>(); //new IParameter[] { new ConstructorArgument("isStandard", false) }

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    this.HeadStructuralDivisionId = view.CurrentPerson.PersonId;
                    this.textBoxHeadStr.Text = view.CurrentPerson.Name;
                }
                else
                {
                    this.HeadStructuralDivisionId = null;
                    this.textBoxHeadStr.Text = "";
                }
            }
        }

        private void btnHeadOrg_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    this.HeadOrganizationId = view.CurrentPerson.PersonId;
                    this.textBoxHeadOrg.Text = view.CurrentPerson.Name;
                }
                else
                {
                    this.HeadOrganizationId = null;
                    this.textBoxHeadOrg.Text = "";
                }
            }
        }

        private void btnAuthored_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    this.AuthoredId = view.CurrentPerson.PersonId;
                    this.textBoxAuthored.Text = view.CurrentPerson.Name;
                }
                else
                {
                    this.AuthoredId = null;
                    this.textBoxAuthored.Text = "";
                }
            }
        }



        private void tsmPersons_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();
            view.ShowDialog();
            if (CurrentBusinessTrip != null)
            {
                if (CurrentBusinessTrip.Party != null)
                {
                    if (GetPartyPersonsByPartyId != null)
                        GetPartyPersonsByPartyId(CurrentBusinessTrip.Party.PartyId);
                }
                else
                {
                    PartyPersons = null;
                }
            }
        }

        private void tsmLocation_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryLocationsView>();
            view.ShowDialog();
        }

        private void tsmOrganisations_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryOrganizationsView>();
            view.ShowDialog();
        }

        private void DirectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryDirectionsView>();
            view.ShowDialog();
        }

        private void tsmWorkType_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryTypeWorksView>();
            view.ShowDialog();
        }

        private void departmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryDepartmentsView>();
            view.ShowDialog();
        }

        private void positionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPositionsView>();
            view.ShowDialog();
        }

        private void transportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryTransportsView>();
            view.ShowDialog();
        }

        private void tsmAuto_Click(object sender, EventArgs e)
        {
            //var view = CompositionRoot.Resolve<DirectoryTransportsView>();
            //view.ShowDialog();
        }

        private void btnAddDirection_Click(object sender, EventArgs e)
        {
            if (CurrentBusinessTrip != null)
            {
                if (btnSave.Enabled == false)
                {
                    var view = CompositionRoot.Resolve<DirectoryDirectionsView>(new IParameter[] { new ConstructorArgument("businessTripId", CurrentBusinessTrip.BusinessTripId) });

                    if (view.ShowDialog() == DialogResult.OK)
                    {
                        //SavePosition();
                        GetAllBusinessTrips();
                        var businessTripId = CurrentBusinessTrip.BusinessTripId;
                        //SelectObject();
                        dataGridViewBusinessTrip.ClearSelection();

                        foreach (DataGridViewRow row in dataGridViewBusinessTrip.Rows)
                        {
                            var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                            if (businessTrip != null)
                            {
                                if (businessTrip.BusinessTripId == businessTripId)
                                {
                                    dataGridViewBusinessTrip.Rows[row.Index].Selected = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Направление добавляется для существующей командировки. Сохранить командировку?",
                        "Оформление командировок", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (IsCheck())
                        {
                            SaveChange();

                            var view = CompositionRoot.Resolve<DirectoryDirectionsView>(new IParameter[] { new ConstructorArgument("businessTripId", CurrentBusinessTrip.BusinessTripId) });

                            if (view.ShowDialog() == DialogResult.OK)
                            {
                                //SavePosition();
                                GetAllBusinessTrips();
                                //SelectObject();
                                var businessTripId = CurrentBusinessTrip.BusinessTripId;

                                dataGridViewBusinessTrip.ClearSelection();

                                foreach (DataGridViewRow row in dataGridViewBusinessTrip.Rows)
                                {
                                    var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                                    if (businessTrip != null)
                                    {
                                        if (businessTrip.BusinessTripId == businessTripId)
                                        {
                                            dataGridViewBusinessTrip.Rows[row.Index].Selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Добавление направления невозможно. Список командировок пуст.",
                        DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewDirections_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentBusinessTrip != null)
            {
                SavePosition();

                DataGridViewRow row = dataGridViewDirections.Rows[e.RowIndex];
                var direction = row.DataBoundItem as Direction;
                if (direction != null)
                {
                    var view = CompositionRoot.Resolve<DirectoryDirectionsView>(new IParameter[] { new ConstructorArgument("direction", direction) });

                    if (view.ShowDialog() == DialogResult.OK)
                    {
                        GetAllBusinessTrips();

                        dataGridViewBusinessTrip.ClearSelection();

                        SelectObject();
                        //dataGridViewBusinessTrip.Rows[].Selected = true;

                    }
                }
            }
            else
            {
                if (MessageBox.Show("Направление добавляется для существующей командировки. Сохранить командировку?",
                    "Оформление командировок", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (IsCheck())
                    {
                        SaveChange();
                    }
                }
            }
        }
        private void btnDelDirection_Click(object sender, EventArgs e)
        {
            SavePosition();

            var directionIds = new List<int>();

            foreach (DataGridViewRow row in dataGridViewDirections.SelectedRows)
            {
                var direction = row.DataBoundItem as Direction;
                if (direction != null)
                {
                    directionIds.Add(direction.DirectionId);
                }
            }

            if (!directionIds.Any())
                return;

            if (DeleteDirectionByIds != null)
                DeleteDirectionByIds(directionIds);

            GetAllBusinessTrips();

            SelectObject();
        }

        private void tsmParty_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPartysView>();
            view.ShowDialog();
            if (CurrentBusinessTrip != null)
            {
                if (CurrentBusinessTrip.Party != null)
                {
                    if (GetPartyPersonsByPartyId != null)
                        GetPartyPersonsByPartyId(CurrentBusinessTrip.Party.PartyId);
                }
                else
                {
                    PartyPersons = null;
                }
            }
        }

        private void btnSetPartyPersons_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPartysView>(new IParameter[] { new ConstructorArgument("isStandard", false) });

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentParty != null)
                {
                    PartyId = view.CurrentParty.PartyId;
                    textBoxParty.Text = view.CurrentParty.Name;

                    //PartyPersons = view.CurrentParty.PartyPersons.ToList();

                    //if (persons.Count() != 0)
                    //{
                    //    bindingSourcePersons.DataSource = persons;
                    //}
                    //else
                    //{
                    //    bindingSourcePersons.DataSource = null;
                    //}
                    //bindingSourcePersons.ResetBindings(true);
                }
                else
                {
                    PartyId = null;
                    textBoxParty.Text = "";
                }
            }
            if (view.CurrentParty != null)
            {
                if (GetPartyPersonsByPartyId != null)
                    GetPartyPersonsByPartyId(view.CurrentParty.PartyId);
            }
            else
            {
                PartyPersons = null;
            }

            //if (CurrentBusinessTrip != null)
            //{
            //    if (CurrentBusinessTrip.Party != null)
            //    {
            //        PartyPersons = CurrentBusinessTrip.Party.PartyPersons.ToList();
            //    }
            //    else
            //    {
            //        PartyPersons = null;
            //    }
            //}
        }

        private void BusinessTripView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void BusinessTripView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabMain.SelectedTab = TabBusinessTrip;
            CurrentBusinessTrip = null;
            ClearForm();

            checkBoxMileage.Checked = true;
            checkBoxTimeHour.Checked = true;
            checkBoxTimeWork.Checked = true;

            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewBusinessTrip.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewBusinessTrip.SelectedRows[countRow - 1];
                var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                if (businessTrip != null)
                {
                    TabMain.SelectedTab = TabBusinessTrip;
                    CurrentBusinessTrip = null;
                    Directions.Clear();
                    dataGridViewDirections.Rows.Clear();
                    SetButtonEnabled(false);
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeleteBusinessTrips();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsCheck())
            {
                SaveChange();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            int indexSelect, indexPrev;
            var countRow = dataGridViewBusinessTrip.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewBusinessTrip.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewBusinessTrip.ClearSelection();
                    dataGridViewBusinessTrip.Rows[indexPrev].Selected = true;
                    dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewBusinessTrip.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewBusinessTrip.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewBusinessTrip.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewBusinessTrip.ClearSelection();
                    dataGridViewBusinessTrip.Rows[indexNext].Selected = true;
                    dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, indexNext];
                }
            }
        }

        private void btnCancel1_ButtonClick(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();

            Close();
        }

        private void dataGridViewBusinessTrip_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewBusinessTrip.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewBusinessTrip.SelectedRows[countRow - 1];
                var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                if (businessTrip != null)
                {
                    SetValues(businessTrip);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxHeadStr_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxHeadOrg_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxAuthored_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxNumber_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void dateTimePickerDate_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxTypeWork_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxParty_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxEqipment_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxCommunication_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxTechnicalSecurity_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxFireSecurity_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxIndustrialSecurity_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxRigInspection_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxDealer_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxReceiving_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxMonitoring_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxDovInformation_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void numericUpDownMileage_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void numericUpDownTimeHour_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void numericUpDownTimeWork_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxTransportCustomer_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxProjectManager_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxMark_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxDriverName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void dateTimePickerDateRT_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxNameContact_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxPhoneContact_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void dateTimePickerDateFormulation_ValueChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void dateTimePickerDateBegin_ValueChanged(object sender, EventArgs e)
        {
            //SetStateForChange();
        }

        private void checkBoxIsHoliday_CheckedChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void checkBoxIsOrenburgWork_CheckedChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxProxyHeadSD_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxProxyHeadO_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void TabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = e.TabPageIndex == 1;
            }
        }

        private void btnHeadStrClear_Click(object sender, EventArgs e)
        {
            HeadStructuralDivisionId = null;
            textBoxHeadStr.Clear();
        }

        private void btnHeadOrgClear_Click(object sender, EventArgs e)
        {
            HeadOrganizationId = null;
            textBoxHeadOrg.Clear();
        }

        private void btnAuthoredClear_Click(object sender, EventArgs e)
        {
            AuthoredId = null;
            textBoxAuthored.Clear();
        }

        private void btnPartyPersonsClear_Click(object sender, EventArgs e)
        {
            PartyId = null;
            textBoxParty.Clear();
            dataGridViewPersons.Rows.Clear();
        }

        private void btnClearTypeWork_Click(object sender, EventArgs e)
        {
            TypeWorkId = null;
            textBoxTypeWork.Clear();
        }

        private void btnEqipmentId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    EquipmentId = view.CurrentPerson.PersonId;
                    textBoxEqipment.Text = view.CurrentPerson.Name;
                }
                else
                {
                    EquipmentId = null;
                    textBoxEqipment.Text = "";
                }
            }
        }

        private void btnDataId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    DataId = view.CurrentPerson.PersonId;
                    textBoxData.Text = view.CurrentPerson.Name;
                }
                else
                {
                    DataId = null;
                    textBoxData.Text = "";
                }
            }
        }

        private void btnCommunicationId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    CommunicationId = view.CurrentPerson.PersonId;
                    textBoxCommunication.Text = view.CurrentPerson.Name;
                }
                else
                {
                    CommunicationId = null;
                    textBoxCommunication.Text = "";
                }
            }
        }

        private void btnTechnicalSecurityId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    TechnicalSecurityId = view.CurrentPerson.PersonId;
                    textBoxTechnicalSecurity.Text = view.CurrentPerson.Name;
                }
                else
                {
                    TechnicalSecurityId = null;
                    textBoxTechnicalSecurity.Text = "";
                }
            }
        }

        private void btnFireSecurityId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    FireSecurityId = view.CurrentPerson.PersonId;
                    textBoxFireSecurity.Text = view.CurrentPerson.Name;
                }
                else
                {
                    FireSecurityId = null;
                    textBoxFireSecurity.Text = "";
                }
            }
        }

        private void btnIndustrialSecurityId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    IndustrialSecurityId = view.CurrentPerson.PersonId;
                    textBoxIndustrialSecurity.Text = view.CurrentPerson.Name;
                }
                else
                {
                    IndustrialSecurityId = null;
                    textBoxIndustrialSecurity.Text = "";
                }
            }
        }

        private void btnRigInspectionId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    RigInspectionId = view.CurrentPerson.PersonId;
                    textBoxRigInspection.Text = view.CurrentPerson.Name;
                }
                else
                {
                    RigInspectionId = null;
                    textBoxRigInspection.Text = "";
                }
            }
        }

        private void btnDealerId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    DealerId = view.CurrentPerson.PersonId;
                    textBoxDealer.Text = view.CurrentPerson.Name;
                }
                else
                {
                    DealerId = null;
                    textBoxDealer.Text = "";
                }
            }
        }

        private void btnReceivingId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    ReceivingId = view.CurrentPerson.PersonId;
                    textBoxReceiving.Text = view.CurrentPerson.Name;
                }
                else
                {
                    ReceivingId = null;
                    textBoxReceiving.Text = "";
                }
            }
        }

        private void btnMonitoringId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    MonitoringId = view.CurrentPerson.PersonId;
                    textBoxMonitoring.Text = view.CurrentPerson.Name;
                }
                else
                {
                    MonitoringId = null;
                    textBoxMonitoring.Text = "";
                }
            }
        }

        private void btnDovInformationId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    InformationId = view.CurrentPerson.PersonId;
                    textBoxDovInformation.Text = view.CurrentPerson.Name;
                }
                else
                {
                    InformationId = null;
                    textBoxDovInformation.Text = "";
                }
            }
        }

        private void btnCustomerId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    TransportCustomerId = view.CurrentPerson.PersonId;
                    //textBoxTransportCustomer.Text = view.CurrentPerson.Name;
                }
                else
                {
                    TransportCustomerId = null;
                    //textBoxTransportCustomer.Text = "";
                }
            }
        }

        private void btnProjectManagerId_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    ProjectManagerId = view.CurrentPerson.PersonId;
                    textBoxProjectManager.Text = view.CurrentPerson.Name;
                }
                else
                {
                    ProjectManagerId = null;
                    textBoxProjectManager.Text = "";
                }
            }
        }

        private void btnCustomerIdClear_Click(object sender, EventArgs e)
        {
            TransportCustomerId = null;
            //textBoxTransportCustomer.Clear();
        }

        private void btnbtnProjectManagerIdClear_Click(object sender, EventArgs e)
        {
            ProjectManagerId = null;
            textBoxProjectManager.Clear();
        }

        private void btnEqipmentIdClear_Click(object sender, EventArgs e)
        {
            EquipmentId = null;
            textBoxEqipment.Clear();
        }

        private void btnDataIdClear_Click(object sender, EventArgs e)
        {
            DataId = null;
            textBoxData.Clear();
        }

        private void btnCommunicationIdClear_Click(object sender, EventArgs e)
        {
            CommunicationId = null;
            textBoxCommunication.Clear();
        }

        private void btnTechnicalSecurityIdClear_Click(object sender, EventArgs e)
        {
            TechnicalSecurityId = null;
            textBoxTechnicalSecurity.Clear();
        }

        private void btnFireSecurityIdClear_Click(object sender, EventArgs e)
        {
            FireSecurityId = null;
            textBoxFireSecurity.Clear();
        }

        private void btnIndustrialSecurityIdClear_Click(object sender, EventArgs e)
        {
            IndustrialSecurityId = null;
            textBoxIndustrialSecurity.Clear();
        }

        private void btnRigInspectionIdClear_Click(object sender, EventArgs e)
        {
            RigInspectionId = null;
            textBoxRigInspection.Clear();
        }

        private void btnDealerIdClear_Click(object sender, EventArgs e)
        {
            DealerId = null;
            textBoxDealer.Clear();
        }

        private void btnReceivingIdClear_Click(object sender, EventArgs e)
        {
            ReceivingId = null;
            textBoxReceiving.Clear();
        }

        private void btnMonitoringIdClear_Click(object sender, EventArgs e)
        {
            MonitoringId = null;
            textBoxMonitoring.Clear();
        }

        private void btnDovInformationIdClear_Click(object sender, EventArgs e)
        {
            InformationId = null;
            textBoxDovInformation.Clear();
        }

        private void checkBoxMileage_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMileage.Checked)
            {
                numericUpDownMileage.Enabled = true;
            }
            else
            {
                numericUpDownMileage.Enabled = false;
            }
            SetStateForChange();
        }

        private void checkBoxTimeHour_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTimeHour.Checked)
            {
                numericUpDownTimeHour.Enabled = true;
            }
            else
            {
                numericUpDownTimeHour.Enabled = false;
            }
            SetStateForChange();
        }

        private void checkBoxTimeWork_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTimeWork.Checked)
            {
                numericUpDownTimeWork.Enabled = true;
            }
            else
            {
                numericUpDownTimeWork.Enabled = false;
            }
            SetStateForChange();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (_row < dataGridViewBusinessTrip.RowCount && _col < dataGridViewBusinessTrip.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewBusinessTrip.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void mnuCopyDGVPersons_Click(object sender, EventArgs e)
        {
            if (_rowDVGPersons < dataGridViewPersons.RowCount && _colDVGPersons < dataGridViewPersons.ColumnCount && _rowDVGPersons >= 0 && _colDVGPersons >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewPersons.Rows[_rowDVGPersons].Cells[_colDVGPersons].Value.ToString());
            }
        }

        private void mnuCopyDGVDirections_Click(object sender, EventArgs e)
        {
            if (_rowDVGDirections < dataGridViewDirections.RowCount && _colDVGDirections < dataGridViewDirections.ColumnCount && _rowDVGDirections >= 0 && _colDVGDirections >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewDirections.Rows[_rowDVGDirections].Cells[_colDVGDirections].Value.ToString());
            }
        }

        private void dataGridViewBusinessTrip_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewPersons_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _rowDVGPersons= e.RowIndex;
                _colDVGPersons= e.ColumnIndex;
            }
        }

        private void dataGridViewDirections_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _rowDVGDirections = e.RowIndex;
                _colDVGDirections = e.ColumnIndex;
            }
        }

        private void dataGridViewBusinessTrip_DoubleClick(object sender, EventArgs e)
        {
            TabMain.SelectedTab = TabBusinessTrip;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllBusinessTrips();
            SelectObject();
            ResetLebelCount();
            textBoxSearch.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
            textBoxSearch.Focus();
        }

        private void comboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllBusinessTrips();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewBusinessTrip_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dataGridViewBusinessTrip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabMain.SelectedTab == TabList)
                {
                    DeleteBusinessTrips();
                }
            }
        }

        private void BusinessTripView_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter && btnSave.Enabled == false && TabMain.SelectedTab == TabList)
            //{
            //    if (IsCheckSave())
            //    {
            //        Close();
            //    }
            //}

            if (e.KeyCode == Keys.Escape && btnSave.Enabled == false /*&& TabMain.SelectedTab == TabList*/)
            {
                if (IsCheckSave())
                {
                    Close();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAllBusinessTrips();

            dataGridViewBusinessTrip.ClearSelection();

            if (dataGridViewBusinessTrip.RowCount > 0)
            {
                dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, 0];
                dataGridViewBusinessTrip.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void dateTimePickerDateEnd_ValueChanged(object sender, EventArgs e)
        {
            //if (dateTimePickerDateEnd.Value.Date < dateTimePickerDateBegin.Value.Date)
            //{
            //    MessageBox.Show("Дата окончания не может быть меньше даты начала.", "Оформление командировок", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dateTimePickerDateEnd.Value = dateTimePickerDateBegin.Value;
            //}

            //SetStateForChange();
        }

        private void tsmPrikaz_Click(object sender, EventArgs e)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                var businessTripIds = new List<int>();
                foreach (DataGridViewRow row in dataGridViewBusinessTrip.SelectedRows)
                {
                    var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                    if (businessTrip != null)
                    {
                        if (businessTrip.TypeWork != null)
                        {
                            DecreeToWord.ExportToWord(businessTrip);
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Формирование приказа прервано. Не задан вид работ для командировки № {0}.",businessTrip.NumberDocument), DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");
        }

        private void tsmSZ_Click(object sender, EventArgs e)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                var businessTripIds = new List<int>();
                foreach (DataGridViewRow row in dataGridViewBusinessTrip.SelectedRows)
                {
                    var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                    if (businessTrip != null)
                    {
                        if (GetTargetsByReasons != null)
                            GetTargetsByReasons(Directions.Select(d => d.Reason).ToList());

                        Dictionary<string,string> targets = TargetReasons != null ? TargetReasons : new Dictionary<string, string>();

                        TaskToExcel.ExportToExcel(businessTrip, targets);
                    }
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");
        }

        private void tsmTranspZ_Click(object sender, EventArgs e)
        {
            AsyncLoaderForm.ShowMarquee((s1, s2) =>
            {
                var businessTripIds = new List<int>();
                foreach (DataGridViewRow row in dataGridViewBusinessTrip.SelectedRows)
                {
                    var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                    if (businessTrip != null)
                    {
                        RequestTransportToExcel.ExportToExcel(businessTrip);
                    }
                }
            }, "Пожалуйста, ожидайте окончания загрузки...");
        }

        private void btnSetTarget_Click(object sender, EventArgs e)
        {
            if (GetTargetsByReasons != null)
                GetTargetsByReasons(Directions.Select(d => d.Reason).ToList());

            if (TargetReasons != null)
            {
                var targets = new List<string>();

                foreach (var targetR in TargetReasons)
                {
                    targets.Add(string.Format("{0} {1}", targetR.Key, targetR.Value));
                }

                var targetsReasonsAsString = string.Join(", ", targets);

                if (targetsReasonsAsString.Length > 2000)
                {
                    targetsReasonsAsString = targetsReasonsAsString.Substring(0, 2000);
                }

                textBoxTarget.Text = targetsReasonsAsString;
            }
        }

        private void BusinessTripView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewBusinessTrip_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewBusinessTrip.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewBusinessTrip.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewPersons_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewPersons.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewPersons.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewDirections_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewDirections.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewDirections.Rows[index].HeaderCell.Value = indexStr;
        }

        private void buttonMark_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryTransportsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentTransport != null)
                {
                    MarkId = view.CurrentTransport.TransportId;
                    textBoxMark.Text = view.CurrentTransport.Mark;
                }
                else
                {
                    MarkId = null;
                    textBoxMark.Text = "";
                }
            }
        }

        private void buttonDriverName_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    DriverNameId = view.CurrentPerson.PersonId;
                    textBoxDriverName.Text = view.CurrentPerson.Name;
                }
                else
                {
                    DriverNameId = null;
                    textBoxDriverName.Text = "";
                }
            }
        }

        private void buttonAddress_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryLocationsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentLocation != null)
                {
                    AddressId = view.CurrentLocation.LocationId;
                    textBoxAddress.Text = view.CurrentLocation.ShortAddress;
                }
                else
                {
                    AddressId = null;
                    textBoxAddress.Text = "";
                }
            }
        }

        private void buttonMarkClear_Click(object sender, EventArgs e)
        {
            MarkId = null;
            textBoxMark.Clear();
        }

        private void buttonDriverNameClear_Click(object sender, EventArgs e)
        {
            DriverNameId = null;
            textBoxDriverName.Clear();
        }

        private void buttonAddressClear_Click(object sender, EventArgs e)
        {
            AddressId = null;
            textBoxAddress.Clear();
        }

        private void dataGridViewBusinessTrip_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexBusinessTrips = e.ColumnIndex;

                if (dataGridViewBusinessTrip.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderBusinessTrips = true;
                }
                else if (dataGridViewBusinessTrip.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderBusinessTrips = false;
                }
                else
                {
                    Settings.Default.SortOrderBusinessTrips = true;
                }
            }
            else
            {
                Settings.Default.SortIndexBusinessTrips = 0;
            }
            Settings.Default.Save();
        }

        //private void toolStripButtonPrev_Click(object sender, EventArgs e)
        //{
        //    int indexSelect, indexPrev;
        //    var countRow = dataGridViewDirections.SelectedRows.Count;

        //    if (countRow > 0)
        //    {
        //        indexSelect = dataGridViewDirections.SelectedRows[0].Index;

        //        if (indexSelect - 1 >= 0)
        //        {
        //            var directionSelect = dataGridViewDirections.Rows[indexSelect].DataBoundItem as Direction;
        //            indexPrev = indexSelect - 1;
        //            var directionPrev = dataGridViewDirections.Rows[indexPrev].DataBoundItem as Direction;


        //            //dataGridViewDirections.Rows[indexPrev].
        //            //dataGridViewDirections.Rows[indexSelect].SetValues(directionPrev);

        //            dataGridViewDirections.ClearSelection();
        //            dataGridViewDirections.Rows[indexPrev].Selected = true;
        //        }
        //    }
        //}

        //private void toolStripButtonNext_Click(object sender, EventArgs e)
        //{
        //    int indexSelect, indexNext;
        //    var countRow = dataGridViewDirections.SelectedRows.Count;

        //    if (countRow > 0)
        //    {
        //        indexSelect = dataGridViewDirections.SelectedRows[0].Index;

        //        if (indexSelect + 1 <= dataGridViewDirections.Rows.Count - 1)
        //        {
        //            indexNext = indexSelect + 1;
        //            dataGridViewDirections.ClearSelection();
        //            dataGridViewDirections.Rows[indexNext].Selected = true;
        //        }
        //    }
        //}

        private void BusinessTripView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewBusinessTrips = true;
            else
                Settings.Default.WindowStateMaximizedViewBusinessTrips = false;

            Settings.Default.WindowWidthViewBusinessTrips = this.Size.Width;
            Settings.Default.WindowHeightViewBusinessTrips = this.Size.Height;

            Settings.Default.Save();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutTheProgramView aboutView = new AboutTheProgramView();
            aboutView.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Doc\About\Инструкция к программе - командировки.docx");
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при получении справки.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteBusinessTrips()
        {
            SavePosition(true, true);

            var businessTrips = new List<Core.BusinessTrip.Domain.BusinessTrip>();

            foreach (DataGridViewRow row in dataGridViewBusinessTrip.SelectedRows)
            {
                var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                if (businessTrip != null)
                {
                    businessTrips.Add(new Core.BusinessTrip.Domain.BusinessTrip { BusinessTripId = businessTrip.BusinessTripId });
                }
            }

            if (!businessTrips.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить командировки (кол-во: {0})?", businessTrips.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(businessTrips);

                GetAllBusinessTrips();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewBusinessTrip.ClearSelection();

            if (dataGridViewBusinessTrip.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewBusinessTrip.Rows)
                    {
                        var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                        if (businessTrip != null)
                        {
                            if (businessTrip.BusinessTripId == SaveIndex)
                            {
                                dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, row.Index];
                                dataGridViewBusinessTrip.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, 0];
                dataGridViewBusinessTrip.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewBusinessTrip.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewBusinessTrip.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewBusinessTrip.SelectedRows.Count < dataGridViewBusinessTrip.RowCount)
                    {
                        var selectedRow = dataGridViewBusinessTrip.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewBusinessTrip.SelectedRows[0].Index == dataGridViewBusinessTrip.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewBusinessTrip.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewBusinessTrip.RowCount)
                            {
                                row = dataGridViewBusinessTrip.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewBusinessTrip.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewBusinessTrip.RowCount)
                            {
                                row = dataGridViewBusinessTrip.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                            if (businessTrip != null)
                            {
                                SaveIndex = businessTrip.BusinessTripId;
                            }
                            else
                            {
                                SaveIndex = null;
                            }
                        }
                        else
                        {
                            SaveIndex = null;
                        }
                    }
                    else
                    {
                        SaveIndex = null;
                    }
                }
                else
                {
                    var row = dataGridViewBusinessTrip.SelectedRows[selectIndex];
                    var businessTrip = row.DataBoundItem as Core.BusinessTrip.Domain.BusinessTrip;
                    if (businessTrip != null)
                    {
                        SaveIndex = businessTrip.BusinessTripId;
                    }
                    else
                    {
                        SaveIndex = null;
                    }
                }
            }
            else
            {
                SaveIndex = null;
            }
        }

        /// <summary>
        /// Переход в состояние редактирования, либо выход из него.
        /// </summary>
        private void SetStateForChange()
        {
            if (CurrentBusinessTrip != null)
            {
                var tempHeadStr = CurrentBusinessTrip.HeadStructuralDivisionId != null ? CurrentBusinessTrip.HeadStructuralDivisionId : null;
                var tempHeadOrg = CurrentBusinessTrip.HeadOrganizationId != null ? CurrentBusinessTrip.HeadOrganizationId : null;
                var tempAuthored = CurrentBusinessTrip.AuthoredId != null ? CurrentBusinessTrip.AuthoredId : null;
                var tempTypeWork = CurrentBusinessTrip.TypeWorkId != null ? CurrentBusinessTrip.TypeWorkId : null;

                var tempEquipmentId = CurrentBusinessTrip.Command.ResponsibleForEquipmentId != null ? CurrentBusinessTrip.Command.ResponsibleForEquipmentId : null;
                var tempDataId = CurrentBusinessTrip.Command.ResponsibleForDataId != null ? CurrentBusinessTrip.Command.ResponsibleForDataId : null;
                var tempCommunicationId = CurrentBusinessTrip.Command.ResponsibleForCommunicationId != null ? CurrentBusinessTrip.Command.ResponsibleForCommunicationId : null;
                var tempTechnicalSecurityId = CurrentBusinessTrip.Command.ResponsibleForTechnicalSecurityId != null ? CurrentBusinessTrip.Command.ResponsibleForTechnicalSecurityId : null;
                var tempFireSecurityId = CurrentBusinessTrip.Command.ResponsibleForFireSecurityId != null ? CurrentBusinessTrip.Command.ResponsibleForFireSecurityId : null;
                var tempIndustrialSecurityId = CurrentBusinessTrip.Command.ResponsibleForIndustrialSecurityId != null ? CurrentBusinessTrip.Command.ResponsibleForIndustrialSecurityId : null;
                var tempRigInspectionId = CurrentBusinessTrip.Command.ResponsibleForRigInspectionId != null ? CurrentBusinessTrip.Command.ResponsibleForRigInspectionId : null;
                var tempDealerId = CurrentBusinessTrip.Command.ResponsibleForDealerId != null ? CurrentBusinessTrip.Command.ResponsibleForDealerId : null;
                var tempReceivingId = CurrentBusinessTrip.Command.ResponsibleForReceivingId != null ? CurrentBusinessTrip.Command.ResponsibleForReceivingId : null;
                var tempMonitoringId = CurrentBusinessTrip.Command.ResponsibleForMonitoringId != null ? CurrentBusinessTrip.Command.ResponsibleForMonitoringId : null;
                var tempInformationId = CurrentBusinessTrip.Command.ResponsibleForInformationId != null ? CurrentBusinessTrip.Command.ResponsibleForInformationId : null;

                var tempProjectManagerId = CurrentBusinessTrip.RequestTransport.ProjectManagerID != null ? CurrentBusinessTrip.RequestTransport.ProjectManagerID : null;
                //var tempTransportCustomerId = CurrentBusinessTrip.RequestTransport.TransportCustomerID != null ? CurrentBusinessTrip.RequestTransport.TransportCustomerID : null;
                var tempMarkId = CurrentBusinessTrip.RequestTransport.TransportId != null ? CurrentBusinessTrip.RequestTransport.TransportId : null;
                var tempDriverNameId = CurrentBusinessTrip.RequestTransport.DriverId != null ? CurrentBusinessTrip.RequestTransport.DriverId : null;
                var tempAddressId = CurrentBusinessTrip.RequestTransport.AddressId != null ? CurrentBusinessTrip.RequestTransport.AddressId : null;

                var tempParty = CurrentBusinessTrip.PartyId != null ? CurrentBusinessTrip.PartyId : null;

                if (HeadStructuralDivisionId != tempHeadStr || HeadOrganizationId != tempHeadOrg || AuthoredId != tempAuthored || TypeWorkId != tempTypeWork ||
                    textBoxNumber.Text != CurrentBusinessTrip.NumberDocument || dateTimePickerDate.Value != CurrentBusinessTrip.DateFormulation ||
                    textBoxTarget.Text != CurrentBusinessTrip.Target || PartyId != tempParty || EquipmentId != tempEquipmentId || DataId != tempDataId || CommunicationId != tempCommunicationId ||
                    TechnicalSecurityId != tempTechnicalSecurityId || FireSecurityId != tempFireSecurityId || IndustrialSecurityId != tempIndustrialSecurityId || RigInspectionId != tempRigInspectionId ||
                    DealerId != tempDealerId || ReceivingId != tempReceivingId || MonitoringId != tempMonitoringId || InformationId != tempInformationId || /*dateTimePickerDateBegin.Value != CurrentBusinessTrip.Command.DateBegin.Value || */
                    /*dateTimePickerDateEnd.Value != CurrentBusinessTrip.Command.DateEnd.Value ||*/ ProjectManagerId != tempProjectManagerId /*|| TransportCustomerId != tempTransportCustomerId*/ || MarkId != tempMarkId ||
                    DriverNameId != tempDriverNameId || AddressId != tempAddressId || /*string.Join("|",new string[] {textBoxNameContact.Text, textBoxPhoneContact.Text }) != CurrentBusinessTrip.RequestTransport.ContactInformation ||*/ 
                    IsEditCheckBoxAndNumeric() || dateTimePickerDateRT.Value != CurrentBusinessTrip.RequestTransport.Date || dateTimePickerDateFormulation.Value != CurrentBusinessTrip.RequestTransport.DateFormulation || checkBoxIsHoliday.Checked != CurrentBusinessTrip.IsHoliday ||
                    textBoxProxyHeadSD.Text != CurrentBusinessTrip.ProxyHeadSD || textBoxProxyHeadO.Text != CurrentBusinessTrip.ProxyHeadO || checkBoxIsOrenburgWork.Checked != CurrentBusinessTrip.IsOrenburgWork || checkBoxMileage.Checked != (CurrentBusinessTrip.RequestTransport.Mileage != null ? true : false) || checkBoxTimeHour.Checked != (CurrentBusinessTrip.RequestTransport.TimeHour != null ? true:false) || checkBoxTimeWork.Checked != (CurrentBusinessTrip.RequestTransport.TimeWork != null ? true : false)/*|| IsEditDirections()*/)
                {
                    SetButtonEnabled(false);
                }
                else
                {
                    SetButtonEnabled(true);
                }
            }
        }


        private bool IsEditCheckBoxAndNumeric()
        {
            //if (checkBoxMileage.Checked != _saveCheckBoxMileageValue || checkBoxTimeHour.Checked != _saveCheckBoxTimeHour || checkBoxTimeWork.Checked != _saveCheckBoxTimeWork)
            //    return true;
            //else
            //{
                if (checkBoxMileage.Checked)
                    if ((double)numericUpDownMileage.Value != CurrentBusinessTrip.RequestTransport.Mileage)
                        return true;
                if (checkBoxTimeHour.Checked)
                    if ((double)numericUpDownTimeHour.Value != CurrentBusinessTrip.RequestTransport.TimeHour)
                        return true;
                if (checkBoxTimeWork.Checked)
                    if ((double)numericUpDownTimeWork.Value != CurrentBusinessTrip.RequestTransport.TimeWork)
                        return true;
                return false;
            //}
        }

        /// <summary>
        /// Настройка активности кнопок.
        /// </summary>
        /// <param name="isActivate">True - все кнопки, кроме "Сохранить" активны. False - все кнопки, кроме "Сохранить" неактивны.</param>
        private void SetButtonEnabled(bool isActivate)
        {
            if (isActivate)
            {
                btnAdd.Enabled = true;
                btnAddCopy.Enabled = true;
                btnDel.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnRefresh.Enabled = true;

                btnSave.Enabled = false;

                errorProvider.Clear();

                btnClear.Enabled = true;
                comboBoxSearch.Enabled = true;
                textBoxSearch.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
                btnAddCopy.Enabled = false;
                btnDel.Enabled = false;
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                btnRefresh.Enabled = false;

                btnSave.Enabled = true;

                btnClear.Enabled = false;
                comboBoxSearch.Enabled = false;
                textBoxSearch.Enabled = false;
            }
        }

        /// <summary>
        /// Установка полей командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        private void SetValues(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            CurrentBusinessTrip = businessTrip;

            if (businessTrip != null)
            {
                Directions = businessTrip.Directions.ToList();
                //Directions = businessTrip.BusinessTripDirections.Select(bd => bd.Direction).ToList();

                HeadStructuralDivisionId = businessTrip.HeadStructuralDivisionId;
                HeadOrganizationId = businessTrip.HeadOrganizationId;
                AuthoredId = businessTrip.AuthoredId;
                TypeWorkId = businessTrip.TypeWorkId;
                PartyId = businessTrip.PartyId;

                EquipmentId = businessTrip.Command.ResponsibleForEquipmentId;
                DataId = businessTrip.Command.ResponsibleForDataId;
                CommunicationId = businessTrip.Command.ResponsibleForCommunicationId;
                TechnicalSecurityId = businessTrip.Command.ResponsibleForTechnicalSecurityId;
                FireSecurityId = businessTrip.Command.ResponsibleForFireSecurityId;
                IndustrialSecurityId = businessTrip.Command.ResponsibleForIndustrialSecurityId;
                RigInspectionId = businessTrip.Command.ResponsibleForRigInspectionId;
                DealerId = businessTrip.Command.ResponsibleForDealerId;
                ReceivingId = businessTrip.Command.ResponsibleForReceivingId;
                MonitoringId = businessTrip.Command.ResponsibleForMonitoringId;
                InformationId = businessTrip.Command.ResponsibleForInformationId;

                ProjectManagerId = businessTrip.RequestTransport.ProjectManagerID;
                //TransportCustomerId = businessTrip.RequestTransport.TransportCustomerID;
                MarkId = businessTrip.RequestTransport.TransportId;
                DriverNameId = businessTrip.RequestTransport.DriverId;
                AddressId = businessTrip.RequestTransport.AddressId;

                if (CurrentBusinessTrip.Party != null)
                {
                    //PartyPersons = CurrentBusinessTrip.Party.PartyPersons.ToList();
                    if (GetPartyPersonsByPartyId != null)
                        GetPartyPersonsByPartyId(CurrentBusinessTrip.Party.PartyId);

                    //if (persons.Count() != 0)
                    //{
                    //    bindingSourcePersons.DataSource = persons;
                    //}
                    //else
                    //{
                    //    bindingSourcePersons.DataSource = null;
                    //}
                    //bindingSourceDirections.ResetBindings(true);
                }
                else
                {
                    PartyPersons = null;
                    //bindingSourcePersons.DataSource = null;
                    //bindingSourceDirections.ResetBindings(true);
                }

                textBoxTypeWork.Text = businessTrip.TypeWork != null ? businessTrip.TypeWork.Name : "";
                textBoxHeadStr.Text = businessTrip.HeadStructuralDivision != null ? businessTrip.HeadStructuralDivision.Name : "";
                textBoxProxyHeadSD.Text = businessTrip.ProxyHeadSD;
                textBoxHeadOrg.Text = businessTrip.HeadOrganization != null ? businessTrip.HeadOrganization.Name : "";
                textBoxProxyHeadO.Text = businessTrip.ProxyHeadO;
                textBoxAuthored.Text = businessTrip.Authored != null ? businessTrip.Authored.Name : "";
                textBoxNumber.Text = businessTrip.NumberDocument;
                dateTimePickerDate.Value = businessTrip.DateFormulation != null ? businessTrip.DateFormulation : DateTime.Now;
                textBoxParty.Text = businessTrip.Party != null ? businessTrip.Party.Name : "";

                textBoxEqipment.Text = businessTrip.Command.ResponsibleForEquipment != null ? businessTrip.Command.ResponsibleForEquipment.Name : "";
                textBoxData.Text = businessTrip.Command.ResponsibleForData != null ? businessTrip.Command.ResponsibleForData.Name : "";
                textBoxCommunication.Text = businessTrip.Command.ResponsibleForCommunication != null ? businessTrip.Command.ResponsibleForCommunication.Name : "";
                textBoxTechnicalSecurity.Text = businessTrip.Command.ResponsibleForTechnicalSecurity != null ? businessTrip.Command.ResponsibleForTechnicalSecurity.Name : "";
                textBoxFireSecurity.Text = businessTrip.Command.ResponsibleForFireSecurity != null ? businessTrip.Command.ResponsibleForFireSecurity.Name : "";
                textBoxIndustrialSecurity.Text = businessTrip.Command.ResponsibleForIndustrialSecurity != null ? businessTrip.Command.ResponsibleForIndustrialSecurity.Name : "";
                textBoxRigInspection.Text = businessTrip.Command.ResponsibleForRigInspection != null ? businessTrip.Command.ResponsibleForRigInspection.Name : "";
                textBoxDealer.Text = businessTrip.Command.ResponsibleForDealer != null ? businessTrip.Command.ResponsibleForDealer.Name : "";
                textBoxReceiving.Text = businessTrip.Command.ResponsibleForReceiving != null ? businessTrip.Command.ResponsibleForReceiving.Name : "";
                textBoxMonitoring.Text = businessTrip.Command.ResponsibleForMonitoring != null ? businessTrip.Command.ResponsibleForMonitoring.Name : "";
                textBoxDovInformation.Text = businessTrip.Command.ResponsibleForInformation != null ? businessTrip.Command.ResponsibleForInformation.Name : "";

                textBoxProjectManager.Text = businessTrip.RequestTransport.ProjectManager != null ? businessTrip.RequestTransport.ProjectManager.Name : "";
                //textBoxTransportCustomer.Text = businessTrip.RequestTransport.TransportCustomer != null ? businessTrip.RequestTransport.TransportCustomer.Name : "";
                textBoxMark.Text = businessTrip.RequestTransport.Transport != null ? businessTrip.RequestTransport.Transport.Mark : "";
                textBoxDriverName.Text = businessTrip.RequestTransport.Driver != null ? businessTrip.RequestTransport.Driver.Name : "";
                textBoxAddress.Text = businessTrip.RequestTransport.Address != null ? businessTrip.RequestTransport.Address.ShortAddress : ""; ;

                //dateTimePickerDateBegin.Value = businessTrip.Command.DateBegin != null ? businessTrip.Command.DateBegin.Value : DateTime.Now;
                //dateTimePickerDateEnd.Value = businessTrip.Command.DateEnd != null ? businessTrip.Command.DateEnd.Value : DateTime.Now;

                var dateBegins = Directions != null ? Directions.Select(r => r.DateBegin) : null;
                DateTime? minDate = dateBegins != null ? dateBegins.Where(d => d == dateBegins.Min(x => x)).FirstOrDefault() : null;
                dateTimePickerDateBegin.Value = minDate != null ? minDate.Value : dateTimePickerDateBegin.MinDate;

                var dateEnds = Directions != null ? Directions.Select(r => r.DateEnd) : null;
                DateTime? maxDate = dateEnds != null ? dateEnds.Where(d => d == dateEnds.Max(x => x)).FirstOrDefault() : null;
                dateTimePickerDateEnd.Value = maxDate != null ? maxDate.Value : dateTimePickerDateBegin.MinDate;

                textBoxTarget.Text = businessTrip.Target;

                //var contactInformations = businessTrip.RequestTransport.ContactInformation.Split('|');
                //textBoxNameContact.Text = contactInformations.Length > 0 ? contactInformations[0] : "";
                //textBoxPhoneContact.Text = contactInformations.Length > 1 ? contactInformations[1] : "";

                textBoxCountMan.Text = bindingSourcePersons != null ?  bindingSourcePersons.Count.ToString() : "0";

                var mileage = businessTrip.RequestTransport.Mileage;
                checkBoxMileage.Checked = mileage != null ? true : false;
                numericUpDownMileage.Value = mileage != null ? (decimal)mileage : 0;

                var timeHour = businessTrip.RequestTransport.TimeHour;
                checkBoxTimeHour.Checked = timeHour != null ? true : false;
                numericUpDownTimeHour.Value = timeHour != null ? (decimal)timeHour : 0;

                var timeWork = businessTrip.RequestTransport.TimeWork;
                checkBoxTimeWork.Checked = timeWork != null ? true : false;
                numericUpDownTimeWork.Value = timeWork != null ? (decimal)timeWork : 0;

                //_saveCheckBoxMileageValue = checkBoxMileage.Checked;
                //_saveCheckBoxTimeHour = checkBoxTimeHour.Checked;
                //_saveCheckBoxTimeWork = checkBoxTimeWork.Checked;


                dateTimePickerDateRT.Value = businessTrip.RequestTransport.Date.Value != null ? businessTrip.RequestTransport.Date.Value : DateTime.Now;
                dateTimePickerDateFormulation.Value = businessTrip.RequestTransport.DateFormulation.Value != null ? businessTrip.RequestTransport.DateFormulation.Value : DateTime.Now;

                checkBoxIsHoliday.Checked = CurrentBusinessTrip.IsHoliday;
                checkBoxIsOrenburgWork.Checked = CurrentBusinessTrip.IsOrenburgWork;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            HeadStructuralDivisionId = null;
            HeadOrganizationId = null;
            AuthoredId = null;
            TypeWorkId = null;
            PartyId = null;

            EquipmentId = null;
            DataId = null;
            CommunicationId = null;
            TechnicalSecurityId = null;
            FireSecurityId = null;
            IndustrialSecurityId = null;
            RigInspectionId = null;
            DealerId = null;
            ReceivingId = null;
            MonitoringId = null;
            InformationId = null;
            ProjectManagerId = null;
            TransportCustomerId = null;
            MarkId = null;
            DriverNameId = null;
            AddressId = null;

            Directions.Clear();
            PartyPersons.Clear();

            //TODO: не все элементы.
            textBoxTypeWork.Clear();
            textBoxHeadStr.Clear();
            textBoxProxyHeadSD.Clear();
            textBoxHeadOrg.Clear();
            textBoxProxyHeadO.Clear();
            textBoxAuthored.Clear();
            textBoxNumber.Clear();
            textBoxParty.Clear();

            textBoxEqipment.Clear();
            textBoxData.Clear();
            textBoxCommunication.Clear();
            textBoxTechnicalSecurity.Clear();
            textBoxFireSecurity.Clear();
            textBoxIndustrialSecurity.Clear();
            textBoxRigInspection.Clear();
            textBoxDealer.Clear();
            textBoxReceiving.Clear();
            textBoxMonitoring.Clear();
            textBoxDovInformation.Clear();
            //textBoxTransportCustomer.Clear();
            textBoxProjectManager.Clear();

            dateTimePickerDate.Value = DateTime.Now;
            textBoxTarget.Clear();
            dataGridViewDirections.Rows.Clear();
            dataGridViewPersons.Rows.Clear();

            textBoxMark.Clear();
            textBoxDriverName.Clear();
            textBoxAddress.Clear();
            textBoxCountMan.Clear();
            checkBoxMileage.Checked = false;
            checkBoxTimeHour.Checked = false;
            checkBoxTimeWork.Checked = false;
            numericUpDownMileage.Value = 0;
            numericUpDownTimeHour.Value = 0;
            numericUpDownTimeWork.Value = 0;
            dateTimePickerDateRT.Value = DateTime.Now;
            dateTimePickerDateFormulation.Value = DateTime.Now;


            //_savePosition = 0;


            textBoxNumber.Focus();
        }

        /// <summary>
        /// Получение всех сотрудников.
        /// </summary>
        private void GetAllBusinessTrips()
        {
            if (GetAll != null)
                GetAll();

            BusinessTrips = Filters();

            if (BusinessTrips.Any())
            {
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
            }
        }

        /// <summary>
        /// Проверка на наличие несохраненных изменениях.
        /// </summary>
        private bool IsCheckSave()
        {
            if (btnSave.Enabled == true)
            {
                var mes = MessageBox.Show("Применить несохраненные изменения?", DirectoryName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (mes == DialogResult.Yes)
                {
                    if (IsCheck())
                    {
                        SaveChange();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (mes == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    GetAllBusinessTrips();

                    SetButtonEnabled(true);
                    if (dataGridViewBusinessTrip.RowCount > 0)
                    {
                        dataGridViewBusinessTrip.ClearSelection();
                        dataGridViewBusinessTrip.Rows[0].Selected = true;
                        dataGridViewBusinessTrip.CurrentCell = dataGridViewBusinessTrip[0, 0];
                    }
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Сохранение изменений.
        /// </summary>
        private void SaveChange()
        {
            if (_isEdit)
            {
                if (Update != null)
                {
                    SavePosition();

                    Update(BusinessTrip);

                    GetAllBusinessTrips();

                    SelectObject();
                }
            }
            else
            {
                if (SaveBusinessTrip != null)
                    SaveBusinessTrip(BusinessTrip);

                GetAllBusinessTrips();
                SelectObject();
            }
            _isEdit = true;
            SetButtonEnabled(true);
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        /// <returns>True - успех. False - не пройдена.</returns>
        private bool IsCheck()
        {
            errorProvider.Clear();
            var ok = true;
            if (string.IsNullOrWhiteSpace(textBoxNumber.Text))
            {
                errorProvider.SetError(textBoxNumber, "Не задан номер командировки");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewBusinessTrip.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewBusinessTrip.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, BusinessTrips.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentBusinessTrip = null;
            }
        }

        public List<Core.BusinessTrip.Domain.BusinessTrip> Filters()
        {
            var businessTrips = BusinessTrips;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        businessTrips = businessTrips.Where(o => o.BusinessTripId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Номер":
                        {
                            businessTrips = businessTrips.Where(o => o.NumberDocument.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Дата":
                        {
                            businessTrips = businessTrips.Where(o => o.DateFormulationToString.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Автор":
                        {
                            businessTrips = businessTrips.Where(o => o.AuthoredToString.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }
            return businessTrips;
            //dataGridViewBusinessTrip.SelectionChanged -= dataGridViewBusinessTrip_SelectionChanged;

            //dataGridViewBusinessTrip.AutoGenerateColumns = false;
            //SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip> businessTripBindingList = new SortableBindingList<Core.BusinessTrip.Domain.BusinessTrip>(businessTrips);
            //dataGridViewBusinessTrip.DataSource = businessTripBindingList;

            //if (businessTripBindingList.Count == 0)
            //{
            //    ClearForm();
            //    SetButtonEnabled(true);
            //}

            //dataGridViewBusinessTrip.SelectionChanged += dataGridViewBusinessTrip_SelectionChanged;
        }


        #endregion

    }
}
