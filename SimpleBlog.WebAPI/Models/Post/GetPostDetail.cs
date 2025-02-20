﻿using SimpleBlog.WebAPI.Models.Tag;

namespace SimpleBlog.WebAPI.Models.Post
{
    public class GetPostDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public List<GetTag> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
    }
}
