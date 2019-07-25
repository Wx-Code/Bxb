using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class ImageHelper
    {
        private static Dictionary<string, Image> cacheWater;

        public static byte[] GetIMGbyte(string PicturePath)
        {
            //将需要存储的图片读取为数据流
            FileStream fs = new FileStream(@PicturePath, FileMode.Open, FileAccess.Read);
            Byte[] btye2 = new byte[fs.Length];
            fs.Read(btye2, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return btye2;
        }

        public static Image ByteToImage(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream(data);
            return Image.FromStream(stream);
        }


        public static byte[] ImageToByte(Image image)
        {
            if (image == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            byte[] buffer = null;
            image.Save(stream, ImageFormat.Jpeg);
            buffer = stream.GetBuffer();
            image.Dispose();
            return buffer;
        }

        public static byte[] Water(byte[] imgData, int x, int y, int rx, int ry, byte[] waterData)
        {
            Image image = ByteToImage(imgData);
            Image image2 = ByteToImage(waterData);
            if (image2 != null)
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                int width = 0;
                int height = 0;
                width = rx;
                height = ry;
                graphics.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                graphics.DrawImage(image2, new Rectangle(x, y, width, height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
                imgData = ImageToByte(bitmap);
                bitmap.Dispose();
            }
            image.Dispose();
            return imgData;
        }

        public static byte[] GetImgBytes(string url)
        {
            HttpWebRequest req2 = null;
            try
            {
                req2 = (WebRequest.Create(url) as HttpWebRequest);
                req2.Credentials = CredentialCache.DefaultCredentials;
                req2.Method = "GET";
                req2.AllowWriteStreamBuffering = false;
                req2.ServicePoint.UseNagleAlgorithm = false;
                req2.ServicePoint.ConnectionLimit = 65500;
                req2.Timeout = 3000;
                req2.Proxy = null;
                req2.AllowAutoRedirect = true;
                req2.KeepAlive = true;
                HttpWebResponse res = (HttpWebResponse)req2.GetResponse();
                Stream receiveStream = res.GetResponseStream();
                MemoryStream stmMemory = new MemoryStream();
                byte[] bf = new byte[4096];
                int i;
                while ((i = receiveStream.Read(bf, 0, bf.Length)) > 0)
                {
                    stmMemory.Write(bf, 0, i);
                }
                byte[] buffer = stmMemory.ToArray();
                stmMemory.Close();
                res.Close();
                receiveStream.Close();
                return buffer;
            }
            catch (Exception ex)
            {               
                return null;
            }
            finally
            {
                req2?.Abort();
            }
        }

        public static byte[] WaterText(byte[] imgData, string text, int x, int y, Color color, Font font)
        {
            Image image = ByteToImage(imgData);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                Font font2 = font;
                SizeF ef = graphics.MeasureString(text, font);
                Brush brush = new SolidBrush(color);
                graphics.DrawString(text, font2, brush, (float)x, (float)y);
                graphics.Dispose();
            }
            imgData = ImageToByte(image);
            image.Dispose();
            return imgData;
        }

        public static byte[] WaterTextCenter(byte[] imgData, string text, int y, Color color, Font font)
        {
            Image image = ByteToImage(imgData);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                Font font2 = font;
                SizeF ef = graphics.MeasureString(text, font);
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };
                Brush brush = new SolidBrush(color);
                graphics.DrawString(text, font2, brush, new Rectangle(0, y, image.Width, image.Height - y), format);
                graphics.Dispose();
            }
            imgData = ImageToByte(image);
            image.Dispose();
            return imgData;
        }

        public static byte[] WaySTwo(byte[] data)
        {
            Image image = ByteToImage(data);
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                using (GraphicsPath path = new GraphicsPath(FillMode.Alternate))
                {
                    path.AddEllipse(0, 0, image.Width, image.Height);
                    graphics.FillPath(new TextureBrush(image), path);
                }
            }
            data = ImageToByte(bitmap);
            image.Dispose();
            bitmap.Dispose();
            return data;
        }

        public static byte[] ZoomAuto(byte[] data, int targetWidth, int targetHeight)
        {
            Image image = ZoomAuto(ByteToImage(data), targetWidth, targetHeight);
            data = ImageToByte(image);
            image.Dispose();
            return data;
        }

        public static Image ZoomAuto(Image image, int targetWidth, int targetHeight)
        {
            if ((image == null) || ((image.Width <= targetWidth) && (image.Height <= targetHeight)))
            {
                return image;
            }
            int height = 0;
            int width = 0;
            if ((targetWidth < image.Width) && (targetWidth > 0))
            {
                width = targetWidth;
                height = (int)(image.Height * (((float)targetWidth) / ((float)image.Width)));
            }
            else
            {
                width = image.Width;
                height = image.Height;
            }
            if ((targetHeight < height) && (targetHeight > 0))
            {
                width = (int)(width * (((float)targetHeight) / ((float)height)));
                height = targetHeight;
            }
            Image image2 = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image2);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.Invalid;
            graphics.Clear(Color.White);
            graphics.DrawImage(image, new Rectangle(0, 0, image2.Width, image2.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            graphics.Dispose();
            image.Dispose();
            return image2;
        }
    }
}
