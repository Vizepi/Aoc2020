using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_1 : IPuzzle {
		protected abstract int Depth {
			get;
		}

		public long Resolve( string input ) {
			string[] elements = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );
			int[] iElements = new int[ elements.Length ];
			try {
				for ( int i = 0; i < elements.Length; ++i ) {
					iElements[ i ] = int.Parse( elements[ i ] );
				}
			} catch ( Exception ) {
				return -1;
			}

			return TrySolve( Depth, 0, 1, iElements, 0, iElements.Length - Depth - 1 );
		}

		long TrySolve( int depth, long sum, long mult, int[] elements, int startIndex, int endIndex ) {
			if ( depth == 0 ) {
				for ( int i = startIndex; i < endIndex; ++i ) {
					if ( sum + elements[ i ] == 2020 ) {
						return mult * elements[ i ];
					}
				}
			} else {
				for ( int i = startIndex; i < endIndex; ++i ) {
					long result = TrySolve( depth - 1, sum + elements[ i ], mult * elements[ i ], elements, startIndex + 1, endIndex + 1 );
					if ( result >= 0 ) {
						return result;
					}
				}
			}
			return -2;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to convert input line to integer";
			case -2:
				return "Result not found";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}
	}
	class Puzzle_1_1 : Puzzle_1 {
		protected override int Depth => 1;
	}
	class Puzzle_1_2 : Puzzle_1 {
		protected override int Depth => 2;
	}
}
