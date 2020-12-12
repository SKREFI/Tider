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
}