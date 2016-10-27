using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nim
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var buttons = new List<Button>();
                splitContainer1.Panel2.Controls.Clear();

                var heaps = textBox1.Text.Trim().Split(' ');
                var table = new TableLayoutPanel();
                table.AutoSize = true;
                splitContainer1.Panel2.Controls.Add(table);
                table.RowCount = heaps.Length;
                var heapSizes = heaps.Select(s => int.Parse(s));
                table.ColumnCount = heapSizes.Max();

                heapSizes.
                    Zip(Enumerable.Range(0, heaps.Length), (n, i) => new { n, i }).
                    SelectMany(ni => Enumerable.Range(0, ni.n).
                    Select(m => Tuple.Create(m, ni.i))).ToList().ForEach(mi =>
                    {
                        var b = new Button();
                        b.Tag = mi;
                        b.Size = new Size(40, 30);
                        buttons.Add(b);
                        b.Click += (object o, EventArgs args) => buttons.Where(bb => ((Tuple<int, int>)bb.Tag).Item2 == mi.Item2 && ((Tuple<int, int>)bb.Tag).Item1 >= mi.Item1).ToList().ForEach(bbb => table.Controls.Remove(bbb));
                        table.Controls.Add(b, mi.Item1, mi.Item2);
                    });
                textBox1.BackColor = Color.Green;
            } catch
            {
                splitContainer1.Panel2.Controls.Clear();
                textBox1.BackColor = Color.Red;
            }
           
        }

        static void NextState()
        {

        }


        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
