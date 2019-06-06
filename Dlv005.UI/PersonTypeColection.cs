using System;

namespace Dlv005.UI
{
    /// <summary>
    /// The tuple that are set in this class are sets for selection table like this --> Check SelectionType Class
    /// </summary>
    internal static class PersonTypeColection
    {
        /// <summary>
        /// The customer tuple
        /// </summary>
        public static readonly Tuple<string, string, string> customerTuple = Tuple.Create("Customer", "CustomerID", "Customer");

        /// <summary>
        /// The customer oe tuple
        /// </summary>
        public static readonly Tuple<string, string, string> customerOETuple = Tuple.Create("CustomerOE", "CustomerOEID", "CustomerOE");

        /// <summary>
        /// The chief tuple
        /// </summary>
        public static readonly Tuple<string, string, string> chiefTuple = Tuple.Create("Chief", "ChiefID", "Chief");

        /// <summary>
        /// The series tuple
        /// </summary>
        public static readonly Tuple<string, string, string> seriesTuple = Tuple.Create("Series", "Series", "Series");

        /// <summary>
        /// The engineering tuple
        /// </summary>
        public static readonly Tuple<string, string, string> engineeringTuple = Tuple.Create("Engineering", "EngineeringID", "Engineering");
    }
}