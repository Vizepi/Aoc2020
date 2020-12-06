using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_6 : IPuzzle {
		public long Resolve( string input ) {
			string[] split = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None );

			bool[] groupAnswer = null;
			long sum = 0;

			foreach ( string line in split ) {
				if ( string.IsNullOrWhiteSpace( line ) ) {
					if ( groupAnswer != null ) {
						long localSum = SumUpAnswers( groupAnswer );
						if ( localSum < 0 ) {
							return localSum;
						}
						sum += localSum;
						groupAnswer = null;
					}
				} else {
					if ( groupAnswer == null ) {
						groupAnswer = CreateGroupAnswers();
					}
					bool[] individualAnswer = new bool[ 26 ];
					foreach ( char c in line ) {
						if ( c < 'a' || c > 'z' ) {
							return -3;
						}
						individualAnswer[ c - 'a' ] = true;
					}
					AddAnswers( groupAnswer, individualAnswer );
				}
			}

			if ( groupAnswer != null ) {
				long localSum = SumUpAnswers( groupAnswer );
				if ( localSum < 0 ) {
					return localSum;
				}
				sum += localSum;
			}

			return sum;
		}

		public string ErrorCodeToString( long code ) {
			return "";
		}

		protected long SumUpAnswers( bool[] answers ) {
			if ( answers == null ) {
				return -1;
			}
			if ( answers.Length != 26 ) {
				return -2;
			}

			long sum = 0;

			foreach ( bool b in answers ) {
				if ( b ) {
					++sum;
				}
			}

			return sum;
		}

		protected abstract bool[] CreateGroupAnswers();
		protected abstract void AddAnswers( bool[] groupAnswers, bool[] individualAnswers );
	}

	class Puzzle_6_1 : Puzzle_6 {
		protected override bool[] CreateGroupAnswers() {
			return new bool[ 26 ];
		}
		protected override void AddAnswers( bool[] groupAnswers, bool[] individualAnswers ) {
			for ( int i = 0; i < groupAnswers.Length; ++i ) {
				groupAnswers[ i ] |= individualAnswers[ i ];
			}
		}
	}

	class Puzzle_6_2 : Puzzle_6 {
		protected override bool[] CreateGroupAnswers() {
			bool[] array = new bool[ 26 ];
			for ( int i = 0; i < array.Length; ++i ) {
				array[ i ] = true;
			}
			return array;
		}
		protected override void AddAnswers( bool[] groupAnswers, bool[] individualAnswers ) {
			for ( int i = 0; i < groupAnswers.Length; ++i ) {
				groupAnswers[ i ] &= individualAnswers[ i ];
			}
		}
	}
}
