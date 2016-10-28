using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Immutable;

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
                var buttons = new List<Tuple<Tuple<int,int>,Button>>();
                splitContainer1.Panel2.Controls.Clear();

                var heaps = textBox1.Text.Trim().Split(' ').Select(stringHeapSize => int.Parse(stringHeapSize)).Where(heapSize => heapSize > 0);

                var table = new TableLayoutPanel();
                table.AutoSize = true;
                table.RowCount = heaps.Count();
                table.ColumnCount = heaps.Max();
                splitContainer1.Panel2.Controls.Add(table);

                Action<Tuple<int, int>> removeButtons = tp => buttons.Where(tpb => tpb.Item1.Item2 == tp.Item2 && tpb.Item1.Item1 >= tp.Item1).ToList().
                    ForEach(_tpb => table.Controls.Remove(_tpb.Item2));
                Action<IEnumerable<int>> renderState = state => state.Zip(Enumerable.Range(0, state.Count()), (n, i) => Tuple.Create(n, i)).ToList().
                    ForEach(tp => removeButtons(tp));
                Func<ImmutableList<int>> currentState = () => buttons.GroupBy(tpb => tpb.Item1.Item2).
                                    Select(tpbs => tpbs.Where(tpb => table.Controls.Contains(tpb.Item2)).Count()).ToImmutableList();
                Func<ImmutableList<int>, bool> winningState = state => state.All(n => n == 0);
                Func<bool, string, bool> isGameOver = (won, who) =>
                {
                    if (won)
                    {
                        splitContainer1.Panel2.Controls.Clear();
                        var l = new Label();
                        l.Text = who + " " + "won!";
                        splitContainer1.Panel2.Controls.Add(l);
                    }
                    return won;
                };

                heaps.
                    Zip(Enumerable.Range(0, heaps.Count()), (n, i) => new { n, i }).
                    SelectMany(ni => Enumerable.Range(0, ni.n).
                    Select(m => Tuple.Create(m, ni.i))).ToList().ForEach(mi =>
                    {
                        var b = new Button();
                        b.Size = new Size(40, 30);
                        buttons.Add(Tuple.Create(mi, b));
                        b.Click += (object o, EventArgs args) =>
                        {
                            removeButtons(mi);
                            var cs = currentState();
                            if (!isGameOver(winningState(cs), "you"))
                            {
                                renderState(Algorithm.NextState(cs));
                                isGameOver(winningState(currentState()), "computer");
                            }

                                
                        };
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
