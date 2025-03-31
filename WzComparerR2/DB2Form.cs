using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using System.Text.RegularExpressions;
using WzComparerR2;
using WzComparerR2.Common;
using WzComparerR2.CharaSimControl;
using WzComparerR2.CharaSim;
using System.Runtime.InteropServices;
using WzComparerR2.Controls;
using WzComparerR2.Common;
using DataGrid;
using System.Threading;
using WzComparerR2.MapRender;
using WzComparerR2.PluginBase;
using Game = Microsoft.Xna.Framework.Game;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;

namespace WzComparerR2
{


    public partial class DB2Form : Form
    {
        public DB2Form()
        {
            InitializeComponent();
            Instance = this;
        }



        public static DB2Form Instance;
        List<string> ColList, ColList1, RowList;
        Dictionary<int, List<string>> RowList1;
        int Row1 = -1;
        int tabIndex = 0;
        DataViewer Grid;
        DataViewer SearchGrid;

        void Dump2(Wz_Node Entry)
        {
            if (Entry != null)
            {
                if (Entry.Value is Wz_Vector)
                {
                    var P = Entry.GetValue<Wz_Vector>();
                    ColList.Add(Entry.GetPathD() + "=" + P.X.ToString() + "," + P.Y.ToString() + ",  ");

                }
                else if (Entry.GetValue("Null") != "Null")
                    ColList.Add(Entry.GetPathD() + "=" + Entry.GetValueEx<string>("-") + ",  ");
                foreach (var E in Entry.Nodes)
                    if (!(E.Value is Wz_Png))
                        Dump2(E);
            }
        }

        void Delete(string s, int index, int count)
        {
            if ((index < 1) | (index > s.Length) | (count <= 0))
                return;
            if (index + count - 1 > s.Length)
                count = s.Length - index + 1;
            s = s.Remove(index - 1, count);
        }
        void DumpData2(Wz_Node Entry)
        {

            Dump2(Entry);
            string FinalStr = "";
            var S = Entry.GetPathD() + ".";
            for (int i = 0; i < ColList.Count; i++)
            {
                ColList[i] = ColList[i].Replace(S, "");
                FinalStr = FinalStr + ColList[i];
            }
            Delete(FinalStr, FinalStr.Length - 2, 1);
            RowList.Add(FinalStr);
            ColList.Clear();
        }

        void DumpData1()
        {
            ColList1 = new List<string>();
            Row1++;
            RowList1.Add(Row1, ColList1);

        }

        void PutGridData1(int Col)
        {

            string[] FinalStr = new string[RowList1.Count + 1];
            foreach (var i in RowList1.Keys)
            {
                for (int j = 0; j < RowList1[i].Count; j++)
                    FinalStr[i] = FinalStr[i] + RowList1[i][j];
                // Delete(FinalStr[i], inttostr(Length(FinalStr[i])) , 1);
                Grid[Col, i].Value = FinalStr[i];
                //RowList1[i].Free;
            }

            RowList1.Clear();
            //    SetLength(FinalStr, 0);
        }


        void LoadItem()
        {
            var ItemDir = tabControl1.TabPages[tabIndex].Name;


            Wz_Node Child = null;

            if (Wz.HasNode("String/Item.img/Etc"))
            {
                switch (ItemDir)
                {

                    case "Cash":
                        Child = Wz.GetNode("String/Item.img/Cash");
                        break;
                    case "Consume":
                        Child = Wz.GetNode("String/Item.img/Con");
                        break;
                    case "Etc":
                        Child = Wz.GetNode("String/Item.img/Etc");
                        break;
                    case "Install":
                        Child = Wz.GetNode("String/Item.img/Ins");
                        break;
                    case "Pet":
                        Child = Wz.GetNode("String/Item.img/Pet");
                        break;
                }
            }
            else
            {
                switch (ItemDir)
                {
                    case "Etc":
                        Child = Wz.GetNode("String/Etc.img/Etc");
                        break;
                    case "Install":
                        Child = Wz.GetNode("String/Ins.img");
                        break;

                    default:
                        Child = Wz.GetNode("String/" + ItemDir + ".img");
                        break;

                }
            }
            string ID;
            Bitmap Icon = null;

           
            foreach (var img in Wz.GetNode("Item/" + ItemDir).Nodes)
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                if (ItemDir == "Pet")
                {
                    ID = img.ImgID();
                    if (Wz.HasNode("Item/Pet/" + img.Text + "/info/iconD"))
                        Icon = Wz.GetBmp("Item/Pet/" + img.Text + "/info/iconD");
                    var Name = Child.GetStr(ID + "/name", "  ");
                    var Desc = Child.GetStr(ID + "/desc", "  ");
                    DumpData2(Wz.GetNode("Item/Pet/" + img.Text + "/info"));
                    Grid.Rows.Add(ID, Icon, Name, Desc, "");
                }
                else if (ItemDir == "Special")
                {
                    if (img.Text != "0910.img")
                        continue;

                    List<string> CashPackages = new List<string>();
                    foreach (var Iter in Wz.GetNodes("Etc/CashPackage.img"))
                    {
                        CashPackages.Add(Iter.Text);

                    }

                    foreach (var Iter in Wz.GetNodes("Item/Special/0910.img"))
                    {
                        if (CashPackages.Contains(Iter.Text))
                        {
                            ID = Iter.Text.IDString();
                            if (Iter.GetNode("icon") != null)
                                Icon = Iter.GetBmp("icon");
                            Grid.Rows.Add(ID, Icon, Iter.GetStr("name", "  "), Iter.GetStr("desc", "  "), " ");
                        }
                    }
                    CashPackages.Clear();
                }
                else
                {
                    foreach (var Iter in Wz.GetNodes("Item/" + ItemDir + "/" + img.Text))
                    {
                        DumpData2(Iter);
                        ID = Iter.Text.IDString();
                        if (Iter.GetNode("info/icon") != null)
                            Icon = Iter.GetNode("info/icon").ExtractPng();
                        Grid.Rows.Add(Iter.Text, Icon,Child.GetStr(ID + "/name", "  "), Child.GetStr(ID + "/desc", "  "), " ");
                    }
                }

            }
            for (int i = 0; i < RowList.Count; i++)
                Grid[4, i].Value = RowList[i];
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            SetToolTipEvent(WzType.Item);

        }



        string StrJoin(string Separator, params string[] StringArray)
        {
            var Result = "";
            for (int i = 0; i < StringArray.Length; i++)
                Result = Result + StringArray[i] + Separator;
            Delete(Result, Result.Length, 1);
            return Result;

        }
        string GetJobID(string ID)
        {
            return (int.Parse(ID) / 10000).ToString();
        }
        string LeftStr(string s, int count)
        {
            if (count > s.Length)
                count = s.Length;
            return s.Substring(0, count);
        }

        void LoadCharacter()
        {

            var Dir = tabControl1.TabPages[tabIndex].Name;
            if (Wz.GetNode("Character/" + Dir) == null)
            {
                MessageBox.Show(Dir + "  not found");
                return;
            }

            Wz_Node Child = null;
            switch (Dir)
            {
                case "Totem":
                    Child = Wz.GetNode("String/Eqp.img/Eqp/Accessory");
                    break;
                case "TamingMob":
                    Child = Wz.GetNode("String/Eqp.img/Eqp/Taming");
                    break;
                default:
                    Child = Wz.GetNode("String/Eqp.img/Eqp/" + Dir);
                    break;
            }
            var Row = -1;
            string ID, Desc, h1;
            Bitmap Icon = null;
            string IName, Data = "", D;
            foreach (var img in Wz.GetNode("Character/" + Dir).Nodes)
            {
                if (LeftStr(img.Text, 1) != "0")
                    continue;
                Row += 1;
                DumpData2(img.GetNode("info"));
                //DumpData1();
                switch (Dir)
                {
                    case "Hair":
                        if (img.GetNode("default/hairOverHead") != null)
                            Icon = img.GetNode("default/hairOverHead").ExtractPng();
                        break;
                    case "Face":
                        if (img.GetNode("default/face") != null)
                            Icon = img.GetNode("default/face").ExtractPng();
                        break;
                    default:
                        if (img.GetNode("info/icon") != null)
                            Icon = img.GetNode("info/icon").ExtractPng();
                        break;
                }
                ID = img.ImgID().IDString();

                Desc = Child.GetStr(ID + "/desc");
                h1 = Child.GetStr(ID + "/h1");
                RowList[Row] += ", " + Desc + h1;

                Grid.Rows.Add(img.ImgID(), Icon, Child.GetStr(ID + "/name", ""), RowList[Row]);


            }

            if (Dir == "TamingMob")
            {
                var Dict = new Dictionary<string, string>();
                for (int i = 11; i <= 28; i++)
                {
                    if (Wz.GetNode("Skill/8000" + i.ToString() + ".img") != null)
                    {
                        foreach (var Iter in Wz.GetNodes("Skill/8000" + i.ToString() + ".img/skill"))
                        {
                            if ((Iter.GetNode("vehicleID") != null) && (!Dict.ContainsKey("0" + Iter.GetNode("vehicleID").Value.ToString())))
                                Dict.Add("0" + Iter.GetNode("vehicleID").Value.ToString(), Iter.Text);

                        }
                    }
                }
                for (int i = 0; i <= 9; i++)
                {
                    if (Wz.GetNode("Skill/80011" + i.ToString() + ".img") != null)
                    {
                        foreach (var Iter in Wz.GetNodes("Skill/80011" + i.ToString() + ".img/skill"))
                        {
                            if ((Iter.GetNode("vehicleID") != null) && (!Dict.ContainsKey("0" + Iter.GetNode("vehicleID").Value.ToString())))
                                Dict.Add("0" + Iter.GetNode("vehicleID").Value.ToString(), Iter.Text);

                        }
                    }
                }

                for (int i = 0; i < Grid.RowCount - 1; i++)
                {
                    // if (Grid[0, i].Value is string)
                    {
                        var TamingID = Grid[0, i].Value.ToString();
                        // if ((Grid[2, i].Value == "") && (Dict.ContainsKey(TamingID)))
                        if (Dict.ContainsKey(TamingID))
                            if (Wz.GetNode("String/Skill.img/" + Dict[TamingID]) != null)
                                Grid[2, i].Value = Wz.GetNode("String/Skill.img/" + Dict[TamingID]).GetStr("name", " ");
                    }

                }

            }

            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            SetToolTipEvent(WzType.Character);

        }

        void LoadMap(int Part)
        {

            var Links = new List<(string, int)>();
            var MapNames = new Dictionary<string, (string, string)>();
            string StreetName, MapName;
            foreach (var Iter in Wz.GetNodes("String/Map.img"))
            {
                foreach (var Iter2 in Iter.Nodes)
                {
                    string ID = Iter2.Text.PadLeft(9, '0');
                    StreetName = Iter2.GetStr("streetName");
                    MapName = Iter2.GetStr("mapName");
                    if (!MapNames.ContainsKey(ID))
                        MapNames.Add(ID, (StreetName, MapName));
                }
            }
            Wz_Node MapDir;

            MapDir = Wz.GetNode("Map/Map");
            Bitmap Icon;
            ;
            var Row = -1;

            foreach (var Dir in MapDir.Nodes)
            {
                if (LeftStr(Dir.Text, 3) != "Map")
                    continue;
                switch (Part)
                {
                    case 1:
                        if ((Dir.Text != "Map0") && (Dir.Text != "Map1") && (Dir.Text != "Map2") && (Dir.Text != "Map3"))
                            continue;
                        break;
                    case 2:
                        if ((Dir.Text != "Map4") && (Dir.Text != "Map5") && (Dir.Text != "Map6") && (Dir.Text != "Map7") && (Dir.Text != "Map8"))
                            continue;
                        break;
                    case 3:
                        if (Dir.Text != "Map9")
                            continue;
                        break;
                }

                foreach (var img in Dir.Nodes)
                {
                    if (!Char.IsNumber(img.Text[0]))
                        continue;
                    Row += 1;
                    DumpData2(img.GetNode("info"));

                    if (MapNames.ContainsKey(img.ImgID()))
                    {
                        StreetName = MapNames[img.ImgID()].Item1;
                        MapName = MapNames[img.ImgID()].Item2;
                    }
                    else
                    {
                        StreetName = "";
                        MapName = "";
                    }

                    if (img.GetNode("miniMap/canvas") != null)
                        Icon = img.GetNode("miniMap/canvas").ExtractPng();
                    else
                        Icon = null;

                    Grid.Rows.Add(img.ImgID(), Icon, StreetName, MapName, "");
                    var Link = img.GetNode("info/link");
                    if (Link != null)
                        Links.Add(("Map" + LeftStr(Link.Value.ToString(), 1) + "/" + Link.Value.ToString() + ".img", Row));
                }

            }

            for (int i = 0; i < Links.Count; i++)
            {
                var Child = MapDir.GetNode(Links[i].Item1 + "/miniMap/canvas");
                if (Child != null)
                    Grid[1, Links[i].Item2].Value = Child.ExtractPng();
            }
            for (int i = 0; i < RowList.Count; i++)
            {
                Grid[4, i].Value = RowList[i];
            }
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

        }



        string Copy(string s, int index, int count)
        {
            if (index < 1)
                index = 1;
            if ((index > s.Length) || (count <= 0))
            {
                return "";
                // exit;
            }
            if (index + count - 1 > s.Length)
                count = s.Length - index + 1;
            return s.Substring(index - 1, count);
        }

        string RightStr(string s, int count)
        {
            if (count > s.Length)
                count = s.Length;
            return s.Substring(s.Length - count, count);

        }
        void LoadMob(int Part)
        {


            if (MainForm.HasMob001)
            {
                var Links = new List<(string, int)>();
                Wz_Node Child = null;
                var Row = -1;
                string Data = "", D = "";
                Bitmap Icon = null;

                foreach (var Iter in Wz.GetNodes("String/Mob.img"))
                {
                    var ID = Iter.Text.PadLeft(7, '0');
                    if (Wz.GetNode("Mob/" + ID + ".img") == null)
                        continue;

                    var LeftNum = LeftStr(ID, 1);
                    switch (Part)
                    {
                        case 1:
                            if (LeftNum == "8" || LeftNum == "9")
                                continue;
                            break;
                        case 2:
                            if (LeftNum == "0" || LeftNum == "1" || LeftNum == "2" || LeftNum == "3" || LeftNum == "4" || LeftNum == "5" || LeftNum == "6" || LeftNum == "7" || LeftNum == "9")
                                continue;
                            break;
                        case 3:
                            if (LeftNum == "0" || LeftNum == "1" || LeftNum == "2" || LeftNum == "3" || LeftNum == "4" || LeftNum == "5" || LeftNum == "6" || LeftNum == "7" || LeftNum == "8")
                                continue;
                            break;
                    }

                    Row += 1;

                    DumpData2(Wz.GetNode("Mob/" + ID + ".img"));

                    if (Wz.GetNode("Mob/" + ID + ".img/info/link") != null)
                    {
                        Links.Add((Wz.GetNode("Mob/" + ID + ".img/info/link").Value.ToString(), Row));
                        //  continue;
                    }

                    if (Wz.GetNode("Mob/" + ID + ".img/stand/0") != null)
                        Child = Wz.GetNode("Mob/" + ID + ".img/stand/0");
                    else if (Wz.GetNode("Mob/" + ID + ".img/fly/0") != null)
                        Child = Wz.GetNode("Mob/" + ID + ".img/fly/0");
                    if (Child != null)
                        Icon = Child.ExtractPng();
                    Grid.Rows.Add(ID, Icon, Wz.GetNode("String/Mob.img/" + Iter.Text).GetStr("name", ""), "");


                }

                for (int i = 0; i < Links.Count; i++)
                {
                    if (Wz.GetNode("Mob/" + Links[i].Item1 + ".img/stand/0") != null)
                        Child = Wz.GetNode("Mob/" + Links[i].Item1 + ".img/stand/0");
                    else if (Wz.GetNode("Mob/" + Links[i].Item1 + ".img/fly/0") != null)
                        Child = Wz.GetNode("Mob/" + Links[i].Item1 + ".img/fly/0");
                    Grid[1, Links[i].Item2].Value = Child.ExtractPng();

                }
                for (int i = 0; i < RowList.Count; i++)
                {
                    Grid[4, i].Value = RowList[i];
                }

                Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);


            }
            else
            {
                var Links = new List<(string, int)>();
                Wz_Node Child = null;
                var Row = -1;
                string Data = "", D = "";
                Bitmap Icon = null;
                foreach (var Iter in Wz.GetNodes("Mob"))
                {
                    if (RightStr(Iter.Text, 4) != ".img")
                        continue;
                    var LeftNum = LeftStr(Iter.Text, 1);
                    switch (Part)
                    {
                        case 1:
                            if (LeftNum == "8" || LeftNum == "9")
                                continue;
                            break;
                        case 2:
                            if (LeftNum == "0" || LeftNum == "1" || LeftNum == "2" || LeftNum == "3" || LeftNum == "4" || LeftNum == "5" || LeftNum == "6" || LeftNum == "7" || LeftNum == "9")
                                continue;
                            break;
                        case 3:
                            if (LeftNum == "0" || LeftNum == "1" || LeftNum == "2" || LeftNum == "3" || LeftNum == "4" || LeftNum == "5" || LeftNum == "6" || LeftNum == "7" || LeftNum == "8")
                                continue;
                            break;
                    }

                    Row += 1;
                    if (Wz.GetNode("Mob/" + Iter.Text + "/stand/0") != null)
                        Child = Wz.GetNode("Mob/" + Iter.Text + "/stand/0");
                    else if (Wz.GetNode("Mob/" + Iter.Text + "/fly/0") != null)
                        Child = Wz.GetNode("Mob/" + Iter.Text + "/fly/0");
                    //DumpData1();
                    DumpData2(Wz.GetNode("Mob/" + Iter.Text));
                    if (Child != null)
                        Icon = Child.ExtractPng();
                    Grid.Rows.Add(Iter.ImgID(), Icon, Wz.GetNode("String/Mob.img/" + Iter.ImgID().IDString()).GetStr("name"), "");

                    //return;
                    var Link = Iter.GetNode("info/link");
                    if (Link != null)
                        Links.Add((Link.Value.ToString() + ".img", Row));

                }

                for (int i = 0; i < Links.Count; i++)
                {
                    if (Wz.GetNode("Mob/" + Links[i].Item1 + "/stand/0") != null)
                        Child = Wz.GetNode("Mob/" + Links[i].Item1 + "/stand/0");
                    else if (Wz.GetNode("Mob/" + Links[i].Item1 + "/fly/0") != null)
                        Child = Wz.GetNode("Mob/" + Links[i].Item1 + "/fly/0");

                    Grid[1, Links[i].Item2].Value = Child.ExtractPng();
                }
                for (int i = 0; i < RowList.Count; i++)
                {
                    Grid[3, i].Value = RowList[i];
                }

                Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            }

            Grid.Columns[3].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            SetToolTipEvent(WzType.Mob);
        }


        [DllImport("Eval2.dll", EntryPoint = "Eval", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void Eval(string Formula, ref double Value, ref int ErrPos);

        double GetFValue(string FormStr, int Level)
        {
            int E = 0;
            double A = 0;
            string Str = "";
            Str = FormStr.Replace("x", Level.ToString());
            Eval(Str, ref A, ref E);
            return A;
        }
        Wz_Node Common;
        int MaxLev;
        string CommonMatch(System.Text.RegularExpressions.Match Match1)
        {

            string MatchName = Copy(Match1.Value, 2, 100);
            foreach (var Iter in Common.Nodes)
            {
                if (Iter.Text == MatchName)
                    return GetFValue(Iter.Value.ToString(), MaxLev).ToString();
            }
            return null;
        }
        void LoadSkill()
        {
            Bitmap Icon;
            foreach (var L1 in MainForm.TreeNode.Nodes)
            {
                if (LeftStr(L1.Text, 5) != "Skill")
                    continue;

                foreach (var img in L1.Nodes)
                {
                    if (Char.IsNumber(img.Text, 0))
                    {
                        var LeftNum = LeftStr(img.Text, 1);

                        DumpData2(Wz.GetNode("Skill/" + img.Text + "/info"));

                        var BookID = img.ImgID();
                        string BookName = "";
                        if (Wz.GetNode("String/Skill.img/" + BookID) != null)
                            BookName = Wz.GetNode("String/Skill.img/" + BookID).GetValue2("bookName", "");

                        if (Wz.GetNode("Skill/" + img.Text + "/info/icon") != null)
                            Icon = Wz.GetNode("Skill/" + img.Text + "/info/icon").ExtractPng();
                        else
                            Icon = null;

                        Grid.Rows.Add(BookID, Icon, BookName, "", "");

                        foreach (var Iter in Wz.GetNode("Skill/" + img.Text).Nodes)
                        {
                            foreach (var Iter2 in Iter.Nodes)
                            {

                                if (Iter.Text == "skill")
                                {
                                    DumpData2(Iter2);
                                    var SkillID = Iter2.Text;
                                    if (Iter2.GetNode("icon") != null)
                                        Icon = Iter2.GetNode("icon").ExtractPng();
                                    string SkillName = "", Desc = "";
                                    if (Wz.GetNode("String/Skill.img/" + SkillID) != null)
                                    {
                                        SkillName = Wz.GetNode("String/Skill.img/" + SkillID).GetValue2("name", "");
                                        Desc = Wz.GetNode("String/Skill.img/" + SkillID).GetValue2("desc", "");
                                    }
                                    string hDesc = "";
                                    var Child = Wz.GetNode("String/Skill.img/" + SkillID);
                                    if (Child != null)
                                    {
                                        if (Child.GetNode("h") != null)
                                        {
                                            if (Child.GetNode("h").Value is string)
                                            {
                                                hDesc = Child.GetNode("h").Value.ToString();
                                            }
                                            hDesc = hDesc.Replace("mpConMP", "mpCon MP");
                                            hDesc = hDesc.Replace(",", " ,");
                                            Common = Iter2.GetNode("common");
                                            if (Common != null)
                                            {
                                                MaxLev = Common.GetValue2("maxLevel", 1);
                                                if (hDesc != "")
                                                {
                                                    hDesc = "Lv." + MaxLev.ToString() + "= " + Regex.Replace(hDesc, "\\#[0-9,_,a-z,A-Z,\\.]+", CommonMatch);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 1; i <= 30; i++)
                                            {
                                                if (Child.GetNode("h" + i.ToString()) != null)
                                                    hDesc = "Lv." + i.ToString() + "= " + Child.GetNode("h" + i.ToString()).Value.ToString();
                                            }
                                        }
                                    }

                                    Grid.Rows.Add(SkillID, Icon, SkillName, Desc, hDesc, "");
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < RowList.Count; i++)
                Grid[5, i].Value = RowList[i];

            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            SetToolTipEvent(WzType.Skill);

        }


        void LoadNpc()
        {
            var Row = -1;
            var Links = new List<(string, int)>();
            Bitmap Icon = null;
            foreach (var Img in Wz.GetNodes("Npc"))
            {
                if (!Char.IsNumber(Img.Text[0]))
                    continue;
                Row += 1;
                var ID = Img.ImgID();
                var Entry = Wz.GetNode("Npc/" + Img.Text);
                if (Entry.GetNode("stand/0") != null)
                    Icon = Entry.GetBmp("stand/0");
                if (Entry.GetNode("default/0") != null)
                    Icon = Entry.GetBmp("default/0");
                string Name = "";
                if (Wz.HasNode("String/Npc.img/" + ID.IntID()))
                    Name = Wz.GetNodeA("String/Npc.img/" + ID.IntID()).GetStr("name");
                DumpData2(Wz.GetNode("String/Npc.img/" + ID.IDString()));
                Grid.Rows.Add(ID, Icon, Name, "");
                var Link = Wz.GetNode("Npc/" + Img.Text + "/info/link");
                if (Link != null)
                    Links.Add((Link.Value.ToString() + ".img", Row));
            }
            Wz_Node Child = null;
            for (int i = 0; i < Links.Count; i++)
            {
                if (Wz.GetNode("Npc/" + Links[i].Item1 + "/stand/0") != null)
                    Child = Wz.GetNode("Npc/" + Links[i].Item1 + "/stand/0");
                else if (Wz.GetNode("Npc/" + Links[i].Item1 + "/default/0") != null)
                    Child = Wz.GetNode("Npc/" + Links[i].Item1 + "/default/0");
                Grid[1, Links[i].Item2].Value = Child.ExtractPng();
            }
            for (int i = 0; i < RowList.Count; i++)
                Grid[3, i].Value = RowList[i];
            ColList.Clear();
            RowList.Clear();
            foreach (var Img in Wz.GetNodes("Npc"))
            {
                DumpData2(Wz.GetNode("Npc/" + Img.Text + "/info"));
            }
            for (int i = 0; i < RowList.Count; i++)
            {
                Grid[4, i].Value = RowList[i];
            }
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            SetToolTipEvent(WzType.Npc);

        }
        void LoadMorph()
        {
            var Dict = new Dictionary<string, (string, string)>();
            var imgs = new List<string>();
            string Desc = "", Name = "";
            foreach (var Iter in Wz.GetNodes("String/Consume.img"))
            {
                Desc = Iter.GetStr("desc");
                Name = Iter.GetStr("name");
                Dict.Add(Iter.Text, (Name, Desc));
            }
            foreach (var img in Wz.GetNodes("Morph"))
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                imgs.Add(img.ImgID());
            }
            Bitmap Icon = null;
            Bitmap MorphPic = null;
            foreach (var Iter in Wz.GetNodes("Item/Consume/0221.img"))
            {
                DumpData2(Iter);
                var ID = Iter.Text;
                if (Dict.ContainsKey(Iter.Text.IDString()))
                {
                    Name = Dict[Iter.Text.IDString()].Item1;
                    Desc = Dict[Iter.Text.IDString()].Item2;
                }
                if (Iter.GetNode("info/icon") != null)
                    Icon = Iter.GetNode("info/icon").ExtractPng();
                var MorphID = Iter.GetStr("spec/morph").PadLeft(4, '0');
                if (imgs.Contains(MorphID))
                {
                    if (Wz.GetNode("Morph/" + MorphID + ".img/walk/0") != null)
                        MorphPic = Wz.GetBmp("Morph/" + MorphID + ".img/walk/0");
                }
                Grid.Rows.Add(ID, Icon, MorphID, MorphPic, Name, Desc, "");
            }

            for (int i = 0; i < RowList.Count; i++)
                Grid[6, i].Value = RowList[i];
        }

        void LoadFamiliar()
        {

            if (Wz.GetNode("Character/Familiar") == null)
            {
                MessageBox.Show("Familiar  not found");
                return;
            }
            Wz_Node CardEntry;
            Bitmap MobPic = null, Icon = null;
            string CardID = "", MobID = "";
            foreach (var img in Wz.GetNodes("Character/Familiar"))
            {
                if (img.Text.RightStr(4) != ".img")
                    continue;
                var ID = img.ImgID();
                var Entry = Wz.GetNode("Character/Familiar/" + img.Text);
                if (Entry.GetNode("info/MobID") != null)
                    MobID = Entry.GetNode("info/MobID").Value.ToString().PadLeft(7, '0');
                else if (Wz.GetNode("Etc/FamiliarInfo.img") != null)
                    MobID = Wz.GetNode("Etc/FamiliarInfo.img/" + ID).GetStr("mob", "100100").PadLeft(7, '0');

                DumpData2(Entry.GetNode("info"));

                if (Wz.GetNode("Mob/" + MobID + ".img") != null)
                {
                    if (Wz.GetNode("Mob/" + MobID + ".img/stand/0") != null)
                        MobPic = Wz.GetBmp("Mob/" + MobID + ".img/stand/0");
                    else if (Wz.GetNode("Mob/" + MobID + ".img/fly/0") != null)
                        MobPic = Wz.GetBmp("Mob/" + MobID + ".img/fly/0");
                }

                string SkillID = "";
                if (Entry.GetNode("info/skill") != null)
                    SkillID = Entry.GetNode("info/skill").GetStr("id");
                string SkillName = "", SkillDesc = "";
                if (Wz.GetNode("String/FamiliarSkill.img") != null)
                {
                    if (Wz.GetNode("String/FamiliarSkill.img/skill/" + SkillID) != null)
                    {
                        SkillName = SkillID + ":" + Wz.GetNode("String/FamiliarSkill.img/skill/" + SkillID).GetStr("name");
                        SkillDesc = Wz.GetNode("String/FamiliarSkill.img/skill/" + SkillID).GetStr("desc");
                    }
                }

                else if (Wz.GetNode("String/Familiar.img") != null)
                {
                    if (Wz.GetNode("String/Familiar.img/skill/" + SkillID) != null)
                    {
                        SkillName = SkillID + ":" + Wz.GetNode("String/Familiar.img/skill/" + SkillID).GetStr("name");
                        SkillDesc = Wz.GetNode("String/Familiar.img/skill/" + SkillID).GetStr("desc");
                    }
                }
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

                if (Wz.GetNode("Item/Consume/0287.img/0" + CardID) != null)
                {
                    CardEntry = Wz.GetNode("Item/Consume/0287.img/0" + CardID);
                    if (CardEntry.GetNode("info/icon") != null)
                        Icon = CardEntry.GetNode("info/icon").ExtractPng();
                }
                else if (Wz.GetNode("Item/Consume/0238.img/0" + CardID) != null)
                {
                    CardEntry = Wz.GetNode("Item/Consume/0238.img/0" + CardID);
                    if (CardEntry.GetNode("info/iconRaw") != null)
                        Icon = CardEntry.GetNode("info/iconRaw").ExtractPng();
                }
                else
                    Icon = null;

                var CardName = Wz.GetNode("String/Consume.img/" + CardID).GetStr("name");
                Grid.Rows.Add(ID, MobPic, "", SkillName, SkillDesc, CardID, Icon, CardName);
            }

            for (int i = 0; i < RowList.Count; i++)
                Grid[2, i].Value = RowList[i];
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }
        void LoadDamageSkin()
        {
            Bitmap Icon = null, Sample = null;
            Bitmap bb = new Bitmap(20, 20);

            foreach (var Iter in Wz.GetNodes("String/Consume.img"))
            {
                var Name = Iter.GetStr("name");
                if ((Name.Contains("字型")) || (Name.Contains("ジスキン")) || (Name.Contains("스킨")) || (Name.Contains
                  ("Damage Skin")) || (Name.Contains("字型")) || (Name.Contains("伤害皮肤")))
                {
                    var ID = "0" + Iter.Text;
                    string[] imgs = new string[] { "0243.img", "0263.img" };
                    for (int i = 0; i <= 1; i++)
                    {
                        var Entry = Wz.GetNode("Item/Consume/" + imgs[i] + "/0" + Iter.Text + "/info/icon");
                        if (Entry != null)
                            Icon = Entry.ExtractPng();

                        var Entry2 = Wz.GetNode("Item/Consume/" + imgs[i] + "/0" + Iter.Text + "/info/sample");
                        if (Entry2 != null)
                        {
                            if (Entry2.GetNode("0") != null)
                                Sample = Entry2.GetNode("0").ExtractPng();
                            else
                                Sample = Entry2.ExtractPng();
                        }

                    }

                    var Desc = Iter.GetStr("desc");

                    Grid.Rows.Add(ID, Icon, Name, Sample, Desc);
                }
            }
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            SetToolTipEvent(WzType.Item);
        }
        void LoadReactor()
        {
            var Row = -1;
            var Links = new List<(string, int)>();
            Bitmap Icon = null;

            foreach (var img in Wz.GetNodes("Reactor"))
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                Row += 1;
                DumpData2(Wz.GetNode("Reactor/" + img.Text + "/info"));
                var ID = img.ImgID();
                var Entry = Wz.GetNode("Reactor/" + img.Text + "/0/0");
                if (Entry != null)
                    Icon = Entry.ExtractPng();
                Entry = Wz.GetNode("Reactor/" + img.Text + "/info/link");
                if (Entry != null)
                    Links.Add((Entry.Value.ToString(), Row));
                Grid.Rows.Add(ID, Icon, "");
            }

            Wz_Node Child = null;
            for (int i = 0; i < Links.Count; i++)
            {
                if (Wz.GetNode("Reactor/" + Links[i].Item1 + ".img/0/0") != null)
                    Child = Wz.GetNode("Reactor/" + Links[i].Item1 + ".img/0/0");
                Grid[1, Links[i].Item2].Value = Child.ExtractPng();
            }
            for (int i = 0; i < RowList.Count; i++)
            {
                Grid[2, i].Value = RowList[i];
            }
            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

        }

        void LoadMusic()
        {
            foreach (var Iter in MainForm.TreeNode.Nodes)
            {
                if (LeftStr(Iter.Text, 5) == "Sound")
                {
                    foreach (var Iter2 in Iter.Nodes)
                    {
                        if (LeftStr(Iter2.Text, 3) == "Bgm" || LeftStr(Iter2.Text, 4) == "PL_3" || LeftStr(Iter2.Text, 4) == "PL_B" || LeftStr(Iter2.Text, 4) == "PL_C" || LeftStr(Iter2.Text, 4) == "PL_M")
                        {
                            foreach (var Iter3 in Wz.GetNode(Iter2.FullPathToFile2()).Nodes)
                                Grid.Rows.Add(Iter3.GetPath());
                        }
                    }
                }
            }

            Grid.Sort(Grid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }
        DataViewer[] Grids = new DataViewer[39];
        DataViewer[] SearchGrids = new DataViewer[39];

        void LoadBIN()
        {
            var BinFile = System.Environment.CurrentDirectory + "\\" + Grid.Parent.Name + ".BIN";
            if (System.IO.File.Exists(BinFile))
            {
                for (int i = 0; i <= 38; i++)
                {
                    Grids[i].Rows.Clear();
                    Grids[i].Refresh();
                    var Graphic = Grids[i].CreateGraphics();
                    var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    Graphic.DrawString("Loading...", Font, Brushes.Black, 300, 200);
                }
                Grid.LoadBin(BinFile);

            }
            else
                MessageBox.Show(Grid.Parent.Name + ".BIN" + " not found");
        }




        Wz_Node find(string c)
        {
            return null;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Load From BIN")
            {


                LoadButton.Visible = false;
                LoadBIN();
            }
            else
            {


                LoadButton.Visible = true;
                Grid.Rows.Clear();
                Grid.Refresh();
            }

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (this.mapRenderGame2 != null)
            {
                mapRenderGame2.form.Visible = false;
                //  this.Focus();
            }


            Row1 = -1;
            tabIndex = tabControl1.SelectedIndex;
            Grid = Grids[tabIndex];
            SearchGrid = SearchGrids[tabIndex];
            Grid.Visible = true;
            SearchGrid.Visible = false;
            SearchBox.Clear();
            comboBox4.SelectedIndex = tabControl1.SelectedIndex;

            switch (Grid.DefaultGridType)
            {
                case GridType.Normal:
                case GridType.Item:
                    Grid.RowTemplate.Height = 40;
                    break;
                case GridType.Map:
                    Grid.RowTemplate.Height = 60;
                    break;

                case GridType.Mob:
                case GridType.Reactor:
                    Grid.RowTemplate.Height = 80;
                    break;

                case GridType.Skill:
                    Grid.RowTemplate.Height = 60;
                    break;

                case GridType.Npc:
                case GridType.Morph:
                case GridType.Familiar:
                    Grid.RowTemplate.Height = 70;
                    break;

                case GridType.DamageSkin:
                    Grid.RowTemplate.Height = 50;
                    break;
            }

            if (comboBox1.Text == "Load From BIN")
                LoadBIN();

            SetGrid();


        }
        string GetWzFileName()
        {

            switch (tabIndex)
            {
                case 0:
                case 1:
                case 2:
                case 25:
                case 26:
                case 35:
                case 36:
                    return "Item.wz";
                    break;

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
                case 16:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 34:
                    return "Character.wz";
                    break;
                case 17:
                case 18:
                case 19:
                    return "Map.wz";
                    break;
                case 20:
                    return "Mob.wz";
                    break;
                case 21:
                    return "Mob001.wz";
                    break;
                case 22:
                    return "Mob2.wz";
                    break;
                case 23:
                    return "Skill.wz";
                    break;

                case 24:
                    return "Npc.wz";
                    break;
                case 33:
                    return "Morph.wz";
                    break;
                case 37:
                    return "Reactor.wz";
                    break;
            }
            return "";
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

            void SetGridType(GridType GridType)
            {
                Grids[tabIndex] = new DataViewer(GridType);
                if (ChangeHeight)
                    Grids[tabIndex].RowTemplate.Height = int.Parse(comboBox3.Text);
                Grids[tabIndex].Parent = tabControl1.TabPages[tabIndex];
                Grid = Grids[tabIndex];
                SearchGrids[tabIndex] = new DataViewer(GridType.Item);
                SearchGrids[tabIndex].Parent = tabControl1.TabPages[tabIndex];
                SearchGrid = SearchGrids[tabIndex];
                SetGrid();
                Grid.Refresh();
                var Graphic = Grid.CreateGraphics();
                var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                Graphic.DrawString("Loading...", Font, Brushes.Black, 300, 200);
            }

            tabControl1_Selected(this, null);
            Grid.Dispose();
            SearchGrid.Dispose();



            // Grid.Visible = true;
            // SetGrid();
            if (PluginManager.FindWz(Wz_Type.Base) == null)
            {
                MessageBox.Show("Base.wz not opened");
                return;
            }

            Row1 = -1;
            Grid.Rows.Clear();
            // SplashGrid.Refresh();
            RowList.Clear();
            ColList.Clear();

            switch (tabIndex)
            {
                case 0:
                case 1:
                case 2:
                case 25:
                case 26:
                case 36:
                    if (!Wz.HasNode("Item/" + tabControl1.TabPages[tabIndex].Name))
                    {
                        MessageBox.Show("Item/"+ tabControl1.TabPages[tabIndex].Name+"   not found");
                        return;
                    }
                    SetGridType(GridType.Item);
                    LoadItem();
                    break;

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
                case 16:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                    if (!Wz.HasNode("Character/" + tabControl1.TabPages[tabIndex].Name))
                    {
                        MessageBox.Show("tabControl1.TabPages[tabIndex].Name" + "   not found");
                        return;
                    }
                    SetGridType(GridType.Normal);
                    LoadCharacter();
                    break;
                case 17:
                    SetGridType(GridType.Map);
                    LoadMap(1);
                    break;

                case 18:
                    SetGridType(GridType.Map);
                    LoadMap(2);
                    break;
                case 19:
                    SetGridType(GridType.Map);
                    LoadMap(3);
                    break;
                case 20:
                    SetGridType(GridType.Mob);
                    LoadMob(1);
                    break;
                case 21:
                    SetGridType(GridType.Mob);
                    LoadMob(2);
                    break;
                case 22:
                    SetGridType(GridType.Mob);
                    LoadMob(3);
                    break;
                case 23:
                    SetGridType(GridType.Skill);
                    LoadSkill();
                    break;

                case 24:
                    SetGridType(GridType.Npc);
                    LoadNpc();
                    break;
                case 33:
                    SetGridType(GridType.Morph);
                    LoadMorph();
                    break;
                case 34:
                    if (!Wz.HasNode("Character/" + tabControl1.TabPages[tabIndex].Name))
                    {
                        MessageBox.Show(tabControl1.TabPages[tabIndex].Name + "   not found");
                        return;
                    }
                    SetGridType(GridType.Familiar);
                  
                    LoadFamiliar();
                    break;
                case 35:
                    if (!Wz.HasNode("Character/" + tabControl1.TabPages[tabIndex].Name))
                    {
                      //  MessageBox.Show("tabControl1.TabPages[tabIndex].Name" + "   not found");
                      //  return;
                    }
                    SetGridType(GridType.DamageSkin);
                    LoadDamageSkin();
                    break;
                case 37:
                    SetGridType(GridType.Reactor);
                    LoadReactor();
                    break;

                case 38:
                    SetGridType(GridType.Music);
                    LoadMusic();
                    break;


            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Grid.SaveBin(System.Environment.CurrentDirectory + "\\" + Grid.Parent.Name + ".BIN");
            MessageBox.Show("儲存 " + Grid.Parent.Name + ".BIN 完成");
        }
        string Trim(string s)
        {

            return s.Trim(' ');
        }
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            var SearchStr = Trim(SearchBox.Text);
            if (SearchStr == "")
            {
                Grid.Visible = true;
                SearchGrid.Visible = false;
            }
            else
            {
                SearchGrid.Rows.Clear();
                var Row = new DataGridViewRow();
                for (int i = 0; i < Grid.RowCount; i++)
                {
                    for (int j = 0; j < Grid.Columns.Count; j++)
                    {
                        if (Grid.Rows[i].Cells[j].Value is string)
                        {
                            if (Grid.Rows[i].Cells[j].Value.ToString().IndexOf(SearchStr, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Row = (DataGridViewRow)Grid.Rows[i].Clone();
                                for (int j2 = 0; j2 < Grid.Columns.Count; j2++)
                                    Row.Cells[j2].Value = Grid.Rows[i].Cells[j2].Value;
                                SearchGrid.Rows.Add(Row);
                                break;
                            }
                        }
                    }
                }
                Grid.Visible = false;
                SearchGrid.Visible = true;
                SearchGrid.Refresh();
            }

        }
        bool FileExists(string Name)
        {
            return System.IO.File.Exists(Name);

        }


        BassSoundPlayer soundPlayer;

        // Wz_Structure s;
        private void button1_Click(object sender, EventArgs e)
        {

        }
        string GetIDPath(string ID)
        {
            switch (tabIndex)
            {
                case 0:
                    return "Item/Cash/" + LeftStr(ID, 4) + ".img/" + ID;
                    break;
                case 1:
                    return "Item/Consume/" + LeftStr(ID, 4) + ".img/" + ID;
                    break;

                case 2:
                    return "Item/Special/0" + LeftStr(ID, 3) + ".img/" + ID;
                case 33:
                case 35:
                    return "Item/Consume/" + LeftStr(ID, 4) + ".img/" + ID;
                    break;

                case 3:
                    return "Character/Weapon/" + ID + ".img";
                    break;
                case 4:
                    return "Character/Cap/" + ID + ".img";
                    break;
                case 5:
                    return "Character/Coat/" + ID + ".img";
                    break;
                case 6:
                    return "Character/Longcoat/" + ID + ".img";
                    break;

                case 7:
                    return "Character/Pants/" + ID + ".img";
                    break;
                case 8:
                    return "Character/Shoes/" + ID + ".img";
                    break;
                case 9:
                    return "Character/Glove/" + ID + ".img";
                    break;
                case 10:
                    return "Character/Ring/" + ID + ".img";
                    break;

                case 11:
                    return "Character/Cape/" + ID + ".img";
                    break;

                case 12:
                    return "Character/Accessory/" + ID + ".img";
                    break;
                case 13:
                    return "Character/Shield/" + ID + ".img";
                    break;
                case 14:
                    return "Character/TamingMob/" + ID + ".img";
                    break;
                case 15:
                    return "Character/Hair/" + ID + ".img";
                    break;

                case 16:
                    return "Character/Face/" + ID + ".img";
                    break;
                case 20:
                case 21:
                case 22:
                    return "Mob/" + ID + ".img";
                    break;

                case 23:

                    var Left1 = LeftStr(ID, 1);
                    switch (Left1)
                    {
                        case "0":
                            return "Skill/000.img/skill/" + ID;
                        case "8":
                            return "Skill/" + (int.Parse(ID) / 100).ToString() + ".img/skill/" + ID;
                        default:
                            return "Skill/" + (int.Parse(ID) / 10000).ToString() + ".img/skill/" + ID;
                    }
                    break;
                case 24:
                    return "Npc/" + ID + ".img";
                    break;
                case 25:
                    return "Item/Pet/" + ID + ".img";
                    break;

                case 26:
                    //   if(Arc.ItemWz == null)
                    //     return null;
                    if (Wz.GetNode("Item/Install/03010.img") != null)
                    {
                        switch (LeftStr(ID, 5))
                        {
                            case "03015":
                                return "Item/Install/" + LeftStr(ID, 6) + ".img/" + ID;
                                break;
                            case "03010":
                            case "03011":
                            case "03012":
                            case "03013":
                            case "03014":
                            case "03016":
                            case "03017":
                            case "03018":
                                return "Item/Install/" + LeftStr(ID, 5) + ".img/" + ID;
                                break;
                            default:
                                return "Item/Install/" + LeftStr(ID, 4) + ".img/" + ID;
                                break;
                        }
                    }
                    else
                    {
                        return "Item/Install/" + LeftStr(ID, 4) + ".img/" + ID;
                    }
                    break;

                case 27:
                    return "Character/Android/" + ID + ".img";
                    break;
                case 28:
                    return "Character/Mechanic/" + ID + ".img";
                    break;

                case 29:
                    return "Character/PetEquip/" + ID + ".img";
                    break;

                case 30:
                    return "Character/Bits/" + ID + ".img";
                    break;

                case 31:
                    return "Character/MonsterBattle/" + ID + ".img";
                    break;

                case 32:
                    return "Character/Totem/" + ID + ".img";
                    break;
                case 34:
                    return "Character/Familiar/" + ID + ".img";
                    break;

                case 36:
                    return "Item/Etc/" + LeftStr(ID, 4) + ".img/" + ID;
                    break;
            }

            return null;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid.DefaultCellStyle.Font = new System.Drawing.Font("Arial", int.Parse(comboBox2.Text), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            SearchGrid.DefaultCellStyle.Font = new System.Drawing.Font("Arial", int.Parse(comboBox2.Text), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
        }

        bool ChangeHeight;
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeHeight = true;
            LoadButton_Click(sender, e);
            ChangeHeight = false;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            tabControl1.SelectedIndex = comboBox4.SelectedIndex;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // if(WindowState == FormWindowState.Minimized)
            //  ToolTip2.tooltipQuickView.Visible = false;

        }
        FrmMapRender2 mapRenderGame2;

        void ShowMap(Wz_Node MapImg)
        {
            Wz_Node node = MapImg;

            //  Wz_Node node = Context.SelectedNode1;
            if (node != null)
            {
                Wz_Image img = node.Value as Wz_Image;
                Wz_File wzFile = node.GetNodeWzFile();

                if (img != null && img.TryExtract())
                {
                    if (wzFile == null || wzFile.Type != Wz_Type.Map)
                    {
                        if (MessageBoxEx.Show("You did not select an IMG file from Map.wz.\r\nDo you want to continue?", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            goto exit;
                        }
                    }

                    StringLinker sl;// = this.Context.DefaultStringLinker;
                                    //if (!sl.HasValues) //生成默认stringLinker
                    {
                        sl = new StringLinker();
                        sl.Load(PluginManager.FindWz(Wz_Type.String).GetValueEx<Wz_File>(null), PluginManager.FindWz(Wz_Type.Item).GetValueEx<Wz_File>(null), PluginManager.FindWz(Wz_Type.Etc).GetValueEx<Wz_File>(null));
                    }

                    //开始绘制
                    Thread thread = new Thread(() =>
                    {
#if !DEBUG
                        try
                        {
#endif
#if MapRenderV1
                        if (sender == btnItemMapRender)
                        {
                            if (this.mapRenderGame1 != null)
                            {
                                return;
                            }
                            this.mapRenderGame1 = new FrmMapRender(img) { StringLinker = sl };
                            try
                            {
                                using (this.mapRenderGame1)
                                {
                                    this.mapRenderGame1.Run();
                                }
                            }
                            finally
                            {
                                this.mapRenderGame1 = null;
                            }
                        }
                        else
#endif
                            {
                                if (this.mapRenderGame2 != null)
                                {
                                    // post message to the opening game.
                                    this.mapRenderGame2.LoadMap(img);
                                    this.mapRenderGame2.form.TopMost = true;
                                    this.mapRenderGame2.form.BringToFront();

                                    return;
                                }
                                else
                                {
                                    this.mapRenderGame2 = new FrmMapRender2() { StringLinker = sl };
                                    this.mapRenderGame2.form.TopMost = true;

                                    this.mapRenderGame2.Window.Title = "MapRender ";
                                    this.mapRenderGame2.LoadMap(img);

                                    try
                                    {
                                        using (this.mapRenderGame2)
                                        {
                                            this.mapRenderGame2.Run();
                                        }
                                    }
                                    finally
                                    {
                                        this.mapRenderGame2 = null;
                                    }
                                }
                            }
#if !DEBUG
                        }
                        catch (Exception ex)
                        {
                            PluginManager.LogError("MapRender", ex, "MapRender Error:");
                            MessageBoxEx.Show(ex.ToString(), "MapRender");
                        }
#endif
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();
                    goto exit;
                }
            }

            MessageBoxEx.Show("Select an IMG from Map.wz.", "Map Render");

        exit:
            return;
        }



        void CellClick(DataViewer DataGrid, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
                return;
            if (e.RowIndex >= Grid.RowCount)
                return;
            if (tabIndex == 37 || tabIndex == 34)
                return;

            if (e.RowIndex >= Grid.RowCount)
                return;
            string SelectID = "";
            if (DataGrid.Rows[e.RowIndex].Cells[0].Value is string)
            {
                SelectID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (tabIndex == 17 || tabIndex == 18 || tabIndex == 19)
                {
                    var imgNode = Wz.GetNode("Map/Map/Map" + LeftStr(SelectID, 1)).FindNodeByPath(SelectID + ".img");
                    if (this.mapRenderGame2 != null)
                    {
                        // this.mapRenderGame2.Dispose();
                        // this.mapRenderGame2 = null;
                    }
                    ShowMap(imgNode);
                    if (imgNode != null)
                        MainForm.ExpandTreeNode(imgNode);
                }
                else if (tabIndex == 38)
                {

                    this.mapRenderGame2.form.BringToFront();
                    Wz_Sound sound = null;
                    if (Wz.GetNode("Sound/" + SelectID) != null)
                        sound = (Wz_Sound)Wz.GetNode("Sound/" + SelectID).Value;
                    soundPlayer.UnLoad();
                    byte[] data = sound.ExtractSound();
                    if (data == null || data.Length <= 0)
                    {
                        return;
                    }
                    soundPlayer.PreLoad(data);
                    soundPlayer.Play();
                }
                else
                {
                    if (tabIndex == 38)
                        return;
                    MainForm.tooltipRef.Visible = true;
                    MainForm.tooltipRef.BringToFront();
                    var Node = PluginManager.FindWz(GetIDPath(SelectID));
                    if (Node != null)
                        MainForm.ExpandTreeNode(Node);
                }
            }
        }

        void GridScroll()
        {
            MainForm.tooltipRef.Visible = false;

        }
        void SetGrid()
        {
            //  Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.DefaultCellStyle.SelectionBackColor = Color.LightCyan;
            Grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            foreach (DataGridViewColumn column in Grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            Grid.EnableHeadersVisualStyles = false;
            Grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Grid.ColumnHeadersDefaultCellStyle.BackColor;
            Grid.RowHeadersVisible = false;
            Grid.Dock = DockStyle.Fill;
            Grid.ShowCellToolTips = false;
            //
            //SearchGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SearchGrid.DefaultCellStyle.SelectionBackColor = Color.LightCyan;
            SearchGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
            foreach (DataGridViewColumn column in SearchGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            SearchGrid.EnableHeadersVisualStyles = false;
            SearchGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Grid.ColumnHeadersDefaultCellStyle.BackColor;
            SearchGrid.RowHeadersVisible = false;
            SearchGrid.Dock = DockStyle.Fill;
            SearchGrid.ShowCellToolTips = false;


            Grid.CellClick += (s, e) =>
            {
                CellClick(Grid, e);

            };
            Grid.Scroll += (s, e) =>
            {
                GridScroll();

            };

            SearchGrid.CellClick += (s, e) =>
            {
                CellClick(SearchGrid, e);
            };
            SearchGrid.Scroll += (s, e) =>
            {
                GridScroll();
            };
            // Grid.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            // SearchGrid.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            Grid.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("Copy");
                    int currentMouseOverRow = Grid.HitTest(e.X, e.Y).RowIndex;
                    m.Show(Grid, new Point(e.X, e.Y));
                    if (Grid.GetCellCount(DataGridViewElementStates.Selected) > 0)
                    {
                        try
                        {
                            // Add the selection to the clipboard.
                            Clipboard.SetDataObject(Grid.GetClipboardContent());
                        }
                        catch (System.Runtime.InteropServices.ExternalException)
                        {
                            MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                        }
                    }
                }

            };
            //
            SearchGrid.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("Copy");
                    int currentMouseOverRow = SearchGrid.HitTest(e.X, e.Y).RowIndex;
                    m.Show(SearchGrid, new Point(e.X, e.Y));
                    if (SearchGrid.GetCellCount(DataGridViewElementStates.Selected) > 0)
                    {
                        try
                        {
                            // Add the selection to the clipboard.
                            Clipboard.SetDataObject(SearchGrid.GetClipboardContent());
                        }
                        catch (System.Runtime.InteropServices.ExternalException)
                        {
                            MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                        }
                    }
                }

            };
            Grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
        }

        public void SetToolTipEvent(WzType WzType)
        {

            Grid.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    Grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
                }
                this.Refresh();
                string ID = Grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                Wz_Node Node = Wz.GetNodeByID(ID, WzType);
                MainForm.Instance.quickView2(Node);
                MainForm.tooltipRef.BringToFront();

                // MainForm.Instance.ToolTipView.Owner = Form;

            };

            Grid.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    Grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }

            };

            SearchGrid.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                string ID = "";
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    SearchGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
                    ID = SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
                this.Refresh();

                Wz_Node Node = Wz.GetNodeByID(ID, WzType);

                MainForm.Instance.quickView2(Node);
                MainForm.tooltipRef.BringToFront();

            };

            SearchGrid.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    SearchGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += (s, e1) =>
           {
               this.Hide();
               e1.Cancel = true;
           };
            this.TransparencyKey = System.Drawing.Color.LightGray;


            ColList = new List<string>();
            RowList = new List<string>();
            RowList1 = new Dictionary<int, List<string>>();
            soundPlayer = new BassSoundPlayer();
            if (!soundPlayer.Init())
            {
                //  Un4seen.Bass.BASSError error = soundPlayer.GetLastError();
                //  MessageBox.Show("Bass初始化失败！\r\n\r\nerrorCode : " + (int)error + "(" + error + ")","虫子");
            }


            for (int i = 0; i <= 38; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 25:
                    case 26:
                    case 36:
                        Grids[i] = new DataViewer(GridType.Item);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Item);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;

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
                    case 16:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                        Grids[i] = new DataViewer(GridType.Normal);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Normal);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;

                    case 17:
                    case 18:
                    case 19:
                        Grids[i] = new DataViewer(GridType.Map);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Map);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 20:
                    case 21:
                    case 22:
                        Grids[i] = new DataViewer(GridType.Mob);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Mob);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 23:

                        Grids[i] = new DataViewer(GridType.Skill);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Skill);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 24:
                        Grids[i] = new DataViewer(GridType.Npc);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Npc);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 33:
                        Grids[i] = new DataViewer(GridType.Morph);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Morph);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 34:
                        Grids[i] = new DataViewer(GridType.Familiar);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Familiar);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 35:
                        Grids[i] = new DataViewer(GridType.DamageSkin);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.DamageSkin);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                    case 37:
                        Grids[i] = new DataViewer(GridType.Reactor);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Reactor);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;

                    case 38:
                        Grids[i] = new DataViewer(GridType.Music);
                        Grids[i].Parent = tabControl1.TabPages[i];
                        SearchGrids[i] = new DataViewer(GridType.Music);
                        SearchGrids[i].Parent = tabControl1.TabPages[i];
                        break;
                }

            }

            Grid = Grids[0];
            SearchGrid = SearchGrids[0];
            SetGrid();
            Grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            MainForm.tooltipRef.Visible = true;


        }

        private void DB2Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.tooltipRef.Visible = false;
        }
    }




}






