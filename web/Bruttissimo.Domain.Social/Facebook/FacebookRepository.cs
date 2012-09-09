﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bruttissimo.Common;
using Bruttissimo.Domain.Entity;
using Facebook;
using Newtonsoft.Json;
using log4net;

namespace Bruttissimo.Domain.Social
{
    public class FacebookRepository : IFacebookRepository
    {
        private const int PAGE_LIMIT = 15;
        private const string GRAPH_FEED_LIMITED = "{0}/feed?limit={1}";
        private const string GRAPH_FEED_SINCE = "{0}&since={1}";

        private const string DEBUG_API_GET = "Facebook API GET: {0}";

        private readonly ILog log = LogManager.GetLogger(typeof(FacebookRepository));

        /// <summary>
        /// Access token used when no user-specific access token is provided to a method.
        /// </summary>
        private readonly string defaultAccessToken;

        public FacebookRepository(string defaultAccessToken)
        {
            if (defaultAccessToken == null)
            {
                throw new ArgumentNullException("defaultAccessToken");
            }
            this.defaultAccessToken = defaultAccessToken;
        }

        public IList<FacebookPost> GetPostsInFeed(FacebookImportOptions opts)
        {
            if (opts == null)
            {
                throw new ArgumentNullException("opts");
            }
            DateTime? since = opts.Since;
            string url = GRAPH_FEED_LIMITED.FormatWith(opts.Feed, PAGE_LIMIT);

            if (since.HasValue)
            {
                string date = since.Value.ToIso8601();
                url = GRAPH_FEED_SINCE.FormatWith(url, date);
            }

            IList<FacebookPost> result = FetchAll(url, since, opts.Log);
            return result;
        }

        public FacebookPost PostToFeed(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException("post");
            }
            if (post.FacebookFeedId == null) // sanity
            {
                return null;
            }
            string accessToken = GetAccessToken(post);
            /*
             * TODO: post to the facebook feed.
             */
            throw new NotImplementedException();
            FacebookPost result = new FacebookPost();
            return result;
        }

        internal string GetAccessToken(Post post)
        {
            throw new NotImplementedException();
            /*
             * this should be produced somewhere other than in this assembly (e.g the invoking method's assembly)
             * TODO: verify the user has a facebook connection
             * TODO: verify the user allows posts to be posted to facebook on his behalf
             * TODO: verify the access token is still valid (invalidate, set to null if it isn't)
             * TODO: if any of the above fail, return defaultAccessToken.
             */
            return defaultAccessToken;
        }

        internal IList<FacebookPost> FetchAll(string url, DateTime? since, FacebookImportLog importLog)
        {
            int queryCount = 0;
            List<FacebookPost> posts = new List<FacebookPost>();
            do
            {
                FacebookPostCollection response = Fetch(url);
                posts.AddRange(response.Data);

                queryCount++;

                if (since.HasValue && response.Data.Any(p => p.UpdatedTime <= since.Value)) // prevent unnecessary over-querying.
                {
                    posts.RemoveAll(p => p.UpdatedTime <= since.Value); // removed already-evaluated posts.
                    break;
                }
                if (response.Paging == null || response.Paging.Next == url) // sanity
                {
                    break;
                }
                url = response.Paging.Next;
            } while (url != null);

            importLog.QueryCount = queryCount;
            importLog.PostCount = posts.Count;
            importLog.PostUpdated = posts.Max(p => (DateTime?)p.UpdatedTime);
            return posts;
        }

        internal FacebookPostCollection Fetch(string url)
        {
            LogApiCall(url);

            FacebookClient client = new FacebookClient(defaultAccessToken);
            string json = (client.Get(url) ?? string.Empty).ToString();

            FacebookPostCollection response = JsonConvert.DeserializeObject<FacebookPostCollection>(json);
            return response;
        }

        private const string FACEBOOK_GRAPH_API = "https://graph.facebook.com/";
        private const string FACEBOOK_ACCESS_TOKEN_REGEX = "&access_token=(?:[^&])+";

        private void LogApiCall(string url)
        {
            string parameters = url;
            if (parameters.StartsWith(FACEBOOK_GRAPH_API))
            {
                parameters = parameters.Remove(0, FACEBOOK_GRAPH_API.Length);
            }
            parameters = Regex.Replace(parameters, FACEBOOK_ACCESS_TOKEN_REGEX, string.Empty);
            log.Debug(DEBUG_API_GET.FormatWith(parameters));
        }
    }
}
