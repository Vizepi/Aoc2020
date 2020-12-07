using System;
using System.Collections.Generic;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_7 : IPuzzle {
		protected class Link {
			public string container;
			public string contained;
			public int multiplicity;
		}

		protected class LinkCollection {
			public bool visited;
			public string first;
			public Dictionary<string, Link> links = new Dictionary<string, Link>();
		}

		public long Resolve( string input ) {
			string[] lines = input.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries );

			Dictionary<string, LinkCollection> rules = new Dictionary<string, LinkCollection>();
			Dictionary<string, LinkCollection> invertedRules = new Dictionary<string, LinkCollection>();

			foreach ( string line in lines ) {
				string[] split = line.Split( new string[] { "contain", "bags", "bag", ",", "." }, StringSplitOptions.RemoveEmptyEntries );

				string container = null;

				foreach ( string linePart in split ) {
					string trimedPart = linePart.Trim();
					if ( string.IsNullOrWhiteSpace( trimedPart ) ) {
						continue;
					}

					if ( container == null ) {
						container = trimedPart;
					} else if ( trimedPart != "no other" ) {
						if ( trimedPart[ 0 ] < '0' || trimedPart[ 0 ] > '9' ) {
							return -1;
						}

						// Should at least return as we ensured trimedPart[ 0 ] is in ['0';'9']
						int spacePosition = trimedPart.IndexOf( ' ' );
						if ( spacePosition < 0 ) {
							return -2;
						}

						Link link = new Link() { container = container };

						if ( !int.TryParse( trimedPart.Substring( 0, spacePosition ), out link.multiplicity ) ) {
							return -3;
						}

						link.contained = trimedPart.Substring( spacePosition ).Trim();

						if ( !rules.ContainsKey( container ) ) {
							LinkCollection collection = new LinkCollection();
							collection.first = container;
							rules.Add( container, collection );
						}
						if ( !invertedRules.ContainsKey( link.contained ) ) {
							LinkCollection collection = new LinkCollection();
							collection.first = link.contained;
							invertedRules.Add( link.contained, collection );
						}

						rules[ container ].links.Add( link.contained, link );
						invertedRules[ link.contained ].links.Add( container, link );
					}
				}

				if ( container == null ) {
					return -4;
				}
			}

			return Compute( rules, invertedRules, "shiny gold" );
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Missing mutliplicty in contained bag";
			case -2:
				return "Failed to parse contained bag: Missing space between multiplicty and bag type";
			case -3:
				return "Failed to parse bag multiplicty: not a number";
			case -4:
				return "Missing container in line";
			case -5:
				return "Container not found in container dictionary";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Compute( Dictionary<string, LinkCollection> rules, Dictionary<string, LinkCollection> invertedRules, string entry );
	}

	class Puzzle_7_1 : Puzzle_7 {
		protected override long Compute( Dictionary<string, LinkCollection> rules, Dictionary<string, LinkCollection> invertedRules, string entry ) {
			List<LinkCollection> fifo = new List<LinkCollection>();
			if ( !invertedRules.TryGetValue( entry, out LinkCollection collection ) ) {
				return 0;
			}
			long count = 0;
			fifo.Add( collection );
			while ( fifo.Count > 0 ) {
				collection = fifo[ 0 ];
				foreach ( Link link in collection.links.Values ) {
					if ( !rules.TryGetValue( link.container, out LinkCollection nonInvertedCollection ) ) {
						return -5;
					}
					if ( !nonInvertedCollection.visited ) {
						nonInvertedCollection.visited = true;
						++count;
						if ( !invertedRules.TryGetValue( link.container, out collection ) ) {
							continue;
						}
						fifo.Add( collection );
					}
				}
				fifo.RemoveAt( 0 );
			}

			return count;
		}
	}

	class Puzzle_7_2 : Puzzle_7 {
		protected override long Compute( Dictionary<string, LinkCollection> rules, Dictionary<string, LinkCollection> invertedRules, string entry ) {
			List<LinkCollection> fifo = new List<LinkCollection>();
			if ( !rules.TryGetValue( entry, out LinkCollection collection ) ) {
				return 0;
			}
			long count = 0;
			fifo.Add( collection );
			while ( fifo.Count > 0 ) {
				collection = fifo[ 0 ];
				foreach ( Link link in collection.links.Values ) {
					count += link.multiplicity;
					if ( rules.TryGetValue( link.contained, out LinkCollection nextCollection ) ) {
						for ( int i = 0; i < link.multiplicity; ++i ) {
							fifo.Add( nextCollection );
						}
					}
				}
				fifo.RemoveAt( 0 );
			}

			return count;
		}
	}
}
