using System;
using System.Collections;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_13 : IPuzzle {
		public long Resolve( string input ) {
			string[] split = input.Split( new string[]{"\r\n", "\r", "\n" } ,StringSplitOptions.RemoveEmptyEntries );

			if ( split.Length != 2 ) {
				return -1;
			}

			if ( !long.TryParse( split[ 0 ], out long minTime ) ) {
				return -2;
			}

			string[] buses = split[ 1 ].Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

			return Compute( minTime, buses );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Input should be 2 lines.";
			case -2:
				return "Failed to parse minimum depart timestamp";
			case -3:
				return "Failed to parse bus ID";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Compute( long minTime, string[] buses );
	}

	class Puzzle_13_1 : Puzzle_13 {
		protected override long Compute( long minTime, string[] buses ) {
			long bestBus = 1;
			long maxWait = long.MaxValue;

			foreach ( string bus in buses ) {
				if ( bus[ 0 ] == 'x' ) {
					continue;
				}

				if ( !long.TryParse( bus, out long id ) ) {
					return -3;
				}

				long wait = id - ( minTime % id );

				if ( wait < maxWait ) {
					maxWait = wait;
					bestBus = id;
				}
			}

			return bestBus * maxWait;
		}
	}

	class Puzzle_13_2 : Puzzle_13 {
		struct Equation : IComparer {
			public long remainder;
			public long modulo;

			public int Compare( object x, object y ) {
				return ( int )( ( ( Equation )y ).modulo - ( ( Equation )x ).modulo );
			}
		}

		protected override long Compute( long minTime, string[] buses ) {
			Equation[] busIDs = new Equation[ buses.Length ];
			int busCount = 0;

			for ( long i = 0; i < buses.Length; ++i ) {
				if ( buses[ i ][ 0 ] == 'x' ) {
					continue;
				}
				busIDs[ busCount ] = new Equation();
				busIDs[ busCount ].remainder = i;
				busIDs[ busCount ].modulo = long.Parse( buses[ i ] );
				busIDs[ busCount ].remainder = ( busIDs[ busCount ].modulo - ( busIDs[ busCount ].remainder % busIDs[ busCount ].modulo ) ) % busIDs[ busCount ].modulo;
				++busCount;
			}

			Array.Resize( ref busIDs, busCount );

			if( busIDs.Length == 0 ) {
				return -4;
			}

			Array.Sort( busIDs, new Equation() );

			Equation result = busIDs[ 0 ];

			for ( int i = 1; i < busIDs.Length; ++i ) {
				while ( result.remainder % busIDs[ i ].modulo != busIDs[ i ].remainder ) {
					result.remainder += result.modulo;
				}
				result.modulo *= busIDs[ i ].modulo;
			}

			return result.remainder;
		}
	}
}
