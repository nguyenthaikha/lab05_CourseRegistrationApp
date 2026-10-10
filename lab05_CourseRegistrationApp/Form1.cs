using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab05_CourseRegistrationApp
{
    public partial class Form1 : Form
    {
        public class KhoaHoc
        {
            public string TenKhoaHoc { get; set; }
            public decimal HocPhi { get; set; }

            public KhoaHoc(string tenKhoaHoc, decimal hocPhi)
            {
                TenKhoaHoc = tenKhoaHoc;
                HocPhi = hocPhi;
            }
        }

       
        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;

            cboKhoaHoc.SelectedIndexChanged += cboKhoaHoc_SelectedIndexChanged;
            numSoThang.ValueChanged += numSoThang_ValueChanged;

            btnDangKi.Click += btnDangKy_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnThoat.Click += btnThoat_Click;
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            // Nạp danh sách 4 khóa học 
            List<KhoaHoc> danhSachKhoaHoc = new List<KhoaHoc>
            {
                new KhoaHoc("C# WinForms cơ bản", 800000),
                new KhoaHoc("SQL Server cơ bản", 700000),
                new KhoaHoc("Web Frontend cơ bản", 750000),
                new KhoaHoc("Lập trình Python cơ bản", 650000)
            };

            // Binding dữ liệu vào ComboBox
            cboKhoaHoc.DataSource = danhSachKhoaHoc;
            cboKhoaHoc.DisplayMember = "TenKhoaHoc";
            cboKhoaHoc.ValueMember = "HocPhi";

            // Chọn mặc định khóa học đầu tiên
            cboKhoaHoc.SelectedIndex = 0;

            // Chọn mặc định hình thức Online
            radOnline.Checked = true;

            // Thiết lập số tháng tối thiểu là 1, tối đa là 12
            numSoThang.Minimum = 1;
            numSoThang.Maximum = 12;
            numSoThang.Value = 1;

            // Hiển thị tổng học phí ban đầu
            TinhTongHocPhi();
        }

        private void TinhTongHocPhi()
        {
            if (cboKhoaHoc.SelectedItem != null)
            {
                KhoaHoc kh = (KhoaHoc)cboKhoaHoc.SelectedItem;
                int soThang = (int)numSoThang.Value;
                decimal tongTien = kh.HocPhi * soThang;
                lblTongTien.Text = string.Format("{0:#,##0} VNĐ", tongTien);
            }
        }

        // Tự động tính lại tổng tiền khi đổi khóa học
        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhTongHocPhi();
        }

        // Tự động tính lại tổng tiền khi đổi số tháng
        private void numSoThang_ValueChanged(object sender, EventArgs e)
        {
            TinhTongHocPhi();
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra họ tên không được rỗng
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên học viên!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 2. Kiểm tra số điện thoại không được rỗng
            string soDienThoai = txtSoDienThoai.Text.Trim();
            if (string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            // 3. Kiểm tra phải chọn khóa học
            if (cboKhoaHoc.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khóa học!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Lấy dữ liệu đã chọn từ giao diện
            KhoaHoc kh = (KhoaHoc)cboKhoaHoc.SelectedItem;
            string ngaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            string hinhThuc = radOnline.Checked ? "Online" : "Trực tiếp";
            int soThang = (int)numSoThang.Value;
            decimal tongTien = kh.HocPhi * soThang;
            string nhanEmail = chkNhanEmail.Checked ? "Có" : "Không";

            // 5. Hiển thị phiếu đăng ký bằng MessageBox
            string thongTinPhieu = $"--- PHIẾU ĐĂNG KÝ KHÓA HỌC ---\n\n" +
                                   $"Họ tên: {hoTen}\n" +
                                   $"Số điện thoại: {soDienThoai}\n" +
                                   $"Ngày sinh: {ngaySinh}\n" +
                                   $"Khóa học: {kh.TenKhoaHoc}\n" +
                                   $"Hình thức: {hinhThuc}\n" +
                                   $"Số tháng: {soThang}\n" +
                                   $"Tổng tiền: {tongTien:#,##0} VNĐ\n" +
                                   $"Nhận email: {nhanEmail}";

            MessageBox.Show(thongTinPhieu, "Thông báo đăng ký",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Xóa họ tên và số điện thoại
            txtHoTen.Clear();
            txtSoDienThoai.Clear();

            // Đưa ngày sinh về ngày hiện tại
            dtpNgaySinh.Value = DateTime.Now;

            // Bỏ chọn nhận email
            chkNhanEmail.Checked = false;

            // Chọn lại khóa học đầu tiên
            if (cboKhoaHoc.Items.Count > 0)
                cboKhoaHoc.SelectedIndex = 0;

            // Chọn lại hình thức Online
            radOnline.Checked = true;

            // Đưa số tháng về 1
            numSoThang.Value = 1;

            // Cập nhật lại học phí hiển thị
            TinhTongHocPhi();

            // Đưa con trỏ về ô họ tên
            txtHoTen.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Gọi hàm đóng Form để kích hoạt sự kiện FormClosing
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận trước khi thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?",
                                                  "Xác nhận thoát",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            // Chỉ đóng Form nếu người dùng chọn Yes
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy lệnh đóng Form
            }
        }
    }
}
