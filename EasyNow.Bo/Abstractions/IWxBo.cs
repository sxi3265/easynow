using System.Threading.Tasks;

namespace EasyNow.Bo.Abstractions
{
    public interface IWxBo
    {
        /// <summary>
        /// 更新wxpusher用户订阅
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task WxPusherUserSubscribeAsync(string appKey,string uid);
    }
}