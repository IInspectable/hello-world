﻿#region Using Directives

using System;
using System.IO;

using JetBrains.Annotations;

using Pharmatechnik.Nav.Language.Internal;

#endregion

namespace Pharmatechnik.Nav.Language.Text {

    public abstract class SourceText {

        [CanBeNull]
        public abstract FileInfo FileInfo { get; }

        [NotNull]
        public abstract string Text { get; }

        public abstract int Length { get; }

        [NotNull]
        public abstract SourceTextLineList TextLines { get; }

        public abstract string Substring(int startIndex, int length);

        public static SourceText From(string text, string filePath = null) {
            return new StringSourceText(text: text, filePath: filePath);
        }

        public static SourceText Empty => new StringSourceText(null, null);

        public Location GetLocation(TextExtent extent) {
            return new Location(extent, GetLineRange(extent), FileInfo?.FullName);
        }

        public override string ToString() {
            return Text;
        }

        public string Substring(TextExtent textExtent) {
            return Text.Substring(startIndex: textExtent.Start, length: textExtent.Length);
        }

        public SourceTextLine GetTextLineAtPosition(int position) {
            if (position < 0 || position > Length) {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return GetTextLineAtPositionCore(position);
        }

        LineRange GetLineRange(TextExtent extent) {

            var start = GetLinePositionAtPosition(extent.Start);
            var end   = GetLinePositionAtPosition(extent.End);

            return new LineRange(start, end);
        }

        LinePosition GetLinePositionAtPosition(int position) {
            var lineInformaton = GetTextLineAtPositionCore(position);
            return new LinePosition(lineInformaton.Line, position - lineInformaton.Extent.Start);
        }

        int _lastLineNumber;

        SourceTextLine GetTextLineAtPositionCore(int position) {

            if (position == 0) {
                return TextLines[0];
            }

            if (position == Length) {
                return TextLines[TextLines.Count - 1];
            }

            // Natürlich ist der Zugriff auf _lastLineNumber nicht "Threadsafe". Das macht aber auch nichts. Wir verwenden den Wert nur als Hint
            // da davon auszugehen ist, dass die Zugriffe auf die Zeileninformationen immer in etwa im selben Bereich stattfinden. Im worst case
            // werden ohnehin alle Zeilen durchsucht-
            var lastLineNumber = _lastLineNumber;
            if (position >= TextLines[lastLineNumber].Start) {
                var limit = Math.Min(TextLines.Count, lastLineNumber + 4);
                for (int i = lastLineNumber; i < limit; i++) {
                    if (position < TextLines[i].Start) {
                        var lineNumber = i - 1;
                        _lastLineNumber = lineNumber;
                        return TextLines[lineNumber];
                    }
                }
            }

            var textLine = TextLines.FindElementAtPosition(position);
            _lastLineNumber = textLine.Line;
            return textLine;
        }

    }

}