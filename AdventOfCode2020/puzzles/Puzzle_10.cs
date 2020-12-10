using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_10 : IPuzzle {
		public long Resolve( string input ) {
			string[] adaptersStr = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			int[] adapters = new int[ adaptersStr.Length + 2 ];
			adapters[ 0 ] = 0;
			adapters[ adapters.Length - 1 ] = int.MaxValue;

			for ( int i = 0; i < adaptersStr.Length; ++i ) {
				if ( !int.TryParse( adaptersStr[ i ], out adapters[ i + 1 ] ) ) {
					return -1;
				}
			}

			Array.Sort( adapters );
			adapters[ adapters.Length - 1 ] = adapters[ adapters.Length - 2 ] + 3;

			return Compute( adapters );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to parse input adapter joltage";
			case -2:
				return "Unexpected two adapters with same joltage";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Compute( int[] adapters );
	}

	class Puzzle_10_1 : Puzzle_10 {
		protected override long Compute( int[] adapters ) {
			long diff1 = 0;
			long diff3 = 0; // Builtin is always 3 above highest adapter

			for ( int i = 1; i < adapters.Length; ++i ) {
				int diff = adapters[ i ] - adapters[ i - 1 ];
				if ( diff == 1 ) {
					++diff1;
				} else if ( diff == 3 ) {
					++diff3;
				} else if ( diff == 0 ) {
					return -2;
				}
			}

			return diff1 * diff3;
		}
	}

	class Puzzle_10_2 : Puzzle_10 {
		protected override long Compute( int[] adapters ) {
			long[] waysToReachEnd = new long[ adapters.Length ];
			waysToReachEnd[ adapters.Length - 1 ] = 1;
			for ( int i = adapters.Length - 1; i >= 0; --i ) {
				for ( int j = i + 1; j < i + 4; ++j ) {
					if ( j >= adapters.Length || adapters[ j ] > adapters[ i ] + 3 ) {
						break;
					}
					waysToReachEnd[ i ] += waysToReachEnd[ j ];
				}
			}
			return waysToReachEnd[ 0 ];
		}
	}
}
