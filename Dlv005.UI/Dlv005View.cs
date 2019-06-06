using Dlv005.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dlv005.UI
{
    public partial class Dlv005View : Form
    {
        /// <summary>
        /// The is request checked                                             
        /// </summary>
        private int isRequestChecked = 1;

        /// <summary>
        /// The is confirmed checked
        /// </summary>
        private int isConfirmedChecked = 1;

        /// <summary>
        /// The current selection table
        /// </summary>
        private ITable currentSelectionTable;

        /// <summary>
        /// Booleans that marks events around the application.
        /// </summary>
        private bool wasSuccesfullSave = false;

        /// <summary>
        /// The new button was clicked
        /// </summary>
        private bool wasNewButtonClicked = false;

        /// <summary>
        /// The do not validate
        /// </summary>
        private bool isValidationAllowed = false;

        /// <summary>
        /// The we are in edit mode
        /// </summary>
        private bool isEditModeOn = false;

        /// <summary>
        /// The new copy button was clicked
        /// </summary>
        private bool wasNewCopyClicked = false;

        /// <summary>
        /// The insert allocation first row
        /// </summary>
        private bool wasAllocationFirstRowInserted = true;

        /// <summary>
        /// The row keeps the value from comboboxes until save is pressed.
        /// </summary>
        private DataRow comboBoxesValues;

        /// <summary>
        /// The new copy current row.
        /// </summary>
        private DataRow currentRowCopy;

        /// <summary>
        /// The current row
        /// </summary>
        private DataRow currentRow;

        /// <summary>
        /// This is a string where are stored allocation's deleted rows. It is a must beacouse filter.
        /// </summary>
        private string allocationDeletedIDs = string.Empty;

        /// <summary>
        /// The TestingNr and Current Index keeper.
        /// </summary>
        private int currentTestingNrField = 0;

        /// <summary>
        /// The selection table factory.
        /// </summary>
        private SelectionTableFactory selectionTableFactory;

        /// <summary>
        ///  Delegate function for selection table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="view">The view.</param>
        public delegate void CallSelectionTable(ITable table, List<KeyValuePair<string, string>> keyValues, Dlv005View view);

        /// <summary>
        /// The key values
        /// </summary>
        private List<KeyValuePair<string, string>> valuesFromSelectionTable;

        /// <summary>
        /// Business Operations/Validations and DataSet initializations.
        /// </summary>                                                            dataAccess
        private Dlv005BusinessOperations businessOperations = new Dlv005BusinessOperations();

        /// <summary>
        /// The business validations
        /// </summary>
        private Dlv005Validations businessValidations;

        /// <summary>
        /// The data set
        /// </summary>
        private Dlv005DataSet dataSet = new Dlv005DataSet();

        /// <summary>
        /// Initializes a new instance of the <see cref="Dlv005View" /> class.
        /// </summary>
        public Dlv005View()
        {
            InitializeComponent();
            dataSet.InitializeTables();
            businessValidations = new Dlv005Validations(dataSet);
            selectionTableFactory = new SelectionTableFactory(dataSet);
            InitializeUI();
            Shown += Dlv005View_Activated;
        }

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitializeUI()
        {
            InitializeUIBindings();
            InitializeUIElements();
            InitializeUIMandatoryFields();
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            InitializeUIStartingEvents();
        }

        /// <summary>
        /// Initializes the UI bindings.
        /// </summary>
        private void InitializeUIBindings()
        {
            bindingSourceBasicData.DataSource = dataSet.BasicDataTable;
            bindingSourceAllocation.DataSource = dataSet.DL32_EXT_KOMM_KONTO;
            bindingSourceDL38Table.DataSource = dataSet.DL38_KOMM_ERPROBUNGSORT_TBL;
            bindingSourceDL39Table.DataSource = dataSet.DL39_KOMM_STRECKENART_TBL;
            bindingSourceDL40Table.DataSource = dataSet.DL40_KOMM_ERPROBUNGSART_TBL;
            bindingSourceSD111TableLicence.DataSource = dataSet.SD111_QUALIFIKATIONENLicence;
            bindingSourceSD111TableLicence.Filter = Filters.licenceFilter;
            bindingSourceSD111TableHV.DataSource = dataSet.SD111_QUALIFIKATIONENHV;
            bindingSourceSD111TableHV.Filter = Filters.hvFilter;
            bindingSourceSD111TableSpecial.DataSource = dataSet.SD111_QUALIFIKATIONENSpecial;
            bindingSourceSD111TableSpecial.Filter = Filters.specialFilter;
            bindingSourceDataGridViewAllocation.Columns["AllocationColumn"].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            bindingSourceDataGridViewAllocation.Columns["ProcentageColumn"].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
        }

        /// <summary>
        /// Initializes the UI elements.
        /// </summary>
        private void InitializeUIElements()
        {
            IsAutoValidate(true);
            tabPageOverview.Enter += EnterInOverViewPageEvent;
            tabPageBasicData.Enter += PreventEnterBasicDataIfNoDataOnGrid;
            currentTestingNrField = businessOperations.GetLastTestingNumber(dataSet);
            btnSave.Visible = false;
            btnNewCopy.Enabled = true;
        }

        /// <summary>
        /// Initializes the UI mandatory fields.
        /// </summary>
        private void InitializeUIMandatoryFields()
        {
            labelTestingContent.Font = new Font(labelTestingContent.Font, FontStyle.Bold);
            labelAllocation.Font = labelTestingContent.Font;
            labelSeries.Font = labelTestingContent.Font;
            labelSort.Font = labelTestingContent.Font;
            labelHV.Font = labelTestingContent.Font;
            labelSeries.Font = labelTestingContent.Font;
            labelSpecial.Font = labelTestingContent.Font;
            labelCustomer.Font = labelTestingContent.Font;
            labelCustomerOE.Font = labelTestingContent.Font;
            labelRoutes.Font = labelTestingContent.Font;
            labelChief.Font = labelTestingContent.Font;
            labelTesting.Font = labelTestingContent.Font;
            labelEngineering.Font = labelTestingContent.Font;
            labelDriving.Font = labelTestingContent.Font;
            labelFrom.Font = labelTestingContent.Font;
            labelTo.Font = labelTestingContent.Font;
        }

        /// <summary>
        /// Initializes the or update data grid view over view elements count.
        /// </summary>
        private void InitializeOrUpdateDataGridViewOverViewElementsCount()
        {
            tbxNumber.Text = bindingSourceDataGridViewOverview.Rows.Count.ToString();
        }

        /// <summary>
        /// Initializes the UI starting events.
        /// </summary>
        private void InitializeUIStartingEvents()
        {
            InitializeValidationsEvents();
            InitializeDataGridViewsEvents();
            InitializeCheckBoxesEvents();
            InitializeButtonsEvents();
        }

        /// <summary>
        /// Initializes the validations events.
        /// </summary>
        private void InitializeValidationsEvents()
        {
            TextBoxSeries.Validating += ValidateAllMandatory;
            TextBoxCustomerOE.Validating += ValidateAllMandatory;
            TextBoxCustomer.Validating += ValidateAllMandatory;
            TextBoxChief.Validating += ValidateAllMandatory;
            TextBoxEngineeringAST.Validating += ValidateAllMandatory;
            TextBoxTestingContent.Validating += ValidateAllMandatory;
            bindingSourceComboBoxSort.Validating += ValidateAllMandatory;
            bindingSourceComboBoxRoutes.Validating += ValidateAllMandatory;
            bindingSourceComboBoxKindOfTesting.Validating += ValidateAllMandatory;
            bindingSourceComboBoxDrivingAutorization.Validating += ValidateAllMandatory;
            bindingSourceComboBoxHVQualification.Validating += ValidateAllMandatory;
            bindingSourceComboBoxSpecialQualification.Validating += ValidateAllMandatory;
            bindingSourceDateTimePickerTo.Validating += ValidateAllMandatory;
            bindingSourceDataGridViewAllocation.Validating += ValidateAllMandatory;
            bindingSourceDateTimePickerFrom.Validating += ValidateAllMandatory;
        }

        /// <summary>
        /// Initializes the data grid views events.
        /// </summary>
        private void InitializeDataGridViewsEvents()
        {
            bindingSourceDataGridViewOverview.CellMouseDoubleClick += DoubleClickOverview;
            bindingSourceDataGridViewOverview.CellDoubleClick += SelectFullRowWhenCellDoubleClickEvent;
            bindingSourceDataGridViewOverview.CellMouseClick += BindingSourceDataGridViewOverview_CellMouseClick;
            bindingSourceDataGridViewAllocation.CellMouseClick += AllocationContexMenu;
            bindingSourceDataGridViewOverview.SelectionChanged += SelectingRowOnOverViewEvent;
            bindingSourceDataGridViewAllocation.UserDeletingRow += GetIdOfAllocationDeletedRow;
            bindingSourceDataGridViewAllocation.KeyDown += DownArrowKeyPressed;
            bindingSourceDateTimePickerFrom.TextChanged += BindingSourceDateTimePickerFrom_GotFocus;
            bindingSourceDateTimePickerTo.TextChanged += BindingSourceDateTimePickerTo_GotFocus;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        /// Contexes the menu.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void AllocationContexMenu(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bindingSourceDataGridViewAllocation.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                ContextMenu m = new ContextMenu();
                MenuItem copyRow = new MenuItem("Copy Row");
                MenuItem copyColumn = new MenuItem("Copy Column");
                m.MenuItems.Add(copyRow);
                m.MenuItems.Add(copyColumn);
                DataGridViewCell cell = bindingSourceDataGridViewAllocation.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var cellRectangle = bindingSourceDataGridViewAllocation.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                m.Show(bindingSourceDataGridViewAllocation, new Point(cellRectangle.Right, cellRectangle.Top));
                copyRow.Click += AllocationCopyRow; ;
                copyColumn.Click += AllocationCopyColumn; ;
            }
            else
            {
                bindingSourceDataGridViewAllocation.Rows[bindingSourceDataGridViewAllocation.CurrentCell.RowIndex].Selected = true;
            }
        }

        /// <summary>
        /// Allocations the copy column.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AllocationCopyColumn(object sender, EventArgs e)
        {
            string column = string.Empty;
            column += bindingSourceDataGridViewAllocation.Rows[GetCurrentAllocationRowIndex()].Cells[bindingSourceDataGridViewAllocation.CurrentCell.ColumnIndex].Value.ToString();
            Clipboard.SetText(column);
        }

        /// <summary>
        /// Allocations the copy row.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AllocationCopyRow(object sender, EventArgs e)
        {
            string row = string.Empty;
            foreach (DataGridViewCell cell in bindingSourceDataGridViewAllocation.Rows[bindingSourceDataGridViewAllocation.CurrentCell.RowIndex].Cells)
            {
                if (cell.Visible == true)
                {
                    row += cell.Value.ToString() + "          ";
                }
            }
            Clipboard.SetText(row);
        }

        /// <summary>
        /// Handles the GotFocus event of the BindingSourceDateTimePickerTo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BindingSourceDateTimePickerTo_GotFocus(object sender, EventArgs e)
        {
            bindingSourceDateTimePickerTo.CustomFormat = "  MM  /  dd  /  yyyy  ";
        }

        /// <summary>
        /// Handles the GotFocus event of the BindingSourceDateTimePickerFrom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BindingSourceDateTimePickerFrom_GotFocus(object sender, EventArgs e)
        {
            bindingSourceDateTimePickerFrom.CustomFormat = "  MM  /  dd  /  yyyy  ";
        }

        /// <summary>
        /// Initializes the check boxes events.
        /// </summary>
        private void InitializeCheckBoxesEvents()
        {
            checkBoxHideConfirmed.CheckedChanged += CheckedChangedCheckBoxConfirmed;
            checkBoxHideRequested.CheckedChanged += CheeckedChangedCheckBoxRequested;
        }

        /// <summary>
        /// Initializes the buttons events.
        /// </summary>
        private void InitializeButtonsEvents()
        {
            btnSelectionTableEngineering.Click += OpenAndSetSelectionTable;
            btnSelectionTableChief.Click += OpenAndSetSelectionTable;
            btnSelectionTableCustomer.Click += OpenAndSetSelectionTable;
            btnSelectionTableCustomerOE.Click += OpenAndSetSelectionTable;
            btnSelectionTableSeries.Click += OpenAndSetSelectionTable;
            btnConfirm.Click += ConfirmOperation;
            btnRequest.Click += RequestOperation;
            btnDelete.Click += DeleteOperaion;
            btnClose.Click += CloseOperation;
            btnSave.Click += SaveOperaion;
            btnNew.Click += CreateNewOperation;
            btnNewCopy.Click += CreateNewCopyOperation;
        }

        /// <summary>
        /// Gets the identifier from ComboBox.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void GetIdFromComboBox(object sender, EventArgs e)
        {
            comboBoxesValues[((ComboBox)sender).Tag.ToString()] = ((ComboBox)sender).SelectedValue;
        }

        /// <summary>
        /// Validates all mandatory.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void ValidateAllMandatory(object sender, CancelEventArgs e)
        {
            businessValidations.Validate(((Control)sender), errorProvider, e);
        }

        /// <summary>
        /// Opens the and set selection table.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OpenAndSetSelectionTable(object sender, EventArgs e)
        {
            OpenSelectionTable((TextBox)Controls.Find(
                (((Control)sender).Tag.ToString())
                , true)[0]);
        }

        /// <summary>
        /// Opens the selection table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="box">The box.</param>
        private void OpenSelectionTable(TextBox box)
        {
            ITable auxTable = currentSelectionTable;
            currentSelectionTable = selectionTableFactory.GetTable(box);
            if (currentSelectionTable != null)
            {
                ExecuteSelectionTableOperation(currentSelectionTable);
            }
            else
            {
                currentSelectionTable = auxTable;
            }
        }

        /// <summary>
        /// Requests the operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RequestOperation(object sender, EventArgs e)
        {
            UpdateStatus("Requested");
            if (tabControl.SelectedTab == tabPageOverview)
            {
                InitializeDefaultGridSelectedRow();
            }
        }

        /// <summary>
        /// Confirms the operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ConfirmOperation(object sender, EventArgs e)
        {
            UpdateStatus("Confirmed");
            if (tabControl.SelectedTab == tabPageOverview)
            {
                InitializeDefaultGridSelectedRow();
            }
        }

        /// <summary>
        /// Sets the date empty value.
        /// </summary>
        private void SetDateEmptyValue()
        {
            bindingSourceDateTimePickerFrom.CustomFormat = " ";
            bindingSourceDateTimePickerFrom.Format = DateTimePickerFormat.Custom;
            bindingSourceDateTimePickerTo.CustomFormat = " ";
            bindingSourceDateTimePickerTo.Format = DateTimePickerFormat.Custom;
        }

        /// <summary>
        /// Sets the buttons for new commission.
        /// </summary>
        private void SetButtonsForNewCommission()
        {
            wasSuccesfullSave = false;
            btnSave.Visible = true;
            btnClose.Text = "Cancel";
            btnConfirm.Visible = false;
            btnRequest.Visible = false;
            btnNew.Visible = false;
            btnNewCopy.Visible = false;
            btnDelete.Visible = false;
        }

        /// <summary>
        /// Creates the new operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CreateNewOperation(object sender, EventArgs e)
        {
            wasNewButtonClicked = true;
            SetDateEmptyValue();
            UndoViewConfirmedCommission();
            SetButtonsForNewCommission();
            businessOperations.CreateNew(dataSet, -1);
            SetNewRowEmpty();
            SetNewAllocation();
            ChangeSelectedControlTab(tabPageBasicData);
            wasAllocationFirstRowInserted = true;
        }

        /// <summary>
        /// Creates the new copy operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CreateNewCopyOperation(object sender, EventArgs e)
        {
            wasNewCopyClicked = true;
            SetDateEmptyValue();
            UndoViewConfirmedCommission();
            SetButtonsForNewCommission();
            InitializeNewCopyMode(currentRow);
        }

        /// <summary>
        /// Undoes the date empty format.
        /// </summary>
        private void UndoDateEmptyFormat()
        {
            bindingSourceDateTimePickerTo.CustomFormat = "  MM  /  dd  /  yyyy  ";
            bindingSourceDateTimePickerFrom.CustomFormat = "  MM  /  dd  /  yyyy  ";
        }

        /// <summary>
        /// Closes the operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseOperation(object sender, EventArgs e)
        {
            errorProvider.Clear();
            UndoDateEmptyFormat();
            if (btnClose.Text == "Cancel")
            {
                bindingSourceDataGridViewAllocation.AllowUserToAddRows = true;
                UndoDataChanges();
            }
            else
            {
                businessOperations.GetConnection().Close();
                Close();
            }
            isEditModeOn = false;
        }

        /// <summary>
        /// Sets the combo boxes and check boxes for save.
        /// </summary>
        private void SetComboBoxesAndCheckBoxesForSave()
        {
            currentRow["Sunday"] = (chbWorkSunday.Checked == true) ? "j" : "n";
            currentRow["Saturday"] = (chbWorkSaturday.Checked == true) ? "j" : "n";
            if (comboBoxesValues != null)
            {
                currentRow["HvID"] = (comboBoxesValues["HvID"].ToString() != string.Empty) ? comboBoxesValues["HvID"] : currentRow["HvID"];
                currentRow["SpecialID"] = (comboBoxesValues["SpecialID"].ToString() != string.Empty) ? comboBoxesValues["SpecialID"] : currentRow["SpecialID"];
                currentRow["LicenceID"] = (comboBoxesValues["LicenceID"].ToString() != string.Empty) ? comboBoxesValues["LicenceID"] : currentRow["LicenceID"];
                currentRow["SortID"] = (comboBoxesValues["SortID"].ToString() != string.Empty) ? comboBoxesValues["SortID"] : currentRow["SortID"];
                currentRow["KindID"] = (comboBoxesValues["KindID"].ToString() != string.Empty) ? comboBoxesValues["KindID"] : currentRow["KindID"];
                currentRow["RouteID"] = (comboBoxesValues["RouteID"].ToString() != string.Empty) ? comboBoxesValues["RouteID"] : currentRow["RouteID"];
            }
        }

        /// <summary>
        /// Does the save operation.
        /// </summary>
        /// <returns></returns>
        private int DoSaveOperation()
        {
            return (wasNewButtonClicked)
                           ? businessOperations.Save(dataSet, currentRow, "New") :
                       (wasNewCopyClicked)
                           ? businessOperations.Save(dataSet, currentRowCopy, "Copy") :
                        businessOperations.Save(dataSet, currentRow, "Update");
        }

        /// <summary>
        /// Defines the allocation operation.
        /// </summary>
        /// <returns></returns>
        private string DefineAllocationOperation()
        {
            return (wasNewButtonClicked)
                               ? "New" :
                           (wasNewCopyClicked)
                               ? "Copy" :
                             "Update";
        }

        /// <summary>
        /// Saves the operaion.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveOperaion(object sender, EventArgs e)
        {
            int idForUpdatedCommission = Convert.ToInt32(currentRow["Id"]);

            isValidationAllowed = true;

            if (ValidateChildren() && CheckIfNullOrNoDisplay())
            {
                if (bindingSourceDateTimePickerFrom.Value < DateTime.Now.AddDays(7))
                {
                    if (!(MessageBox.Show(Messages.saveDateIfLowerThenCurrentDate, "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        return;
                    }
                }
            }
            else
            {
                isValidationAllowed = false;
                return;
            }
            SetComboBoxesAndCheckBoxesForSave();

            int idForSavedCommission = DoSaveOperation();
            string action = DefineAllocationOperation();
            decimal allocationOperationID = (action == "New" || action == "Copy") ? idForSavedCommission : Convert.ToDecimal(currentRow["Id"]);
            bindingSourceAllocation.EndEdit();

            businessOperations.SaveAllocation(dataSet, allocationDeletedIDs, allocationOperationID);

            wasSuccesfullSave = true;
            wasNewCopyClicked = false;
            wasNewButtonClicked = false;

            SetAllocation(currentRow["Id"].ToString());
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            Reload();
            int id = idForSavedCommission != 0 ? idForSavedCommission : idForUpdatedCommission;
            SelectInsertedRow(id);
            ExitEditMode(true);
            isEditModeOn = false;
        }

        /// <summary>
        /// Deletes the operaion.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeleteOperaion(object sender, EventArgs e)
        {
            if (MessageBox.Show(Messages.deleteCheck, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (CheckIfNullOrNoDisplay())
                {
                    if (bindingSourceDataGridViewOverview.CurrentRow.Cells["StatusTextColumn"].Value.ToString() != "Confirmed")
                    {
                        string id = currentRow["Id"].ToString();
                        businessOperations.DeleteAllocationData(dataSet, id);
                        businessOperations.DoDelete(dataSet, currentRow);
                        Reload();
                    }
                    else
                    {
                        MessageBox.Show("You can't delete a confirmed commission !");
                    }
                }
                ChangeSelectedControlTab(tabPageOverview);
                InitializeOrUpdateDataGridViewOverViewElementsCount();
            }
            if (tabControl.SelectedTab == tabPageOverview)
            {
                InitializeDefaultGridSelectedRow();
            }
            InitializeOrUpdateDataGridViewOverViewElementsCount();
        }

        /// <summary>
        /// Cheeckeds the changed CheckBox requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CheeckedChangedCheckBoxRequested(object sender, EventArgs e)
        {
            if (bindingSourceBasicData.Filter == null || bindingSourceBasicData.Filter == Filters.confirmedCondition)
            {
                bindingSourceBasicData.Filter = (bindingSourceBasicData.Filter == null) ? Filters.requestedCondition : Filters.confirmedAndRequestedCondition;
            }
            else
            {
                bindingSourceBasicData.Filter = (bindingSourceBasicData.Filter == Filters.requestedCondition) ? null : Filters.confirmedCondition;
            }
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            InitializeDefaultGridSelectedRow();
        }

        /// <summary>
        /// Checkeds the changed CheckBox confirmed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CheckedChangedCheckBoxConfirmed(object sender, EventArgs e)
        {
            if (bindingSourceBasicData.Filter == null || bindingSourceBasicData.Filter == Filters.requestedCondition)
            {
                bindingSourceBasicData.Filter = (bindingSourceBasicData.Filter == null) ? Filters.confirmedCondition : Filters.confirmedAndRequestedCondition;
            }
            else
            {
                bindingSourceBasicData.Filter = (bindingSourceBasicData.Filter == Filters.confirmedCondition) ? null : Filters.requestedCondition;
            }
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            InitializeDefaultGridSelectedRow();
        }

        /// <summary>
        /// Handles the CellMouseClick event of the BindingSourceDataGridViewOverview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void BindingSourceDataGridViewOverview_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SetCurrentRow();
            if (bindingSourceDataGridViewOverview.CurrentCell != null && e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (wasNewButtonClicked == false && wasSuccesfullSave == false)
                {
                    chbWorkSunday.Checked = (currentRow["Sunday"].ToString() == "j") ?
                     true : false;
                    chbWorkSaturday.Checked = (currentRow["Saturday"].ToString() == "j") ?
                   true : false;
                }
                else
                {
                    chbWorkSunday.Checked = (currentRow["Sunday"].ToString() == "j") ?
                           true : false;
                    chbWorkSaturday.Checked = (currentRow["Saturday"].ToString() == "j") ?
                     true : false;
                }
                if (e.Button == MouseButtons.Right)
                {
                    bindingSourceDataGridViewOverview.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                    ContextMenu m = new ContextMenu();
                    MenuItem copyRow = new MenuItem("Copy Row");
                    MenuItem copyColumn = new MenuItem("Copy Column");
                    m.MenuItems.Add(copyRow);
                    m.MenuItems.Add(copyColumn);
                    DataGridViewCell cell = bindingSourceDataGridViewOverview.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    var cellRectangle = bindingSourceDataGridViewOverview.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    m.Show(bindingSourceDataGridViewOverview, new Point(cellRectangle.Right, cellRectangle.Top));
                    copyRow.Click += OverviewCopyRow;
                    copyColumn.Click += OverviewCopyColumn;
                }
                else
                {
                    bindingSourceDataGridViewOverview.Rows[bindingSourceDataGridViewOverview.CurrentCell.RowIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the CopyColumn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OverviewCopyColumn(object sender, EventArgs e)
        {
            string column = string.Empty;
            column += bindingSourceDataGridViewOverview.Rows[GetCurrentDataGridViewRowIndex()].Cells[bindingSourceDataGridViewOverview.CurrentCell.ColumnIndex].Value.ToString();
            Clipboard.SetText(column);
        }

        /// <summary>
        /// Handles the Click event of the CopyRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OverviewCopyRow(object sender, EventArgs e)
        {
            string row = string.Empty;
            foreach (DataGridViewCell cell in bindingSourceDataGridViewOverview.Rows[bindingSourceDataGridViewOverview.CurrentCell.RowIndex].Cells)
            {
                if (cell.Visible == true)
                {
                    row += cell.Value.ToString() + "     ";
                }
            }
            Clipboard.SetText(row);
        }

        /// <summary>
        /// Selects the full row when cell double click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void SelectFullRowWhenCellDoubleClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            if (CheckIfNullOrNoDisplay())
            {
                if (bindingSourceDataGridViewOverview.SelectedCells.Count == 0)
                {
                    bindingSourceDataGridViewOverview.Rows[0].Selected = true;
                }
                bindingSourceDataGridViewOverview.CurrentRow.Selected = true;
                btnDelete.Enabled = true;
                btnNewCopy.Enabled = true;
            }
        }

        /// <summary>
        /// Doubles the click overview.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void DoubleClickOverview(object sender, DataGridViewCellMouseEventArgs e)
        {
            SetCurrentRow();
            ChangeSelectedControlTab(tabPageBasicData);
        }

        /// <summary>
        /// Selectings the row on over view event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SelectingRowOnOverViewEvent(object sender, EventArgs e)
        {
            SetCurrentRow();
            UpdateButtonsStatus();
        }

        /// <summary>
        /// Overviews the add row event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewRowsAddedEventArgs"/> instance containing the event data.</param>
        private void OverviewAddRowEvent(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bindingSourceDataGridViewOverview.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Gets the identifier of allocation deleted row.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void GetIdOfAllocationDeletedRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataRow row = (e.Row.DataBoundItem as DataRowView).Row;
            allocationDeletedIDs += row["DL32_KOMM_ANFORDERUNG_KONTO_ID"].ToString() + ',';
        }

        /// <summary>
        /// Downs the arrow key pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void DownArrowKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down && CheckIfLastAllocationRowIsFilled() && bindingSourceDataGridViewAllocation.AllowUserToAddRows == false)
            {
                DataRow row = dataSet.DL32_EXT_KOMM_KONTO.NewDL32_EXT_KOMM_KONTORow();
                row["SourceID"] = currentRow["Id"];
                dataSet.DL32_EXT_KOMM_KONTO.Rows.Add(row);
            }
        }

        /// <summary>
        /// Stops the delete allocation row if only one row.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void StopDeleteAllocationRowIfOnlyOneRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = ((bindingSourceDataGridViewAllocation.Rows.Count == 1 && bindingSourceDataGridViewAllocation.AllowUserToAddRows == false) ||
                 (bindingSourceDataGridViewAllocation.Rows.Count == 2 && bindingSourceDataGridViewAllocation.AllowUserToAddRows == true) ) ? true : false;
        }

        /// <summary>
        /// Selects the current row.
        /// </summary>
        private void SelectCurrentRow(string id)
        {
            foreach (DataGridViewRow row in bindingSourceDataGridViewOverview.Rows)
            {
                if (row.Cells["IdColumn"].Value.ToString() == id)
                {
                    currentRow = (bindingSourceDataGridViewOverview.Rows[row.Index].DataBoundItem as DataRowView).Row;
                    row.Selected = true;
                    bindingSourceDataGridViewOverview.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if commission is confirmed and take action.
        /// </summary>
        private void CheckIfCommissionIsConfirmedAndTakeAction()
        {
            if (currentRow["StatusText"].ToString() == "Confirmed" && wasNewCopyClicked == false && wasNewButtonClicked == false)
            {
                ViewConfirmedCommission();
                btnRequest.Enabled = false;
                btnConfirm.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                UndoViewConfirmedCommission();
            }
        }

        /// <summary>
        /// Stores the CheckBox values.
        /// </summary>
        private void StoreCheckBoxValues()
        {
            if (checkBoxHideRequested.Checked != false && checkBoxHideConfirmed.Checked != false)
            {
                isRequestChecked = (checkBoxHideRequested.Checked) ? 1 : 0;
                isConfirmedChecked = (checkBoxHideConfirmed.Checked) ? 1 : 0;
            }
            checkBoxHideConfirmed.Checked = false;
            checkBoxHideRequested.Checked = false;
        }

        /// <summary>
        /// Prevents the enter basic data if no data on grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PreventEnterBasicDataIfNoDataOnGrid(object sender, EventArgs e)
        {
            if (CheckIfNullOrNoDisplay() == false && wasNewCopyClicked == false)
            {
                tabControl.SelectedTab = tabPageOverview;
                return;
            }
            string selectedRowId = currentRow["Id"].ToString();
            StoreCheckBoxValues();
            SetAllocation(selectedRowId);
            UpdateButtonsStatus();
            CheckIfCommissionIsConfirmedAndTakeAction();
        }

        /// <summary>
        /// Reverts the check boxes values.
        /// </summary>
        private void RevertCheckBoxesValues()
        {
            checkBoxHideRequested.Checked = (isRequestChecked == 1) ? true : false;
            checkBoxHideConfirmed.Checked = (isConfirmedChecked == 1) ? true : false;
        }

        /// <summary>
        /// Enters the in over view page event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EnterInOverViewPageEvent(object sender, EventArgs e)
        {
            if (wasNewButtonClicked || wasNewCopyClicked || isEditModeOn)
            {
                tabControl.SelectedTab = tabPageBasicData;
                return;
            }
            RevertCheckBoxesValues();
        }

        /// <summary>
        /// Handles the Activated event of the Dlv005View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Dlv005View_Activated(object sender, EventArgs e)
        {
            checkBoxHideConfirmed.Checked = true;
            checkBoxHideRequested.Checked = true;
            bindingSourceDataGridViewOverview.Columns[0].Visible = false;
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            bindingSourceDataGridViewOverview.RowsAdded += OverviewAddRowEvent;
            comboBoxesValues = dataSet.BasicDataTable.NewBasicDataTableRow();
            if (CheckIfNullOrNoDisplay())
            {
                InitializeDefaultGridSelectedRow();
                tabPageBasicData.Show();
                bindingSourceDataGridViewOverview.Rows[0].Selected = true;
                bindingSourceDataGridViewOverview.CurrentCell = bindingSourceDataGridViewOverview.Rows[0].Cells[1];
            }
            InitializeChecksForEditMode();
        }

        /// <summary>
        /// Doddges the text change unecessary checks.
        /// </summary>
        private void ClearErrorProviderWhenTextChanged(object sender)
        {
            if ((((Control)sender).GetType().Name == "TextBox" || ((Control)sender).GetType().Name == "ComboBox") && isValidationAllowed == false)
            {
                errorProvider.SetError(((Control)sender), null);
            }

            if (((Control)sender).Name == "bindingSourceDateTimePickerTo" && isValidationAllowed == false)
            {
                if (((DateTimePicker)sender).Value > bindingSourceDateTimePickerFrom.Value)
                {
                    errorProvider.SetError(((Control)sender), null);
                }
                else
                {
                    errorProvider.SetError(((Control)sender), Messages.dateInThePast);
                }
            }
            if (((Control)sender).Name == "bindingSourceDateTimePickerFrom" && isValidationAllowed == false)
            {
                bindingSourceDateTimePickerFrom.Tag = "Modified";
            }
        }

        /// <summary>
        /// Texts the box text changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TextBoxTextChangedEvent(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageOverview)
            {
                return;
            }
            ClearErrorProviderWhenTextChanged(sender);
        }

        /// <summary>
        /// Initializes the checks for edit mode.       THIS IS USED ONLY ONCE !
        /// </summary>
        private void InitializeChecksForEditMode()
        {
            bindingSourceComboBoxHVQualification.SelectionChangeCommitted += GetIdFromComboBox;
            bindingSourceComboBoxSpecialQualification.SelectionChangeCommitted += GetIdFromComboBox;
            bindingSourceComboBoxSort.SelectionChangeCommitted += GetIdFromComboBox; ;
            bindingSourceComboBoxRoutes.SelectionChangeCommitted += GetIdFromComboBox;
            bindingSourceComboBoxDrivingAutorization.SelectionChangeCommitted += GetIdFromComboBox;
            bindingSourceComboBoxKindOfTesting.SelectionChangeCommitted += GetIdFromComboBox;

            bindingSourceComboBoxHVQualification.SelectionChangeCommitted += TextBoxTextChangedEvent;
            bindingSourceComboBoxSpecialQualification.SelectionChangeCommitted += TextBoxTextChangedEvent;
            bindingSourceComboBoxSort.SelectionChangeCommitted += TextBoxTextChangedEvent; ;
            bindingSourceComboBoxRoutes.SelectionChangeCommitted += TextBoxTextChangedEvent;
            bindingSourceComboBoxDrivingAutorization.SelectionChangeCommitted += TextBoxTextChangedEvent;
            bindingSourceComboBoxKindOfTesting.SelectionChangeCommitted += TextBoxTextChangedEvent;

            TextBoxSeries.TextChanged += TextBoxTextChangedEvent;
            TextBoxCustomerOE.TextChanged += TextBoxTextChangedEvent;
            TextBoxCustomer.TextChanged += TextBoxTextChangedEvent;
            TextBoxChief.TextChanged += TextBoxTextChangedEvent;
            TextBoxEngineeringAST.TextChanged += TextBoxTextChangedEvent;
            TextBoxTestingContent.TextChanged += TextBoxTextChangedEvent;
            bindingSourceDateTimePickerTo.TextChanged += TextBoxTextChangedEvent;
            bindingSourceDateTimePickerFrom.TextChanged += TextBoxTextChangedEvent;
            chbWorkSaturday.MouseClick += EnterInEditModeIfCheckBoxWasPressed; ;
            chbWorkSunday.MouseClick += EnterInEditModeIfCheckBoxWasPressed;
            checkBoxHideConfirmed.MouseClick += UpdateRequestConfirmHide;
            checkBoxHideRequested.MouseClick += UpdateRequestConfirmHide;
            bindingSourceDataGridViewAllocation.UserDeletingRow += StopDeleteAllocationRowIfOnlyOneRow;
            bindingSourceDataGridViewAllocation.UserDeletedRow += TextBoxTextChangedEvent;
            bindingSourceDataGridViewAllocation.KeyDown += DoddgeTabInAllocation;

            dataSet.BasicDataTable.ColumnChanged += CheckForChanges;
            dataSet.BasicDataTable.RowChanged += StopUnwantedEditMode;
            bindingSourceDataGridViewAllocation.UserAddedRow += FirstRowInAllocation;
            dataSet.DL32_EXT_KOMM_KONTO.ColumnChanged += AllocationEditModeEnter;
            dataSet.DL32_EXT_KOMM_KONTO.RowDeleted += AllocationRowDeleted;
        }

        /// <summary>
        /// Allocations the row deleted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataRowChangeEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void AllocationRowDeleted(object sender, DataRowChangeEventArgs e)
        {
            EditMode();
        }

        /// <summary>
        /// Stops the unwanted edit mode.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataRowChangeEventArgs"/> instance containing the event data.</param>
        private void StopUnwantedEditMode(object sender, DataRowChangeEventArgs e)
        {
            if(wasNewButtonClicked == false && wasNewCopyClicked == false)
            {
                ExitEditMode(true);
            }
        }

        /// <summary>
        /// Handles the UserAddedRow event of the BindingSourceDataGridViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowEventArgs" /> instance containing the event data.</param>
        private void FirstRowInAllocation(object sender, DataGridViewRowEventArgs e)
        {
            bindingSourceDataGridViewAllocation.EndEdit();

            if (bindingSourceDataGridViewAllocation.Rows[e.Row.Index - 1].Cells["AllocationColumn"].Value.ToString() != string.Empty ||
                bindingSourceDataGridViewAllocation.Rows[e.Row.Index - 1].Cells["ProcentageColumn"].Value.ToString() != string.Empty)
            {
                DataRow newRow = dataSet.DL32_EXT_KOMM_KONTO.NewDL32_EXT_KOMM_KONTORow();

                newRow["SourceID"] = currentRow["Id"];
                newRow["DL32_KONTIERUNG"] = bindingSourceDataGridViewAllocation.Rows[e.Row.Index - 1].Cells[0].Value.ToString();
                newRow["DL32_ANTEIL_PROZENT"] = bindingSourceDataGridViewAllocation.Rows[e.Row.Index - 1].Cells[1].Value.ToString();
                dataSet.DL32_EXT_KOMM_KONTO.Rows.Add(newRow);
            }
            bindingSourceDataGridViewAllocation.AllowUserToAddRows = false;
            bindingSourceAllocation.EndEdit();
        }

        /// <summary>
        /// Doddges the tab in allocation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void DoddgeTabInAllocation(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                bindingSourceDataGridViewAllocation.Enabled = false;
                bindingSourceDataGridViewAllocation.GetNextControl(bindingSourceDataGridViewAllocation, true).Focus();
                bindingSourceDataGridViewAllocation.Enabled = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Enters the in edit mode if CheckBox was pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void EnterInEditModeIfCheckBoxWasPressed(object sender, MouseEventArgs e)
        {
            if (wasNewButtonClicked == false && wasNewCopyClicked == false && isEditModeOn == false)
            {
                EditMode();
            }
        }

        /// <summary>
        /// Handles the ColumnChanged event of the BasicDataTable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataColumnChangeEventArgs"/> instance containing the event data.</param>
        private void CheckForChanges(object sender, DataColumnChangeEventArgs e)
        {
            if (wasNewButtonClicked == false && wasNewCopyClicked == false && isEditModeOn == false
                && tabControl.SelectedTab == tabPageBasicData)
            {
                EditMode();
            }
        }

        /// <summary>
        /// Updates the request confirm hide.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateRequestConfirmHide(object sender, EventArgs e)
        {
            isRequestChecked = (checkBoxHideRequested.Checked) ? 1 : 0;
            isConfirmedChecked = (checkBoxHideConfirmed.Checked) ? 1 : 0;
        }

        /// <summary>
        /// Allocations the edit mode enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataColumnChangeEventArgs"/> instance containing the event data.</param>
        private void AllocationEditModeEnter(object sender, DataColumnChangeEventArgs e)
        {
            EnterInEditMode();
        }

        /// <summary>
        /// Stops the sorting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataColumnChangeEventArgs"/> instance containing the event data.</param>
        private void StopSorting(object sender, DataColumnChangeEventArgs e)
        {
            bindingSourceAllocation.RemoveSort();
        }

        /// <summary>
        /// Initializes the default grid selected row THIS IS USED AFTER ANY OPERATION.
        /// </summary>
        private void InitializeDefaultGridSelectedRow()
        {
            if (bindingSourceDataGridViewOverview.Rows.Count != 0)
            {
                bindingSourceDataGridViewOverview.CurrentCell = bindingSourceDataGridViewOverview.Rows[0].Cells[1];
                bindingSourceDataGridViewOverview.Rows[bindingSourceDataGridViewOverview.CurrentCell.RowIndex].Selected = true;
            }
            else
            {
                tabControl.SelectedTab = tabPageOverview;
            }
            UpdateButtonsStatus();
        }

        /// <summary>
        /// Updates the buttons status.
        /// </summary>
        private void UpdateButtonsStatus()
        {
            if (CheckIfNullOrNoDisplay() && (currentRow["StatusText"] != null))
            {
                if (currentRow["StatusText"].ToString() == "Unchecked")
                {
                    btnConfirm.Enabled = false;
                    btnRequest.Enabled = true;
                }
                if (currentRow["StatusText"].ToString() == "Requested")
                {
                    btnConfirm.Enabled = true;
                    btnRequest.Enabled = false;
                }
                if (currentRow["StatusText"].ToString() == "Confirmed")
                {
                    btnConfirm.Enabled = false;
                    btnRequest.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Initializes the default grid selected row  THIS IS USED AFTER SAVE OPERATION.
        /// </summary>
        private void SelectInsertedRow(int id)
        {
            foreach (DataGridViewRow row in bindingSourceDataGridViewOverview.Rows)
            {
                if (Convert.ToDecimal(row.Cells["IdColumn"].Value) == id)
                {
                    if (bindingSourceDataGridViewOverview.Rows.Count != 0 && bindingSourceDataGridViewOverview.FirstDisplayedCell != null)
                    {
                        bindingSourceDataGridViewOverview.CurrentCell = row.Cells[1];
                        row.Selected = true;
                        currentRow = (row.DataBoundItem as DataRowView).Row;
                    }
                    break;
                }
            }
            bindingSourceAllocation.Filter = "SourceID = " + currentRow["Id"];
        }

        /// <summary>
        /// Takes the values from selection table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keyValues">The key values.</param>
        public void TakeValues(ITable table, List<KeyValuePair<string, string>> keyValues)
        {
            table.SendValues(currentRow, keyValues);
            table.DeleteInstance();
        }

        /// <summary>
        /// Executes the selection table operation. THIS IS USED FOR EACH SELECTION TABLE.
        /// </summary>
        /// <param name="del">The delete.</param>
        private void ExecuteSelectionTableOperation(ITable table)
        {
            SelectionTable seriesTable = new SelectionTable();
            valuesFromSelectionTable = new List<KeyValuePair<string, string>>();
            seriesTable.FormClosed += SelectionTableClose;
            CallSelectionTable callTable = new CallSelectionTable(seriesTable.InitializeValues);
            table.OpenSelectionTable(callTable, valuesFromSelectionTable, this);
            seriesTable.Show();
        }

        /// <summary>
        /// Selections the table close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void SelectionTableClose(object sender, FormClosedEventArgs e)
        {
            currentSelectionTable.DeleteInstance();
        }

        /// <summary>
        /// Updates the status. THIS IS USED WHEN WE UPDATE STATUS FOR COMMISSIONS.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(string status)
        {
            if (CheckIfNullOrNoDisplay())
            {
                decimal id = Convert.ToDecimal(currentRow["Id"]);
                if (status == "Requested")
                {
                    string str = (currentTestingNrField / 100 != 0) ? "" : (currentTestingNrField / 10 != 0) ? "0" : "00";
                    str += currentTestingNrField;
                    currentRow["TestingNr"] = DateTime.Today.ToString("yy") + "/" + str;
                    businessOperations.UpdateStatus(dataSet, id, status);
                    businessOperations.UpdateTestingNr(dataSet, id, currentTestingNrField);
                    currentTestingNrField += 1;
                }
                else
                {
                    businessOperations.UpdateStatus(dataSet, id, "Confirmed");
                }
                currentRow["StatusText"] = status;
                currentRow.AcceptChanges();
            }
            else
            {
                btnRequest.Enabled = false;
                btnConfirm.Enabled = false;
            }
            ExitEditMode(true);
            InitializeOrUpdateDataGridViewOverViewElementsCount();
            UpdateButtonsStatus();
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        private void Reload()
        {
            dataSet.BasicDataTable.Clear();
            businessOperations.InitializeBasicDataTable(dataSet);
            dataSet.SetSeriesNumber(dataSet.BasicDataTable);
            dataSet.BasicDataTable.AcceptChanges();
        }

        /// <summary>
        /// Gets the index of the current data grid view row.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentDataGridViewRowIndex()
        {
            foreach (DataGridViewRow row in bindingSourceDataGridViewOverview.SelectedRows)
            {
                return row.Index;
            }
            if (bindingSourceDataGridViewOverview.CurrentCell != null)
            {
                return bindingSourceDataGridViewOverview.CurrentCell.RowIndex;
            }
            return -1;
        }

        /// <summary>
        /// Gets the index of the current allocation row.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentAllocationRowIndex()
        {
            foreach (DataGridViewRow row in bindingSourceDataGridViewAllocation.SelectedRows)
            {
                return row.Index;
            }
            if (bindingSourceDataGridViewAllocation.CurrentCell != null)
            {
                return bindingSourceDataGridViewAllocation.CurrentCell.RowIndex;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether [is automatic validate] [the specified validate].
        /// </summary>
        /// <param name="validate">if set to <c>true</c> [validate].</param>
        private void IsAutoValidate(bool validate)
        {
            AutoValidate = (validate == true) ? AutoValidate.EnableAllowFocusChange : AutoValidate.Disable;
        }

        /// <summary>
        /// Changes the selected control tab.
        /// </summary>
        /// <param name="control">The control.</param>
        private void ChangeSelectedControlTab(TabPage control)
        {
            tabControl.SelectedTab = control;
        }

        /// <summary>
        /// Sets the new row empty.
        /// </summary>
        private void SetNewRowEmpty()
        {
            if (bindingSourceDataGridViewOverview.Rows.Count != 1 && bindingSourceDataGridViewOverview.FirstDisplayedCell != null)
            {
                bindingSourceDataGridViewOverview.CurrentCell = bindingSourceDataGridViewOverview.Rows[GetCurrentDataGridViewRowIndex()].Cells[1];
            }
        }

        /// <summary>
        /// Checks if null or no display.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfNullOrNoDisplay()
        {
            if (bindingSourceDataGridViewOverview.Rows.Count == 0
                || bindingSourceDataGridViewOverview.FirstDisplayedCell == null)
            {
                btnNewCopy.Enabled = false;
                btnDelete.Enabled = false;
                btnConfirm.Enabled = false;
                btnRequest.Enabled = false;
                return false;
            }
            btnNewCopy.Enabled = true;
            btnDelete.Enabled = true;
            btnConfirm.Enabled = true;
            btnRequest.Enabled = true;
            return true;
        }

        /// <summary>
        /// Sorts the allocation.
        /// </summary>
        private void SortAllocation()
        {
            if (bindingSourceDataGridViewAllocation.Rows.Count != 0)
            {
                bindingSourceDataGridViewAllocation.Sort(bindingSourceDataGridViewAllocation.Rows[0].Cells["ProcentageColumn"].OwningColumn, ListSortDirection.Descending);
            }
        }

        /// <summary>
        /// Filters the allocation.
        /// </summary>
        private void FilterAllocation(string id)
        {
            if (wasNewCopyClicked == false && wasNewButtonClicked == false)
            {
                bindingSourceAllocation.Filter = "SourceID = " + id;
            }
        }

        /// <summary>
        /// Sets the allocation.
        /// </summary>
        private void SetAllocation(string id)
        {
            SelectCurrentRow(id);
            ResetCheckBoxes();
            FilterAllocation(id);
            SortAllocation();
        }

        /// <summary>
        /// Copies the allocation.
        /// </summary>
        private List<DataRow> CopyAllocation()
        {
            bindingSourceAllocation.Filter = "SourceID = " + -1;
            List<DataRow> Rows = new List<DataRow>();
            foreach (DataRow row in dataSet.DL32_EXT_KOMM_KONTO.Rows)
            {
                if (row["SourceID"].ToString() == currentRow["Id"].ToString())
                {
                    DataRow newRow = dataSet.DL32_EXT_KOMM_KONTO.NewDL32_EXT_KOMM_KONTORow();
                    newRow.ItemArray = row.ItemArray;
                    newRow["SourceID"] = -1;
                    Rows.Add(newRow);
                }
            }
            return Rows;
        }

        /// <summary>
        /// Sets the new copy allocation.
        /// </summary>
        /// <param name="Rows">The rows.</param>
        private void SetNewCopyAllocation(List<DataRow> Rows)
        {
            foreach (DataRow row in Rows)
            {
                dataSet.DL32_EXT_KOMM_KONTO.Rows.Add(row);
            }
        }

        /// <summary>
        /// Sets the allocation.
        /// </summary>
        private void SetCopyAllocation()
        {
            SetNewCopyAllocation(CopyAllocation());
        }

        /// <summary>
        /// Sets the new allocation.
        /// </summary>
        private void SetNewAllocation()
        {
            bindingSourceAllocation.Filter = "SourceID = " + -1;
            if (wasAllocationFirstRowInserted == true && wasNewButtonClicked == true && bindingSourceDataGridViewAllocation.Rows.Count == 0)
            {
                DataRow newRow = dataSet.DL32_EXT_KOMM_KONTO.NewDL32_EXT_KOMM_KONTORow();
                newRow["SourceID"] = -1;
                dataSet.DL32_EXT_KOMM_KONTO.Rows.Add(newRow);
                wasAllocationFirstRowInserted = false;
            }
        }

        /// <summary>
        /// Undoes the data changes.
        /// </summary>
        private void UndoDataChanges()
        {
            isValidationAllowed = true;
            dataSet.DL32_EXT_KOMM_KONTO.RejectChanges();
            if (wasNewButtonClicked || wasNewCopyClicked)
            {
                wasNewButtonClicked = false;
                wasNewCopyClicked = false;
                tabControl.SelectedTab = tabPageOverview;
                InitializeDefaultGridSelectedRow();
            }
            else
            {
                ResetDataAfterCancel();
            }
            ExitEditMode(false);

            isValidationAllowed = false;
        }

        /// <summary>
        /// Enters the in edit mode.
        /// </summary>
        public void EnterInEditMode()
        {
            wasSuccesfullSave = false;
            if (tabControl.SelectedTab == tabPageBasicData)
            {
                EditMode();
            }
        }

        /// <summary>
        /// Views the confirmed commission.
        /// </summary>
        private void ViewConfirmedCommission()
        {
            foreach (Control control in tabPageBasicData.Controls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// Undoes the view confirmed commission.
        /// </summary>
        private void UndoViewConfirmedCommission()
        {
            btnDelete.Enabled = true;
            foreach (Control control in tabPageBasicData.Controls)
            {
                control.Enabled = true;
            }
        }

        /// <summary>
        /// Edits the mode.
        /// </summary>
        private void EditMode()
        {
            wasSuccesfullSave = false;
            btnSave.Visible = true;
            btnClose.Text = "Cancel";
            btnNew.Visible = false;
            btnNewCopy.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            btnRequest.Visible = false;
            btnConfirm.Visible = false;
            isEditModeOn = true;
        }

        /// <summary>
        /// Resets the check boxes.
        /// </summary>
        private void ResetCheckBoxes()
        {
            chbWorkSunday.Checked = (currentRow["Sunday"].ToString() == "j") ? true : false;
            chbWorkSaturday.Checked = (currentRow["Saturday"].ToString() == "j") ? true : false;
        }

        /// <summary>
        /// Resets the data after cancel.
        /// </summary>
        private void ResetDataAfterCancel()
        {
            ResetCheckBoxes();
            bindingSourceDateTimePickerFrom.Text = currentRow["From"].ToString();
            bindingSourceDateTimePickerTo.Text = currentRow["To"].ToString();
            TextBoxTestingContent.Text = currentRow["TestingContent"].ToString();
            bindingSourceAllocation.EndEdit();
            bindingSourceBasicData.EndEdit();
        }

        /// <summary>
        /// Exits the edit mode.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        private void ExitEditMode(bool success)
        {
            if (success == false)
            {
                dataSet.BasicDataTable.RejectChanges();
            }
            errorProvider.Clear();
            btnSave.Visible = false;
            btnClose.Text = "Close";
            btnNew.Visible = true;
            btnNewCopy.Visible = true;
            btnDelete.Visible = true;
            btnConfirm.Visible = true;
            btnRequest.Visible = true;
            isEditModeOn = false;
        }

        /// <summary>
        /// Initializes the new copy mode.
        /// </summary>
        /// <param name="row">The row.</param>
        private void InitializeNewCopyMode(DataRow row)
        {
            ChangeSelectedControlTab(tabPageBasicData);
            isValidationAllowed = true;
            currentRowCopy = row;
            wasNewButtonClicked = true;
            btnRequest.Visible = false;
            btnConfirm.Visible = false;
            btnClose.Text = "Cancel";
            btnNew.Visible = false;
            btnNewCopy.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            SetCopyAllocation();
            businessOperations.CreateNewCopy(dataSet, row);
            bindingSourceDataGridViewOverview.CurrentCell = bindingSourceDataGridViewOverview.Rows[GetCurrentDataGridViewRowIndex()].Cells[1];
        }

        /// <summary>
         /// Gets the current row.
        /// </summary>
        /// <returns></returns>
        private void SetCurrentRow()
        {
            if (CheckIfNullOrNoDisplay())
            {
                currentRow = (bindingSourceDataGridViewOverview.Rows[GetCurrentDataGridViewRowIndex()].DataBoundItem as DataRowView).Row;
            }
        }

        /// <summary>
        /// Checks if last allocation row is filled.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfLastAllocationRowIsFilled()
        {
            if (dataSet.DL32_EXT_KOMM_KONTO.GetChanges() != null)
            {
                foreach (DataRow row in dataSet.DL32_EXT_KOMM_KONTO.GetChanges().Rows)
                {
                    if (row.RowState != DataRowState.Deleted &&
                        row["SourceID"].ToString() == currentRow["Id"].ToString() &&
                        (row["DL32_ANTEIL_PROZENT"].ToString() == string.Empty || row["DL32_KONTIERUNG"].ToString() == string.Empty)
                        )                                           
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}