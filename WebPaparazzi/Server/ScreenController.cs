using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace WebPaparazzi
{
    public class ScreenController : ApiController
    {
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly MultipartContent _content = new MultipartContent("x-mixed-replace");

        private ImageStreamingServer2 _Server;

        public ScreenController(ImageStreamingServer2 server)
        {
            _Server = server;
        }

        public HttpResponseMessage Get(String resolution)
        {
            Trace.TraceInformation("Get");
            //var content = new PushStreamContent(PushImage);
            var content = new PushStreamContent((stream, cont, ctx) =>
            {
                Trace.TraceInformation("pushing new frame");

                long beginTime = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);
                long endTime = 0;

                while (true)
                {
                    endTime = _Server.GetCurrentImageEndTime();
                    if(endTime == 0)
                    {
                        endTime = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds) + 3 * 1000;
                    }
                    MemoryStream img = _Server.GetCurrentImage(Size.Empty);

                    do
                    {
                        // wait for 25fps
                        Thread.Sleep(40);

                        img.Seek(0, SeekOrigin.Begin);
                        img.WriteTo(stream);
                        img.Flush();
                    
                    } while (Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds) < endTime);
                }

                //stream.Close();
            });
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/jpeg");
            _content.Add(content);
            return new HttpResponseMessage()
            {
                Content = _content
            };
        }

        /*private void PushImage(Stream stream, HttpContent cont, TransportContext ctx)
        {
            Trace.TraceInformation("pushing new frame");
            var content = new PushStreamContent(PushImage);
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/jpeg");
            _content.Add(content);
            MemoryStream img = _Server.GetCurrentImage(Size.Empty);
            img.WriteTo(stream);
            // wait for 25fps
            Thread.Sleep(40);
            stream.Close();
        }*/
    }
}
