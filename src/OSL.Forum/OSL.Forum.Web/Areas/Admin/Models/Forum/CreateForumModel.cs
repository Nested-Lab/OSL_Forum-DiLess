﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using OSL.Forum.NHibernate.Core.Enums;
using OSL.Forum.NHibernate.Core.Services;
using OSL.Forum.NHibernate.Core.Utilities;
using OSL.Forum.Membership.Services;
using OSL.Forum.Web.Models;
using BO = OSL.Forum.NHibernate.Core.BusinessObjects;

namespace OSL.Forum.Web.Areas.Admin.Models.Forum
{
    public class CreateForumModel : BaseModel
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        [Display(Name = "Forum Name")]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public BO.Category BoCategory { get; set; }
        private ILifetimeScope _scope;
        private IProfileService _profileService;
        private IForumService _forumService;
        private IDateTimeUtility _dateTimeUtility;
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CreateForumModel()
        {
        }

        public CreateForumModel(ICategoryService categoryService,
            IProfileService profileService, IMapper mapper,
            IDateTimeUtility dateTimeUtility,
            IForumService forumService)
        {
            _profileService = profileService;
            _dateTimeUtility = dateTimeUtility;
            _forumService = forumService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public override async Task ResolveAsync(ILifetimeScope scope)
        {
            _scope = scope;
            _profileService = _scope.Resolve<IProfileService>();
            _dateTimeUtility = _scope.Resolve<IDateTimeUtility>();
            _forumService = _scope.Resolve<IForumService>();
            _categoryService = _scope.Resolve<ICategoryService>();
            _mapper = _scope.Resolve<IMapper>();

            await base.ResolveAsync(_scope);
        }

        public void GetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentNullException(nameof(categoryId));

            BoCategory = _categoryService.GetCategory(categoryId);

            if (BoCategory == null)
                throw new InvalidOperationException("Forum not found");
        }

        public void Create()
        {
            var user = _profileService.GetUser();

            if (user == null)
                throw new InvalidOperationException(nameof(user));

            var time = _dateTimeUtility.Now;
            var forum = new BO.Forum()
            {
                Name = this.Name,
                CategoryId = this.CategoryId,
                ApplicationUserId = user.Id,
                CreationDate = time,
                ModificationDate = time
            };

            _forumService.CreateForum(forum);
            _categoryService.UpdateModificationDate(time, forum.CategoryId);
        }
    }
}