using System.IO;

namespace FileConversion
{


    class Program
    {
        static void Main(string[] args)
        {
            BinaryReader blkreader = new BinaryReader(File.OpenRead(@"Data/hblk01.unc"));
            BinaryReader palreader = new BinaryReader(File.OpenRead(@"Data/hpal01.unc"));

            Palettecolour[] pal = new Palettecolour[256];
            for (var i = 0; i < 255; ++i)
            {
                pal[i] = new Palettecolour();
                // palette in BGR not RGB
                pal[i].Red = (byte)((int)palreader.ReadByte() * 4);
                pal[i].Green = (byte)((int)palreader.ReadByte() * 4);
                pal[i].Blue = (byte)((int)palreader.ReadByte() * 4);               
            }

            /*
            HBlkConverter r = new HBlkConverter();
            var bmp = r.Process(blkreader,pal);
            // Save
            bmp.Save(@"C:\temp\test.png", ImageFormat.Png);
            */

            /*
            MapConverter m = new MapConverter();
            m.Process(
                new BinaryReader(File.OpenRead(@"Data/MAP01.unc")),
                new StreamWriter(@"Data/test.tmx")
                );
            */

        }

 
    }
}
