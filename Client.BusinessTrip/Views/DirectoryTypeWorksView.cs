using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.BusinessTrip.Helpers;
using Client.BusinessTrip.Helpers.Sortable;
using System.IO;
using NLog;
using Client.BusinessTrip.Properties;

namespace Client.BusinessTrip.Views
{
    /// <summary>
    /// Класс представления "Справочник видов работ".
    /// </summary>
    public partial class DirectoryTypeWorksView : Form, IDirectoryTypeWorksView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник адресов";

        private List<string> _listSearch = new List<string> { "Наименование", /*"Ид",*/ "Шаблон (приказ)", "Шаблон (служебное задание)", "Шаблон (заявка на транспорт)" };

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IDirectoryTypeWorksPresenter _presenter;

        private IBaseModel<TypeWork> _model;

        private List<TypeWork> _typeWorks;

        private TypeWork _typeWork;

        private TypeWork _currentTypeWork;

        //режим редактирования.
        private bool _isEdit = false;

        //стандартный режим / режим выбора объекта.
        //private bool _isStandard = true;

        //private int _savePosition = 0;
        private int? _saveIndex = 0;

        private int _col = -1;
        private int _row = -1;

        private bool _oneShow = true;

        private DialogResult _resultDialog = DialogResult.No;

        #endregion //Private Fields

        #region Constructors

        public DirectoryTypeWorksView()
        {
            InitializeComponent();

            //openFileDialog.Filter = "Все файлы (*.pdf)|*.pdf";
            openFileDialog.RestoreDirectory = true;
        }

        /// <summary>
        /// Создание представления "Справочник видов работ".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryTypeWorksView(IDirectoryTypeWorksPresenter presenter, IBaseModel<TypeWork> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewTypeWorks)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewTypeWorks, Settings.Default.WindowHeightViewTypeWorks);
            }

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            #region dataGridViewPersons settings

            //dataGridViewTypeWorks.Columns.Add("TypeWorkIdColumn", "Ид");
            //dataGridViewTypeWorks.Columns["TypeWorkIdColumn"].DataPropertyName = "TypeWorkId";
            //dataGridViewTypeWorks.Columns["TypeWorkIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewTypeWorks.Columns["TypeWorkIdColumn"].Width = 50;
            //dataGridViewTypeWorks.Columns["TypeWorkIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            //dataGridViewTypeWorks.Columns["TypeWorkIdColumn"].ToolTipText = "Ид";

            dataGridViewTypeWorks.Columns.Add("NameColumn", "Наименование");
            dataGridViewTypeWorks.Columns["NameColumn"].DataPropertyName = "Name";
            dataGridViewTypeWorks.Columns["NameColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewTypeWorks.Columns["NameColumn"].FillWeight = 125;
            dataGridViewTypeWorks.Columns["NameColumn"].ToolTipText = "Наименование";

            dataGridViewTypeWorks.Columns.Add("TemplateDecreeColumn", "Шаблон (приказ)");
            dataGridViewTypeWorks.Columns["TemplateDecreeColumn"].DataPropertyName = "TemplateDecree";
            dataGridViewTypeWorks.Columns["TemplateDecreeColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewTypeWorks.Columns["TemplateDecreeColumn"].FillWeight = 75;
            dataGridViewTypeWorks.Columns["TemplateDecreeColumn"].ToolTipText = "Шаблон (приказ)";

            dataGridViewTypeWorks.Columns.Add("TemplateTaskColumn", "Шаблон (служ. задание)");
            dataGridViewTypeWorks.Columns["TemplateTaskColumn"].DataPropertyName = "TemplateTask";
            dataGridViewTypeWorks.Columns["TemplateTaskColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewTypeWorks.Columns["TemplateTaskColumn"].FillWeight = 75;
            dataGridViewTypeWorks.Columns["TemplateTaskColumn"].ToolTipText = "Шаблон (служебное задание)";

            dataGridViewTypeWorks.Columns.Add("TemplateRequestTransportColumn", "Шаблон (заявка на транспорт)");
            dataGridViewTypeWorks.Columns["TemplateRequestTransportColumn"].DataPropertyName = "TemplateRequestTransport";
            dataGridViewTypeWorks.Columns["TemplateRequestTransportColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewTypeWorks.Columns["TemplateRequestTransportColumn"].FillWeight = 75;
            dataGridViewTypeWorks.Columns["TemplateRequestTransportColumn"].ToolTipText = "Шаблон (заявка на транспорт)";

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewTypeWorks.ContextMenuStrip = contextMenu;

            dataGridViewTypeWorks.DoubleBuffered(true);

            #endregion //dataGridViewPersons settings

            this.DoubleBuffered(true);
            
            RefreshListTemplete();

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (TypeWorks != null)
            {
                if (TypeWorks.Count > 0)
                {
                    CurrentTypeWork = TypeWorks[0];
                }
            }

            //_isStandard = isStandard;

            //if (!_isStandard)
            //{
            //    btnAdd.Enabled = false;
            //    btnAddCopy.Enabled = false;
            //    btnDel.Enabled = false;
            //    btnNext.Enabled = false;
            //    btnPrev.Enabled = false;
            //    btnSave.Enabled = false;
            //}
        }

        #endregion //Constructors

        #region IBaseView, IDirectoryTypeWorksView

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
        /// Список видов работ.
        /// </summary>
        public List<TypeWork> TypeWorks
        {
            get
            {
                return _typeWorks;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewTypeWorks.SelectionChanged -= dataGridViewTypeWorks_SelectionChanged;

                    dataGridViewTypeWorks.AutoGenerateColumns = false;
                    SortableBindingList<TypeWork> typeWorkBindingList = new SortableBindingList<TypeWork>(value);
                    dataGridViewTypeWorks.DataSource = typeWorkBindingList;

                    if (dataGridViewTypeWorks.ColumnCount > Settings.Default.SortIndexTypeWorks)
                    {
                        dataGridViewTypeWorks.Sort(dataGridViewTypeWorks.Columns[Settings.Default.SortIndexTransports],
                        Settings.Default.SortOrderTransports ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (typeWorkBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _typeWorks = value;

                    dataGridViewTypeWorks.SelectionChanged += dataGridViewTypeWorks_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Новый вид работ.
        /// </summary>
        public TypeWork TypeWork
        {
            get
            {
                if (_isEdit)
                {
                    _typeWork = new TypeWork
                    {
                        TypeWorkId = CurrentTypeWork.TypeWorkId,
                        Name = textBoxName.Text,
                        TemplateDecree = comboBoxTempleteDecree.Items.Count > 0 ? comboBoxTempleteDecree.SelectedItem.ToString() : "",
                        TemplateTask = comboBoxTempleteTask.Items.Count > 0 ? comboBoxTempleteTask.SelectedItem.ToString() : "",
                        TemplateRequestTransport = comboBoxTempleteRequestTransport.Items.Count > 0 ? comboBoxTempleteRequestTransport.SelectedItem.ToString() : "",
                        //TemplateDecree = textBoxTemplates.Text
                    };
                }
                else
                {
                    _typeWork = new TypeWork
                    {
                        Name = textBoxName.Text,
                        TemplateDecree = comboBoxTempleteDecree.Items.Count > 0 ? comboBoxTempleteDecree.SelectedItem.ToString() : "",
                        TemplateTask = comboBoxTempleteTask.Items.Count > 0 ? comboBoxTempleteTask.SelectedItem.ToString() : "",
                        TemplateRequestTransport = comboBoxTempleteRequestTransport.Items.Count > 0 ? comboBoxTempleteRequestTransport.SelectedItem.ToString() : "",
                        //TemplateDecree = textBoxTemplates.Text
                    };
                }
                return _typeWork;
            }
            set
            {
                _typeWork = value;
            }
        }

        /// <summary>
        /// Выбранный вид работ.
        /// </summary>
        public TypeWork CurrentTypeWork
        {
            get
            {
                return _currentTypeWork;
            }
            set
            {
                _currentTypeWork = value;
            }
        }

        /// <summary>
        /// Событие запускает сохранение вида работ.
        /// </summary>
        public event Action<TypeWork> Save;

        /// <summary>
        /// Событие запускает обновление вида работ.
        /// </summary>
        public event Action<TypeWork> Update;

        /// <summary>
        /// Событие запускает удаление вида работ.
        /// </summary>
        public event Action<List<TypeWork>> Delete;

        /// <summary>
        /// Событие запускает получение видов работ.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryPersonsView

        #region EventHandlers

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentTypeWork = null;
            ClearForm();
            SetButtonEnabled(false);
            //_savePosition = 0;
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTypeWorks.SelectedRows[countRow - 1];
                var typeWork = row.DataBoundItem as TypeWork;
                if (typeWork != null)
                {
                    TabControl.SelectedTab = TabCard;
                    SetButtonEnabled(false);
                    //_savePosition = 0;
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeleteTypeWorks();
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
            var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewTypeWorks.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewTypeWorks.ClearSelection();
                    dataGridViewTypeWorks.Rows[indexPrev].Selected = true;
                    dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewTypeWorks.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewTypeWorks.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewTypeWorks.ClearSelection();
                    dataGridViewTypeWorks.Rows[indexNext].Selected = true;
                    dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, indexNext];
                }
            }
        }

        private void btnOK1_ButtonClick(object sender, EventArgs e)
        {
            //if (!_isStandard)
            //{
            //    this.DialogResult = DialogResult.OK;
            //    Close();
            //}
            //else
            //{
            if (IsCheckSave())
            {
                _resultDialog = DialogResult.OK;

                Close();
            }
            //}
        }

        private void btnCancel1_ButtonClick(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();

            _resultDialog = DialogResult.No;

            Close();
        }

        private void dataGridViewTypeWorks_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTypeWorks.SelectedRows[countRow - 1];
                var typeWork = row.DataBoundItem as TypeWork;
                if (typeWork != null)
                {
                    SetValues(typeWork);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxTemplates_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void comboBoxTempleteDecree_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void comboBoxTempleteTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void comboBoxTempleteRequestTransport_SelectedIndexChanged(object sender, EventArgs e)
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
            if (_row < dataGridViewTypeWorks.RowCount && _col < dataGridViewTypeWorks.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewTypeWorks.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewTypeWorks_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewTypeWorks_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllTypeWorks();
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
            GetAllTypeWorks();
            SelectObject();
            ResetLebelCount();
        }

        //private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Escape && btnSave.Enabled == false && TabControl.SelectedTab == tabList)
        //    {
        //        if (IsCheckSave())
        //        {
        //            _resultDialog = DialogResult.No;

        //            Close();
        //        }
        //    }
        //}


        private void dataGridViewTypeWorks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeleteTypeWorks();
                }
            }
        }

        private void DirectoryTypeWorksView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnSave.Enabled == false && TabControl.SelectedTab == tabList)
            {
                if (IsCheckSave())
                {
                    _resultDialog = DialogResult.OK;

                    Close();
                }
            }
            if (e.KeyCode == Keys.Escape && btnSave.Enabled == false /*&& TabControl.SelectedTab == tabList*/)
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
            //RefreshListTemplete();

            GetAllTypeWorks();

            dataGridViewTypeWorks.ClearSelection();

            if (dataGridViewTypeWorks.RowCount > 0)
            {
                dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, 0];
                dataGridViewTypeWorks.Rows[0].Selected = true;
            }

            //RefreshListTemplete();

            //var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            //if (countRow > 0)
            //{
            //    var row = dataGridViewTypeWorks.SelectedRows[countRow - 1];
            //    var typeWork = row.DataBoundItem as TypeWork;
            //    if (typeWork != null)
            //    {
            //        SetValues(typeWork);
            //        //_isEdit = true;
            //    }
            //}

            textBoxSearch.Focus();
        }

        private void DirectoryTypeWorksView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void DirectoryTypeWorksView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryTypeWorksView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewTypeWorks_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewTypeWorks.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewTypeWorks.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewTypeWorks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexTypeWorks = e.ColumnIndex;

                if (dataGridViewTypeWorks.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderTypeWorks = true;
                }
                else if (dataGridViewTypeWorks.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderTypeWorks = false;
                }
                else
                {
                    Settings.Default.SortOrderTypeWorks = true;
                }
            }
            else
            {
                Settings.Default.SortIndexTypeWorks = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryTypeWorksView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewTypeWorks = true;
            else
                Settings.Default.WindowStateMaximizedViewTypeWorks = false;

            Settings.Default.WindowWidthViewTypeWorks = this.Size.Width;
            Settings.Default.WindowHeightViewTypeWorks = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteTypeWorks()
        {
            SavePosition(true,true);

            var typeWorks = new List<TypeWork>();

            foreach (DataGridViewRow row in dataGridViewTypeWorks.SelectedRows)
            {
                var typeWork = row.DataBoundItem as TypeWork;
                if (typeWork != null)
                {
                    typeWorks.Add(new TypeWork { TypeWorkId = typeWork.TypeWorkId });
                }
            }

            if (!typeWorks.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить вид работ (кол-во: {0})?", typeWorks.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(typeWorks);

                GetAllTypeWorks();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewTypeWorks.ClearSelection();

            if (dataGridViewTypeWorks.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewTypeWorks.Rows)
                    {
                        var typeWork = row.DataBoundItem as TypeWork;
                        if (typeWork != null)
                        {
                            if (typeWork.TypeWorkId == SaveIndex)
                            {
                                dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, row.Index];
                                dataGridViewTypeWorks.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, 0];
                dataGridViewTypeWorks.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewTypeWorks.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewTypeWorks.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewTypeWorks.SelectedRows.Count < dataGridViewTypeWorks.RowCount)
                    {
                        var selectedRow = dataGridViewTypeWorks.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewTypeWorks.SelectedRows[0].Index == dataGridViewTypeWorks.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewTypeWorks.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewTypeWorks.RowCount)
                            {
                                row = dataGridViewTypeWorks.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewTypeWorks.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewTypeWorks.RowCount)
                            {
                                row = dataGridViewTypeWorks.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var typeWork = row.DataBoundItem as TypeWork;
                            if (typeWork != null)
                            {
                                SaveIndex = typeWork.TypeWorkId;
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
                    var row = dataGridViewTypeWorks.SelectedRows[selectIndex];
                    var typeWork = row.DataBoundItem as TypeWork;
                    if (typeWork != null)
                    {
                        SaveIndex = typeWork.TypeWorkId;
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
            if (CurrentTypeWork != null /*&& _isStandard*/)
            {
                if (textBoxName.Text != CurrentTypeWork.Name || ((string)comboBoxTempleteDecree.SelectedItem != CurrentTypeWork.TemplateDecree && (string)comboBoxTempleteDecree.SelectedItem != null) ||
                    ((string)comboBoxTempleteTask.SelectedItem != CurrentTypeWork.TemplateTask && (string)comboBoxTempleteTask.SelectedItem != null) || ((string)comboBoxTempleteRequestTransport.SelectedItem != CurrentTypeWork.TemplateRequestTransport && (string)comboBoxTempleteRequestTransport.SelectedItem != null))
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
                btnSave.Enabled = true;
                btnRefresh.Enabled = false;

                btnClear.Enabled = false;
                comboBoxSearch.Enabled = false;
                textBoxSearch.Enabled = false;
            }
        }

        /// <summary>
        /// Установка полей сотрудника.
        /// </summary>
        /// <param name="typeWork">Вид работ.</param>
        private void SetValues(TypeWork typeWork)
        {
            CurrentTypeWork = typeWork;
            if (typeWork != null)
            {
                textBoxName.Text = typeWork.Name;
                if (comboBoxTempleteDecree.Items.Count > 0)
                {
                    comboBoxTempleteDecree.SelectedItem = typeWork.TemplateDecree;
                }
                if (comboBoxTempleteTask.Items.Count > 0)
                {
                    comboBoxTempleteTask.SelectedItem = typeWork.TemplateTask;
                }
                if (comboBoxTempleteRequestTransport.Items.Count > 0)
                {
                    comboBoxTempleteRequestTransport.SelectedItem = typeWork.TemplateRequestTransport;
                }
            }
            //textBoxTemplates.Text = typeWork.TemplateDecree;
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxName.Clear();
            if (comboBoxTempleteDecree.Items.Count > 0)
            {
                comboBoxTempleteDecree.SelectedIndex = 0;
            }
            if (comboBoxTempleteTask.Items.Count > 0)
            {
                comboBoxTempleteTask.SelectedIndex = 0;
            }
            if (comboBoxTempleteRequestTransport.Items.Count > 0)
            {
                comboBoxTempleteRequestTransport.SelectedIndex = 0;
            }
            //_savePosition = 0;
            //textBoxTemplates.Clear();

            textBoxName.Focus();
        }

        /// <summary>
        /// Получение всех видов работ.
        /// </summary>
        private void GetAllTypeWorks()
        {
            if (GetAll != null)
                GetAll();

            TypeWorks = Filters();

            if (TypeWorks.Any())
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
                    GetAllTypeWorks();

                    SetButtonEnabled(true);
                    if (dataGridViewTypeWorks.RowCount > 0)
                    {
                        dataGridViewTypeWorks.ClearSelection();
                        dataGridViewTypeWorks.Rows[0].Selected = true;
                        dataGridViewTypeWorks.CurrentCell = dataGridViewTypeWorks[0, 0];
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

                    Update(TypeWork);

                    GetAllTypeWorks();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(TypeWork);

                GetAllTypeWorks();

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
                errorProvider.SetError(textBoxName, "Не задано наименование");
                ok = false;
            }
            return ok;
        }

        private void RefreshListTemplete()
        {
            try
            {
                List<string> filesNames = new DirectoryInfo(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Doc\TempleteDecree").EnumerateFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && f.Extension == ".dotx").Select(f => f.Name).ToList();
                comboBoxTempleteDecree.DataSource = filesNames;
                if (comboBoxTempleteDecree.Items.Count > 0)
                {
                    comboBoxTempleteDecree.SelectedIndex = 0;
                }

                filesNames.Clear();
                filesNames = new DirectoryInfo(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Excel\TemplateTask").EnumerateFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && f.Extension == ".xltx").Select(f => f.Name).ToList();
                comboBoxTempleteTask.DataSource = filesNames;
                if (comboBoxTempleteTask.Items.Count > 0)
                {
                    comboBoxTempleteTask.SelectedIndex = 0;
                }

                filesNames.Clear();
                filesNames = new DirectoryInfo(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Excel\TemplateRequestTransport\").EnumerateFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && f.Extension == ".xltx").Select(f => f.Name).ToList();
                comboBoxTempleteRequestTransport.DataSource = filesNames;
                if (comboBoxTempleteRequestTransport.Items.Count > 0)
                {
                    comboBoxTempleteRequestTransport.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при загрузке шаблонов.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewTypeWorks.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTypeWorks.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, TypeWorks.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentTypeWork = null;
            }
        }

        public List<TypeWork> Filters()
        {
            var typeWorks = TypeWorks;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        typeWorks = typeWorks.Where(o => o.TypeWorkId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Наименование":
                        {
                            typeWorks = typeWorks.Where(o => o.Name.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Шаблон (приказ)":
                        {
                            typeWorks = typeWorks.Where(o => o.TemplateDecree.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Шаблон (служебное задание)":
                        {
                            typeWorks = typeWorks.Where(o => o.TemplateTask.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Шаблон (заявка на транспорт)":
                        {
                            typeWorks = typeWorks.Where(o => o.TemplateRequestTransport.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }
            //dataGridViewTypeWorks.SelectionChanged -= dataGridViewTypeWorks_SelectionChanged;

            //dataGridViewTypeWorks.AutoGenerateColumns = false;
            //SortableBindingList<TypeWork> typeWorkBindingList = new SortableBindingList<TypeWork>(typeWorks);
            //dataGridViewTypeWorks.DataSource = typeWorkBindingList;

            //if (typeWorkBindingList.Count == 0)
            //{
            //    ClearForm();
            //    SetButtonEnabled(true);
            //}

            //dataGridViewTypeWorks.SelectionChanged += dataGridViewTypeWorks_SelectionChanged;

            return typeWorks;
        }


        #endregion //Private Methods
    }
}
