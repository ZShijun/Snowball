using Snowball.Bookshelf.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Bookshelf.Dtos
{
    public class BookDto
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
