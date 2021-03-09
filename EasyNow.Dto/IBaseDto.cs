using System;

namespace EasyNow.Dto
{
    /// <summary>
    /// 默认Dto接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseDto<TKey> : IDto where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        TKey Id { get; set; }
    }
}