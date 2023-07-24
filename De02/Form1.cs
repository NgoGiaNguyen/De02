using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace De02
{
    public partial class Form1 : Form
    {
        QLSanPhamModels dbcontext = new QLSanPhamModels();
        List<SanPham> ListSanPham;
        public Form1()
        {
            InitializeComponent();

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("BẠN CÓ MUỐN THOÁT ỨNG DỤNG?", "XÁC NHẬN THOÁT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string MaSanPham = txtMaSP.Text;
                SanPham ThemDuLieu = dbcontext.SanPhams.FirstOrDefault(P => P.MaSP == MaSanPham);

                if (ThemDuLieu == null)
                {
                    SanPham P = new SanPham()
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        Ngaynhap = DateTime.Parse(dtNgayNhap.Text),
                        MaLoai = cboLoaiSP.Text,
                    };
                    dbcontext.SanPhams.Add(P);
                    dbcontext.SaveChanges();
                    DanhSachSanPham(dbcontext.SanPhams.ToList());
                    MessageBox.Show("THÊM DỮ LIỆU THÀNH CÔNG!!!!");
                }
                else
                {
                    MessageBox.Show("DỮ LIỆU ĐÃ TỒN TẠI !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI:" + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListSanPham = dbcontext.SanPhams.ToList();
            List<LoaiSP> listloai = dbcontext.LoaiSPs.ToList();

            DanhSachSanPham(ListSanPham);
            ListLoaiSP(listloai);
            lvSanPham.Click += lvSanPham_Click;
        }
        public void ListLoaiSP(List<LoaiSP> listloaisanpham)
        {
            cboLoaiSP.DataSource = listloaisanpham;
            cboLoaiSP.DisplayMember = "MaLoai";
            cboLoaiSP.ValueMember = "TenLoai";
        }


        public void DanhSachSanPham(List<SanPham> listsanpham)
        {
            lvSanPham.Items.Clear();
            foreach (SanPham sp in listsanpham)
            {
                ListViewItem lv = new ListViewItem(sp.MaSP);
                lv.SubItems.Add(sp.TenSP);
                lv.SubItems.Add(sp.Ngaynhap.ToString());
                lv.SubItems.Add(sp.MaLoai);
                lvSanPham.Items.Add(lv);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lvSanPham.SelectedItems.Count > 0)
            {
                try
                {
                    string maSP = lvSanPham.SelectedItems[0].Text;
                    SanPham sanPhamXoa = dbcontext.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanPhamXoa != null)
                    {
                        
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                          
                            dbcontext.SanPhams.Remove(sanPhamXoa);
                            dbcontext.SaveChanges();

                            
                            ListSanPham = dbcontext.SanPhams.ToList();
                            DanhSachSanPham(ListSanPham);

                            MessageBox.Show("XÓA SẢN PHẨM THÀNH CÔNG!", " THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("SẢN PHẨM KHÔNG TỒN TẠI TRONG CƠ SỞ DỮ LIỆU!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI: " + ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("VUI LÒNG CHỌN SẢN PHẨM MUỐN XÓA!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {


            try
            {
                foreach (ListViewItem item in lvSanPham.Items)
                {
                    string maSP = item.SubItems[0].Text;
                    SanPham sanPham = dbcontext.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanPham != null)
                    {
                        
                        sanPham.TenSP = item.SubItems[1].Text;
                        sanPham.Ngaynhap = DateTime.Parse(item.SubItems[2].Text);
                        sanPham.MaLoai = item.SubItems[3].Text;
                    }
                }

               
                dbcontext.SaveChanges();

                MessageBox.Show("LƯU THÔNG TIN SẢN PHẨM THÀNH CÔNG!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI: " + ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTìm_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower().Trim();

            // Tìm kiếm các sản phẩm có mã hoặc tên chứa từ khoá tìm kiếm
            List<SanPham> ketQuaTimKiem = dbcontext.SanPhams.Where(sp => sp.MaSP.ToLower().Contains(keyword) || sp.TenSP.ToLower().Contains(keyword)).ToList();

            if (ketQuaTimKiem.Count > 0)
            {
                // Hiển thị kết quả tìm kiếm trên ListView
                DanhSachSanPham(ketQuaTimKiem);
                MessageBox.Show("TÌM KIẾM THÀNH CÔNG!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("KHÔNG TÌM THẤY SẢN PHẨM!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void lvSanPham_Click(object sender, EventArgs e)
        {
            if (lvSanPham.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvSanPham.SelectedItems[0];
                string maSP = selectedItem.SubItems[0].Text;
                SanPham sanPhamChon = dbcontext.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanPhamChon != null)
                {
                    
                    txtMaSP.Text = sanPhamChon.MaSP;
                    txtTenSP.Text = sanPhamChon.TenSP;
                    dtNgayNhap.Value = sanPhamChon.Ngaynhap.HasValue ? sanPhamChon.Ngaynhap.Value : DateTime.Now;
                    cboLoaiSP.Text = sanPhamChon.MaLoai;
                }
            }
            else
            {
                
                txtMaSP.Clear();
                txtTenSP.Clear();
                dtNgayNhap.Value = DateTime.Now;
                cboLoaiSP.SelectedIndex = -1;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (lvSanPham.SelectedItems.Count > 0)
            {
                try
                {
                    string maSP = lvSanPham.SelectedItems[0].Text;
                    SanPham sanPham = dbcontext.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanPham != null)
                    {
                        
                        sanPham.TenSP = txtTenSP.Text;
                        sanPham.Ngaynhap = dtNgayNhap.Value;
                        sanPham.MaLoai = cboLoaiSP.Text;

                        
                        dbcontext.SaveChanges();

                        
                        ListSanPham = dbcontext.SanPhams.ToList();
                        DanhSachSanPham(ListSanPham);

                        MessageBox.Show("CẬP NHẬT THÔNG TIN THÀNH CÔNG!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SẢN PHẨM KHÔNG TỒN TẠI TRONG CƠ SỞ DỮ LIỆU!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI: " + ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("VUI LÒNG CHỌN SẢN PHẨM ĐỂ CHỈNH SỮA!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnKluu_Click(object sender, EventArgs e)
        {
            
            
        }
    }
}



