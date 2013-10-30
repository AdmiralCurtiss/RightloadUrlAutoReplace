using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RightloadUrlAutoReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 6)
            {
                Console.WriteLine(
					"Usage: RightloadUrlAutoReplace"
					+ " update.txt"
					+ " urls.txt"
					+ " [update_replaced.txt]"
					+ " [replace name prefix]"
					+ " [replace name postfix]"
					+ " [-keepextension]"
				);
                return;
            }

            string UpdateInFilename;
            string UrlsInFilename;
            string UpdateOutFilename;
            string ReplacementPrefix;
            string ReplacementPostfix;
			bool KeepFilenameExtension;

			try {
				UpdateInFilename = Path.GetFullPath( args[0] );
				UrlsInFilename = Path.GetFullPath( args[1] );
				UpdateOutFilename = args.Length >= 3 ? Path.GetFullPath( args[2] ) :
					Path.GetDirectoryName( UpdateInFilename ) + System.IO.Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension( UpdateInFilename ) + "_replaced" + Path.GetExtension( UpdateInFilename );
				ReplacementPrefix = args.Length >= 4 ? args[3] : "[";
				ReplacementPostfix = args.Length >= 5 ? args[4] : "]";
				KeepFilenameExtension = args.Length >= 6 ? args[5] == "-keepextension" : false;
			} catch ( Exception ex ) {
				Console.WriteLine( "Failed parsing arguments." );
				Console.WriteLine( "Exception: " + ex.Message );
				return;
			}

            string Update;
            string[] Urls;
            try {
                Update = File.ReadAllText(UpdateInFilename);
            } catch (Exception) {
                Console.Write("Failed opening update!");
                return;
            }
            try {
                Urls = File.ReadAllLines(UrlsInFilename);
            } catch (Exception) {
                Console.Write("Failed opening URLs!");
                return;
            }

            foreach ( string u in Urls ) {
                try {
                    string s = u;

					if ( s.Contains( '|' ) ) {
						s = s.Substring( s.IndexOf( '|' ) + 1 );
					}

					if ( s.StartsWith( "[img]", StringComparison.InvariantCultureIgnoreCase ) )
                        s = s.Substring("[img]".Length);
                    if ( s.EndsWith("[/img]", StringComparison.InvariantCultureIgnoreCase) )
                        s = s.Substring(0, s.Length - "[/img]".Length);
                    string url = s;

                    int imgnameStart = s.LastIndexOf('/');
                    if ( imgnameStart == -1 ) continue;
                    imgnameStart += 1;

                    int imgnameEnd;
					if ( !KeepFilenameExtension ) {
						imgnameEnd = s.LastIndexOf( '.' );
						if ( imgnameStart == -1 || imgnameEnd < imgnameStart )
							imgnameEnd = s.Length;
					} else {
						imgnameEnd = s.Length;
					}

                    string imgname = s.Substring(imgnameStart, imgnameEnd - imgnameStart);
					if ( u.Contains( '|' ) ) {
						imgname = u.Substring( 0, u.IndexOf( '|' ) );
					}

                    Update = Update.Replace(ReplacementPrefix + imgname + ReplacementPostfix, "[img]" + url + "[/img]");
                } catch (Exception) {
                    continue;
                }
            }

            try {
                File.WriteAllText(UpdateOutFilename, Update);
            } catch (Exception) {
                Console.Write("Failed writing replaced update!");
                return;
            }
        }
    }
}
