
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BTCh9_Bai3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;

        private GroupBox grpLop = null!,
            grpSinhVien = null!, grpDiem = null!;

        private ListBox lstLop = null!;

        private TextBox txtTimKiem = null!,
            txtMSSV = null!, txtHoTen = null!,
            txtTenMon = null!;

        private DataGridView dgvSinhVien = null!,
            dgvDiem = null!;

        private DateTimePicker dtpNgaySinh = null!;

        private RadioButton radNam = null!, radNu = null!;

        private ComboBox cboLop = null!;

        private NumericUpDown nudTinChi = null!,
            nudDiem = null!;

        private Button btnThemSV = null!,
            btnSuaSV = null!, btnXoaSV = null!;

        private Button btnThemMon = null!,
            btnSuaDiem = null!, btnXoaMon = null!;

        private Button btnXemTruoc = null!,
            btnIn = null!, btnCaiDat = null!,
            btnXuat = null!;

        private Label lblTongTC = null!,
            lblDiemTB = null!, lblXepLoai = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private static Label MakeLabel(
            string text, int x, int y, int width)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 26),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static Button MakeButton(
            string text, int x, int y, int width)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 32),
                UseVisualStyleBackColor = true
            };
        }

        private static DataGridView MakeGrid(
            int x, int y, int w, int h)
        {
            return new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackgroundColor =
                    Color.FromArgb(180, 180, 180),

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.None,

                RowHeadersWidth = 30,
                RowTemplate = { Height = 29 },
                ColumnHeadersHeight = 32,

                ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing,

                ScrollBars = ScrollBars.Both,
                AllowUserToResizeRows = false
            };
        }

        private static void AddColumn(
            DataGridView grid, string name,
            string title, int width)
        {
            grid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = title,
                    Width = width,
                    SortMode =
                        DataGridViewColumnSortMode.NotSortable
                });
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            SuspendLayout();

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.None;

            ClientSize = new Size(1150, 630);
            MinimumSize = new Size(1166, 669);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý sinh viên và điểm";
            Font = new Font("Segoe UI", 9F);

            // ===============================
            // 1. DANH SACH LOP
            // ===============================

            grpLop = new GroupBox
            {
                Name = "grpLop",
                Text = "Lớp",
                Location = new Point(12, 20),
                Size = new Size(233, 307)
            };

            lstLop = new ListBox
            {
                Name = "lstLop",
                Location = new Point(10, 26),
                Size = new Size(214, 269),
                IntegralHeight = false,
                HorizontalScrollbar = true
            };

            grpLop.Controls.Add(lstLop);
            Controls.Add(grpLop);

            // ===============================
            // 2. TIM KIEM SINH VIEN
            // ===============================

            txtTimKiem = new TextBox
            {
                Name = "txtTimKiem",
                Location = new Point(255, 26),
                Size = new Size(585, 29),
                PlaceholderText = "Tìm MSSV / họ tên"
            };

            Controls.Add(txtTimKiem);

            // ===============================
            // 3. BANG SINH VIEN
            // ===============================

            dgvSinhVien = MakeGrid(255, 65, 585, 262);
            dgvSinhVien.Name = "dgvSinhVien";

            AddColumn(
                dgvSinhVien, "colMSSV", "MSSV", 79);

            AddColumn(
                dgvSinhVien, "colHoTen", "Họ tên", 180);

            AddColumn(
                dgvSinhVien, "colNgaySinh", "Ngày sinh", 108);

            AddColumn(
                dgvSinhVien, "colGioiTinh", "Giới tính", 84);

            AddColumn(
                dgvSinhVien, "colLop", "Lớp", 89);

            Controls.Add(dgvSinhVien);

            // ===============================
            // 4. THONG TIN SINH VIEN
            // ===============================

            grpSinhVien = new GroupBox
            {
                Name = "grpSinhVien",
                Text = "Thông tin sinh viên",
                Location = new Point(850, 20),
                Size = new Size(286, 307)
            };

            grpSinhVien.Controls.Add(
                MakeLabel("MSSV", 12, 30, 70));

            txtMSSV = new TextBox
            {
                Name = "txtMSSV",
                Location = new Point(88, 30),
                Size = new Size(184, 29)
            };

            grpSinhVien.Controls.Add(txtMSSV);

            grpSinhVien.Controls.Add(
                MakeLabel("Họ tên", 12, 75, 70));

            txtHoTen = new TextBox
            {
                Name = "txtHoTen",
                Location = new Point(88, 75),
                Size = new Size(184, 29)
            };

            grpSinhVien.Controls.Add(txtHoTen);

            grpSinhVien.Controls.Add(
                MakeLabel("Ngày sinh", 12, 119, 70));

            dtpNgaySinh = new DateTimePicker
            {
                Name = "dtpNgaySinh",
                Location = new Point(88, 119),
                Size = new Size(184, 29),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };

            grpSinhVien.Controls.Add(dtpNgaySinh);

            grpSinhVien.Controls.Add(
                MakeLabel("Giới tính", 12, 163, 70));

            radNam = new RadioButton
            {
                Name = "radNam",
                Text = "Nam",
                Location = new Point(90, 163),
                Size = new Size(74, 28)
            };

            radNu = new RadioButton
            {
                Name = "radNu",
                Text = "Nữ",
                Location = new Point(170, 163),
                Size = new Size(73, 28)
            };

            grpSinhVien.Controls.Add(radNam);
            grpSinhVien.Controls.Add(radNu);

            grpSinhVien.Controls.Add(
                MakeLabel("Lớp", 12, 206, 70));

            cboLop = new ComboBox
            {
                Name = "cboLop",
                Location = new Point(88, 206),
                Size = new Size(184, 29),
                DropDownStyle = ComboBoxStyle.DropDown
            };

            grpSinhVien.Controls.Add(cboLop);

            // NUT THEM - SUA - XOA SINH VIEN

            btnThemSV = MakeButton(
                "Thêm", 12, 259, 80);
            btnThemSV.Name = "btnThemSV";

            btnSuaSV = MakeButton(
                "Sửa", 101, 259, 80);
            btnSuaSV.Name = "btnSuaSV";

            btnXoaSV = MakeButton(
                "Xóa", 190, 259, 82);
            btnXoaSV.Name = "btnXoaSV";

            grpSinhVien.Controls.AddRange(
                new Control[]
                {
                    btnThemSV,
                    btnSuaSV,
                    btnXoaSV
                });

            Controls.Add(grpSinhVien);

            // ===============================
            // 5. BANG DIEM SINH VIEN
            // ===============================

            grpDiem = new GroupBox
            {
                Name = "grpDiem",
                Text = "Điểm của sinh viên",
                Location = new Point(12, 339),
                Size = new Size(1124, 218)
            };

            dgvDiem = MakeGrid(10, 27, 510, 176);
            dgvDiem.Name = "dgvDiem";

            AddColumn(
                dgvDiem, "colSTT", "STT", 54);

            AddColumn(
                dgvDiem, "colMon", "Môn học", 237);

            AddColumn(
                dgvDiem, "colTC", "Số TC", 85);

            AddColumn(
                dgvDiem, "colDiem", "Điểm", 85);

            grpDiem.Controls.Add(dgvDiem);

            // NHAP MON HOC

            grpDiem.Controls.Add(
                MakeLabel("Tên môn", 534, 27, 220));

            txtTenMon = new TextBox
            {
                Name = "txtTenMon",
                Location = new Point(534, 54),
                Size = new Size(291, 29)
            };

            grpDiem.Controls.Add(txtTenMon);

            grpDiem.Controls.Add(
                MakeLabel("Số TC", 839, 27, 86));

            nudTinChi = new NumericUpDown
            {
                Name = "nudTinChi",
                Location = new Point(839, 54),
                Size = new Size(102, 29),
                Minimum = 1,
                Maximum = 30,
                Value = 3
            };

            grpDiem.Controls.Add(nudTinChi);

            grpDiem.Controls.Add(
                MakeLabel("Điểm", 955, 27, 100));

            nudDiem = new NumericUpDown
            {
                Name = "nudDiem",
                Location = new Point(955, 54),
                Size = new Size(154, 29),
                Minimum = 0,
                Maximum = 10,
                DecimalPlaces = 1,
                Increment = 0.5M
            };

            grpDiem.Controls.Add(nudDiem);

            // NUT THEM - SUA - XOA MON

            btnThemMon = MakeButton(
                "Thêm môn", 534, 101, 110);
            btnThemMon.Name = "btnThemMon";

            btnSuaDiem = MakeButton(
                "Sửa điểm", 655, 101, 110);
            btnSuaDiem.Name = "btnSuaDiem";

            btnXoaMon = MakeButton(
                "Xóa môn", 776, 101, 110);
            btnXoaMon.Name = "btnXoaMon";

            grpDiem.Controls.AddRange(
                new Control[]
                {
                    btnThemMon,
                    btnSuaDiem,
                    btnXoaMon
                });

            // TONG KET DIEM

            lblTongTC = MakeLabel(
                "Tổng tín chỉ: 0", 534, 165, 175);
            lblTongTC.Name = "lblTongTC";

            lblDiemTB = MakeLabel(
                "Điểm TB: —", 716, 165, 184);
            lblDiemTB.Name = "lblDiemTB";

            lblXepLoai = MakeLabel(
                "Xếp loại: —", 907, 165, 205);
            lblXepLoai.Name = "lblXepLoai";

            foreach (var label in new[]
            {
                lblTongTC,
                lblDiemTB,
                lblXepLoai
            })
            {
                label.Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold);
            }

            grpDiem.Controls.AddRange(
                new Control[]
                {
                    lblTongTC,
                    lblDiemTB,
                    lblXepLoai
                });

            Controls.Add(grpDiem);

            // ===============================
            // 6. NUT IN VA XUAT EXCEL
            // ===============================

            btnXemTruoc = MakeButton(
                "Xem trước phiếu", 12, 571, 145);
            btnXemTruoc.Name = "btnXemTruoc";

            btnIn = MakeButton(
                "In...", 167, 571, 100);
            btnIn.Name = "btnIn";

            btnCaiDat = MakeButton(
                "Cài đặt trang...", 277, 571, 150);
            btnCaiDat.Name = "btnCaiDat";

            btnXuat = MakeButton(
                "Xuất Excel điểm lớp", 437, 571, 182);
            btnXuat.Name = "btnXuat";

            Controls.AddRange(
                new Control[]
                {
                    btnXemTruoc,
                    btnIn,
                    btnCaiDat,
                    btnXuat
                });

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
