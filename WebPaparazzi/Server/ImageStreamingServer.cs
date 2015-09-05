using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;

// -------------------------------------------------
// Developed By : Ragheed Al-Tayeb
// e-Mail       : ragheedemail@gmail.com
// Date         : April 2012
// -------------------------------------------------

namespace WebPaparazzi
{

    /// <summary>
    /// Provides a streaming server that can be used to stream any images source
    /// to any client.
    /// </summary>
    public class ImageStreamingServer:IDisposable
    {
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private List<Socket> _Clients;
        private Socket _Server;
        private Thread _Thread;
        private Thread _Thread_Render;
        private byte[] _CurrentImage;
        private long _CurrentImageEnd;

        private ReaderWriterLock _ImageLock = new ReaderWriterLock();

        public ImageStreamingServer():this(Screen.Snapshots(600,450,true, 3))
        {

        }

        public ImageStreamingServer(IEnumerable<ImageWithTime> imagesSource)
        {

            _Clients = new List<Socket>();
            _Thread = null;
            _Thread_Render = null;

            this.ImagesSource = imagesSource;

        }


        /// <summary>
        /// Gets or sets the source of images that will be streamed to the 
        /// any connected client.
        /// </summary>
        public IEnumerable<ImageWithTime> ImagesSource { get; set; }

        /// <summary>
        /// Gets a collection of client sockets.
        /// </summary>
        public IEnumerable<Socket> Clients { get { return _Clients; } }

        /// <summary>
        /// Returns the status of the server. True means the server is currently 
        /// running and ready to serve any client requests.
        /// </summary>
        public bool IsRunning { get { return (_Thread != null && _Thread.IsAlive); } }

        /// <summary>
        /// Starts the server to accepts any new connections on the specified port.
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {

            lock (this)
            {
                _Thread = new Thread(new ParameterizedThreadStart(ServerThread));
                _Thread.Name = "MjpegServerThread";
                _Thread.IsBackground = true;
                _Thread.Start(port);


                _Thread_Render = new Thread(new ParameterizedThreadStart(RenderThread));
                _Thread_Render.Name = "RenderThread";
                _Thread_Render.IsBackground = true;
                _Thread_Render.Start();
            }

        }

        /// <summary>
        /// Starts the server to accepts any new connections on the default port (8080).
        /// </summary>
        public void Start()
        {
            this.Start(8080);
        }

        public void Stop()
        {

            if (this.IsRunning)
            {
                try
                {
                    _Server.Close();
                    _Thread.Abort();
                    _Thread_Render.Abort();
                }
                finally
                {

                    lock (_Clients)
                    {
                        
                        foreach (var s in _Clients)
                        {
                            try
                            {
                                s.Close();
                            }
                            catch { }
                        }
                        _Clients.Clear();

                    }

                    _Thread = null;
                    _Thread_Render = null;
                }
            }
        }

        /// <summary>
        /// This the main thread of the server that serves all the new 
        /// connections from clients.
        /// </summary>
        /// <param name="state"></param>
        private void ServerThread(object state)
        {

            try
            {
                _Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _Server.Bind(new IPEndPoint(IPAddress.Any, (int)state));
                _Server.Listen(10);

                System.Diagnostics.Debug.WriteLine(string.Format("Server started on port {0}.", state));

                foreach (Socket client in _Server.IncommingConnections())
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), client);
                }                    
            
            }
            catch { }

            this.Stop();
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
                        System.Diagnostics.Debug.WriteLine("No screenshot!");
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

        /// <summary>
        /// Each client connection will be served by this thread.
        /// </summary>
        /// <param name="client"></param>
        private void ClientThread(object client)
        {            
            Socket socket = (Socket)client;
            IPEndPoint remoteIpEndPoint = socket.RemoteEndPoint as IPEndPoint;
            
            
            System.Diagnostics.Debug.WriteLine(string.Format("New client from {0}",socket.RemoteEndPoint.ToString()));

            lock (_Clients)
                _Clients.Add(socket);

            try
            {
                if (!socket.Connected)
                {
                    throw new Exception("Socket disconnected from "+remoteIpEndPoint.Address);
                }

                using (MjpegWriter wr = new MjpegWriter(new NetworkStream(socket, true)))
                {

                    // Writes the response header to the client.
                    wr.WriteHeader();

                    long beginTime = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);
                    long endTime = 0;
                    MemoryStream img = null;

                    //Boolean isNew = false;
                    while(true)
                    {
                        //isNew = true;
                        _ImageLock.AcquireReaderLock(100);
                        try
                        {
                            if (_CurrentImage != null)
                            {
                                img = new MemoryStream(_CurrentImage);
                            }
                            else
                            {
                                img = new MemoryStream(new byte[0]);
                            }
                            endTime = _CurrentImageEnd;
                        }
                        finally
                        {
                            _ImageLock.ReleaseReaderLock();
                        }

                        do
                        {
                            // wait for ~60fps
                            //Thread.Sleep(17);
                            // wait for 25fps
                            Thread.Sleep(40);

                            if (!socket.Connected)
                            {
                                throw new Exception("Socket disconnected from " + remoteIpEndPoint.Address);
                            }

                            //Thread.Sleep(1000);
                            //if (isNew)
                            //{
                                img.Seek(0, SeekOrigin.Begin);
                                wr.Write(img);
                                //isNew = false;
                            //}                            

                        } while (Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds) < endTime);
                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            finally
            {
                lock (_Clients)
                    _Clients.Remove(socket);
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            this.Stop();
        }

        #endregion
    }

    static class SocketExtensions
    {

        public static IEnumerable<Socket> IncommingConnections(this Socket server)
        {
            while(true)
            {
                Socket s = server.Accept();
                yield return s;
            }                
        }

    }


    static class Screen
    {


        public static IEnumerable<ImageWithTime> Snapshots()
        {
            return Screen.Snapshots(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,true, 3);
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public static IEnumerable<ImageWithTime> Snapshots(int width,int height,bool showCursor, int frequency)
        {
            Size size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            
            Bitmap srcImage = new Bitmap(size.Width, size.Height);
            Graphics srcGraphics = Graphics.FromImage(srcImage);

            bool scaled = (width != size.Width || height != size.Height);

            Bitmap dstImage = srcImage;
            Graphics dstGraphics = srcGraphics;

            if(scaled)
            {
                dstImage = new Bitmap(width, height);
                dstGraphics = Graphics.FromImage(dstImage);
            }

            Rectangle src = new Rectangle(0, 0, size.Width, size.Height);
            Rectangle dst = new Rectangle(0, 0, width, height);
            Size curSize = new Size(32, 32);

            while (true)
            {
                srcGraphics.CopyFromScreen(0, 0, 0, 0, size);

                if (showCursor)
                    Cursors.Default.Draw(srcGraphics,new Rectangle(Cursor.Position,curSize));

                if (scaled)
                    dstGraphics.DrawImage(srcImage, dst, src, GraphicsUnit.Pixel);

                yield return new ImageWithTime { Image = dstImage, TimeInMilliseconds = frequency };

            }

            srcGraphics.Dispose();
            dstGraphics.Dispose();

            srcImage.Dispose();
            dstImage.Dispose();

            yield break;
        }

        internal static IEnumerable<MemoryStream> Streams(this IEnumerable<Image> source)
        {
            MemoryStream ms = new MemoryStream();

            foreach (var img in source)
            {
                ms.SetLength(0);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                yield return ms;
            }

            ms.Close();
            ms = null;

            yield break;
        }

        internal static MemoryStream Stream(this Image source)
        {
            MemoryStream ms = new MemoryStream();

            ms.SetLength(0);
            source.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms;
        }

    }
}
