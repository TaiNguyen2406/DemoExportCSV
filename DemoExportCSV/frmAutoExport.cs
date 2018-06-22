using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExportCSV
{
    public partial class frmAutoExport : Form
    {
        public frmAutoExport()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgv.DataSource = TaoBang();
            dgv.Columns[0].Width = 80;
            dgv.Columns[1].Width = 80;
            for (int i=2;i<dgv.ColumnCount -1;i++)
            {
                dgv.Columns[i].Width = 40;
            }
        }
        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        private DataTable TaoBang()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(String));
            dt.Columns.Add("Time", typeof(String));
            for (int i = 0; i < 4; i++)
            {
                dt.Columns.Add("nhietdo" + i, typeof(Decimal));
            }
            for (int i = 4; i <= 6; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    dt.Columns.Add(i + "L" + j, typeof(Decimal));
                }
                for (int j = 1; j <= 20; j++)
                {
                    dt.Columns.Add(i + "R" + j, typeof(Decimal));
                }
            }
            dt.Rows.Add(dt.NewRow());
            dt.Rows[0]["Date"] = DateTime.Now.ToString("dd/MM/yyyy");
            dt.Rows[0]["Time"] = DateTime.Now.ToString("HH:mm:ss");
            for (int i = 2; i <= dt.Columns.Count -1; i++)
            {
                Random rnd = new Random();
                if (i<=5)
                {
                    dt.Rows[0][i] = random.Value.Next(1000, 10000);
                }
                else
                {
                    dt.Rows[0][i] = random.Value.Next(0, 500);
                }
               
            }
                return dt;
        }
        public void Export(DataTable dt)
        {

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));

            }
            //modify
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV|*.csv|All file|*.*";
            save.FilterIndex = 1;
            DateTime hientai = DateTime.Now;
            Directory.CreateDirectory("D:/csv/" + hientai.Month.ToString());
            File.WriteAllText("D:/csv/"+ hientai.Month.ToString()+"/"+ hientai.ToString("dd-MM-yyyy HHmmss")+".csv", sb.ToString());
            for(int i=1;i<=12;i++)
            {
                if (i!= hientai.Month)
                {
                    string filePath = "D:/csv/" + i.ToString();
                    if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath, true);
                    }
                    
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = TaoBang();
            dgv.DataSource = dt;
            Export(dt);
        }
    }
}
