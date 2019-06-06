using System;
using System.IO;
using System.Windows.Forms;

namespace Dlv005.UI
{
    internal class Observer : IObservable
    {
        /// <summary>
        /// Sets the observer for event.
        /// </summary>
        /// <returns></returns>
        public EventHandler SetObserverForEvent()
        {
            return new EventHandler(WriteToFileAfterSave);
        }

        /// <summary>
        /// Writes to file after save.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WriteToFileAfterSave(object sender, EventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                File.AppendAllText("C:/Users/denis.marchis/Desktop/Path.txt", ((Button)sender).Tag.ToString());
                File.AppendAllText("C:/Users/denis.marchis/Desktop/Path.txt", "\n");
            }
        }
    }
}