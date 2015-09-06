using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.SelfHost;
using WebPaparazzi.Model;

namespace WebPaparazzi
{
    public class ImageStreamingServer2
    {
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private PaparazziResolution _DefaultResolution;

        private HttpSelfHostServer _Server;
        private Thread _Thread_Render;
        private byte[] _CurrentImage;
        private long _CurrentImageEnd;
        private ReaderWriterLock _ImageLock = new ReaderWriterLock();

        public ImageStreamingServer2(IEnumerable<ImageWithTime> imagesSource)
        {
            this.ImagesSource = imagesSource;
        }

        /// <summary>
        /// Gets or sets the source of images that will be streamed to the 
        /// any connected client.
        /// </summary>
        public IEnumerable<ImageWithTime> ImagesSource { get; set; }

        public void Start(int port, PaparazziResolution resolution)
        {
            lock (this)
            {
                _DefaultResolution = resolution;

                var builder = new ContainerBuilder();
                builder.RegisterInstance<ImageStreamingServer2>(this);
                builder.RegisterApiControllers(typeof(ImageStreamingServer2).Assembly);
                var container = builder.Build();
                var dependencyResolver = new AutofacWebApiDependencyResolver(container);
                
                var config = new HttpSelfHostConfiguration("http://localhost:" + port)
                {
                    TransferMode = TransferMode.Streamed
                };
                config.DependencyResolver = dependencyResolver;
                config.Routes.MapHttpRoute(
                    "ApiDefault",
                    "{controller}/{resolution}",
                    new { resolution = RouteParameter.Optional });
                config.ReceiveTimeout = TimeSpan.FromHours(1);
                config.SendTimeout = TimeSpan.FromHours(1);
                _Server = new HttpSelfHostServer(config);
                _Server.OpenAsync().Wait();
                Trace.TraceInformation("server is opened");


                _Thread_Render = new Thread(new ParameterizedThreadStart(RenderThread));
                _Thread_Render.Name = "RenderThread";
                _Thread_Render.IsBackground = true;
                _Thread_Render.Start();
            }
        }

        private void RenderThread(object state)
        {
            // while there's an image...
            foreach (var img in this.ImagesSource)
            {
                _ImageLock.AcquireWriterLock(img.TimeInMilliseconds);
                try
                {
                    if (img.Image == null)
                    {
                        Trace.WriteLine("No screenshot!");
                    }
                    else
                    {
                        _CurrentImage = MjpegWriter.BytesOf(img.Image).ToArray();
                        _CurrentImageEnd = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds) + img.TimeInMilliseconds;
                    }
                }
                finally
                {
                    _ImageLock.ReleaseWriterLock();
                }
                Thread.Sleep(img.TimeInMilliseconds);
            }
        }

        public void Stop()
        {
            _Server.CloseAsync().Wait();
        }

        public long GetCurrentImageEndTime()
        {
            _ImageLock.AcquireReaderLock(100);
            try
            {
                return _CurrentImageEnd;
            }
            finally
            {
                _ImageLock.ReleaseReaderLock();
            }
        }

        public MemoryStream GetCurrentImage(Size outputSize)
        {
            MemoryStream img = null;
            _ImageLock.AcquireReaderLock(100);
            try
            {
                if (_CurrentImage != null)
                {
                    var originalImage = new Bitmap(new MemoryStream(_CurrentImage));
                    Bitmap resizedImage = null;
                    if (outputSize != Size.Empty)
                    {
                        resizedImage = new Bitmap(originalImage, outputSize);
                    }
                    else
                    {
                        resizedImage = originalImage;
                    }

                    img = new MemoryStream(MjpegWriter.BytesOf(resizedImage).ToArray());
                }
                else
                {
                    img = new MemoryStream(new byte[0]);
                }
            }
            finally
            {
                _ImageLock.ReleaseReaderLock();
            }

            return img;
        }
    }

}
