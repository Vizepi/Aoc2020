using System;

namespace AdventOfCode2020.puzzles {
	abstract class Puzzle_4 : IPuzzle {
		protected class Passport {
			public Passport() {
				byr = null;
				iyr = null;
				eyr = null;
				hgt = null;
				hcl = null;
				ecl = null;
				pid = null;
				cid = null;
			}

			public long AddField( string field ) {
				string[] split = field.Split( ':' );
				if ( split.Length != 2 ) {
					return -1;
				}

				switch ( split[ 0 ] ) {
				case "byr":
					byr = split[ 1 ];
					return 0;
				case "iyr":
					iyr = split[ 1 ];
					return 0;
				case "eyr":
					eyr = split[ 1 ];
					return 0;
				case "hgt":
					hgt = split[ 1 ];
					return 0;
				case "hcl":
					hcl = split[ 1 ];
					return 0;
				case "ecl":
					ecl = split[ 1 ];
					return 0;
				case "pid":
					pid = split[ 1 ];
					return 0;
				case "cid":
					cid = split[ 1 ];
					return 0;
				default:
					return -2;
				}
			}

			public string byr, iyr, eyr, hgt, hcl, ecl, pid, cid;
		}

		public long Resolve( string input ) {
			string[] fields = input.Split( new string[] { "\r\n", "\r", "\n", " " }, StringSplitOptions.None );
			Passport passport = null;

			long valids = 0;
			foreach ( string field in fields ) {
				if ( string.IsNullOrWhiteSpace( field ) ) {
					if ( passport != null ) {
						long isValid = Validate( passport );
						if ( isValid < 0 ) {
							return isValid;
						} else if ( isValid != 0 ) {
							++valids;
						}
						passport = null;
					}
				} else {
					if ( passport == null ) {
						passport = new Passport();
					}
					long res = passport.AddField( field );
					if ( res < 0 ) {
						return res;
					}
				}
			}
			if ( passport != null ) {
				long isValid = Validate( passport );
				if ( isValid < 0 ) {
					return isValid;
				} else if ( isValid != 0 ) {
					++valids;
				}
			}

			return valids;
		}

		public string ErrorCodeToString( long code ) {
			switch ( code ) {
			case -1:
				return "Invalid field format, expected \"name:value\"";
			case -2:
				return "Invalid format name";
			case -3:
				return "Unexpected null Passport value";
			default:
				if ( code < 0 ) {
					return "Unknown error";
				}
				return "No error";
			}
		}

		protected abstract long Validate( Passport passport );
	}

	class Puzzle_4_1 : Puzzle_4 {
		protected override long Validate( Passport passport ) {
			if ( passport == null ) {
				return -3;
			}
			if ( passport.byr != null &&
				passport.iyr != null &&
				passport.eyr != null &&
				passport.hgt != null &&
				passport.hcl != null &&
				passport.ecl != null &&
				passport.pid != null ) {
				return 1;
			}
			return 0;
		}
	}

	class Puzzle_4_2 : Puzzle_4 {
		protected override long Validate( Passport passport ) {
			if ( passport == null ) {
				return -3;
			}

			// byr
			{
				if ( passport.byr == null ) {
					return 0;
				}
				if ( !int.TryParse( passport.byr, out int byr ) ) {
					return 0;
				}
				if ( byr < 1920 || byr > 2002 ) {
					return 0;
				}
			}

			// iyr
			{
				if ( passport.iyr == null ) {
					return 0;
				}
				if ( !int.TryParse( passport.iyr, out int iyr ) ) {
					return 0;
				}
				if ( iyr < 2010 || iyr > 2020 ) {
					return 0;
				}
			}

			// eyr
			{
				if ( passport.eyr == null ) {
					return 0;
				}
				if ( !int.TryParse( passport.eyr, out int eyr ) ) {
					return 0;
				}
				if ( eyr < 2020 || eyr > 2030 ) {
					return 0;
				}
			}

			// hgt
			{
				if ( passport.hgt == null ) {
					return 0;
				}
				if ( passport.hgt.EndsWith( "cm" ) ) {
					if ( passport.hgt.Length != 5 ) {
						return 0;
					}
					if ( !int.TryParse( passport.hgt.Substring( 0, 3 ), out int hgt ) ) {
						return 0;
					}
					if ( hgt < 150 || hgt > 193 ) {
						return 0;
					}
				} else if ( passport.hgt.EndsWith( "in" ) ) {
					if ( passport.hgt.Length != 4 ) {
						return 0;
					}
					if ( !int.TryParse( passport.hgt.Substring( 0, 2 ), out int hgt ) ) {
						return 0;
					}
					if ( hgt < 59 || hgt > 76 ) {
						return 0;
					}
				} else {
					return 0;
				}
			}

			// hcl
			{
				if ( passport.hcl == null ) {
					return 0;
				}
				if ( passport.hcl.Length != 7 ) {
					return 0;
				}
				if ( passport.hcl[ 0 ] != '#' ) {
					return 0;
				}
				for ( int i = 1; i < 7; ++i ) {
					char c = passport.hcl[ i ];
					if ( ( c < '0' || c > '9' ) && ( c < 'a' && c > 'f' ) ) {

						return 0;
					}
				}
			}

			// ecl
			{
				if ( passport.ecl == null ) {
					return 0;
				}
				if ( passport.ecl != "amb" &&
					passport.ecl != "blu" &&
					passport.ecl != "brn" &&
					passport.ecl != "gry" &&
					passport.ecl != "grn" &&
					passport.ecl != "hzl" &&
					passport.ecl != "oth" ) {
					return 0;
				}
			}

			// pid
			{
				if ( passport.pid == null ) {
					return 0;
				}
				if ( passport.pid.Length != 9 ) {
					return 0;
				}
				foreach ( char c in passport.pid ) {
					if ( c < '0' || c > '9' ) {
						return 0;
					}
				}
			}

			return 1;
		}
	}
}
