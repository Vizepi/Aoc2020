using System;
using System.Collections.Generic;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_14 : IPuzzle {
		protected struct Instruction {
			public long maskX;
			public long mask0;
			public long mask1;
			public long address;
			public long value;
		}

		public long Resolve( string input ) {
			string[] split = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			Instruction[] instructions = new Instruction[ split.Length ];
			int instructionCount = 0;

			long currentMaskX = 0;
			long currentMask0 = 0;
			long currentMask1 = 0;
			foreach ( string line in split ) {
				if ( line.StartsWith( "mask" ) ) {
					int equalsPosition = line.IndexOf( '=' );
					if ( equalsPosition < 0 ) {
						return -1;
					}

					string mask = line.Substring( equalsPosition + 1 ).Trim();
					if ( mask.Length != 36 ) {
						return -2;
					}

					currentMaskX = 0;
					currentMask0 = 0;
					currentMask1 = 0;
					for ( int i = 0, j = 35; i < 36; ++i, --j ) {
						switch ( mask[ i ] ) {
						case 'X':
							currentMaskX |= ( long )1 << j;
							continue;
						case '0':
							currentMask0 |= ( long )1 << j;
							continue;
						case '1':
							currentMask1 |= ( long )1 << j;
							continue;
						default:
							return -3;
						}
					}
				} else if ( line.StartsWith( "mem[" ) ) {
					int equalsPosition = line.IndexOf( '=' );
					if ( equalsPosition < 0 ) {
						return -4;
					}

					int closePosition = line.IndexOf( ']' );
					if ( closePosition < 0 ) {
						return -5;
					}

					string addressStr = line.Substring( 4, closePosition - 4 ).Trim();
					string valueStr = line.Substring( equalsPosition + 1 ).Trim();

					if ( !long.TryParse( addressStr, out long address ) ) {
						return -6;
					}
					if ( !long.TryParse( valueStr, out long value ) ) {
						return -7;
					}

					Instruction inst = new Instruction();
					inst.maskX = currentMaskX;
					inst.mask0 = currentMask0;
					inst.mask1 = currentMask1;
					inst.address = address;
					inst.value = value;
					instructions[ instructionCount++ ] = inst;
				} else {
					return -8;
				}
			}

			Array.Resize( ref instructions, instructionCount );

			Dictionary< long, long > memory = new Dictionary<long, long>();

			RunProgram( instructions, memory );

			return SumMemory( memory );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to find '=' operator in mask affectation";
			case -2:
				return "Invalid mask length. Expected 36 characters";
			case -3:
				return "Invalid character in mask. Expected 'X', '0' or '1'";
			case -4:
				return "Failed to find '=' operator in memory affectation";
			case -5:
				return "Failed to find closing bracket of instructions address";
			case -6:
				return "Failed to parse mem instruction's address";
			case -7:
				return "Failed to parse mem instruction's value";
			case -8:
				return "Failed to parse line.Expected 'mask' or 'mem'";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		void RunProgram( Instruction[] instructions, Dictionary<long, long> memory ) {
			foreach ( Instruction inst in instructions ) {
				SetMemoryValue( memory, inst );
			}
		}

		long SumMemory( Dictionary<long, long> memory ) {
			long sum = 0;
			foreach ( long value in memory.Values ) {
				sum += value;
			}
			return sum;
		}

		protected abstract void SetMemoryValue( Dictionary<long, long> memory, Instruction instruction );
	}

	class Puzzle_14_1 : Puzzle_14 {

		protected override void SetMemoryValue( Dictionary<long, long> memory, Instruction instruction ) {
			memory[ instruction.address ] = ( instruction.value & instruction.maskX ) | instruction.mask1;
		}
	}

	class Puzzle_14_2 : Puzzle_14 {

		protected override void SetMemoryValue( Dictionary<long, long> memory, Instruction instruction ) {
			long baseAddress = ( instruction.address | instruction.mask1 ) & ~instruction.maskX;
			long[] bits = new long[ 36 ];
			int bitCount = 0;
			int iterations = 0;
			for ( int i = 0; i < 36; ++i ) {
				if ( ( instruction.maskX & ( ( long )1 << i ) ) != 0 ) {
					iterations |= 1 << bitCount;
					bits[ bitCount++ ] = ( long )1 << i;
				}
			}
			for ( int i = 0; i <= iterations; ++i ) {
				long address = baseAddress;
				for ( int j = 0; j < bitCount; ++j ) {
					if ( ( i & ( ( long )1 << j ) ) != 0 ) {
						address |= bits[ j ];
					}
				}
				memory[ address ] = instruction.value;
			}
		}
	}
}
