namespace RoeiVerenigingLibrary
{
    public interface IImageRepository
    {
        public void Add(int id, List<Stream> images);
        public Stream GetFirstImage(int id);
        public List<Stream> Get(int id);
    }
}