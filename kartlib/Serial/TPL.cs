using kartlib.Imaging;
using System.Drawing;

namespace kartlib.Serial
{
    public class TPL
    {
        public class _FileHeader
        {
            public UInt32 Version;
            public UInt32 ImageCount;
            public UInt32 ImageTableOffset;

            public _FileHeader()
            {
                Version = 0x00;
                ImageCount = 1;
                ImageTableOffset = 0x0C;
            }

            public _FileHeader(EndianReader reader)
            {
                Version = reader.ReadUInt32();
                ImageCount = reader.ReadUInt32();
                ImageTableOffset = reader.ReadUInt32();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteUInt32(Version);
                writer.WriteUInt32(ImageCount);
                writer.WriteUInt32(ImageTableOffset);
            }
        }

        public class _ImageTable
        {
            public UInt32 ImageOffset;
            public UInt32 PaletteOffset;

            public _ImageTable() { }

            public _ImageTable(EndianReader reader)
            {
                ImageOffset = reader.ReadUInt32();
                PaletteOffset = reader.ReadUInt32();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteUInt32(ImageOffset);
                writer.WriteUInt32(PaletteOffset);
            }
        }

        public class _PaletteHeader
        {
            public enum PaletteFormat : int
            {
                IA8 = 0x00,
                RGB565 = 0x01,
                RGB5A3 = 0x02,
            }

            public UInt16 EntryCount;
            public Byte Unpacked;
            public Byte Reserved;
            public PaletteFormat Format;
            public UInt32 DataAddress;

            public _PaletteHeader() { }

            public _PaletteHeader(EndianReader reader)
            {
                EntryCount = reader.ReadUInt16();
                Unpacked = reader.ReadByte();
                Reserved = reader.ReadByte();
                Format = (PaletteFormat)reader.ReadUInt32();
                DataAddress = reader.ReadUInt32();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteUInt16(EntryCount);
                writer.WriteByte(Unpacked);
                writer.WriteByte(Reserved);
                writer.WriteUInt32((uint)Format);
                writer.WriteUInt32(DataAddress);
            }
        }

        public class _ImageHeader
        {
            public UInt16 Height;
            public UInt16 Width;
            public ImageFormatEnum Format;
            public UInt32 DataAddress;
            public UInt32 WrapS;
            public UInt32 WrapT;
            public UInt32 MinFilter;
            public UInt32 MagFilter;
            public float LODBias;
            public Byte EdgeLODEnable;
            public Byte MinLOD;
            public Byte MaxLOD;
            public Byte Unpacked;

            public _ImageHeader()
            {
            }

            public _ImageHeader(EndianReader reader)
            {
                Height = reader.ReadUInt16();
                Width = reader.ReadUInt16();
                Format = (ImageFormatEnum)reader.ReadUInt32();
                DataAddress = reader.ReadUInt32();
                WrapS = reader.ReadUInt32();
                WrapT = reader.ReadUInt32();
                MinFilter = reader.ReadUInt32();
                MagFilter = reader.ReadUInt32();
                LODBias = reader.ReadFloat();
                EdgeLODEnable = reader.ReadByte();
                MinLOD = reader.ReadByte();
                MaxLOD = reader.ReadByte();
                Unpacked = reader.ReadByte();
            }

            public void Write(EndianWriter writer)
            {
                writer.WriteUInt16(Height);
                writer.WriteUInt16(Width);
                writer.WriteUInt32((uint)Format);
                writer.WriteUInt32(DataAddress);
                writer.WriteUInt32(WrapS);
                writer.WriteUInt32(WrapT);
                writer.WriteUInt32(MinFilter);
                writer.WriteUInt32(MagFilter);
                writer.WriteSingle(LODBias);
                writer.WriteByte(EdgeLODEnable);
                writer.WriteByte(MinLOD);
                writer.WriteByte(MaxLOD);
                writer.WriteByte(Unpacked);
            }
        }

        public class _Image
        {
            public _ImageHeader ImageHeader;
            public _PaletteHeader? PaletteHeader;
            public ImageFormat Format;
            public byte[] ImageData;
            public byte[]? PaletteData;

            public Bitmap? Image;

            public _Image() { }

            public _Image (EndianReader reader, _ImageTable table)
            {
                // Palette Header/Data
                if (table.PaletteOffset != 0)
                {
                    reader.Position = (int)table.PaletteOffset;
                    PaletteHeader = new _PaletteHeader(reader);

                    reader.Position = (int)PaletteHeader.DataAddress;
                    PaletteData = reader.ReadBytes(PaletteHeader.EntryCount * 2);
                }

                // Image Header/Data
                reader.Position = (int)table.ImageOffset;
                ImageHeader = new _ImageHeader(reader);

                ImageFormat? format = ImageFactory.GetFormat(ImageHeader.Format);
                if(format != null) Format = format;
                else               return;

                int sizeInBytes = (int)(ImageHeader.Width * ImageHeader.Height * ((double)Format.BitsPerPixel / 8));

                reader.Position = (int)ImageHeader.DataAddress;
                ImageData = reader.ReadBytes(sizeInBytes);

                Image = Format.ToBitmap(ImageData, ImageHeader.Width, ImageHeader.Height, PaletteData, PaletteHeader?.Format);
            }

            public ImageFormatEnum GetFormat()
            {
                return ImageHeader.Format;
            }
        }

        public string Filename;
        public _FileHeader FileHeader;
        public List<_ImageTable> ImageTables;
        public List<_Image> Images;

        private ImageFormat Format;

        public TPL(string filename = "untitled.tpl")
        {
            Filename = filename;

            FileHeader = new _FileHeader
            {
                Version = 0x0020AF30,
                ImageCount = 0,
                ImageTableOffset = 0x0C
            };

            ImageTables = new List<_ImageTable>();
            Images = new List<_Image>();
        }

        public TPL(byte[] buffer, string filename)
        {
            Filename = filename;
            EndianReader reader = new EndianReader(buffer, Endianness.BigEndian);
            try
            {
                FileHeader = new _FileHeader(reader);
                ImageTables = new List<_ImageTable>();
                Images = new List<_Image>();

                for(int i = 0; i < FileHeader.ImageCount; i++)
                    ImageTables.Add(new _ImageTable(reader));

                foreach(_ImageTable table in ImageTables)
                    Images.Add(new _Image(reader, table));
            }
            finally
            {
                reader.Close();
            }
        }

        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                EndianWriter writer = new EndianWriter(ms, Endianness.BigEndian);

                uint fileHeaderSize = 12;
                uint imageTableSize = 8;
                uint imageHeaderSize = 36;
                uint paletteHeaderSize = 12;

                FileHeader.ImageCount = (uint)Images.Count;
                FileHeader.ImageTableOffset = fileHeaderSize;
                FileHeader.Write(writer);

                uint currentHeaderOffset = fileHeaderSize + (uint)(Images.Count * imageTableSize);
                uint currentDataOffset = currentHeaderOffset;

                foreach (var img in Images)
                {
                    currentDataOffset += imageHeaderSize;
                    if (img.PaletteHeader != null)
                        currentDataOffset += paletteHeaderSize;
                }

                currentDataOffset = (currentDataOffset + 31) & ~31u;

                for (int i = 0; i < Images.Count; i++)
                {
                    var img = Images[i];
                    var table = ImageTables[i];

                    table.ImageOffset = currentHeaderOffset;
                    currentHeaderOffset += imageHeaderSize;

                    if (img.PaletteHeader != null)
                    {
                        table.PaletteOffset = currentHeaderOffset;
                        currentHeaderOffset += paletteHeaderSize;

                        img.PaletteHeader.DataAddress = currentDataOffset;
                        currentDataOffset += (uint)img.PaletteData.Length;
                        currentDataOffset = (currentDataOffset + 31) & ~31u; // 32-byte alignment after palette
                    }
                    else
                    {
                        table.PaletteOffset = 0;
                    }

                    img.ImageHeader.DataAddress = currentDataOffset;
                    currentDataOffset += (uint)img.ImageData.Length;
                    currentDataOffset = (currentDataOffset + 31) & ~31u; // 32-byte alignment after image

                    table.Write(writer);
                }

                foreach (var img in Images)
                {
                    img.ImageHeader.Write(writer);
                    if (img.PaletteHeader != null)
                        img.PaletteHeader.Write(writer);
                }

                foreach (var img in Images)
                {
                    if (img.PaletteHeader != null)
                    {
                        while (ms.Position < img.PaletteHeader.DataAddress) writer.WriteByte(0);
                        writer.WriteBytes(img.PaletteData);
                    }

                    while (ms.Position < img.ImageHeader.DataAddress) writer.WriteByte(0);
                    writer.WriteBytes(img.ImageData);
                }

                return ms.ToArray();
            }
        }

        public void AddImage(Bitmap bitmap, ImageFormatEnum formatEnum, _PaletteHeader.PaletteFormat paletteFormat = _PaletteHeader.PaletteFormat.RGB5A3)
        {
            ImageFormat format = ImageFactory.GetFormat(formatEnum);
            if (format == null)
                throw new NotSupportedException($"ImageFormat {formatEnum} is not supported.");

            byte[] imageData = format.FromBitmap(bitmap);
            byte[] paletteData = format.LastGeneratedPalette;

            _ImageHeader imgHeader = new _ImageHeader
            {
                Width = (ushort)bitmap.Width,
                Height = (ushort)bitmap.Height,
                Format = formatEnum,
                WrapS = 0,
                WrapT = 0,
                MinFilter = 1,
                MagFilter = 1,
                LODBias = 0,
                EdgeLODEnable = 0,
                MinLOD = 0,
                MaxLOD = 0,
                Unpacked = 0
            };

            _PaletteHeader palHeader = null;
            if (paletteData != null && paletteData.Length > 0)
            {
                palHeader = new _PaletteHeader
                {
                    EntryCount = (ushort)(paletteData.Length / 2),
                    Unpacked = 0,
                    Reserved = 0,
                    Format = paletteFormat
                };
            }

            _Image newImage = new _Image
            {
                ImageHeader = imgHeader,
                PaletteHeader = palHeader,
                ImageData = imageData,
                PaletteData = paletteData,
                Format = format,
                Image = bitmap
            };

            Images.Add(newImage);
            ImageTables.Add(new _ImageTable());
            FileHeader.ImageCount = (uint)Images.Count;
        }

        public void RemoveImage(int index)
        {
            if (index < 0 || index >= Images.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Image index is out of bounds.");

            Images.RemoveAt(index);
            ImageTables.RemoveAt(index);
            FileHeader.ImageCount = (uint)Images.Count;
        }

        private byte[] CreateTPL(byte[] rawPixelData, ushort width, ushort height, ImageFormatEnum format, byte[]? paletteData = null, TPL._PaletteHeader.PaletteFormat paletteFormat = TPL._PaletteHeader.PaletteFormat.RGB5A3)
        {
            uint fileHeaderSize = 12;
            uint imageTableSize = 8;
            uint imageHeaderSize = 36;
            uint paletteHeaderSize = 12;

            uint imageTableOffset = fileHeaderSize;
            uint imageHeaderOffset = imageTableOffset + imageTableSize;

            uint paletteHeaderOffset = 0;
            uint paletteDataOffset = 0;
            uint rawDataOffset = 0;

            if (paletteData != null && paletteData.Length > 0)
            {
                paletteHeaderOffset = imageHeaderOffset + imageHeaderSize;
                paletteDataOffset = paletteHeaderOffset + paletteHeaderSize;
                rawDataOffset = paletteDataOffset + (uint)paletteData.Length;
            }
            else
            {
                rawDataOffset = imageHeaderOffset + imageHeaderSize;
            }

            uint padding = (32 - (rawDataOffset % 32)) % 32;
            rawDataOffset += padding;

            TPL._FileHeader fileHeader = new TPL._FileHeader();
            fileHeader.Version = 0x0020AF30;
            fileHeader.ImageCount = 1;
            fileHeader.ImageTableOffset = imageTableOffset;

            TPL._ImageTable table = new TPL._ImageTable();
            table.ImageOffset = imageHeaderOffset;
            table.PaletteOffset = paletteHeaderOffset;

            TPL._ImageHeader header = new TPL._ImageHeader();
            header.Height = height;
            header.Width = width;
            header.Format = format;
            header.DataAddress = rawDataOffset;
            header.WrapS = 0;
            header.WrapT = 0;
            header.MinFilter = 1;
            header.MagFilter = 1;
            header.LODBias = 0;
            header.EdgeLODEnable = 0;
            header.MinLOD = 0;
            header.MaxLOD = 0;
            header.Unpacked = 0;

            TPL._PaletteHeader? pHeader = null;
            if (paletteData != null && paletteData.Length > 0)
            {
                pHeader = new TPL._PaletteHeader();
                pHeader.EntryCount = (ushort)(paletteData.Length / 2);
                pHeader.Unpacked = 0;
                pHeader.Reserved = 0;
                pHeader.Format = paletteFormat;
                pHeader.DataAddress = paletteDataOffset;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                EndianWriter writer = new EndianWriter(ms, Endianness.BigEndian);

                fileHeader.Write(writer);
                table.Write(writer);
                header.Write(writer);

                if (pHeader != null && paletteData != null)
                {
                    pHeader.Write(writer);
                    writer.WriteBytes(paletteData);
                }

                long currentPos = ms.Position;
                long paddingNeeded = rawDataOffset - currentPos;
                for (int i = 0; i < paddingNeeded; i++)
                {
                    writer.WriteByte(0);
                }

                writer.WriteBytes(rawPixelData);

                return ms.ToArray();
            }
        }
    }
}
