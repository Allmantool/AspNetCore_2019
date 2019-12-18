using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;

namespace Northwind.Common.Extensions
{
    public static class ImageExtensions
    {
        public static ImageFormat GetImageFormat(this System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            if (img.RawFormat.Equals(ImageFormat.Wmf))
                return ImageFormat.Wmf;
            else
                return null;
        }

        public static bool IsImageFormatOrExtension(string formatOrExtension)
        {
            formatOrExtension = formatOrExtension.Replace(".", string.Empty);

            if (ImageFormat.Jpeg.ToString().Contains(formatOrExtension,StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Bmp.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Png.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Emf.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Exif.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Gif.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Icon.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.MemoryBmp.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Tiff.ToString()))
                return true;
            if (formatOrExtension.Equals(ImageFormat.Wmf.ToString()))
                return true;
            else
                return false;
        }
    }
}
