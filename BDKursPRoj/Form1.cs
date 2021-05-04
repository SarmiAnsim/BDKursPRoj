using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDKursPRoj
{
    public partial class Form1 : Form
    {
        UZ Uzel = new UZ();
        public Form1()
        {
            InitializeComponent();
            for (int i = 11; i < dataGridView1.Columns.Count-1; i++)
            {
                listBox3.SetSelected(i, true);
            }
            listBox6.SetSelected(0, true);
            listBox9.SetSelected(0, true);
            listBox11.SetSelected(0, true);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex == 0)
            {
                numericUpDown5.Enabled = true;
                numericUpDown6.Enabled = true;
                numericUpDown7.Enabled = false;
            }
            if(listBox1.SelectedIndex == 1)
            {
                numericUpDown5.Enabled = false;
                numericUpDown6.Enabled = false;
                numericUpDown7.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(((double)numericUpDown7.Value != 0 || 
                (!numericUpDown7.Enabled && numericUpDown5.Value + numericUpDown6.Value <= 1)) && 
                (double)numericUpDown3.Value != 0 &&
                (int)numericUpDown1.Value != 0 && (int)numericUpDown2.Value != 0 &&
                listBox1.SelectedItem != null && listBox2.SelectedItem != null
                )
            { 
                Uzel.Add(
                    (double)numericUpDown3.Value,
                    listBox1.SelectedIndex,
                    listBox2.SelectedIndex,
                    (int)numericUpDown1.Value,
                    (int)numericUpDown2.Value,
                    (double)numericUpDown4.Value,
                    (double)numericUpDown5.Value,
                    (double)numericUpDown6.Value,
                    (double)numericUpDown7.Value
                    );
                ChangeTabel();
            } 
            else
                MessageBox.Show(
                "Не все вводные данные валидны",
                "Сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }
        void ChangeTabel()
        {
            dataGridView1.Rows.Clear();
            foreach (SE tmp in Uzel
                .GetSortFiltSEs(listBox4.SelectedIndex, listBox5.SelectedIndex,
                listBox6.SelectedIndex, listBox7.SelectedIndex, (double)numericUpDown9.Value,
                listBox9.SelectedIndex, listBox8.SelectedIndex, (double)numericUpDown10.Value,
                listBox11.SelectedIndex, listBox10.SelectedIndex, (double)numericUpDown11.Value
                ))
                dataGridView1.Rows.Add(
                    tmp.CodeSE,
                    tmp.det.a,
                    tmp.det.at,
                    tmp.det.atw,
                    tmp.X,
                    tmp.det.u,
                    tmp.det.dw1,
                    tmp.det.dw2,
                    tmp.y,
                    tmp.Δy,
                    tmp.det.df1,
                    tmp.det.df2,
                    tmp.det.x1,
                    tmp.det.x2,
                    tmp.det.aw,
                    tmp.det.d1,
                    tmp.det.d2,
                    tmp.det.da1,
                    tmp.det.da2
                    );
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }
            foreach(int i in listBox3.SelectedIndices)
            {
                dataGridView1.Columns[i+1].Visible = true;
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                    numericUpDown8.Value = i;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if((int)numericUpDown8.Value < dataGridView1.Rows.Count)
            { 
                Uzel.Delete((int)numericUpDown8.Value);
                ChangeTabel();
            }
            else
                MessageBox.Show(
                "Такой строки не существует",
                "Сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if ((int)numericUpDown8.Value < dataGridView1.Rows.Count)
            {
                Uzel.Change(
                (int)numericUpDown8.Value,
                (double)numericUpDown3.Value,
                listBox1.SelectedIndex,
                listBox2.SelectedIndex,
                (int)numericUpDown1.Value,
                (int)numericUpDown2.Value,
                (double)numericUpDown4.Value,
                (double)numericUpDown5.Value,
                (double)numericUpDown6.Value,
                (double)numericUpDown7.Value
                );
                ChangeTabel();
            }
            else
                MessageBox.Show(
                "Такой строки не существует",
                "Сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        #region Сортировка и фильтры
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            ChangeTabel();
        }
        #endregion
    }
}
