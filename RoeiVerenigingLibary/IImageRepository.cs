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
        public List<Stream> get(int id);
    }
}
