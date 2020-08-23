using System.Threading.Tasks;
using EasyNow.Dto.WxPusher;

namespace EasyNow.Bo.Abstractions
{
    public interface IWxBo
    {
        /// <summary>
        /// 更新wxpusher用户订阅
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task WxPusherUserSubscribeAsync(string appKey, UserDto user);
    }
}