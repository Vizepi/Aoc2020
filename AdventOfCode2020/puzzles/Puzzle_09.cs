using System;

namespace AdventOfCode2020.puzzles {
	class Puzzle_9 : IPuzzle {
		public virtual long Resolve( string input ) {
			string[] numberStrs = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			if ( numberStrs.Length <= 1 ) {
				return -1;
			}

			if ( !int.TryParse( numberStrs[ 0 ], out int preambleSize ) ) {
				return -2;
			}

			if ( numberStrs.Length <= preambleSize ) {
				return -3;
			}

			int ringIndex = 0;
			long[] ring = new long[ preambleSize ];

			for ( int i = 0; i < preambleSize; ++i ) {
				if ( !long.TryParse( numberStrs[ i + 1 ], out ring[ i ] ) ) {
					return -4;
				}
			}

			for ( int i = preambleSize + 1; i < numberStrs.Length; ++i ) {
				if ( !long.TryParse( numberStrs[ i ], out long number ) ) {
					return -5;
				}
				if ( !IsASumOf2( ring, number, preambleSize ) ) {
					return number;
				}
				ring[ ringIndex ] = number;
				ringIndex = ( ringIndex + 1 ) % preambleSize;
			}

			return -6;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid number of inputs, at least preamble size expected";
			case -2:
				return "Failed to parse preamble size";
			case -3:
				return "Preamble size bigger than input size";
			case -4:
				return "Failed to parse preamble number";
			case -5:
				return "Failed to parse number";
			case -6:
				return "Invalid number not found";
			case -7:
				return "Not enough numbers to add. Expected at least 2";
			case -8:
				return "No consecutive numbers sum up to the invalid number";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		static bool IsASumOf2( long[] ring, long value, int preambleSize ) {
			for ( int i = 0; i < preambleSize - 1; ++i ) {
				for ( int j = i + 1; j < preambleSize; ++j ) {
					if ( ring[ i ] + ring[ j ] == value ) {
						return true;
					}
				}
			}
			return false;
		}
	}

	class Puzzle_9_1 : Puzzle_9 {
	}

	class Puzzle_9_2 : Puzzle_9 {
		public override long Resolve( string input ) {
			long invalidSum = base.Resolve( input );
			if ( invalidSum < 0 ) {
				return invalidSum;
			}

			string[] numberStrs = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );
			long[] numbers = new long[ numberStrs.Length ]; // Keep an empty place at the end to avoid out of range indexing

			for ( int i = 0; i < numbers.Length - 1; ++i ) {
				numbers[ i ] = long.Parse( numberStrs[ i + 1 ] );
			}

			if ( numbers.Length < 2 ) {
				return -7;
			}

			int firstIndex = 0;
			int lastIndex = 1;

			long sum = numbers[ firstIndex ] + numbers[ lastIndex ];

			while ( lastIndex < numbers.Length - 1 ) {
				if ( sum == invalidSum ) {
					long min = long.MaxValue;
					long max = long.MinValue;
					for ( int i = firstIndex; i <= lastIndex; ++i ) {
						if ( numbers[ i ] > max ) {
							max = numbers[ i ];
						}
						if ( numbers[ i ] < min ) {
							min = numbers[ i ];
						}
					}
					return min + max;
				} else if ( sum < invalidSum ) {
					sum += numbers[ ++lastIndex ];
				} else if ( sum > invalidSum ) {
					if ( firstIndex == lastIndex - 1 ) {
						sum = numbers[ ++firstIndex ] + numbers[ ++lastIndex ];
					} else {
						sum -= numbers[ firstIndex++ ];
					}
				}
			}

			return -8;
		}
	}
}
