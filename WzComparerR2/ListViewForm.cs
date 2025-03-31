using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using MapleNecrocer;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzComparerR2;

public partial class ListViewForm : Form
{
    public ListViewForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ListViewForm Instance;
    public DevComponents.AdvTree.AdvTree advTree1;
   
    Bitmap NullBmp = new(1, 1);

    void Dump2(Wz_Node Entry)
    {
        if (Entry != null)
        {
            switch (Entry.Value)
            {
                case Wz_Uol:
                    ListGrid.Rows.Add(NullBmp, Entry.FullPathToFile2().Replace("/",".") + " = " + ((Wz_Uol)Entry.Value).Uol);
                    break;
                case Wz_Png:
                   
                    ListGrid.Rows.Add(Wz.GetImgNode(Entry.FullPathToFile2()).ExtractPng() , Entry.FullPathToFile2().Replace("/", "."));
                    break;

                case Wz_Vector:
                    var P = Entry.GetValue<Wz_Vector>();
                    ListGrid.Rows.Add(NullBmp, Entry.FullPathToFile2().Replace("/",".") + " = " + P.X.ToString() + ", " + P.Y.ToString());
                    break;
                default:
                    if (Entry.Value != null)
                    {
                        ListGrid.Rows.Add(NullBmp, Entry.FullPathToFile2().Replace("/",".") + " = " + Entry.Value);
                    }
                    break;
            }

            foreach (var E in Entry.Nodes)
                if (!(E.Value is Wz_Image))
                    Dump2(E);
        }
    }

    public void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0) return;

        
        if (e.ColumnIndex == 0)
        {
            Bitmap Bmp = e.Value as Bitmap;
            string str = ListGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            var Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            int TextWidth = (int)e.Graphics.MeasureString(str, Font).Width;
            e.PaintBackground(e.ClipBounds, false);

            if (Bmp.Width > 1)
            {
                if (Bmp.Width < 200 && Bmp.Height < 200)
                {
                    e.Graphics.DrawImage(Bmp, new Rectangle(TextWidth + e.CellBounds.Location.X + 5,
                                         e.CellBounds.Location.Y + (e.CellBounds.Height / 2) - (Bmp.Height / 2), Bmp.Width, Bmp.Height));

                }
                else
                {
                    e.Graphics.DrawImage(Bmp, new Rectangle(TextWidth + e.CellBounds.Location.X + 5,
                                         e.CellBounds.Location.Y + (e.CellBounds.Height / 2) - (200 / 2), 200, 200));

                }
            }

            e.Graphics.DrawString(str, Font, new SolidBrush(Color.Black),
                                  new PointF(e.CellBounds.X, e.CellBounds.Location.Y + (e.CellBounds.Height / 2) - 10),
                                  StringFormat.GenericDefault);

            e.Handled = true;
            ListGrid.Update();
        }

    }

    private void ListViewForm_Load(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
       
        this.advTree1 = new DevComponents.AdvTree.AdvTree();
        this.advTree1.Parent = panel1;
        this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
        this.advTree1.AllowDrop = true;
        this.advTree1.BackColor = System.Drawing.SystemColors.Window;
        this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
        this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
        this.advTree1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.advTree1.DragDropEnabled = false;
        this.advTree1.Indent = 14;
        this.advTree1.Location = new System.Drawing.Point(0, 0);
        this.advTree1.Name = "advTree1";
        this.advTree1.NodeStyle = DevComponents.DotNetBar.ElementStyle.GetDefaultCellStyle(null);
        this.advTree1.PathSeparator = ";";
        this.advTree1.Size = new System.Drawing.Size(200, 250);
        this.advTree1.TabIndex = 0;
        this.advTree1.Text = "advTree1";
        advTree1.Nodes.Add(MainForm.Instance.advTree1Node);
        ListViewForm.Instance.advTree1.Update();

        this.advTree1.AfterNodeSelect += (n, g) =>
        {
            ListGrid.Rows.Clear();
            ListGrid.Refresh();


            string SelectText = advTree1.SelectedNode.Text;

            var Graphic = ListGrid.CreateGraphics();
            var Font = new System.Drawing.Font("Arial", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 250, 200);
        
            Win32.SendMessage(ListGrid.Handle, false);
            if (SelectText.Length >= 4 && SelectText.RightStr(4) == ".img")
            {
                string Path = advTree1.SelectedNode.FullPath.Replace(";", "/").Replace("Base.wz/", "");
                Dump2(Wz.GetNode(Path));
            }

            for (int i = 0; i < ListGrid.RowCount; i++)
            {
              //  if(!( ListGrid.Rows[i].Cells[0].Value is Bitmap))
                 //   continue;
                    Bitmap Bmp = ListGrid.Rows[i].Cells[0].Value as Bitmap;
                if (Bmp.Width > 1)
                {
                    if (Bmp.Width < 200 && Bmp.Height < 200)
                    {
                        ListGrid.Rows[i].Height = Bmp.Height + 5;
                        if (Bmp.Height < 25)
                            ListGrid.Rows[i].Height = 25;
                    }
                    else
                    {
                        ListGrid.Rows[i].Height = 200 + 5;
                    }
                }
                else
                {
                    ListGrid.Rows[i].Height = 25;
                }
            }
            Win32.SendMessage(ListGrid.Handle, true);
            ListGrid.Refresh();
        };


        ListGrid.CellPainting += CellPainting;
        var Icon = new DataGridViewImageColumn();
        Icon.DataPropertyName = "Icon";
        Icon.ReadOnly = true;
        Icon.Width = 1000;

        var Temp = new DataGridViewTextBoxColumn();
        Temp.Width = 0;

        ListGrid.Columns.AddRange(Icon, Temp);

        ListGrid.RowTemplate.Height = 50;
        ListGrid.Refresh();


    }
}
