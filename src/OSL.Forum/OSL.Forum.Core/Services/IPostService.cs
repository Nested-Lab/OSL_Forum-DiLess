﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;

namespace OSL.Forum.Core.Services
{
    public interface IPostService
    {
        BO.Post GetPost(Guid postId);
        void EditPost(BO.Post post);
        void DeletePost(Guid postId);
        void CreatePost(BO.Post post);
        List<BO.Post> PendingPosts(int pagerCurrentPage, int pagerPageSize);
        void UpdatePostStatus(Guid postId, string status);
        int UserPostCount(string applicationUserId);
        List<BO.Post> GetMyPosts(int pagerCurrentPage, int pagerPageSize, string applicationUserId);
        int GetPendingPostCount();
    }
}
