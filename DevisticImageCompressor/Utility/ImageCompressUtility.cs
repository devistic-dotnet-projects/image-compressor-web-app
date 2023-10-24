using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DevisticImageCompressor
{
    public static class ImageCompressUtility
    {
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 2000.0f;
                    float maxWidth = 2000.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;
                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(filename);
                    extension = extension.ToLower();
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                        ////Watermark
                        //Font font = new Font("Calibri", 25, FontStyle.Regular, GraphicsUnit.Pixel);
                        //Color color = Color.FromArgb(0, 0, 0);
                        //var x = 30 / 2;
                        //Point atpoint = new Point(bitMAP1.Width / 8, bitMAP1.Height / x);
                        //SolidBrush brush = new SolidBrush(color);
                        //Graphics graphics = Graphics.FromImage(bitMAP1);
                        //StringFormat sf = new StringFormat();
                        //sf.Alignment = StringAlignment.Center;
                        //sf.LineAlignment = StringAlignment.Center;
                        //graphics.DrawString("iProvide", font, brush, atpoint, sf);
                        //graphics.Dispose();
                        ////Watermark

                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg" || extension == ".jpeg")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        ////Watermark
                        //Font font = new Font("Calibri", 25, FontStyle.Regular, GraphicsUnit.Pixel);
                        //Color color = Color.FromArgb(0, 0, 0);
                        //var x = 30 / 2;
                        //Point atpoint = new Point(bitMAP1.Width / 8, bitMAP1.Height / x);
                        //SolidBrush brush = new SolidBrush(color);
                        //Graphics graphics = Graphics.FromImage(bitMAP1);
                        //StringFormat sf = new StringFormat();
                        //sf.Alignment = StringAlignment.Center;
                        //sf.LineAlignment = StringAlignment.Center;
                        //graphics.DrawString("iProvide", font, brush, atpoint, sf);
                        //graphics.Dispose();
                        ////Watermark

                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }

                }

            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
            }
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}