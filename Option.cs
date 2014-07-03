using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace PdoxParserTest
{
	enum OptionType
	{
		Group,
		String,	 
		StringList,
		Bool,
		Float,
		Integer,
		IntList,
		Colour,
		Date,
	}

	[DebuggerDisplay( "Tag: {GetIDString}, Data: {GetValueString}" )]
	abstract class Option
	{
		public readonly OptionType Type;

		protected Option( OptionType type )
		{
			Type = type;
		}

		public abstract string GetIDString
		{
			get;
		}
		public abstract string GetValueString
		{
			get;
		}

		public static Option Parse( string tag, string data )
		{
			Option opt;

			if( Char.IsDigit( data[0] ) || ( data[0] == '-' && Char.IsDigit( data[1] ) ) )
			{
				if( data.Contains( "." ) )
				{
					if( data.IndexOf( "." ) != data.LastIndexOf( "." ) )
					{
						string[] parts = data.Split( '.' );
						opt = new DateOption( tag, new DateTime( Int32.Parse( parts[0] ), Int32.Parse( parts[1] ), Int32.Parse( parts[1] ) ) );
					} else
						opt = new FloatOption( tag, Double.Parse( data, CultureInfo.InvariantCulture ) );
				} else
					opt = new IntegerOption( tag, Int32.Parse( data ) );
			} else if( data == "yes" || data == "no" )
			{
				opt = new BoolOption( tag, data == "yes" );
			} else
			{
				opt = new StringOption( tag, data );
			}

			return opt;
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, SubOptions: {SubOptions.Count}" )]
	class GroupOption : Option
	{
		public List<Option> SubOptions;
		private readonly string m_id;

		public GroupOption( string id )
			: base( OptionType.Group )
		{
			m_id = id;
			SubOptions = new List<Option>();
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return SubOptions.Count.ToString( CultureInfo.InvariantCulture );
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class StringOption : Option
	{
		private readonly string m_id;
		private readonly string m_value;

		public StringOption( string id, string value )
			: base( OptionType.String )
		{
			m_id = id;
			m_value = value;
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return
					m_value;
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class BoolOption : Option
	{
		private readonly string m_id;
		private readonly bool m_value;

		public BoolOption( string id, bool value )
			: base( OptionType.Bool )
		{
			m_id = id;
			m_value = value;
		}

		public bool Value
		{
			get
			{
				return m_value;
			}
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return
					Value ? "yes" : "no";
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class IntegerOption : Option
	{
		private readonly string m_id;
		private readonly int m_value;

		public IntegerOption( string id, int value )
			: base( OptionType.Integer )
		{
			m_id = id;
			m_value = value;
		}

		public int Value
		{
			get
			{
				return m_value;
			}
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return Value.ToString( CultureInfo.InvariantCulture );
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class FloatOption : Option
	{
		private readonly string m_id;
		private readonly double m_value;

		public FloatOption( string id, double value )
			: base( OptionType.Float )
		{
			m_id = id;
			m_value = Math.Round( value, 3 );
		}

		public double Value
		{
			get
			{
				return m_value;
			}
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return m_value.ToString( CultureInfo.InvariantCulture );
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class ColourOption : Option
	{
		private readonly string m_id;
		private readonly Color m_value;

		public ColourOption( string id, Color value )
			: base( OptionType.Colour )
		{
			m_id = id;
			m_value = value;
		}

		public Color Value
		{
			get
			{
				return m_value;
			}
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return m_value.ToString();
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data: {m_value}" )]
	class DateOption : Option
	{
		private readonly string m_id;
		private readonly DateTime m_value;

		public DateOption( string id, DateTime value )
			: base( OptionType.Date )
		{
			m_id = id;
			m_value = value;
		}

		public DateTime Value
		{
			get
			{
				return m_value;
			}
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return m_value.ToString( CultureInfo.InvariantCulture );
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data Count: {Items.Count}" )]
	class IntListOption : Option
	{
		private readonly string m_id;
		public readonly IList<int> Items;

		public IntListOption( string id, IList<int> value )
			: base( OptionType.IntList )
		{
			m_id = id;
			Items = value;
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return Items.Count.ToString();
			}
		}
	}

	[DebuggerDisplay( "Tag: {m_id}, Data Count: {Items.Count}" )]
	class StringListOption : Option
	{
		private readonly string m_id;
		public readonly IList<string> Items;

		public StringListOption( string id, IList<string> value )
			: base( OptionType.StringList )
		{
			m_id = id;
			Items = value;
		}

		public override string GetIDString
		{
			get
			{
				return m_id;
			}
		}

		public override string GetValueString
		{
			get
			{
				return Items.Count.ToString();
			}
		}
	}
}
