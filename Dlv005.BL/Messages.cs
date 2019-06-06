namespace Dlv005.BL
{
    public class Messages
    {
        /// <summary>
        /// Static messages for validations.
        /// </summary>
        public static readonly string incorectFormat = "The field has incorrect format! Please correct your entry.";

        /// <summary>
        /// The allocation100
        /// </summary>
        public static readonly string allocation100 = "The total of the account assignment shares must be 100%";

        /// <summary>
        /// The date in the past
        /// </summary>
        public static readonly string dateInThePast = "The date must be in the future!";

        /// <summary>
        /// To lower then from date
        /// </summary>
        public static readonly string toLowerThenFromDate = "Bis date must be greater or equal with von date!";

        /// <summary>
        /// The value not contained in selection table
        /// </summary>
        public static readonly string valueNotContainedInSelectionTable = "The value is not contained into the selection table. Please select a valid value.";

        /// <summary>
        /// The trash value ComboBox
        /// </summary>
        public static readonly string trashValueComboBox = "The value is incorrect.Please correct your entry.";

        /// <summary>
        /// The empty mandatory
        /// </summary>
        public static readonly string emptyMandatory = "The mandatory field does not contain any data. Please enter a value.";

        /// <summary>
        /// The delete check
        /// </summary>
        public static readonly string deleteCheck = "Should the external picking really be deleted? ";

        /// <summary>
        /// The save date if lower then current date
        /// </summary>
        public static readonly string saveDateIfLowerThenCurrentDate = "The date entered for starting picking is very short-term. Do you really want to save the data? ";
    }
}