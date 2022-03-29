﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSL.Forum.Base;

namespace OSL.Forum.Core.Entities
{
    [Table("FavoriteForums")]
    public class FavoriteForum : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("Forum")]
        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
