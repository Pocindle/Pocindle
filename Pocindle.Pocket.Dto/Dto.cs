using System;


namespace Pocindle.Pocket.Dto
{
    public record Dto
    {
        public string ItemId { get; set; }
        public string ResolvedId { get; set; }
        public string GivenUrl { get; set; }
        public string ResolvedUrl { get; set; }
        public string? AmpUrl { get; set; }
        public string GivenTitle { get; set; }
        public string ResolvedTitle { get; set; }
        public bool Favorite { get; set; }
        public string Status { get; set; }
        public string Excerpt { get; set; }
        public bool IsArticle { get; set; }
        public int WordCount { get; set; }
        public int ListenDurationEstimate { get; set; }
        public int? TimeToRead { get; set; }
        public DateTimeOffset TimeAdded { get; set; }
        public DateTimeOffset TimeUpdated { get; set; }
    }
}
