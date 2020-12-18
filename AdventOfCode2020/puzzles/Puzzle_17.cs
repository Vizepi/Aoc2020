using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_17 : IPuzzle {
		protected const int Cycles = 6;

		public abstract long Resolve( string input );

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid empty puzzle input";
			case -2:
				return "Inconsistent puzzle input line length";
			case -3:
				return "Unknown input characcter, expected '#' or '.'";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}
	}

	class Puzzle_17_1 : Puzzle_17 {

		public override long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			if ( lines.Length == 0 ) {
				return -1;
			}

			int x0 = Cycles + 1;
			int y0 = Cycles + 1;
			int z0 = Cycles + 1;
			int sx = lines[ 0 ].Length;
			int sy = lines.Length;
			int sz = 1;

			bool[,,] cubes = new bool[ sx + x0 * 2, sy + y0 * 2, sz + z0 * 2 ];
			bool[,,] cubes2 = new bool[ sx + x0 * 2, sy + y0 * 2, sz + z0 * 2 ];

			for ( int iLine = 0; iLine < lines.Length; ++iLine ) {
				string line = lines[ iLine ];
				if ( line.Length != sx ) {
					return -2;
				}
				for ( int iChar = 0; iChar < line.Length; ++iChar ) {
					switch ( line[ iChar ] ) {
					case '.':
						break;
					case '#':
						cubes[ x0 + iChar, y0 + iLine, z0 ] = true;
						break;
					default:
						return -3;
					}
				}
			}

			long count = 0;
			for ( int i = 0; i < Cycles; ++i ) {
				--x0;
				--y0;
				--z0;
				sx += 2;
				sy += 2;
				sz += 2;
				count = Simulate( cubes, cubes2, x0, y0, z0, sx, sy, sz );
				if ( count < 0 ) {
					return count;
				}
				bool[,,] tmp = cubes;
				cubes = cubes2;
				cubes2 = tmp;
			}

			return count;
		}

		long Simulate( bool[,,] cubes, bool[,,] cubes2, int x0, int y0, int z0, int sx, int sy, int sz ) {
			long count = 0;
			for ( int x = x0, x1 = x0 + sx; x < x1; ++x ) {
				for ( int y = y0, y1 = y0 + sy; y < y1; ++y ) {
					for ( int z = z0, z1 = z0 + sz; z < z1; ++z ) {
						long neighbors = CountNeighbors( cubes, x, y, z );
						if ( neighbors < 0 ) {
							return neighbors;
						}
						if ( cubes[ x, y, z ] ) {
							cubes2[ x, y, z ] = neighbors == 2 || neighbors == 3;
						} else {
							cubes2[ x, y, z ] = neighbors == 3;
						}
						if ( cubes2[ x, y, z ] ) {
							++count;
						}
					}
				}
			}
			return count;
		}

		long CountNeighbors( bool[,,] cubes, int x, int y, int z ) {
			long count = cubes[ x, y, z ] ? -1 : 0;

			for ( int xx = x - 1; xx <= x + 1; ++xx ) {
				for ( int yy = y - 1; yy <= y + 1; ++yy ) {
					for ( int zz = z - 1; zz <= z + 1; ++zz ) {
						if ( cubes[ xx, yy, zz ] ) {
							++count;
						}
					}
				}
			}

			return count;
		}
	}

	class Puzzle_17_2 : Puzzle_17 {

		public override long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			if ( lines.Length == 0 ) {
				return -1;
			}

			int x0 = Cycles + 1;
			int y0 = Cycles + 1;
			int z0 = Cycles + 1;
			int w0 = Cycles + 1;
			int sx = lines[ 0 ].Length;
			int sy = lines.Length;
			int sz = 1;
			int sw = 1;

			bool[,,,] cubes = new bool[ sx + x0 * 2, sy + y0 * 2, sz + z0 * 2, sw + w0 * 2 ];
			bool[,,,] cubes2 = new bool[ sx + x0 * 2, sy + y0 * 2, sz + z0 * 2, sw + w0 * 2 ];

			for ( int iLine = 0; iLine < lines.Length; ++iLine ) {
				string line = lines[ iLine ];
				if ( line.Length != sx ) {
					return -2;
				}
				for ( int iChar = 0; iChar < line.Length; ++iChar ) {
					switch ( line[ iChar ] ) {
					case '.':
						break;
					case '#':
						cubes[ x0 + iChar, y0 + iLine, z0, w0 ] = true;
						break;
					default:
						return -3;
					}
				}
			}

			long count = 0;
			for ( int i = 0; i < Cycles; ++i ) {
				--x0;
				--y0;
				--z0;
				--w0;
				sx += 2;
				sy += 2;
				sz += 2;
				sw += 2;
				count = Simulate( cubes, cubes2, x0, y0, z0, w0, sx, sy, sz, sw );
				if ( count < 0 ) {
					return count;
				}
				bool[,,,] tmp = cubes;
				cubes = cubes2;
				cubes2 = tmp;
			}

			return count;
		}

		long Simulate( bool[,,,] cubes, bool[,,,] cubes2, int x0, int y0, int z0, int w0, int sx, int sy, int sz, int sw ) {
			long count = 0;
			for ( int x = x0, x1 = x0 + sx; x < x1; ++x ) {
				for ( int y = y0, y1 = y0 + sy; y < y1; ++y ) {
					for ( int z = z0, z1 = z0 + sz; z < z1; ++z ) {
						for ( int w = w0, w1 = w0 + sw; w < w1; ++w ) {
							long neighbors = CountNeighbors( cubes, x, y, z, w );
							if ( neighbors < 0 ) {
								return neighbors;
							}
							if ( cubes[ x, y, z, w ] ) {
								cubes2[ x, y, z, w ] = neighbors == 2 || neighbors == 3;
							} else {
								cubes2[ x, y, z, w ] = neighbors == 3;
							}
							if ( cubes2[ x, y, z, w ] ) {
								++count;
							}
						}
					}
				}
			}
			return count;
		}

		long CountNeighbors( bool[,,,] cubes, int x, int y, int z, int w ) {
			long count = cubes[ x, y, z, w ] ? -1 : 0;

			for ( int xx = x - 1; xx <= x + 1; ++xx ) {
				for ( int yy = y - 1; yy <= y + 1; ++yy ) {
					for ( int zz = z - 1; zz <= z + 1; ++zz ) {
						for ( int ww = w - 1; ww <= w + 1; ++ww ) {
							if ( cubes[ xx, yy, zz, ww ] ) {
								++count;
							}
						}
					}
				}
			}

			return count;
		}
	}
}
