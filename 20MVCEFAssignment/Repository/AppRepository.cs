using _20MVCEFAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20MVCEFAssignment.Repository
{
    public class AppRepository
    {
        FSEAssignmentEntities _dbContext;
        public AppRepository()
        {
            _dbContext = new FSEAssignmentEntities();
        }

        public Person AuthenticateUser(string UserName, string Password)
        {
            Person person = null;
            try
            {
                Person authPerson = _dbContext.People.Where(a => a.User_Id.ToLower() == UserName.ToLower()).FirstOrDefault();
                if (authPerson != null)
                {
                    if (authPerson.Password == Password)
                    {
                        return authPerson;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return person;
        }

        public bool UserAlreadyExists(string userId)
        {
            bool isUserExist = false;
            using (_dbContext = new FSEAssignmentEntities())
            {
                var items = _dbContext.People.Where(x => x.User_Id.ToLower().Trim() == userId.ToLower().Trim()).FirstOrDefault();
                isUserExist = (items == null) ? false : true;
            }

            return isUserExist;
        }

        public PersonModel GetProfile(string userId)
        {
            PersonModel prModel = new PersonModel();
            using (_dbContext = new FSEAssignmentEntities())
            {
                var items = _dbContext.People.Where(x => x.User_Id.Trim() == userId.Trim()).FirstOrDefault();
                if (items != null)
                {
                    prModel.User_Id = items.User_Id;
                    prModel.Password = items.Password;
                    prModel.FullName = items.FullName;
                    prModel.Email = items.Email;
                    prModel.Joined = items.Joined;
                    prModel.Active = items.Active;
                }
            }

            return prModel;
        }


        public List<string> GetUserList()
        {
            List<string> lstUser = new List<string>();
            using (_dbContext = new FSEAssignmentEntities())
            {
                lstUser = _dbContext.People.Select(x => x.User_Id).ToList();
            }

            return lstUser != null ? lstUser : new List<string>();
        }

        public bool RegisterOrUpdateUser(PersonModel personModel)
        {
            bool isRegistered = false;
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    Person person = _dbContext.People.Where(x => x.User_Id.ToUpper().Trim() == personModel.User_Id.ToUpper().Trim()).FirstOrDefault();

                    if (person == null)
                    {
                        Person user = new Person();
                        user.User_Id = personModel.User_Id;
                        user.Password = personModel.Password;
                        user.FullName = personModel.FullName;
                        user.Email = personModel.Email;
                        user.Joined = DateTime.Now;
                        user.Active = true;
                        _dbContext.People.Add(user);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(personModel.Password.Trim()))
                            person.Password = personModel.Password;
                        person.FullName = personModel.FullName;
                        person.Email = personModel.Email;
                        _dbContext.Entry<Person>(person).State = System.Data.Entity.EntityState.Modified;
                    }

                    _dbContext.SaveChanges();
                    isRegistered = true;
                }
            }
            catch (Exception ex)
            {
                isRegistered = false;
            }

            return isRegistered;
        }

        public TweetModel GetTweetMessageDetails(string userName)
        {
            TweetModel tweetModel = new TweetModel();
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    var tweets = _dbContext.Tweets.Where(x => x.User_Id.Trim().ToUpper() == userName.ToUpper().Trim());
                    var following = _dbContext.Followings.Where(x => x.User_Id.Trim().ToUpper() == userName.ToUpper().Trim());
                    var followers = _dbContext.Followings.Where(x => x.Following_Id.Trim().ToUpper() == userName.ToUpper().Trim());

                    if (tweets != null)
                        tweetModel.TotalTweets = tweets.Count();

                    if (followers != null)
                        tweetModel.TotalFollowers = followers.Count();

                    if (following != null)
                        tweetModel.TotalFollowing = following.Count();


                }
            }
            catch (Exception ex)
            {

            }
            return tweetModel;
        }

        public List<TweetMessage> GetAllTweets(string userName)
        {
            List<TweetMessage> lstTweetMessage = new List<TweetMessage>();
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    var tweets = _dbContext.Tweets.ToList();// .Where(x => x.User_Id.Trim().ToUpper() == userName.ToUpper().Trim());

                    lstTweetMessage = tweets.Select(t =>
                    new TweetMessage
                    {
                        Tweet_Id = t.Tweet_Id,
                        Message = t.Message,
                        User_Id = t.User_Id,
                        Created = t.Created,
                        IsUserTweet = t.User_Id.Trim().ToUpper() == userName.ToUpper().Trim() ? true : false
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return lstTweetMessage;
        }

        public bool SaveTweet(TweetMessage tweetMsg)
        {
            bool IsSuccess = false;
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    var tweets = _dbContext.People.Where(x => x.User_Id.Trim().ToUpper() == tweetMsg.User_Id.Trim().ToUpper()).FirstOrDefault();
                    var tweet = _dbContext.Tweets.Where(x => x.Tweet_Id == tweetMsg.Tweet_Id).FirstOrDefault();
                    if (tweet == null && tweetMsg.Tweet_Id == 0)
                    {
                        tweet = new Tweet();
                        tweet.User_Id = tweetMsg.User_Id;
                        tweet.Message = tweetMsg.Message;
                        tweet.Created = DateTime.Now;

                        _dbContext.Entry(tweet).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        tweet.Message = tweetMsg.Message;
                    }
                    IsSuccess = true;

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return IsSuccess;
        }
        public string DeleteTweet(int tweetId)
        {
            string strUserName = string.Empty;
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    var tweet = _dbContext.Tweets.Where(x => x.Tweet_Id == tweetId).FirstOrDefault();//_context.TWEETs.Where(x => x.tweet_id == tweetMsg.TweetId).FirstOrDefault();
                    if (tweet != null && tweetId != 0)
                    {
                        strUserName = tweet.User_Id;
                        _dbContext.Tweets.Remove(tweet);
                    }
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return strUserName;
        }

        public TweetMessage EditTweet(int tweetId)
        {
            TweetMessage _editTweet = new TweetMessage();
            try
            {
                using (_dbContext = new FSEAssignmentEntities())
                {
                    var tweet = _dbContext.Tweets.Where(x => x.Tweet_Id == tweetId).FirstOrDefault();//_context.TWEETs.Where(x => x.tweet_id == tweetMsg.TweetId).FirstOrDefault();
                    _editTweet.User_Id = tweet.User_Id;
                    _editTweet.Message = tweet.Message;
                    _editTweet.Tweet_Id = tweet.Tweet_Id;
                    _editTweet.Created = DateTime.Now;
                    _dbContext.Entry(_editTweet).State = System.Data.Entity.EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return _editTweet;

        }
    }
}