using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Resource = CharaSimResource.Resource;
using WzComparerR2.PluginBase;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzComparerR2.CharaSim;
using GraphicsExtension;
using DPIUtils;

namespace WzComparerR2.CharaSimControl
{
    public class ItemTooltipRender2 : TooltipRender
    {
        public ItemTooltipRender2()
        {
        }

        private Item item;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public override object TargetItem
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value as Item;
            }
        }


        public bool LinkRecipeInfo { get; set; }
        public bool LinkRecipeItem { get; set; }
        public bool ShowLevelOrSealed { get; set; }
        public bool ShowNickTag { get; set; }

        public TooltipRender LinkRecipeInfoRender { get; set; }
        public TooltipRender LinkRecipeGearRender { get; set; }
        public TooltipRender LinkRecipeItemRender { get; set; }
        public TooltipRender SetItemRender { get; set; }
        public TooltipRender CashPackageRender { get; set; }

        public override Bitmap Render()
        {
            if (this.item == null)
            {
                return null;
            }
            //绘制道具
            int picHeight;
            Bitmap itemBmp = RenderItem(out picHeight);
            List<Bitmap> recipeInfoBmps = new();
            List<Bitmap> recipeItemBmps = new();
            Bitmap setItemBmp = null;
            Bitmap levelBmp = null;
            int levelHeight = 0;
            if (this.ShowLevelOrSealed)
            {
                levelBmp = RenderLevel(out levelHeight);
            }

            if (this.item.ItemID / 10000 == 910)
            {
                Wz_Node itemNode = PluginBase.PluginManager.FindWz(string.Format(@"Item\Special\{0:D4}.img\{1}", this.item.ItemID / 10000, this.item.ItemID));
                Wz_Node cashPackageNode = PluginBase.PluginManager.FindWz(string.Format(@"Etc\CashPackage.img\{0}", this.item.ItemID));
                CashPackage cashPackage = CashPackage.CreateFromNode(itemNode, cashPackageNode, PluginBase.PluginManager.FindWz);
                return RenderCashPackage(cashPackage);
            }

            Action<int> AppendGearOrItem = (int itemID) =>
            {
                int itemIDClass = itemID / 1000000;
                if (itemIDClass == 1) //通过ID寻找装备
                {
                    Wz_Node charaWz = PluginManager.FindWz(Wz_Type.Character);
                    if (charaWz != null)
                    {
                        string imgName = itemID.ToString("d8") + ".img";
                        foreach (Wz_Node node0 in charaWz.Nodes)
                        {
                            Wz_Node imgNode = node0.FindNodeByPath(imgName, true);
                            if (imgNode != null)
                            {
                                Gear gear = Gear.CreateFromNode(imgNode, path => PluginManager.FindWz(path));
                                if (gear != null)
                                {
                                    gear.Props[GearPropType.timeLimited] = 0;
                                    long tuc, tucCnt;
                                    if (Item.Props.TryGetValue(ItemPropType.addTooltip_tuc, out tuc) && Item.Props.TryGetValue(ItemPropType.addTooltip_tucCnt, out tucCnt))
                                    {
                                        Wz_Node itemWz = PluginManager.FindWz(Wz_Type.Item);
                                        if (itemWz != null)
                                        {
                                            string imgClass = (tuc / 10000).ToString("d4") + ".img\\" + tuc.ToString("d8") + "\\info";
                                            foreach (Wz_Node node1 in itemWz.Nodes)
                                            {
                                                Wz_Node infoNode = node1.FindNodeByPath(imgClass, true);
                                                if (infoNode != null)
                                                {
                                                    gear.Upgrade(infoNode, (int)tucCnt);

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    recipeItemBmps.Add(RenderLinkRecipeGear(gear));
                                }

                                break;
                            }
                        }
                    }
                }
                else if (itemIDClass >= 2 && itemIDClass <= 5) //通过ID寻找道具
                {
                    Wz_Node itemWz = PluginManager.FindWz(Wz_Type.Item);
                    if (itemWz != null)
                    {
                        string imgClass = (itemID / 10000).ToString("d4") + ".img\\" + itemID.ToString("d8");
                        foreach (Wz_Node node0 in itemWz.Nodes)
                        {
                            Wz_Node imgNode = node0.FindNodeByPath(imgClass, true);
                            if (imgNode != null)
                            {
                                Item item = Item.CreateFromNode(imgNode, PluginManager.FindWz);
                                item.Props[ItemPropType.timeLimited] = 0;
                                if (item != null)
                                {
                                    recipeItemBmps.Add(RenderLinkRecipeItem(item));
                                }

                                break;
                            }
                        }
                    }
                }
            };

            //图纸相关
            if (this.item.Recipes.Count > 0)
            {
                foreach (int recipeID in this.item.Recipes)
                {
                    long recipeSkillID = recipeID / 10000;
                    Recipe recipe = null;
                    //寻找配方
                    Wz_Node recipeNode = PluginBase.PluginManager.FindWz(string.Format(@"Skill\Recipe_{0}.img\{1}", recipeSkillID, recipeID));
                    if (recipeNode != null)
                    {
                        recipe = Recipe.CreateFromNode(recipeNode);
                    }
                    //生成配方图像
                    if (recipe != null)
                    {
                        if (this.LinkRecipeInfo)
                        {
                            recipeInfoBmps.Add(RenderLinkRecipeInfo(recipe));
                        }

                        if (this.LinkRecipeItem)
                        {
                            int itemID = recipe.MainTargetItemID;
                            AppendGearOrItem(itemID);
                        }
                    }
                }
            }

            long value;
            if (this.item.Props.TryGetValue(ItemPropType.dressUpgrade, out value))
            {
                long itemID = value;
                AppendGearOrItem((int)itemID);
            }
            if (this.item.Props.TryGetValue(ItemPropType.tamingMob, out value))
            {
                long itemID = value;
                AppendGearOrItem((int)itemID);
            }

            if (this.item.AddTooltips.Count > 0)
            {
                foreach (int itemID in item.AddTooltips)
                {
                    AppendGearOrItem(itemID);
                }
            }

            if (this.item.Props.TryGetValue(ItemPropType.setItemID, out long setID))
            {
                SetItem setItem;
                if (CharaSimLoader.LoadedSetItems.TryGetValue((int)setID, out setItem))
                {
                    setItemBmp = RenderSetItem(setItem);
                }
            }

            //计算布局
            Size totalSize = new Size(itemBmp.Width, picHeight);
            Point recipeInfoOrigin = Point.Empty;
            Point recipeItemOrigin = Point.Empty;
            Point setItemOrigin = Point.Empty;
            Point levelOrigin = Point.Empty;

            if (recipeItemBmps.Count > 0)
            {
                // layout:
                //   item        |  recipeItem
                //   recipeInfo  |
                recipeItemOrigin.X = totalSize.Width;
                totalSize.Width += recipeItemBmps.Max(bmp => bmp.Width);

                if (recipeInfoBmps.Count > 0)
                {
                    recipeInfoOrigin.X = itemBmp.Width - recipeInfoBmps.Max(bmp => bmp.Width);
                    recipeInfoOrigin.Y = picHeight;
                    totalSize.Height = Math.Max(picHeight + recipeInfoBmps.Sum(bmp => bmp.Height), recipeItemBmps.Sum(bmp => bmp.Height));
                }
                else
                {
                    totalSize.Height = Math.Max(picHeight, recipeItemBmps.Sum(bmp => bmp.Height));
                }
            }
            else if (recipeInfoBmps.Count > 0)
            {
                // layout:
                //   item  |  recipeInfo
                totalSize.Width += recipeInfoBmps.Max(bmp => bmp.Width);
                totalSize.Height = Math.Max(picHeight, recipeInfoBmps.Sum(bmp => bmp.Height));
                recipeInfoOrigin.X = itemBmp.Width;
            }
            if (setItemBmp != null)
            {
                setItemOrigin = new Point(totalSize.Width, 0);
                totalSize.Width += setItemBmp.Width;
                totalSize.Height = Math.Max(totalSize.Height, setItemBmp.Height);
            }
            if (levelBmp != null)
            {
                levelOrigin = new Point(totalSize.Width, 0);
                totalSize.Width += levelBmp.Width;
                totalSize.Height = Math.Max(totalSize.Height, levelHeight);
            }

            //开始绘制
            Bitmap tooltip = new Bitmap(totalSize.Width, totalSize.Height);
            Graphics g = Graphics.FromImage(tooltip);

            if (itemBmp != null)
            {
                //绘制背景区域
                GearGraphics.DrawNewTooltipBack(g, 0, 0, itemBmp.Width, picHeight);
                //复制图像
                g.DrawImage2(itemBmp, 0, 0, new Rectangle(0, 0, itemBmp.Width, picHeight), GraphicsUnit.Pixel);
                //左上角
                g.DrawImage2(Resource.UIToolTip_img_Item_Frame2_cover, 3, 3);

                if (this.ShowObjectID)
                {
                    GearGraphics.DrawGearDetailNumber(g, 3, 3, item.ItemID.ToString("d8"), true);
                }
            }

            //绘制配方
            if (recipeInfoBmps.Count > 0)
            {
                for (int i = 0, y = recipeInfoOrigin.Y; i < recipeInfoBmps.Count; i++)
                {
                    g.DrawImage2(recipeInfoBmps[i], recipeInfoOrigin.X, y,
                        new Rectangle(Point.Empty, recipeInfoBmps[i].Size), GraphicsUnit.Pixel);
                    y += recipeInfoBmps[i].Height;
                }
            }

            //绘制产出道具
            if (recipeItemBmps.Count > 0)
            {
                for (int i = 0, y = recipeItemOrigin.Y; i < recipeItemBmps.Count; i++)
                {
                    g.DrawImage2(recipeItemBmps[i], recipeItemOrigin.X, y,
                        new Rectangle(Point.Empty, recipeItemBmps[i].Size), GraphicsUnit.Pixel);
                    y += recipeItemBmps[i].Height;
                }
            }

            //绘制套装
            if (setItemBmp != null)
            {
                g.DrawImage2(setItemBmp, setItemOrigin.X, setItemOrigin.Y,
                    new Rectangle(Point.Empty, setItemBmp.Size), GraphicsUnit.Pixel);
            }

            if (levelBmp != null)
            {
                //绘制背景区域
                GearGraphics.DrawNewTooltipBack(g, levelOrigin.X, levelOrigin.Y, levelBmp.Width, levelHeight);
                //复制图像
                g.DrawImage2(levelBmp, levelOrigin.X, levelOrigin.Y, new Rectangle(0, 0, levelBmp.Width, levelHeight), GraphicsUnit.Pixel);
            }

            if (itemBmp != null)
                itemBmp.Dispose();
            if (recipeInfoBmps.Count > 0)
                recipeInfoBmps.ForEach(bmp => bmp.Dispose());
            if (recipeItemBmps.Count > 0)
                recipeItemBmps.ForEach(bmp => bmp.Dispose());
            if (setItemBmp != null)
                setItemBmp.Dispose();
            if (levelBmp != null)
                levelBmp.Dispose();

            g.Dispose();
            return tooltip;
        }

        private Bitmap RenderItem(out int picH)
        {
            bool isTranslateRequired = Translator.IsTranslateEnabled;
            Bitmap tooltip = new Bitmap(290, DefaultPicHeight);
            Graphics g = Graphics.FromImage(tooltip);
            StringFormat format = (StringFormat)StringFormat.GenericDefault.Clone();
            long value;
            int intvalue;

            picH = 10;
            //物品标题
            StringResult sr;
            if (StringLinker == null || !StringLinker.StringItem.TryGetValue(item.ItemID, out sr))
            {
                sr = new StringResult();
                sr.Name = "(null)";
            }
            string itemName = sr.Name.Replace(Environment.NewLine, "");
            string nameAdd = item.ItemID / 10000 == 313 || item.ItemID / 10000 == 501 ? "OFF" : null;
            if (!string.IsNullOrEmpty(nameAdd))
            {
                itemName += " (" + nameAdd + ")";
            }
            if (isTranslateRequired)
            {
                string translatedItemName = Translator.MergeString(itemName, Translator.TranslateString(itemName, true), 0, false, true);
                if (translatedItemName != itemName)
                {
                    itemName = translatedItemName;
                }
            }
            //SizeF titleSize = TextRenderer.MeasureText(g, sr.Name.Replace(Environment.NewLine, ""), GearGraphics.ItemNameFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPrefix);
            SizeF titleSize = TextRenderer.MeasureText(g, itemName, GearGraphics.ItemNameFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPrefix);
            titleSize.Width += 9 * 2;//9 was 12
            if (titleSize.Width > 290)
            {
                //重构大小
                g.Dispose();
                tooltip.Dispose();

                tooltip = new Bitmap((int)Math.Ceiling(titleSize.Width), DefaultPicHeight);
                g = Graphics.FromImage(tooltip);
                picH = 21;
            }
            if (sr["fixWidth"] != null)
            {
                //重构大小
                g.Dispose();
                tooltip.Dispose();

                tooltip = new Bitmap(Int32.Parse(sr["fixWidth"]), DefaultPicHeight);
                g = Graphics.FromImage(tooltip);
                picH = 10;
            }

            //绘制标题
            bool hasPart2 = false;
            g.DrawImage2(Resource.ToolTip_Equip_Dot_0, 9, picH + 5);//GMS Version blue dot in ITEMS
            format.Alignment = StringAlignment.Near;
            TextRenderer.DrawText(g, itemName, GearGraphics.ItemNameFont2, new Point(tooltip.Width / 21, picH), Color.White, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            //TextRenderer.DrawText(g, sr.Name.Replace(Environment.NewLine, ""), GearGraphics.ItemNameFont2, new Point(tooltip.Width / 21, picH), Color.White, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            picH += 21;//default value is 21

            if (Item.Props.TryGetValue(ItemPropType.wonderGrade, out value) && value > 0)
            {
                switch (value)
                {
                    case 1:
                        TextRenderer.DrawText(g, "Wonder Black", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), ((SolidBrush)GearGraphics.OrangeBrush3).Color, TextFormatFlags.HorizontalCenter);
                        break;
                    case 4:
                        TextRenderer.DrawText(g, "Sweet Luna", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), GearGraphics.itemPinkColor, TextFormatFlags.HorizontalCenter);
                        break;
                    case 5:
                        TextRenderer.DrawText(g, "Dream Luna", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), ((SolidBrush)GearGraphics.BlueBrush).Color, TextFormatFlags.HorizontalCenter);
                        break;
                    case 6:
                        TextRenderer.DrawText(g, "Petite Luna", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), GearGraphics.itemPurpleColor, TextFormatFlags.HorizontalCenter);
                        break;
                    default:
                        picH -= 15;
                        break;
                }
                picH += 15;
            }
            else if (Item.Props.TryGetValue(ItemPropType.BTSLabel, out value) && value > 0)
            {
                TextRenderer.DrawText(g, "BTS Label", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), Color.FromArgb(187, 102, 238), TextFormatFlags.HorizontalCenter);
                picH += 15;
            }
            else if (Item.Props.TryGetValue(ItemPropType.BLACKPINKLabel, out value) && value > 0)
            {
                TextRenderer.DrawText(g, "BLACKPINK Label", GearGraphics.EquipDetailFont, new Point(tooltip.Width, picH), Color.FromArgb(255, 136, 170), TextFormatFlags.HorizontalCenter);
                picH += 15;
            }

            //额外特性
            var attrList = GetItemAttributeString();
            if (attrList.Count > 0)
            {
                var font = GearGraphics.ItemDetailFont;
                string attrStr = null;
                for (int i = 0; i < attrList.Count; i++)
                {
                    var newStr = (attrStr != null ? (attrStr + ", ") : null) + attrList[i];
                    if (TextRenderer.MeasureText(g, newStr, font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width > tooltip.Width - 7 || (attrList[i].Contains('\n') && attrStr != null))
                    {
                        TextRenderer.DrawText(g, attrStr, GearGraphics.ItemDetailFont, new Point(tooltip.Width, picH), ((SolidBrush)GearGraphics.OrangeBrush4).Color, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
                        picH += 20;
                        attrStr = attrList[i];
                    }
                    else
                    {
                        attrStr = newStr;
                    }
                }
                if (!string.IsNullOrEmpty(attrStr))
                {
                    foreach (string attrLine in attrStr.Split('\n'))
                    {
                        TextRenderer.DrawText(g, attrLine, GearGraphics.ItemDetailFont, new Point(tooltip.Width, picH), ((SolidBrush)GearGraphics.OrangeBrush4).Color, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
                        picH += 20;
                    }
                    picH -= 3;
                }
                hasPart2 = true;
            }

            string expireTime = null;
            if (item.TimeLimited)
            {
                DateTime time = DateTime.Now.AddDays(7d);
                if (!item.Cash)
                {
                    expireTime = "Usable Until: " + time.ToString(@"M\/d\/yyyy HH\:mm") + " UTC";
                }
                else
                {
                    expireTime = "Usable Until: " + time.ToString(@"M\/d\/yyyy HH\:mm") + " UTC";
                }
            }
            else if (item.ConsumableFrom != null || item.EndUseDate != null)
            {
                expireTime = "";
                if (item.ConsumableFrom != null)
                {
                    expireTime += string.Format("\nUsable From: {1}/{2}/{0} {3:D2}:{4:D2} UTC", Convert.ToInt32(item.ConsumableFrom.Substring(0, 4)), Convert.ToInt32(item.ConsumableFrom.Substring(4, 2)), Convert.ToInt32(item.ConsumableFrom.Substring(6, 2)), Convert.ToInt32(item.ConsumableFrom.Substring(8, 2)), Convert.ToInt32(item.ConsumableFrom.Substring(10, 2)));
                }
                if (item.EndUseDate != null)
                {
                    expireTime += string.Format("\nUsable Until: {1}/{2}/{0} {3:D2}:{4:D2} UTC", Convert.ToInt32(item.EndUseDate.Substring(0, 4)), Convert.ToInt32(item.EndUseDate.Substring(4, 2)), Convert.ToInt32(item.EndUseDate.Substring(6, 2)), Convert.ToInt32(item.EndUseDate.Substring(8, 2)), Convert.ToInt32(item.EndUseDate.Substring(10, 2)));
                }
            }
            else if ((item.Props.TryGetValue(ItemPropType.permanent, out value) && value != 0) || (item.ItemID / 10000 == 500 && item.Props.TryGetValue(ItemPropType.life, out value) && value == 0))
            {
                picH -= 3;
                if (value == 0)
                {
                    value = 1;
                }
                expireTime = ItemStringHelper.GetItemPropString(ItemPropType.permanent, value);
            }
            else if (item.ItemID / 10000 == 500 && item.Props.TryGetValue(ItemPropType.limitedLife, out value) && value > 0)
            {
                picH -= 3;
                expireTime = string.Format("DAYS OF MAGIC: {0}hrs. {1}min.", value / 3600, (value % 3600) / 60);
            }
            else if (item.ItemID / 10000 == 500 && item.Props.TryGetValue(ItemPropType.life, out value) && value > 0)
            {
                picH -= 3;
                DateTime time = DateTime.Now.AddDays(value);
                expireTime = "Water of Life Dries Up: " + time.ToString(@"M\/d\/yyyy HH\:mm") + " UTC";
            }
            if (!string.IsNullOrEmpty(expireTime))
            {
                if (attrList.Count > 0)
                {
                    picH += 3;
                }
                //picH += 3;
                //TextRenderer.DrawText(g, expireTime, GearGraphics.EquipDetailFont, new Point(tooltip.Width / 25, picH), Color.White, TextFormatFlags.Left);
                //picH += 16;
                //hasPart2 = true;
                foreach (string expireTimeLine in expireTime.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    g.DrawImage2(Resource.ToolTip_Equip_Dot_1, 9, picH + 6);//GMS Version, default value is 9, + X?
                    TextRenderer.DrawText(g, expireTimeLine, GearGraphics.EquipDetailFont, new Point(tooltip.Width / 25, picH), Color.White, TextFormatFlags.Left);
                    picH += 16;
                }
                if (expireTime.Contains('\n'))
                {
                    picH += 4;
                }
                hasPart2 = true;
            }

            if (hasPart2)
            {
                picH += 4;
            }

            //绘制图标
            int iconY = picH;
            int iconX = 10;
            g.DrawImage2(Resource.UIToolTip_img_Item_ItemIcon_base, iconX, picH);
            if (item.Icon.Bitmap != null)
            {
                g.DrawImage2(GearGraphics.EnlargeBitmap(item.Icon.Bitmap),
                iconX + 6 + (1 - item.Icon.Origin.X) * 2,
                picH + 6 + (33 - item.Icon.Origin.Y) * 2);
                //picH + 8 + (33 - item.Icon.Bitmap.Height) * 2);
            }
            if (item.Cash)
            {
                Bitmap cashImg = null;
                Point cashOrigin = new Point(12, 12);

                if (item.Props.TryGetValue(ItemPropType.wonderGrade, out value) && value > 0)
                {
                    string resKey = $"CashShop_img_CashItem_label_{value + 3}";
                    cashImg = Resource.ResourceManager.GetObject(resKey) as Bitmap;
                }
                else if (Item.Props.TryGetValue(ItemPropType.BTSLabel, out value) && value > 0)
                {
                    cashImg = Resource.CashShop_img_CashItem_label_10;
                    cashOrigin = new Point(cashImg.Width, cashImg.Height);
                }
                else if (Item.Props.TryGetValue(ItemPropType.BLACKPINKLabel, out value) && value > 0)
                {
                    cashImg = Resource.CashShop_img_CashItem_label_11;
                    cashOrigin = new Point(cashImg.Width, cashImg.Height);
                }
                if (cashImg == null) //default cashImg
                {
                    cashImg = Resource.CashItem_0;
                }

                g.DrawImage2(GearGraphics.EnlargeBitmap(cashImg),
                    iconX + 6 + 68 - cashOrigin.X * 2 - 2,
                    picH + 6 + 68 - cashOrigin.Y * 2 - 2);
            }
            g.DrawImage2(Resource.UIToolTip_img_Item_ItemIcon_new, iconX + 7, picH + 7);
            g.DrawImage2(Resource.UIToolTip_img_Item_ItemIcon_cover, iconX + 4, picH + 4); //绘制左上角cover

            value = 0;
            if (item.Props.TryGetValue(ItemPropType.reqLevel, out value) || item.ItemID / 10000 == 301 || item.ItemID / 1000 == 5204)
            {
                picH += 4;//default value is 4
                g.DrawImage2(Resource.ToolTip_Equip_Can_reqLEV, 100, picH);
                GearGraphics.DrawGearDetailNumber(g, 150, picH, value.ToString(), true);
                picH += 15;
            }
            else
            {
                picH += 3;
            }

            int right = tooltip.Width - 18;

            string desc = null;
            if (item.Level > 0)
            {
                desc += $"[LV.{item.Level}] ";
            }
            desc += sr.Desc;
            if (item.ItemID / 10000 == 500)
            {
                if (item.Props.TryGetValue(ItemPropType.wonderGrade, out value) && value > 0)
                {
                    if (item.Props.TryGetValue(ItemPropType.setItemID, out long setID))
                    {
                        SetItem setItem;
                        if (CharaSimLoader.LoadedSetItems.TryGetValue((int)setID, out setItem))
                        {
                            string wonderGradeString = null;
                            string setItemName = setItem.SetItemName;
                            string setSkillName = "";
                            switch (value)
                            {
                                case 1:
                                    wonderGradeString = "Wonder Black";
                                    foreach (KeyValuePair<GearPropType, object> prop in setItem.Effects.Values.SelectMany(f => f.PropsV5))
                                    {
                                        if (prop.Key == GearPropType.activeSkill)
                                        {
                                            SetItemActiveSkill p = ((List<SetItemActiveSkill>)prop.Value)[0];
                                            StringResult sr2;
                                            if (StringLinker == null || !StringLinker.StringSkill.TryGetValue(p.SkillID, out sr2))
                                            {
                                                sr2 = new StringResult();
                                                sr2.Name = p.SkillID.ToString();
                                            }
                                            setSkillName = Regex.Replace(sr2.Name, " Lv.\\d", "");
                                            break;
                                        }
                                    }
                                    break;
                                case 4:
                                    wonderGradeString = "Sweet Luna";
                                    setSkillName = "Sweet Luna";
                                    break;
                                case 5:
                                    wonderGradeString = "Dream Luna";
                                    setSkillName = "Dream Luna";
                                    break;
                            }
                            if (wonderGradeString != null)
                            {
                                desc += $"\nEquipping #c{wonderGradeString}# rank #c{setItemName}# pet gives the #c{setSkillName}# set effect. (Up to 3 levels.)\nSet effects can be enhanced up to 3 times depending on the type of #c{setItemName}# pet you have equipped.";
                            }
                        }
                    }
                }
                desc += "\n#cSkill: Meso Magnet";
                if (item.Props.TryGetValue(ItemPropType.pickupItem, out value) && value > 0)
                {
                    desc += ", Item Pouch";
                }
                if (item.Props.TryGetValue(ItemPropType.longRange, out value) && value > 0)
                {
                    desc += ", Expanded Auto Move";
                }
                if (item.Props.TryGetValue(ItemPropType.sweepForDrop, out value) && value > 0)
                {
                    desc += ", Auto Move";
                }
                if (item.Props.TryGetValue(ItemPropType.pickupAll, out value) && value > 0)
                {
                    desc += ", Expired Pickup Skill";
                }
                if (item.Props.TryGetValue(ItemPropType.consumeHP, out value) && value > 0)
                {
                    desc += ", Auto HP Potion Pouch";
                }
                if (item.Props.TryGetValue(ItemPropType.consumeMP, out value) && value > 0)
                {
                    desc += ", Auto MP Potion Pouch";
                }
                if (item.Props.TryGetValue(ItemPropType.autoBuff, out value) && value > 0)
                {
                    desc += ", Auto Buff";
                }
                if (item.Props.TryGetValue(ItemPropType.giantPet, out value) && value > 0)
                {
                    desc += ", Fatten Up";
                }
                if (item.Props.TryGetValue(ItemPropType.consumeCure, out value) && value > 0)
                {
                    desc += ", Take All Cure Potion";
                }
                desc += "#";
            }
            if (!string.IsNullOrEmpty(desc))
            {
                if (isTranslateRequired)
                {
                    string mergedDesc = Translator.MergeString(desc, Translator.TranslateString(desc), 2);
                    GearGraphics.DrawString(g, mergedDesc, GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
                else
                {
                    GearGraphics.DrawString(g, desc, GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
            }
            if (!string.IsNullOrEmpty(sr.AutoDesc))
            {
                if (isTranslateRequired)
                {
                    string mergedAutoDesc = Translator.MergeString(sr.AutoDesc, Translator.TranslateString(sr.AutoDesc), 2);
                    GearGraphics.DrawString(g, mergedAutoDesc, GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
                else
                {
                    GearGraphics.DrawString(g, sr.AutoDesc, GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
            }
            if (item.Props.TryGetValue(ItemPropType.tradeAvailable, out value) && value > 0)
            {
                string attr = ItemStringHelper.GetItemPropString(ItemPropType.tradeAvailable, value);
                if (!string.IsNullOrEmpty(attr))
                    GearGraphics.DrawString(g, "#c" + attr + "#", GearGraphics.ItemDetailFont2, 100, right, ref picH, 16);
            }
            if (item.Props.TryGetValue(ItemPropType.pointCost, out value) && value > 0)
            {
                picH += 16;
                GearGraphics.DrawString(g, "- " + value + " Point(s)", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.Specs.TryGetValue(ItemSpecType.recipeValidDay, out value) && value > 0)
            {
                GearGraphics.DrawString(g, "(Use within: " + value + " days )", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.Specs.TryGetValue(ItemSpecType.recipeUseCount, out value) && value > 0)
            {
                GearGraphics.DrawString(g, "(Craftable: " + value + " time(s))", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.ItemID / 1000 == 5533)
            {
                GearGraphics.DrawString(g, "\n#cBy double-clicking on it, you can preview the items inside the box, in order, every 3 seconds. Some random boxes can't be previewed.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.Cash)
            {
                if (item.Props.TryGetValue(ItemPropType.noMoveToLocker, out value) && value > 0)
                {
                    GearGraphics.DrawString(g, "\n#cThis item cannot be moved to the Cash inventory.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
                else if (item.Props.TryGetValue(ItemPropType.onlyCash, out value) && value > 0)
                {
                    GearGraphics.DrawString(g, "\n#cCan only be purchased with NX.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
                else if ((!item.Props.TryGetValue(ItemPropType.tradeBlock, out value) || value == 0) && item.ItemID / 10000 != 501 && item.ItemID / 10000 != 502 && item.ItemID / 10000 != 516)
                {
                    /*GearGraphics.DrawString(g, "\n#cThis item cannot be traded once it has been used.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);*/ //GMS - Enable when GMS uses this line.
                }
            }
            if (item.Props.TryGetValue(ItemPropType.flatRate, out value) && value > 0)
            {
                GearGraphics.DrawString(g, "\n#c기간 정액제 아이템입니다.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.Props.TryGetValue(ItemPropType.noScroll, out value) && value > 0)
            {
                GearGraphics.DrawString(g, "#cYou cannot use the Pet Skill Scroll or Pet Name tag at this time.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }
            if (item.Props.TryGetValue(ItemPropType.noRevive, out value) && value > 0)
            {
                GearGraphics.DrawString(g, "#cYou cannot use the Water of Life.#", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }

            if (item.ItemID / 10000 == 500)
            {
                Wz_Node petDialog = PluginManager.FindWz("String\\PetDialog.img\\" + item.ItemID);
                Dictionary<string, int> commandLev = new Dictionary<string, int>();
                foreach (Wz_Node commandNode in PluginManager.FindWz("Item\\Pet\\" + item.ItemID + ".img\\interact").Nodes)
                {
                    foreach (string command in petDialog?.Nodes[commandNode.Nodes["command"].GetValue<string>()].GetValueEx<string>(null)?.Split('|') ?? Enumerable.Empty<string>())
                    {
                        int l0;
                        if (!commandLev.TryGetValue(command, out l0))
                        {
                            commandLev.Add(command, commandNode.Nodes["l0"].GetValue<int>());
                        }
                        else
                        {
                            commandLev[command] = Math.Min(l0, commandNode.Nodes["l0"].GetValue<int>());
                        }
                    }
                }

                GearGraphics.DrawString(g, "[Usable Command]", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                foreach (int l0 in commandLev.Values.OrderBy(i => i).Distinct())
                {
                    GearGraphics.DrawString(g, "Lv. " + l0 + "+: " + string.Join(", ", commandLev.Where(i => i.Value == l0).Select(i => i.Key).OrderBy(s => s)), GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                }
                GearGraphics.DrawString(g, "Tip: You can control what your\n\r pet says once it reaches Lv. 15.", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
                GearGraphics.DrawString(g, "#cEx) /Pet [what to say]#", GearGraphics.ItemDetailFont, new Dictionary<string, Color>() { { "c", ((SolidBrush)GearGraphics.OrangeBrush4).Color } }, 100, right, ref picH, 16);
            }

            string incline = null;
            ItemPropType[] inclineTypes = new ItemPropType[]{
                    ItemPropType.charismaEXP,
                    ItemPropType.insightEXP,
                    ItemPropType.willEXP,
                    ItemPropType.craftEXP,
                    ItemPropType.senseEXP,
                    ItemPropType.charmEXP };

            string[] inclineString = new string[]{
                    " Ambition"," Insight"," Willpower"," Diligence"," Empathy"," Charm"};

            for (int i = 0; i < inclineTypes.Length; i++)
            {
                if (item.Props.TryGetValue(inclineTypes[i], out value) && value > 0)
                {
                    incline += ", " + value + inclineString[i];
                }
            }

            if (!string.IsNullOrEmpty(incline))
            {
                GearGraphics.DrawString(g, "#cGrants " + incline.Substring(2) + " EXP when first equipped\n(up to the daily maximum).", GearGraphics.ItemDetailFont, 100, right, ref picH, 16);
            }

            picH += 3;

            Wz_Node nickResNode = null;
            bool willDrawNickTag = this.ShowNickTag
                && this.Item.Props.TryGetValue(ItemPropType.nickTag, out value)
                && this.TryGetNickResource(value, out nickResNode);
            string descLeftAlign = sr["desc_leftalign"];
            long minLev = 0, maxLev = 0;
            bool willDrawExp = item.Props.TryGetValue(ItemPropType.exp_minLev, out minLev) && item.Props.TryGetValue(ItemPropType.exp_maxLev, out maxLev);

            if (!string.IsNullOrEmpty(descLeftAlign) || item.CoreSpecs.Count > 0 || item.Sample.Bitmap != null || item.DamageSkinID != null || item.SamplePath != null || willDrawNickTag || willDrawExp)
            {
                if (picH < iconY + 84)
                {
                    picH = iconY + 84;
                }
                if (!string.IsNullOrEmpty(descLeftAlign))
                {
                    picH += 12;
                    if (isTranslateRequired)
                    {
                        string mergedDescLeftAlign = Translator.MergeString(descLeftAlign, Translator.TranslateString(descLeftAlign), 2);
                        GearGraphics.DrawString(g, mergedDescLeftAlign, GearGraphics.ItemDetailFont, 14, right, ref picH, 16);
                    }
                    else
                    {
                        GearGraphics.DrawString(g, descLeftAlign, GearGraphics.ItemDetailFont, 14, right, ref picH, 16);
                    }
                }
                if (item.CoreSpecs.Count > 0)
                {
                    g.DrawLine(Pens.White, 6, picH - 1, tooltip.Width - 7, picH - 1);
                    picH += 9;
                    foreach (KeyValuePair<ItemCoreSpecType, Wz_Node> p in item.CoreSpecs)
                    {
                        string coreSpec;
                        intvalue = 0;
                        switch (p.Key)
                        {
                            case ItemCoreSpecType.Ctrl_addMob:
                                StringResult srMob;
                                if (StringLinker == null || !StringLinker.StringMob.TryGetValue(Convert.ToInt32(p.Value.Nodes["mobID"].Value), out srMob))
                                {
                                    srMob = new StringResult();
                                    srMob.Name = "(null)";
                                }
                                foreach (Wz_Node addMobNode in p.Value.Nodes)
                                {
                                    if (int.TryParse(addMobNode.Text, out intvalue))
                                    {
                                        break;
                                    }
                                }
                                coreSpec = ItemStringHelper.GetItemCoreSpecString(ItemCoreSpecType.Ctrl_addMob, intvalue, srMob.Name);
                                break;

                            default:
                                try
                                {
                                    coreSpec = ItemStringHelper.GetItemCoreSpecString(p.Key, Convert.ToInt32(p.Value.Value), Convert.ToString(p.Value.Nodes["desc"]?.Value));
                                }
                                finally
                                {
                                }
                                break;
                        }
                        GearGraphics.DrawString(g, "* " + coreSpec, GearGraphics.ItemDetailFont, 14, right, ref picH, 16);
                    }
                }
                if (item.Sample.Bitmap != null)
                {
                    g.DrawImage2(item.Sample.Bitmap, (tooltip.Width - item.Sample.Bitmap.Width) / 2, picH);
                    picH += item.Sample.Bitmap.Height;
                    picH += 2;
                }
                else if (item.DamageSkinID != null)
                {
                    Wz_Node sampleNode = PluginManager.FindWz($@"Etc\DamageSkin.img\{item.DamageSkinID}\sample");
                    if (sampleNode != null)
                    {
                        BitmapOrigin sample = BitmapOrigin.CreateFromNode(sampleNode, PluginManager.FindWz);
                        g.DrawImage2(sample.Bitmap, (tooltip.Width - sample.Bitmap.Width) / 2, picH);
                        picH += sample.Bitmap.Height;
                        picH += 2;
                    }
                }
                if (item.SamplePath != null)
                {
                    Wz_Node sampleNode = PluginManager.FindWz(item.SamplePath);
                    int sampleW = 15;
                    for (int i = 1; ; i++)
                    {
                        Wz_Node effectNode = sampleNode.FindNodeByPath(string.Format("{0}{1:D4}\\effect\\0", sampleNode.Text, i));
                        if (effectNode == null)
                        {
                            break;
                        }

                        BitmapOrigin effect = BitmapOrigin.CreateFromNode(effectNode, PluginManager.FindWz);
                        if (sampleW + 87 >= tooltip.Width)
                        {
                            picH += 62;
                            sampleW = 15;
                        }
                        g.DrawImage2(effect.Bitmap, sampleW + (85 - effect.Bitmap.Width - 1) / 2, picH + (62 - effect.Bitmap.Height - 1) / 2);
                        sampleW += 87;
                    }
                    picH += 62;
                }
                if (nickResNode != null)
                {
                    //获取称号名称
                    string nickName;
                    string nickWithQR = sr["nickWithQR"];
                    if (nickWithQR != null)
                    {
                        string qrDefault = sr["qrDefault"] ?? string.Empty;
                        nickName = Regex.Replace(nickWithQR, "#qr.*?#", qrDefault);
                    }
                    else
                    {
                        nickName = sr.Name;
                    }
                    GearGraphics.DrawNameTag(g, nickResNode, nickName, tooltip.Width, ref picH);
                    picH += 4; // value is either 4 or 14 (it was 14 in previous iteration)
                }
                if (minLev > 0 && maxLev > 0)
                {
                    long totalExp = 0;

                    for (int i = (int)minLev; i < (int)maxLev; i++)
                        totalExp += Character.ExpToNextLevel(i);

                    g.DrawLine(Pens.White, 6, picH, tooltip.Width - 7, picH);
                    picH += 8;

                    TextRenderer.DrawText(g, "Total EXP: " + totalExp, GearGraphics.ItemDetailFont2, new Point(10, picH), ((SolidBrush)GearGraphics.OrangeBrush4).Color, TextFormatFlags.NoPadding);
                    picH += 16;

                    TextRenderer.DrawText(g, "Remaining EXP: " + totalExp, GearGraphics.ItemDetailFont2, new Point(10, picH), Color.Red, TextFormatFlags.NoPadding);
                    picH += 16;

                    string cantAccountSharable = null;
                    Wz_Node itemWz = PluginManager.FindWz(Wz_Type.Item);
                    if (itemWz != null)
                    {
                        string imgClass = (item.ItemID / 10000).ToString("d4") + ".img\\" + item.ItemID.ToString("d8");
                        foreach (Wz_Node node0 in itemWz.Nodes)
                        {
                            Wz_Node imgNode = node0.FindNodeByPath(imgClass, true);
                            if (imgNode != null)
                            {
                                cantAccountSharable = imgNode.FindNodeByPath("info\\cantAccountSharable\\tooltip").GetValueEx<string>(null);
                                break;
                            }
                        }
                    }

                    if (cantAccountSharable != null)
                    {
                        TextRenderer.DrawText(g, cantAccountSharable, GearGraphics.ItemDetailFont2, new Point(10, picH), ((SolidBrush)GearGraphics.SetItemNameBrush).Color, TextFormatFlags.NoPadding);
                        picH += 16;
                        picH += 16;
                    }
                }
            }


            //绘制配方需求
            if (item.Recipes.Count > 0)
            {
                long reqSkill, reqSkillLevel;
                if (!item.Specs.TryGetValue(ItemSpecType.reqSkill, out reqSkill))
                {
                    reqSkill = item.Recipes[0] / 10000 * 10000;
                }

                if (!item.Specs.TryGetValue(ItemSpecType.reqSkillLevel, out reqSkillLevel))
                {
                    reqSkillLevel = 1;
                }

                picH = Math.Max(picH, iconY + 107);
                g.DrawLine(Pens.White, 6, picH, 283, picH);//分割线
                picH += 10;
                TextRenderer.DrawText(g, "<Requirements>", GearGraphics.ItemDetailFont, new Point(8, picH), ((SolidBrush)GearGraphics.SetItemNameBrush).Color, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                picH += 17;

                //技能标题
                if (StringLinker == null || !StringLinker.StringSkill.TryGetValue((int)reqSkill, out sr))
                {
                    sr = new StringResult();
                    sr.Name = "- (null)";
                }
                TextRenderer.DrawText(g, string.Format("- {0} Lv {1}", sr.Name, reqSkillLevel), GearGraphics.ItemDetailFont, new Point(13, picH), ((SolidBrush)GearGraphics.SetItemNameBrush).Color, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                picH += 16;
                picH += 6;
            }

            picH = Math.Max(iconY + 94, picH + 6);
            return tooltip;
        }

        private List<string> GetItemAttributeString()
        {
            long value, value2;
            List<string> tags = new List<string>();

            if (item.Props.TryGetValue(ItemPropType.quest, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.quest, value));
            }
            if (item.Props.TryGetValue(ItemPropType.pquest, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.pquest, value));
            }
            if (item.Props.TryGetValue(ItemPropType.only, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.only, value));
            }
            if (item.Props.TryGetValue(ItemPropType.tradeBlock, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.tradeBlock, value));
            }
            if (item.Props.TryGetValue(ItemPropType.useTradeBlock, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.useTradeBlock, value));
            }
            else if (item.ItemID / 10000 == 501 || item.ItemID / 10000 == 502 || item.ItemID / 10000 == 516)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.tradeBlock, 1));
            }
            if (item.Props.TryGetValue(ItemPropType.accountSharable, out value) && value != 0)
            {
                if (item.Props.TryGetValue(ItemPropType.exp_minLev, out value2) && value2 != 0)
                {
                    tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.useTradeBlock, 1));
                }
                if (item.Props.TryGetValue(ItemPropType.sharableOnce, out value2) && value2 != 0)
                {
                    tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.sharableOnce, value2));
                }
                else
                {
                    tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.accountSharable, value));
                }
            }
            if (item.Props.TryGetValue(ItemPropType.exchangeableOnce, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.exchangeableOnce, value));
            }
            if (item.Props.TryGetValue(ItemPropType.multiPet, out value))
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.multiPet, value));
            }
            else if (item.ItemID / 10000 == 500)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.multiPet, 0));
            }
            if (item.Props.TryGetValue(ItemPropType.accountSharableAfterExchange, out value) && value != 0)
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.accountSharableAfterExchange, value));
            }
            if (item.Props.TryGetValue(ItemPropType.mintable, out value))
            {
                tags.Add(ItemStringHelper.GetItemPropString(ItemPropType.mintable, value));
            }

            return tags;
        }

        private Bitmap RenderLinkRecipeInfo(Recipe recipe)
        {
            TooltipRender renderer = this.LinkRecipeInfoRender;
            if (renderer == null)
            {
                RecipeTooltipRender defaultRenderer = new RecipeTooltipRender();
                defaultRenderer.StringLinker = this.StringLinker;
                defaultRenderer.ShowObjectID = false;
                renderer = defaultRenderer;
            }

            renderer.TargetItem = recipe;
            return renderer.Render();
        }

        private Bitmap RenderLinkRecipeGear(Gear gear)
        {
            TooltipRender renderer = this.LinkRecipeGearRender;
            if (renderer == null)
            {
                GearTooltipRender2 defaultRenderer = new GearTooltipRender2();
                defaultRenderer.StringLinker = this.StringLinker;
                defaultRenderer.ShowObjectID = false;
                renderer = defaultRenderer;
            }

            renderer.TargetItem = gear;
            return renderer.Render();
        }

        private Bitmap RenderLinkRecipeItem(Item item)
        {
            TooltipRender renderer = this.LinkRecipeItemRender;
            if (renderer == null)
            {
                ItemTooltipRender2 defaultRenderer = new ItemTooltipRender2();
                defaultRenderer.StringLinker = this.StringLinker;
                defaultRenderer.ShowObjectID = false;
                renderer = defaultRenderer;
            }

            renderer.TargetItem = item;
            return renderer.Render();
        }

        private Bitmap RenderSetItem(SetItem setItem)
        {
            TooltipRender renderer = this.SetItemRender;
            if (renderer == null)
            {
                var defaultRenderer = new SetItemTooltipRender();
                defaultRenderer.StringLinker = this.StringLinker;
                defaultRenderer.ShowObjectID = false;
                renderer = defaultRenderer;
            }

            renderer.TargetItem = setItem;
            return renderer.Render();
        }

        private Bitmap RenderCashPackage(CashPackage cashPackage)
        {
            TooltipRender renderer = this.CashPackageRender;
            if (renderer == null)
            {
                var defaultRenderer = new CashPackageTooltipRender();
                defaultRenderer.StringLinker = this.StringLinker;
                defaultRenderer.ShowObjectID = this.ShowObjectID;
                renderer = defaultRenderer;
            }

            renderer.TargetItem = cashPackage;
            return renderer.Render();
        }

        private Bitmap RenderLevel(out int picHeight)
        {
            Bitmap level = null;
            Graphics g = null;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            picHeight = 0;
            if (Item.Levels != null)
            {
                if (level == null)
                {
                    level = new Bitmap(261, DefaultPicHeight);
                    g = Graphics.FromImage(level);
                }
                picHeight += 13;
                TextRenderer.DrawText(g, "Growth Stats", GearGraphics.EquipDetailFont, new Point(261, picHeight), ((SolidBrush)GearGraphics.GreenBrush2).Color, TextFormatFlags.HorizontalCenter);
                picHeight += 15;

                for (int i = 0; i < Item.Levels.Count; i++)
                {
                    var info = Item.Levels[i];
                    TextRenderer.DrawText(g, "Level " + info.Level + (i >= Item.Levels.Count - 1 ? " (MAX)" : null), GearGraphics.EquipDetailFont, new Point(10, picHeight), ((SolidBrush)GearGraphics.GreenBrush2).Color, TextFormatFlags.NoPadding);
                    picHeight += 15;
                    foreach (var kv in info.BonusProps)
                    {
                        GearLevelInfo.Range range = kv.Value;

                        string propString = ItemStringHelper.GetGearPropString(kv.Key, kv.Value.Min);
                        if (propString != null)
                        {
                            if (range.Max != range.Min)
                            {
                                propString += " ~ " + kv.Value.Max + (propString.EndsWith("%") ? "%" : null);
                            }
                            TextRenderer.DrawText(g, propString, GearGraphics.EquipDetailFont, new Point(10, picHeight), Color.White, TextFormatFlags.NoPadding);
                            picHeight += 15;
                        }
                    }
                    if (info.Skills.Count > 0)
                    {
                        string title = string.Format("{2:P2}({0}/{1}) 확률로 스킬 강화 옵션 추가 :", info.Prob, info.ProbTotal, info.Prob * 1.0 / info.ProbTotal);
                        TextRenderer.DrawText(g, title, GearGraphics.EquipDetailFont, new Point(10, picHeight), Color.White, TextFormatFlags.NoPadding);
                        picHeight += 15;
                        foreach (var kv in info.Skills)
                        {
                            StringResult sr = null;
                            if (this.StringLinker != null)
                            {
                                this.StringLinker.StringSkill.TryGetValue(kv.Key, out sr);
                            }
                            string text = string.Format(" +{2} {0}", sr == null ? null : sr.Name, kv.Key, kv.Value);
                            TextRenderer.DrawText(g, text, GearGraphics.EquipDetailFont, new Point(10, picHeight), ((SolidBrush)GearGraphics.OrangeBrush).Color, TextFormatFlags.NoPadding);
                            picHeight += 15;
                        }
                    }
                    if (info.EquipmentSkills.Count > 0)
                    {
                        string title;
                        if (info.Prob < info.ProbTotal)
                        {
                            title = string.Format("{2:P2}({0}/{1}) 확률로 스킬 사용 가능 :", info.Prob, info.ProbTotal, info.Prob * 1.0 / info.ProbTotal);
                        }
                        else
                        {
                            title = "스킬 사용 가능 :";
                        }
                        TextRenderer.DrawText(g, title, GearGraphics.EquipDetailFont, new Point(10, picHeight), Color.White, TextFormatFlags.NoPadding);
                        picHeight += 15;
                        foreach (var kv in info.EquipmentSkills)
                        {
                            StringResult sr = null;
                            if (this.StringLinker != null)
                            {
                                this.StringLinker.StringSkill.TryGetValue(kv.Key, out sr);
                            }
                            string text = string.Format(" {0} {2}레벨", sr == null ? null : sr.Name, kv.Key, kv.Value);
                            TextRenderer.DrawText(g, text, GearGraphics.EquipDetailFont, new Point(10, picHeight), ((SolidBrush)GearGraphics.OrangeBrush).Color, TextFormatFlags.NoPadding);
                            picHeight += 15;
                        }
                    }
                    if (info.Exp > 0)
                    {
                        TextRenderer.DrawText(g, "EXP : " + info.Exp + "%", GearGraphics.EquipDetailFont, new Point(10, picHeight), Color.White, TextFormatFlags.NoPadding);
                        picHeight += 15;
                    }

                    picHeight += 2;
                }
            }


            format.Dispose();
            if (g != null)
            {
                g.Dispose();
                picHeight += 13;
            }
            return level;
        }

        private bool TryGetNickResource(long nickTag, out Wz_Node resNode)
        {
            resNode = PluginBase.PluginManager.FindWz("UI/NameTag.img/nick/" + nickTag);
            return resNode != null;
        }
    }
}