using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Tider.Models
{
    public class Category {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image_url { get; set; }
        public DateTime Date { get; set; }
        public string OpId { get; set; }           // Linked1
        public virtual ApplicationUser Op { get; set; }     // Linked1
        public virtual ICollection<Post> Posts { get; set; }
    }
    public class Post {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image_url { get; set; }
        public DateTime Date { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser OP { get; set; }
    }

    


    //public class Comment
    //{
    //    public int ID { get; set; }
    //    public int Op_id { get; set; } // User connection
    //    public string Content { get; set; }
    //    public DateTime Date { get; set; }
    //    public virtual Post Post { get; set; } // Posts ->>> Comments
    //}

    // ----------------------------------

    //public class View
    //{
    //    public int ID { get; set; }
    //    public int User_id { get; set; } // User connection, TODO: may be replaced with virtual User
    //    public DateTime Date { get; set; }
    //    //public int Post_id { get; set; } // Post connection
    //    public virtual Post Post { get; set; }
    //}

    //public class Post
    //{
    //    public int ID { get; set; }
    //    public int Op_id { get; set; } // User connection
    //    public string Title { get; set; } 
    //    public string Content { get; set; }
    //    public string Image_url { get; set; }
    //    public DateTime Date { get; set; }
    //    public virtual Category Category { get; set; } // Category ->>> Post
    //    public virtual ICollection<Comment> Comments { get; set; } // Comment -> Post
    //    public virtual ICollection<View> Views { get; set; } // View ->>> Post
    //}
}