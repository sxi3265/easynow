using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyNow.AutoJsServer
{
    public class AutoJsHandler:WebSocketHandler
    {
        private readonly ConcurrentDictionary<string,ClientInfo> _socketClientInfoDic=new ConcurrentDictionary<string, ClientInfo>();

        private static readonly Regex AndroidRegex=new Regex(@"D\:\sAndroidId\:(?<androidId>[^;]*)");
        private readonly ILifetimeScope _scope;
        public AutoJsHandler(ConnectionManager webSocketConnectionManager, ILifetimeScope scope) : base(webSocketConnectionManager)
        {
            _scope = scope;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var msg = JsonConvert.DeserializeObject<AutoJsMsg>(Encoding.UTF8.GetString(buffer, 0, result.Count));
            switch (msg.Type)
            {
                case "hello":
                    _socketClientInfoDic.TryAdd(this.WebSocketConnectionManager.GetId(socket), msg.Data.ToJson().FromJson<ClientInfo>());
                    await this.SendMessageAsync(socket, JsonConvert.SerializeObject(new AutoJsMsg
                    {
                        Type = "hello",
                        Data = new Dictionary<string, object>
                        {
                            {"server_version",2}
                        }
                    }));
                    // 发送获取AndroidId的命令
                    await this.SendMessageAsync(socket, new AutoJsMsg
                    {
                        Type = "command",
                        Data=new Dictionary<string, object>
                        {
                            {"command","run"},
                            {"id",Guid.NewGuid().ToString("N")},
                            {"name","test"},
                            {"script","console.log('AndroidId:'+device.getAndroidId()+';');"}
                        }
                    }.ToJson());
                    break;
                case "log":
                    var log = msg.Data["log"].ToString();
                    var socketId = this.WebSocketConnectionManager.GetId(socket);
                    if (_socketClientInfoDic.ContainsKey(socketId)&&_socketClientInfoDic.TryRemove(socketId,out var clientInfo))
                    {
                        var match = AndroidRegex.Match(log);
                        if (match.Success)
                        {
                            var androidId = match.Groups["androidId"].Value;
                            await _scope.Resolve<IDeviceBo>().AddOrUpdateAsync(new DeviceDto
                            {
                                Name = clientInfo.DeviceName,
                                Ip = _scope.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                                Status = DeviceStatus.Online,
                                LastOnlineTime = DateTime.UtcNow,
                                SocketId = socketId,
                                Uuid = androidId
                            });
                        }
                    }
                    Console.WriteLine(log);
                    break;
            }
        }
    }
}