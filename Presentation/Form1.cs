using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;

namespace Presentation
{
    public partial class Form1 : Form
    {
        #region Bien toan cuc
        string path = "../../Hinh";
        static QLSinhVienBDDataContext db = new QLSinhVienBDDataContext();
        Table<SINHVIEN> BangSinhVien = db.SINHVIENs;
        Table<LOP> BangLop = db.LOPs;
        BindingManagerBase DSSV;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //BangSinhVien = new XL_SINHVIEN();
            //BangLop = new XL_LOP();

            LoadCBLop();
            LoadGVSinhVien();

            txtMaSV.DataBindings.Add("text", BangSinhVien, "MaSV", true);
            txtHoTen.DataBindings.Add("text", BangSinhVien, "HoTen", true);
            dateTimePickerNgaySinh.DataBindings.Add("text", BangSinhVien, "NgaySinh", true);
            radNam.DataBindings.Add("checked", BangSinhVien, "GioiTinh", true);
            cbbLop.DataBindings.Add("SelectedValue", BangSinhVien, "MaLop", true);
            txtDiaChi.DataBindings.Add("text", BangSinhVien, "DiaChi", true);
            pictureBoxHinh.DataBindings.Add("ImageLocation", BangSinhVien, "Hinh", true);
            DSSV = this.BindingContext[BangSinhVien];
            BatNutLenh(false);
        }

        private void BatNutLenh(bool isEnabled)
        {
            btnThem.Enabled = !isEnabled;
            btnXoa.Enabled = !isEnabled;
            btnSua.Enabled = !isEnabled;
            btnThoat.Enabled = !isEnabled;
            btnLuu.Enabled = isEnabled;
            btnHuy.Enabled = isEnabled;
        }

        private void LoadGVSinhVien()
        {
            gvSinhVien.AutoGenerateColumns = false;
            gvSinhVien.DataSource = BangSinhVien;
        }

        private void LoadCBLop()
        {
            cbbLop.DataSource = BangLop;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";
        }

        private void radNam_CheckedChanged(object sender, EventArgs e)
        {
            radNu.Checked = !radNam.Checked;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DSSV.AddNew();
            BatNutLenh(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                db.SubmitChanges();
                BatNutLenh(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.RemoveAt(DSSV.Position);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DSSV.CancelCurrentEdit();
            ChangeSet cs = db.GetChangeSet();
            db.Refresh(RefreshMode.OverwriteCurrentValues, cs.Updates);
            BatNutLenh(false);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //BangSinhVien.LocDuLieu("HoTen LIKE '%" + txtTimKiem.Text + "%'");
            DSSV.Position = BangSinhVien.ToList().FindIndex(sv => sv.MaSV.Contains(txtTimKiem.Text));
        }

        private void txtTimKiem_MouseDown(object sender, MouseEventArgs e)
        {
            txtTimKiem.Text = "";
        }

        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            openFileDialogHinh.Filter = "JPG File|*.jpg|PNG File|*.png|All File|*.*";
            if(openFileDialogHinh.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialogHinh.SafeFileName;
                string pathFile = path + "/" + fileName;
                if (!File.Exists(pathFile)) File.Copy(openFileDialogHinh.FileName, pathFile);
                pictureBoxHinh.ImageLocation = pathFile;
            }
        }
    }
}
