using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace xToolBarcode;

internal static class Code128Png {
    private const int BarcodeWidth = 180;
    private const int BarcodeHeight = 50;
    private const int ImageWidth = 230;
    private const int ImageHeight = 100;
    private const int TextGap = 2;

    public static void Save(string value, string path) {
        var writer = new BarcodeWriterPixelData {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions {
                Width = BarcodeWidth,
                Height = BarcodeHeight,
                Margin = 0,
                PureBarcode = true
            },
            Renderer = new PixelDataRenderer {
                Foreground = new PixelDataRenderer.Color(unchecked((int)0xFF000000)),
                Background = new PixelDataRenderer.Color(0)
            }
        };

        var pixelData = writer.Write(value);
        if (pixelData.Width != BarcodeWidth || pixelData.Height != BarcodeHeight) {
            throw new InvalidOperationException(
                $"Barcode for {value} was {pixelData.Width}x{pixelData.Height}, expected {BarcodeWidth}x{BarcodeHeight}.");
        }

        var pixels = pixelData.Pixels;
        var hasOpaquePixel = false;
        for (var i = 3; i < pixels.Length; i += 4) {
            if (pixels[i] != 0) {
                hasOpaquePixel = true;
                break;
            }
        }

        for (var i = 0; i < pixels.Length; i += 4) {
            var dark = pixels[i] < 128 && pixels[i + 1] < 128 && pixels[i + 2] < 128;
            var isBar = hasOpaquePixel ? dark && pixels[i + 3] != 0 : dark;
            if (isBar) {
                pixels[i] = 0;
                pixels[i + 1] = 0;
                pixels[i + 2] = 0;
                pixels[i + 3] = 255;
            } else {
                pixels[i] = 0;
                pixels[i + 1] = 0;
                pixels[i + 2] = 0;
                pixels[i + 3] = 0;
            }
        }

        using var barcode = new Bitmap(BarcodeWidth, BarcodeHeight, PixelFormat.Format32bppArgb);
        var data = barcode.LockBits(new Rectangle(0, 0, BarcodeWidth, BarcodeHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        try {
            var rowBytes = BarcodeWidth * 4;
            for (var y = 0; y < BarcodeHeight; y++) {
                Marshal.Copy(pixels, y * rowBytes, IntPtr.Add(data.Scan0, y * data.Stride), rowBytes);
            }
        } finally {
            barcode.UnlockBits(data);
        }

        using var bitmap = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bitmap)) {
            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var barcodeX = (ImageWidth - BarcodeWidth) / 2;
            var barcodeY = (ImageHeight - BarcodeHeight) / 2;
            graphics.DrawImageUnscaled(barcode, barcodeX, barcodeY);

            var textTop = barcodeY + BarcodeHeight + TextGap;
            using var font = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            var textRect = new RectangleF(0, textTop, ImageWidth, ImageHeight - textTop);
            using var format = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            };
            graphics.DrawString(value, font, Brushes.Black, textRect, format);
        }

        bitmap.Save(path, ImageFormat.Png);
    }
}
