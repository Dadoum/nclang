﻿using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXToken
	{
		[MarshalAs (UnmanagedType.SafeArray, SizeConst = 4)]
		public readonly uint[] int_data;
		public readonly IntPtr ptr_data;
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern TokenKind 	clang_getTokenKind (CXToken _);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getTokenSpelling (CXTranslationUnit _, CXToken __);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation 	clang_getTokenLocation (CXTranslationUnit _, CXToken __);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange 	clang_getTokenExtent (CXTranslationUnit _, CXToken __);
		// CXToken** Tokens
		[DllImport (LibraryName)]
		 internal static extern void 	clang_tokenize (CXTranslationUnit TU, CXSourceRange Range, out IntPtr Tokens, [MarshalAs (UnmanagedType.SysUInt)] out uint NumTokens);

		[DllImport (LibraryName)]
		internal static extern void 	clang_annotateTokens (CXTranslationUnit TU, IntPtr Tokens, [MarshalAs (UnmanagedType.SysUInt)] uint NumTokens, ref IntPtr Cursors);

		[DllImport (LibraryName)]
		internal static extern void 	clang_disposeTokens (CXTranslationUnit TU, IntPtr Tokens, [MarshalAs (UnmanagedType.SysUInt)] uint NumTokens);
	}
}