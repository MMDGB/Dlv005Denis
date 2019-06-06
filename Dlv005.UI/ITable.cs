using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using static Dlv005.UI.Dlv005View;

namespace Dlv005.UI
{
    public interface ITable
    {
        /// <summary>
        /// Initializes the name.
        /// </summary>
        /// <param name="form">The form.</param>
        void InitializeName(Form form);

        /// <summary>
        /// Initializes the binding source.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        void InitializeBindingSource(DataGridView sourceTable);

        /// <summary>
        /// Initializes the multi select.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        void InitializeMultiSelect(DataGridView sourceTable);

        /// <summary>
        /// Opens the selection table.
        /// </summary>
        /// <param name="callTableList">The call table list.</param>
        /// <param name="valuesFromSelectionTable">The values from selection table.</param>
        /// <param name="view">The view.</param>
        void OpenSelectionTable(CallSelectionTable callTableList, List<KeyValuePair<string, string>> valuesFromSelectionTable, Dlv005View view);

        /// <summary>
        /// Sends the values.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="values">The values.</param>
        void SendValues(DataRow row, List<KeyValuePair<string, string>> values);

        /// <summary>
        /// Saves the operation.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="dataGridViewSelectionTable">The data grid view selection table.</param>
        /// <returns></returns>
        List<KeyValuePair<string, string>> SaveOperation(int currentPosition, List<KeyValuePair<string, string>> keyValues, DataGridView dataGridViewSelectionTable);

        /// <summary>
        /// Deletes the instance.
        /// </summary>
        void DeleteInstance();
    }
}