using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Client.BusinessTrip.Helpers;
using Client.BusinessTrip.Helpers.Sortable;
using Core.BusinessTrip.Context;
using Client.BusinessTrip.Helpers.HelperReports;
using Client.BusinessTrip.IoC;
using System.ComponentModel;
using Client.BusinessTrip.Properties;
using System.Drawing;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Справочник персонала".
    /// </summary>
    public partial class DirectoryPersonsView : Form, IDirectoryPersonsView
    {
        #region Private Fields and Consts

        private const string DirectoryName = "Справочник сотрудников";

        private List<string> _listSearch = new List<string> { "ФИО", "Должность", "Таб. №", "Отдел" };

        private IDirectoryPersonsPresenter _presenter;

        private IBaseModel<Person> _model;

        private List<Person> _persons;
        private Person _person;
        private Person _currentPerson;

        private int? _positionId;
        private int? _departmentId;
        private int? _headId;
        private int? _locationId;

        //режим редактирования.
        private bool _isEdit = false;

        //стандартный режим / режим выбора объекта.
        //private bool _isStandard = true;

        //private int _savePosition = 0;
        private int? _saveIndex = 0;

        private int _col = -1;
        private int _row = -1;

        private List<PartyPerson> _selectedPersons;

        private bool _oneShow = true;

        private DialogResult _resultDialog = DialogResult.No;

        #endregion //Private Fields and Consts

        #region Constructors

        public DirectoryPersonsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник сотрудников".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        ///// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryPersonsView(IDirectoryPersonsPresenter presenter, IBaseModel<Person> model/*, bool isStandard = true*/)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewPersons)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewPersons, Settings.Default.WindowHeightViewPersons);
            }

            #region dataGridViewPersons settings

            dataGridViewPersons.Columns.Add("NameColumn", "ФИО");
            dataGridViewPersons.Columns["NameColumn"].DataPropertyName = "Name";
            dataGridViewPersons.Columns["NameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewPersons.Columns["NameColumn"].FillWeight = 135;
            dataGridViewPersons.Columns["NameColumn"].ToolTipText = "ФИО";

            dataGridViewPersons.Columns.Add("PositionToStringColumn", "Должность");
            dataGridViewPersons.Columns["PositionToStringColumn"].DataPropertyName = "PositionToString";
            dataGridViewPersons.Columns["PositionToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewPersons.Columns["PositionToStringColumn"].FillWeight = 120;
            dataGridViewPersons.Columns["PositionToStringColumn"].ToolTipText = "Должность";

            dataGridViewPersons.Columns.Add("PersonnelNumberColumn", "Таб. №");
            dataGridViewPersons.Columns["PersonnelNumberColumn"].DataPropertyName = "PersonnelNumber";
            dataGridViewPersons.Columns["PersonnelNumberColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewPersons.Columns["PersonnelNumberColumn"].FillWeight = 72;
            dataGridViewPersons.Columns["PersonnelNumberColumn"].ToolTipText = "Таб. №";

            dataGridViewPersons.Columns.Add("DepartmentToStringColumn", "Отдел");
            dataGridViewPersons.Columns["DepartmentToStringColumn"].DataPropertyName = "DepartmentToString";
            dataGridViewPersons.Columns["DepartmentToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewPersons.Columns["DepartmentToStringColumn"].FillWeight = 72;
            dataGridViewPersons.Columns["DepartmentToStringColumn"].ToolTipText = "Отдел";

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewPersons.ContextMenuStrip = contextMenu;

            dataGridViewPersons.DoubleBuffered(true);
            this.DoubleBuffered(true);

            #endregion //dataGridViewPersons settings

            btnSave.Enabled = false;
            TabControl1.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Persons != null)
            {
                if (Persons.Count > 0)
                {
                    CurrentPerson = Persons[0];
                }
            }

            //_isStandard = isStandard;

            //if (!_isStandard)
            //{
            //    btnAdd.Enabled = false;
            //    btnAddCopy.Enabled = false;
            //    btnDel.Enabled = false;
            //    btnSave.Enabled = false;
            //}

            //LoadExcelPersons load = new LoadExcelPersons();

            //var persons = load.LoadPerson();

            //foreach (var person in persons)
            //{
            //    if (Save != null)
            //        Save(person);
            //}
        }

        #endregion //Constructors

        #region IBaseView, IDirectoryPersonsView

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
        /// Список сотрудников.
        /// </summary>
        public List<Person> Persons
        {
            get
            {
                return _persons;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewPersons.SelectionChanged -= dataGridViewPersons_SelectionChanged;

                    dataGridViewPersons.AutoGenerateColumns = false;
                    SortableBindingList<Person> personsBindingList = new SortableBindingList<Person>(value);
                    dataGridViewPersons.DataSource = personsBindingList;

                    if (dataGridViewPersons.ColumnCount > Settings.Default.SortIndexPersons)
                    {
                        dataGridViewPersons.Sort(dataGridViewPersons.Columns[Settings.Default.SortIndexPersons],
                        Settings.Default.SortOrderPersons ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (personsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _persons = value;

                    dataGridViewPersons.SelectionChanged += dataGridViewPersons_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Новый сотрудник.
        /// </summary>
        public Person Person
        {
            get
            {
                if (_isEdit)
                {
                    _person = new Person
                    {
                        PersonId = CurrentPerson.PersonId,
                        Name = textBoxName.Text,
                        PersonnelNumber = textBoxPersonnelNumber.Text,
                        PositionId = (int)PositionId,
                        DepartmentId = DepartmentId,
                        HeadId = HeadId,
                        LocationId = LocationId,
                        PhoneNumber = textBoxPhoneNumber.Text
                    };
                }
                else
                {
                    _person = new Person
                    {
                        Name = textBoxName.Text,
                        PersonnelNumber = textBoxPersonnelNumber.Text,
                        PositionId = (int)PositionId,
                        DepartmentId = DepartmentId,
                        HeadId = HeadId,
                        LocationId = LocationId,
                        PhoneNumber = textBoxPhoneNumber.Text
                    };
                }
                return _person;
            }
            set
            {
                _person = value;
            }
        }

        /// <summary>
        /// Выбранный сотрудник.
        /// </summary>
        public Person CurrentPerson
        {
            get
            {
                return _currentPerson;
            }
            set
            {
                _currentPerson = value;
            }
        }

        public List<PartyPerson> SelectedPersons
        {
            get
            {
                return _selectedPersons;
            }
            set
            {
                if (value != null)
                {
                    _selectedPersons = value;
                }
                else
                {
                    _selectedPersons = new List<PartyPerson>();
                }
            }
        }

        /// <summary>
        /// Ид должности.
        /// </summary>
        public int? PositionId
        {
            get { return _positionId; }
            set { _positionId = value; }
        }

        /// <summary>
        /// Ид отдела.
        /// </summary>
        public int? DepartmentId
        {
            get { return _departmentId; }
            set { _departmentId = value; }
        }

        /// <summary>
        /// Ид руководителя.
        /// </summary>
        public int? HeadId
        {
            get { return _headId; }
            set { _headId = value; }
        }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        public int? LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }

        /// <summary>
        /// Событие запускает сохранение сотрудника.
        /// </summary>
        public event Action<Person> Save;

        /// <summary>
        /// Событие запускает обновление сотрудника.
        /// </summary>
        public event Action<Person> Update;

        /// <summary>
        /// Событие запускает удаление сотрудника.
        /// </summary>
        public event Action<List<Person>> Delete;

        /// <summary>
        /// Событие запускает получение сотрудников.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryPersonsView

        #region EventHandlers

        //TODO: Посмотреть что сделать с закрашиванием неактивной вкладки (сейчас она просто становится неактивной).

        //private void DisableTab_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    TabControl tabControl = sender as TabControl;
        //    TabPage page = tabControl.TabPages[e.Index];      

        //    if (!page.Enabled)
        //    {
        //        //Draws disabled tab
        //        using (SolidBrush brush = new SolidBrush(SystemColors.GrayText))
        //        {
        //            e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
        //        }
        //    }
        //    else
        //    {
        //        // Draws normal tab
        //        using (SolidBrush brush = new SolidBrush(page.ForeColor))
        //        {
        //            e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
        //        }
        //    }
        //}

        private void DirectoryPersonsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Закрытие формы.
        /// </summary>
        private void DirectoryPersonsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPersons.SelectedRows[countRow - 1];
                var person = row.DataBoundItem as Person;
                if (person != null)
                {
                    SetValues(person);
                    _isEdit = true;
                }
            }

            var persons = new List<PartyPerson>();

            foreach (DataGridViewRow row in dataGridViewPersons.SelectedRows)
            {
                var person = row.DataBoundItem as Person;
                if (person != null)
                {
                    if (person.PersonId != 0)
                    {
                        persons.Add(new PartyPerson
                        {
                            Party_PartyId = 0,
                            Person_PersonId = person.PersonId,
                            Person = person
                        });
                    }
                }
            }
            persons.Reverse();
            SelectedPersons = persons;//.OrderBy(p => p.Name).ToList();

            if (ViewClosed != null)
                ViewClosed();
        }

        /// <summary>
        /// Добавление.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl1.SelectedTab = TabCard;
            CurrentPerson = null;
            ClearForm();
            SetButtonEnabled(false);
            //_savePosition = 0;
            SaveIndex = null;
            _isEdit = false;
        }

        /// <summary>
        /// Добавление копии.
        /// </summary>
        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPersons.SelectedRows[countRow - 1];
                var person = row.DataBoundItem as Person;
                if (person != null)
                {
                    TabControl1.SelectedTab = TabCard;
                    //SetValues(person);
                    SetButtonEnabled(false);
                    //_savePosition = 0;
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        /// <summary>
        /// Удаление.
        /// </summary>
        private void btnDel_Click(object sender, EventArgs e)
        {
            DeletePersons();
        }

        /// <summary>
        /// Сохранение по нажатию кнопки "Сохранить".
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsCheck())
            {
                SaveChange();
            }
        }

        /// <summary>
        /// Выбор предыдущей командировки.
        /// </summary>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            int indexSelect, indexPrev;
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPersons.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewPersons.ClearSelection();
                    dataGridViewPersons.Rows[indexPrev].Selected = true;
                    dataGridViewPersons.CurrentCell = dataGridViewPersons[0, indexPrev];
                }
            }
        }

        /// <summary>
        /// Выбор следующей командировки.
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPersons.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewPersons.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewPersons.ClearSelection();
                    dataGridViewPersons.Rows[indexNext].Selected = true;
                    dataGridViewPersons.CurrentCell = dataGridViewPersons[0, indexNext];
                }
            }
        }

        /// <summary>
        /// Сохранение по нажатию кнопки "Ок" и закрытие формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_ButtonClick(object sender, EventArgs e)
        {
            if (IsCheckSave())
            {
                _resultDialog = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Отмена.
        /// </summary>
        private void btnCancel_ButtonClick(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();

            _resultDialog = DialogResult.No;

            Close();
        }

        /// <summary>
        /// Выбор сотрудника в списке.
        /// </summary>
        private void dataGridViewPersons_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPersons.SelectedRows[countRow - 1];
                var person = row.DataBoundItem as Person;
                if (person != null)
                {
                    SetValues(person);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxPersonnelNumber_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxDepartment_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxPosition_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxHead_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        /// <summary>
        /// Переход между вкладками.
        /// </summary>
        private void TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = e.TabPageIndex == 1;
            }
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (_row < dataGridViewPersons.RowCount && _col < dataGridViewPersons.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewPersons.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewPersons_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewPersons_DoubleClick(object sender, EventArgs e)
        {
            TabControl1.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllPersons();
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
            GetAllPersons();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewPersons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl1.SelectedTab == tabList)
                {
                    DeletePersons();
                }
            }
        }

        private void DirectoryPersonsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnSave.Enabled == false && TabControl1.SelectedTab == tabList)
            {
                if (IsCheckSave())
                {
                    _resultDialog = DialogResult.OK;

                    Close();
                }
            }
            else if (e.KeyCode == Keys.Escape && btnSave.Enabled == false /*&& TabControl1.SelectedTab == tabList*/)
            {
                if (IsCheckSave())
                {
                    _resultDialog = DialogResult.No;

                    Close();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAllPersons();

            dataGridViewPersons.ClearSelection();

            if (dataGridViewPersons.RowCount > 0)
            {
                dataGridViewPersons.CurrentCell = dataGridViewPersons[0, 0];
                dataGridViewPersons.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryPersonsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryDepartmentsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentDepartment != null)
                {
                    this.DepartmentId = view.CurrentDepartment.DepartmentId;
                    this.textBoxDepartment.Text = view.CurrentDepartment.Nominative;
                }
                else
                {
                    this.DepartmentId = null;
                    this.textBoxDepartment.Text = "";
                }
            }
        }

        private void btnPosition_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPositionsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPosition != null)
                {
                    this.PositionId = view.CurrentPosition.PositionId;
                    this.textBoxPosition.Text = view.CurrentPosition.Nominative;
                }
                else
                {
                    this.PositionId = null;
                    this.textBoxPosition.Text = "";
                }
            }
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryLocationsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentLocation != null)
                {
                    this.LocationId = view.CurrentLocation.LocationId;
                    this.textBoxAddress.Text = view.CurrentLocation.ShortAddress;
                }
                else
                {
                    this.LocationId = null;
                    this.textBoxAddress.Text = "";
                }
            }
        }

        private void btnHead_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>();

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.CurrentPerson != null)
                {
                    this.HeadId = view.CurrentPerson.PersonId;
                    this.textBoxHead.Text = view.CurrentPerson.Name;
                }
                else
                {
                    this.HeadId = null;
                    this.textBoxHead.Text = "";
                }
            }
        }

        private void buttonDepartmentClear_Click(object sender, EventArgs e)
        {
            DepartmentId = null;
            textBoxDepartment.Clear();
        }

        private void btnPositionClear_Click(object sender, EventArgs e)
        {
            PositionId = null;
            textBoxPosition.Clear();
        }

        private void buttonAddressClear_Click(object sender, EventArgs e)
        {
            LocationId = null;
            textBoxAddress.Clear();
        }

        private void buttonHeadClear_Click(object sender, EventArgs e)
        {
            HeadId = null;
            textBoxHead.Clear();
        }

        private void dataGridViewPersons_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewPersons.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewPersons.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewPersons_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexPersons = e.ColumnIndex;

                if (dataGridViewPersons.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderPersons = true;
                }
                else if (dataGridViewPersons.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderPersons = false;
                }
                else
                {
                    Settings.Default.SortOrderPersons = true;
                }
            }
            else
            {
                Settings.Default.SortIndexPersons = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryPersonsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewPersons = true;
            else
                Settings.Default.WindowStateMaximizedViewPersons = false;

            Settings.Default.WindowWidthViewPersons = this.Size.Width;
            Settings.Default.WindowHeightViewPersons = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeletePersons()
        {
            SavePosition(true,true);

            var persons = new List<Person>();

            foreach (DataGridViewRow row in dataGridViewPersons.SelectedRows)
            {
                var person = row.DataBoundItem as Person;
                if (person != null)
                {
                    persons.Add(new Person { PersonId = person.PersonId });
                }
            }

            if (!persons.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить сотрудников (кол-во: {0})?", persons.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(persons);

                GetAllPersons();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewPersons.ClearSelection();

            if (dataGridViewPersons.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewPersons.Rows)
                    {
                        var person = row.DataBoundItem as Person;
                        if (person != null)
                        {
                            if (person.PersonId == SaveIndex)
                            {
                                dataGridViewPersons.CurrentCell = dataGridViewPersons[0, row.Index];
                                dataGridViewPersons.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewPersons.CurrentCell = dataGridViewPersons[0, 0];
                dataGridViewPersons.Rows[0].Selected = true;
            }

            //if (dataGridViewPersons.RowCount > 0)
            //{
            //    if (_savePosition > dataGridViewPersons.RowCount - 1)
            //    {
            //        if (isEnd)
            //        {
            //            dataGridViewPersons.CurrentCell = dataGridViewPersons[0, dataGridViewPersons.RowCount - 1];
            //            dataGridViewPersons.Rows[dataGridViewPersons.RowCount - 1].Selected = true;
            //        }
            //        else
            //        {
            //            dataGridViewPersons.CurrentCell = dataGridViewPersons[0, 0];
            //            dataGridViewPersons.Rows[0].Selected = true;
            //        }
            //    }
            //    else
            //    {
            //        dataGridViewPersons.CurrentCell = dataGridViewPersons[0, _savePosition];
            //        dataGridViewPersons.Rows[_savePosition].Selected = true;
            //    }
            //}
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewPersons.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewPersons.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewPersons.SelectedRows.Count < dataGridViewPersons.RowCount)
                    {
                        var selectedRow = dataGridViewPersons.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewPersons.SelectedRows[0].Index == dataGridViewPersons.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewPersons.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewPersons.RowCount)
                            {
                                row = dataGridViewPersons.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewPersons.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewPersons.RowCount)
                            {
                                row = dataGridViewPersons.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var person = row.DataBoundItem as Person;
                            if (person != null)
                            {
                               SaveIndex = person.PersonId;
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
                    var row = dataGridViewPersons.SelectedRows[selectIndex];
                    var person = row.DataBoundItem as Person;
                    if (person != null)
                    {
                        SaveIndex = person.PersonId;
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

            //if (dataGridViewPersons.SelectedRows.Count > 0)
            //{
            //    if (isEnd)
            //    {
            //        _savePosition = dataGridViewPersons.SelectedRows[dataGridViewPersons.SelectedRows.Count - 1].Index;
            //    }
            //    else
            //    {
            //        _savePosition = dataGridViewPersons.SelectedRows[0].Index;
            //    }
            //}
        }


        /// <summary>
        /// Переход в состояние редактирования, либо выход из него.
        /// </summary>
        private void SetStateForChange()
        {
            if (CurrentPerson != null)
            {
                var tempDepartmentId = CurrentPerson.DepartmentId;
                var tempPositionId = CurrentPerson.PositionId;
                var tempLocationId = CurrentPerson.LocationId != null ? CurrentPerson.LocationId : null;
                var tempHeadId = CurrentPerson.HeadId != null ? CurrentPerson.HeadId : null;

                if (textBoxName.Text != CurrentPerson.Name || textBoxPersonnelNumber.Text != CurrentPerson.PersonnelNumber ||
                    DepartmentId != tempDepartmentId || PositionId != tempPositionId || LocationId != tempLocationId || HeadId != tempHeadId ||
                    textBoxPhoneNumber.Text != CurrentPerson.PhoneNumber)
                {
                    SetButtonEnabled(false);
                }
                else
                {
                    SetButtonEnabled(true);
                }
            }
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

                btnClear.Enabled = true;
                comboBoxSearch.Enabled = true;
                textBoxSearch.Enabled = true;

                errorProvider.Clear();
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
        /// Установка полей сотрудника.
        /// </summary>
        /// <param name="person">Сотрудник.</param>
        private void SetValues(Person person)
        {
            CurrentPerson = person;
            if (person != null)
            {
                DepartmentId = CurrentPerson.DepartmentId;
                PositionId = CurrentPerson.PositionId;
                LocationId = CurrentPerson.LocationId;
                HeadId = CurrentPerson.HeadId;

                textBoxName.Text = person.Name;
                textBoxPersonnelNumber.Text = person.PersonnelNumber;
                textBoxDepartment.Text = person.Department != null ? person.Department.Nominative : "";
                textBoxPosition.Text = person.Position != null ? person.Position.Nominative : "";
                textBoxAddress.Text = person.Location != null ? person.Location.ShortAddress : "";
                textBoxHead.Text = person.Head != null ? person.Head.Name : "";

                textBoxPhoneNumber.Text = person.PhoneNumber;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            DepartmentId = null;
            PositionId = null;
            LocationId = null;
            HeadId = null;

            textBoxName.Clear();
            textBoxPersonnelNumber.Clear();
            textBoxDepartment.Clear();
            textBoxPosition.Clear();
            textBoxAddress.Clear();
            textBoxHead.Clear();
            textBoxPhoneNumber.Clear();
            //_savePosition = 0;

            textBoxName.Focus();
        }

        /// <summary>
        /// Получение всех сотрудников.
        /// </summary>
        private void GetAllPersons()
        {
            if (GetAll != null)
                GetAll();

            Persons = Filters();

            if (Persons.Any())
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
                    GetAllPersons();

                    SetButtonEnabled(true);
                    if (dataGridViewPersons.RowCount > 0)
                    {
                        dataGridViewPersons.ClearSelection();
                        dataGridViewPersons.Rows[0].Selected = true;
                        dataGridViewPersons.CurrentCell = dataGridViewPersons[0, 0];
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

                    Update(Person);

                    GetAllPersons();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Person);

                GetAllPersons();

                SelectObject();
                //dataGridViewPersons.ClearSelection();

                //if (dataGridViewPersons.RowCount > 0)
                //{
                //    dataGridViewPersons.CurrentCell = dataGridViewPersons[0, dataGridViewPersons.RowCount - 1];
                //    dataGridViewPersons.Rows[dataGridViewPersons.RowCount - 1].Selected = true;
                //}
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
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                errorProvider.SetError(textBoxName, "Не задано имя сотрудника");
                ok = false;
            }
            else if (string.IsNullOrWhiteSpace(textBoxPersonnelNumber.Text))
            {
                errorProvider.SetError(textBoxPersonnelNumber, "Не задан табельный номер сотрудника");
                ok = false;
            }
            //else if (string.IsNullOrWhiteSpace(textBoxDepartment.Text) || DepartmentId == null)
            //{
            //    errorProvider.SetError(textBoxDepartment, "Не задан отдел сотрудника");
            //    ok = false;
            //}
            else if (string.IsNullOrWhiteSpace(textBoxPosition.Text) || PositionId == null)
            {
                errorProvider.SetError(textBoxPosition, "Не задана должность сотрудника");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPersons.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Persons.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentPerson = null;
            }
        }

        public List<Person> Filters()
        {
            var persons = Persons;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    case "ФИО":
                        {
                            persons = persons.Where(o => o.Name.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Должность":
                        {
                            persons = persons.Where(o => o.PositionToString.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Таб. №":
                        {
                            persons = persons.Where(o => o.PersonnelNumber.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Отдел":
                        {
                            persons = persons.Where(o => o.DepartmentToString.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }
            return persons;
        }


        #endregion //Private Methods
    }
}
