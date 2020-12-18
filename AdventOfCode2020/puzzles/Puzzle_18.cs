using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_18 : IPuzzle {
		public long Resolve( string input ) {
			string[] expressions = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			long sum = 0;

			foreach ( string exp in expressions ) {
				long result = Calculate( exp );
				if ( result < 0 ) {
					return result;
				}
				sum += result;
			}

			return sum;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Reached '+' token but an operation is already pending";
			case -2:
				return "Reached '*' token but an operation is already pending";
			case -3:
				return "Failed to parse token as long";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		long Calculate( string expression ) {
			string[] tokens = expression.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
			int current = 0;
			return CalculateRecurse( tokens, ref current, false );
		}

		protected abstract long CalculateRecurse( string[] tokens, ref int current, bool closeMult );
	}

	class Puzzle_18_1 : Puzzle_18 {
		protected override long CalculateRecurse( string[] tokens, ref int current, bool closeMult ) {
			long value = 0;
			long operand = 0;
			bool pendingAdd = false;
			bool pendingMult = false;
			for ( ; current < tokens.Length; ++current ) {
				switch ( tokens[ current ] ) {
				case "(":
					++current;
					operand = CalculateRecurse( tokens, ref current, false );
					if ( operand < 0 ) {
						return operand;
					}
					if ( pendingAdd ) {
						pendingAdd = false;
						value += operand;
					} else if ( pendingMult ) {
						pendingMult = false;
						value *= operand;
					} else {
						value = operand;
					}
					break;
				case ")":
					return value;
				case "+":
					if ( pendingAdd || pendingMult ) {
						return -1;
					}
					pendingAdd = true;
					break;
				case "*":
					if ( pendingAdd || pendingMult ) {
						return -2;
					}
					pendingMult = true;
					break;
				default:
					if ( !long.TryParse( tokens[ current ], out operand ) ) {
						return -3;
					}
					if ( pendingAdd ) {
						pendingAdd = false;
						value += operand;
					} else if ( pendingMult ) {
						pendingMult = false;
						value *= operand;
					} else {
						value = operand;
					}
					break;
				}
			}

			return value;
		}
	}

	class Puzzle_18_2 : Puzzle_18 {
		protected override long CalculateRecurse( string[] tokens, ref int current, bool closeMult ) {
			long value = 0;
			long operand = 0;
			bool pendingAdd = false;
			for ( ; current < tokens.Length; ++current ) {
				switch ( tokens[ current ] ) {
				case "(":
					++current;
					operand = CalculateRecurse( tokens, ref current, false );
					if ( operand < 0 ) {
						return operand;
					}
					if ( pendingAdd ) {
						pendingAdd = false;
						value += operand;
					} else {
						value = operand;
					}
					break;
				case ")":
					return value;
				case "+":
					if ( pendingAdd ) {
						return -1;
					}
					pendingAdd = true;
					break;
				case "*":
					if ( closeMult ) {
						return value;
					}
					++current;
					operand = CalculateRecurse( tokens, ref current, true );
					value *= operand;
					if ( current >= tokens.Length || tokens[ current ] == ")" ) {
						return value;
					}
					--current;
					break;
				default:
					if ( !long.TryParse( tokens[ current ], out operand ) ) {
						return -3;
					}
					if ( pendingAdd ) {
						pendingAdd = false;
						value += operand;
					} else {
						value = operand;
					}
					break;
				}
			}

			return value;
		}
	}
}


