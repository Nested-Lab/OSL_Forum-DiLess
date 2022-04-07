﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OSL.Forum.Core.Entities;
using OSL.Forum.Core.Services;
using OSL.Forum.Core.Utilities;
using OSL.Forum.Membership.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Category
{
    public class CategoryDetailsModel : BaseModel
    {
        public BO.Category Category { get; set; }
        public IList<BO.Forum> Forums { get; set; }
        public IList<string> Roles { get; set; }
        public Pager Pager { get; set; }
        private ILifetimeScope _scope;
        private ICategoryService _categoryService;
        private IForumService _forumService;
        private IMapper _mapper;
        private IProfileService _profileService;

        public CategoryDetailsModel()
        {
        }

        public CategoryDetailsModel(ICategoryService categoryService,
            IMapper mapper, IForumService forumService, IProfileService profileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _forumService = forumService;
            _profileService = profileService;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();
            _forumService = _scope.Resolve<IForumService>();
            _profileService = _scope.Resolve<IProfileService>();

            await base.ResolveAsync(_scope);
        }

        public void GetCategory(Guid categoryId)
        {
            Category = _categoryService.GetCategory(categoryId);
        }

        public void GetForums(Guid categoryId, int? page)
        {
            var totalItem = _forumService.GetForumCount(categoryId);

            Pager = new Pager(totalItem, page);
            Forums = _forumService.GetForums(Pager.CurrentPage, Pager.PageSize, categoryId);
        }

        public async Task LoadUserInfo()
        {
            Roles = await _profileService.UserRolesAsync();
        }
    }
}