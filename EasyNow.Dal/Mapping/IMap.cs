using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public interface IMap
    {
        void Map(ModelBuilder builder);
    }
}