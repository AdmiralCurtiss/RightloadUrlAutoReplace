using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RightloadUrlAutoReplace {
	class Program {
		static void PrintUsage() {
			Console.WriteLine( "Usage:" );
			Console.WriteLine( "RightloadUrlAutoReplace [options] update.txt urls.txt [update_replaced.txt] [placeholder prefix] [placeholder postfix]" );
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine( "Available options:" );
			Console.WriteLine();
			Console.WriteLine( "-keepextension" );
			Console.WriteLine( "  Keep filename extensions in placeholders, ie. search for [image1.png] instead of just [image1]." );
			Console.WriteLine();
		}

		static void Main( string[] args ) {
			string UpdateInFilename = null;
			string UrlsInFilename = null;
			string UpdateOutFilename = null;
			string ReplacementPrefix = "[";
			string ReplacementPostfix = "]";
			bool KeepFilenameExtension = false;

			int parsedArgs = 0;
			for ( int i = 0; i < args.Length; ++i ) {
				// parse options
				switch ( args[i] ) {
					case "-keepextension": KeepFilenameExtension = true; break;

					default:
						// and remaining, non-option arguments
						switch ( parsedArgs ) {
							case 0: UpdateInFilename = Path.GetFullPath( args[i] ); break;
							case 1: UrlsInFilename = Path.GetFullPath( args[i] ); break;
							case 2: UpdateOutFilename = Path.GetFullPath( args[i] ); break;
							case 3: ReplacementPrefix = args[i]; break;
							case 4: ReplacementPostfix = args[i]; break;
						}
						++parsedArgs;
						break;
				}

			}

			if ( UpdateInFilename == null || UrlsInFilename == null ) {
				PrintUsage();
				return;
			}

			if ( UpdateOutFilename == null ) {
				UpdateOutFilename = Path.Combine( Path.GetDirectoryName( UpdateInFilename ), Path.GetFileNameWithoutExtension( UpdateInFilename ) + "_replaced" + Path.GetExtension( UpdateInFilename ) );
			}

			string Update;
			string[] Urls;
			try {
				Update = File.ReadAllText( UpdateInFilename );
			} catch ( Exception ) {
				Console.Write( "Failed opening update file!" );
				return;
			}
			try {
				Urls = File.ReadAllLines( UrlsInFilename );
			} catch ( Exception ) {
				Console.Write( "Failed opening URL file!" );
				return;
			}

			foreach ( string u in Urls ) {
				try {
					string s = u;

					if ( s.Contains( '|' ) ) {
						s = s.Substring( s.IndexOf( '|' ) + 1 );
					}

					if ( s.StartsWith( "[img]", StringComparison.InvariantCultureIgnoreCase ) )
						s = s.Substring( "[img]".Length );
					if ( s.EndsWith( "[/img]", StringComparison.InvariantCultureIgnoreCase ) )
						s = s.Substring( 0, s.Length - "[/img]".Length );
					string url = s;

					int imgnameStart = s.LastIndexOf( '/' );
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

					string imgname = s.Substring( imgnameStart, imgnameEnd - imgnameStart );
					if ( u.Contains( '|' ) ) {
						imgname = u.Substring( 0, u.IndexOf( '|' ) );
					}

					Update = Update.Replace( ReplacementPrefix + imgname + ReplacementPostfix, "[img]" + url + "[/img]" );
				} catch ( Exception ) {
					continue;
				}
			}

			try {
				File.WriteAllText( UpdateOutFilename, Update );
			} catch ( Exception ) {
				Console.Write( "Failed writing replaced update!" );
				return;
			}
		}
	}
}
