using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_16 : IPuzzle {
		protected class Intervals {
			public string name;
			public long[] values = new long[ 4 ];
			public bool IsValid( long value ) {
				return ( value >= values[ 0 ] && value <= values[ 1 ] ) || ( value >= values[ 2 ] && value <= values[ 3 ] );
			}
		}

		protected class Rules {
			public Intervals[] intervals = null;
			public bool Matches( long[] ticket, out ulong invalidBitMask ) {
				invalidBitMask = 0;
				ulong invalidBit = 1;
				foreach ( long value in ticket ) {
					bool valid = false;
					foreach ( Intervals interval in intervals ) {
						if ( interval.IsValid( value ) ) {
							valid = true;
							break;
						}
					}
					if ( !valid ) {
						invalidBitMask |= invalidBit;
					}
					invalidBit <<= 1;
				}
				return invalidBitMask == 0;
			}
		}

		public long Resolve( string input ) {
			string[] groups = input.Split( new string[] {
				"\r\n\r\n",
				"\n\n",
				"\r\n\n",
				"\n\r\n",
				"your ticket:\r\n",
				"your ticket:\n",
				"nearby tickets:\r\n",
				"nearby tickets:\n" },
				StringSplitOptions.RemoveEmptyEntries );

			if ( groups.Length != 3 ) {
				return -1;
			}

			Rules rules = new Rules();
			{
				string[] ruleLines = groups[ 0 ].Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );
				rules.intervals = new Intervals[ ruleLines.Length ];
				for ( int i = 0; i < ruleLines.Length; ++i ) {
					rules.intervals[ i ] = new Intervals();
					string[] rulesStr = ruleLines[ i ].Split( new string[] { ":", " ", "or", "-" }, StringSplitOptions.RemoveEmptyEntries );
					if ( rulesStr.Length < 5 ) {
						return -2;
					}
					rules.intervals[ i ].name = rulesStr[ 0 ];
					for ( int j = 1; j < rulesStr.Length - 4; ++j ) {
						rules.intervals[ i ].name += ' ' + rulesStr[ j ];
					}
					for ( int j = 0; j < 4; ++j ) {
						if ( !long.TryParse( rulesStr[ rulesStr.Length - 4 + j ], out rules.intervals[ i ].values[ j ] ) ) {
							return -3;
						}
					}
				}
			}

			long[] myTicket = null;
			{
				string[] myTicketStrs = groups[ 1 ].Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
				myTicket = new long[ myTicketStrs.Length ];
				for ( int i = 0; i < myTicketStrs.Length; ++i ) {
					if ( !long.TryParse( myTicketStrs[ i ], out myTicket[ i ] ) ) {
						return -4;
					}
				}
			}

			long[][] theirTickets = null;
			{
				string[] theirTicketsLines = groups[ 2 ].Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );
				theirTickets = new long[ theirTicketsLines.Length ][];
				for ( int i = 0; i < theirTicketsLines.Length; ++i ) {
					theirTickets[ i ] = new long[ myTicket.Length ];
					string[] theirTicketStrs = theirTicketsLines[ i ].Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
					if ( myTicket.Length != theirTicketStrs.Length ) {
						return -5;
					}
					for ( int j = 0; j < theirTicketStrs.Length; ++j ) {
						if ( !long.TryParse( theirTicketStrs[ j ], out theirTickets[ i ][ j ] ) ) {
							return -6;
						}
					}
				}
			}

			return Compute( rules, myTicket, theirTickets );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Failed to split input in 3 groups";
			case -2:
				return "Failed to parse rule line: Expected 5 fields";
			case -3:
				return "Failed to parse rule line: Invalid integer value";
			case -4:
				return "Failed to parse my ticket's value";
			case -5:
				return "Mismatch length between my ticket and their ticket";
			case -6:
				return "Failed to parse their ticket's value";
			case -7:
				return "Invalid puzzle: Multiple solutions exists";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Compute( Rules rules, long[] myTicket, long[][] theirTickets );
	}

	class Puzzle_16_1 : Puzzle_16 {
		protected override long Compute( Rules rules, long[] myTicket, long[][] theirTickets ) {
			long sum = 0;

			foreach ( long[] ticket in theirTickets ) {
				if ( !rules.Matches( ticket, out ulong invalids ) ) {
					for ( int i = 0; i < ticket.Length; ++i ) {
						if ( ( invalids & ( ( ulong )1 << i ) ) != 0 ) {
							sum += ticket[ i ];
						}
					}
				}
			}

			return sum;
		}
	}

	class Puzzle_16_2 : Puzzle_16 {
		protected override long Compute( Rules rules, long[] myTicket, long[][] theirTickets ) {
			long[][] validTickets = new long[ theirTickets.Length ][];
			int validCount = 0;

			foreach ( long[] ticket in theirTickets ) {
				if ( rules.Matches( ticket, out ulong invalids ) ) {
					validTickets[ validCount++ ] = ticket;
				}
			}

			ulong[] possibleBits = new ulong[ rules.intervals.Length ];
			ulong mask = ( ( ulong )1 << rules.intervals.Length ) - 1;

			for ( int iInterval = 0; iInterval < rules.intervals.Length; ++iInterval ) {
				possibleBits[ iInterval ] = mask;
				for ( int iTicket = 0; iTicket < validCount; ++iTicket ) {
					for ( int iValue = 0; iValue < validTickets[ iTicket ].Length; ++iValue ) {
						if ( !rules.intervals[ iInterval ].IsValid( validTickets[ iTicket ][ iValue ] ) ) {
							possibleBits[ iInterval ] &= ~( ( ulong )1 << iValue );
						}
					}
				}
			}

			int totalBitCount = int.MaxValue;
			for (; ; ) {
				int previousTotalBitCount = totalBitCount;
				totalBitCount = 0;

				for ( int i = 0; i < possibleBits.Length; ++i ) {
					int bitCount = 0;
					for ( ulong bit = 1; bit <= mask; bit <<= 1 ) {
						if ( ( possibleBits[ i ] & bit ) != 0 ) {
							++bitCount;
						}
					}
					if ( bitCount == 1 ) {
						for ( int j = 0; j < possibleBits.Length; ++j ) {
							if ( i == j ) {
								continue;
							}
							possibleBits[ j ] &= ~possibleBits[ i ];
						}
					}
					totalBitCount += bitCount;
				}

				if ( previousTotalBitCount == totalBitCount ) {
					break;
				}
			}

			if ( totalBitCount > possibleBits.Length ) {
				return 7;
			}

			long mult = 1;
			for ( int i = 0; i < rules.intervals.Length; ++i ) {
				if ( rules.intervals[ i ].name.StartsWith( "departure" ) ) {
					int index = 0;
					for ( ; ( ( ulong )1 << index ) != possibleBits[ i ]; ++index ) {
					}
					mult *= myTicket[ index ];
				}
			}

			return mult;
		}
	}
}
