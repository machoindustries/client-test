using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace xcentium.Helpers
{
    public class utilities
    {
        WebClient client = new WebClient();
        public string getHTMLContent(string HTMLString, string userAgent)
        {
            string HTMLpagetoParse = "";
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers[HttpRequestHeader.UserAgent] = userAgent;
            Uri getSomeContent = new Uri(HTMLString);

            // Instead of asking the client to tell us which document they are requesting
            // we assume a default one
            //if (getSomeContent.AbsolutePath == "/")
            //{
            //    getSomeContent = new Uri(getSomeContent, "index.html");
            //}

            HTMLpagetoParse = client.DownloadString(getSomeContent);

            return HTMLpagetoParse;
        }

        public string stripHTML(string inputString)
        {
            string htmlPattern = "<[^>]+?>";
            
            inputString = RemoveTag(inputString, "<!--", "-->");
            inputString = RemoveTag(inputString, "<script", "</script>");
            inputString = RemoveTag(inputString, "<style", "</style>");

            var html = Regex.Replace(inputString, htmlPattern, string.Empty);
            html = SingleSpacedTrim(html);
            return html;
        }

        public int countWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }

        private static readonly Regex _tags_ = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        //add characters that are should not be removed to this regex
        private static readonly Regex _notOkCharacter_ = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);

        public String UnHtml(String html)
        {
            html = HttpUtility.UrlDecode(html);
            html = HttpUtility.HtmlDecode(html);

            html = RemoveTag(html, "<!--", "-->");
            html = RemoveTag(html, "<script", "</script>");
            html = RemoveTag(html, "<style", "</style>");

            //replace matches of these regexes with space
            html = _tags_.Replace(html, " ");
            html = _notOkCharacter_.Replace(html, " ");
            html = SingleSpacedTrim(html);

            return html;
        }

        private static String RemoveTag(String html, String startTag, String endTag)
        {
            Boolean bAgain;
            do
            {
                bAgain = false;
                Int32 startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
                if (startTagPos < 0)
                    continue;
                Int32 endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
                if (endTagPos <= startTagPos)
                    continue;
                html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
                bAgain = true;
            } while (bAgain);
            return html;
        }

        private static String SingleSpacedTrim(String inString)
        {
            StringBuilder sb = new StringBuilder();
            Boolean inBlanks = false;
            foreach (Char c in inString)
            {
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        if (!inBlanks)
                        {
                            inBlanks = true;
                            sb.Append(' ');
                        }
                        continue;
                    default:
                        inBlanks = false;
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Trim();
        }

    }
}