using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CsvParser2
{
    private const char CommaCharacter = ',';
    private const char QuoteCharacter = '"';

    #region Nested types

    private abstract class ParserState
    {
        public static readonly LineStartState LineStartState = new LineStartState();
        public static readonly ValueStartState ValueStartState = new ValueStartState();
        public static readonly ValueState ValueState = new ValueState();
        public static readonly QuotedValueState QuotedValueState = new QuotedValueState();
        public static readonly QuoteState QuoteState = new QuoteState();

        public abstract ParserState AnyChar(char ch, ParserContext context);
        public abstract ParserState Comma(ParserContext context);
        public abstract ParserState Quote(ParserContext context);
        public abstract ParserState EndOfLine(ParserContext context);
    }

    private class LineStartState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return ValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return ValueStartState;
        }

        public override ParserState Quote(ParserContext context)
        {
            return QuotedValueState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddLine();
            return LineStartState;
        }
    }

    private class ValueStartState : LineStartState
    {
        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }

    private class ValueState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return ValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return ValueStartState;
        }

        public override ParserState Quote(ParserContext context)
        {
            context.AddChar(QuoteCharacter);
            return ValueState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }

    private class QuotedValueState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return QuotedValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddChar(CommaCharacter);
            return QuotedValueState;
        }

        public override ParserState Quote(ParserContext context)
        {
            return QuoteState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddChar('\r');
            context.AddChar('\n');
            return QuotedValueState;
        }
    }

    private class QuoteState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            //undefined, ignore "
            context.AddChar(ch);
            return QuotedValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return ValueStartState;
        }

        public override ParserState Quote(ParserContext context)
        {
            context.AddChar(QuoteCharacter);
            return QuotedValueState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }

    private class ParserContext
    {
        private readonly StringBuilder _currentValue = new StringBuilder();
        private readonly List<string[]> _lines = new List<string[]>();
        private readonly List<string> _currentLine = new List<string>();

        public ParserContext()
        {
            MaxColumnsToRead = 1000;
        }

        public int MaxColumnsToRead { get; set; }
        
        public void AddChar(char ch)
        {
            _currentValue.Append(ch);
        }

        public void AddValue()
        {
            if (_currentLine.Count < MaxColumnsToRead)
                _currentLine.Add(_currentValue.ToString());
            _currentValue.Remove(0, _currentValue.Length);
        }

        public void AddLine()
        {
            _lines.Add(_currentLine.ToArray());
            _currentLine.Clear();
        }

        public List<string[]> GetAllLines()
        {
            if (_currentValue.Length > 0)
            {
                AddValue();
            }
            if (_currentLine.Count > 0)
            {
                AddLine();
            }
            return _lines;
        }
    }

    #endregion

    public bool TrimTrailingEmptyLines { get; set; }
    public int MaxColumnsToRead { get; set; }

    public string[][] Parse(TextReader reader)
    {
        var context = new ParserContext();
        if (MaxColumnsToRead != 0)
            context.MaxColumnsToRead = MaxColumnsToRead;

        ParserState currentState = ParserState.LineStartState;
        string next;
        while ((next = reader.ReadLine()) != null)
        {
            foreach (char ch in next)
            {
                switch (ch)
                {
                    case CommaCharacter:
                        currentState = currentState.Comma(context);
                        break;
                    case QuoteCharacter:
                        currentState = currentState.Quote(context);
                        break;
                    default:
                        currentState = currentState.AnyChar(ch, context);
                        break;
                }
            }
            currentState = currentState.EndOfLine(context);
        }
        List<string[]> allLines = context.GetAllLines();
        if (TrimTrailingEmptyLines && allLines.Count > 0)
        {
            bool isEmpty = true;
            for (int i = allLines.Count - 1; i >= 0; i--)
            {
// ReSharper disable RedundantAssignment
                isEmpty = true;
// ReSharper restore RedundantAssignment
                for (int j = 0; j < allLines[i].Length; j++)
                {
                    if (!String.IsNullOrEmpty(allLines[i][j]))
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                {
                    if (i < allLines.Count - 1)
                        allLines.RemoveRange(i + 1, allLines.Count - i - 1);
                    break;
                }
            }
            if (isEmpty)
                allLines.RemoveRange(0, allLines.Count);
        }
        return allLines.ToArray();
    }

	public static string[][] Parse(string input)
	{
		CsvParser parser = new CsvParser();
		
		using (StringReader reader = new StringReader(input))
		{
			return parser.Parse(reader);
		}
	}
}