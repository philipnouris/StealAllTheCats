namespace StealAllTheCats.Dto
{
    public class CatApiResponse
    {
        public string id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public List<Breed> breeds { get; set; }
    }
}
