using System;
using System.ServiceModel;

namespace EasyNow.GraphDetection.Service.Abstractions
{
    /// <summary>
    /// 验证码服务
    /// </summary>
    [ServiceContract(Name = "GraphDetection.VerifyCodeService")]
    public interface IVerifyCodeService
    {
    }
}
