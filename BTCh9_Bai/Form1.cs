
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using BTCh9_Bai3.Data;
using BTCh9_Bai3.Entities;
namespace BTCh9_Bai3
{
    public partial class Form1 : Form
    {
        private sealed class SinhVien
        {
            public string MSSV { get; set; } = "";
            public string HoTen { get; set; } = "";
            public DateTime NgaySinh { get; set; }
            public string GioiTinh { get; set; } = "";
            public string Lop { get; set; } = "";
        }

        private sealed class MonHoc
        {
            public string TenMon { get; set; } = "";
            public int TinChi { get; set; }
            public decimal Diem { get; set; }
        }

        private readonly List<SinhVien> dsSinhVien = new();
        private readonly Dictionary<string, List<MonHoc>>
            bangDiem = new();

        private readonly PrintDocument printDocument = new();
        private readonly PrintPreviewDialog previewDialog = new();
        private readonly PageSetupDialog pageSetupDialog = new();

        private string? maDangChon;
        private bool dangNap;

        public Form1()
        {
            InitializeComponent();

            // Khong co du lieu mau
            lstLop.Items.Add("Tất cả");
            lstLop.SelectedIndex = 0;
            radNam.Checked = true;

            GanSuKien();
            CapNhatDanhSach();

            printDocument.PrintPage += InPhieu_PrintPage;
            previewDialog.Document = printDocument;
            pageSetupDialog.Document = printDocument;
        }

        // ==================================
        // 1. GAN SU KIEN
        // ==================================

        private void GanSuKien()
        {
            lstLop.SelectedIndexChanged +=
                (_, _) => CapNhatDanhSach();

            txtTimKiem.TextChanged +=
                (_, _) => CapNhatDanhSach();

            dgvSinhVien.SelectionChanged +=
                (_, _) => ChonSinhVien();

            dgvDiem.SelectionChanged +=
                (_, _) => ChonMonHoc();

            btnThemSV.Click +=
                (_, _) => ThemSinhVien();

            btnSuaSV.Click +=
                (_, _) => SuaSinhVien();

            btnXoaSV.Click +=
                (_, _) => XoaSinhVien();

            btnThemMon.Click +=
                (_, _) => ThemMon();

            btnSuaDiem.Click +=
                (_, _) => SuaMon();

            btnXoaMon.Click +=
                (_, _) => XoaMon();

            btnXemTruoc.Click +=
                (_, _) => XemTruoc();

            btnIn.Click +=
                (_, _) => In();

            btnCaiDat.Click +=
                (_, _) => pageSetupDialog.ShowDialog(this);

            btnXuat.Click +=
                (_, _) => XuatCsv();
        }

        // ==================================
        // 2. CAP NHAT DANH SACH LOP
        // ==================================

        private void CapNhatLop(string? lopMuonChon = null)
        {
            var lopCu = lopMuonChon ??
                lstLop.SelectedItem?.ToString() ??
                "Tất cả";

            var cacLop = dsSinhVien
                .Select(s => s.Lop)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s)
                .ToList();

            dangNap = true;

            lstLop.Items.Clear();
            lstLop.Items.Add("Tất cả");

            foreach (var lop in cacLop)
                lstLop.Items.Add(lop);

            lstLop.SelectedItem =
                lstLop.Items.Cast<string>()
                .FirstOrDefault(x => x == lopCu)
                ?? "Tất cả";

            cboLop.Items.Clear();

            foreach (var lop in cacLop)
                cboLop.Items.Add(lop);

            dangNap = false;

            CapNhatDanhSach();
        }

        // ==================================
        // 3. TIM KIEM SINH VIEN
        // ==================================

        private IEnumerable<SinhVien> SinhVienDaLoc()
        {
            var lop = lstLop.SelectedItem?
                .ToString() ?? "Tất cả";

            var tuKhoa = txtTimKiem.Text.Trim();

            return dsSinhVien.Where(s =>
                (lop == "Tất cả" || s.Lop == lop)
                &&
                (
                    tuKhoa.Length == 0 ||
                    s.MSSV.Contains(
                        tuKhoa,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    s.HoTen.Contains(
                        tuKhoa,
                        StringComparison.CurrentCultureIgnoreCase)
                ));
        }

        private void CapNhatDanhSach(
            string? maCanChon = null)
        {
            if (dangNap) return;

            dangNap = true;
            dgvSinhVien.Rows.Clear();

            foreach (var s in SinhVienDaLoc())
            {
                dgvSinhVien.Rows.Add(
                    s.MSSV,
                    s.HoTen,
                    s.NgaySinh.ToString("dd/MM/yyyy"),
                    s.GioiTinh,
                    s.Lop);
            }

            dgvSinhVien.ClearSelection();

            var dong = dgvSinhVien.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r =>
                    (string?)r.Cells[0].Value ==
                    (maCanChon ?? maDangChon));

            if (dong == null &&
                dgvSinhVien.Rows.Count > 0)
            {
                dong = dgvSinhVien.Rows[0];
            }

            if (dong != null)
                dong.Selected = true;

            dangNap = false;
            ChonSinhVien();
        }

        // ==================================
        // 4. CHON SINH VIEN
        // ==================================

        private void ChonSinhVien()
        {
            if (dangNap) return;

            maDangChon =
                dgvSinhVien.SelectedRows.Count == 0
                ? null
                : dgvSinhVien.SelectedRows[0]
                    .Cells[0].Value?.ToString();

            var s = dsSinhVien.FirstOrDefault(
                x => x.MSSV == maDangChon);

            if (s != null)
            {
                txtMSSV.Text = s.MSSV;
                txtHoTen.Text = s.HoTen;
                dtpNgaySinh.Value = s.NgaySinh;

                radNam.Checked =
                    s.GioiTinh == "Nam";

                radNu.Checked =
                    s.GioiTinh == "Nữ";

                cboLop.Text = s.Lop;
            }
            else
            {
                txtMSSV.Clear();
                txtHoTen.Clear();
                cboLop.Text = "";
                txtTenMon.Clear();
            }

            CapNhatDiem();
        }

        // ==================================
        // 5. LAY THONG TIN SINH VIEN
        // ==================================

        private SinhVien? LayThongTin()
        {
            string ma = txtMSSV.Text.Trim();
            string ten = txtHoTen.Text.Trim();
            string lop = cboLop.Text.Trim();

            if (ma.Length == 0 ||
                ten.Length == 0 ||
                lop.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhập MSSV, họ tên và lớp.",
                    "Thiếu thông tin");

                return null;
            }

            if (lop.Equals(
                "Tất cả",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Tên lớp không được là 'Tất cả'.");

                return null;
            }

            return new SinhVien
            {
                MSSV = ma,
                HoTen = ten,
                Lop = lop,
                NgaySinh = dtpNgaySinh.Value.Date,
                GioiTinh =
                    radNam.Checked ? "Nam" : "Nữ"
            };
        }

        // ==================================
        // 6. THEM SINH VIEN
        // ==================================

        private void ThemSinhVien()
        {
            var s = LayThongTin();

            if (s == null) return;

            if (dsSinhVien.Any(x =>
                x.MSSV.Equals(
                    s.MSSV,
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("MSSV đã tồn tại.");
                return;
            }

            dsSinhVien.Add(s);

            bangDiem[s.MSSV] = new List<MonHoc>();

            txtTimKiem.Clear();

            CapNhatLop("Tất cả");
            CapNhatDanhSach(s.MSSV);
        }

        // ==================================
        // 7. SUA SINH VIEN
        // ==================================

        private void SuaSinhVien()
        {
            if (maDangChon == null)
            {
                MessageBox.Show(
                    "Chọn sinh viên cần sửa.");
                return;
            }

            var s = LayThongTin();
            if (s == null) return;

            if (s.MSSV != maDangChon &&
                dsSinhVien.Any(x =>
                    x.MSSV.Equals(
                        s.MSSV,
                        StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "MSSV mới đã tồn tại.");
                return;
            }

            var viTri = dsSinhVien.FindIndex(
                x => x.MSSV == maDangChon);

            if (viTri < 0) return;

            dsSinhVien[viTri] = s;

            if (s.MSSV != maDangChon)
            {
                bangDiem[s.MSSV] =
                    bangDiem.TryGetValue(
                        maDangChon, out var diem)
                    ? diem
                    : new List<MonHoc>();

                bangDiem.Remove(maDangChon);
            }

            txtTimKiem.Clear();
            CapNhatLop("Tất cả");
            CapNhatDanhSach(s.MSSV);
        }

        // ==================================
        // 8. XOA SINH VIEN
        // ==================================

        private void XoaSinhVien()
        {
            if (maDangChon == null)
            {
                MessageBox.Show(
                    "Chọn sinh viên cần xóa.");
                return;
            }

            var result = MessageBox.Show(
                "Xóa sinh viên và toàn bộ điểm?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            dsSinhVien.RemoveAll(
                x => x.MSSV == maDangChon);

            bangDiem.Remove(maDangChon);

            maDangChon = null;
            CapNhatLop("Tất cả");
        }

        // ==================================
        // 9. CAP NHAT BANG DIEM
        // ==================================

        private void CapNhatDiem()
        {
            dangNap = true;
            dgvDiem.Rows.Clear();

            var dsMon =
                maDangChon != null &&
                bangDiem.TryGetValue(
                    maDangChon, out var ds)
                ? ds
                : new List<MonHoc>();

            for (int i = 0; i < dsMon.Count; i++)
            {
                dgvDiem.Rows.Add(
                    i + 1,
                    dsMon[i].TenMon,
                    dsMon[i].TinChi,
                    dsMon[i].Diem.ToString(
                        "0.0",
                        CultureInfo.GetCultureInfo("vi-VN")));
            }

            dgvDiem.ClearSelection();

            if (dgvDiem.Rows.Count > 0)
                dgvDiem.Rows[0].Selected = true;

            int tc = dsMon.Sum(m => m.TinChi);

            decimal tb = tc == 0
                ? 0
                : dsMon.Sum(
                    m => m.TinChi * m.Diem) / tc;

            lblTongTC.Text =
                $"Tổng tín chỉ: {tc}";

            lblDiemTB.Text = tc == 0
                ? "Điểm TB: —"
                : $"Điểm TB: {tb:0.00}";

            string xepLoai =
                tc == 0 ? "—" :
                tb >= 9 ? "Xuất sắc" :
                tb >= 8 ? "Giỏi" :
                tb >= 7 ? "Khá" :
                tb >= 5 ? "Trung bình" :
                "Yếu";

            lblXepLoai.Text =
                "Xếp loại: " + xepLoai;

            dangNap = false;
            ChonMonHoc();
        }

        // ==================================
        // 10. CHON MON HOC
        // ==================================

        private void ChonMonHoc()
        {
            if (dangNap ||
                maDangChon == null ||
                dgvDiem.SelectedRows.Count == 0)
                return;

            int i = dgvDiem.SelectedRows[0].Index;

            if (!bangDiem.TryGetValue(
                maDangChon, out var ds) ||
                i >= ds.Count)
                return;

            txtTenMon.Text = ds[i].TenMon;
            nudTinChi.Value = ds[i].TinChi;
            nudDiem.Value = ds[i].Diem;
        }

        private MonHoc? LayMonHoc()
        {
            if (maDangChon == null)
            {
                MessageBox.Show(
                    "Chọn sinh viên trước.");
                return null;
            }

            var ten = txtTenMon.Text.Trim();

            if (ten.Length == 0)
            {
                MessageBox.Show(
                    "Nhập tên môn học.");
                return null;
            }

            return new MonHoc
            {
                TenMon = ten,
                TinChi = (int)nudTinChi.Value,
                Diem = nudDiem.Value
            };
        }

        // ==================================
        // 11. THEM MON HOC
        // ==================================

        private void ThemMon()
        {
            var mon = LayMonHoc();

            if (mon == null || maDangChon == null)
                return;

            if (bangDiem[maDangChon].Any(x =>
                x.TenMon.Equals(
                    mon.TenMon,
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "Môn học đã tồn tại.");
                return;
            }

            bangDiem[maDangChon].Add(mon);
            CapNhatDiem();
        }

        // ==================================
        // 12. SUA DIEM
        // ==================================

        private void SuaMon()
        {
            if (maDangChon == null ||
                dgvDiem.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Chọn môn học cần sửa.");
                return;
            }

            var mon = LayMonHoc();
            if (mon == null) return;

            int i = dgvDiem.SelectedRows[0].Index;

            if (bangDiem[maDangChon]
                .Where((_, index) => index != i)
                .Any(x => x.TenMon.Equals(
                    mon.TenMon,
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "Môn học đã tồn tại.");
                return;
            }

            bangDiem[maDangChon][i] = mon;
            CapNhatDiem();
        }

        // ==================================
        // 13. XOA MON HOC
        // ==================================

        private void XoaMon()
        {
            if (maDangChon == null ||
                dgvDiem.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Chọn môn học cần xóa.");
                return;
            }

            bangDiem[maDangChon].RemoveAt(
                dgvDiem.SelectedRows[0].Index);

            CapNhatDiem();
        }

        // ==================================
        // 14. XUAT CSV MO BANG EXCEL
        // ==================================

        private void XuatCsv()
        {
            using var dialog = new SaveFileDialog
            {
                Filter =
                    "CSV mở bằng Excel (*.csv)|*.csv",
                FileName = "BangDiem.csv"
            };

            if (dialog.ShowDialog(this) !=
                DialogResult.OK)
                return;

            static string BaoChuoi(string s)
            {
                return "\"" +
                    s.Replace("\"", "\"\"") + "\"";
            }

            var csv = new StringBuilder(
                "MSSV,Họ tên,Lớp,Môn học,Số tín chỉ,Điểm\r\n");

            foreach (var s in SinhVienDaLoc())
            {
                if (!bangDiem.TryGetValue(
                    s.MSSV, out var monHoc) ||
                    monHoc.Count == 0)
                {
                    csv.AppendLine(
                        $"{BaoChuoi(s.MSSV)}," +
                        $"{BaoChuoi(s.HoTen)}," +
                        $"{BaoChuoi(s.Lop)},,,");

                    continue;
                }

                foreach (var m in monHoc)
                {
                    csv.AppendLine(
                        $"{BaoChuoi(s.MSSV)}," +
                        $"{BaoChuoi(s.HoTen)}," +
                        $"{BaoChuoi(s.Lop)}," +
                        $"{BaoChuoi(m.TenMon)}," +
                        $"{m.TinChi}," +
                        m.Diem.ToString(
                            CultureInfo.InvariantCulture));
                }
            }

            File.WriteAllText(
                dialog.FileName,
                csv.ToString(),
                new UTF8Encoding(true));

            MessageBox.Show(
                "Đã xuất CSV, có thể mở bằng Excel.");
        }

        // ==================================
        // 15. XEM TRUOC PHIEU DIEM
        // ==================================

        private void XemTruoc()
        {
            if (maDangChon == null)
            {
                MessageBox.Show(
                    "Chọn sinh viên để xem phiếu điểm.");
                return;
            }

            previewDialog.ShowDialog(this);
        }

        // ==================================
        // 16. IN PHIEU DIEM
        // ==================================

        private void In()
        {
            if (maDangChon == null)
            {
                MessageBox.Show(
                    "Chọn sinh viên để in phiếu điểm.");
                return;
            }

            using var dialog = new PrintDialog
            {
                Document = printDocument,
                UseEXDialog = true
            };

            if (dialog.ShowDialog(this) ==
                DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void InPhieu_PrintPage(
            object? sender,
            PrintPageEventArgs e)
        {
            var g = e.Graphics;
            if (g == null) return;

            using var title = new Font(
                "Segoe UI", 16, FontStyle.Bold);

            using var normal = new Font(
                "Segoe UI", 10);

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            g.DrawString(
                "PHIẾU ĐIỂM SINH VIÊN",
                title, Brushes.Black, x, y);

            y += 50;

            var s = dsSinhVien.FirstOrDefault(
                st => st.MSSV == maDangChon);

            if (s == null) return;

            g.DrawString(
                $"MSSV: {s.MSSV}     " +
                $"Họ tên: {s.HoTen}     " +
                $"Lớp: {s.Lop}",
                normal, Brushes.Black, x, y);

            y += 40;

            foreach (var m in bangDiem[s.MSSV])
            {
                g.DrawString(
                    $"{m.TenMon}  |  " +
                    $"{m.TinChi} tín chỉ  |  " +
                    $"Điểm: {m.Diem:0.0}",
                    normal, Brushes.Black, x, y);

                y += 28;
            }

            y += 20;

            g.DrawString(
                $"{lblTongTC.Text}     " +
                $"{lblDiemTB.Text}     " +
                $"{lblXepLoai.Text}",
                normal, Brushes.Black, x, y);
        }
    }
}
