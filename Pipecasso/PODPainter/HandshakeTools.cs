using PodHandshake;
using System.Runtime.InteropServices;
using System.Text;

namespace PodHandshake
{
    class HandshakeTools
    {
        #region Dll Directory functions
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetDllDirectory(int nBufferLength, StringBuilder lpPathName);
        #endregion

        

        internal static string GetPODHandshake(string bwfcFolder)
        {
            const int MAX_PATH = 1025;

            string lastDllDirectory = null;
            StringBuilder tmp = new StringBuilder(MAX_PATH); //MAX_PATH + 1
            if (GetDllDirectory(MAX_PATH, tmp) != 0)
            {
                lastDllDirectory = tmp.ToString();
            }

            SetDllDirectory(bwfcFolder);
            StringBuilder handshake = BuildPODHandshake();

            SetDllDirectory(lastDllDirectory);

            return handshake.ToString();
        }

        private static StringBuilder BuildPODHandshake()
        {
            PODHandshaker handshaker = new PODHandshaker();
            return handshaker.GetHandshake(HandshakeStrings.PodStaticKey, HandshakeStrings.DevId, "POD");
        }
    }
}
