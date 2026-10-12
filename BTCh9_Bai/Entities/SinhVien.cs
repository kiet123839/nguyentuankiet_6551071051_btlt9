using System;
using System.Collections.Generic;

namespace BTCh9_Bai.Entities;

public partial class SinhVien
{
    public string MaSv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string GioiTinh { get; set; } = null!;

    public string MaLop { get; set; } = null!;

    public virtual ICollection<DiemMon> DiemMons { get; set; } = new List<DiemMon>();

    public virtual Lop MaLopNavigation { get; set; } = null!;
}
