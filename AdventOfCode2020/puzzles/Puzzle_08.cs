using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_8 : IPuzzle {
		protected enum Instruction {
			nop,
			jmp,
			acc
		}
		protected class Program {
			public Instruction[] instructions;
			public long[]        arguments;
		}
		protected class VirtualMachine {
			public Program program = new Program();
			public long[]  visit;
			public long    accumulator;
			public long    programCounter;
			public long    programSize;

			public void Reset() {
				accumulator = 0;
				programCounter = 0;
				for ( long i = 0; i < programSize; ++i ) {
					visit[ i ] = 0;
				}
			}
		}
		public long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			VirtualMachine vm = new VirtualMachine();
			vm.programSize = lines.Length;
			vm.program.instructions = new Instruction[ vm.programSize ];
			vm.program.arguments = new long[ vm.programSize ];
			vm.visit = new long[ vm.programSize ];

			int i = -1;
			foreach ( string line in lines ) {
				++i;
				string[] instruction = line.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
				if ( instruction.Length != 2 ) {
					return -1;
				}
				if ( !Enum.TryParse( instruction[ 0 ], out vm.program.instructions[ i ] ) ) {
					return -2;
				}
				if ( !long.TryParse( instruction[ 1 ], out vm.program.arguments[ i ] ) ) {
					return -3;
				}
			}

			long execResult = Execute( vm );
			if ( execResult < 0 ) {
				return execResult;
			}

			return vm.accumulator;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to parse instruction line";
			case -2:
				return "Failed to parse instruction type";
			case -3:
				return "Failed to parse instruction argument";
			case -4:
				return "Invalid praogrma counter value";
			case -5:
				return "Invalid instruction type";
			case -6:
				return "Expected an infinite loop";
			case -7:
				return "No instruction swap fixes the program";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected long Step( VirtualMachine vm ) {
			if ( vm.programCounter < 0 || vm.programCounter >= vm.visit.Length ) {
				return -4;
			}

			Instruction i = vm.program.instructions[ vm.programCounter ];
			long arg = vm.program.arguments[ vm.programCounter ];

			switch ( i ) {
			case Instruction.nop:
				++vm.programCounter;
				break;
			case Instruction.jmp:
				vm.programCounter += arg;
				break;
			case Instruction.acc:
				vm.accumulator += arg;
				++vm.programCounter;
				break;
			default:
				return -5;
			}

			return 0;
		}

		protected abstract long Execute( VirtualMachine vm );
	}

	class Puzzle_8_1 : Puzzle_8 {
		protected override long Execute( VirtualMachine vm ) {
			while ( vm.programCounter >= 0 && vm.programCounter < vm.programSize ) {
				if ( vm.visit[ vm.programCounter ] != 0 ) {
					return vm.accumulator;
				}
				++vm.visit[ vm.programCounter ];
				Step( vm );
			}
			return -6;
		}
	}

	class Puzzle_8_2 : Puzzle_8 {
		protected override long Execute( VirtualMachine vm ) {
			int swappedInstructionIndex = SeekNextSawppableInstruction( vm, -1 );

			while ( swappedInstructionIndex < vm.programSize ) {
				SwapInstruction( vm, swappedInstructionIndex );
				vm.Reset();

				while ( vm.programCounter >= 0 && vm.programCounter < vm.programSize ) {
					if ( vm.visit[ vm.programCounter ] != 0 ) {
						break;
					}
					++vm.visit[ vm.programCounter ];
					Step( vm );
				}
				if ( vm.programCounter >= vm.programSize ) {
					return vm.accumulator;
				}

				SwapInstruction( vm, swappedInstructionIndex );
				swappedInstructionIndex = SeekNextSawppableInstruction( vm, swappedInstructionIndex );
			}
			return -7;
		}

		int SeekNextSawppableInstruction( VirtualMachine vm, int index ) {
			for ( int i = index + 1; i < vm.visit.Length; ++i ) {
				Instruction inst = vm.program.instructions[ i ];
				if ( inst == Instruction.jmp || inst == Instruction.nop ) {
					return i;
				}
			}
			return vm.visit.Length;
		}

		void SwapInstruction( VirtualMachine vm, int index ) {
			if ( vm.program.instructions[ index ] == Instruction.jmp ) {
				vm.program.instructions[ index ] = Instruction.nop;
			} else if ( vm.program.instructions[ index ] == Instruction.nop ) {
				vm.program.instructions[ index ] = Instruction.jmp;
			}
		}
	}
}
