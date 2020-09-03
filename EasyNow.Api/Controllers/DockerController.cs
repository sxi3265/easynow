using EasyNow.Dto.Docker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class DockerController : ApiBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// {
        ///   "events": [
        ///     {
        ///       "id": "428e2f4e-79e9-41ce-a4eb-fc9056e8c89c",
        ///       "timestamp": "2020-02-16T20:05:44.9312292+08:00",
        ///       "action": "push",
        ///       "target": {
        ///         "mediaType": "application/vnd.docker.distribution.manifest.v2+json",
        ///         "size": 1792,
        ///         "digest": "sha256:1f991105479c40b307aeff32b5bf76633143010e10ce09b824a7797e2134f65d",
        ///         "length": 1792,
        ///         "repository": "test",
        ///         "url": "http://localhost:5000/v2/test/manifests/sha256:1f991105479c40b307aeff32b5bf76633143010e10ce09b824a7797e2134f65d",
        ///         "tag": "Web"
        ///       },
        ///       "request": {
        ///         "id": "c9b982ae-5aa9-4bf2-a1a4-0402281e7689",
        ///         "addr": "172.17.0.1:46162",
        ///         "host": "localhost:5000",
        ///         "method": "PUT",
        ///         "useragent": "docker/19.03.5 go/go1.12.12 git-commit/633a0ea kernel/4.19.76-linuxkit os/linux arch/amd64 UpstreamClient(Docker-Client/19.03.5 \\(darwin\\))"
        ///       },
        ///       "actor": {},
        ///       "source": {
        ///         "addr": "d2542f5aa94a:5000",
        ///         "instanceID": "5c225f92-e19a-4102-ad39-301f19557acb"
        ///       }
        ///     }
        ///   ]
        /// }
        /// ]]>
        /// </example>
        /// <param name="req"></param>
        [HttpPost,AllowAnonymous]
        public void RegistryWebhookReq(RegistryRequest req)
        {
            foreach (var @event in req.Events)
            {
                if (@event.Target.MediaType == "application/vnd.docker.distribution.manifest.v2+json")
                {
                    // todo ´ýÊµÏÖ
                }
            }
        }
    }
}