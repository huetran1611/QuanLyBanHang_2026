using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class frmHang : Form
    {
        public frmHang()
        {
            InitializeComponent();
        }

        private void frmHang_Load(object sender, EventArgs e)
        {
            fillDataToCombo();
            fillDataToGridView();
        }

        private void fillDataToCombo()
        {
            string sql = "select * from tblChatLieu";
            SqlDataAdapter ChatLieuAdapter = new SqlDataAdapter(sql, DAO.Connection);
            DataTable ChatLieuTable = new DataTable();
            ChatLieuAdapter.Fill(ChatLieuTable);
            cmbChatLieu.DataSource = ChatLieuTable;
            cmbChatLieu.ValueMember = "Machatlieu";
            cmbChatLieu.DisplayMember = "TenChatLieu";
        }
        private void fillDataToGridView()// lây dữ liệu của tblHang đổ vào datagridviewHang
        {
            string sql = "select * from tblHang";
            SqlDataAdapter HangAdapter = new SqlDataAdapter(sql, DAO.Connection);
            DataTable HangTable = new DataTable();
            HangAdapter.Fill(HangTable);
            dataGridViewHang.DataSource = HangTable;
        }

        private void dataGridViewHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaHang.Text = dataGridViewHang.CurrentRow.Cells[0].Value.ToString();
            txtTenHang.Text = dataGridViewHang.CurrentRow.Cells["TenHang"].Value.ToString();
            string macl = dataGridViewHang.CurrentRow.Cells["Machatlieu"].Value.ToString();
            cmbChatLieu.Text = GetTenChatLieuFromMa(macl);
            txtSoLuong.Text = dataGridViewHang.CurrentRow.Cells["Soluong"].Value.ToString();
            txtDonGiaBan.Text = dataGridViewHang.CurrentRow.Cells["Dongianhap"].Value.ToString();
            txtDonGiaBan.Text = dataGridViewHang.CurrentRow.Cells["Dongiaban"].Value.ToString();
            txtAnh.Text = dataGridViewHang.CurrentRow.Cells["Anh"].Value.ToString();
            //MessageBox.Show(txtAnh.Text.Trim());
            pictureBox.Image = Image.FromFile (txtAnh.Text);
            txtGhiChu.Text = dataGridViewHang.CurrentRow.Cells["Ghichu"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;

        }
        private string GetTenChatLieuFromMa(string ma) // sử dụng để lấy tên chất liệu theo mã chất liệu
        {
            string sql = "Select Tenchatlieu from tblChatlieu where Machatlieu = '" + ma + "'";
            SqlCommand mycmd = new SqlCommand(sql, DAO.Connection);
            SqlDataReader TenChatlieu = mycmd.ExecuteReader();
            while (TenChatlieu.Read())
            {
                return TenChatlieu.GetValue(0).ToString();
            }
            { return ""; }
        }
    }
}
