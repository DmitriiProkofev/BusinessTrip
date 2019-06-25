using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IoC;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Client.BusinessTrip.Helpers;
using Client.BusinessTrip.Helpers.Sortable;
using Core.BusinessTrip.Helpers.DomainHelpers;
using Client.BusinessTrip.Properties;
using System.ComponentModel;
using System.Drawing;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Справочник партий".
    /// </summary>
    public partial class DirectoryPartysView : Form, IDirectoryPartysView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник партий";

        private List<string> _listSearch = new List<string> { "Наименование", /*"Ид",*/ };

        private IDirectoryPartysPresenter _presenter;

        private IDirectoryPartysModel _model;

        private List<Party> _partys;

        private Party _party;

        private Party _currentParty;

        private List<PartyPerson> _persons;
        private List<PartyPerson> _addPersons;
        private List<PartyPerson> _deletePersons;
        private List<PartyPerson> _editPersons;
        private List<int> _editDGVPersons = new List<int>();

        //режим редактирования.
        private bool _isEdit = false;

        //стандартный режим / режим выбора объекта.
        //private bool _isStandard = true;

        private int? _saveIndex = 0;

        private int _col = -1;
        private int _row = -1;

        private int _colDVGPersons = -1;
        private int _rowDVGPersons = -1;

        private bool _oneShow = true;

        private DialogResult _resultDialog = DialogResult.No;

        #endregion //Private Fields

        #region Constructors

        public DirectoryPartysView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник сотрудников".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryPartysView(IDirectoryPartysPresenter presenter, IDirectoryPartysModel model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewPartys)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewPartys, Settings.Default.WindowHeightViewPartys);
            }

            #region dataGridViewPartys\dataGridViewPersons settings

            //dataGridViewPartys.Columns.Add("PartyIdColumn", "Ид");
            //dataGridViewPartys.Columns["PartyIdColumn"].DataPropertyName = "PartyId";
            //dataGridViewPartys.Columns["PartyIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewPartys.Columns["PartyIdColumn"].Width = 50;
            //dataGridViewPartys.Columns["PartyIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridViewPartys.Columns.Add("NameColumn", "Наименование");
            dataGridViewPartys.Columns["NameColumn"].DataPropertyName = "Name";
            dataGridViewPartys.Columns["NameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewPartys.Columns["NameColumn"].FillWeight = 300;

            //dataGridViewPersons.Columns.Add("NameColumn", "Наименование");
            //dataGridViewPersons.Columns["NameColumn"].DataPropertyName = "Name";
            //dataGridViewPersons.Columns["NameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPersons.Columns["NameColumn"].FillWeight = 140;

            //dataGridViewPersons.Columns.Add("PositionToStringColumn", "Должность");
            //dataGridViewPersons.Columns["PositionToStringColumn"].DataPropertyName = "PositionToString";
            //dataGridViewPersons.Columns["PositionToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPersons.Columns["PositionToStringColumn"].FillWeight = 114;

            //dataGridViewPersons.Columns.Add("PersonnelNumberColumn", "Таб. №");
            //dataGridViewPersons.Columns["PersonnelNumberColumn"].DataPropertyName = "PersonnelNumber";
            //dataGridViewPersons.Columns["PersonnelNumberColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPersons.Columns["PersonnelNumberColumn"].FillWeight = 71;

            //dataGridViewPersons.Columns.Add("DepartmentToStringColumn", "Отдел");
            //dataGridViewPersons.Columns["DepartmentToStringColumn"].DataPropertyName = "DepartmentToString";
            //dataGridViewPersons.Columns["DepartmentToStringColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPersons.Columns["DepartmentToStringColumn"].FillWeight = 71;


            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewPartys.ContextMenuStrip = contextMenu;

            ContextMenuStrip contextMenuDGVPersons = new ContextMenuStrip();
            ToolStripMenuItem itemDGVPersons = new ToolStripMenuItem("Копировать");
            itemDGVPersons.Click += new EventHandler(mnuCopyDGVPersons_Click);
            contextMenuDGVPersons.Items.AddRange(new ToolStripItem[] { itemDGVPersons });
            dataGridViewPersons.ContextMenuStrip = contextMenuDGVPersons;

            dataGridViewPersons.DoubleBuffered(true);
            dataGridViewPartys.DoubleBuffered(true);

            #endregion //dataGridViewPartys\dataGridViewPersons settings

            btnSave.Enabled = false;
            TabControl1.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Partys != null)
            {
                if (Partys.Count > 0)
                {
                    CurrentParty = Partys[0];
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
        public List<Party> Partys
        {
            get
            {
                return _partys;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewPartys.SelectionChanged -= dataGridViewPartys_SelectionChanged;

                    dataGridViewPartys.AutoGenerateColumns = false;
                    SortableBindingList<Party> partyBindingList = new SortableBindingList<Party>(value);
                    dataGridViewPartys.DataSource = partyBindingList;

                    if (dataGridViewPartys.ColumnCount > Settings.Default.SortIndexPartys)
                    {
                        dataGridViewPartys.Sort(dataGridViewPartys.Columns[Settings.Default.SortIndexPartys],
                        Settings.Default.SortOrderPartys ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (partyBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _partys = value;

                    dataGridViewPartys.SelectionChanged += dataGridViewPartys_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Новый сотрудник.
        /// </summary>
        public Party Party
        {
            get
            {
                if (_isEdit)
                {
                    _party = new Party
                    {
                        PartyId = CurrentParty.PartyId,
                        Name = textBoxName.Text
                    };
                }
                else
                {
                    _party = new Party
                    {
                        Name = textBoxName.Text
                    };
                }
                return _party;
            }
            set
            {
                _party = value;
            }
        }

        /// <summary>
        /// Выбранный сотрудник.
        /// </summary>
        public Party CurrentParty
        {
            get
            {
                return _currentParty;
            }
            set
            {
                _currentParty = value;
            }
        }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        public List<PartyPerson> PartyPersons
        {
            get
            {
                if (_persons != null)
                {
                    labelCountPersons.Text = string.Format("{0} чел.", _persons.Count.ToString());
                }

                //PartyPersonComparer comparer = new PartyPersonComparer();
                //if (_persons != null)
                //{
                //    _persons.Sort(comparer);
                //}

                return _persons;
            }
            set
            {
                if (value != null)
                {
                    PartyPersonComparer comparer = new PartyPersonComparer();
                    value.Sort(comparer);
                    bindingSourcePersons.DataSource = value;
                    bindingSourcePersons.ResetBindings(true);
                    _persons = value;
                }
                else
                {
                    _persons = new List<PartyPerson>();
                }
            }
        }

        /// <summary>
        /// Новые сотрудники в партии.
        /// </summary>
        public List<PartyPerson> AddPersons
        {
            get
            {
                return _addPersons;
            }
            set
            {
                if (value != null)
                {
                    _addPersons = value;
                }
                else
                {
                    _addPersons = new List<PartyPerson>();
                }
            }
        }

        /// <summary>
        /// Сотрудники к удалению из партии. 
        /// </summary>
        public List<PartyPerson> DeletePersons
        {
            get
            {
                return _deletePersons;
            }
            set
            {
                if (value != null)
                {
                    _deletePersons = value;
                }
                else
                {
                    _deletePersons = new List<PartyPerson>();
                }
            }
        }

        /// <summary>
        /// Сотрудники к редактированию из партии. 
        /// </summary>
        public List<PartyPerson> EditPersons
        {
            get
            {
                return _editPersons;
            }
            set
            {
                if (value != null)
                {
                    _editPersons = value;
                }
                else
                {
                    _editPersons = new List<PartyPerson>();
                }
            }
        }

        /// <summary>
        /// Событие запускает сохранение сотрудника.
        /// </summary>
        public event Action<Party> Save;

        /// <summary>
        /// Событие запускает обновление сотрудника.
        /// </summary>
        public event Action<Party> Update;

        /// <summary>
        /// Событие запускает удаление сотрудника.
        /// </summary>
        public event Action<List<Party>> Delete;

        /// <summary>
        /// Событие запускает получение сотрудников.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        /// <summary>
        /// Сохранение партии.
        /// </summary>
        public event Action<Party, List<PartyPerson>> SaveParty;

        /// <summary>
        /// Обновление партии.
        /// </summary>
        public event Action<Party, List<PartyPerson>, List<PartyPerson>, List<PartyPerson>> UpdateParty;

        #endregion //IBaseView, IDirectoryPersonsView

        #region EventHandlers

        private void DirectoryPartysView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }
        private void DirectoryPartysView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl1.SelectedTab = TabCard;
            CurrentParty = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewPartys.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPartys.SelectedRows[countRow - 1];
                var party = row.DataBoundItem as Party;
                if (party != null)
                {
                    TabControl1.SelectedTab = TabCard;
                    SetButtonEnabled(false);

                    foreach (var partyPerson in PartyPersons)
                    {
                        partyPerson.Party_PartyId = 0;
                    }
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeletePartys();
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
            var countRow = dataGridViewPartys.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPartys.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewPartys.ClearSelection();
                    dataGridViewPartys.Rows[indexPrev].Selected = true;
                    dataGridViewPartys.CurrentCell = dataGridViewPartys[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewPartys.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPartys.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewPartys.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewPartys.ClearSelection();
                    dataGridViewPartys.Rows[indexNext].Selected = true;
                    dataGridViewPartys.CurrentCell = dataGridViewPartys[0, indexNext];
                }
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            var view = CompositionRoot.Resolve<DirectoryPersonsView>(); //(new IParameter[] { new ConstructorArgument("isStandard", false) });

            if (view.ShowDialog() == DialogResult.OK)
            {
                if (view.SelectedPersons != null)
                {
                    var isEq = false;

                    foreach (var personId in PartyPersons.Select(d => d.Person_PersonId))
                    {
                        if (view.SelectedPersons.Select(p => p.Person_PersonId).Contains(personId))
                            isEq = true;
                    }

                    if (!isEq)
                    {
                        PartyPersons.AddRange(view.SelectedPersons);

                        //if (dataGridViewPersons.RowCount > 0)
                        //{
                        //    if (!PartyPersons.Any(p => p.IsResponsible == true))
                        //    {
                        //        dataGridViewPersons.Rows[0].Cells["isResponsibleDataGridViewCheckBoxColumn"].Value = true;
                        //        //PartyPersons[0].IsResponsible = true;
                        //    }
                        //}

                        //if (PartyPersons.Count > 0)
                        //{
                        //    if (!PartyPersons.Any(p => p.IsResponsible == true))
                        //    {
                        //        PartyPersons[0].IsResponsible = true;
                        //    }
                        //}

                        bindingSourcePersons.DataSource = PartyPersons;
                        bindingSourcePersons.ResetBindings(true);



                        SetStateForChange();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Сотрудники не добавлены. Один или несколько сотрудников уже содержатся в данной партии."),
                            DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnDelPerson_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewPersons.SelectedRows.Count;

            if (countRow > 0)
            {
                //var persons = new List<Person>();

                var ids = new List<int>();
                foreach (DataGridViewRow row in dataGridViewPersons.SelectedRows)
                {
                    ids.Add(row.Index);
                }
                PartyPersons.RemoveRange(ids.Min(), ids.Count);
                //PartyPersons.OrderBy(p => p.Name).ToList();

                //if (PartyPersons.Count > 0)
                //{
                //    foreach (var person in PartyPersons)
                //    {
                //        person.IsResponsible = false;
                //    }

                //    var partyPerson = PartyPersons[0]; //.IsResponsible;

                //    if (partyPerson != null)
                //    {
                //        if (partyPerson.Party_PartyId != 0)
                //        {
                //            if (_editDGVPersons != null)
                //                _editDGVPersons.Add(e.RowIndex);
                //            SetButtonEnabled(false);

                //            _isEdit = true;
                //        }
                //    }
                //}

                bindingSourcePersons.DataSource = PartyPersons;
                bindingSourcePersons.ResetBindings(true);

                SetStateForChange();
            }
        }

        private void btnOK_ButtonClick(object sender, EventArgs e)
        {
            //if (!_isStandard)
            //{
            //    this.DialogResult = DialogResult.OK;
            //    Close();
            //}
            //else
            //{

            _resultDialog = DialogResult.OK;

            if (IsCheckSave())
            {
                Close();
            }
            //}
        }

        private void btnCancel_ButtonClick(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();

            _resultDialog = DialogResult.No;

            Close();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void dataGridViewPartys_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewPartys.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPartys.SelectedRows[countRow - 1];
                var party = row.DataBoundItem as Party;
                if (party != null)
                {
                    SetValues(party);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (!_isStandard)
            //{
            //    e.Cancel = e.TabPageIndex == 0;
            //}
            //else
            //{
            if (!IsCheckSave())
            {
                e.Cancel = e.TabPageIndex == 1;
            }
            //}
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (_row < dataGridViewPartys.RowCount && _col < dataGridViewPartys.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewPartys.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewPartys_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void mnuCopyDGVPersons_Click(object sender, EventArgs e)
        {
            if (_rowDVGPersons < dataGridViewPersons.RowCount && _colDVGPersons < dataGridViewPersons.ColumnCount && _rowDVGPersons >= 0 && _colDVGPersons >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewPersons.Rows[_rowDVGPersons].Cells[_colDVGPersons].Value.ToString());
            }
        }

        private void dataGridViewPersons_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _rowDVGPersons = e.RowIndex;
                _colDVGPersons = e.ColumnIndex;
            }
        }

        private void dataGridViewPartys_DoubleClick(object sender, EventArgs e)
        {
            TabControl1.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllPartys();
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
            GetAllPartys();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewPartys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl1.SelectedTab == tabList)
                {
                    DeletePartys();
                }
            }
        }

        private void DirectoryPartysView_KeyUp(object sender, KeyEventArgs e)
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
            GetAllPartys();

            dataGridViewPartys.ClearSelection();

            if (dataGridViewPartys.RowCount > 0)
            {
                dataGridViewPartys.CurrentCell = dataGridViewPartys[0, 0];
                dataGridViewPartys.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryPartysView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewPersons_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPersons.RowCount > 0)
            {
                if (e.ColumnIndex == 8)
                {
                    var partyPerson = dataGridViewPersons.Rows[e.RowIndex].DataBoundItem as PartyPerson;
                    if (partyPerson != null)
                    {
                        if (partyPerson.Party_PartyId != 0)
                        {
                            if (_editDGVPersons != null)
                                _editDGVPersons.Add(e.RowIndex);
                            SetButtonEnabled(false);

                            _isEdit = true;
                        }
                    }
            }
        }
        }

        private void dataGridViewPersons_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPersons.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
            {
                for (int i = 0; i < dataGridViewPersons.RowCount; i++)
                {
                    //if (dataGridViewPersons.Rows[i].Cells["isResponsibleDataGridViewCheckBoxColumn"] != dataGridViewPersons.CurrentCell)
                    //{
                    dataGridViewPersons.Rows[i].Cells["isResponsibleDataGridViewCheckBoxColumn"].Value = false;
                    //}
                }

                //if ((bool)dataGridViewPersons.CurrentCell.Value)
                //{
                //dataGridViewPersons.CurrentCell.Value = false;
                dataGridViewPersons.CurrentCell.Value = true;
                //}
                //else
                //{
                //    dataGridViewPersons.CurrentCell.Value = true;
                //}
            }
        }

        private void dataGridViewPartys_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewPartys.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewPartys.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewPersons_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewPersons.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewPersons.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewPartys_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexPartys = e.ColumnIndex;

                if (dataGridViewPartys.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderPartys = true;
                }
                else if (dataGridViewPartys.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderPartys = false;
                }
                else
                {
                    Settings.Default.SortOrderPartys = true;
                }
            }
            else
            {
                Settings.Default.SortIndexPartys = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryPartysView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewPartys = true;
            else
                Settings.Default.WindowStateMaximizedViewPartys = false;

            Settings.Default.WindowWidthViewPartys = this.Size.Width;
            Settings.Default.WindowHeightViewPartys = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeletePartys()
        {
            SavePosition(true, true);

            var partys = new List<Party>();

            foreach (DataGridViewRow row in dataGridViewPartys.SelectedRows)
            {
                var party = row.DataBoundItem as Party;
                if (party != null)
                {
                    partys.Add(new Party { PartyId = party.PartyId });
                }
            }

            if (!partys.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить партии (кол-во: {0})?", partys.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(partys);

                GetAllPartys();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewPartys.ClearSelection();

            if (dataGridViewPartys.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewPartys.Rows)
                    {
                        var party = row.DataBoundItem as Party;
                        if (party != null)
                        {
                            if (party.PartyId == SaveIndex)
                            {
                                dataGridViewPartys.CurrentCell = dataGridViewPartys[0, row.Index];
                                dataGridViewPartys.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewPartys.CurrentCell = dataGridViewPartys[0, 0];
                dataGridViewPartys.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewPartys.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewPartys.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewPartys.SelectedRows.Count < dataGridViewPartys.RowCount)
                    {
                        var selectedRow = dataGridViewPartys.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewPartys.SelectedRows[0].Index == dataGridViewPartys.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewPartys.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewPartys.RowCount)
                            {
                                row = dataGridViewPartys.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewPartys.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewPartys.RowCount)
                            {
                                row = dataGridViewPartys.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var party = row.DataBoundItem as Party;
                            if (party != null)
                            {
                                SaveIndex = party.PartyId;
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
                    var row = dataGridViewPartys.SelectedRows[selectIndex];
                    var party = row.DataBoundItem as Party;
                    if (party != null)
                    {
                        SaveIndex = party.PartyId;
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
            if (CurrentParty != null/* && _isStandard*/)
            {
                if (textBoxName.Text != CurrentParty.Name || IsEditPersons())
                {
                    SetButtonEnabled(false);
                }
                else
                {
                    SetButtonEnabled(true);
                }
            }
        }


        private bool IsEditPersons()
        {
            var isEditDirections = false;

            if (PartyPersons != null)
            {
                foreach (var personId in PartyPersons.Select(d => d.Person_PersonId))
                {
                    if (!CurrentParty.PartyPersons.Select(pp => pp.Person).Select(p => p.PersonId).Contains(personId))
                        isEditDirections = true;
                }
                foreach (var curPersonId in CurrentParty.PartyPersons.Select(pp => pp.Person).Select(p => p.PersonId))
                {
                    if (!PartyPersons.Select(p => p.Person_PersonId).Contains(curPersonId))
                        isEditDirections = true;
                }
            }

            return isEditDirections;
        }

        //var isEditDirections = false;

        //    if (Directions != null)
        //    {
        //        foreach (var directionId in Directions.Select(d => d.DirectionId))
        //        {
        //            if (!CurrentBusinessTrip.BusinessTripDirections.Select(bd=>bd.Direction).Select(d => d.DirectionId).Contains(directionId))
        //                isEditDirections = true;
        //        }
        //        foreach (var curDirectionId in CurrentBusinessTrip.BusinessTripDirections.Select(bd => bd.Direction).Select(d => d.DirectionId))
        //        {
        //            if (!Directions.Select(d => d.DirectionId).Contains(curDirectionId))
        //                isEditDirections = true;
        //        }
        //    }

        //    return isEditDirections;




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
        /// Установка полей партии.
        /// </summary>
        /// <param name="party">Партия.</param>
        private void SetValues(Party party)
        {
            CurrentParty = party;

            if (party != null)
            {
                PartyPersons = party.PartyPersons != null ? party.PartyPersons.ToList()/*.OrderBy(p => p.Name).ToList()*/ : new List<PartyPerson>();

                textBoxName.Text = party.Name;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            PartyPersons.Clear();

            textBoxName.Clear();
            dataGridViewPersons.Rows.Clear();
            //_savePosition = 0;
            if (_editDGVPersons != null)
                _editDGVPersons.Clear();

            textBoxName.Focus();
        }

        /// <summary>
        /// Получение всех партий.
        /// </summary>
        private void GetAllPartys()
        {
            if (GetAll != null)
                GetAll();

            Partys = Filters();

            if (Partys.Any())
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
                    GetAllPartys();

                    SetButtonEnabled(true);
                    if (dataGridViewPartys.RowCount > 0)
                    {
                        dataGridViewPartys.ClearSelection();
                        dataGridViewPartys.Rows[0].Selected = true;
                        dataGridViewPartys.CurrentCell = dataGridViewPartys[0, 0];
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
                if (UpdateParty != null)
                {
                    SavePosition();

                    var deletePersons = new List<PartyPerson>();
                    foreach (var curPerson in CurrentParty.PartyPersons)
                    {
                        if (!PartyPersons.Select(p => p.Person).Select(p=>p.PersonId).Contains(curPerson.Person.PersonId))
                            deletePersons.Add(new PartyPerson
                            {
                                Party_PartyId = CurrentParty != null ? CurrentParty.PartyId : 0,
                                Person_PersonId = curPerson.Person_PersonId,
                                IsResponsible = curPerson.IsResponsible
                            });
                    }

                    var addPersons = new List<PartyPerson>();
                    foreach (var person in PartyPersons)
                    {
                        if (!CurrentParty.PartyPersons.Select(pp => pp.Person).Select(p => p.PersonId).Contains(person.Person_PersonId))
                            addPersons.Add(new PartyPerson
                            {
                                Party_PartyId = CurrentParty != null ? CurrentParty.PartyId : 0,
                                Person_PersonId = person.Person_PersonId,
                                IsResponsible = person.IsResponsible
                            });
                    }

                    var editPersons = new List<PartyPerson>();

                    foreach (var index in _editDGVPersons.Distinct())
                    {
                        //if (dataGridViewPersons.RowCount)
                        if (dataGridViewPersons.RowCount > index)
                        {
                            var partyPerson = dataGridViewPersons.Rows[index].DataBoundItem as PartyPerson;
                            if (partyPerson != null)
                            {
                                if ((!addPersons.Select(d => d.Person_PersonId).Contains(partyPerson.Person_PersonId) || !deletePersons.Select(d => d.Person_PersonId).Contains(partyPerson.Person_PersonId)) && partyPerson.Party_PartyId != 0)
                                {
                                    editPersons.Add(new PartyPerson
                                    {
                                        Party_PartyId = CurrentParty != null ? CurrentParty.PartyId : 0,
                                        Person_PersonId = partyPerson.Person_PersonId,
                                        IsResponsible = partyPerson.IsResponsible
                                    });
                                }
                            }
                        }
                    }

                    DeletePersons = deletePersons;
                    AddPersons = addPersons;
                    EditPersons = editPersons;

                    UpdateParty(Party, AddPersons, DeletePersons, EditPersons);

                    GetAllPartys();

                    SelectObject();
                }
            }
            else
            {
                if (SaveParty != null)
                    SaveParty(Party, PartyPersons);

                GetAllPartys();
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
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                errorProvider.SetError(textBoxName, "Не задано наименование партии");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewPartys.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPartys.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Partys.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentParty = null;
            }
        }

        public List<Party> Filters()
        {
            var partys = Partys;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        partys = partys.Where(o => o.PartyId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Наименование":
                        {
                            partys = partys.Where(o => o.Name.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }

            return partys;
        }


        #endregion //PrivateMethods

    }
}