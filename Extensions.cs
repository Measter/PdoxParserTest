using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdoxParserTest
{
	static class Extensions
	{
		public static Color ParseColour( this IList<string> stringList )
		{
			if( stringList.Count < 3 )
				return Color.White;

			int r, g, b;

			bool isFloat = stringList.Any( s => s.Contains( "." ) );

			if( isFloat )
			{
				double rF = Double.Parse( stringList[0] );
				double gF = Double.Parse( stringList[1] );
				double bF = Double.Parse( stringList[2] );

				r = (int)Math.Round( rF * 255 );
				g = (int)Math.Round( gF * 255 );
				b = (int)Math.Round( bF * 255 );
			} else
			{
				r = Int32.Parse( stringList[0] );
				g = Int32.Parse( stringList[1] );
				b = Int32.Parse( stringList[2] );
			}

			r = r.Clamp( 0, 255 );
			g = g.Clamp( 0, 255 );
			b = b.Clamp( 0, 255 );

			return Color.FromArgb( r, g, b );
		}

		public static int Clamp( this int n, int min, int max )
		{
			if( n > max )
				return max;
			if( n < min )
				return min;
			return n;
		}
	}
}
