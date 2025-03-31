using DataGrid;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using MapleNecrocer;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzComparerR2;

public partial class StringWzForm : Form
{
    public StringWzForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static StringWzForm Instance;

    DataGridViewEx[] Grids = new DataGridViewEx[33];
    bool[] Loaded = new bool[33];
    int Index;

    void LoadWz(string Path)
    {
        foreach (var Iter in Wz.GetNodes("String/" + Path))
        {
            string ID = Iter.Text;

            Name = Wz.GetStr("String/" + Path + "/" + ID + "/name");
            if (Path == "Skill.img" || Path == "Npc.img" || Path == "Pet.img")
                ID = Iter.Text;
            else
                ID = '0' + Iter.Text;
            Grids[Index].Rows.Add(ID, "  " + Name);
        }
    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        Wz_Node Node = null;
        string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        switch (Index)
        {
            case 0:
            case 1:

            case 21:
            case 31:
            case 33:
                Node = Wz.GetNodeByID(ID, WzType.Item);
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 20:

            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 29:
            case 30:
                Node = Wz.GetNodeByID(ID, WzType.Character);
                break;
            case 16:
                Node = Wz.GetNodeByID(ID, WzType.Map);
                break;

            case 17:
                Node = Wz.GetNodeByID(ID, WzType.Mob);
                break;
            case 18:
                Node = Wz.GetNodeByID(ID, WzType.Skill);
                break;
            case 19:
                Node = Wz.GetNodeByID(ID, WzType.Npc);
                break;

            case 32:
                Node = Wz.GetNodeByID(ID, WzType.Reactor);
                break;
        }
        if (Node != null)
            MainForm.ExpandTreeNode(Node);

    }

    private void StringWzForm_Load(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        for (int i = 0; i <= 32; i++)
        {
            Grids[i] = new(60, 164, 0, 0, 220, 400, false, panel1);
            Grids[i].Dock = DockStyle.Fill;
            Grids[i].SearchGrid.Dock = DockStyle.Fill;
            Grids[i].Parent = panel1;


            Grids[i].CellClick += (s, e) =>
            {
                CellClick(Grids[Index], e);
            };

            Grids[i].SearchGrid.CellClick += (s, e) =>
            {
                CellClick(Grids[i].SearchGrid, e);
            };

            switch (i)
            {
                case 0:
                case 1:
                case 21:
                case 31:
                case 33:
                    Grids[i].SetToolTipEvent(WzType.Item, this);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 20:

                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 29:
                case 30:
                    Grids[i].SetToolTipEvent(WzType.Character, this);
                    break;

                case 16:
                    Grids[i].SetToolTipEvent(WzType.Map, this);

                    break;

                case 17:
                    Grids[i].SetToolTipEvent(WzType.Mob, this);

                    break;
                case 18:
                    Grids[i].SetToolTipEvent(WzType.Skill, this);

                    break;
                case 19:
                    Grids[i].SetToolTipEvent(WzType.Npc, this);

                    break;

                case 32:

                    break;

            }




        }
    }

    struct MapNameRec
    {
        public string ID;
        public string MapName;
        public string StreetName;

        public MapNameRec(string id, string mapName, string streetName)
        {
            ID = id;
            MapName = mapName;
            StreetName = streetName;
        }
    }


    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Index = listBox1.SelectedIndex;
        for (int i = 0; i <= 32; i++)
            Grids[i].Visible = false;
        Grids[Index].Visible = true;

        switch (Index)
        {
            case 0:
                if (!Wz.HasNode("Item/Cash"))
                {
                    MessageBox.Show("Cash not found");
                    return;
                }
                break;
            case 22:
                if (!Wz.HasNode("Character/Android"))
                {
                    MessageBox.Show("Character/Android not found");
                    return;
                }
                break;
            case 23:
                if (!Wz.HasNode("Character/Mechanic"))
                {
                    MessageBox.Show("Mechanic not found");
                    return;
                }
                break;
            case 24:
                if (!Wz.HasNode("Character/PetEquip"))
                {
                    MessageBox.Show("PetEquip not found");
                    return;
                }
                break;
            case 25:
                if (!Wz.HasNode("Character/Bits"))
                {
                    MessageBox.Show("Bits not found");
                    return;
                }
                break;
            case 26:
                if (!Wz.HasNode("Character/MonsterBattle.img"))
                {
                    MessageBox.Show("MonsterBattle not found");
                    return;
                }
                break;
            case 27:
                if (!Wz.HasNode("Character/Totem"))
                {
                    MessageBox.Show("Totem not found");
                    return;
                }
                break;
        }

        if (!Loaded[Index])
        {

            var Graphic = Grids[Index].CreateGraphics();
            var Font = new System.Drawing.Font("Arial", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 250, 200);
            Win32.SendMessage(Grids[Index].Handle, false);

            switch (Index)
            {
                case 0:
                    LoadWz("Cash.img");
                    break;
                case 1:
                    LoadWz("Consume.img");
                    break;
                case 2:
                    LoadWz("Eqp.img/Eqp/Weapon");
                    break;
                case 3:
                    LoadWz("Eqp.img/Eqp/Cap");
                    break;
                case 4:
                    LoadWz("Eqp.img/Eqp/Coat");
                    break;

                case 5:
                    LoadWz("Eqp.img/Eqp/Longcoat");
                    break;
                case 6:
                    LoadWz("Eqp.img/Eqp/Pants");
                    break;
                case 7:
                    LoadWz("Eqp.img/Eqp/Shoes");
                    break;
                case 8:
                    LoadWz("Eqp.img/Eqp/Glove");
                    break;
                case 9:
                    LoadWz("Eqp.img/Eqp/Ring");
                    break;
                case 10:
                    LoadWz("Eqp.img/Eqp/Cape");
                    break;

                case 11:
                    LoadWz("Eqp.img/Eqp/Accessory");
                    break;
                case 12:
                    LoadWz("Eqp.img/Eqp/Shield");
                    break;
                case 13:
                    LoadWz("Eqp.img/Eqp/Taming");
                    break;
                case 14:
                    LoadWz("Eqp.img/Eqp/Hair");
                    break;
                case 15:
                    LoadWz("Eqp.img/Eqp/Face");
                    break;
                case 16:
                    Dictionary<string, MapNameRec> MapNameList = new();
                    Dictionary<string, string> MapNames = new();
                    if (Wz.HasNode("String/Map.img"))
                    {
                        foreach (var Iter in Wz.GetNodes("String/Map.img"))
                        {
                            foreach (var Iter2 in Iter.Nodes)
                            {
                                string ID = Iter2.Text.PadLeft(9, '0');
                                var MapName = Iter2.GetStr("mapName");
                                var StreetName = Iter2.GetStr("streetName");
                                MapNameList.AddOrReplace(ID, new MapNameRec(ID, MapName, StreetName));
                                if (!MapNames.ContainsKey(ID))
                                {
                                    MapNames.Add(ID, MapName);
                                }
                            }
                        }

                        foreach (var Dir in Wz.GetNodes("Map/Map"))
                        {
                            if (Dir.Text.LeftStr(3) != "Map" && Wz.HasHardCodedStrings == false)
                                continue;
                            foreach (var img in Dir.Nodes)
                            {
                                if (!Char.IsNumber(img.Text[0]))
                                    continue;
                                var ID = img.ImgID();
                                if (MapNames.ContainsKey(ID))
                                    Grids[Index].Rows.Add(ID, "  " + MapNames[ID]);
                                else
                                    Grids[Index].Rows.Add(ID, "");
                            }
                        }
                    }
                    else if (Wz.HasHardCodedStrings)
                    {
                        foreach (var Iter in Wz.GetNodes("Map/Map"))
                        {
                            string ID = Iter.Text.Replace(".img", "");
                            ID = ID.PadLeft(9, '0');
                            var MapName = Iter.HasNode("info/mapName") ? Iter.GetStr("info/mapName") : "";
                            var StreetName = Iter.HasNode("info/streetName") ? Iter.GetStr("info/mapName") : "";
                            MapNameList.AddOrReplace(ID, new MapNameRec(ID, MapName, StreetName));
                            if (!MapNames.ContainsKey(ID))
                            {
                                Grids[Index].Rows.Add(ID, "  " + MapName);
                                MapNames.Add(ID, MapName);
                            }
                        }
                    }
                    break;

                case 17:
                    LoadWz("Mob.img");
                    break;

                case 18:
                    LoadWz("Skill.img");
                    break;

                case 19:
                    LoadWz("Npc.img");
                    break;
                case 20:
                    LoadWz("Pet.img");
                    break;
                case 21:
                    LoadWz("Ins.img");
                    break;
                case 22:
                    LoadWz("Eqp.img/Eqp/Android");
                    break;

                case 23:
                    LoadWz("Eqp.img/Eqp/Mechanic");
                    break;
                case 24:
                    LoadWz("Eqp.img/Eqp/PetEquip");
                    break;

                case 25:
                    LoadWz("Eqp.img/Eqp/Bit");
                    break;
                case 26:
                    LoadWz("Eqp.img/Eqp/MonsterBattle");
                    break;
                case 27:
                    var Child = Wz.GetNode("String/Eqp.img/Eqp/Accessory");
                    foreach (var img in Wz.GetNodes("Character/Totem"))
                    {
                        if (img.Text.LeftStr(1) != "0")
                            continue;
                        string ID = img.ImgID().IDString();
                        Grids[Index].Rows.Add(img.ImgID(), "  " + Child.GetStr(ID + "/name", ""));
                    }
                    break;
                case 28:
                    break;

                case 29:
                    string CardID = "";
                    foreach (var img in Wz.GetNodes("Character/Familiar"))
                    {
                        if (img.Text.RightStr(4) != ".img")
                            continue;
                        var ID = img.ImgID();
                        var Entry = Wz.GetNode("Character/Familiar/" + img.Text);
                        if (Wz.GetNode("Etc/FamiliarInfo.img") != null)
                        {
                            if (Wz.GetNode("Etc/FamiliarInfo.img/" + ID) != null)
                                CardID = Wz.GetNode("Etc/FamiliarInfo.img/" + ID).GetStr("consume");
                        }
                        else
                        {
                            if (Entry.GetNode("info/monsterCardID") != null)
                                CardID = Entry.GetNode("info/monsterCardID").Value.ToString();
                            else
                                CardID = "";
                        }
                        var CardName = Wz.GetNode("String/Consume.img/" + CardID).GetStr("name");
                        Grids[Index].Rows.Add(ID, CardName);
                    }
                    break;
                case 30:
                    foreach (var Iter in Wz.GetNodes("String/Consume.img"))
                    {
                        var Name = Iter.GetStr("name");
                        if ((Name.Contains("字型")) || (Name.Contains("ジスキン")) || (Name.Contains("스킨")) || (Name.Contains
                          ("Damage Skin")) || (Name.Contains("字型")) || (Name.Contains("伤害皮肤")))
                        {
                            var ID = "0" + Iter.Text;
                            string[] imgs = new string[] { "0243.img", "0263.img" };
                            Grids[Index].Rows.Add(ID, Name);
                        }
                    }
                    break;

                case 31:
                    LoadWz("Etc.img/Etc");
                    break;
            }

            Grids[Index].Sort(Grids[Index].Columns[0], ListSortDirection.Ascending);
            Win32.SendMessage(Grids[Index].Handle, true);
            Grids[Index].Refresh();
            Loaded[Index] = true;
        }
    }

    private void StringWzForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.tooltipRef.Visible = false;
    }
}
