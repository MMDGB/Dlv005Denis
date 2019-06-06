using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Dlv005.UI
{
    internal class SelectionTableDepartaments : ITable
    {
        /// <summary>
        /// The text box
        /// </summary>
        private readonly TextBox textBox;

        /// <summary>
        /// The source table
        /// </summary>
        private readonly DataTable sourceTable;

        /// <summary>
        /// The selection type
        /// </summary>
        private readonly SelectionType selectionType;

        /// <summary>
        /// The m instance
        /// </summary>
        private static SelectionTableDepartaments mInstance;

        /// <summary>
        /// Prevents a default instance of the <see cref="SelectionTableDepartaments" /> class from being created.
        /// </summary>
        /// <exception cref="Exception">WTF, who called this constructor?!?</exception>
        private SelectionTableDepartaments()
        {
            MessageBox.Show("Already exists");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionTableDepartaments"/> class.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="sourceTable">The source table.</param>
        private SelectionTableDepartaments(TextBox textBox, DataTable sourceTable, SelectionType type)
        {
            this.textBox = textBox;
            this.sourceTable = sourceTable;
            this.selectionType = type;
        }

        /// <summary>
        /// Initializes the name.
        /// </summary>
        /// <param name="form">The form.</param>
        public void InitializeName(Form form)
        {
            form.Text = "Selection Departaments";
        }

        /// <summary>
        /// Initializes the binding source.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public void InitializeBindingSource(DataGridView grid)
        {
            grid.DataSource = sourceTable;
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
        /// Saves the operation.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="dataGridViewSelectionTable">The data grid view selection table.</param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> SaveOperation(int currentPosition, List<KeyValuePair<string, string>> keyValues, DataGridView dataGridViewSelectionTable)

        {
            keyValues.Add(new KeyValuePair<string, string>
                                        (sourceTable.Rows[currentPosition]["Section"].ToString(),
                                        sourceTable.Rows[currentPosition]["Short description"].ToString()));
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
            row[selectionType.InsertionID] = values[0].Key;
            row[selectionType.InsertionText] = values[0].Value;
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
        /// <param name="sourceTable">The source table.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Object already created</exception>
        public static SelectionTableDepartaments Create(TextBox textBox, DataTable sourceTable, SelectionType type)
        {
            return (mInstance == null) ? mInstance = new SelectionTableDepartaments(textBox, sourceTable, type) : null;
        }
    }
}