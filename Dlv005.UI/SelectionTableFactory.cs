using Dlv005.BL;
using System.Windows.Forms;

namespace Dlv005.UI
{
    public class SelectionTableFactory
    {
        /// <summary>
        /// The data set
        /// </summary>
        private readonly Dlv005DataSet dataSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionTableFactory"/> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public SelectionTableFactory(Dlv005DataSet dataSet)
        {
            this.dataSet = dataSet;
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="box">The box.</param>
        /// <returns></returns>
        public ITable GetTable(TextBox box)
        {
            switch (box.Name)
            {
                case "TextBoxCustomer":
                    return SelectionTablePersons.Create(
                        box, new SelectionType(PersonTypeColection.customerTuple),
                    dataSet.BD09_PERSON, dataSet.BD06_ORG_EINHEIT_TBL);

                case "TextBoxChief":
                    return SelectionTablePersons.Create(box, new SelectionType(PersonTypeColection.chiefTuple),
                    dataSet.BD09_PERSON, dataSet.BD06_ORG_EINHEIT_TBL);

                case "TextBoxEngineeringAST":
                    return SelectionTablePersons.Create(box, new SelectionType(PersonTypeColection.engineeringTuple),
                   dataSet.BD09_PERSON, dataSet.BD06_ORG_EINHEIT_TBL);

                case "TextBoxCustomerOE":
                    return SelectionTableDepartaments.Create(box, dataSet.BD06_ORG_EINHEIT_TBL,
                        new SelectionType(PersonTypeColection.customerOETuple));

                case "TextBoxSeries":
                    return SelectionTableSeries.Create(box, dataSet.BD12_BAUREIHE,
                        new SelectionType(PersonTypeColection.seriesTuple));

                default:
                    return null;
            }
        }
    }
}