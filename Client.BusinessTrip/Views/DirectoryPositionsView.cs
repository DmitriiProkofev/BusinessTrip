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
using Client.BusinessTrip.Helpers.HelperReports;
using Client.BusinessTrip.Properties;
using System.ComponentModel;
using System.Drawing;

namespace Client.BusinessTrip.Views
{
    public partial class DirectoryPositionsView : Form, IDirectoryPositionsView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник должностей";

        //private List<string> _categories = new List<string> { "Группа", "Отдел", "Специалист", "Управление", "Нет подразделения" };
        private List<string> _listSearch = new List<string> { "Наименование"/*, "Категория", "Ид"*/ };
        //private List<string> _categoriesSearch = new List<string> { "Все", "Группа", "Отдел", "Специалист", "Управление", "Нет подразделения" };

        private IDirectoryPositionsPresenter _presenter;

        private IBaseModel<Position> _model;

        private List<Position> _positions;

        private Position _position;

        private Position _currentPosition;

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

        public DirectoryPositionsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник должностей".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryPositionsView(IDirectoryPositionsPresenter presenter, IBaseModel<Position> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewPositions)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewPositions, Settings.Default.WindowHeightViewPositions);
            }

            #region dataGridViewPositions settings

            //dataGridViewPositions.Columns.Add("PositionIdColumn", "Ид");
            //dataGridViewPositions.Columns["PositionIdColumn"].DataPropertyName = "PositionId";
            //dataGridViewPositions.Columns["PositionIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewPositions.Columns["PositionIdColumn"].Width = 50;
            //dataGridViewPositions.Columns["PositionIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridViewPositions.Columns.Add("NominativeColumn", "Наименование");
            dataGridViewPositions.Columns["NominativeColumn"].DataPropertyName = "Nominative";
            dataGridViewPositions.Columns["NominativeColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPositions.Columns["NominativeColumn"].FillWeight = 125;

            //dataGridViewPositions.Columns.Add("CategoryColumn", "Тип подразделения");
            //dataGridViewPositions.Columns["CategoryColumn"].DataPropertyName = "Category";
            //dataGridViewPositions.Columns["CategoryColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewPositions.Columns["CategoryColumn"].FillWeight = 100;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewPositions.ContextMenuStrip = contextMenu;

            dataGridViewPositions.DoubleBuffered(true);

            #endregion //dataGridViewPositions settings

            //comboBoxCategory.DataSource = _categories;
            //comboBoxCategory.SelectedIndex = 0;

            //comboBoxCategoriesSearch.Items.AddRange(_categoriesSearch.ToArray());
            //comboBoxCategoriesSearch.SelectedIndex = 0;

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Positions != null)
            {
                if (Positions.Count > 0)
                {
                    CurrentPosition = Positions[0];
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

            //var positions = load.LoadPosition();

            //foreach (var position in positions)
            //{
            //    if (Save != null)
            //        Save(position);
            //}
        }


        #endregion //Constructors

        #region IBaseView, IDirectoryPositionsView

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

        public List<Position> Positions
        {
            get
            {
                return _positions;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewPositions.SelectionChanged -= dataGridViewPositions_SelectionChanged;

                    dataGridViewPositions.AutoGenerateColumns = false;
                    SortableBindingList<Position> positionsBindingList = new SortableBindingList<Position>(value);
                    dataGridViewPositions.DataSource = positionsBindingList;

                    if (dataGridViewPositions.ColumnCount > Settings.Default.SortIndexPositions)
                    {
                        dataGridViewPositions.Sort(dataGridViewPositions.Columns[Settings.Default.SortIndexPositions],
                        Settings.Default.SortOrderPositions ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (positionsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _positions = value;

                    dataGridViewPositions.SelectionChanged += dataGridViewPositions_SelectionChanged;
                }
            }
        }

        public Position CurrentPosition
        {
            get
            {
                return _currentPosition;
            }

            set
            {
                _currentPosition = value;
            }
        }

        public Position Position
        {
            get
            {
                if (_isEdit)
                {
                    _position = new Position
                    {
                        PositionId = CurrentPosition.PositionId,
                        Nominative = textBoxNominative.Text,
                        Genitive = textBoxGenitive.Text,
                        Dative = textBoxDative.Text,
                        //Category = comboBoxCategory.SelectedItem.ToString()
                    };
                }
                else
                {
                    _position = new Position
                    {
                        Nominative = textBoxNominative.Text,
                        Genitive = textBoxGenitive.Text,
                        Dative = textBoxDative.Text,
                        //Category = comboBoxCategory.SelectedItem.ToString()
                    };
                }
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public event Action<Position> Update;
        public event Action<Position> Save;
        public event Action<List<Position>> Delete;
        public event Action GetAll;
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryPositionsView

        #region EventHandlers

        private void DirectoryPositionsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryPositionsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentPosition = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewPositions.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPositions.SelectedRows[countRow - 1];
                var position = row.DataBoundItem as Position;
                if (position != null)
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
            DeletePositions();
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
            var countRow = dataGridViewPositions.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPositions.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewPositions.ClearSelection();
                    dataGridViewPositions.Rows[indexPrev].Selected = true;
                    dataGridViewPositions.CurrentCell = dataGridViewPositions[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewPositions.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewPositions.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewPositions.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewPositions.ClearSelection();
                    dataGridViewPositions.Rows[indexNext].Selected = true;
                    dataGridViewPositions.CurrentCell = dataGridViewPositions[0, indexNext];
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

        private void dataGridViewPositions_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewPositions.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPositions.SelectedRows[countRow - 1];
                var position = row.DataBoundItem as Position;
                if (position != null)
                {
                    SetValues(position);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxNominative_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxGenitive_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxDative_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
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
            if (_row < dataGridViewPositions.RowCount && _col < dataGridViewPositions.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewPositions.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewPositions_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewPositions_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
            textBoxSearch.Focus();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllpositions();
            SelectObject();
            ResetLebelCount();
            textBoxSearch.Focus();
        }

        private void comboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if ((string)comboBoxSearch.SelectedItem == "Категория")
            //{
            //    textBoxSearch.Visible = false;
            //    comboBoxCategoriesSearch.Visible = true;
            //}
            //else
            //{
            //    textBoxSearch.Visible = true;
            //    comboBoxCategoriesSearch.Visible = false;
            //}

            SavePosition();
            GetAllpositions();
            SelectObject();
            ResetLebelCount();
        }

        private void comboBoxCategoriesSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllpositions();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewPositions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeletePositions();
                }
            }
        }

        private void DirectoryPositionsView_KeyUp(object sender, KeyEventArgs e)
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
            GetAllpositions();

            dataGridViewPositions.ClearSelection();

            if (dataGridViewPositions.RowCount > 0)
            {
                dataGridViewPositions.CurrentCell = dataGridViewPositions[0, 0];
                dataGridViewPositions.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryPositionsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewPositions_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewPositions.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewPositions.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewPositions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexPositions = e.ColumnIndex;

                if (dataGridViewPositions.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderPositions = true;
                }
                else if (dataGridViewPositions.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderPositions = false;
                }
                else
                {
                    Settings.Default.SortOrderPositions = true;
                }
            }
            else
            {
                Settings.Default.SortIndexPositions = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryPositionsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewPositions = true;
            else
                Settings.Default.WindowStateMaximizedViewPositions = false;

            Settings.Default.WindowWidthViewPositions = this.Size.Width;
            Settings.Default.WindowHeightViewPositions = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeletePositions()
        {
            SavePosition(true, true);

            var positions = new List<Position>();

            foreach (DataGridViewRow row in dataGridViewPositions.SelectedRows)
            {
                var position = row.DataBoundItem as Position;
                if (position != null)
                {
                    positions.Add(new Position { PositionId = position.PositionId });
                }
            }

            if (!positions.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить должности (кол-во: {0})?", positions.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(positions);

                GetAllpositions();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewPositions.ClearSelection();

            if (dataGridViewPositions.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewPositions.Rows)
                    {
                        var position = row.DataBoundItem as Position;
                        if (position != null)
                        {
                            if (position.PositionId == SaveIndex)
                            {
                                dataGridViewPositions.CurrentCell = dataGridViewPositions[0, row.Index];
                                dataGridViewPositions.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewPositions.CurrentCell = dataGridViewPositions[0, 0];
                dataGridViewPositions.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewPositions.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewPositions.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewPositions.SelectedRows.Count < dataGridViewPositions.RowCount)
                    {
                        var selectedRow = dataGridViewPositions.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewPositions.SelectedRows[0].Index == dataGridViewPositions.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewPositions.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewPositions.RowCount)
                            {
                                row = dataGridViewPositions.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewPositions.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewPositions.RowCount)
                            {
                                row = dataGridViewPositions.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var position = row.DataBoundItem as Position;
                            if (position != null)
                            {
                                SaveIndex = position.PositionId;
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
                    var row = dataGridViewPositions.SelectedRows[selectIndex];
                    var position = row.DataBoundItem as Position;
                    if (position != null)
                    {
                        SaveIndex = position.PositionId;
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
            if (CurrentPosition != null /*&& _isStandard*/)
            {
                if (textBoxNominative.Text != CurrentPosition.Nominative || textBoxGenitive.Text != CurrentPosition.Genitive || textBoxDative.Text != CurrentPosition.Dative)
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
                //comboBoxCategoriesSearch.Enabled = true;

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
                //comboBoxCategoriesSearch.Enabled = false;

                btnClear.Enabled = false;
                comboBoxSearch.Enabled = false;
                textBoxSearch.Enabled = false;
            }
        }

        /// <summary>
        /// Установка полей должности.
        /// </summary>
        /// <param name="position">Должность.</param>
        private void SetValues(Position position)
        {
            CurrentPosition = position;

            if (position != null)
            {
                textBoxNominative.Text = position.Nominative;
                textBoxGenitive.Text = position.Genitive;
                textBoxDative.Text = position.Dative;
                //comboBoxCategory.SelectedItem = position.Category;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxNominative.Clear();
            textBoxGenitive.Clear();
            textBoxDative.Clear();
            //_savePosition = 0;

            textBoxNominative.Focus();
        }

        /// <summary>
        /// Получение всех должностей.
        /// </summary>
        private void GetAllpositions()
        {
            if (GetAll != null)
                GetAll();

            Positions = Filters();

            if (Positions.Any())
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
                    GetAllpositions();

                    SetButtonEnabled(true);
                    if (dataGridViewPositions.RowCount > 0)
                    {
                        dataGridViewPositions.ClearSelection();
                        dataGridViewPositions.Rows[0].Selected = true;
                        dataGridViewPositions.CurrentCell = dataGridViewPositions[0, 0];
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

                    Update(Position);

                    GetAllpositions();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Position);

                GetAllpositions();
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
            if (string.IsNullOrWhiteSpace(textBoxNominative.Text))
            {
                errorProvider.SetError(textBoxNominative, "Не задано наименование в именительном падеже");
                ok = false;
            }
            else if (string.IsNullOrWhiteSpace(textBoxGenitive.Text))
            {
                errorProvider.SetError(textBoxGenitive, "Не задано наименование в родительном падеже");
                ok = false;
            }
            else if (string.IsNullOrWhiteSpace(textBoxDative.Text))
            {
                errorProvider.SetError(textBoxDative, "Не задано наименование в дательном падеже");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewPositions.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewPositions.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Positions.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentPosition = null;
            }
        }

        public List<Position> Filters()
        {
            var positions = Positions;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text)/* || (string)comboBoxCategoriesSearch.SelectedItem != "Категория"*/)
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        positions = positions.Where(o => o.PositionId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Наименование":
                        {
                            positions = positions.Where(o => o.Nominative.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    //case "Категория":
                    //    {
                    //        if ((string)comboBoxCategoriesSearch.SelectedItem == "Все")
                    //        {
                    //            break;
                    //        }
                    //        positions = positions.Where(o => o.Category == (string)comboBoxCategoriesSearch.SelectedItem).ToList();
                    //        break;
                    //    }
                }
            }

            return positions;
        }

        #endregion //Private Methods
    }
}
