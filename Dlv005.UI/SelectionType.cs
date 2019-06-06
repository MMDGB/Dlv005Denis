using System;

namespace Dlv005.UI
{
    public class SelectionType
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the insertion identifier.
        /// </summary>
        /// <value>
        /// The insertion identifier.
        /// </value>
        public string InsertionID { get; }

        /// <summary>
        /// Gets the insertion text.
        /// </summary>
        /// <value>
        /// The insertion text.
        /// </value>
        public string InsertionText { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public SelectionType(Tuple<string, string, string> type)
        {
            Name = type.Item1;
            InsertionID = type.Item2;
            InsertionText = type.Item3;
        }
    }
}