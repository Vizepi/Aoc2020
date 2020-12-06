using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_5 : IPuzzle {
		public abstract long Resolve( string input );

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid line length. Expected 10 characters";
			case -2:
				return "Invalid row character. Expected 'B' or 'F'";
			case -3:
				return "Invalid column character. Expected 'L' or 'R'";
			case -4:
				return "Seat not found";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected long GetSeatInfos( string line, out long row, out long column ) {
			row = 0;
			column = 0;
			if ( line.Length != 10 ) {
				return -1;
			}

			row = MapBinary( line.Substring( 0, 7 ), 'B', 'F', -2 );
			if ( row < 0 ) {
				return row;
			}
			column = MapBinary( line.Substring( 7 ), 'R', 'L', -3 );
			if ( column < 0 ) {
				return column;
			}
			return row * 8 + column;
		}

		long MapBinary( string value, char one, char zero, long error ) {
			long result = 0;
			foreach ( char c in value ) {
				result *= 2;
				if ( c == one ) {
					++result;
				} else if ( c != zero ) {
					return error;
				}
			}
			return result;
		}
	}

	class Puzzle_5_1 : Puzzle_5 {
		public override long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			long maxId = 0;

			foreach ( string line in lines ) {
				long id = GetSeatInfos( line, out long row, out long column );
				if ( id < 0 ) {
					return id;
				}
				if ( id > maxId ) {
					maxId = id;
				}
			}

			return maxId;
		}
	}

	class Puzzle_5_2 : Puzzle_5 {
		public override long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			int maxPossibleValue = 0b1111111 * 8 + 0b111;
			bool[] seats = new bool[ maxPossibleValue ];
			long minAssigned = maxPossibleValue;

			foreach ( string line in lines ) {
				long id = GetSeatInfos( line, out long row, out long column );
				if ( id < 0 ) {
					return id;
				}
				if ( id < minAssigned ) {
					minAssigned = id;
				}
				seats[ id ] = true;
			}

			for ( long i = minAssigned + 1; i < maxPossibleValue; ++i ) {
				if ( !seats[ i ] ) {
					return i;
				}
			}

			return -4;
		}
	}
}
