using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Dlv005.UI
{
    internal class SelectionTablePersons : ITable
    {
        /// <summary>
        /// The text box
        /// </summary>
        private readonly TextBox textBox;

        /// <summary>
        /// The person type
        /// </summary>
        private readonly SelectionType personType;

        /// <summary>
        /// The source table
        /// </summary>
        private readonly DataTable sourceTable;

        /// <summary>
        /// The select table
        /// </summary>
        private readonly DataTable selectTable;

        /// <summary>
        /// The m instance
        /// </summary>
        private static SelectionTablePersons mInstance;

        /// <summary>
        /// Prevents a default instance of the <see cref="SelectionTableDepartaments"/> class from being created.
        /// </summary>
        /// <exception cref="Exception">WTF, who called this constructor?!?</exception>
        private SelectionTablePersons()
        {
            MessageBox.Show("Already exists");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionTablePersons"/> class.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="personType">Type of the person.</param>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="selectTable">The select table.</param>
        public SelectionTablePersons(TextBox textBox, SelectionType personType, DataTable sourceTable, DataTable selectTable)
        {
            this.personType = personType;
            this.textBox = textBox;
            this.sourceTable = sourceTable;
            this.selectTable = selectTable;
        }

        /// <summary>
        /// Initializes the binding source.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public void InitializeBindingSource(DataGridView grid)
        {
            grid.DataSource = sourceTable;
            string sectionText = string.Empty;
            foreach (DataRow row in selectTable.Rows)
            {
                foreach (DataRow perRow in sourceTable.Rows)
                    if (row["Section"].ToString() == perRow["Department"].ToString())
                    {
                        perRow["Department"] = row["Short description"].ToString();
                        break;
                    }
            }
        }

        /// <summary>
        /// Initializes the multi select.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        public void InitializeMultiSelect(DataGridView sourceTable)
        {
            sourceTable.MultiSelect = false;
        }

        /// <summary>
        /// Initializes the name.
        /// </summary>
        /// <param name="form">The form.</param>
        public void InitializeName(Form form)
        {
            form.Text = "Selection Persons";
        }

        /// <summary>
        /// Saves the operation.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="dataGridViewSelectionTable">The data grid view selection table.</param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> SaveOperation(int currentPosition, List<KeyValuePair<string, string>> keyValues, DataGridView dataGridViewSelectionTable)

        {
            string str = sourceTable.Rows[currentPosition]["Name"].ToString() + ", "
                                       + sourceTable.Rows[currentPosition]["Vorname"].ToString()
                                       + ", " + sourceTable.Rows[currentPosition]["Department"].ToString();
            keyValues.Add(new KeyValuePair<string, string>
                (sourceTable.Rows[currentPosition]["Id"].ToString(), str));

            return keyValues;
        }

        /// <summary>
        /// Sends the values.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="values">The values.</param>
        public void SendValues(DataRow row, List<KeyValuePair<string, string>> values)
        {
            textBox.Text = values[0].Value;

            row[personType.InsertionID] = values[0].Key;
            row[personType.InsertionText] = values[0].Value;
        }

        /// <summary>
        /// Opens the selection table.
        /// </summary>
        /// <param name="callTableList">The call table list.</param>
        /// <param name="valuesFromSelectionTable">The values from selection table.</param>
        /// <param name="view">The view.</param>
        public void OpenSelectionTable(Dlv005View.CallSelectionTable callTableList, List<KeyValuePair<string, string>> valuesFromSelectionTable, Dlv005View view)
        {
            callTableList(this, valuesFromSelectionTable, view);
        }

        /// <summary>
        /// Deletes the instance.
        /// </summary>
        public void DeleteInstance()
        {
            mInstance = null;
        }

        /// <summary>
        /// Creates the specified text box.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="personType">Type of the person.</param>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="selectTable">The select table.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object already created</exception>
        public static SelectionTablePersons Create(TextBox textBox, SelectionType personType, DataTable sourceTable, DataTable selectTable)
        {
            return (mInstance == null) ? mInstance = new SelectionTablePersons(textBox, personType, sourceTable, selectTable) : null;
        }
    }
}