using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using xcentium.Helpers;
using xcentium.Models;

namespace xcentium.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string siteToParse)
        {
            ViewBag.Message = "";
            ViewBag.topTenWords = "";
            ViewBag.nameOfWebsite = "";
            ViewBag.totalWordCount = "";
            utilities util = new utilities();
            List<htmlImageViewModels> listOfImages = new List<htmlImageViewModels>();
            

            // Download string.
            var userAgent = Request.UserAgent;

            //string HTMLString = "https://www.nytimes.com";
            string HTMLString = siteToParse;
            var value = util.getHTMLContent(HTMLString, userAgent);
            if (value.Count() > 0)
            {
                // Write values.
                string imgPattern = @"<img.+?src=[""'](.+?)[""'].*?>";
                string picturePattern = @"<source.+?srcset=[""'](.+?)[""'].*?>";

                Regex imgReg = new Regex(imgPattern);
                Regex picReg = new Regex(picturePattern);
                MatchCollection imgMatches = imgReg.Matches(value);
                MatchCollection picMatches = picReg.Matches(value);

                foreach (Match match in imgMatches)
                {
                    htmlImageViewModels vImage = new htmlImageViewModels();
                    vImage.imageSource = match.Groups[1].Value;

                    listOfImages.Add(vImage);
                }
                foreach (Match match in picMatches)
                {
                    htmlImageViewModels vImage = new htmlImageViewModels();
                    vImage.imageSource = match.Groups[1].Value;

                    listOfImages.Add(vImage);
                }

                string remainderText = util.stripHTML(value);

                int totalCountofWords = util.countWords(remainderText);

                IEnumerable<htmlWordCoundViewModels> orderedWords = Regex.Split(remainderText.ToLower(), @"\W+").GroupBy(s => s).Select(term => new htmlWordCoundViewModels { popularWord = term.Key, totalPageWordCount = totalCountofWords, wordCount = term.Count() }).OrderByDescending(g => g.wordCount).Take(10);

                var popularTerms = orderedWords;

                ViewBag.topTenWords = orderedWords;
                ViewBag.nameOfWebsite = siteToParse;
                ViewBag.totalWordCount = totalCountofWords;
            }

            else

            {
                ViewBag.Message = "Whoops! Seems like we aren't able to find this webpage right now. Please verify what you entered is accurate.";
            }
                        
            ViewData.Model = listOfImages;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}