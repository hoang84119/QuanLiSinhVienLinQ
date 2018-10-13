using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class XL_BANG : DataTable
    {
        #region Bien cuc bo
        public string chuoiKetNoi = @"Data Source=SANG-LAPTOP\SQLEXPRESS;Initial Catalog=QLSINHVIEN4;Integrated Security=True";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private string chuoiSQL;
        private string tenBang;
        #endregion
        #region Thuoc tinh
        public string ChuoiSQL { get => chuoiSQL; set => chuoiSQL = value; }
        public string TenBang { get => tenBang; set => tenBang = value; }
        public int SoDong { get => this.DefaultView.Count; }
        #endregion
        #region Phuong thuc khoi tao
        public XL_BANG() : base()
        {
        }

        public XL_BANG(string tenBang)
        {
            this.tenBang = tenBang;
            DocBang();
        }

        public XL_BANG(string tenBang, string chuoiSQL)
        {
            this.tenBang = tenBang;
            this.chuoiSQL = chuoiSQL;
            DocBang();
        }
        #endregion
        #region Cac phuong thuc xu ly: doc, ghi, loc du lieu
        public void DocBang()
        {
            if(chuoiSQL == null)
            {
                chuoiSQL = "SELECT * from " + tenBang;
            }
            if(connection == null)
            {
                connection = new SqlConnection(chuoiKetNoi);
            }
            try
            {
                adapter = new SqlDataAdapter(chuoiSQL, connection);
                adapter.FillSchema(this, SchemaType.Mapped);
                adapter.Fill(this);
                adapter.RowUpdated += new SqlRowUpdatedEventHandler(DARowUpdated);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Boolean GhiBang()
        {
            Boolean ketQua = true;
            try
            {
                adapter.Update(this);
                this.AcceptChanges();
            }catch(SqlException ex)
            {
                this.RejectChanges();
                ketQua = false;
                throw ex;
            }
            return ketQua;
        }

        public void LocDuLieu (string dieuKienLoc)
        {
            try
            {
                this.DefaultView.RowFilter = dieuKienLoc;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Cac phuong thuc thuc hien lenh

        #endregion
        #region Xu ly su kien
        private void DARowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if(this.PrimaryKey[0].AutoIncrement)
            {
                if((e.Status == UpdateStatus.Continue)&&(e.StatementType == StatementType.Insert))
                {
                    SqlCommand cmd = new SqlCommand("Select @@IDENTITY", connection);
                    e.Row.ItemArray[0] = cmd.ExecuteScalar();
                    e.Row.AcceptChanges();
                }
            }
        }
        #endregion
    }
}
