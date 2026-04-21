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
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
            txtTenHang.Enabled = false;
            // cac texbox khac cung bi visible
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
            if (txtTenHang.Text.Trim().Length > 0)
            {
               // MessageBox.Show(txtAnh.Text.Trim());
              //  pictureBox.Image = Image.FromFile(txtAnh.Text);
            }
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
                string ten_chat_lieu = TenChatlieu.GetValue(0).ToString();
                TenChatlieu.Close();
                return ten_chat_lieu;
            }
            {
                TenChatlieu.Close() ;
                return ""; }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaHang.Enabled = true;
            txtTenHang.Enabled = true;
            txtDonGiaBan.Enabled = true;
            txtGhiChu.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            btnLuu.Enabled = true;
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // check xem du duwx lieu ko
            if (!CheckInputData())
            {
                return;
            }
            else 
            {
                // check Ma Hang co bi trung hay khong
                if (checkKeyExit())
                    return;
                else { 
                    // insert du lieu vao db
                    string sql = "Insert into tblHang (Mahang, tenHang, MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan)" +
                        "values('" + txtMaHang.Text + "' , N'"+ txtTenHang.Text + "', " +
                        "       '"+ cmbChatLieu.SelectedValue.ToString() + "', "+
                        txtSoLuong.Text + ", " + txtDonGiaNhap.Text + ", "+ txtDonGiaBan.Text + ")";
                    MessageBox.Show(sql);
                    try
                    {
                        SqlCommand mycmd = new SqlCommand(sql, DAO.Connection);
                        mycmd.ExecuteNonQuery();
                        MessageBox.Show("Luu du lieu thanh cong");
                        fillDataToGridView();
                    }
                    catch (Exception ex) { 
                        MessageBox.Show(ex.Message);
                    }
                
                }
            }
        }

        private bool CheckInputData()
        {
            bool result = true;
            // check ma hang hop le ko
            if (txtMaHang.Text.Trim() == "")
            {
                MessageBox.Show("Ban phai nhap ma hang");
                txtMaHang.Focus();
                result = false;
                return result;
            }
            if (txtTenHang.Text.Trim() == "")
            {
                MessageBox.Show("Ban phai nhap ten hang");
                txtTenHang.Focus();
                result = false;
                return result;
            }
            
            if (txtSoLuong.Text.Trim() == "")
            {
                MessageBox.Show("Ban Phai nhap so luong");
                txtSoLuong.Focus(); result = false; 
                return result;
            }
            try
            {
                int soluong = Convert.ToInt32(txtSoLuong.Text);
            }
            catch (Exception ex) { 
                MessageBox.Show("Ban nhap so luong khong phai so nguyen");
                txtSoLuong.Focus();
                result = false;
                return result;
            }
            return result;
        }

        private bool checkKeyExit() {
            string sql = "select count (*) from tblHang where mahang = @maHang";
            SqlCommand cmd = new SqlCommand(sql,DAO.Connection);

            cmd.Parameters.Add(new SqlParameter("@mahang", txtMaHang.Text.Trim()));
            int result = Convert.ToInt16(cmd.ExecuteScalar().ToString());
            if (result == 0) {
                return false;
            }
            else
            {
                MessageBox.Show("Ma hang da ton tai");
                return true;
            }

        }
    
    }
}
