using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dlv005.BL
{
    public class Dlv005Validations
    {
        /// <summary>
        /// The data set
        /// </summary>
        private readonly Dlv005DataSet dataSet;

        /// <summary>
        /// The error provider
        /// </summary>
        private ErrorProvider globalErrorProvider;

        /// <summary>
        /// The e
        /// </summary>
        private CancelEventArgs cancelEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dlv005Validations" /> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public Dlv005Validations(Dlv005DataSet dataSet)
        {
            this.dataSet = dataSet;
        }

        /// <summary>
        /// Validates the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="errorProvider">The error provider.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        public void Validate(Control control, ErrorProvider errorProvider, CancelEventArgs e)
        {
            globalErrorProvider = errorProvider;
            cancelEvent = e;
            string message = null;

            switch (control.Name)
            {
                case "bindingSourceDataGridViewAllocation":
                    message = ValidateAllocationTable((DataGridView)control);
                    cancelEvent.Cancel = (message == null) ? false : true;

                    break;

                case "bindingSourceComboBoxSpecialQualification":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.SD111_QUALIFIKATIONENSpecial, "SD111_WERT")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceComboBoxHVQualification":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.SD111_QUALIFIKATIONENHV, "SD111_WERT")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceComboBoxDrivingAutorization":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.SD111_QUALIFIKATIONENLicence, "SD111_WERT")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceDateTimePickerFrom":
                    message = (((DateTimePicker)control).CustomFormat == " ") ? Messages.emptyMandatory
                        : (((DateTimePicker)control).Value < DateTime.Now.AddDays(-1)) ? Messages.dateInThePast : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceDateTimePickerTo":
                    message = (((DateTimePicker)control).CustomFormat == " ") ? Messages.emptyMandatory
                        : (((DateTimePicker)control).Value < ((DateTimePicker)control).Value) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceComboBoxKindOfTesting":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                         : !(ValidateComboBoxTrashValue(control.Text, dataSet.DL40_KOMM_ERPROBUNGSART_TBL, "DL40_BEZEICHNUNG")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "bindingSourceComboBoxRoutes":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.DL39_KOMM_STRECKENART_TBL, "DL39_BEZEICHNUNG")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;

                    break;

                case "bindingSourceComboBoxSort":
                    message = (((ComboBox)control).Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.DL38_KOMM_ERPROBUNGSORT_TBL, "DL38_BEZEICHNUNG")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "TextBoxSeries":
                    message = (control.Text == string.Empty) ? Messages.emptyMandatory
                        : (TrashSeries(control.Text, dataSet.BD12_BAUREIHE)) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "TextBoxCustomerOE":
                    message = (control.Text == string.Empty) ? Messages.emptyMandatory
                        : !(ValidateComboBoxTrashValue(control.Text, dataSet.BD06_ORG_EINHEIT_TBL, "Short description")) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;

                case "TextBoxTestingContent":
                    message = (control.Text == string.Empty) ? Messages.emptyMandatory : null;
                    cancelEvent.Cancel = (message == null) ? false : true;

                    break;

                case "TextBoxCustomer":
                case "TextBoxChief":
                case "TextBoxEngineeringAST":
                    message = (control.Text == string.Empty) ? Messages.emptyMandatory : !(ValidateCustomerAndChiefAndEngineering(control.Text, dataSet.BD09_PERSON, dataSet.BD06_ORG_EINHEIT_TBL)) ? Messages.trashValueComboBox : null;
                    cancelEvent.Cancel = (message == null) ? false : true;
                    break;
            }
            ValidateEveryThing(control, message);
        }

        /// <summary>
        /// Validates the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="messasge">The messasge.</param>
        private void ValidateEveryThing(Control control, string messasge)
        {
            globalErrorProvider.SetError(control, messasge);
        }

        /// <summary>
        /// Validates the series.
        /// </summary>
        /// <param name="valueFromTextBox">The value from text box.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        private bool TrashSeries(string valueFromTextBox, Dlv005DataSet.BD12_BAUREIHEDataTable table)
        {
            return Regex.Matches(valueFromTextBox, @"[a-zA-Z]").Count != 0 ||
                !IsSerriesTrashValue(valueFromTextBox, table);
        }

        /// <summary>
        /// Checks the series trash value.    Regex : https://regex101.com/r/wqdBKT/1
        /// </summary>
        /// <param name="valueFromTextBox">The value from text box.</param>
        /// <param name="table">The table.</param>
        /// <returns>
        ///   <c>true</c> if [is serries trash value] [the specified value from text box]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSerriesTrashValue(string valueFromTextBox, Dlv005DataSet.BD12_BAUREIHEDataTable table)
        {
            char[] delimiterChars = { ',', ';', '`' };
            string[] words = valueFromTextBox.Split(delimiterChars);
            int matchedValues = 0;

            foreach (DataRow row in table)
            {                     
                if (Regex.Matches(valueFromTextBox,
                    "(^" + row["Series"] + "($|[;',][0-9a-zA-Z]))|([;',])(" + row["Series"] + "($|[;',][0-9a-zA-Z]))|([;',])(" + row["Series"] + ")($)").Count == 1)
                {
                    matchedValues++;
                }
            }
            return (matchedValues == words.Length);
        }

        /// <summary>
        /// Validates the engineering.
        /// </summary>
        /// <param name="valueFromBox">The value from box.</param>
        /// <param name="table">The table.</param>
        /// <param name="BD06Table">The b D06 table.</param>
        /// <returns></returns>
        public bool ValidateCustomerAndChiefAndEngineering(string valueFromBox, Dlv005DataSet.BD09_PERSONDataTable table, Dlv005DataSet.BD06_ORG_EINHEIT_TBLDataTable BD06Table)
        {
            foreach (DataRow row in table)
            {
                if (GeneratePersonFullNameWithDepartament(row, BD06Table).Equals(valueFromBox, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Generates the person full name with departament.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        private string GeneratePersonFullNameWithDepartament(DataRow row, Dlv005DataSet.BD06_ORG_EINHEIT_TBLDataTable table)
        {
            string totalValue = string.Empty;
            totalValue += row["Name"].ToString() + ", " + row["Vorname"].ToString() + ", " + GetSection(row["Department"].ToString(), table);

            return totalValue;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        private string GetSection(string departament, Dlv005DataSet.BD06_ORG_EINHEIT_TBLDataTable table)
        {
            string str = string.Empty;
            foreach (DataRow row in table)
            {
                if (row[1].ToString() == departament || row[0].ToString() == departament)
                {
                    str = row[0].ToString();
                    break;
                }
            }
            return str;
        }

        /// <summary>
        /// Procentages the has only digits.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="grid">The grid.</param>
        /// <returns>
        ///   <c>true</c> if [has procentage only digits] [the specified row]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasProcentageOnlyDigits(DataRow row, DataGridView grid)
        {
            return Regex.Matches(row["DL32_ANTEIL_PROZENT"].ToString(), @"[^0-9]+").Count != 0 ? false : true;
        }

        /// <summary>
        /// Determines whether [has allocation special chars] [the specified row].
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="grid">The grid.</param>
        /// <returns>
        ///   <c>true</c> if [has allocation special chars] [the specified row]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAllocationSpecialChars(DataRow row, DataGridView grid)
        {
            return (Regex.Matches(row["DL32_KONTIERUNG"].ToString(), @"[^a-zA-Z0-9]+").Count == 0) ? false : true;
        }

        /// <summary>
        /// Determines whether [is procentage bigger then100] [the specified row].
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="grid">The grid.</param>
        /// <returns>
        ///   <c>true</c> if [is procentage bigger then100] [the specified row]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsProcentageBiggerThen100(DataRow row, DataGridView grid)
        {
            return (Convert.ToDecimal(row["DL32_ANTEIL_PROZENT"]) > 100);
        }

        /// <summary>
        /// Determines whether [has allocation row null values] [the specified row].
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="grid">The grid.</param>
        /// <returns>
        ///   <c>true</c> if [has allocation row null values] [the specified row]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAllocationRowNullValues(DataRow row, DataGridView grid)
        {
            return (row["DL32_ANTEIL_PROZENT"].ToString() == string.Empty ||
                row["DL32_KONTIERUNG"].ToString() == string.Empty);
        }

        /// <summary>
        /// Validates the allocation table.
        /// </summary>
        /// <param name="grid">The grid.</param>
        private string ValidateAllocationTable(DataGridView grid)
        {
            decimal procentageColumnsSum = 0;

            DataRow convertedRow = null;
            string message = null;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if ((row.DataBoundItem != null))
                {
                    convertedRow = (row.DataBoundItem as DataRowView).Row;

                    message = (HasAllocationRowNullValues(convertedRow, grid)) ?
                    Messages.emptyMandatory :
                    !HasProcentageOnlyDigits(convertedRow, grid) ?
                    Messages.incorectFormat :
                         HasAllocationSpecialChars(convertedRow, grid) ?
                         Messages.incorectFormat :
                         IsProcentageBiggerThen100(convertedRow, grid) ?
                                  Messages.incorectFormat : null;

                    if (message == null)
                    {
                        procentageColumnsSum += Convert.ToDecimal(convertedRow["DL32_ANTEIL_PROZENT"]);
                    }
                    else
                    {
                        return message;
                    }
                }
                else
                {
                    if(grid.Rows.Count < 1)
                    {
                        return Messages.emptyMandatory;
                    }
                }
            }

            if (procentageColumnsSum != 100)
            {
                return Messages.allocation100;
            }

            return null;
        }

        /// <summary>
        /// Validates the licence.
        /// </summary>
        /// <param name="valueFromBox">The value from box.</param>
        /// <param name="sD111_QUALIFIKATIONENLicence">The s D111 qualifikationen licence.</param>
        /// <returns></returns>
        public bool ValidateComboBoxTrashValue(string valueFromBox, DataTable table, string column)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[column].ToString() == valueFromBox)
                {
                    return true;
                }
            }
            return false;
        }
    }
}