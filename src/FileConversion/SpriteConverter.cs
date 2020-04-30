using System.IO;

namespace FileConversion
{

    class SpriteConverter
    {
        /*
         * Sprite Graphics
 
        Occurrences
 
        hpointer.dat/hpointer.tab; hspr-?-d.dat/hspr-?-d.tab; mfnt-0.dat/mfnt-0.tab; mspr-0-d.dat/mspr-0-d.tab
         Format
 
        The various sprite files make heavy use of the eight-pixel blocks as described in the tile section. If you haven't read that yet then you should do so now or else you won't be able to understand the following sections.
         Each sprite group is comprised of two files; a .dat file that contains the actual pixel data of the sprites and a .tab file that is used to index into the .dat file in order to find the relevant pixel data.
         The .tab files are simply a continuous collection of the following structures, which we'll refer to as tab entries: struct TabEntry {
            Uint32  offset;
            Uint8   width;
            Uint8   height;
        }; 
        The offset field is in little endian format, and gives the absolute position in the .dat file where the pixel data for the sprite is located. The width and height fields obviously give the dimensions of the relevant sprite.
         Each sprite is stored as a sequence of pixel lines, with the lines in bottom to top order as read in sequentially from the .dat file, similar to the lines of each subtile within hblk01.dat. Each line within the sprite is comprised of the smallest possible number of complete 8-pixel blocks that can represent the entire width of the sprite; as such if the width of a sprite is not a multiple of eight there will be extra "padding" pixels within the last pixel block of every line that are not considered part of the sprite, and are usually transparent.
                 */

        public void Process(BinaryReader reader)
        {
            // Grab # sprites in file
            int spriteCount = reader.ReadInt16();

            if (spriteCount != 0)
            {
                // Apparently if not zero, then data is RLE encoded
            }
            else
            {
                // That nice weird packing format ^______^
                // TODO: Forach...
                Block8 b8 = new Block8();
                b8.alpha = reader.ReadByte();
                b8.bit0 = reader.ReadByte();
                b8.bit1 = reader.ReadByte();
                b8.bit2 = reader.ReadByte();
                b8.bit3 = reader.ReadByte();


               // var bmp = new Bitmap()


            }

        }

    }
}
