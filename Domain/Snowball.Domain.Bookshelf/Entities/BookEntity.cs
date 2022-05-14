using System;

namespace Snowball.Domain.Bookshelf.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Name { get; set; }

        public string OriginalName { get; set; }

        public string Author { get; set; }

        public string Translator { get; set; }

        public BookCategory Category { get; set; }

        public float DoubanScore { get; set; }

        public string DownloadUrl { get; set; }

        public string ExtractionCode { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDel { get; set; }
    }
}