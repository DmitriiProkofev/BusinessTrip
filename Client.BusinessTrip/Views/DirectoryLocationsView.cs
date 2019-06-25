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
    /// Класс представления "Справочник адресов".
    /// </summary>
    public partial class DirectoryLocationsView : Form, IDirectoryLocationsView
    {
        #region Private Fields 

        private const string DirectoryName = "Оформление командировок";

        private List<string> _listSearch = new List<string> { "Адрес", /*"Ид",*/ "Полный адрес" };

        private IDirectoryLocationsPresenter _presenter;

        private IBaseModel<Location> _model;

        private List<Location> _locations;

        private Location _location;

        private Location _currentLocation;

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

        public DirectoryLocationsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание представления "Справочник адресов".
        /// </summary>
        /// <param name="presenter">Представитель.</param>
        /// <param name="model">Модель.</param>
        /// <param name="isStandard">Режим отображения представления.</param>
        public DirectoryLocationsView(IDirectoryLocationsPresenter presenter, IBaseModel<Location> model, bool isStandard = true)
            : this()
        {
            _presenter = presenter;
            _model = model;
            _presenter.Init(this, _model);

            if (Settings.Default.WindowStateMaximizedViewLocations)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(Settings.Default.WindowWidthViewLocations, Settings.Default.WindowHeightViewLocations);
            }

            #region dataGridViewPersons settings

            //dataGridViewLocations.Columns.Add("LocationIdColumn", "Ид");
            //dataGridViewLocations.Columns["LocationIdColumn"].DataPropertyName = "LocationId";
            //dataGridViewLocations.Columns["LocationIdColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dataGridViewLocations.Columns["LocationIdColumn"].Width = 50;
            //dataGridViewLocations.Columns["LocationIdColumn"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridViewLocations.Columns.Add("ShortAddressColumn", "Адрес");
            dataGridViewLocations.Columns["ShortAddressColumn"].DataPropertyName = "ShortAddress";
            dataGridViewLocations.Columns["ShortAddressColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewLocations.Columns["ShortAddressColumn"].FillWeight = 75;

            dataGridViewLocations.Columns.Add("AddressColumn", "Полный адрес");
            dataGridViewLocations.Columns["AddressColumn"].DataPropertyName = "Address";
            dataGridViewLocations.Columns["AddressColumn"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewLocations.Columns["AddressColumn"].FillWeight = 125;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Копировать");
            item.Click += new EventHandler(mnuCopy_Click);
            contextMenu.Items.AddRange(new ToolStripItem[] { item });
            dataGridViewLocations.ContextMenuStrip = contextMenu;

            dataGridViewLocations.DoubleBuffered(true);

            #endregion //dataGridViewPersons settings

            btnSave.Enabled = false;
            TabControl.SelectedTab = tabList;

            comboBoxSearch.Items.AddRange(_listSearch.ToArray());
            comboBoxSearch.SelectedIndex = 0;

            if (Locations != null)
            {
                if (Locations.Count > 0)
                {
                    CurrentLocation = Locations[0];
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

        #region IBaseView, IDirectoryLocationsView

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
        /// Список адресов.
        /// </summary>
        public List<Location> Locations
        {
            get
            {
                return _locations;
            }

            set
            {
                if (value != null)
                {
                    dataGridViewLocations.SelectionChanged -= dataGridViewLocations_SelectionChanged;

                    dataGridViewLocations.AutoGenerateColumns = false;
                    SortableBindingList<Location> locationsBindingList = new SortableBindingList<Location>(value);
                    dataGridViewLocations.DataSource = locationsBindingList;

                    if (dataGridViewLocations.ColumnCount > Settings.Default.SortIndexLocations)
                    {
                        dataGridViewLocations.Sort(dataGridViewLocations.Columns[Settings.Default.SortIndexLocations],
                        Settings.Default.SortOrderLocations ? ListSortDirection.Ascending : ListSortDirection.Descending);
                    }

                    if (locationsBindingList.Count == 0)
                    {
                        ClearForm();
                        SetButtonEnabled(true);
                    }

                    _locations = value;

                    dataGridViewLocations.SelectionChanged += dataGridViewLocations_SelectionChanged;
                }
            }
        }

        /// <summary>
        /// Адрес.
        /// </summary>
        public Location Location
        {
            get
            {
                if (_isEdit)
                {
                    _location = new Location
                    {
                        LocationId = CurrentLocation.LocationId,
                        ShortAddress = textBoxShortAddress.Text,
                        Address = textBoxAddress.Text
                    };
                }
                else
                {
                    _location = new Location
                    {
                        ShortAddress = textBoxShortAddress.Text,
                        Address = textBoxAddress.Text
                    };
                }
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        /// <summary>
        /// Выбранный адрес.
        /// </summary>
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
            }
        }

        /// <summary>
        /// Событие запускает сохранение адреса.
        /// </summary>
        public event Action<Location> Save;

        /// <summary>
        /// Событие запускает обновление адреса.
        /// </summary>
        public event Action<Location> Update;

        /// <summary>
        /// Событие запускает удаление адреса.
        /// </summary>
        public event Action<List<Location>> Delete;

        /// <summary>
        /// Событие запускает получение адреса.
        /// </summary>
        public event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        public event Action ViewClosed;

        #endregion //IBaseView, IDirectoryLocationsView

        #region EventHandlers

        private void DirectoryLocationsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCheckSave())
            {
                e.Cancel = true;
            }
        }

        private void DirectoryLocationsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = _resultDialog;

            if (ViewClosed != null)
                ViewClosed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
            CurrentLocation = null;
            ClearForm();
            SetButtonEnabled(false);
            SaveIndex = null;
            _isEdit = false;
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            var countRow = dataGridViewLocations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewLocations.SelectedRows[countRow - 1];
                var location = row.DataBoundItem as Location;
                if (location != null)
                {
                    TabControl.SelectedTab = TabCard;
                    SetButtonEnabled(false);
                    SaveIndex = null;
                    _isEdit = false;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeleteLocations();
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
            var countRow = dataGridViewLocations.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewLocations.SelectedRows[0].Index;

                if (indexSelect - 1 >= 0)
                {
                    indexPrev = indexSelect - 1;
                    dataGridViewLocations.ClearSelection();
                    dataGridViewLocations.Rows[indexPrev].Selected = true;
                    dataGridViewLocations.CurrentCell = dataGridViewLocations[0, indexPrev];
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int indexSelect, indexNext;
            var countRow = dataGridViewLocations.SelectedRows.Count;

            if (countRow > 0)
            {
                indexSelect = dataGridViewLocations.SelectedRows[0].Index;

                if (indexSelect + 1 <= dataGridViewLocations.Rows.Count - 1)
                {
                    indexNext = indexSelect + 1;
                    dataGridViewLocations.ClearSelection();
                    dataGridViewLocations.Rows[indexNext].Selected = true;
                    dataGridViewLocations.CurrentCell = dataGridViewLocations[0, indexNext];
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

        private void dataGridViewLocations_SelectionChanged(object sender, EventArgs e)
        {
            var countRow = dataGridViewLocations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewLocations.SelectedRows[countRow - 1];
                var location = row.DataBoundItem as Location;
                if (location != null)
                {
                    SetValues(location);
                    _isEdit = true;
                }
            }
            ResetLebelCount();
        }

        private void textBoxShortAddress_TextChanged(object sender, EventArgs e)
        {
            SetStateForChange();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
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
            if (_row < dataGridViewLocations.RowCount && _col < dataGridViewLocations.ColumnCount && _row >= 0 && _col >= 0)
            {
                Clipboard.SetData(DataFormats.Text, dataGridViewLocations.Rows[_row].Cells[_col].Value.ToString());
            }
        }

        private void dataGridViewLocations_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _row = e.RowIndex;
                _col = e.ColumnIndex;
            }
        }

        private void dataGridViewLocations_DoubleClick(object sender, EventArgs e)
        {
            TabControl.SelectedTab = TabCard;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SavePosition();
            GetAllLocations();
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
            GetAllLocations();
            SelectObject();
            ResetLebelCount();
        }

        private void dataGridViewLocations_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnSave.Enabled == false && TabControl.SelectedTab == tabList)
                {
                    DeleteLocations();
                }
            }
        }

        private void DirectoryLocationsView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnSave.Enabled == false && TabControl.SelectedTab == tabList)
            {
                if (IsCheckSave())
                {
                    _resultDialog = DialogResult.OK;

                    Close();
                }
            }

            else if (e.KeyCode == Keys.Escape && btnSave.Enabled == false/* && TabControl.SelectedTab == tabList*/)
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
            GetAllLocations();

            dataGridViewLocations.ClearSelection();

            if (dataGridViewLocations.RowCount > 0)
            {
                dataGridViewLocations.CurrentCell = dataGridViewLocations[0, 0];
                dataGridViewLocations.Rows[0].Selected = true;
            }

            textBoxSearch.Focus();
        }

        private void DirectoryLocationsView_Shown(object sender, EventArgs e)
        {
            if (_oneShow)
            {
                textBoxSearch.Focus();
                _oneShow = false;
            }
        }

        private void dataGridViewLocations_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridViewLocations.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridViewLocations.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dataGridViewLocations_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                Settings.Default.SortIndexLocations = e.ColumnIndex;

                if (dataGridViewLocations.SortOrder == SortOrder.Ascending)
                {
                    Settings.Default.SortOrderLocations = true;
                }
                else if (dataGridViewLocations.SortOrder == SortOrder.Descending)
                {
                    Settings.Default.SortOrderLocations = false;
                }
                else
                {
                    Settings.Default.SortOrderLocations = true;
                }
            }
            else
            {
                Settings.Default.SortIndexLocations = 0;
            }
            Settings.Default.Save();
        }

        private void DirectoryLocationsView_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                Settings.Default.WindowStateMaximizedViewLocations = true;
            else
                Settings.Default.WindowStateMaximizedViewLocations = false;

            Settings.Default.WindowWidthViewLocations = this.Size.Width;
            Settings.Default.WindowHeightViewLocations = this.Size.Height;

            Settings.Default.Save();
        }

        #endregion //EventHandlers

        #region Private Methods

        private void DeleteLocations()
        {
            SavePosition(true, true);

            var locations = new List<Location>();

            foreach (DataGridViewRow row in dataGridViewLocations.SelectedRows)
            {
                var location = row.DataBoundItem as Location;
                if (location != null)
                {
                    locations.Add(new Location { LocationId = location.LocationId });
                }
            }

            if (!locations.Any())
                return;

            if (MessageBox.Show(string.Format("Вы действительно хотите удалить адреса (кол-во: {0})?", locations.Count),
                DirectoryName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Delete != null)
                    Delete(locations);

                GetAllLocations();

                SelectObject();
            }
        }

        private void SelectObject(bool isEnd = true)
        {
            dataGridViewLocations.ClearSelection();

            if (dataGridViewLocations.RowCount > 0)
            {
                if (SaveIndex != null)
                {
                    foreach (DataGridViewRow row in dataGridViewLocations.Rows)
                    {
                        var location = row.DataBoundItem as Location;
                        if (location != null)
                        {
                            if (location.LocationId == SaveIndex)
                            {
                                dataGridViewLocations.CurrentCell = dataGridViewLocations[0, row.Index];
                                dataGridViewLocations.Rows[row.Index].Selected = true;

                                return;
                            }
                        }
                    }

                }
                dataGridViewLocations.CurrentCell = dataGridViewLocations[0, 0];
                dataGridViewLocations.Rows[0].Selected = true;
            }
        }

        private void SavePosition(bool isEnd = true, bool isDel = false)
        {
            if (dataGridViewLocations.SelectedRows.Count > 0)
            {
                int selectIndex;
                if (isEnd)
                {
                    selectIndex = dataGridViewLocations.SelectedRows.Count - 1;
                }
                else
                {
                    selectIndex = 0;
                }

                if (isDel)
                {
                    if (dataGridViewLocations.SelectedRows.Count < dataGridViewLocations.RowCount)
                    {
                        var selectedRow = dataGridViewLocations.SelectedRows[selectIndex];
                        DataGridViewRow row = null;
                        if (dataGridViewLocations.SelectedRows[0].Index == dataGridViewLocations.RowCount - 1)
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index - 1;
                            }
                            else
                            {
                                newIndex = selectedRow.Index - dataGridViewLocations.SelectedRows.Count;
                            }
                            if (newIndex < dataGridViewLocations.RowCount)
                            {
                                row = dataGridViewLocations.Rows[newIndex];
                            }
                        }
                        else
                        {
                            int newIndex;
                            if (isEnd)
                            {
                                newIndex = selectedRow.Index + dataGridViewLocations.SelectedRows.Count;
                            }
                            else
                            {
                                newIndex = selectedRow.Index + 1;
                            }
                            if (newIndex < dataGridViewLocations.RowCount)
                            {
                                row = dataGridViewLocations.Rows[newIndex];
                            }
                        }
                        if (row != null)
                        {
                            var location = row.DataBoundItem as Location;
                            if (location != null)
                            {
                                SaveIndex = location.LocationId;
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
                    var row = dataGridViewLocations.SelectedRows[selectIndex];
                    var location = row.DataBoundItem as Location;
                    if (location != null)
                    {
                        SaveIndex = location.LocationId;
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
            if (CurrentLocation != null/* && _isStandard*/)
            {
                if (textBoxAddress.Text != CurrentLocation.Address || textBoxShortAddress.Text != CurrentLocation.ShortAddress)
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
        /// Установка полей адреса.
        /// </summary>
        /// <param name="location">Адрес.</param>
        private void SetValues(Location location)
        {
            CurrentLocation = location;
            if (location != null)
            {
                textBoxShortAddress.Text = location.ShortAddress;
                textBoxAddress.Text = location.Address;
            }
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            textBoxShortAddress.Clear();
            textBoxAddress.Clear();
            //_savePosition = 0;

            textBoxShortAddress.Focus();
        }

        /// <summary>
        /// Получение всех сотрудников.
        /// </summary>
        private void GetAllLocations()
        {
            if (GetAll != null)
                GetAll();

            Locations = Filters();

            if (Locations.Any())
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
                    GetAllLocations();

                    SetButtonEnabled(true);
                    if (dataGridViewLocations.RowCount > 0)
                    {
                        dataGridViewLocations.ClearSelection();
                        dataGridViewLocations.Rows[0].Selected = true;
                        dataGridViewLocations.CurrentCell = dataGridViewLocations[0, 0];
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

                    Update(Location);

                    GetAllLocations();

                    SelectObject();
                }
            }
            else
            {
                if (Save != null)
                    Save(Location);

                GetAllLocations();
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
            if (string.IsNullOrWhiteSpace(textBoxShortAddress.Text))
            {
                errorProvider.SetError(textBoxShortAddress, "Не задан адрес");
                ok = false;
            }
            return ok;
        }

        private void ResetLebelCount()
        {
            var countRow = dataGridViewLocations.SelectedRows.Count;

            if (countRow > 0)
            {
                var row = dataGridViewLocations.SelectedRows[countRow - 1];

                lblCount.Text = string.Format("Запись: {0} из {1}, Выбрано: {2}", row.Index + 1, Locations.Count.ToString(), countRow);
            }
            else
            {
                lblCount.Text = "Запись: 0 из 0";
                ClearForm();
                CurrentLocation = null;
            }
        }

        public List<Location> Filters()
        {
            var locations = Locations;

            if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                switch ((string)comboBoxSearch.SelectedItem)
                {
                    //case "Ид":
                    //    {
                    //        locations = locations.Where(o => o.LocationId.ToString().Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    //        break;
                    //    }
                    case "Адрес":
                        {
                            locations = locations.Where(o => o.ShortAddress.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                    case "Полный адрес":
                        {
                            locations = locations.Where(o => o.Address.Contains(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        }
                }
            }

            return locations;
        }


        #endregion //Private Methods

    }
}
