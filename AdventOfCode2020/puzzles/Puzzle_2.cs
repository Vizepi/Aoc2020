using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_2 : IPuzzle {
		static readonly char[] c_splitters = new char[] { '-', ' ', ':' };

		public long Resolve( string input ) {
			string[] elements = input.Split( new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );

			long valid = 0;
			foreach ( string element in elements ) {
				string[] split = element.Split( c_splitters, StringSplitOptions.RemoveEmptyEntries );
				if ( split.Length != 4 ) {
					return -1;
				}
				if ( !int.TryParse( split[ 0 ], out int a ) ) {
					return -2;
				}
				if ( !int.TryParse( split[ 1 ], out int b ) ) {
					return -3;
				}
				if ( split[ 2 ].Length != 1 ) {
					return -4;
				}
				char lookupChar = split[ 2 ][ 0 ];
				string pass = split[ 3 ];

				int policyRespect = PolicyRespected( a, b, lookupChar, pass );
				switch ( policyRespect ) {
				case 1:
					++valid;
					break;
				case 0:
					break;
				default:
					return policyRespect;
				}
			}

			return valid;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Bad line format";
			case -2:
				return "Failed to parse integer for first policy value";
			case -3:
				return "Failed to parse integer for second policy value";
			case -4:
				return "Invalid letter: Length is not 1";
			case -5:
				return "Invalid index for first policy value in password string";
			case -6:
				return "Invalid index for second policy value in password string";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract int PolicyRespected( int a, int b, char c, string pass );
	}

	class Puzzle_2_1 : Puzzle_2 {
		protected override int PolicyRespected( int a, int b, char c, string pass ) {
			int count = 0;
			foreach ( char passC in pass ) {
				if ( passC == c ) {
					++count;
				}
			}

			return ( count >= a && count <= b ) ? 1 : 0;
		}
	}

	class Puzzle_2_2 : Puzzle_2 {
		protected override int PolicyRespected( int a, int b, char c, string pass ) {
			if ( a > pass.Length || a < 1 ) {
				return -5;
			}
			if ( b > pass.Length || b < 1 ) {
				return -6;
			}

			int count = 0;
			if ( pass[ a - 1 ] == c ) {
				++count;
			}
			if ( pass[ b - 1 ] == c ) {
				++count;
			}

			return ( count == 1 ) ? 1 : 0;
		}
	}
}
