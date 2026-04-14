using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    internal class DAO
    {
        public static SqlConnection Connection { get; set; }
        public static string ConnectionString = "Data Source=localhost\\SQLEXPRESS;" +
            "Initial Catalog=QuanLyBanHang;" +
                "Integrated Security=True;";
        public static void Connect()
        {
            Connection = new SqlConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            catch (Exception ex){
                MessageBox.Show("Lỗi kết nối csdl:" + ex.ToString());
            }
        }
        public static void Disconnect()
        {
            try
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.ToString());
            }
        }

    }
}
