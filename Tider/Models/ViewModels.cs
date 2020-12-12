using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tider.Models {
    public class ProfilePageViewModel {
        public ApplicationUser User { get; set; }
        public ICollection<Post> Posts { get; set; }
        public int PostsCount { get; set; }
    }

    public class PostsViewModel {
        public bool IsAdmin { get; set; }
        public bool IsMod { get; set; }
        public bool IsUser { get; set; }
        public string UserImage { get; set; }
        public ICollection<Post> Posts { get; set; }
        public int PostsCount { get; set; }
        public int? CategoryID { get; set; }
    }

    public class ProfilesTableViewModel {
        public ICollection<Tuple<ApplicationUser, string>> PostRoleTuples { get; set; }
    }
}