using System;
using System.Collections.Generic;

namespace BTCh9_Bai.Entities;

public partial class DiemMon
{
    public int MaDiem { get; set; }

    public string MaSv { get; set; } = null!;

    public string TenMon { get; set; } = null!;

    public int SoTinChi { get; set; }

    public decimal Diem { get; set; }

    public virtual SinhVien MaSvNavigation { get; set; } = null!;
}
