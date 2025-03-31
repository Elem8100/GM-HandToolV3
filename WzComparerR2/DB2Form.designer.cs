
namespace WzComparerR2
{
    partial class DB2Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DB2Form));
            panel1 = new System.Windows.Forms.Panel();
            SearchBox = new System.Windows.Forms.TextBox();
            comboBox2 = new System.Windows.Forms.ComboBox();
            comboBox4 = new System.Windows.Forms.ComboBox();
            comboBox3 = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            LoadButton = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            tabControl1 = new System.Windows.Forms.TabControl();
            Cash = new System.Windows.Forms.TabPage();
            Consume = new System.Windows.Forms.TabPage();
            Special = new System.Windows.Forms.TabPage();
            Weapon = new System.Windows.Forms.TabPage();
            Cap = new System.Windows.Forms.TabPage();
            Coat = new System.Windows.Forms.TabPage();
            Longcoat = new System.Windows.Forms.TabPage();
            Pants = new System.Windows.Forms.TabPage();
            Shoes = new System.Windows.Forms.TabPage();
            Glove = new System.Windows.Forms.TabPage();
            Ring = new System.Windows.Forms.TabPage();
            Cape = new System.Windows.Forms.TabPage();
            Accessory = new System.Windows.Forms.TabPage();
            Shield = new System.Windows.Forms.TabPage();
            TamingMob = new System.Windows.Forms.TabPage();
            Hair = new System.Windows.Forms.TabPage();
            Face = new System.Windows.Forms.TabPage();
            Map1 = new System.Windows.Forms.TabPage();
            Map2 = new System.Windows.Forms.TabPage();
            Map3 = new System.Windows.Forms.TabPage();
            Mob = new System.Windows.Forms.TabPage();
            Mob001 = new System.Windows.Forms.TabPage();
            Mob2 = new System.Windows.Forms.TabPage();
            Skill = new System.Windows.Forms.TabPage();
            Npc = new System.Windows.Forms.TabPage();
            Pet = new System.Windows.Forms.TabPage();
            Install = new System.Windows.Forms.TabPage();
            Android = new System.Windows.Forms.TabPage();
            Mechanic = new System.Windows.Forms.TabPage();
            PetEquip = new System.Windows.Forms.TabPage();
            Bits = new System.Windows.Forms.TabPage();
            MonsterBattle = new System.Windows.Forms.TabPage();
            Totem = new System.Windows.Forms.TabPage();
            Morph = new System.Windows.Forms.TabPage();
            Familiar = new System.Windows.Forms.TabPage();
            DamageSkin = new System.Windows.Forms.TabPage();
            Etc = new System.Windows.Forms.TabPage();
            Reactor = new System.Windows.Forms.TabPage();
            Music = new System.Windows.Forms.TabPage();
            imageList1 = new System.Windows.Forms.ImageList(components);
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.CausesValidation = false;
            panel1.Controls.Add(SearchBox);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(comboBox4);
            panel1.Controls.Add(comboBox3);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(SaveButton);
            panel1.Controls.Add(LoadButton);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label4);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Font = new System.Drawing.Font("微軟正黑體", 8F);
            panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1174, 35);
            panel1.TabIndex = 3;
            // 
            // SearchBox
            // 
            SearchBox.Font = new System.Drawing.Font("微軟正黑體", 9F);
            SearchBox.Location = new System.Drawing.Point(308, 3);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(160, 27);
            SearchBox.TabIndex = 7;
            SearchBox.TextChanged += SearchBox_TextChanged;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" });
            comboBox2.Location = new System.Drawing.Point(836, 4);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(50, 25);
            comboBox2.TabIndex = 10;
            comboBox2.Text = "14";
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox4
            // 
            comboBox4.DropDownHeight = 500;
            comboBox4.DropDownWidth = 120;
            comboBox4.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            comboBox4.FormattingEnabled = true;
            comboBox4.IntegralHeight = false;
            comboBox4.ItemHeight = 17;
            comboBox4.Items.AddRange(new object[] { "Cash", "Consume", "Package", "Weapon", "Cap", "Coat", "Longcoat", "Pants", "Shoes", "Glove", "Ring", "Cape", "Accessory", "Shield", "TamingMob", "Hair", "Face", "Map(1)", "Map(2)", "Map(3)", "Mob(1)", "Mob(2)", "Mob(3)", "Skill", "Npc", "Pet", "Install", "Android", "Mechanic", "PetEquip", "Bits", "MonaterBattle", "Totem", "Morph", "Familiar", "DamageSkin", "Etc", "Reactor", "Music" });
            comboBox4.Location = new System.Drawing.Point(12, 4);
            comboBox4.MaxDropDownItems = 15;
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new System.Drawing.Size(100, 25);
            comboBox4.TabIndex = 15;
            comboBox4.Text = "Cash";
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "15", "20", "25", "30", "35", "40", "45", "50", "55", "60", "65", "70", "75", "80", "85", "90", "95", "100", "105", "110", "120", "125", "130" });
            comboBox3.Location = new System.Drawing.Point(1009, 4);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(60, 25);
            comboBox3.TabIndex = 12;
            comboBox3.Text = "40";
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label3.Location = new System.Drawing.Point(769, 8);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 17);
            label3.TabIndex = 11;
            label3.Text = "Font Size";
            // 
            // SaveButton
            // 
            SaveButton.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            SaveButton.Location = new System.Drawing.Point(636, 3);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(102, 27);
            SaveButton.TabIndex = 8;
            SaveButton.Text = "Save BIN";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // LoadButton
            // 
            LoadButton.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            LoadButton.Location = new System.Drawing.Point(140, 3);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new System.Drawing.Size(90, 27);
            LoadButton.TabIndex = 6;
            LoadButton.Text = "Load WZ";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // comboBox1
            // 
            comboBox1.Font = new System.Drawing.Font("微軟正黑體", 9F);
            comboBox1.FormattingEnabled = true;
            comboBox1.ItemHeight = 19;
            comboBox1.Items.AddRange(new object[] { "Load From WZ", "Load From BIN" });
            comboBox1.Location = new System.Drawing.Point(490, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(130, 27);
            comboBox1.TabIndex = 0;
            comboBox1.Text = "Load From WZ";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label2.Location = new System.Drawing.Point(253, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(55, 17);
            label2.TabIndex = 9;
            label2.Text = "Search";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label4.Location = new System.Drawing.Point(927, 8);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(83, 17);
            label4.TabIndex = 13;
            label4.Text = "Row Height";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(13, 109);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(190, 115);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(Cash);
            tabControl1.Controls.Add(Consume);
            tabControl1.Controls.Add(Special);
            tabControl1.Controls.Add(Weapon);
            tabControl1.Controls.Add(Cap);
            tabControl1.Controls.Add(Coat);
            tabControl1.Controls.Add(Longcoat);
            tabControl1.Controls.Add(Pants);
            tabControl1.Controls.Add(Shoes);
            tabControl1.Controls.Add(Glove);
            tabControl1.Controls.Add(Ring);
            tabControl1.Controls.Add(Cape);
            tabControl1.Controls.Add(Accessory);
            tabControl1.Controls.Add(Shield);
            tabControl1.Controls.Add(TamingMob);
            tabControl1.Controls.Add(Hair);
            tabControl1.Controls.Add(Face);
            tabControl1.Controls.Add(Map1);
            tabControl1.Controls.Add(Map2);
            tabControl1.Controls.Add(Map3);
            tabControl1.Controls.Add(Mob);
            tabControl1.Controls.Add(Mob001);
            tabControl1.Controls.Add(Mob2);
            tabControl1.Controls.Add(Skill);
            tabControl1.Controls.Add(Npc);
            tabControl1.Controls.Add(Pet);
            tabControl1.Controls.Add(Install);
            tabControl1.Controls.Add(Android);
            tabControl1.Controls.Add(Mechanic);
            tabControl1.Controls.Add(PetEquip);
            tabControl1.Controls.Add(Bits);
            tabControl1.Controls.Add(MonsterBattle);
            tabControl1.Controls.Add(Totem);
            tabControl1.Controls.Add(Morph);
            tabControl1.Controls.Add(Familiar);
            tabControl1.Controls.Add(DamageSkin);
            tabControl1.Controls.Add(Etc);
            tabControl1.Controls.Add(Reactor);
            tabControl1.Controls.Add(Music);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            tabControl1.ImageList = imageList1;
            tabControl1.Location = new System.Drawing.Point(0, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1174, 506);
            tabControl1.TabIndex = 0;
            tabControl1.Selected += tabControl1_Selected;
            // 
            // Cash
            // 
            Cash.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            Cash.ImageIndex = 0;
            Cash.Location = new System.Drawing.Point(4, 35);
            Cash.Name = "Cash";
            Cash.Padding = new System.Windows.Forms.Padding(3);
            Cash.Size = new System.Drawing.Size(1166, 467);
            Cash.TabIndex = 0;
            Cash.Text = "Cash";
            Cash.UseVisualStyleBackColor = true;
            // 
            // Consume
            // 
            Consume.ImageIndex = 1;
            Consume.Location = new System.Drawing.Point(4, 35);
            Consume.Name = "Consume";
            Consume.Padding = new System.Windows.Forms.Padding(3);
            Consume.Size = new System.Drawing.Size(1166, 467);
            Consume.TabIndex = 1;
            Consume.Text = "Consume";
            Consume.UseVisualStyleBackColor = true;
            // 
            // Special
            // 
            Special.ImageIndex = 34;
            Special.Location = new System.Drawing.Point(4, 35);
            Special.Name = "Special";
            Special.Size = new System.Drawing.Size(1166, 467);
            Special.TabIndex = 40;
            Special.Text = "Package";
            Special.UseVisualStyleBackColor = true;
            // 
            // Weapon
            // 
            Weapon.ImageIndex = 2;
            Weapon.Location = new System.Drawing.Point(4, 35);
            Weapon.Name = "Weapon";
            Weapon.Padding = new System.Windows.Forms.Padding(3);
            Weapon.Size = new System.Drawing.Size(1166, 467);
            Weapon.TabIndex = 2;
            Weapon.Text = "Weapon";
            Weapon.UseVisualStyleBackColor = true;
            // 
            // Cap
            // 
            Cap.ImageIndex = 3;
            Cap.Location = new System.Drawing.Point(4, 35);
            Cap.Name = "Cap";
            Cap.Padding = new System.Windows.Forms.Padding(3);
            Cap.Size = new System.Drawing.Size(1166, 467);
            Cap.TabIndex = 3;
            Cap.Text = "Cap";
            Cap.UseVisualStyleBackColor = true;
            // 
            // Coat
            // 
            Coat.ImageIndex = 4;
            Coat.Location = new System.Drawing.Point(4, 35);
            Coat.Name = "Coat";
            Coat.Padding = new System.Windows.Forms.Padding(3);
            Coat.Size = new System.Drawing.Size(1166, 467);
            Coat.TabIndex = 4;
            Coat.Text = "Coat";
            Coat.UseVisualStyleBackColor = true;
            // 
            // Longcoat
            // 
            Longcoat.ImageIndex = 5;
            Longcoat.Location = new System.Drawing.Point(4, 35);
            Longcoat.Name = "Longcoat";
            Longcoat.Padding = new System.Windows.Forms.Padding(3);
            Longcoat.Size = new System.Drawing.Size(1166, 467);
            Longcoat.TabIndex = 5;
            Longcoat.Text = "Longcoat";
            Longcoat.UseVisualStyleBackColor = true;
            // 
            // Pants
            // 
            Pants.ImageIndex = 6;
            Pants.Location = new System.Drawing.Point(4, 35);
            Pants.Name = "Pants";
            Pants.Padding = new System.Windows.Forms.Padding(3);
            Pants.Size = new System.Drawing.Size(1166, 467);
            Pants.TabIndex = 6;
            Pants.Text = "Pants";
            Pants.UseVisualStyleBackColor = true;
            // 
            // Shoes
            // 
            Shoes.ImageIndex = 7;
            Shoes.Location = new System.Drawing.Point(4, 35);
            Shoes.Name = "Shoes";
            Shoes.Padding = new System.Windows.Forms.Padding(3);
            Shoes.Size = new System.Drawing.Size(1166, 467);
            Shoes.TabIndex = 7;
            Shoes.Text = "Shoes";
            Shoes.UseVisualStyleBackColor = true;
            // 
            // Glove
            // 
            Glove.ImageIndex = 8;
            Glove.Location = new System.Drawing.Point(4, 35);
            Glove.Name = "Glove";
            Glove.Padding = new System.Windows.Forms.Padding(3);
            Glove.Size = new System.Drawing.Size(1166, 467);
            Glove.TabIndex = 8;
            Glove.Text = "Glove";
            Glove.UseVisualStyleBackColor = true;
            // 
            // Ring
            // 
            Ring.ImageIndex = 9;
            Ring.Location = new System.Drawing.Point(4, 35);
            Ring.Name = "Ring";
            Ring.Padding = new System.Windows.Forms.Padding(3);
            Ring.Size = new System.Drawing.Size(1166, 467);
            Ring.TabIndex = 9;
            Ring.Text = "Ring";
            Ring.UseVisualStyleBackColor = true;
            // 
            // Cape
            // 
            Cape.ImageIndex = 10;
            Cape.Location = new System.Drawing.Point(4, 35);
            Cape.Name = "Cape";
            Cape.Padding = new System.Windows.Forms.Padding(3);
            Cape.Size = new System.Drawing.Size(1166, 467);
            Cape.TabIndex = 10;
            Cape.Text = "Cape";
            Cape.UseVisualStyleBackColor = true;
            // 
            // Accessory
            // 
            Accessory.ImageIndex = 11;
            Accessory.Location = new System.Drawing.Point(4, 35);
            Accessory.Name = "Accessory";
            Accessory.Padding = new System.Windows.Forms.Padding(3);
            Accessory.Size = new System.Drawing.Size(1166, 467);
            Accessory.TabIndex = 11;
            Accessory.Text = "Accessory";
            Accessory.UseVisualStyleBackColor = true;
            // 
            // Shield
            // 
            Shield.ImageIndex = 12;
            Shield.Location = new System.Drawing.Point(4, 35);
            Shield.Name = "Shield";
            Shield.Padding = new System.Windows.Forms.Padding(3);
            Shield.Size = new System.Drawing.Size(1166, 467);
            Shield.TabIndex = 12;
            Shield.Text = "Shield";
            Shield.UseVisualStyleBackColor = true;
            // 
            // TamingMob
            // 
            TamingMob.ImageIndex = 13;
            TamingMob.Location = new System.Drawing.Point(4, 35);
            TamingMob.Name = "TamingMob";
            TamingMob.Padding = new System.Windows.Forms.Padding(3);
            TamingMob.Size = new System.Drawing.Size(1166, 467);
            TamingMob.TabIndex = 13;
            TamingMob.Text = "TamingMob";
            TamingMob.UseVisualStyleBackColor = true;
            // 
            // Hair
            // 
            Hair.ImageIndex = 14;
            Hair.Location = new System.Drawing.Point(4, 35);
            Hair.Name = "Hair";
            Hair.Padding = new System.Windows.Forms.Padding(3);
            Hair.Size = new System.Drawing.Size(1166, 467);
            Hair.TabIndex = 14;
            Hair.Text = "Hair";
            Hair.UseVisualStyleBackColor = true;
            // 
            // Face
            // 
            Face.ImageIndex = 15;
            Face.Location = new System.Drawing.Point(4, 35);
            Face.Name = "Face";
            Face.Padding = new System.Windows.Forms.Padding(3);
            Face.Size = new System.Drawing.Size(1166, 467);
            Face.TabIndex = 15;
            Face.Text = "Face";
            Face.UseVisualStyleBackColor = true;
            // 
            // Map1
            // 
            Map1.ImageIndex = 16;
            Map1.Location = new System.Drawing.Point(4, 35);
            Map1.Name = "Map1";
            Map1.Padding = new System.Windows.Forms.Padding(3);
            Map1.Size = new System.Drawing.Size(1166, 467);
            Map1.TabIndex = 16;
            Map1.Text = "Map(1)";
            Map1.UseVisualStyleBackColor = true;
            // 
            // Map2
            // 
            Map2.ImageIndex = 16;
            Map2.Location = new System.Drawing.Point(4, 35);
            Map2.Name = "Map2";
            Map2.Padding = new System.Windows.Forms.Padding(3);
            Map2.Size = new System.Drawing.Size(1166, 467);
            Map2.TabIndex = 17;
            Map2.Text = "Map(2)";
            Map2.UseVisualStyleBackColor = true;
            // 
            // Map3
            // 
            Map3.ImageIndex = 16;
            Map3.Location = new System.Drawing.Point(4, 35);
            Map3.Name = "Map3";
            Map3.Padding = new System.Windows.Forms.Padding(3);
            Map3.Size = new System.Drawing.Size(1166, 467);
            Map3.TabIndex = 18;
            Map3.Text = "Map(3)";
            Map3.UseVisualStyleBackColor = true;
            // 
            // Mob
            // 
            Mob.ImageIndex = 17;
            Mob.Location = new System.Drawing.Point(4, 35);
            Mob.Name = "Mob";
            Mob.Padding = new System.Windows.Forms.Padding(3);
            Mob.Size = new System.Drawing.Size(1166, 467);
            Mob.TabIndex = 19;
            Mob.Text = "Mob(1)";
            Mob.UseVisualStyleBackColor = true;
            // 
            // Mob001
            // 
            Mob001.ImageIndex = 17;
            Mob001.Location = new System.Drawing.Point(4, 35);
            Mob001.Name = "Mob001";
            Mob001.Padding = new System.Windows.Forms.Padding(3);
            Mob001.Size = new System.Drawing.Size(1166, 467);
            Mob001.TabIndex = 20;
            Mob001.Text = "Mob(2)";
            Mob001.UseVisualStyleBackColor = true;
            // 
            // Mob2
            // 
            Mob2.ImageIndex = 17;
            Mob2.Location = new System.Drawing.Point(4, 35);
            Mob2.Name = "Mob2";
            Mob2.Padding = new System.Windows.Forms.Padding(3);
            Mob2.Size = new System.Drawing.Size(1166, 467);
            Mob2.TabIndex = 21;
            Mob2.Text = "Mob(3)";
            Mob2.UseVisualStyleBackColor = true;
            // 
            // Skill
            // 
            Skill.ImageIndex = 18;
            Skill.Location = new System.Drawing.Point(4, 35);
            Skill.Name = "Skill";
            Skill.Padding = new System.Windows.Forms.Padding(3);
            Skill.Size = new System.Drawing.Size(1166, 467);
            Skill.TabIndex = 22;
            Skill.Text = "Skill";
            Skill.UseVisualStyleBackColor = true;
            // 
            // Npc
            // 
            Npc.ImageIndex = 19;
            Npc.Location = new System.Drawing.Point(4, 35);
            Npc.Name = "Npc";
            Npc.Padding = new System.Windows.Forms.Padding(3);
            Npc.Size = new System.Drawing.Size(1166, 467);
            Npc.TabIndex = 25;
            Npc.Text = "Npc";
            Npc.UseVisualStyleBackColor = true;
            // 
            // Pet
            // 
            Pet.ImageIndex = 20;
            Pet.Location = new System.Drawing.Point(4, 35);
            Pet.Name = "Pet";
            Pet.Padding = new System.Windows.Forms.Padding(3);
            Pet.Size = new System.Drawing.Size(1166, 467);
            Pet.TabIndex = 26;
            Pet.Text = "Pet";
            Pet.UseVisualStyleBackColor = true;
            // 
            // Install
            // 
            Install.ImageIndex = 21;
            Install.Location = new System.Drawing.Point(4, 35);
            Install.Name = "Install";
            Install.Padding = new System.Windows.Forms.Padding(3);
            Install.Size = new System.Drawing.Size(1166, 467);
            Install.TabIndex = 27;
            Install.Text = "Install";
            Install.UseVisualStyleBackColor = true;
            // 
            // Android
            // 
            Android.ImageIndex = 22;
            Android.Location = new System.Drawing.Point(4, 35);
            Android.Name = "Android";
            Android.Padding = new System.Windows.Forms.Padding(3);
            Android.Size = new System.Drawing.Size(1166, 467);
            Android.TabIndex = 28;
            Android.Text = "Android";
            Android.UseVisualStyleBackColor = true;
            // 
            // Mechanic
            // 
            Mechanic.ImageIndex = 23;
            Mechanic.Location = new System.Drawing.Point(4, 35);
            Mechanic.Name = "Mechanic";
            Mechanic.Padding = new System.Windows.Forms.Padding(3);
            Mechanic.Size = new System.Drawing.Size(1166, 467);
            Mechanic.TabIndex = 29;
            Mechanic.Text = "Mechanic";
            Mechanic.UseVisualStyleBackColor = true;
            // 
            // PetEquip
            // 
            PetEquip.ImageIndex = 24;
            PetEquip.Location = new System.Drawing.Point(4, 35);
            PetEquip.Name = "PetEquip";
            PetEquip.Padding = new System.Windows.Forms.Padding(3);
            PetEquip.Size = new System.Drawing.Size(1166, 467);
            PetEquip.TabIndex = 30;
            PetEquip.Text = "PetEquip";
            PetEquip.UseVisualStyleBackColor = true;
            // 
            // Bits
            // 
            Bits.ImageIndex = 25;
            Bits.Location = new System.Drawing.Point(4, 35);
            Bits.Name = "Bits";
            Bits.Padding = new System.Windows.Forms.Padding(3);
            Bits.Size = new System.Drawing.Size(1166, 467);
            Bits.TabIndex = 31;
            Bits.Text = "Bits";
            Bits.UseVisualStyleBackColor = true;
            // 
            // MonsterBattle
            // 
            MonsterBattle.ImageIndex = 26;
            MonsterBattle.Location = new System.Drawing.Point(4, 35);
            MonsterBattle.Name = "MonsterBattle";
            MonsterBattle.Padding = new System.Windows.Forms.Padding(3);
            MonsterBattle.Size = new System.Drawing.Size(1166, 467);
            MonsterBattle.TabIndex = 32;
            MonsterBattle.Text = "MonsterBattle";
            MonsterBattle.UseVisualStyleBackColor = true;
            // 
            // Totem
            // 
            Totem.ImageIndex = 27;
            Totem.Location = new System.Drawing.Point(4, 35);
            Totem.Name = "Totem";
            Totem.Padding = new System.Windows.Forms.Padding(3);
            Totem.Size = new System.Drawing.Size(1166, 467);
            Totem.TabIndex = 33;
            Totem.Text = "Totem";
            Totem.UseVisualStyleBackColor = true;
            // 
            // Morph
            // 
            Morph.ImageIndex = 28;
            Morph.Location = new System.Drawing.Point(4, 35);
            Morph.Name = "Morph";
            Morph.Padding = new System.Windows.Forms.Padding(3);
            Morph.Size = new System.Drawing.Size(1166, 467);
            Morph.TabIndex = 34;
            Morph.Text = "Morph";
            Morph.UseVisualStyleBackColor = true;
            // 
            // Familiar
            // 
            Familiar.ImageIndex = 29;
            Familiar.Location = new System.Drawing.Point(4, 35);
            Familiar.Name = "Familiar";
            Familiar.Padding = new System.Windows.Forms.Padding(3);
            Familiar.Size = new System.Drawing.Size(1166, 467);
            Familiar.TabIndex = 35;
            Familiar.Text = "Familiar";
            Familiar.UseVisualStyleBackColor = true;
            // 
            // DamageSkin
            // 
            DamageSkin.ImageIndex = 30;
            DamageSkin.Location = new System.Drawing.Point(4, 35);
            DamageSkin.Name = "DamageSkin";
            DamageSkin.Padding = new System.Windows.Forms.Padding(3);
            DamageSkin.Size = new System.Drawing.Size(1166, 467);
            DamageSkin.TabIndex = 36;
            DamageSkin.Text = "DamageSkin";
            DamageSkin.UseVisualStyleBackColor = true;
            // 
            // Etc
            // 
            Etc.ImageIndex = 31;
            Etc.Location = new System.Drawing.Point(4, 35);
            Etc.Name = "Etc";
            Etc.Padding = new System.Windows.Forms.Padding(3);
            Etc.Size = new System.Drawing.Size(1166, 467);
            Etc.TabIndex = 37;
            Etc.Text = "Etc";
            Etc.UseVisualStyleBackColor = true;
            // 
            // Reactor
            // 
            Reactor.ImageIndex = 32;
            Reactor.Location = new System.Drawing.Point(4, 35);
            Reactor.Name = "Reactor";
            Reactor.Padding = new System.Windows.Forms.Padding(3);
            Reactor.Size = new System.Drawing.Size(1166, 467);
            Reactor.TabIndex = 38;
            Reactor.Text = "Reactor";
            Reactor.UseVisualStyleBackColor = true;
            // 
            // Music
            // 
            Music.ImageIndex = 33;
            Music.Location = new System.Drawing.Point(4, 35);
            Music.Name = "Music";
            Music.Size = new System.Drawing.Size(1166, 467);
            Music.TabIndex = 39;
            Music.Text = "Music";
            Music.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "0.png");
            imageList1.Images.SetKeyName(1, "1.png");
            imageList1.Images.SetKeyName(2, "2.png");
            imageList1.Images.SetKeyName(3, "3.png");
            imageList1.Images.SetKeyName(4, "4.png");
            imageList1.Images.SetKeyName(5, "5.png");
            imageList1.Images.SetKeyName(6, "6.png");
            imageList1.Images.SetKeyName(7, "7.png");
            imageList1.Images.SetKeyName(8, "8.png");
            imageList1.Images.SetKeyName(9, "9.png");
            imageList1.Images.SetKeyName(10, "10.png");
            imageList1.Images.SetKeyName(11, "11.png");
            imageList1.Images.SetKeyName(12, "12.png");
            imageList1.Images.SetKeyName(13, "13.png");
            imageList1.Images.SetKeyName(14, "14.png");
            imageList1.Images.SetKeyName(15, "15.png");
            imageList1.Images.SetKeyName(16, "16.png");
            imageList1.Images.SetKeyName(17, "17.png");
            imageList1.Images.SetKeyName(18, "18.png");
            imageList1.Images.SetKeyName(19, "19.png");
            imageList1.Images.SetKeyName(20, "20.png");
            imageList1.Images.SetKeyName(21, "21.png");
            imageList1.Images.SetKeyName(22, "22.png");
            imageList1.Images.SetKeyName(23, "23.png");
            imageList1.Images.SetKeyName(24, "24.png");
            imageList1.Images.SetKeyName(25, "25.png");
            imageList1.Images.SetKeyName(26, "26.png");
            imageList1.Images.SetKeyName(27, "27.png");
            imageList1.Images.SetKeyName(28, "28.png");
            imageList1.Images.SetKeyName(29, "29.png");
            imageList1.Images.SetKeyName(30, "30.png");
            imageList1.Images.SetKeyName(31, "31.png");
            imageList1.Images.SetKeyName(32, "32.png");
            imageList1.Images.SetKeyName(33, "image33.png");
            imageList1.Images.SetKeyName(34, "9101325.icon.png");
            // 
            // DB2Form
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(1174, 541);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Controls.Add(button1);
            Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "DB2Form";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "GM HandTool";
            TopMost = true;
            FormClosing += DB2Form_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Cash;
        private System.Windows.Forms.TabPage Consume;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage Weapon;
        private System.Windows.Forms.TabPage Cap;
        private System.Windows.Forms.TabPage Coat;
        private System.Windows.Forms.TabPage Longcoat;
        private System.Windows.Forms.TabPage Pants;
        private System.Windows.Forms.TabPage Shoes;
        private System.Windows.Forms.TabPage Glove;
        private System.Windows.Forms.TabPage Ring;
        private System.Windows.Forms.TabPage Cape;
        private System.Windows.Forms.TabPage Accessory;
        private System.Windows.Forms.TabPage Shield;
        private System.Windows.Forms.TabPage TamingMob;
        private System.Windows.Forms.TabPage Hair;
        private System.Windows.Forms.TabPage Face;
        private System.Windows.Forms.TabPage Map1;
        private System.Windows.Forms.TabPage Map2;
        private System.Windows.Forms.TabPage Map3;
        private System.Windows.Forms.TabPage Mob;
        private System.Windows.Forms.TabPage Mob001;
        private System.Windows.Forms.TabPage Mob2;
        private System.Windows.Forms.TabPage Skill;
        private System.Windows.Forms.TabPage Npc;
        private System.Windows.Forms.TabPage Pet;
        private System.Windows.Forms.TabPage Install;
        private System.Windows.Forms.TabPage Android;
        private System.Windows.Forms.TabPage Mechanic;
        private System.Windows.Forms.TabPage PetEquip;
        private System.Windows.Forms.TabPage Bits;
        private System.Windows.Forms.TabPage MonsterBattle;
        private System.Windows.Forms.TabPage Totem;
        private System.Windows.Forms.TabPage Morph;
        private System.Windows.Forms.TabPage Familiar;
        private System.Windows.Forms.TabPage DamageSkin;
        private System.Windows.Forms.TabPage Etc;
        private System.Windows.Forms.TabPage Reactor;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.TabPage Music;
        private System.Windows.Forms.TabPage Special;
    }
}

