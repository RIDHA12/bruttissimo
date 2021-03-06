﻿using System;
using System.Collections.Generic;

namespace Bruttissimo.Domain.Entity.Entities
{
    public class Post
    {
        public long Id { get; set; }

        public long? UserId { get; set; }
        public User User { get; set; }

        public long? LinkId { get; set; }
        public Link Link { get; set; }

        public string UserMessage { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        // Facebook specific properties.
        public string FacebookFeedId { get; set; }
        public string FacebookPostId { get; set; }
        public string FacebookUserId { get; set; }

        public string TwitterPostId { get; set; }
        public string TwitterUserId { get; set; }

        public IEnumerable<Comment> Comments { get; set; } // lazily loaded by request.
    }
}
