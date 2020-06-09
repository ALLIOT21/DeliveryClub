using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeliveryClub.Infrastructure.Validation
{
    public class ImageValidator
    {
        private readonly Dictionary<string, byte[]> _imageHeaders = new Dictionary<string, byte[]>
        {
            {"JPG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }},
            {"JPEG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }},
            {"PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 }},
            {"TIF", new byte[] { 0x49, 0x49, 0x2A, 0x00 }},
            {"TIFF", new byte[] { 0x49, 0x49, 0x2A, 0x00 }},
            {"GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 }},
            {"BMP", new byte[] { 0x42, 0x4D }},
            {"ICO", new byte[] { 0x00, 0x00, 0x01, 0x00 }},
        };

        private string GetFileExtension(string imageName)
        {
            return imageName.Substring(imageName.LastIndexOf('.') + 1).ToUpper();
        }

        private byte[] GetCurrentFileHeader(Stream stream, int headerLength)
        {
            var buffer = new byte[headerLength];
            stream.Read(buffer, 0, headerLength);
            return buffer;
        }

        private byte[] GetCorrectFileHeader(string fileExtension) {
            if (_imageHeaders.ContainsKey(fileExtension))
            {
                return _imageHeaders[fileExtension];
            }
            else
                return null;
        }

        public bool Validate(IFormFile file)
        {
            var fileExt = GetFileExtension(file.FileName);
            var correctFileHeader = GetCorrectFileHeader(fileExt);

            if (correctFileHeader != null)
            {
                byte[] currentFileHeader;
                using (Stream currentFileStream = file.OpenReadStream())
                {
                    currentFileHeader = GetCurrentFileHeader(currentFileStream, correctFileHeader.Length);
                }

                return CompareHeaders(currentFileHeader, correctFileHeader);
            }

            return false; 
        }


        private bool CompareHeaders(byte[] target, byte[] source)
        {
            if (source.Length != target.Length)
                return false;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] != source[i])
                    return false;
            }
            return true;
        }
    }
}
