namespace AdventOfCode2020 {
	interface IPuzzle {
		long   Resolve( string input );
		string ErrorCodeToString( long code );
	}
}
