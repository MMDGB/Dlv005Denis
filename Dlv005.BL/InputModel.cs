using System;
using System.Data;

namespace Dlv005.BL
{
    internal class InputModel
    {
        /// <summary>
        /// Gets or sets the testing nr.
        /// </summary>
        /// <value>
        /// The testing nr.
        /// </value>
        public string TestingNr { get; set; }

        /// <summary>
        /// Gets or sets the content of the testing.
        /// </summary>
        /// <value>
        /// The content of the testing.
        /// </value>
        public string TestingContent { get; set; }

        /// <summary>
        /// Gets or sets the status string.
        /// </summary>
        /// <value>
        /// The status string.
        /// </value>
        public string StatusString { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public string StatusID { get; set; }

        /// <summary>
        /// Gets or sets the customer string.
        /// </summary>
        /// <value>
        /// The customer string.
        /// </value>
        public string CustomerString { get; set; }

        /// <summary>
        /// Gets or sets the chief string.
        /// </summary>
        /// <value>
        /// The chief string.
        /// </value>
        public string ChiefString { get; set; }

        /// <summary>
        /// Gets or sets the engineering string.
        /// </summary>
        /// <value>
        /// The engineering string.
        /// </value>
        public string EngineeringString { get; set; }

        /// <summary>
        /// Gets or sets the sort string.
        /// </summary>
        /// <value>
        /// The sort string.
        /// </value>
        public string SortString { get; set; }

        /// <summary>
        /// Gets or sets the route string.
        /// </summary>
        /// <value>
        /// The route string.
        /// </value>
        public string RouteString { get; set; }

        /// <summary>
        /// Gets or sets the kind string.
        /// </summary>
        /// <value>
        /// The kind string.
        /// </value>
        public string KindString { get; set; }

        /// <summary>
        /// Gets or sets the series string.
        /// </summary>
        /// <value>
        /// The series string.
        /// </value>
        public string SeriesString { get; set; }

        /// <summary>
        /// Gets or sets the special string.
        /// </summary>
        /// <value>
        /// The special string.
        /// </value>
        public string SpecialString { get; set; }

        /// <summary>
        /// Gets or sets the series identifier.
        /// </summary>
        /// <value>
        /// The series identifier.
        /// </value>
        public string SeriesID { get; set; }

        /// <summary>
        /// Gets or sets the hv string.
        /// </summary>
        /// <value>
        /// The hv string.
        /// </value>
        public string HvString { get; set; }

        /// <summary>
        /// Gets or sets the licence string.
        /// </summary>
        /// <value>
        /// The licence string.
        /// </value>
        public string LicenceString { get; set; }

        /// <summary>
        /// Gets or sets the customer oe string.
        /// </summary>
        /// <value>
        /// The customer oe string.
        /// </value>
        public string CustomerOEString { get; set; }

        /// <summary>
        /// Gets or sets the saturday.
        /// </summary>
        /// <value>
        /// The saturday.
        /// </value>
        public string Saturday { get; set; }

        /// <summary>
        /// Gets or sets the sunday.
        /// </summary>
        /// <value>
        /// The sunday.
        /// </value>
        public string Sunday { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public decimal CustomerID { get; set; }

        /// <summary>
        /// Gets or sets the cheif identifier.
        /// </summary>
        /// <value>
        /// The cheif identifier.
        /// </value>
        public decimal CheifID { get; set; }

        /// <summary>
        /// Gets or sets the engineering identifier.
        /// </summary>
        /// <value>
        /// The engineering identifier.
        /// </value>
        public decimal EngineeringID { get; set; }

        /// <summary>
        /// Gets or sets the sort identifier.
        /// </summary>
        /// <value>
        /// The sort identifier.
        /// </value>
        public decimal SortID { get; set; }

        /// <summary>
        /// Gets or sets the route identifier.
        /// </summary>
        /// <value>
        /// The route identifier.
        /// </value>
        public decimal RouteID { get; set; }

        /// <summary>
        /// Gets or sets the kind identifier.
        /// </summary>
        /// <value>
        /// The kind identifier.
        /// </value>
        public decimal KindID { get; set; }

        /// <summary>
        /// Gets or sets the special identifier.
        /// </summary>
        /// <value>
        /// The special identifier.
        /// </value>
        public decimal SpecialID { get; set; }

        /// <summary>
        /// Gets or sets the hv identifier.
        /// </summary>
        /// <value>
        /// The hv identifier.
        /// </value>
        public decimal HvID { get; set; }

        /// <summary>
        /// Gets or sets the licence identifier.
        /// </summary>
        /// <value>
        /// The licence identifier.
        /// </value>
        public decimal LicenceID { get; set; }

        /// <summary>
        /// Gets or sets the customer oeid.
        /// </summary>
        /// <value>
        /// The customer oeid.
        /// </value>
        public decimal CustomerOEID { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public DateTime To { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public DateTime From { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputModel"/> class.
        /// </summary>
        /// <param name="DataSet">The data set.</param>
        /// <param name="row">The row.</param>
        /// <param name="action">The action.</param>
        public InputModel(Dlv005DataSet DataSet, DataRow row, string action)
        {
            int lastRowIndex = DataSet.BasicDataTable.Rows.Count - 1; //maybe lastRowIndex

            DataRow lastRow = DataSet.BasicDataTable.Rows[lastRowIndex]; // this is actually the lastRow

            switch (action)
            {
                case "New":
                    SetHeaderValues(lastRow);
                    SetCheckBoxesValue(lastRow);
                    SetStringValues(row, lastRow);
                    SetIdValues(row, lastRow);
                    break;

                case "Copy":
                    SetHeaderValues(lastRow);
                    SetCheckBoxesValue(row);
                    SetStringValues(row, lastRow);
                    SetIdValues(row, lastRow);
                    break;

                case "Update":

                    SetHeaderValues(row);
                    SetCheckBoxesValue(row);

                    CustomerOEString = row["CustomerOE"].ToString();
                    CustomerString = row["Customer"].ToString();
                    ChiefString = row["Chief"].ToString();
                    SortString = row["SortingTest"].ToString();
                    EngineeringString = row["Engineering"].ToString();
                    RouteString = row["RouteOfTesting"].ToString();
                    KindString = row["KindOfTesting"].ToString();
                    SeriesString = row["Series"].ToString();
                    SpecialString = row["Special"].ToString();
                    SeriesID = row["Series"].ToString();
                    HvString = row["Hv"].ToString();
                    LicenceString = row["Licence"].ToString();

                    CustomerOEID = Convert.ToDecimal(row["CustomerOEID"].ToString());
                    CustomerID = Convert.ToDecimal(row["CustomerID"].ToString());
                    CheifID = Convert.ToDecimal(row["ChiefID"].ToString());
                    EngineeringID = Convert.ToDecimal(row["EngineeringID"].ToString());
                    SortID = Convert.ToDecimal(row["SortID"].ToString());
                    RouteID = Convert.ToDecimal(row["RouteID"].ToString());
                    KindID = Convert.ToDecimal(row["KindID"].ToString());
                    SpecialID = Convert.ToDecimal(row["SpecialID"].ToString());
                    HvID = Convert.ToDecimal(row["HvID"].ToString());
                    LicenceID = Convert.ToDecimal(row["LicenceID"].ToString());
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the check boxes value.
        /// </summary>
        /// <param name="row">The row.</param>
        private void SetCheckBoxesValue(DataRow row)
        {
            Saturday = row["Saturday"].ToString();
            Sunday = row["Sunday"].ToString();
        }

        /// <summary>
        /// Sets the header values.
        /// </summary>
        /// <param name="row">The row.</param>
        private void SetHeaderValues(DataRow row)
        {
            To = Convert.ToDateTime(row["To"]);
            From = Convert.ToDateTime(row["From"]);
            TestingContent = row["TestingContent"].ToString();
            TestingNr = row["TestingNr"].ToString();
            StatusString = row["StatusText"].ToString();
            StatusID = row["StatusID"].ToString();
        }

        /// <summary>
        /// Sets the identifier values.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="lrow">The lrow.</param>
        private void SetIdValues(DataRow row, DataRow lrow)
        {
            CustomerOEID = (lrow["CustomerOEID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["CustomerOEID"].ToString()) : Convert.ToDecimal(lrow["CustomerOEID"].ToString());
            CustomerID = (lrow["CustomerID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["CustomerID"].ToString()) : Convert.ToDecimal(lrow["CustomerID"].ToString());
            CheifID = (lrow["ChiefID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["ChiefID"].ToString()) : Convert.ToDecimal(lrow["ChiefID"].ToString());
            EngineeringID = (lrow["EngineeringID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["EngineeringID"].ToString()) : Convert.ToDecimal(lrow["EngineeringID"].ToString());
            SortID = (lrow["SortID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["SortID"].ToString()) : Convert.ToDecimal(lrow["SortID"].ToString());
            RouteID = (lrow["RouteID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["RouteID"].ToString()) : Convert.ToDecimal(lrow["RouteID"].ToString());
            KindID = (lrow["KindID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["KindID"].ToString()) : Convert.ToDecimal(lrow["KindID"].ToString());
            SeriesID = (lrow["Series"].ToString() == string.Empty) ?
                row["Series"].ToString() : lrow["Series"].ToString();
            SpecialID = (lrow["SpecialID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["SpecialID"].ToString()) : Convert.ToDecimal(lrow["SpecialID"].ToString());
            HvID = (lrow["HvID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["HvID"].ToString()) : Convert.ToDecimal(lrow["HvID"].ToString());
            LicenceID = (lrow["LicenceID"].ToString() == string.Empty) ?
                Convert.ToDecimal(row["LicenceID"].ToString()) : Convert.ToDecimal(lrow["LicenceID"].ToString());
        }

        /// <summary>
        /// Sets the string values.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="lastRow">The last row.</param>
        private void SetStringValues(DataRow row, DataRow lastRow)
        {
            CustomerOEString = (lastRow["CustomerOE"].ToString() == string.Empty) ? row["CustomerOE"].ToString() : lastRow["CustomerOE"].ToString();
            CustomerString = (lastRow["Customer"].ToString() == string.Empty) ? row["Customer"].ToString() : lastRow["Customer"].ToString();
            ChiefString = (lastRow["Chief"].ToString() == string.Empty) ? row["Chief"].ToString() : lastRow["Chief"].ToString();
            EngineeringString = (lastRow["Engineering"].ToString() == string.Empty) ? row["Engineering"].ToString() : lastRow["Engineering"].ToString();
            SortString = (lastRow["SortingTest"].ToString() == string.Empty) ? row["SortingTest"].ToString() : lastRow["SortingTest"].ToString();
            RouteString = (lastRow["RouteOfTesting"].ToString() == string.Empty) ? row["RouteOfTesting"].ToString() : lastRow["RouteOfTesting"].ToString();
            KindString = (lastRow["KindOfTesting"].ToString() == string.Empty) ? row["KindOfTesting"].ToString() : lastRow["KindOfTesting"].ToString();
            SeriesString = (lastRow["Series"].ToString() == string.Empty) ? row["Series"].ToString() : lastRow["Series"].ToString();
            SpecialString = (lastRow["Special"].ToString() == string.Empty) ? row["Special"].ToString() : lastRow["Special"].ToString();
            HvString = (lastRow["Hv"].ToString() == string.Empty) ? row["Hv"].ToString() : lastRow["Hv"].ToString();
            LicenceString = (lastRow["Licence"].ToString() == string.Empty) ? row["Licence"].ToString() : lastRow["Licence"].ToString();
        }
    }
}