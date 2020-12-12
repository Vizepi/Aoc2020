using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_12 : IPuzzle {
		protected const double c_degToRad = Math.PI / 180.0;

		protected enum Action {
			North,
			South,
			East,
			West,
			Left,
			Right,
			Forward
		}

		protected struct Instruction {
			public Action action;
			public long   value;
		}

		protected class Point {
			public long angle = 0;
			public long x = 0;
			public long y = 0;
		}

		public long Resolve( string input ) {
			string[] split = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			Instruction[] instructions = new Instruction[ split.Length ];

			for ( int i = 0; i < split.Length; ++i ) {
				string line = split[ i ];
				if ( line.Length < 2 ) {
					return -1;
				}

				Instruction inst = new Instruction();

				switch ( line[ 0 ] ) {
				case 'N':
					inst.action = Action.North;
					break;
				case 'S':
					inst.action = Action.South;
					break;
				case 'E':
					inst.action = Action.East;
					break;
				case 'W':
					inst.action = Action.West;
					break;
				case 'L':
					inst.action = Action.Left;
					break;
				case 'R':
					inst.action = Action.Right;
					break;
				case 'F':
					inst.action = Action.Forward;
					break;
				default:
					return -2;
				}

				if ( !long.TryParse( line.Substring( 1 ), out inst.value ) ) {
					return -3;
				}

				instructions[ i ] = inst;
			}

			Point ship = new Point();
			Point waypoint = new Point() {
				x = 10,
				y = 1
			};

			MoveShip( ship, waypoint, instructions );

			return Math.Abs( ship.x ) + Math.Abs( ship.y );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid line length. Expected at least 2 characters for action and value";
			case -2:
				return "Invalid action. Expected one of 'N', 'S', 'E', 'W', 'L', 'R' or 'F'";
			case -3:
				return "Failed to parse instruction numeric value";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long MoveShip( Point ship, Point waypoint, Instruction[] instructions );
	}

	class Puzzle_12_1 : Puzzle_12 {
		protected override long MoveShip( Point ship, Point waypoint, Instruction[] instructions ) {
			foreach ( Instruction inst in instructions ) {
				switch ( inst.action ) {
				case Action.North:
					ship.y += inst.value;
					break;
				case Action.South:
					ship.y -= inst.value;
					break;
				case Action.East:
					ship.x += inst.value;
					break;
				case Action.West:
					ship.x -= inst.value;
					break;
				case Action.Left:
					ship.angle = ( ship.angle + inst.value ) % 360;
					break;
				case Action.Right:
					ship.angle = ( ship.angle - inst.value ) % 360;
					break;
				case Action.Forward:
					ship.x += ( long )( Math.Cos( c_degToRad * ( double )ship.angle ) * inst.value );
					ship.y += ( long )( Math.Sin( c_degToRad * ( double )ship.angle ) * inst.value );
					break;
				}
			}

			return 0;
		}
	}

	class Puzzle_12_2 : Puzzle_12 {
		protected override long MoveShip( Point ship, Point waypoint, Instruction[] instructions ) {
			double degToRad = Math.PI / 180.0;

			foreach ( Instruction inst in instructions ) {
				switch ( inst.action ) {
				case Action.North:
					waypoint.y += inst.value;
					break;
				case Action.South:
					waypoint.y -= inst.value;
					break;
				case Action.East:
					waypoint.x += inst.value;
					break;
				case Action.West:
					waypoint.x -= inst.value;
					break;
				case Action.Left:
					Rotate( waypoint, inst.value );
					break;
				case Action.Right:
					Rotate( waypoint, -inst.value );
					break;
				case Action.Forward:
					ship.x += waypoint.x * inst.value;
					ship.y += waypoint.y * inst.value;
					break;
				}
			}

			return 0;
		}

		void Rotate( Point p, long rotation ) {
			double length = Math.Sqrt( p.x * p.x + p.y * p.y );
			double angle = Math.Atan2( p.y, p.x );
			angle += rotation * c_degToRad;
			p.x = ( long )Math.Round( Math.Cos( angle ) * length );
			p.y = ( long )Math.Round( Math.Sin( angle ) * length );
		}
	}
}
