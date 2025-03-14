
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Steam_Authenticator.Controls
{
    internal class ItemPanel : Panel
    {
        public readonly bool HasItem;

        public PictureBox ItemIcon;
        public Label ItemName;

        public ItemPanel(bool hasItem) : base()
        {
            this.HasItem = hasItem;

            BorderWidth = 1;
            BorderColor = Color.Transparent;
        }

        public ItemPanel SetItemIconBox(PictureBox icon)
        {
            this.ItemIcon = icon;
            this.Controls.Add(this.ItemIcon);
            return this;
        }

        public ItemPanel SetItemNameBox(Label name)
        {
            this.ItemName = name;
            this.Controls.Add(this.ItemName);
            return this;
        }

        public ItemPanel SetItemIcon(string icon)
        {
            ItemIcon?.LoadAsync(icon);
            return this;
        }

        public ItemPanel SetItemName(string name, Color color)
        {
            if (!HasItem)
            {
                return this;
            }

            ItemName.Text = name;
            ItemName.ForeColor = color;
            return this;
        }

        public string ItemDisplayName => HasItem ? ItemName?.Text : null;

        private Color bgColor = Color.LightGray;
        [DefaultValue(typeof(Color), "LightGray"), Description("控件的背景色1")]
        public Color BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                //Refresh();//立即重绘 
                Invalidate();//引发重绘，不会立即执行
            }
        }

        private Color bgColor2 = Color.Transparent;
        [DefaultValue(typeof(Color), "Transparent"), Description("控件的背景色2")]
        public Color BgColor2
        {
            get { return bgColor2; }
            set
            {
                bgColor2 = value;
                Invalidate();//引发重绘，不会立即执行
            }
        }

        private Color borderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray"), Description("控件的边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();//引发重绘，不会立即执行
            }
        }

        private int borderWidth = 0;
        [DefaultValue(typeof(int), "0"), Description("控件的边框粗细")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                Invalidate();//引发重绘，不会立即执行
            }
        }

        private int radius = 5;
        [DefaultValue(typeof(int), "5"), Description("控件的边框圆角半径")]
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                Invalidate();//引发重绘，不会立即执行
            }
        }

        private LinearGradientMode gradientMode = LinearGradientMode.Vertical;
        [DefaultValue(typeof(LinearGradientMode), "Vertical"), Description("控件的背景渐变模式")]
        public LinearGradientMode GradientMode
        {
            get { return gradientMode; }
            set
            {
                gradientMode = value;
                Invalidate();//引发重绘，不会立即执行
            }
        }

        Rectangle r;//绘制区域
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            r = this.ClientRectangle; //获取当前绘制区域
            this.Region = new Region(r);
            r.Width -= 1;
            r.Height -= 1;
        }

        //重写重绘
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //绘制对象
            Graphics g = e.Graphics;
            //呈现质量   HighQuality  AntiAlias
            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath path = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            //内部填充矩形
            Rectangle rect;
            //生成外部矩形的路径
            path = PaintClass.GetRoundRectangle(r, radius);

            if (this.BorderWidth > 0)
            {
                //填充外部矩形---边框
                g.FillPath(new SolidBrush(BorderColor), path);
                //定义内部矩形
                rect = new Rectangle(r.X + BorderWidth, r.Y + BorderWidth, r.Width - 2 * borderWidth, r.Height - 2 * borderWidth);
                //生成内部矩形的圆角路径
                path2 = PaintClass.GetRoundRectangle(rect, radius - 1);

            }
            else //无边框时的内部矩形
            {
                path2 = path;
                rect = r;
            }
            if (this.BgColor2 != Color.Transparent)
            {
                //线型渐变画刷
                LinearGradientBrush bgBrush = new LinearGradientBrush(rect, BgColor, BgColor2, GradientMode);
                g.FillPath(bgBrush, path2);//填充圆角矩形内部
            }
            else
            {
                Brush b = new SolidBrush(BgColor);
                g.FillPath(b, path2);//填充圆角矩形内部
            }
        }
    }
}
