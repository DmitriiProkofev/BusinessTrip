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
    public partial class DirectoryTransportsView : Form, IDirectoryTransportsView
    {
        #region Private Fields 

        private const string DirectoryName = "Справочник транспортных средств";

        private List<string> _listSearch = new List<string> {"Марка",/* "Ид" */};

        private IDirectoryTransportsPresenter _presenter;

        private IBaseModel<Transport> _model;

        private List<Transport> _transports;

        private Transport _transport;

        private Transport _currentTransport;

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

        public DirectoryTransportsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник отделов".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryTransportsView(IDirectoryTransportsPresenter presenter, IBaseModel<Transport> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewTransports)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewTransports, Settings.Default.WindowHeightViewTransports);
            }

            #region dataGridViewTransports settings

            //dataGridViewTransports.Columns.Add("TransportIdColumn", "Ид");
            //dataGridViewTransports.Columns["TransportIdColumn"].DataPropertyName = "TransportId";
            //dataGridViewTransports.Columns["TransportIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewTransports.Columns["TransportIdColumn"].Width = 50;
            //dataGridViewTransports.Columns["TransportIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridViewTransports.Columns.Add("MarkColumn", "Марка");
            dataGridViewTransports.Columns["MarkColumn"].DataPropertyName = "Mark";
            dataGridViewTransports.Columns["MarkColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewTransports.Columns["MarkColumn"].FillWeight = 125;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewTransports.ContextMenuStrip = contextMenu;

            dataGridViewTransports.DoubleBuffered(true);

            #endregion //dataGridViewTransports settings

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Transports != null)
            {
                if (Transports.Count > 0)
                {
                    CurrentTransport = Transports[0];
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

        #region IBaseView, IDirectoryTransportsView

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

        public Transport CurrentTransport
        {
            get
            {
                return _currentTransport;
            }

            set
            {
                _currentTransport = value;
            }
        }

        public List<Transport> Transports
        {
            get
            {
                return _transports;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewTransports.SelectionChanged -= dataGridViewTransports_SelectionChanged;

                    dataGridViewTransports.AutoGenerateColumns = false;
                    SortableBindingList<Transport> transportsBindingList = new SortableBindingList<Transport>(value);
                    dataGridViewTransports.DataSource = transportsBindingList;

                    if (dataGridViewTransports.ColumnCount > Settings.Default.SortIndexTransports)
                    {
                        dataGridViewTransports.Sort(dataGridViewTransports.Columns[Settings.Default.SortIndexTransports],
                        Settings.Default.SortOrderTransports ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (transportsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _transports = value;

                    dataGridViewTransports.SelectionChanged += dataGridViewTransports_SelectionChanged;
                }
            }
        }

        public Transport Transport
        {
            get
            {
                if (_isEdit)
                {
                    _transport = new Transport
                    {
                        TransportId = CurrentTransport.TransportId,
                        Mark = textBoxMark.Text
                    };
                }
                else
                {
                    _transport = new Transport
                    {
                        Mark = textBoxMark.Text
                    };
                }
                return _transport;
            }
            set
            {
                _transport = value;
            }
        }

        public event Action<List<Transport>> Delete;
        public event Action GetAll;
        public event Action<Transport> Save;
        public event Action ViewClosed;
        public event Action<Transport> Update;

        #endregion //IBaseView, IDirectoryTransportsView

        #region EventHandlers

        private void DirectoryTransportsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryTransportsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentTransport = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewTransports.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTransports.SelectedRows[countRow - 1];
                var transport = row.DataBoundItem as Transport;
                if (transport != null)
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
            DeleteTransports();
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
            var countRow = dataGridViewTransports.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewTransports.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewTransports.ClearSelection();
                    dataGridViewTransports.Rows[indexPrev].Selected = true;
                    dataGridViewTransports.CurrentCell = dataGridViewTransports[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewTransports.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewTransports.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewTransports.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewTransports.ClearSelection();
                    dataGridViewTransports.Rows[indexNext].Selected = true;
                    dataGridViewTransports.CurrentCell = dataGridViewTransports[0, indexNext];
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

        private void dataGridViewTransports_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewTransports.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTransports.SelectedRows[countRow - 1];
                var transport = row.DataBoundItem as Transport;
                if (transport != null)
                {
                    SetValues(transport);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxMark_TextChanged(object sender, EventArgs e)
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
            if (_row < dataGridViewTransports.RowCount && _col < dataGridViewTransports.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewTransports.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewTransports_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewTransports_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAlltransports();
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
            GetAlltransports();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewTransports_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeleteTransports();
                }
            }
        }

        private void DirectoryTransportsView_KeyUp(object sender, KeyEventArgs e)
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
            GetAlltransports();

            dataGridViewTransports.ClearSelection();

            if (dataGridViewTransports.RowCount > 0)
            {
                dataGridViewTransports.CurrentCell = dataGridViewTransports[0, 0];
                dataGridViewTransports.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryTransportsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewTransports_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewTransports.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewTransports.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewTransports_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexTransports = e.ColumnIndex;

                if (dataGridViewTransports.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderTransports = true;
                }
                else if (dataGridViewTransports.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderTransports = false;
                }
                else
                {
                    Settings.Default.SortOrderTransports = true;
                }
            }
            else
            {
                Settings.Default.SortIndexTransports = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryTransportsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewTransports = true;
            else
                Settings.Default.WindowStateMaximizedViewTransports = false;

            Settings.Default.WindowWidthViewTransports= this.Size.Width;
            Settings.Default.WindowHeightViewTransports = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteTransports()
        {
            SavePosition(true,true);

            var transports = new List<Transport>();

            foreach (DataGridViewRow row in dataGridViewTransports.SelectedRows)
            {
                var transport = row.DataBoundItem as Transport;
                if (transport != null)
                {
                    transports.Add(new Transport { TransportId = transport.TransportId });
                }
            }

            if (!transports.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить транспортные средства (кол-во: {0})?", transports.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(transports);

                GetAlltransports();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewTransports.ClearSelection();

            if (dataGridViewTransports.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewTransports.Rows)
                    {
                        var transport = row.DataBoundItem as Transport;
                        if (transport != null)
                        {
                            if (transport.TransportId == SaveIndex)
                            {
                                dataGridViewTransports.CurrentCell = dataGridViewTransports[0, row.Index];
                                dataGridViewTransports.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }
                }
                dataGridViewTransports.CurrentCell = dataGridViewTransports[0, 0];
                dataGridViewTransports.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewTransports.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewTransports.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewTransports.SelectedRows.Count < dataGridViewTransports.RowCount)
                    {
                        var selectedRow = dataGridViewTransports.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewTransports.SelectedRows[0].Index == dataGridViewTransports.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewTransports.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewTransports.RowCount)
                            {
                                row = dataGridViewTransports.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewTransports.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewTransports.RowCount)
                            {
                                row = dataGridViewTransports.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var transport = row.DataBoundItem as Transport;
                            if (transport != null)
                            {
                                SaveIndex = transport.TransportId;
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
                    var row = dataGridViewTransports.SelectedRows[selectIndex];
                    var transport = row.DataBoundItem as Transport;
                    if (transport != null)
                    {
                        SaveIndex = transport.TransportId;
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
            if (CurrentTransport != null /*&& _isStandard*/)
            {
                if (textBoxMark.Text != CurrentTransport.Mark)
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
        /// Установка полей транспорта.
        /// </summary>
        /// <param name="transport">Транспорт.</param>
        private void SetValues(Transport transport)
        {
            CurrentTransport = transport;

            if (transport != null)
            {
                textBoxMark.Text = transport.Mark;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxMark.Clear();
            //_savePosition = 0;

            textBoxMark.Focus();
        }

        /// <summary>
        /// Получение всех транспортов.
        /// </summary>
        private void GetAlltransports()
        {
            if (GetAll != null)
                GetAll();

            Transports = Filters();

            if (Transports.Any())
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
                    GetAlltransports();

                    SetButtonEnabled(true);
                    if (dataGridViewTransports.RowCount > 0)
                    {
                        dataGridViewTransports.ClearSelection();
                        dataGridViewTransports.Rows[0].Selected = true;
                        dataGridViewTransports.CurrentCell = dataGridViewTransports[0, 0];
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

                    Update(Transport);

                    GetAlltransports();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Transport);

                GetAlltransports();
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
            if (string.IsNullOrWhiteSpace(textBoxMark.Text))
            {
                errorProvider.SetError(textBoxMark, "Не задана марка транспортного средства");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewTransports.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewTransports.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Transports.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentTransport = null;
            }
        }

        public List<Transport> Filters()
        {
            var transports = Transports;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        transports = transports.Where(o => o.TransportId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Марка":
                        {
                            transports = transports.Where(o => o.Mark.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }

            return transports;
        }

        #endregion //Private Methods

    }
}
