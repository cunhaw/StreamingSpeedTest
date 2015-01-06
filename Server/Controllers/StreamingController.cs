using Common.Utils;
using Server.Streaming;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [RoutePrefix("StreamingSpeedTest")]
    public class StreamingController : ApiController
    {
        [HttpGet, Route("GetUsingPushStreamContent")]
        public HttpResponseMessage GetUsingPushStreamContent()
        {
            HttpResponseMessage vResponse;

            try
            {
                Streamer vStreamer = new Streamer();

                vResponse = Request.CreateResponse();
                vResponse.Content = new PushStreamContent(vStreamer.StreamWriter);
            }
            catch (Exception ex)
            {
                vResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                vResponse.ReasonPhrase = ex.Message;
            }

            return vResponse;
        }

        [HttpGet, Route("GetUsingStreamContent")]
        public HttpResponseMessage GetUsingStreamContent()
        {
            HttpResponseMessage vResponse;

            try
            {
                FileStream vTestFileStream = File.Open(PathResolver.ServerTestFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                vResponse = Request.CreateResponse();
                vResponse.Content = new StreamContent(vTestFileStream);
            }
            catch (Exception ex)
            {
                vResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                vResponse.ReasonPhrase = ex.Message;
            }

            return vResponse;
        }    
    }
}
