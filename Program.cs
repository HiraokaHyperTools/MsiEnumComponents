using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MsiEnumComponents {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 1 && args[0] == "/all") {
                new Program().Find(null);
            }
            else if (args.Length == 2 && args[0] == "/find") {
                new Program().Find(args[1]);
            }
            else if (args.Length == 2 && args[0] == "/productof") {
                new Program().Productof(args[1]);
            }
            else {
                Console.Error.WriteLine("MsiEnumComponents /all");
                Console.Error.WriteLine("MsiEnumComponents /find vcruntime140.dll");
                Console.Error.WriteLine("MsiEnumComponents /productof {83CCA637-410A-58E4-BD1E-8DE029108ADB}");
                Console.Error.WriteLine("MsiEnumComponents /productof {D282951C-BBCA-572D-83E5-CC72E934A4B2}");
                Console.Error.WriteLine("MsiEnumComponents /productof {E8E39D3B-4F35-36D8-B892-4B28336FE041}");
                Console.Error.WriteLine("MsiEnumComponents /productof {B33258FD-750C-3B42-8BE4-535B48E97DB4}");
                Environment.ExitCode = 1;
                return;
            }
        }

        private void Productof(String component) {
            StringBuilder tmp = new StringBuilder(1024);
            String product = myMsiGetProductCode(component, tmp);
            if (product == null) return;
            String ProductName = myMsiGetProductInfo(product, "ProductName", tmp);
            Console.WriteLine(product + "\t" + ProductName);
        }

        public enum INSTALLSTATE {
            NotUsed = -7,  // component disabled
            BadConfig = -6,  // configuration data corrupt
            Incomplete = -5,  // installation suspended or in progress
            Sourceabsent = -4,  // run from source, source is unavailable
            MoreData = -3,  // return buffer overflow
            InvalidArg = -2,  // invalid function argument
            Unknown = -1,  // unrecognized product or feature
            Broken = 0,  // broken
            Advertised = 1,  // advertised feature
            Removed = 1,  // component being removed (action state, not settable)
            Absent = 2,  // uninstalled (or action state absent but clients remain)
            Local = 3,  // installed on local drive
            Source = 4,  // run from source, CD or net
            Default = 5,  // use default, local or source
        }

        static String myMsiEnumComponents(
            int iComponentIndex,
            StringBuilder tmp
        ) {
            tmp.Length = 256;
            int r = MsiEnumComponents(iComponentIndex, tmp);
            if (r != 0) return null;
            return tmp.ToString();
        }

        static String myMsiLocateComponent(
            String szComponent,
            StringBuilder tmp,
            out INSTALLSTATE iss
        ) {
            int cch = tmp.Length = 256;
            iss = (INSTALLSTATE)MsiLocateComponent(szComponent, tmp, ref cch);
            return tmp.ToString();
        }

        static String myMsiGetProductCode(
            string szComponent,
            StringBuilder tmp
        ) {
            tmp.Length = 256;
            int r = MsiGetProductCode(szComponent, tmp);
            if (r != 0) return null;
            return tmp.ToString();
        }

        static String myMsiGetProductInfo(
            string szProduct,
            string szProperty,
            StringBuilder tmp
        ) {
            int cch = tmp.Length = 256;
            int r = MsiGetProductInfo(szProduct, szProperty, tmp, ref cch);
            if (r != 0) return null;
            return tmp.ToString();
        }

        private void Find(String filter) {
            StringBuilder tmp = new StringBuilder();
            for (int y = 0; ; y++) {
                String component = myMsiEnumComponents(y, tmp);
                if (component == null) break;

                INSTALLSTATE iss;
                String path = myMsiLocateComponent(component, tmp, out iss);
                bool show = filter == null || (path.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0);

                String product = myMsiGetProductCode(component, tmp);
                String ProductName = myMsiGetProductInfo(product, "ProductName", tmp);

                if (show) Console.WriteLine(y + "\t" + component + "\t" + iss + "\t" + path + "\t" + product + "\t" + ProductName);
            }
        }

        [DllImport("msi.dll")]
        static extern int MsiLocateComponent(
            string szComponent,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpPathBuf,
            ref int pcchBuf
            );

        [DllImport("msi.dll")]
        static extern int MsiEnumComponents(
            int iComponentIndex,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpComponentBuf
            );

        [DllImport("msi.dll")]
        static extern int MsiGetProductCode(
            string szComponent,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpProductBuf
            );

        [DllImport("msi.dll")]
        static extern int MsiGetProductInfo(
            string szProduct,
            string szProperty,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpValueBuf,
            ref int pcchValueBuf
            );

        [DllImport("msi.dll")]
        static extern int MsiEnumClients(
            string szComponent,
            int iProductIndex,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpProductBuf
            );

        [DllImport("msi.dll")]
        static extern int MsiGetComponentPath(
            string szProduct,
            string szComponent,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpPathBuf,
           ref int pcchBuf
            );
    }
}
