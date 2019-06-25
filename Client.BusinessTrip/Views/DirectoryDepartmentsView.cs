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
    public partial class DirectoryDepartmentsView : Form, IDirectoryDepartmentsView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник подразделений";

        private List<string> _categories = new List<string> { "Группа", "Отдел", "Управление", "Другой" };
        private List<string> _listSearch = new List<string> { "Номер", "Наименование", "Тип подразделения" };
        private List<string> _categoriesSearch = new List<string> { "Все", "Группа", "Отдел", "Управление", "Другой" };

        private IDirectoryDepartmentsPresenter _presenter;

        private IBaseModel<Department> _model;

        private List<Department> _departments;

        private Department _department;

        private Department _currentDepartment;

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

        public DirectoryDepartmentsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник подразделений".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryDepartmentsView(IDirectoryDepartmentsPresenter presenter, IBaseModel<Department> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewDepartments)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewDepartments, Settings.Default.WindowHeightViewDepartments);
            }

            #region dataGridviewDepartment settings

            //dataGridviewDepartments.Columns.Add("DepartmentIdColumn", "Ид");
            //dataGridviewDepartments.Columns["DepartmentIdColumn"].DataPropertyName = "DepartmentId";
            //dataGridviewDepartments.Columns["DepartmentIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridviewDepartments.Columns["DepartmentIdColumn"].Width = 50;
            //dataGridviewDepartments.Columns["DepartmentIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridviewDepartments.Columns.Add("NumberColumn", "Номер");
            dataGridviewDepartments.Columns["NumberColumn"].DataPropertyName = "Number";
            dataGridviewDepartments.Columns["NumberColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridviewDepartments.Columns["NumberColumn"].FillWeight = 71;

            dataGridviewDepartments.Columns.Add("CategoryColumn", "Тип подразделения");
            dataGridviewDepartments.Columns["CategoryColumn"].DataPropertyName = "Category";
            dataGridviewDepartments.Columns["CategoryColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridviewDepartments.Columns["CategoryColumn"].FillWeight = 100;

            dataGridviewDepartments.Columns.Add("NominativeColumn", "Наименование");
            dataGridviewDepartments.Columns["NominativeColumn"].DataPropertyName = "Nominative";
            dataGridviewDepartments.Columns["NominativeColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridviewDepartments.Columns["NominativeColumn"].FillWeight = 125;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridviewDepartments.ContextMenuStrip = contextMenu;

            dataGridviewDepartments.DoubleBuffered(true);

            #endregion //dataGridviewDepartment settings

            comboBoxCategory.DataSource = _categories;
            comboBoxCategory.SelectedIndex = 0;

            comboBoxCategoriesSearch.Items.AddRange(_categoriesSearch.ToArray());
            comboBoxCategoriesSearch.SelectedIndex = 0;

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Departments != null)
            {
                if (Departments.Count > 0)
                {
                    CurrentDepartment = Departments[0];
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

        #region IBaseView, IDirectoryDepartmentsView

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
        /// Список подразделений.
        /// </summary>
        public List<Department> Departments
        {
            get
            {
                return _departments;
            }

            set
            {
                if (value != null)
                {
                    dataGridviewDepartments.SelectionChanged -= dataGridviewDepartments_SelectionChanged;

                    dataGridviewDepartments.AutoGenerateColumns = false;
                    SortableBindingList<Department> departmentsBindingList = new SortableBindingList<Department>(value);
                    dataGridviewDepartments.DataSource = departmentsBindingList;

                    if (dataGridviewDepartments.ColumnCount > Settings.Default.SortIndexDepartments)
                    {
                        dataGridviewDepartments.Sort(dataGridviewDepartments.Columns[Settings.Default.SortIndexDepartments],
                        Settings.Default.SortOrderDepartments ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (departmentsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _departments = value;

                    dataGridviewDepartments.SelectionChanged += dataGridviewDepartments_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Организация.
        /// </summary>
        public Department Department
        {
            get
            {
                if (_isEdit)
                {
                    _department = new Department
                    {
                        DepartmentId = CurrentDepartment.DepartmentId,
                        Number = textBoxNumber.Text,
                        Nominative = textBoxNominative.Text,
                        Category = comboBoxCategory.SelectedItem.ToString()
                        //Genitive = textBoxGenitive.Text,
                        //Dative = textBoxDative.Text
                    };
                }
                else
                {
                    _department = new Department
                    {
                        Number = textBoxNumber.Text,
                        Nominative = textBoxNominative.Text,
                        Category = comboBoxCategory.SelectedItem.ToString()
                        //Genitive = textBoxGenitive.Text,
                        //Dative = textBoxDative.Text
                    };
                }
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        /// <summary>
        /// Выбранная организация.
        /// </summary>
        public Department CurrentDepartment
        {
            get
            {
                return _currentDepartment;
            }
            set
            {
                _currentDepartment = value;
            }
        }

        /// <summary>
        /// Событие запускает сохранение организации.
        /// </summary>
        public event Action<Department> Save;

        /// <summary>
        /// Событие запускает обновление организации.
        /// </summary>
        public event Action<Department> Update;

        /// <summary>
        /// Событие запускает удаление организации.
        /// </summary>
        public event Action<List<Department>> Delete;

        /// <summary>
        /// Событие запускает получение организации.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryDepartmentsView

        #region EventHandlers

        private void DirectoryDepartmentsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryDepartmentsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentDepartment = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridviewDepartments.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewDepartments.SelectedRows[countRow - 1];
                var department = row.DataBoundItem as Department;
                if (department != null)
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
            DeleteDepartments();
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
            var countRow = dataGridviewDepartments.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridviewDepartments.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridviewDepartments.ClearSelection();
                    dataGridviewDepartments.Rows[indexPrev].Selected = true;
                    dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridviewDepartments.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridviewDepartments.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridviewDepartments.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridviewDepartments.ClearSelection();
                    dataGridviewDepartments.Rows[indexNext].Selected = true;
                    dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, indexNext];
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

        private void dataGridviewDepartments_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridviewDepartments.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewDepartments.SelectedRows[countRow - 1];
                var department = row.DataBoundItem as Department;
                if (department != null)
                {
                    SetValues(department);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxNumber_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxNominative_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        //private void textBoxGenitive_TextChanged(object sender, EventArgs e)
        //{
        //    SetStateForChange();
        //}

        //private void textBoxDative_TextChanged(object sender, EventArgs e)
        //{
        //    SetStateForChange();
        //}

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
            if (_row < dataGridviewDepartments.RowCount && _col < dataGridviewDepartments.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridviewDepartments.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridviewDepartments_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridviewDepartments_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAlldepartments();
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
            if ((string)comboBoxSearch.SelectedItem == "Тип подразделения")
            {
                textBoxSearch.Visible = false;
                comboBoxCategoriesSearch.Visible = true;
            }
            else
            {
                textBoxSearch.Visible = true;
                comboBoxCategoriesSearch.Visible = false;
            }

            SavePosition();
            GetAlldepartments();
            SelectObject();
            ResetLebelCount();
        }

        private void comboBoxCategoriesSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAlldepartments();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridviewDepartments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeleteDepartments();
                }
            }
        }

        private void DirectoryDepartmentsView_KeyUp(object sender, KeyEventArgs e)
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
            GetAlldepartments();

            dataGridviewDepartments.ClearSelection();

            if (dataGridviewDepartments.RowCount > 0)
            {
                dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, 0];
                dataGridviewDepartments.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryDepartmentsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridviewDepartments_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridviewDepartments.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridviewDepartments.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridviewDepartments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexDepartments = e.ColumnIndex;

                if (dataGridviewDepartments.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderDepartments = true;
                }
                else if (dataGridviewDepartments.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderDepartments = false;
                }
                else
                {
                    Settings.Default.SortOrderDepartments = true;
                }
            }
            else
            {
                Settings.Default.SortIndexDepartments = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryDepartmentsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewDepartments = true;
            else
                Settings.Default.WindowStateMaximizedViewDepartments = false;

            Settings.Default.WindowWidthViewDepartments = this.Size.Width;
            Settings.Default.WindowHeightViewDepartments = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteDepartments()
        {
            SavePosition(true, true);

            var departments = new List<Department>();

            foreach (DataGridViewRow row in dataGridviewDepartments.SelectedRows)
            {
                var department = row.DataBoundItem as Department;
                if (department != null)
                {
                    departments.Add(new Department { DepartmentId = department.DepartmentId });
                }
            }

            if (!departments.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить подразделения (кол-во: {0})?", departments.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(departments);

                GetAlldepartments();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridviewDepartments.ClearSelection();

            if (dataGridviewDepartments.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridviewDepartments.Rows)
                    {
                        var department = row.DataBoundItem as Department;
                        if (department != null)
                        {
                            if (department.DepartmentId == SaveIndex)
                            {
                                dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, row.Index];
                                dataGridviewDepartments.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, 0];
                dataGridviewDepartments.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridviewDepartments.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridviewDepartments.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridviewDepartments.SelectedRows.Count < dataGridviewDepartments.RowCount)
                    {
                        var selectedRow = dataGridviewDepartments.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridviewDepartments.SelectedRows[0].Index == dataGridviewDepartments.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridviewDepartments.SelectedRows.Count;
                            }
                            if (newIndex < dataGridviewDepartments.RowCount)
                            {
                                row = dataGridviewDepartments.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridviewDepartments.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridviewDepartments.RowCount)
                            {
                                row = dataGridviewDepartments.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var department = row.DataBoundItem as Department;
                            if (department != null)
                            {
                                SaveIndex = department.DepartmentId;
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
                    var row = dataGridviewDepartments.SelectedRows[selectIndex];
                    var department = row.DataBoundItem as Department;
                    if (department != null)
                    {
                        SaveIndex = department.DepartmentId;
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
            if (CurrentDepartment != null /*&& _isStandard*/)
            {
                if (textBoxNumber.Text != CurrentDepartment.Number || textBoxNominative.Text != CurrentDepartment.Nominative || comboBoxCategory.SelectedItem.ToString()  != CurrentDepartment.Category /*||*/
                    /*textBoxGenitive.Text != CurrentDepartment.Genitive || textBoxDative.Text != CurrentDepartment.Dative*/)
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
                comboBoxCategoriesSearch.Enabled = true;

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
                comboBoxCategoriesSearch.Enabled = false;

                btnClear.Enabled = false;
                comboBoxSearch.Enabled = false;
                textBoxSearch.Enabled = false;
            }
        }

        /// <summary>
        /// Установка полей подразделения.
        /// </summary>
        /// <param name="department">Подразделение.</param>
        private void SetValues(Department department)
        {
            CurrentDepartment = department;

            if (department != null)
            {
                textBoxNumber.Text = department.Number;
                textBoxNominative.Text = department.Nominative;
                comboBoxCategory.SelectedItem = department.Category;
                //textBoxGenitive.Text = department.Genitive;
                //textBoxDative.Text = department.Dative;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxNumber.Clear();
            textBoxNominative.Clear();
            comboBoxCategory.SelectedIndex = 0;
            //textBoxGenitive.Clear();
            //textBoxDative.Clear();
            //_savePosition = 0;

            textBoxNumber.Focus();
        }

        /// <summary>
        /// Получение всех подразделений.
        /// </summary>
        private void GetAlldepartments()
        {
            if (GetAll != null)
                GetAll();

            Departments = Filters();

            if (Departments.Any())
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
                    GetAlldepartments();

                    SetButtonEnabled(true);
                    if (dataGridviewDepartments.RowCount > 0)
                    {
                        dataGridviewDepartments.ClearSelection();
                        dataGridviewDepartments.Rows[0].Selected = true;
                        dataGridviewDepartments.CurrentCell = dataGridviewDepartments[0, 0];
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

                    Update(Department);

                    GetAlldepartments();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Department);

                GetAlldepartments();
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
            //if (string.IsNullOrWhiteSpace(textBoxNumber.Text))
            //{
            //    errorProvider.SetError(textBoxNumber, "Не задан номер подразделения");
            //    ok = false;
            //}
            /*else */if (string.IsNullOrWhiteSpace(textBoxNominative.Text))
            {
                errorProvider.SetError(textBoxNominative, "Не задано наименование");
                ok = false;
            }
            //else if (string.IsNullOrWhiteSpace(textBoxGenitive.Text))
            //{
            //    errorProvider.SetError(textBoxGenitive, "Не задано наименование в родительном падеже");
            //    ok = false;
            //}
            //else if (string.IsNullOrWhiteSpace(textBoxDative.Text))
            //{
            //    errorProvider.SetError(textBoxDative, "Не задано наименование в дательном падеже");
            //    ok = false;
            //}
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridviewDepartments.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridviewDepartments.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Departments.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentDepartment = null;
            }
        }

        public List<Department> Filters()
        {
            var departments = Departments;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text) || (string)comboBoxCategoriesSearch.SelectedItem != "Тип подразделения")
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        departments = departments.Where(o => o.DepartmentId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Наименование":
                        {
                            departments = departments.Where(o => o.Nominative.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Номер":
                        {
                            departments = departments.Where(o => o.Number.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Тип подразделения":
                        {
                            if ((string)comboBoxCategoriesSearch.SelectedItem == "Все")
                            {
                                break;
                            }
                            departments = departments.Where(o => o.Category == (string)comboBoxCategoriesSearch.SelectedItem).ToList();
                            break;
                        }

                }
            }

            return departments;
        }

        #endregion //Private Methods

    }
}
