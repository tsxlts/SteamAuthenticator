using System.Drawing.Imaging;

namespace Steam_Authenticator.Controls
{

    internal class CustomIcon
    {
        private readonly Icon source;
        private readonly Options options;

        public CustomIcon(Icon icon, Options options)
        {
            source = icon;
            this.options = options;
        }

        public string Name { get; set; }

        public Icon Icon => options.Convert(source);

        public static Icon ConvertToGrayscale(Icon icon)
        {
            Bitmap original = icon.ToBitmap();
            Bitmap newBmp = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBmp);
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                   new float[] {.3f, .3f, .3f, 0, 0},
                   new float[] {.59f, .59f, .59f, 0, 0},
                   new float[] {.11f, .11f, .11f, 0, 0},
                   new float[] {0, 0, 0, 1, 0},
                   new float[] {0, 0, 0, 0, 1}
               });
            ImageAttributes img = new ImageAttributes();
            img.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, img);
            g.Dispose();

            return Icon.FromHandle(newBmp.GetHicon());
        }

        public class Options
        {
            public delegate Icon IconConvert(Icon icon);

            public IconConvert Convert { get; set; }
        }
    }
}
