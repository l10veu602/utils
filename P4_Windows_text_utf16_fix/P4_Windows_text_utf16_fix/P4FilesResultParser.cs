using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4_Windows_text_utf16_fix
{
    static class P4FileTypes
    {
        public const string Text = "text";
    }

    struct P4FilesResult
    {
        public string DepotPath { get; set; }
        public int Revision { get; set; }
        public string Action { get; set; }
        public int Changelist { get; set; }
        public string FileType { get; set; }
    }

    class P4FilesResultParser
    {
        private const string DepotPathRegexString = "([^#]*)";
        private const string RevisionRegexString = "([^ ]*)";
        private const string ActionRegexString = "([^ ]*)";
        private const string ChangelistRegexString = "([^ ]*)";
        private const string FileTypeRegexString = "(.*)";

        private static readonly Regex P4FilesResultRegex = new Regex(
            $"{DepotPathRegexString}#{RevisionRegexString} - {ActionRegexString} change {ChangelistRegexString} \\({FileTypeRegexString}\\)");

        public static P4FilesResult Parse(string p4FilesResultString)
        {
            var match = P4FilesResultRegex.Match(p4FilesResultString);
            var groups = match.Groups;

            var p4FilesResult = new P4FilesResult();

            p4FilesResult.DepotPath = groups[1].Value;

            string revisionString = groups[2].Value;
            p4FilesResult.Revision = int.Parse(revisionString);

            p4FilesResult.Action = groups[3].Value;

            string changelistString = groups[4].Value;
            p4FilesResult.Changelist = int.Parse(changelistString);

            p4FilesResult.FileType = groups[5].Value;

            return p4FilesResult;
        }

        public static IEnumerable<P4FilesResult> ParseList(IEnumerable<string> p4FilesResults)
        {
            return p4FilesResults.Select(result => Parse(result)).ToList();
        }
    }
}
