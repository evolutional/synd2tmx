using System;
using System.Collections.Generic;
using System.IO;

namespace FileConversion
{
   
    class MapConverter
    {
        /// <summary>
        /// Convert raw (unpacked) syndicate map file to TMX map file format (XML)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        public void Process(BinaryReader reader, TextWriter writer)
        {
            const int GridTileWidth = 64;
            const int GridTileHeight = 32;

            var mapWidth = reader.ReadInt32();
            var mapHeight = reader.ReadInt32();
            var mapDepth = reader.ReadInt32();

            int[] offsets = new int[mapWidth * mapHeight];

            for (var o = 0; o < offsets.Length; ++o)
            {
                offsets[o] = reader.ReadInt32() + 12;
            }

            byte[, ,] mapdata = new byte[mapWidth, mapHeight, mapDepth];

            for (int j = 0; j < mapHeight; j++)
            {                    // y
                for (int i = 0; i < mapWidth; i++)
                {               // x
                    reader.BaseStream.Seek(offsets[i + (j * mapWidth)], SeekOrigin.Begin);

                    for (int k = 0; k < mapDepth; k++)
                    {
                        // mapDepth tiles per column
                        mapdata[i, j, k] = reader.ReadByte();
                    }
                }
            }

            // TODO: We need to have the tilesheet size passed in, this is currently hard coded
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            writer.WriteLine("<map version=\"1.0\" orientation=\"isometric\" width=\"{0}\" height=\"{1}\" tilewidth=\"{2}\" tileheight=\"{3}\">", mapWidth, mapHeight, GridTileWidth, GridTileHeight);
            writer.WriteLine("<tileset firstgid=\"1\" name=\"test\" tilewidth=\"64\" tileheight=\"48\">");
            // TODO: Need to pass in sprite dimensions and number of tiles X and Y
            // TODO: Need to pass in tilesheet name
            writer.WriteLine("<image source=\"test.png\" width=\"1024\" height=\"768\"/>");
            writer.WriteLine("</tileset>");

            // Do every layer...
            for (var k = 0; k < 12; ++k)
            {

                writer.WriteLine("<layer name=\"Tile Layer {0}\" width=\"{1}\" height=\"{2}\">", k, mapWidth, mapHeight);
                writer.WriteLine("<data encoding=\"csv\">");

                for (int j = 0; j < mapHeight; j++)
                {                    // y
                    string linebuffer = "";
                    for (int i = 0; i < mapWidth; i++)
                    {
                        var t = (int)mapdata[i, j, k];
                        linebuffer += string.Format("{0},", (t == 0 ? 0 : t + 1));

                        if ((j == mapHeight - 1) && (i == mapWidth - 1))
                        {
                            linebuffer = linebuffer.Substring(0, linebuffer.Length - 1);
                        }

                    }


                    writer.WriteLine(linebuffer);
                }

                writer.WriteLine("</data></layer>");
            }
            writer.WriteLine("</map>");
            writer.Flush();
            writer.Close();

        }

    }
}
