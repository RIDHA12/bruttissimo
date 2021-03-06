using System.Collections.Generic;
using Bruttissimo.Common.Extensions;
using Bruttissimo.Common.Guard;
using Bruttissimo.Domain.DTO.Facebook;
using Bruttissimo.Domain.Entity.Entities;
using Bruttissimo.Domain.Entity.Social.Facebook;
using Bruttissimo.Domain.Repository;
using Bruttissimo.Domain.Repository.Social;
using Bruttissimo.Domain.Service;
using log4net;

namespace Bruttissimo.Domain.Logic.Service
{
    public class FacebookImporterService : BaseService, IFacebookImporterService
    {
        private readonly IFacebookRepository fbRepository;
        private readonly IPostRepository postRepository;
        private readonly ILinkRepository linkRepository;
        private readonly IUserRepository userRepository;

        private readonly ILog log = LogManager.GetLogger(typeof(FacebookImporterService));

        public FacebookImporterService(IFacebookRepository fbRepository, IPostRepository postRepository, ILinkRepository linkRepository, IUserRepository userRepository)
        {
            Ensure.That(() => fbRepository).IsNotNull();
            Ensure.That(() => postRepository).IsNotNull();
            Ensure.That(() => linkRepository).IsNotNull();
            Ensure.That(() => userRepository).IsNotNull();

            this.fbRepository = fbRepository;
            this.postRepository = postRepository;
            this.linkRepository = linkRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Imports a list of posts from Facebook, filters them and inserts into the persistance storage.
        /// </summary>
        public void Import(FacebookImportOptions opts)
        {
            int insertCount = 0;
            const string LINK_EXISTS = "Link exists: {0}";
            const string LINK_INSERTION = "Inserted Link #{0}";
            const string POST_INSERTION = "Inserted Post #{0}";

            IEnumerable<FacebookPost> posts = fbRepository.GetPostsInFeed(opts);

            foreach (FacebookPost fbPost in posts)
            {
                if (fbPost.Link == null || fbPost.Type != FacebookPostType.Link) // only links.
                {
                    continue;
                }
                Link link = linkRepository.GetByReferenceUri(fbPost.Link);
				if (link != null)
				{
					log.Debug(LINK_EXISTS.FormatWith(link.ReferenceUri));
					if (link.PostId.HasValue) // if link exists and it has a related post, ignore it.
					{
						continue;
					}
				}
				else
				{
					link = mapper.Map<FacebookPost, Link>(fbPost);
					linkRepository.Insert(link);
					log.Debug(LINK_INSERTION.FormatWith(link.Id));
				}
	            Post post = mapper.Map<FacebookPost, Post>(fbPost);
                User user = userRepository.GetByFacebookGraphId(post.FacebookUserId);
				
                post.LinkId = link.Id;
                post.UserId = user == null ? (long?)null : user.Id;
                postRepository.Insert(post);
                log.Debug(POST_INSERTION.FormatWith(post.Id));

                insertCount++;
            }

            opts.Log.InsertCount = insertCount;
        }
    }
}
