using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace PinExceptionCons
{
    class FileIO
    {
        //public const uint MAX_PATH = 0x00000104;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        IntPtr handle;

        [Serializable,
            System.Runtime.InteropServices.StructLayout
                (System.Runtime.InteropServices.LayoutKind.Sequential,
                CharSet = System.Runtime.InteropServices.CharSet.Auto
                ),
            System.Runtime.InteropServices.BestFitMapping(false)]
        private struct WIN32_FIND_DATA
        {
            public int dwFileAttributes;
            public int ftCreationTime_dwLowDateTime;
            public int ftCreationTime_dwHighDateTime;
            public int ftLastAccessTime_dwLowDateTime;
            public int ftLastAccessTime_dwHighDateTime;
            public int ftLastWriteTime_dwLowDateTime;
            public int ftLastWriteTime_dwHighDateTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;
            [System.Runtime.InteropServices.MarshalAs
                 (System.Runtime.InteropServices.UnmanagedType.ByValTStr,
                 SizeConst = 260)]
            public string cFileName;
            [System.Runtime.InteropServices.MarshalAs
                 (System.Runtime.InteropServices.UnmanagedType.ByValTStr,
                 SizeConst = 14)]
            public string cAlternateFileName;
        }

        [System.Runtime.InteropServices.DllImport
             ("kernel32.dll",
             CharSet = System.Runtime.InteropServices.CharSet.Auto,
             SetLastError = true)]
        private static extern IntPtr FindFirstFile(string pFileName, ref WIN32_FIND_DATA pFindFileData);

        [DllImport("kernel32", SetLastError = true)]
        public static extern unsafe IntPtr MoveFile(string Source, string Destination);

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe IntPtr CreateFile(
              string FileName,                    // file name
              uint DesiredAccess,                 // access mode
              uint ShareMode,                     // share mode
              uint SecurityAttributes,            // Security Attributes
              uint CreationDisposition,           // how to create
              uint FlagsAndAttributes,            // file attributes
              int hTemplateFile                   // handle to template file
              );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile(
              IntPtr hFile,                       // handle to file
              void* pBuffer,                      // data buffer
              int NumberOfBytesToRead,            // number of bytes to read
              int* pNumberOfBytesRead,            // number of bytes read
              int Overlapped                      // overlapped buffer
              );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle(
              IntPtr hObject   // handle to object
              );

        public bool Open(string FileName, uint FileAccess, uint FileMode)
        {
            // open the existing file for reading          
            handle = CreateFile(
                  FileName,
                  FileAccess,
                  0,
                  0,
                  FileMode,
                  0,
                  0);

            if (handle != INVALID_HANDLE_VALUE)
                return true;
            else
                return false;
        }

        public unsafe int Read(byte[] buffer, int index, int count)
        {
            int n = 0;
            fixed (byte* p = buffer)
            {
                if (!ReadFile(handle, p + index, count, &n, 0))
                    return 0;
            }
            return n;
        }

        public bool Close()
        {
            // close file handle
            return CloseHandle(handle);
        }

        public bool FileExists(string FileName)
        {
            WIN32_FIND_DATA fd = new WIN32_FIND_DATA();

            if (FindFirstFile(FileName, ref fd) == INVALID_HANDLE_VALUE)
                return false;

            return true;
        }
    }
}
