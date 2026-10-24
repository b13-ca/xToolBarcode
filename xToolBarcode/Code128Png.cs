using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace xToolBarcode;

internal static class Code128Png {
    public const int Width = 180;
    public const int Height = 50;

    public static void Save(string value, string path) {
        var writer = new BarcodeWriterPixelData {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions {
                Width = Width,
                Height = Height,
                Margin = 0,
                PureBarcode = true
            },
            Renderer = new PixelDataRenderer {
                Foreground = new PixelDataRenderer.Color(unchecked((int)0xFF000000)),
                Background = new PixelDataRenderer.Color(0)
            }
        };

        var pixelData = writer.Write(value);
        if (pixelData.Width != Width || pixelData.Height != Height) {
            throw new InvalidOperationException(
                $"Barcode for {value} was {pixelData.Width}x{pixelData.Height}, expected {Width}x{Height}.");
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

        using var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
        var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        try {
            var rowBytes = Width * 4;
            for (var y = 0; y < Height; y++) {
                Marshal.Copy(pixels, y * rowBytes, IntPtr.Add(data.Scan0, y * data.Stride), rowBytes);
            }
        } finally {
            bitmap.UnlockBits(data);
        }

        bitmap.Save(path, ImageFormat.Png);
    }
}
