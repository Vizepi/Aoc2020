using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace AdventOfCode2020 {
	class Program {
		public static void Main( string[] args ) {
			try {
				if ( !ParseArguments( args, out int day, out int num, out string test ) ) {
					Help();
					return;
				}

				string puzzleName = "AdventOfCode2020.puzzles.Puzzle_" + day + "_" + num;
				IPuzzle puzzle = Activator.CreateInstance( Type.GetType( puzzleName ) ) as IPuzzle;
				if ( puzzle == null ) {
					Console.WriteLine( "Impossible to instantiate puzzle '" + puzzleName + "'." );
					return;
				}

				string puzzleInputPath;

				if ( !string.IsNullOrWhiteSpace( test ) ) {
					Console.WriteLine( "Testing puzzle " + num + " of day " + day + " with test " + test + "." );
					puzzleInputPath = "tests/test_" + day + '_' + test;
				} else {
					Console.WriteLine( "Solving puzzle " + num + " of day " + day + "." );
					puzzleInputPath = "inputs/input_" + day;
				}
				if ( !ReadInput( puzzleInputPath, out string puzzleInput ) ) {
					Console.WriteLine( "Failed to open file '" + puzzleInputPath + ".txt'." );
					return;
				}


				try {
					if ( !string.IsNullOrWhiteSpace( test ) ) {
						LaunchTest( puzzle, num, puzzleInput );
					} else {
						LaunchResolve( puzzle, puzzleInput );
					}
				} catch ( Exception e ) {
					Console.WriteLine( "Resolve execution failed:\n" + e );
				}
			} finally {
				Console.WriteLine( "Press a key to continue..." );
				Console.ReadLine();
			}
		}

		static void LaunchTest( IPuzzle puzzle, int num, string puzzleInput ) {
			puzzleInput = puzzleInput.Trim();
			int test2ResultPosition = puzzleInput.LastIndexOf( '\n' );
			if ( test2ResultPosition < 0 ) {
				Console.WriteLine( "Invalid test format: missing Puzzle's second test result." );
				return;
			}
			int test1ResultPosition = puzzleInput.LastIndexOf( '\n', test2ResultPosition - 1 );
			if ( test2ResultPosition < 0 ) {
				Console.WriteLine( "Invalid test format: missing Puzzle's first test result." );
				return;
			}

			if ( !long.TryParse( puzzleInput.Substring( test1ResultPosition + 1, test2ResultPosition - test1ResultPosition - 1 ), out long test1Result ) ) {
				Console.WriteLine( "Invalid test format: failed to parse Puzzle's first test result." );
				return;
			} else if ( num == 1 && test1Result < 0 ) {
				Console.WriteLine( "No test result is provided for puzzle number 1." );
				return;
			}

			if ( !long.TryParse( puzzleInput.Substring( test2ResultPosition + 1 ), out long test2Result ) ) {
				Console.WriteLine( "Invalid test format: failed to parse Puzzle's second test result." );
				return;
			} else if ( num == 2 && test2Result < 0 ) {
				Console.WriteLine( "No test result is provided for puzzle number 2." );
				return;
			}

			long expected = num == 1 ? test1Result : test2Result;

			puzzleInput = puzzleInput.Substring( 0, test1ResultPosition );

			long result = puzzle.Resolve( puzzleInput );
			if ( result >= 0 ) {
				Console.WriteLine( "Result: " + result );
				Console.WriteLine( "Expect: " + expected );
				Console.WriteLine( result == expected ? "Test succeeded." : "Test failed." );
			} else {
				Console.WriteLine( "Error: " + puzzle.ErrorCodeToString( result ) );
			}
		}

		static void LaunchResolve( IPuzzle puzzle, string puzzleInput ) {
			long result = puzzle.Resolve( puzzleInput );
			if ( result >= 0 ) {
				Console.WriteLine( "Result: " + result );
			} else {
				Console.WriteLine( "Error: " + puzzle.ErrorCodeToString( result ) );
			}
		}

		static bool ParseArguments( string[] args, out int day, out int num, out string test ) {
			day = 0;
			num = 0;
			test = "";
			if ( args.Length == 1 && args[ 0 ].ToLower() == "help" ) {
				return false;
			} else if ( args.Length != 2 && args.Length != 3 ) {
				Console.WriteLine( "Invalid number of arguments." );
				return false;
			}
			if ( !int.TryParse( args[ 0 ], out day ) ) {
				Console.WriteLine( "Invalid day parameter." );
				return false;
			}
			if ( day < 1 || day > 25 ) {
				Console.WriteLine( "Invalid day value." );
				return false;
			}
			if ( !int.TryParse( args[ 1 ], out num ) ) {
				Console.WriteLine( "Invalid puzzle parameter." );
				return false;
			}
			if ( num < 1 || num > 2 ) {
				Console.WriteLine( "Invalid puzzle value." );
				return false;
			}
			if ( args.Length == 3 ) {
				test = args[ 2 ];
				if ( test.Length == 0 ) {
					Console.WriteLine( "Invalid test parameter." );
					return false;
				} else {
					for ( int i = 0; i < test.Length; ++i ) {
						char c = test[ i ];
						if ( ( c < '0' || c > '9' ) && ( c < 'a' || c > 'z' ) && ( c < 'A' || c > 'Z' ) && ( c != '_' ) ) {
							Console.WriteLine( "Invalid test parameter format." );
							return false;
						}
					}
				}
			}
			return true;
		}

		static bool ReadInput( string path, out string content ) {
			content = "";
			try {
				content = File.ReadAllText( path + ".txt" );
				return !string.IsNullOrEmpty( content );
			} catch ( Exception ) {
				return false;
			}
		}

		static void Help() {
			Console.WriteLine(
				"Usage:\n" +
				"AdventOfCode2020 <day> <puzzle> [test]\n" +
				"  day     The day of the puzzle in range [1;25]\n" +
				"  puzzle  The number of the puzzle, 1 or 2\n" +
				"  test    The test to execute instead of the puzzle's input\n" );
		}
	}
}
