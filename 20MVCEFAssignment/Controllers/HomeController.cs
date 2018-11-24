using _20MVCEFAssignment.Models;
using _20MVCEFAssignment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20MVCEFAssignment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(int EdittweetId = 0, string userName = null)
        {
            TweetModel model = new TweetModel();
            userName = Convert.ToString(Session["UserName"]);
            ViewBag.Users = GetUserList();
            model = GetTweet(userName, EdittweetId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TweetModel tweetModel)
        {
            AppRepository repo = new AppRepository();
            if (ModelState.IsValid)
            {
                
                var status = repo.SaveTweet(tweetModel.TweetMessage);
                if (status)
                {
                    var res = GetTweet(tweetModel.TweetMessage.User_Id, tweetModel.TweetMessage.Tweet_Id);
                    return RedirectToAction("Index", tweetModel);
                }
            }

            return RedirectToAction("Index", tweetModel);

        }

        public ActionResult EditTweet(int tweetId)
        {
            return RedirectToAction("Index", new { EdittweetId = tweetId });
        }

        public ActionResult DeleteTweet(int tweetId)
        {
            AppRepository repo = new AppRepository();
            repo.DeleteTweet(tweetId);
            return RedirectToAction("Index");            //, new { EdittweetId = tweetId }
        }

        private List<string> GetUserList()
        {
            AppRepository repo = new AppRepository();
            return repo.GetUserList();
        }

        private TweetModel GetTweet(string userName, int tweetId)
        {
            TweetModel model = new TweetModel();
            AppRepository repo = new AppRepository();
            model.TweetMessage = new TweetMessage();

            var Tweets = repo.GetAllTweets(userName);
            var tweetDetails = repo.GetTweetMessageDetails(userName);
            if (tweetId == 0)
                model.TweetMessage.User_Id = userName;
            else
            {
                model.TweetMessage = Tweets.Where(x => x.Tweet_Id == tweetId).FirstOrDefault();
            }

            model.lstTweetMessage = Tweets;
            model.TotalTweets = tweetDetails.TotalTweets;
            model.TotalFollowing = tweetDetails.TotalFollowing;
            model.TotalFollowers = tweetDetails.TotalFollowers;
            return model;
        }
    }
}