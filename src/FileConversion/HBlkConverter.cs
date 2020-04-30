using System;
using System.IO;
using System.Drawing;


namespace FileConversion
{
    class SubTile
    {
        public Block32[] Lines = new Block32[16];
        public Bitmap BlockImage;
    }

    class MapTile
    {
        // Raw offsets
        public UInt32[,] Offset = new UInt32[256, 6];

        public UInt32[,] SubTileIds = new UInt32[256, 6];

        public SubTile[] Sub = new SubTile[986];
    }

    // For tiles
    class Block32
    {
        public UInt32 alpha;
        public UInt32 bit0;
        public UInt32 bit1;
        public UInt32 bit2;
        public UInt32 bit3;
    }

    class HBlkConverter
    {
        UInt32[] _TileLookupTable = new UInt32[1536];

        public Bitmap Process(BinaryReader reader, Palettecolour[] pal)
        {
            MapTile mt = new MapTile();
            var currOffset = 6144;
            // Read map tile lookup table
            for (var tileid = 0; tileid < 256; ++tileid)
            {
                for (var j = 0; j < 6; ++j)
                {
                    byte[] z = new byte[4];
                    z[0] = reader.ReadByte();
                    z[1] = reader.ReadByte();
                    z[2] = reader.ReadByte();
                    z[3] = reader.ReadByte();
                    UInt32 x = BitConverter.ToUInt32(z, 0);
                    int zp = ((int)x - 6144);
                    if (zp < 0)
                        zp = 0;


                    zp /= (5 * 4 * 16);

                    mt.SubTileIds[tileid, j] = (UInt32)zp;

                    //currOffset += 4;
                    mt.Offset[tileid, j] = x;
                    
                }
            }

            // 		x	7744	uint
            // 1600 / 320

            for (var t = 0; t < 986; t++)
            {

                SubTile st = new SubTile();
                mt.Sub[t] = st;
                for (var zz = 0; zz < 16; ++zz)
                {
                    Block32 b32 = new Block32();
                    b32.alpha = reader.ReadUInt32();
                    b32.bit0 = reader.ReadUInt32();
                    b32.bit1 = reader.ReadUInt32();
                    b32.bit2 = reader.ReadUInt32();
                    b32.bit3 = reader.ReadUInt32();

                    st.Lines[zz] = b32;
                }
            }

            

   
            // 32w, 16h
            //int []px = new int[32*16];
            foreach(var subBlock in mt.Sub)
            {
                
                subBlock.BlockImage = new Bitmap(32, 16);

                for (var line = 0; line < 16; ++line)
                {
                    for (var x = 0; x < 32; ++x)
                    {
                        System.Collections.BitArray ba = new System.Collections.BitArray(8, false);

                        ba[0] = (subBlock.Lines[line].bit0 & (1 << x)) != 0;
                        ba[1] = (subBlock.Lines[line].bit1 & (1 << x)) != 0;
                        ba[2] = (subBlock.Lines[line].bit2 & (1 << x)) != 0;
                        ba[3] = (subBlock.Lines[line].bit3 & (1 << x)) != 0;
                        ba[4] = false;
                        ba[5] = false;
                        ba[6] = false;
                        ba[7] = false;

                        byte[] p = new byte[1];
                        ba.CopyTo(p, 0);
                        //px[(line * 32) + x] = p[0];

                        byte alpha = (byte)((subBlock.Lines[line].alpha & (1 << x)) != 0 ? 0 : 255);

                        Palettecolour pc = pal[p[0]];

                        //////////////////////
                        // 0       7

                        var px = 0;
                        var py = line;

                        if (x < 8)
                            px = (24 + (x % 8));
                        else if (x < 16)
                            px = (16 + (x % 8));
                        else if (x < 24)
                            px = (8 + (x % 8));
                        else if (x < 32)
                            px = (0 + (x % 8));

                        subBlock.BlockImage.SetPixel(31 - px, py, Color.FromArgb(alpha, pc.Red, pc.Green, pc.Blue));

                        //Console.Write( (p[0] > 0 ? 1 : 0));

                    }//x
                    //Console.WriteLine();

                    
                } // line

                
            }
            
            var myTile = 33;
            //Bitmap bmp = new Bitmap(64, 48);
            // 3 0
            // 4 1
            // 5 2

            // Lay out thhe tiles 16 wide by 16 down

            int row= 0, col = 0;
            int numrows = 16, numcols = 16;

            Bitmap bmp = new Bitmap(2 * 32 * numcols, 16 * 3* numrows);
            for (var t = 0; t < 256; t++)
            {
                if (col >= numcols)
                {
                    ++row;
                    col = 0;
                }

                var xpos = col * 64;
                var ypos = (row * 48);

                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 0]].BlockImage, xpos + 0, ypos + 0);
                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 3]].BlockImage, xpos + 32, ypos + 0);
                // 2
                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 1]].BlockImage, xpos + 0, ypos + 16);
                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 4]].BlockImage, xpos + 32, ypos + 16);
                // 3
                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 2]].BlockImage, xpos + 0, ypos + 32);
                bmp.BlitIntoBitmap(mt.Sub[mt.SubTileIds[t, 5]].BlockImage, xpos + 32, ypos + 32);

                ++col;
            }


            return bmp;

        }

    }
}
