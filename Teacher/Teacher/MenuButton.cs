using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Teacher
{
    public partial class MenuButton : UserControl
    {
        // 定数
        public const int Radius = 24;
        public const int ColorDivCnt = 48;

        // フィールド
        public Color Color;
        public int ColorRatio;

        public Thread ThreadAnimeColorUp;
        public Thread ThreadAnimeColorDown;

        // プロパティ
        public string ButtonText { get; set; }
        public Point TextLocation { get; set; }
        public Color MainColor { get; set; }
        public Color HoverColor { get; set; }

        // コンストラクタ
        public MenuButton()
        {
            // プロパティデフォルト
            TextLocation = new Point(10, 10);
            Text = "";
            MainColor = Color.FromArgb(255, 192, 192);
            HoverColor = Color.FromArgb(255, 64, 64);

            // 初期化
            Color = MainColor;
            ColorRatio = 0;

            // ダブルバッファリング有効
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();
        }

        // 描画
        protected override void OnPaint(PaintEventArgs e)
        {
            // Console.WriteLine(TextLocation);

            Graphics g = e.Graphics;
            SolidBrush b = new SolidBrush(Color);

            g.FillRectangle(b, Radius / 2, 0, this.Width - Radius, this.Height);
            g.FillRectangle(b, 0, Radius / 2, this.Width, this.Height - Radius);

            g.FillPie(b, 0, 0, Radius, Radius, 0, -180);
            g.FillPie(b, this.Width - Radius, 0, Radius, Radius, 0, -180);

            g.FillPie(b, 0, this.Height - Radius, Radius, Radius, 0, 180);
            g.FillPie(b, this.Width - Radius, this.Height - Radius, Radius, Radius, 0, 180);

            g.DrawString(ButtonText, this.Font, Brushes.Black, TextLocation);
        }

        // マウスオーバー
        protected override void OnMouseEnter(EventArgs e)
        {
            if (ThreadAnimeColorDown != null)
            {
                ThreadAnimeColorDown.Abort();
                ThreadAnimeColorDown.Join();
                ThreadAnimeColorDown = null;
            }
            if (ThreadAnimeColorUp != null)
            {
                return;
            }

            ThreadAnimeColorUp = new Thread(new ThreadStart(AnimeColorUp));
            ThreadAnimeColorUp.Start();
        }

        // マウスアウト
        protected override void OnMouseLeave(EventArgs e)
        {
            if (ThreadAnimeColorUp != null)
            {
                ThreadAnimeColorUp.Abort();
                ThreadAnimeColorUp.Join();
                ThreadAnimeColorUp = null;
            }
            if (ThreadAnimeColorDown != null)
            {
                return;
            }

            ThreadAnimeColorDown = new Thread(new ThreadStart(AnimeColorDown));
            ThreadAnimeColorDown.Start();
        }

        // 色アニメーションUp
        private void AnimeColorUp()
        {
            while(ColorRatio <= ColorDivCnt)
            {
                Color = GetRatioAnimeColor();
                Invalidate(Region);
                ColorRatio++;
                Thread.Sleep(10);
            }
            ColorRatio = ColorDivCnt;
        }

        // 色アニメーションDown
        private void AnimeColorDown()
        {
            while (ColorRatio >= 0)
            {
                Color = GetRatioAnimeColor();
                Invalidate(Region);
                ColorRatio--;
                Thread.Sleep(10);
            }
            ColorRatio = 0;
        }

        // アニメーション色取得
        private Color GetRatioAnimeColor()
        {
            double mr = (double)(ColorDivCnt - ColorRatio) / ColorDivCnt;
            double hr = (double)ColorRatio / ColorDivCnt;

            double r = MainColor.R * mr + HoverColor.R * hr;
            double g = MainColor.G * mr + HoverColor.G * hr;
            double b = MainColor.B * mr + HoverColor.B * hr;

            return Color.FromArgb((int)r, (int)g, (int)b);
        }
    }
}
