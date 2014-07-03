using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Internal.Xml.XPath;
using Pdoxcl2Sharp;

namespace PdoxParserTest
{
	class Program
	{
		static void Main( string[] args )
		{
			GenericParser gp = new GenericParser();
			gp.Run();
		}
	}

	internal class GenericParser
	{
		public List<Option> Options = new List<Option>();

		private Stack<Option> m_lastGroup = new Stack<Option>();

		public void Run()
		{
			const string filename = @"title.txt";

			Stopwatch sw;
			using( FileStream fs = new FileStream( filename, FileMode.Open ) )
			{
				sw = Stopwatch.StartNew();
				ParadoxParser.Parse( fs, ParseOption );
				sw.Stop();
			}
		}

		private void ParseOption( ParadoxParser parser, string tag )
		{
			Option opt;

			if( parser.NextIsBracketed() )
			{
				if( tag == "color" || tag == "color2" )
				{
					opt = new ColourOption( tag, parser.ReadStringList().ParseColour() );
				} else if( tag == "male_names" || tag == "female_names" )
				{
					opt = new StringListOption( tag, parser.ReadStringList() );
				} else if( tag == "data" )
				{
					opt = new IntListOption( tag, parser.ReadIntList() );
				} else
				{
					opt = new GroupOption( tag );
					m_lastGroup.Push( opt );
					parser.Parse( ParseOption );
					m_lastGroup.Pop();
				}
			} else
			{
				opt = Option.Parse( tag, parser.ReadString() );
			}

			if( m_lastGroup.Count == 0 )
			{
				Options.Add( opt );
			} else
			{
				GroupOption grp = (GroupOption)m_lastGroup.Peek();
				grp.SubOptions.Add( opt );
			}
		}
	}


}
