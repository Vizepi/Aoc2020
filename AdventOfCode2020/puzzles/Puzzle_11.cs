using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_11 : IPuzzle {
		protected enum Seat {
			None,
			Free,
			Occupied
		}

		public long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			Seat[,] seats = new Seat[ lines.Length + 2, lines[ 0 ].Length + 2 ];
			Seat[,] seats2 = new Seat[ lines.Length + 2, lines[ 0 ].Length + 2 ];

			// Parse input
			for ( int i = 1; i <= lines.Length; ++i ) {
				string line = lines[ i - 1 ];
				if ( line.Length != seats.GetLength( 1 ) - 2 ) {
					return -1;
				}
				for ( int j = 1; j <= line.Length; ++j ) {
					switch ( line[ j - 1 ] ) {
					case '.':
						seats[ i, j ] = Seat.None;
						break;
					case 'L':
						seats[ i, j ] = Seat.Free;
						break;
					case '#':
						seats[ i, j ] = Seat.Occupied;
						break;
					default:
						return -2;
					}
				}
			}

			long changes = 1;
			while ( changes != 0 ) {
				changes = Simulate( seats, seats2 );
				if ( changes < 0 ) {
					return changes;
				}
				Seat[,] tmp = seats2;
				seats2 = seats;
				seats = tmp;
			}

			return Count( seats );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid line length. All lines should have the same length";
			case -2:
				return "Invalid input: Expected characters '.', 'L' or '#'";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		long Simulate( Seat[,] seats, Seat[,] seats2 ) {
			int l0 = seats.GetLength( 0 ) - 1;
			int l1 = seats.GetLength( 1 ) - 1;
			long changes = 0;
			for ( int i = 1; i < l0; ++i ) {
				for ( int j = 1; j < l1; ++j ) {
					if ( seats[ i, j ] == Seat.None ) {
						continue;
					} else if ( seats[ i, j ] == Seat.Free ) {
						if ( CountAdjacentSeats( i, j, seats ) == 0 ) {
							++changes;
							seats2[ i, j ] = Seat.Occupied;
						} else {
							seats2[ i, j ] = Seat.Free;
						}
					} else {
						if ( CountAdjacentSeats( i, j, seats ) >= GetMaxOccupiedSeatsAround ) {
							++changes;
							seats2[ i, j ] = Seat.Free;
						} else {
							seats2[ i, j ] = Seat.Occupied;
						}
					}
				}
			}

			return changes;
		}

		long Count( Seat[,] seats ) {
			int l0 = seats.GetLength( 0 ) - 1;
			int l1 = seats.GetLength( 1 ) - 1;
			long count = 0;
			for ( int i = 1; i < l0; ++i ) {
				for ( int j = 1; j < l1; ++j ) {
					if ( seats[ i, j ] == Seat.Occupied ) {
						++count;
					}
				}
			}

			return count;
		}

		protected abstract int CountAdjacentSeats( int x, int y, Seat[,] seats );
		protected abstract int GetMaxOccupiedSeatsAround { get; }
	}

	class Puzzle_11_1 : Puzzle_11 {
		protected override int CountAdjacentSeats( int x, int y, Seat[,] seats ) {
			int neighbors = 0;
			if ( seats[ x - 1, y - 1 ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x - 1, y     ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x - 1, y + 1 ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x    , y - 1 ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x    , y + 1 ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x + 1, y - 1 ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x + 1, y     ] == Seat.Occupied ) { ++neighbors; }
			if ( seats[ x + 1, y + 1 ] == Seat.Occupied ) { ++neighbors; }
			return neighbors;
		}

		protected override int GetMaxOccupiedSeatsAround {
			get {

				return 4;
			}
		}
	}

	class Puzzle_11_2 : Puzzle_11 {
		int[] m_xDir = new int[] {  1,  1,  0, -1, -1, -1,  0,  1 };
		int[] m_yDir = new int[] {  0,  1,  1,  1,  0, -1, -1, -1 };

		protected override int CountAdjacentSeats( int x, int y, Seat[,] seats ) {
			int l0 = seats.GetLength( 0 ) - 1;
			int l1 = seats.GetLength( 1 ) - 1;

			int neighbors = 0;
			for ( int i = 0; i < m_xDir.Length; ++i ) {
				int dx = m_xDir[ i ], dy = m_yDir[ i ];
				int px = x + dx, py = y + dy;

				while ( px > 0 && px < l0 && py > 0 && py < l1 ) {
					if ( seats[ px, py ] == Seat.Occupied ) {
						++neighbors;
						break;
					} else if ( seats[ px, py ] == Seat.Free ) {
						break;
					}
					px += dx;
					py += dy;
				}
			}
			return neighbors;
		}

		protected override int GetMaxOccupiedSeatsAround {
			get {
				return 5;
			}
		}
	}
}
