using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dlv005.UI
{
    public partial class SelectionTable : Form
    {
        /// <summary>
        /// The binding source for grid.
        /// </summary>
        private BindingSource bindingSourceSelectionTable = new BindingSource();

        /// <summary>
        /// The direction of sorting.
        /// </summary>
        private string sortingDirection = "upwards";

        /// <summary>
        /// The DLV005VIEW.
        /// </summary>
        private Dlv005View dlv005DelegateView;

        /// <summary>
        /// The delegate from Dlv005View.
        /// </summary>
        private ITable selectionTable;

        /// <summary>
        /// The key values that will send data to DLV005.
        /// </summary>
        private List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// The Call Back Delegate that sends data to DLV005VIEW.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="keyValues">The key values.</param>
        public delegate void CallBackSelectionTable(ITable table, List<KeyValuePair<string, string>> keyValues);

        /// <summary>
        /// Initializes the values from DLV005
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="view">The view.</param>
        public void InitializeValues(ITable table, List<KeyValuePair<string, string>> keyValues, Dlv005View view)
        {
            selectionTable = table;
            dlv005DelegateView = view;
            this.keyValues = keyValues;
            InitializeUI();
        }

        private void Show(object sender, EventArgs e)
        {
            dataGridViewSelectionTable.Rows[0].Selected = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionTable" /> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public SelectionTable()
        {
            InitializeComponent();
            Shown += Show;
        }

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitializeUI()
        {
            InitializeSelectionTable();
            InitializeComboBoxItemps();
            InitializeEvents();
            UpdateDataGridViewElementsCount();
        }

        /// <summary>
        /// Initializes selection table from name.
        /// </summary>
        private void InitializeSelectionTable()
        {
            selectionTable.InitializeName(this);
            selectionTable.InitializeBindingSource(dataGridViewSelectionTable);
            selectionTable.InitializeMultiSelect(dataGridViewSelectionTable);
            bindingSourceSelectionTable.DataSource = dataGridViewSelectionTable.DataSource;
        }

        /// <summary>
        /// Updates the data grid view elements count.
        /// </summary>
        private void UpdateDataGridViewElementsCount()
        {
            textBoxCountData.Text = dataGridViewSelectionTable.Rows.Count.ToString();
        }

        /// <summary>
        /// Initializes the ComboBox itemps.
        /// </summary>
        private void InitializeComboBoxItemps()
        {
            foreach (DataGridViewColumn column in dataGridViewSelectionTable.Columns)
            {
                if (column.Visible == true)
                {
                    comboBoxColumn.Items.Add(column.Name);
                    comboBoxSorting.Items.Add(column.Name);
                }
            }
        }

        /// <summary>
        /// Initializes UI events.
        /// </summary>
        private void InitializeEvents()

        {
            dataGridViewSelectionTable.MouseClick += SelectFullRowWhenClick;
            dataGridViewSelectionTable.DoubleClick += SelectFullRow;
            comboBoxColumn.SelectionChangeCommitted += SelectionChangedEvent;
            textBoxFilter.TextChanged += FilterFieldTextChangedEvent;
            comboBoxSorting.GotFocus += SortingComboBoxGotFocusEvent;
        }

        /// <summary>
        /// Selects the full row when click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void SelectFullRowWhenClick(object sender, MouseEventArgs e)
        {
            dataGridViewSelectionTable.Rows[GetCurrentDataGridViewRowIndex()].Selected = true;
        }

        /// <summary>
        /// Doddges the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SelectFullRow(object sender, EventArgs e)
        {
            dataGridViewSelectionTable.Rows[GetCurrentDataGridViewRowIndex()].Selected = true;
            btnSave.PerformClick();
        }

        /// <summary>
        /// <para></para>
        /// <para>Handles the TextChanged event of the TextBoxFilter control.
        /// </para>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FilterFieldTextChangedEvent(object sender, EventArgs e)
        {
            if (textBoxFilter.Text == string.Empty)
            {
                bindingSourceSelectionTable.Filter = null;
                btnUpdate.Enabled = false;

            }
            else
            {
                btnSave.Enabled = true;
                btnUpdate.Enabled = true;
            }
            UpdateDataGridViewElementsCount();
        }

        /// <summary>
        /// <para></para>
        /// <para>Handles the SelectionChangeCommitted event of the ComboBoxColumn control.
        /// </para>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void SelectionChangedEvent(object sender, EventArgs e)
        {
            textBoxFilter.Enabled = true;
        }

        /// <summary>
        /// Handles the GotFocus event of the ComboBoxSorting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void SortingComboBoxGotFocusEvent(object sender, EventArgs e)
        {
            btnArrow.Enabled = true;
            btnSort.Enabled = true;
        }

        /// <summary>
        /// Gets the index of the current data grid view row.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentDataGridViewRowIndex()
        {
            foreach (DataGridViewRow row in dataGridViewSelectionTable.SelectedRows)
            {
                return row.Index;
            }

            if (dataGridViewSelectionTable.CurrentCell != null)
            {
                return dataGridViewSelectionTable.CurrentCell.RowIndex;
            }
            return 0;
        }

        /// <summary>
        /// Handles the Click event of the BtnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void SaveOperation(object sender, System.EventArgs e)
        {
            btnSave.Enabled = false;
            if (dataGridViewSelectionTable.Rows.Count != 0)
            {
                btnSave.Enabled = true;
                CallBackSelectionTable callBack = new CallBackSelectionTable(dlv005DelegateView.TakeValues);
                callBack(selectionTable, selectionTable.SaveOperation(GetCurrentDataGridViewRowIndex(), keyValues, dataGridViewSelectionTable));
                Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UpdateOperation(object sender, EventArgs e)
        {
            string filter = (!string.IsNullOrEmpty(comboBoxColumn.Text) && !string.IsNullOrEmpty(textBoxFilter.Text))
                ? "[" + comboBoxColumn.Text + "]" + " = " + "'" + textBoxFilter.Text + "'" :
               comboBoxColumn.Text + " = " + "'" + "-1" + "'";

            bindingSourceSelectionTable.Filter = filter;
            UpdateDataGridViewElementsCount();
            if (dataGridViewSelectionTable.FirstDisplayedCell != null)
            {
                dataGridViewSelectionTable.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnDownArrow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ChangeSortDirection(object sender, EventArgs e)
        {
            btnArrow.BackgroundImage = (sortingDirection == "downwards") ? Properties.Resources.downArrow : Properties.Resources.upArrow;
            sortingDirection = (sortingDirection == "downwards") ? "upwards" : "downwards";
        }

        /// <summary>
        /// Handles the Click event of the BtnSort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SortOperation(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridViewSelectionTable.Columns)
            {
                ListSortDirection listSortDirection;
                if (column.HeaderText == comboBoxSorting.Text)
                {
                    listSortDirection = (sortingDirection == "upwards") ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    dataGridViewSelectionTable.Sort(column, listSortDirection);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ButtonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CancelOperation(object sender, EventArgs e)
        {
            bindingSourceSelectionTable.Filter = null;
            Close();
        }
    }
}