using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_3 : IPuzzle {
		protected struct Point2 {
			public Point2( int x = 0, int y = 0 ) {
				this.x = x;
				this.y = y;
			}

			public int x;
			public int y;
		}

		public abstract long Resolve( string input );

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected long CollisionsForMove( string[] lines, Point2 move ) {
			Point2 position = new Point2( 0, 0 );

			long treeCount = 0;
			while ( position.y < lines.Length ) {
				if ( lines[ position.y ][ position.x ] == '#' ) {
					++treeCount;
				}
				position = NextPosition( position, move, lines[ 0 ].Length );
			}

			return treeCount;
		}

		Point2 NextPosition( Point2 position, Point2 move, int length ) {
			return new Point2( ( position.x + move.x ) % length, position.y + move.y );
		}
	}

	class Puzzle_3_1 : Puzzle_3 {

		public override long Resolve( string input ) {
			string[] lines = input.Split( new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );
			return CollisionsForMove( lines, new Point2( 3, 1 ) );
		}
	}

	class Puzzle_3_2 : Puzzle_3 {

		public override long Resolve( string input ) {
			string[] lines = input.Split( new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );
			long mult = 1;
			mult *= CollisionsForMove( lines, new Point2( 1, 1 ) );
			mult *= CollisionsForMove( lines, new Point2( 3, 1 ) );
			mult *= CollisionsForMove( lines, new Point2( 5, 1 ) );
			mult *= CollisionsForMove( lines, new Point2( 7, 1 ) );
			mult *= CollisionsForMove( lines, new Point2( 1, 2 ) );
			return mult;
		}
	}
}
