using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public interface IImageRepository
    {
        public void Add(int id, Stream image);
        public void Add(int id, List<Stream> images);
        public List<Stream> getAsync(int id);
        public Stream GetFirstImage(int id);
        public List<Stream> get(int id);
    }
}
