using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangDiagnostic : ClangObject, IDisposable
	{
		public static string GetCategoryName (uint category)
		{
			return LibClang.clang_getDiagnosticCategoryName (category).Unwrap ();
		}
		
		public struct CommandLineOptions
		{
			public CommandLineOptions (string enable, string disable)
			{
				this.enable = enable;
				this.disable = disable;
			}

			readonly string enable, disable;

			public string Enable {
				get { return enable; }
			}
			public string Disable {
				get { return disable; }
			}
		}

		public struct FixIt
		{
			public FixIt (ClangSourceRange replacementRange, string replatementText)
			{
				replacement_range = replacementRange;
				replatement_text = replatementText;
			}

			readonly ClangSourceRange replacement_range;
			readonly string replatement_text;

			public ClangSourceRange ReplacementRange {
				get { return replacement_range; }
			}

			public string ReplacementText {
				get { return replatement_text; }
			}
		}

		public ClangDiagnostic (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_disposeDiagnostic (Handle);
		}

		public ClangDiagnosticSet ChildDiagnostics {
			get { return new ClangDiagnosticSet (LibClang.clang_getChildDiagnostics (Handle)); }
		}

		public string Format (DiagnosticDisplayOptions options)
		{
			return LibClang.clang_formatDiagnostic (Handle, (uint) options).Unwrap ();
		}

		public DiagnosticSeverity Severity {
			get { return (DiagnosticSeverity) LibClang.clang_getDiagnosticSeverity (Handle); }
		}

		public ClangSourceLocation Location {
			get { return new ClangSourceLocation (LibClang.clang_getDiagnosticLocation (Handle)); }
		}

		public string Spelling {
			get { return LibClang.clang_getDiagnosticSpelling (Handle).Unwrap (); }
		}

		public CommandLineOptions Options {
			get
			{
				Pointer<CXString> d = default (Pointer<CXString>);
				var e = LibClang.clang_getDiagnosticOption (Handle, d);
				var ptr = Marshal.ReadIntPtr (d);
				return new CommandLineOptions (e.Unwrap (), Marshal.PtrToStringAnsi (ptr));
			}
		}

		// no corresponding enum for this...
		public uint Category {
			get { return LibClang.clang_getDiagnosticCategory (Handle); }
		}

		public string CategoryText {
			get { return LibClang.clang_getDiagnosticCategoryText (Handle).Unwrap (); }
		}

		public int RangeCount {
			get { return (int) LibClang.clang_getDiagnosticNumRanges (Handle); }
		}

		public ClangSourceRange GetDiagnosticRange (int range)
		{
			return new ClangSourceRange (LibClang.clang_getDiagnosticRange (Handle, (uint) range));
		}

		public int FixItCount {
			get { return (int) LibClang.clang_getDiagnosticNumFixIts (Handle); }
		}

		public FixIt GetFixIt (int index)
		{
			Pointer<CXSourceRange> range = default (Pointer<CXSourceRange>);
			var ret = LibClang.clang_getDiagnosticFixIt (Handle, (uint) index, range).Unwrap ();
			return new FixIt (new ClangSourceRange (Marshal.PtrToStructure<CXSourceRange> (range)), ret);
		}
	}
}
