using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GörselProje
{
    internal class sıralama
    {
        public static void Azsıralama(ListBox listBox)
        {
            var sortedItems = listBox.Items.Cast<string>().OrderBy(item => item).ToList();
            listBox.Items.Clear();
            foreach (var item in sortedItems)
            {
                listBox.Items.Add(item);
            }
        }
    }
}
