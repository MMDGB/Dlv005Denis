namespace Dlv005.UI
{
    internal class Filters
    {
        /// <summary>
        /// Static filters for overview hide checkboxes.
        /// </summary>
        public static readonly string confirmedCondition = "StatusText <> 'Confirmed'";

        /// <summary>
        /// The requested condition
        /// </summary>
        public static readonly string requestedCondition = "StatusText <> 'Requested'";

        /// <summary>
        /// The confirmed and requested condition
        /// </summary>
        public static readonly string confirmedAndRequestedCondition = "StatusText <> 'Confirmed' and StatusText <> 'Requested'";

        /// <summary>
        /// The licence filter
        /// </summary>
        public static readonly string licenceFilter = "SD111_TYP = 'FAHRBERECHTIGUNG3'";

        /// <summary>
        /// The hv filter
        /// </summary>
        public static readonly string hvFilter = "SD111_TYP = 'HVQUALIFIKATION'";

        /// <summary>
        /// The special filter
        /// </summary>
        public static readonly string specialFilter = "SD111_TYP = 'SONDQUALIFIKATION'";
    }
}