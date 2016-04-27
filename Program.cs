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
                //Console.Error.WriteLine("MsiEnumComponents {63E949F6-03BC-5C40-A01F-C8B3B9A1E18E}");
                Environment.ExitCode = 1;
                return;
            }
            //new Program().Run2(args[0]);
            //new Program().Run3(args[0]);
        }

        private void Productof(String comp) {
            StringBuilder buff = new StringBuilder(2048);
            int r = MsiGetProductCode(comp, buff);
            if (r != 0) return;
            String product = Cutter.Null(buff.ToString());
            buff.Length = buff.Capacity;
            int cb = buff.Length;
            r = MsiGetProductInfo(product, "ProductName", buff, ref cb);
            if (r != 0) return;
            String nam = Cutter.Null(buff.ToString());
            Console.WriteLine(product + "\t" + nam);
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

        class Cutter {
            internal static string Null(string s) {
                int p = s.IndexOf((char)0);
                if (p < 0) return s;
                return s.Substring(0, p);
            }

            internal static string Null(StringBuilder b) {
                int x = 0, cx = b.Length;
                while (x < cx && b[x] != 0) x++;
                return b.ToString(0, x);
            }
        }

        private void Run3(string p) {
            StringBuilder buff = new StringBuilder();
            buff.Length = 256;
            for (int x = 0; ; x++) {
                int r = MsiEnumClients(p, x, buff);
                if (r != 0) break;

                String g = buff.ToString();
                int cb = 256;
                r = MsiGetProductInfo(g, "ProductName", buff, ref cb);
                String name = buff.ToString();

                cb = 256;
                int state = r = MsiGetComponentPath(g, p, buff, ref cb);
                String path = buff.ToString();

                Console.WriteLine(g + "\t" + state + "\t" + name + "\t" + path);
                //Console.WriteLine();
            }
        }

        private void Run2(String g) {
            StringBuilder buff = new StringBuilder();
            buff.Length = 256;
            int cb = 256;
            int r = MsiLocateComponent(g, buff, ref cb);
            Console.WriteLine(r);
            Console.WriteLine(buff);

            r = MsiGetProductCode(g, buff);
            Console.WriteLine(r);
            Console.WriteLine(buff);

            String product = buff.ToString();
            cb = 256;
            r = MsiGetProductInfo(product, "ProductName", buff, ref cb);
            Console.WriteLine(buff);

            // http://support.microsoft.com/kb/884468/ja

            // http://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/18a64389-fb80-4b54-82ac-0aa776075719/the-msilocatecomponent-function-behaves-differently-on-windows-8-compared-to-previous-versions?forum=windowscompatibility
            //             INSTALLSTATE_LOCAL = 3,  // installed on local drive

            // http://support.microsoft.com/kb/2643995/en-us
        }

        private void Run() {
            StringBuilder buff = new StringBuilder();
            buff.Length = 256;
            int r = 0;
            Guid g = new Guid("63E949F6-03BC-5C40-A01F-C8B3B9A1E18E");
            for (int x = 0; ; x++) {
                r = MsiEnumComponents(x, buff);
                if (r != 0) break;
                Console.WriteLine(x + " " + buff);
                Guid g2 = new Guid(buff.ToString());
                if (g == g2) {
                    break;
                }
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
