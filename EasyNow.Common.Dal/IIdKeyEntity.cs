using System;

namespace EasyNow.Common.Dal
{
    public interface IIdKeyEntity : IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Id { get; set; }
    }
}