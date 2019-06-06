using MySql.Data.MySqlClient;
using System;
using System.Data;
using static Dlv005.BL.Dlv005DataSet;

namespace Dlv005.BL
{
    public class Dlv005BusinessOperations
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        private readonly DL.Dlv005DataAccessComponent dataAccess = new DL.Dlv005DataAccessComponent();

        /// <summary>
        /// Gets the application connection.
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = dataAccess.GetConnection();
            return connection;
        }

        /// <summary>
        /// Initializes the basic data table.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public void InitializeBasicDataTable(Dlv005DataSet dataSet)
        {
            dataSet.InitializeBasicDataTable();
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        public void CreateNew(Dlv005DataSet dataModel, decimal id)
        {
            BasicDataTableRow newRow = dataModel.BasicDataTable.NewBasicDataTableRow();
            newRow.Id = id;
            newRow.TestingNr = string.Empty;
            newRow.Customer = string.Empty;
            newRow.Chief = string.Empty;
            newRow.Engineering = string.Empty;
            newRow.StatusText = string.Empty;
            newRow.SortingTest = string.Empty;
            newRow.RouteOfTesting = string.Empty;
            newRow.KindOfTesting = string.Empty;
            newRow.Series = string.Empty;
            newRow.TestingContent = string.Empty;
            newRow.Special = string.Empty;
            newRow.Hv = string.Empty;
            newRow.Licence = string.Empty;
            newRow.CustomerOE = string.Empty;
            newRow.Saturday = string.Empty;
            newRow.Sunday = string.Empty;
            newRow.To = DateTime.Now;
            newRow.From = DateTime.Now;
            dataModel.BasicDataTable.AddBasicDataTableRow(newRow);
        }

        /// <summary>
        /// Creates the new copy.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="row">The row.</param>
        public void CreateNewCopy(Dlv005DataSet dataModel, DataRow row)
        {
            BasicDataTableRow newRow = dataModel.BasicDataTable.NewBasicDataTableRow();
            newRow.Id = -1;
            newRow.StatusID = 1;
            newRow.ChiefID = Convert.ToDecimal(row["ChiefID"]);
            newRow.CustomerID = Convert.ToDecimal(row["CustomerID"]);
            newRow.EngineeringID = Convert.ToDecimal(row["EngineeringID"]);
            newRow.SortID = Convert.ToDecimal(row["SortID"]);
            newRow.RouteID = Convert.ToDecimal(row["RouteID"]);
            newRow.SpecialID = Convert.ToDecimal(row["SpecialID"]);
            newRow.LicenceID = Convert.ToDecimal(row["LicenceID"]);
            newRow.CustomerOEID = Convert.ToDecimal(row["CustomerOEID"]);
            newRow.KindID = Convert.ToDecimal(row["KindID"]);
            newRow.HvID = Convert.ToDecimal(row["HvID"]);
            newRow.TestingNr = string.Empty;
            newRow.StatusText = string.Empty;
            newRow.TestingContent = row["TestingContent"].ToString();
            newRow.Saturday = row["Saturday"].ToString();
            newRow.Sunday = row["Sunday"].ToString();
            newRow.Customer = row["Customer"].ToString();
            newRow.Chief = row["Chief"].ToString();
            newRow.Engineering = row["Engineering"].ToString();
            newRow.Series = row["Series"].ToString();
            newRow.SortingTest = row["SortingTest"].ToString();
            newRow.RouteOfTesting = row["RouteOfTesting"].ToString();
            newRow.KindOfTesting = row["KindOfTesting"].ToString();
            newRow.Special = row["Special"].ToString();
            newRow.Hv = row["Hv"].ToString();
            newRow.Licence = row["Licence"].ToString();
            newRow.CustomerOE = row["CustomerOE"].ToString();
            newRow.To = DateTime.Now;
            newRow.From = DateTime.Now;
            dataModel.BasicDataTable.AddBasicDataTableRow(newRow);
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="whatStatusShouldIUpdate">The status.</param>
        /// <returns></returns>
        public bool UpdateStatus(Dlv005DataSet dataModel, decimal id, string whatStatusShouldIUpdate)
        {
            GetConnection().Open();
            MySqlCommand command = dataAccess.UpdateStatus();

            int value = whatStatusShouldIUpdate == "Requested" ? 2 : 3;

            command.Parameters.AddWithValue("@StatusId", value);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            GetConnection().Close();

            return true;
        }

        /// <summary>
        /// Updates the testing nr.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="updateId">The position.</param>
        /// <param name="testingNrValue">The value.</param>
        /// <returns></returns>
        public bool UpdateTestingNr(Dlv005DataSet dataModel, decimal updateId, int testingNrValue)
        {
            GetConnection().Open();
            MySqlCommand command = dataAccess.UpdateTestingNr();
            string str = (testingNrValue / 100 != 0) ? "" : (testingNrValue / 10 != 0) ? "0" : "00";
            str += testingNrValue;

            command.Parameters.AddWithValue("@TestingNr", DateTime.Today.ToString("yy") + "/" + str);
            command.Parameters.AddWithValue("@Id", updateId);
            command.ExecuteNonQuery();

            GetConnection().Close();
            return true;
        }

        /// <summary>
        /// Adds the alocation row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="addedRow">The row.</param>
        /// <returns></returns>
        private void AddAlocationRow(Dlv005DataSet dataSet, DataRow addedRow)
        {
            if (GetConnection().State == ConnectionState.Closed)
            {
                GetConnection().Open();
            }
            if (addedRow["SourceID"].ToString() != string.Empty && addedRow["DL32_ANTEIL_PROZENT"].ToString() != string.Empty &&
                addedRow["DL32_KONTIERUNG"].ToString() != string.Empty)
            {
                MySqlCommand allocation = dataAccess.InsertInDL32();
                allocation.Parameters.AddWithValue("@Text", addedRow["DL32_KONTIERUNG"]);
                allocation.Parameters.AddWithValue("@Procentage", addedRow["DL32_ANTEIL_PROZENT"]);
                allocation.Parameters.AddWithValue("@Id", addedRow["SourceID"]);
                allocation.ExecuteNonQuery();
            }
            GetConnection().Close();
        }

        /// <summary>
        /// Updates the allocation row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="updatedRow">The row.</param>
        /// <returns></returns>
        private void UpdateAllocationRow(Dlv005DataSet dataSet, DataRow updatedRow)
        {
            GetConnection().Open();
            MySqlCommand allocation = dataAccess.UpdateDL32();
            allocation.Parameters.AddWithValue("@Text", updatedRow["DL32_KONTIERUNG"]);
            allocation.Parameters.AddWithValue("@Procentage", updatedRow["DL32_ANTEIL_PROZENT"]);
            allocation.Parameters.AddWithValue("@SourceID", updatedRow["SourceID"]);
            allocation.Parameters.AddWithValue("@Id", updatedRow["DL32_KOMM_ANFORDERUNG_KONTO_ID"]);
            allocation.ExecuteNonQuery();
            GetConnection().Close();
        }

        /// <summary>
        /// Deletes the allocation row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="deletedRowID">The identifier.</param>
        /// <returns></returns>
        public void DeleteAllocationData(Dlv005DataSet dataSet, string deletedRowID)
        {
            GetConnection().Open();
            MySqlCommand allocation = dataAccess.DeleteDL32WithCommission();
            allocation.Parameters.AddWithValue("@Id", deletedRowID);
            allocation.ExecuteNonQuery();
            GetConnection().Close();
        }


        /// <summary>
        /// Deletes the allocation row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="deletedRowID">The identifier.</param>
        /// <returns></returns>
        public void DeleteAllocationRow(Dlv005DataSet dataSet, string deletedRowID)
        {
            GetConnection().Open();
            MySqlCommand allocation = dataAccess.DeleteDL32();
            allocation.Parameters.AddWithValue("@Id", deletedRowID);
            allocation.ExecuteNonQuery();
            GetConnection().Close();
        }

        /// <summary>
        /// Defines the command.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="command">The command.</param>
        /// <param name="model">The model.</param>
        private void DefineCommand(string action, MySqlCommand command, InputModel model, DataRow updatedRow)
        {
            if (action == "New" || action == "Copy")
            {
                command.Parameters.AddWithValue("@TestingNr", string.Empty);
                command.Parameters.AddWithValue("@Status", 1);
            }
            else
            {
                command.Parameters.AddWithValue("@TestingNr", model.TestingNr);
                command.Parameters.AddWithValue("@Status", model.StatusString);
            }
            command.Parameters.AddWithValue("@Saturday", model.Saturday);
            command.Parameters.AddWithValue("@Sunday", model.Sunday);
            command.Parameters.AddWithValue("@TestingContent", model.TestingContent);
            command.Parameters.AddWithValue("@FromDateTime", model.From);
            command.Parameters.AddWithValue("@ToDateTime", model.To);
            command.Parameters.AddWithValue("@CustomerOE", model.CustomerOEID);
            command.Parameters.AddWithValue("@Customer", model.CustomerID);
            command.Parameters.AddWithValue("@Chief", model.CheifID);
            command.Parameters.AddWithValue("@Engineering", model.EngineeringID);
            command.Parameters.AddWithValue("@Id", updatedRow["Id"]);
            command.Parameters.AddWithValue("@Series", model.SeriesID);
            command.Parameters.AddWithValue("@Sort", model.SortID);
            command.Parameters.AddWithValue("@Route", model.RouteID);
            command.Parameters.AddWithValue("@Kind", model.KindID);
            command.Parameters.AddWithValue("@Licence", model.LicenceID);
            command.Parameters.AddWithValue("@Hv", model.HvID);
            command.Parameters.AddWithValue("@Special", model.SpecialID);
        }

        /// <summary>
        /// Saves the specified data model.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="updatedRow">The row.</param>
        /// <param name="action">The action.</param>
        /// <param name="allocationDeletedRowsIds">The allocation deleted rows ids.</param>
        /// <returns></returns>
        public int Save(Dlv005DataSet dataModel, DataRow updatedRow, string action)
        {
            GetConnection().Open();
            MySqlCommand command = (action == "New" || action == "Copy") ? dataAccess.InsertInDL31() : dataAccess.UpdateDL31();
            InputModel model = new InputModel(dataModel, updatedRow, action);

            DefineCommand(action, command, model, updatedRow);

            int id = Convert.ToInt32(command.ExecuteScalar());
            GetConnection().Close();

            return id;
        }

        /// <summary>
        /// Does the delete.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="deletedRow">The row.</param>
        public bool DoDelete(Dlv005DataSet dataModel, DataRow deletedRow)
        {
            GetConnection().Open();
            MySqlCommand command = dataAccess.Delete();
            command.Parameters.AddWithValue("@Id", deletedRow["Id"]);
            command.ExecuteScalar();
            GetConnection().Close();
            return true;
        }

        /// <summary>
        /// Deletes the allocation data.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="idS">The identifier s.</param>
        private void DeleteAllocationData(Dlv005DataSet dataModel, string[] idS)
        {
            foreach (string deleteId in idS)
            {
                DeleteAllocationRow(dataModel, deleteId);
            }
        }

        /// <summary>
        /// Deletes the allocation data.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="idS">The identifier s.</param>
        private void DeleteAllocationRow(Dlv005DataSet dataModel, string[] idS)
        {
            foreach (string deleteId in idS)
            {
                DeleteAllocationRow(dataModel, deleteId);
            }
        }

        /// <summary>
        /// Saves the or update allocation.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="lastModifiedCommisionID">The last modified commision identifier.</param>
        private void SaveOrUpdateAllocation(Dlv005DataSet dataModel, decimal lastModifiedCommisionID)
        {
            foreach (DataRow allocationRow in dataModel.DL32_EXT_KOMM_KONTO.GetChanges().Rows)
            {
                if (allocationRow.RowState != DataRowState.Deleted)
                {
                    allocationRow["SourceID"] = lastModifiedCommisionID;
                }
                if (allocationRow.RowState == DataRowState.Modified)
                {
                    UpdateAllocationRow(dataModel, allocationRow);
                }
                if (allocationRow.RowState == DataRowState.Added)
                {
                    AddAlocationRow(dataModel, allocationRow);
                }
            }
        }

        /// <summary>
        /// Saves the allocation.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="allocationDeletedRowsIds">The allocation deleted rows ids.</param>
        /// <param name="lastModifiedCommisionID">The identifier.</param>
        /// <param name="action">The action.</param>
        public void SaveAllocation(Dlv005DataSet dataModel, string allocationDeletedRowsIds, decimal lastModifiedCommisionID)
        {
            if (dataModel.DL32_EXT_KOMM_KONTO.GetChanges() != null)
            {
                SaveOrUpdateAllocation(dataModel, lastModifiedCommisionID);

                DeleteAllocationRow(dataModel, allocationDeletedRowsIds.Split(','));

                DeleteAllocationUnsavedData(dataModel, lastModifiedCommisionID);
            }
            dataModel.DL32_EXT_KOMM_KONTO.AcceptChanges();
        }

        /// <summary>
        /// Deletes the allocation unsaved data.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="lastModifiedCommisionID">The last modified commision identifier.</param>
        private void DeleteAllocationUnsavedData(Dlv005DataSet dataModel, decimal lastModifiedCommisionID)
        {
            foreach (DataRow row in dataModel.DL32_EXT_KOMM_KONTO.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    if (row["SourceID"].ToString() == "-1")
                    {
                        row["SourceID"] = lastModifiedCommisionID;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the count.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <returns></returns>
        public int GetLastTestingNumber(Dlv005DataSet dataModel)
        {
            if (dataModel.GetLastTestingNrValue(dataAccess) != string.Empty)
            {
                string tNr = dataModel.GetLastTestingNrValue(dataAccess).Split('/')[1];
                return ((tNr[0] == '0') ? (tNr[1] == '0') ? (Convert.ToInt32(tNr[2]) - 48) :
                    (Convert.ToInt32(tNr[1]) - 48) * 10 + (Convert.ToInt32(tNr[2]) - 48) :
                    (Convert.ToInt32(tNr[0]) - 48) * 100 + (Convert.ToInt32(tNr[1]) - 48)
                    * 10 + Convert.ToInt32(tNr[2]) - 48) + 1;
            }
            return 0;
        }
    }
}