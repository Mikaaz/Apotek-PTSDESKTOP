using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGVPrinterHelper;

namespace DesktopApotekPTS
{
    public partial class Form2 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-SU2UKK05\SQLEXPRESS02;Initial Catalog=ApotekPTS;Integrated Security=True");

        public Form2()
        {
            InitializeComponent();
        }

        string ImageLocation = "";
        SqlCommand cmd;

        int JenisBarang = 0;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int hargaItem = int.Parse(comboBox2.Text);
            int item = int.Parse(numericUpDown1.Value.ToString());
            int total = hargaItem * item;
            textBox4.Text = total.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [tb_obat] (kodebarang,namabarang,dosis,hargabarang,jumlahbarang,resep) values ('" + textBox5.Text + "', '" + comboBox3.Text + "', '" + comboBox1.Text + "', '" + comboBox2.Text + "', '" + numericUpDown1.Text + "', '" + JenisBarang + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Berhasil Ter-Input!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            comboBox3.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            numericUpDown1.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            imboost.Visible = false;
            betadin.Visible = false;
            mefenamic.Visible = false;
            pictureBox4.ImageLocation = null;
        }

        public void display_data()
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [tb_obat]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Apakah kamu yakin ingin menghapus data?", "Warning", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from [tb_obat] where kodebarang = '" + textBox5.Text + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
                textBox1.Text = "";
                display_data();
                MessageBox.Show("Berhasil Ter-Delete!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [tb_obat] set kodebarang = '" + this.textBox5.Text + "', namabarang = '" + this.comboBox3.Text + "' , dosis = '" + this.comboBox1.Text + "' , hargabarang = '" + this.comboBox2.Text + "' , jumlahbarang = '" + this.numericUpDown1.Text + "', resep = '" + this.JenisBarang + "' where kodebarang = '" + textBox5.Text + "' ";
            cmd.ExecuteNonQuery();
            conn.Close();
            display_data();
            MessageBox.Show("Data Berhasil ter-Update!");
            ;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            display_data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Data Obat";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("dddd-MMMM-yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Center;
            printer.Footer = "Mikhail Haqeen";
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView1);
            dataGridView1.Columns[1].Width = 108;
            dataGridView1.Columns[2].Width =
                dataGridView1.Width
                - dataGridView1.Columns[0].Width
                - dataGridView1.Columns[1].Width - 92;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string barCode = textBox4.Text;
            try
            {
                Zen.Barcode.Code128BarcodeDraw brCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                pictureBox4.Image = brCode.Draw(barCode, 40);
            }
            catch (Exception)
            {

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [tb_obat] where namabarang='" + textBox1.Text + "'";
            DataTable dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            textBox4.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Warning!!!", "Yakin Ingin Keluar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            JenisBarang = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            JenisBarang = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Imboost")
            {
                imboost.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add(50000);
                comboBox2.Text = comboBox2.Items[0].ToString();
                betadin.Visible = false;
                mefenamic.Visible = false;
            }
            else if (comboBox3.Text == "Betadin")
            {
                betadin.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add(30000);
                comboBox2.Text = comboBox2.Items[0].ToString();
                mefenamic.Visible = false;
                imboost.Visible = false;
            }
            else if (comboBox3.Text == "Mefenamic")
            {
                mefenamic.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add(90000);
                comboBox2.Text = comboBox2.Items[0].ToString();
                imboost.Visible = false;
                betadin.Visible = false;
            }
            else
            {
                mefenamic.Visible = false;
                imboost.Visible = false;
                betadin.Visible = false;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
