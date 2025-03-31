using DevComponents.DotNetBar;
using Manina.Windows.Forms;
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
using WzComparerR2.Common;
using WzComparerR2.PluginBase;
using System.Threading;
using WzComparerR2.MapRender;
using MapleNecrocer;
using DevComponents.AdvTree;

namespace WzComparerR2;

public partial class IconForm : Form
{
    public IconForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static IconForm Instance;
    bool[] HasLoaded = new bool[34];
    ImageListView[] IconGrids = new ImageListView[34];
    int Index;
    private void IconForm_Load(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        for (int i = 0; i <= 33; i++)
        {
            IconGrids[i] = new ImageListView();
            IconGrids[i].Parent = panel1;
            IconGrids[i].Dock = DockStyle.Fill;
            IconGrids[i].BackColor = SystemColors.Window;
            IconGrids[i].Colors.BackColor = SystemColors.ButtonFace;
            IconGrids[i].Colors.SelectedBorderColor = Color.Red;
            IconGrids[i].BorderStyle = BorderStyle.None;

            IconGrids[i].ThumbnailSize = new System.Drawing.Size(32, 32);
            switch (i)
            {
                case 14:
                    IconGrids[i].ThumbnailSize = new System.Drawing.Size(50, 50);
                    break;
                case 16:
                    IconGrids[i].ThumbnailSize = new System.Drawing.Size(100, 100);
                    break;
                case 17:
                case 19:
                    IconGrids[i].ThumbnailSize = new System.Drawing.Size(80, 80);
                    break;
                case 28:
                    IconGrids[i].ThumbnailSize = new System.Drawing.Size(80, 80);
                    break;
                case 32:
                    IconGrids[i].ThumbnailSize = new System.Drawing.Size(80, 80);
                    break;
            }

            Wz_Node Node = null;

            IconGrids[i].ItemClick += (o, e) =>
            {
                MainForm.ExpandTreeNode(Node);
            };

            IconGrids[i].ItemHover += (o, e) =>
            {
                if (e.Item == null) return;
                switch (Index)
                {
                    case 0:
                    case 1:
                    case 21:
                    case 31:
                    case 33:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Item);
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
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Character);
                        break;
                    case 16:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Map);
                        break;

                    case 17:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Mob);
                        break;
                    case 18:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Skill);
                        break;
                    case 19:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Npc);
                        break;

                    case 32:
                        Node = Wz.GetNodeByID(e.Item.FileName, WzType.Reactor);
                        break;
                }


                MainForm.Instance.quickView2(Node);
                MainForm.tooltipRef.BringToFront();
            };
        }
    }

    void LoadItem(string ItemDir)
    {
        foreach (var img in Wz.GetNode("Item/" + ItemDir).Nodes)
        {
            if (!Char.IsNumber(img.Text[0]))
                continue;
            if (ItemDir == "Pet")
            {
                if (Wz.HasNode("Item/Pet/" + img.Text + "/info/iconD"))
                    IconGrids[Index].Items.Add(img.ImgID(), Wz.GetBmp("Item/Pet/" + img.Text + "/info/iconD"));
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
                        string ID = Iter.Text.IDString();
                        Bitmap Icon = null;
                        if (Iter.GetNode("icon") != null)
                            Icon = Iter.GetBmp("icon");
                        IconGrids[Index].Items.Add(ID, Icon);
                    }
                }
                CashPackages.Clear();
            }

            else
            {
                foreach (var Iter in Wz.GetNode("Item/" + ItemDir + "/" + img.Text).Nodes)
                {
                    if (Iter.HasNode("info/icon"))
                        IconGrids[Index].Items.Add(Iter.Text, Iter.GetBmp("info/icon"));
                }
            }
        }
    }

    void LoadCharacter(string Dir)
    {
        foreach (var img in Wz.GetNode("Character/" + Dir).Nodes)
        {
            if (!Char.IsNumber(img.Text[0]))
                continue;
            var ID = img.ImgID();
            switch (Dir)
            {
                case "Hair":
                    if (img.HasNode("default/hairOverHead"))
                        IconGrids[Index].Items.Add(img.ImgID(), img.GetBmp("default/hairOverHead"));

                    break;
                case "Face":
                    if (img.HasNode("default/face"))
                        IconGrids[Index].Items.Add(img.ImgID(), img.GetBmp("default/face"));

                    break;
                default:
                    if (img.HasNode("info/icon"))
                        IconGrids[Index].Items.Add(img.ImgID(), img.GetBmp("info/icon"));
                    break;
            }
        }
    }




    void LoadMap()
    {
        var Links = new List<(string, string)>();
        foreach (var Dir in Wz.GetNode("Map/Map").Nodes)
        {
            if (Dir.Text.LeftStr(3) != "Map")
                continue;
            foreach (var img in Dir.Nodes)
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                var ID = img.ImgID();
                if (img.HasNode("miniMap/canvas"))
                    IconGrids[Index].Items.Add(img.ImgID(), img.GetBmp("miniMap/canvas"));

                var Link = img.GetNode("info/link");
                if (Link != null)
                    Links.Add(("Map" + (Link.Value.ToString().LeftStr(1)) + "/" + Link.Value.ToString() + ".img", ID));
            }
        }
        for (int i = 0; i < Links.Count; i++)
        {
            var Child = Wz.GetBmp("Map/Map/" + Links[i].Item1 + "/miniMap/canvas");
            if (Child != null)
                IconGrids[Index].Items.Add(Links[i].Item2, Child);
        }
    }

    void LoadMob()
    {
        var Links = new List<(string, string)>();
        Wz_Node Child = null;
        foreach (var Iter in Wz.GetNode("String/Mob.img").Nodes)
        {
            var ID = Iter.Text.PadLeft(7, '0');
            if (!Wz.HasNode("Mob/" + ID + ".img"))
                continue;

            if (Wz.HasNode("Mob/" + ID + ".img/info/link"))
            {
                Links.Add((Wz.GetNode("Mob/" + ID + ".img/info/link").Value.ToString(), ID));
                continue;
            }

            if (Wz.HasNode("Mob/" + ID + ".img/stand/0"))
                Child = Wz.GetNode("Mob/" + ID + ".img/stand/0");
            else if (Wz.HasNode("Mob/" + ID + ".img/fly/0"))
                Child = Wz.GetNode("Mob/" + ID + ".img/fly/0");
            if (Child != null)
                IconGrids[Index].Items.Add(ID, Child.ExtractPng());
        }

        for (int i = 0; i < Links.Count; i++)
        {
            if (Wz.HasNode("Mob/" + Links[i].Item1 + ".img/stand/0"))
                Child = Wz.GetNode("Mob/" + Links[i].Item1 + ".img/stand/0");
            else if (Wz.HasNode("Mob/" + Links[i].Item1 + ".img/fly/0"))
                Child = Wz.GetNode("Mob/" + Links[i].Item1 + ".img/fly/0");
            IconGrids[Index].Items.Add(Links[i].Item2, Child.ExtractPng());
        }
    }

    void LoadSkill()
    {
        Wz_Node Child;
        if (MainForm.HasSkill001)
        {
            foreach (var Iter in Wz.GetNode("String/Skill.img").Nodes)
            {
                var ID = Iter.Text;
                //  if(GetNode(GetIDPath(ID)) == null)
                //   continue;
                if (Wz.HasNode("Skill/" + ID + ".img/info/icon"))
                    IconGrids[Index].Items.Add(ID, Wz.GetBmp("Skill/" + ID + ".img/Info/icon"));
                Child = Wz.GetNode(Wz.GetNodeByID(ID, WzType.Skill) + "/icon");
                if (Child != null)
                    IconGrids[Index].Items.Add(ID, Child.ExtractPng());
            }
        }
        else
        {
            foreach (var img in Wz.GetNodes("Skill"))
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                var BookID = img.ImgID();
                //if (Wz.HasNode("Skill/" + img.Text + "/info/icon"))
                // IconGrids[Index].Items.Add(BookID, Wz.GetBmp("Skill/" + img.Text + "/info/icon"));
                foreach (var ID in Wz.GetNodes("Skill/" + img.Text + "/skill"))
                {
                    if (ID.HasNode("icon"))
                        IconGrids[Index].Items.Add(ID.Text, ID.GetBmp("icon"));
                }
            }
        }
    }

    void LoadNpc()
    {
        var Links = new List<(string, string)>();
        Bitmap Icon = null;
        foreach (var Img in Wz.GetNode("Npc").Nodes)
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            var ID = Img.ImgID();
            var Link = Wz.GetNode("Npc/" + Img.Text + "/info/link");
            if (Link != null)
            {
                Links.Add((Link.Value.ToString() + ".img", ID));
                continue;
            }
            var Entry = Wz.GetNode("Npc/" + Img.Text);
            if (Entry.HasNode("stand/0"))
                Icon = Entry.GetBmp("stand/0");
            if (Entry.HasNode("default/0"))
                Icon = Entry.GetBmp("default/0");
            IconGrids[Index].Items.Add(ID, Icon);
        }

        Wz_Node Child = null;
        for (int i = 0; i < Links.Count; i++)
        {
            if (Wz.HasNode("Npc/" + Links[i].Item1 + "/stand/0"))
                Child = Wz.GetNode("Npc/" + Links[i].Item1 + "/stand/0");
            else if (Wz.HasNode("Npc/" + Links[i].Item1 + "/default/0"))
                Child = Wz.GetNode("Npc/" + Links[i].Item1 + "/default/0");
            IconGrids[Index].Items.Add(Links[i].Item2, Child.ExtractPng());
        }
    }

    void LoadMorph()
    {
        foreach (var Img in Wz.GetNode("Morph").Nodes)
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            var ID = Img.ImgID();
            Bitmap MorphPic = null;
            if (Wz.HasNode("Morph/" + ID + ".img/walk/0"))
                MorphPic = Wz.GetBmp("Morph/" + ID + ".img/walk/0");
            if (MorphPic != null)
                IconGrids[Index].Items.Add(ID, MorphPic);
        }
    }

    void LoadFamiliar()
    {
        if (Wz.GetNode("Character/Familiar") == null)
        {
            MessageBox.Show("Familiar  not found");
            return;
        }
        Wz_Node CardEntry;
        Bitmap Icon = null;
        string CardID = "";
        foreach (var img in Wz.GetNode("Character/Familiar").Nodes)
        {
            if (!Char.IsNumber(img.Text[0]))
                continue;
            var ID = img.ImgID();
            var Entry = Wz.GetNode("Character/Familiar/" + img.Text);

            if (Wz.GetNode("Etc/FamiliarInfo.img") != null)
            {
                if (Wz.GetNode("Etc/FamiliarInfo.img/" + ID) != null)
                    CardID = Wz.GetNode("Etc/FamiliarInfo.img/" + ID).GetValue2("consume", "");
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

            var CardName = Wz.GetNode("String/Consume.img/" + CardID).GetValue2("name", "");
            IconGrids[Index].Items.Add("0" + CardID, Icon);

        }
    }

    void LoadDamageSkin()
    {
        Bitmap Icon = null;
        foreach (var Iter in Wz.GetNode("String/Consume.img").Nodes)
        {
            var Name = Iter.GetValue2("name", "");
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
                }
                IconGrids[Index].Items.Add(ID, Icon);

            }
        }
    }

    void LoadReactor()
    {
        var Links = new List<(string, string)>();
        Bitmap Icon = null;
        foreach (var img in Wz.GetNode("Reactor").Nodes)
        {
            if (!Char.IsNumber(img.Text[0]))
                continue;
            var ID = img.ImgID();
            var Link = Wz.GetNode("Reactor/" + ID + "/info/link");
            if (Link != null)
            {
                Links.Add((Link.Value.ToString() + ".img", ID));
                continue;
            }
            var Entry = Wz.GetNode("Reactor/" + img.Text + "/0/0");
            if (Entry != null)
                Icon = Entry.ExtractPng();
            IconGrids[Index].Items.Add(ID, Icon);
        }

        Wz_Node Child = null;
        for (int i = 0; i < Links.Count; i++)
        {
            if (Wz.GetNode("Reactor/" + Links[i].Item1 + ".img/0/0") != null)
                Child = Wz.GetNode("Reactor/" + Links[i].Item1 + ".img/0/0");
            IconGrids[Index].Items.Add(Links[i].Item2, Child.ExtractPng());
        }
    }


    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PluginManager.FindWz(Wz_Type.Base) == null)
        {
            MessageBox.Show("Base.wz not open");
            return;
        }

        Index = listBox1.SelectedIndex;

        for (int i = 0; i <= 33; i++)
            IconGrids[i].Visible = false;
        IconGrids[Index].Visible = true;

        switch (Index)
        {
            case 0:
                if (!Wz.HasNode("Item/Cash"))
                {
                    MessageBox.Show("Item/Cash not found");
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
                    MessageBox.Show("Character/Mechanic not found");
                    return;
                }
                break;
            case 24:
                if (!Wz.HasNode("Character/PetEquip"))
                {
                    MessageBox.Show("Character/PetEquip not found");
                    return;
                }
                break;
            case 25:
                if (!Wz.HasNode("Character/Bits"))
                {
                    MessageBox.Show("Character/Bits not found");
                    return;
                }
                break;
            case 26:
                if (!Wz.HasNode("Character/MonsterBattle.img"))
                {
                    MessageBox.Show("Character/MonsterBattle not found");
                    return;
                }
                break;
            case 27:
                if (!Wz.HasNode("Character/Totem"))
                {
                    MessageBox.Show("Character/Totem not found");
                    return;
                }
                break;
        }


        if (!HasLoaded[Index])
        {

            var Graphic = IconGrids[Index].CreateGraphics();
            var Font = new System.Drawing.Font("Arial", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 250, 200);

            Win32.SendMessage(IconGrids[Index].Handle, false);

            switch (Index)
            {
                case 0:
                    LoadItem("Cash");
                    break;
                case 1:
                    LoadItem("Consume");
                    break;
                case 2:
                    LoadCharacter("Weapon");
                    break;
                case 3:
                    LoadCharacter("Cap");
                    break;
                case 4:
                    LoadCharacter("Coat");
                    break;
                case 5:
                    LoadCharacter("Longcoat");
                    break;
                case 6:
                    LoadCharacter("Pants");
                    break;
                case 7:
                    LoadCharacter("Shoes");
                    break;
                case 8:
                    LoadCharacter("Glove");
                    break;
                case 9:
                    LoadCharacter("Ring");
                    break;
                case 10:
                    LoadCharacter("Cape");
                    break;
                case 11:
                    LoadCharacter("Accessory");
                    break;
                case 12:
                    LoadCharacter("Shield");
                    break;
                case 13:
                    LoadCharacter("TamingMob");
                    break;
                case 14:
                    LoadCharacter("Hair");
                    break;
                case 15:
                    LoadCharacter("Face");
                    break;
                case 16:
                    LoadMap();
                    break;
                case 17:
                    LoadMob();
                    break;
                case 18:
                    LoadSkill();
                    break;
                case 19:
                    LoadNpc();
                    break;
                case 20:
                    LoadItem("Pet");
                    break;
                case 21:
                    LoadItem("Install");
                    break;
                case 22:
                    LoadCharacter("Android");
                    break;
                case 23:
                    LoadCharacter("Mechanic");
                    break;
                case 24:
                    LoadCharacter("PetEquip");
                    break;
                case 25:
                    LoadCharacter("Bits");
                    break;
                case 26:
                    LoadCharacter("MonsterBattle");
                    break;
                case 27:
                    LoadCharacter("Totem");
                    break;
                case 28:
                    LoadMorph();
                    break;
                case 29:
                    LoadFamiliar();
                    break;
                case 30:
                    LoadDamageSkin();
                    break;
                case 31:
                    LoadItem("Etc");
                    break;
                case 32:
                    LoadReactor();
                    break;

                case 33:
                    LoadItem("Special");
                    break;
            }
            IconGrids[Index].Sort();
            Win32.SendMessage(IconGrids[Index].Handle, true);
            IconGrids[Index].Refresh();
            HasLoaded[Index] = true;
        }
    }

    private void IconForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.tooltipRef.Visible = false;
    }
}
