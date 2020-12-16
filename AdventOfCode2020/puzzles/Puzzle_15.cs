using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_15 : IPuzzle {
		public long Resolve( string input ) {
			string[] numberStrs = input.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

			long[] previous = new long[ Iterations ];
			for ( long i = 0; i < Iterations; ++i ) {
				previous[ i ] = -1;
			}

			long last = 0;

			for ( long i = 0; i < numberStrs.Length; ++i ) {
				if ( !long.TryParse( numberStrs[ i ], out last ) ) {
					return -1;
				}

				previous[ last ] = i;
			}

			for ( int i = numberStrs.Length - 1; i < Iterations - 1; ++i ) {
				if ( previous[ last ] >= 0 ) {
					long diff = i - previous[ last ];
					previous[ last ] = i;
					last = diff;
				} else {
					previous[ last ] = i;
					last = 0;
				}
			}

			return last;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to parse input number";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Iterations {
			get;
		}
	}

	class Puzzle_15_1 : Puzzle_15 {
		protected override long Iterations {
			get {
				return 2020;
			}
		}
	}

	class Puzzle_15_2 : Puzzle_15 {
		protected override long Iterations {
			get {
				return 30000000;
			}
		}
	}
}
