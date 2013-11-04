﻿using System;
using System.Runtime.InteropServices;

namespace LibGit2Sharp.Core
{
    [Flags]
    internal enum GitDiffOptionFlags
    {
        /// <summary>
        /// Normal diff, the default
        /// </summary>
        GIT_DIFF_NORMAL = 0,

        /*
         * Options controlling which files will be in the diff
         */

        /// <summary>
        /// Reverse the sides of the diff
        /// </summary>
        GIT_DIFF_REVERSE = (1 << 0),

        /// <summary>
        /// Include ignored files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_IGNORED = (1 << 1),

        /// <summary>
        /// Even with GIT_DIFF_INCLUDE_IGNORED, an entire ignored directory
        /// will be marked with only a single entry in the diff; this flag
        /// adds all files under the directory as IGNORED entries, too.
        /// </summary>
        GIT_DIFF_RECURSE_IGNORED_DIRS = (1 << 2),

        /// <summary>
        /// Include untracked files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNTRACKED = (1 << 3),

        /// <summary>
        /// Even with GIT_DIFF_INCLUDE_UNTRACKED, an entire untracked
        /// directory will be marked with only a single entry in the diff
        /// (a la what core Git does in `git status`); this flag adds *all*
        /// files under untracked directories as UNTRACKED entries, too.
        /// </summary>
        GIT_DIFF_RECURSE_UNTRACKED_DIRS = (1 << 4),

        /// <summary>
        /// Include unmodified files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNMODIFIED = (1 << 5),

        /// <summary>
        /// Normally, a type change between files will be converted into a
        /// DELETED record for the old and an ADDED record for the new; this
        /// options enabled the generation of TYPECHANGE delta records.
        /// </summary>
        GIT_DIFF_INCLUDE_TYPECHANGE = (1 << 6),

        /// <summary>
        /// Even with GIT_DIFF_INCLUDE_TYPECHANGE, blob->tree changes still
        /// generally show as a DELETED blob.  This flag tries to correctly
        /// label blob->tree transitions as TYPECHANGE records with new_file's
        /// mode set to tree.  Note: the tree SHA will not be available.
        /// </summary>
        GIT_DIFF_INCLUDE_TYPECHANGE_TREES = (1 << 7),

        /// <summary>
        /// Ignore file mode changes
        /// </summary>
        GIT_DIFF_IGNORE_FILEMODE = (1 << 8),

        /// <summary>
        /// Treat all submodules as unmodified
        /// </summary>
        GIT_DIFF_IGNORE_SUBMODULES = (1 << 9),

        /// <summary>
        /// Use case insensitive filename comparisons
        /// </summary>
        GIT_DIFF_IGNORE_CASE = (1 << 10),

        /// <summary>
        /// If the pathspec is set in the diff options, this flags means to
        /// apply it as an exact match instead of as an fnmatch pattern.
        /// </summary>
        GIT_DIFF_DISABLE_PATHSPEC_MATCH = (1 << 12),

        /// <summary>
        /// Disable updating of the `binary` flag in delta records.  This is
        /// useful when iterating over a diff if you don't need hunk and data
        /// callbacks and want to avoid having to load file completely.
        /// </summary>
        GIT_DIFF_SKIP_BINARY_CHECK = (1 << 13),

        /// <summary>
        /// When diff finds an untracked directory, to match the behavior of
        /// core Git, it scans the contents for IGNORED and UNTRACKED files.
        /// If *all* contents are IGNORED, then the directory is IGNORED; if
        /// any contents are not IGNORED, then the directory is UNTRACKED.
        /// This is extra work that may not matter in many cases.  This flag
        /// turns off that scan and immediately labels an untracked directory
        /// as UNTRACKED (changing the behavior to not match core Git).
        /// </summary>
        GIT_DIFF_ENABLE_FAST_UNTRACKED_DIRS = (1 << 14),

        /*
         * Options controlling how output will be generated
         */

        /// <summary>
        /// Treat all files as text, disabling binary attributes and detection
        /// </summary>
        GIT_DIFF_FORCE_TEXT = (1 << 20),

        /// <summary>
        /// Treat all files as binary, disabling text diffs
        /// </summary>
        GIT_DIFF_FORCE_BINARY = (1 << 21),

        /// <summary>
        /// Ignore all whitespace
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE = (1 << 22),

        /// <summary>
        /// Ignore changes in amount of whitespace
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE_CHANGE = (1 << 23),

        /// <summary>
        /// Ignore whitespace at end of line
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE_EOL = (1 << 24),

        /// <summary>
        /// When generating patch text, include the content of untracked
        /// files.  This automatically turns on GIT_DIFF_INCLUDE_UNTRACKED but
        /// it does not turn on GIT_DIFF_RECURSE_UNTRACKED_DIRS.  Add that
        /// flag if you want the content of every single UNTRACKED file.
        /// </summary>
        GIT_DIFF_SHOW_UNTRACKED_CONTENT = (1 << 25),

        /// <summary>
        /// When generating output, include the names of unmodified files if
        /// they are included in the git_diff.  Normally these are skipped in
        /// the formats that list files (e.g. name-only, name-status, raw).
        /// Even with this, these will not be included in patch format.
        /// </summary>
        GIT_DIFF_SHOW_UNMODIFIED = (1 << 26),

        /// <summary>
        /// Use the "patience diff" algorithm
        /// </summary>
        GIT_DIFF_PATIENCE = (1 << 28),

        /// <summary>
        /// Take extra time to find minimal diff
        /// </summary>
        GIT_DIFF_MINIMAL = (1 << 29),
    }

    internal delegate int diff_notify_cb(
        IntPtr diff_so_far,
        IntPtr delta_to_add,
        IntPtr matched_pathspec,
        IntPtr payload);

    [StructLayout(LayoutKind.Sequential)]
    internal class GitDiffOptions : IDisposable
    {
        public uint Version = 1;
        public GitDiffOptionFlags Flags;

        /* options controlling which files are in the diff */

        public SubmoduleIgnore IgnoreSubmodules;
        public GitStrArrayIn PathSpec;
        public diff_notify_cb NotifyCallback;
        public IntPtr NotifyPayload;

        /* options controlling how to diff text is generated */

        public ushort ContextLines;
        public ushort InterhunkLines;
        public ushort OidAbbrev;
        public Int64 MaxSize;
        public IntPtr OldPrefixString;
        public IntPtr NewPrefixString;

        public void Dispose()
        {
            if (PathSpec == null)
            {
                return;
            }

            PathSpec.Dispose();
        }
    }

    [Flags]
    internal enum GitDiffFileFlags
    {
        GIT_DIFF_FLAG_BINARY = (1 << 0),
        GIT_DIFF_FLAG_NOT_BINARY = (1 << 1),
        GIT_DIFF_FLAG_VALID_OID = (1 << 2),
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class GitDiffFile
    {
        public GitOid Oid;
        public IntPtr Path;
        public Int64 Size;
        public GitDiffFileFlags Flags;
        public ushort Mode;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class GitDiffDelta
    {
        public ChangeKind Status;
        public uint Flags;
        public ushort Similarity;
        public ushort NumberOfFiles;
        public GitDiffFile OldFile;
        public GitDiffFile NewFile;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class GitDiffHunk
    {
        public int OldStart;
        public int OldLines;
        public int NewStart;
        public int NewLines;
        public UIntPtr HeaderLen;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] Header;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class GitDiffLine
    {
        public GitDiffLineOrigin lineOrigin;
        public int OldLineNo;
        public int NewLineNo;
        public int NumLines;
        public UIntPtr contentLen;
        public IntPtr content;
    }

    enum GitDiffLineOrigin : byte
    {
        GIT_DIFF_LINE_CONTEXT = 0x20, //' ',
        GIT_DIFF_LINE_ADDITION = 0x2B, //'+',
        GIT_DIFF_LINE_DELETION = 0x2D, //'-',
        GIT_DIFF_LINE_ADD_EOFNL = 0x0A, //'\n', /**< LF was added at end of file */
        GIT_DIFF_LINE_DEL_EOFNL = 0x0, //'\0', /**< LF was removed at end of file */

        /* these values will only be sent to a `git_diff_output_fn` */
        GIT_DIFF_LINE_FILE_HDR = 0x46, //'F',
        GIT_DIFF_LINE_HUNK_HDR = 0x48, //'H',
        GIT_DIFF_LINE_BINARY = 0x42, //'B',
    }
}
