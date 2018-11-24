using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20MVCEFAssignment.Models
{
    public class TweetModel
    {
        public int TotalTweets { get; set; }
        public int TotalFollowing { get; set; }
        public int TotalFollowers { get; set; }
        public TweetMessage TweetMessage { get; set; }
        public List<TweetMessage> lstTweetMessage { get; set; }
    }

    public class TweetMessage
    {
        public int Tweet_Id { get; set; }
        public string User_Id { get; set; }
        public string Message { get; set; }
        public System.DateTime Created { get; set; }
        public bool IsUserTweet { get; set; }
    }
}