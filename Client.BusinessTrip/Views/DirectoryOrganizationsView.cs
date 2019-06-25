using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Client.BusinessTrip.Helpers;
using Client.BusinessTrip.Helpers.Sortable;
using Client.BusinessTrip.Properties;
using System.ComponentModel;
using System.Drawing;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Справочник организаций".
    /// </summary>
    public partial class DirectoryOrganizationsView : Form, IDirectoryOrganizationsView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник организаций";

        private List<string> _listSearch = new List<string> { "Наименование", /*"Ид",*/ "Полное наименование" };

        private IDirectoryOrganizationsPresenter _presenter;

        private IBaseModel<Organization> _model;

        private List<Organization> _organizations;

        private Organization _organization;

        private Organization _currentOrganization;

        //режим редактирования.
        private bool _isEdit = false;

        //стандартный режим / режим выбора объекта.
        //private bool _isStandard = true;

        private int? _saveIndex = 0;

        private int _col = -1;
        private int _row = -1;

        private bool _oneShow = true;

        private DialogResult _resultDialog = DialogResult.No;

        #endregion //Private Fields

        #region Constructors

        public DirectoryOrganizationsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник организаций".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryOrganizationsView(IDirectoryOrganizationsPresenter presenter, IBaseModel<Organization> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewOrganizations)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewOrganizations, Settings.Default.WindowHeightViewOrganizations);
            }

            #region dataGridviewOrganizations settings

            //dataGridviewOrganizations.Columns.Add("OrganizationIdColumn", "Ид");
            //dataGridviewOrganizations.Columns["OrganizationIdColumn"].DataPropertyName = "OrganizationId";
            //dataGridviewOrganizations.Columns["OrganizationIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridviewOrganizations.Columns["OrganizationIdColumn"].Width = 50;
            //dataGridviewOrganizations.Columns["OrganizationIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridviewOrganizations.Columns.Add("ShortNameColumn", "Наименование");
            dataGridviewOrganizations.Columns["ShortNameColumn"].DataPropertyName = "ShortName";
            dataGridviewOrganizations.Columns["ShortNameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridviewOrganizations.Columns["ShortNameColumn"].FillWeight = 75;

            dataGridviewOrganizations.Columns.Add("NameColumn", "Полное наименование");
            dataGridviewOrganizations.Columns["NameColumn"].DataPropertyName = "Name";
            dataGridviewOrganizations.Columns["NameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridviewOrganizations.Columns["NameColumn"].FillWeight = 125;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridviewOrganizations.ContextMenuStrip = contextMenu;

            dataGridviewOrganizations.DoubleBuffered(true);

            #endregion //dataGridviewOrganizations settings

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Organizations != null)
            {
                if (Organizations.Count > 0)
                {
                    CurrentOrganization = Organizations[0];
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

        #region IBaseView, IDirectoryOrganizationsView

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
        /// Список организаций.
        /// </summary>
        public List<Organization> Organizations
        {
            get
            {
                return _organizations;
            }

            set
            {
                if (value != null)
                {
                    dataGridviewOrganizations.SelectionChanged -= dataGridviewOrganizations_SelectionChanged;

                    dataGridviewOrganizations.AutoGenerateColumns = false;
                    SortableBindingList<Organization> organizationsBindingList = new SortableBindingList<Organization>(value);
                    dataGridviewOrganizations.DataSource = organizationsBindingList;

                    if (dataGridviewOrganizations.ColumnCount > Settings.Default.SortIndexOrganizations)
                    {
                        dataGridviewOrganizations.Sort(dataGridviewOrganizations.Columns[Settings.Default.SortIndexOrganizations],
                        Settings.Default.SortOrderOrganizations ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (organizationsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _organizations = value;

                    dataGridviewOrganizations.SelectionChanged += dataGridviewOrganizations_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Организация.
        /// </summary>
        public Organization Organization
        {
            get
            {
                if (_isEdit)
                {
                    _organization = new Organization
                    {
                        OrganizationId = CurrentOrganization.OrganizationId,
                        Name = textBoxName.Text,
                        ShortName = textBoxShortName.Text
                    };
                }
                else
                {
                    _organization = new Organization
                    {
                        Name = textBoxName.Text,
                        ShortName = textBoxShortName.Text
                    };
                }
                return _organization;
            }
            set
            {
                _organization = value;
            }
        }

        /// <summary>
        /// Выбранная организация.
        /// </summary>
        public Organization CurrentOrganization
        {
            get
            {
                return _currentOrganization;
            }
            set
            {
                _currentOrganization = value;
            }
        }

        /// <summary>
        /// Событие запускает сохранение организации.
        /// </summary>
        public event Action<Organization> Save;

        /// <summary>
        /// Событие запускает обновление организации.
        /// </summary>
        public event Action<Organization> Update;

        /// <summary>
        /// Событие запускает удаление организации.
        /// </summary>
        public event Action<List<Organization>> Delete;

        /// <summary>
        /// Событие запускает получение организации.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryOrganizationsView

        #region EventHandlers

        private void DirectoryOrganizationsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryOrganizationsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentOrganization = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridviewOrganizations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewOrganizations.SelectedRows[countRow - 1];
                var organization = row.DataBoundItem as Organization;
                if (organization != null)
                {
                    TabControl.SelectedTab = TabCard;
                    //SetValues(organization);
                    SetButtonEnabled(false);
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeleteOrganizations();
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
            var countRow = dataGridviewOrganizations.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridviewOrganizations.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridviewOrganizations.ClearSelection();
                    dataGridviewOrganizations.Rows[indexPrev].Selected = true;
                    dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridviewOrganizations.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridviewOrganizations.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridviewOrganizations.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridviewOrganizations.ClearSelection();
                    dataGridviewOrganizations.Rows[indexNext].Selected = true;
                    dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, indexNext];
                }
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

        private void dataGridviewOrganizations_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridviewOrganizations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewOrganizations.SelectedRows[countRow - 1];
                var organization = row.DataBoundItem as Organization;
                if (organization != null)
                {
                    SetValues(organization);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxShortName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
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
            if (_row < dataGridviewOrganizations.RowCount && _col < dataGridviewOrganizations.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridviewOrganizations.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridviewOrganizations_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridviewOrganizations_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllorganizations();
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
            GetAllorganizations();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridviewOrganizations_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeleteOrganizations();
                }
            }
        }

        private void DirectoryOrganizationsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnSave.Enabled == false && TabControl.SelectedTab == tabList)
            {
                if (IsCheckSave())
                {
                    _resultDialog = DialogResult.OK;

                    Close();
                }
            }
            else if (e.KeyCode == Keys.Escape && btnSave.Enabled == false /*&& TabControl.SelectedTab == tabList*/)
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
            GetAllorganizations();

            dataGridviewOrganizations.ClearSelection();

            if (dataGridviewOrganizations.RowCount > 0)
            {
                dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, 0];
                dataGridviewOrganizations.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryOrganizationsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridviewOrganizations_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridviewOrganizations.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridviewOrganizations.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridviewOrganizations_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexOrganizations = e.ColumnIndex;

                if (dataGridviewOrganizations.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderOrganizations = true;
                }
                else if (dataGridviewOrganizations.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderOrganizations = false;
                }
                else
                {
                    Settings.Default.SortOrderOrganizations = true;
                }
            }
            else
            {
                Settings.Default.SortIndexOrganizations = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryOrganizationsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewOrganizations = true;
            else
                Settings.Default.WindowStateMaximizedViewOrganizations = false;

            Settings.Default.WindowWidthViewOrganizations = this.Size.Width;
            Settings.Default.WindowHeightViewOrganizations = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteOrganizations()
        {
            SavePosition(true, true);

            var organizations = new List<Organization>();

            foreach (DataGridViewRow row in dataGridviewOrganizations.SelectedRows)
            {
                var organization = row.DataBoundItem as Organization;
                if (organization != null)
                {
                    organizations.Add(new Organization { OrganizationId = organization.OrganizationId });
                }
            }

            if (!organizations.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить организации (кол-во: {0})?", organizations.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(organizations);

                GetAllorganizations();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridviewOrganizations.ClearSelection();

            if (dataGridviewOrganizations.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridviewOrganizations.Rows)
                    {
                        var organization = row.DataBoundItem as Organization;
                        if (organization != null)
                        {
                            if (organization.OrganizationId == SaveIndex)
                            {
                                dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, row.Index];
                                dataGridviewOrganizations.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, 0];
                dataGridviewOrganizations.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridviewOrganizations.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridviewOrganizations.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridviewOrganizations.SelectedRows.Count < dataGridviewOrganizations.RowCount)
                    {
                        var selectedRow = dataGridviewOrganizations.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridviewOrganizations.SelectedRows[0].Index == dataGridviewOrganizations.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridviewOrganizations.SelectedRows.Count;
                            }
                            if (newIndex < dataGridviewOrganizations.RowCount)
                            {
                                row = dataGridviewOrganizations.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridviewOrganizations.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridviewOrganizations.RowCount)
                            {
                                row = dataGridviewOrganizations.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var organization = row.DataBoundItem as Organization;
                            if (organization != null)
                            {
                                SaveIndex = organization.OrganizationId;
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
                    var row = dataGridviewOrganizations.SelectedRows[selectIndex];
                    var organization = row.DataBoundItem as Organization;
                    if (organization != null)
                    {
                        SaveIndex = organization.OrganizationId;
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
            if (CurrentOrganization != null /*&& _isStandard*/)
            {
                if (textBoxName.Text != CurrentOrganization.Name || textBoxShortName.Text != CurrentOrganization.ShortName)
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
                btnSave.Enabled = false;
                btnRefresh.Enabled = true;

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
                btnSave.Enabled = true;
                btnRefresh.Enabled = false;

                btnClear.Enabled = false;
                comboBoxSearch.Enabled = false;
                textBoxSearch.Enabled = false;
            }
        }

        /// <summary>
        /// Установка полей организации.
        /// </summary>
        /// <param name="organization">Организация.</param>
        private void SetValues(Organization organization)
        {
            CurrentOrganization = organization;

            if (organization != null)
            {
                textBoxName.Text = organization.Name;
                textBoxShortName.Text = organization.ShortName;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxName.Clear();
            textBoxShortName.Clear();
            //_savePosition = 0;

            textBoxShortName.Focus();
        }

        /// <summary>
        /// Получение всех сотрудников.
        /// </summary>
        private void GetAllorganizations()
        {
            if (GetAll != null)
                GetAll();

            Organizations = Filters();

            if (Organizations.Any())
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
                    GetAllorganizations();

                    SetButtonEnabled(true);
                    if (dataGridviewOrganizations.RowCount > 0)
                    {
                        dataGridviewOrganizations.ClearSelection();
                        dataGridviewOrganizations.Rows[0].Selected = true;
                        dataGridviewOrganizations.CurrentCell = dataGridviewOrganizations[0, 0];
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

                    Update(Organization);

                    GetAllorganizations();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Organization);

                GetAllorganizations();
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
            if (string.IsNullOrWhiteSpace(textBoxShortName.Text))
            {
                errorProvider.SetError(textBoxShortName, "Не задано наименование организации");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridviewOrganizations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewOrganizations.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Organizations.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentOrganization = null;
            }
        }

        public List<Organization> Filters()
        {
            var organizations = Organizations;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        organizations = organizations.Where(o => o.OrganizationId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Наименование":
                        {
                            organizations = organizations.Where(o => o.ShortName.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Полное наименование":
                        {
                            organizations = organizations.Where(o => o.Name.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }

            return organizations;
        }

        #endregion //Private Methods

    }
}
