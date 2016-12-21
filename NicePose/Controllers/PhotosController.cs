
using System;
using System.Net;

using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Xml.Serialization;
using NicePose.Models;
using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Linq;
using NicePose.Models.SupportModel;

namespace NicePose.Controllers
{
    public class mPhotos {
       public String photoURL { get; set; }
        public String photoName { get; set; }
        public int usage { get; set; }
    }
 
    public class PhotosController : iController
    {

        String IMGUR_ANONYMOUS_API_KEY = "f615f2976080959";
        private object file;

        public object Cursor { get; private set; }
        public object MessageBox { get; private set; }

        // GET: Photos
        public ActionResult Index()
        {
            PostToImgur(@"C:\Users\NhatVHN\Desktop\New folder - Copy\hahaha.jpg", IMGUR_ANONYMOUS_API_KEY);
            return View();
        }
        public String getImagesOfCate(int cateID) {
            Category cate = dbContext.Categories.Find(cateID);
            List<mPhotos> lst = new List<mPhotos>();
            foreach (TemplatePicture pic in cate.TemplatePictures) {
                mPhotos mphoto = new mPhotos();
                mphoto.photoName = pic.name;
                mphoto.photoURL = pic.link;
                mphoto.usage = pic.usage;
                lst.Add(mphoto);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(lst);

        }
        public ActionResult imageCate() {
            ViewData["catelist"] = dbContext.Categories.ToList();
            return View("AllCate");
        }
        public ActionResult CateDetail(int id)
        {
            ViewData["CateDetail"] = dbContext.Categories.Find(id);
            return View("CateDetail");
        }

        public ActionResult UploadTemplate(int cateID, HttpPostedFileBase newpic) {
           
                System.Diagnostics.Debug.WriteLine("I was called ?? ");
                string contentType = "image/jpeg"; ;
                CookieContainer cookie = new CookieContainer();
                NameValueCollection par = new NameValueCollection();
                par["MAX_FILE_SIZE"] = "3145728";
                par["refer"] = "";
                par["brand"] = "";
                par["key"] = "01567DQW77d6d472ef877a4bf1d5b3ff8caebaa1";
                par["optimage"] = "1";
                par["rembar"] = "1";
                par["submit"] = "host it!";
                List<String> l = new List<String>();
                string resp;
                par["optsize"] = "resample";
                resp = mUploadPicture(newpic, "http://www.imageshack.us/upload_api.php", "fileupload", contentType, par, cookie);
                if (resp != null) {
                    TemplatePicture pic = new TemplatePicture();
                    pic.link = resp;
                    pic.name = newpic.FileName;
                    pic.usage = 0;
                    pic.createdDate = DateTime.Now;
                    pic.cateID = cateID;
                    dbContext.TemplatePictures.Add(pic);
                    dbContext.SaveChanges();
                }

            
          
            return RedirectToAction("CateDetail", new { id = cateID });
        }
        public ActionResult UploadManyTemplate(int cateID, IEnumerable<HttpPostedFileBase> files)
        {
            foreach (HttpPostedFileBase newpic in files) {
               string contentType = "image/jpeg"; ;
                CookieContainer cookie = new CookieContainer();
                NameValueCollection par = new NameValueCollection();
                par["MAX_FILE_SIZE"] = "3145728";
                par["refer"] = "";
                par["brand"] = "";
                par["key"] = "01567DQW77d6d472ef877a4bf1d5b3ff8caebaa1";
                par["optimage"] = "1";
                par["rembar"] = "1";
                par["submit"] = "host it!";
                List<String> l = new List<String>();
                string resp;
                par["optsize"] = "resample";
                resp = mUploadPicture(newpic, "http://www.imageshack.us/upload_api.php", "fileupload", contentType, par, cookie);
                if (resp != null)
                {
                    TemplatePicture pic = new TemplatePicture();
                    pic.link = resp;
                    pic.name = newpic.FileName;
                    pic.usage = 0;
                    pic.createdDate = DateTime.Now;
                    pic.cateID = cateID;
                    dbContext.TemplatePictures.Add(pic);
                    dbContext.SaveChanges();
                }
            }
            



            return RedirectToAction("CateDetail", new { id = cateID });
        }
        public static void PostToImgur(string imagFilePath, string apiKey)
        {
          
            byte[] imageData;

            FileStream fileStream = System.IO.File.OpenRead(imagFilePath);
            imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);
            fileStream.Close();

            string uploadRequestString = "image=" + Uri.EscapeDataString(System.Convert.ToBase64String(imageData)) + "&key=" + apiKey;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://api.imgur.com/2/upload");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();
        }
        public static void PostToImgurFromAndroid(string imagFilePath, string apiKey, String base64StringImage)
        {

     

            string uploadRequestString = "image=" + Uri.EscapeDataString(base64StringImage) + "&key=" + apiKey;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://api.imgur.com/2/upload");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();
        }

        public string Upload(string fileName)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("I was called ?? ");
                string contentType = "image/jpeg"; ;
                CookieContainer cookie = new CookieContainer();
                NameValueCollection par = new NameValueCollection();
                par["MAX_FILE_SIZE"] = "3145728";
                par["refer"] = "";
                par["brand"] = "";
                par["key"] = "01567DQW77d6d472ef877a4bf1d5b3ff8caebaa1";
                par["optimage"] = "1";
                par["rembar"] = "1";
                par["submit"] = "host it!";
                List<String> l = new List<String>();
                string resp;
                par["optsize"] = "resample";
                resp = UploadFileEx(@"C:\Users\NhatVHN\Desktop\model.jpg", "http://www.imageshack.us/upload_api.php", "fileupload", contentType, par, cookie);

                return resp;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string UploadFromAndroid(string fileName,String userEmail,String userName,String Templat)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("I was called ?? ");
                string contentType = "image/jpeg"; ;
                CookieContainer cookie = new CookieContainer();
                NameValueCollection par = new NameValueCollection();
                par["MAX_FILE_SIZE"] = "3145728";
                par["refer"] = "";
                par["brand"] = "";
                par["key"] = "01567DQW77d6d472ef877a4bf1d5b3ff8caebaa1";
                par["optimage"] = "1";
                par["rembar"] = "1";
                par["submit"] = "host it!";
                List<String> l = new List<String>();
                string resp;
                par["optsize"] = "resample";
                resp = UploadFileEx(@"C:\Users\NhatVHN\Desktop\model.jpg", "http://www.imageshack.us/upload_api.php", "fileupload", contentType, par, cookie);
                NiceposeUser user;
                user = dbContext.NiceposeUsers.Where(u => u.email.Equals(userEmail)).FirstOrDefault();
                if (user == null) {
                    user = new NiceposeUser();
                    user.createDate = DateTime.Today;
                    user.email = userEmail;
                    user.name = userName;
                    user.isActive = true;
                    dbContext.NiceposeUsers.Add(user);
                    dbContext.SaveChanges();
                }
                if (resp != null && resp.Length > 5) {
                    UserPicture ip = new UserPicture();
                    ip.isActive = true;
                    ip.Point = 0;
                    ip.TotalRated = 0;
                    ip.templateLink = Templat;
                    ip.userID = user.id;
                    ip.uploadedDate = DateTime.Today;
                    ip.url = resp;
                    dbContext.UserPictures.Add(ip);
                    dbContext.SaveChanges();
                }
                return resp;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string UploadFileEx(string uploadfile, string url, string fileFormName, string contenttype, NameValueCollection querystring, CookieContainer cookies)
        {
            if ((fileFormName == null) ||
              (fileFormName.Length == 0))
            {
                fileFormName = "fileupload";
            }

            if ((contenttype == null) ||
              (contenttype.Length == 0))
            {
                contenttype = "application/octet-stream";
            }


            string postdata;
            postdata = "?";
            if (querystring != null)
            {
                foreach (string key in querystring.Keys)
                {
                    postdata += key + "=" + querystring.Get(key) + "&";
                }
            }
            Uri uri = new Uri(url + postdata);


            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";


            // Build up the post message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(uploadfile));
            System.Diagnostics.Debug.WriteLine("cAi path la ne " + Path.GetFileName(uploadfile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes =
                Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = new FileStream(uploadfile,
                          FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length +
                                boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // Write out our post header
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents
            byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                         (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            XmlDocument doc = new XmlDocument();
            doc.Load(s);
            XmlNodeList imgLink = doc.GetElementsByTagName("image_link");
            System.Diagnostics.Debug.WriteLine("cAi doc la ne " + doc.ToString() );
      
            fileStream.Close();
            s.Close();
            
            return imgLink[0].InnerText;


        }
        private string mUploadPicture(HttpPostedFileBase file , string url, string fileFormName, string contenttype, NameValueCollection querystring, CookieContainer cookies)
        {
            if ((fileFormName == null) ||
              (fileFormName.Length == 0))
            {
                fileFormName = "fileupload";
            }

            if ((contenttype == null) ||
              (contenttype.Length == 0))
            {
                contenttype = "application/octet-stream";
            }


            string postdata;
            postdata = "?";
            if (querystring != null)
            {
                foreach (string key in querystring.Keys)
                {
                    postdata += key + "=" + querystring.Get(key) + "&";
                }
            }
            Uri uri = new Uri(url + postdata);


            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";


            // Build up the post message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(file.FileName));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.Append("\r\n");
            sb.Append("\r\n");
            
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes =
                Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
           
      
            long length = postHeaderBytes.Length + file.InputStream.Length +
                                boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();
            // Write out our post header
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents
            byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                         (int)file.InputStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = file.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            XmlDocument doc = new XmlDocument();
            doc.Load(s);
            XmlNodeList imgLink = doc.GetElementsByTagName("image_link");
            System.Diagnostics.Debug.WriteLine("cAi doc la ne " + doc.ToString());

            file.InputStream.Close();
            s.Close();

            return imgLink[0].InnerText;


        }

        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ns.imageshack.us/imginfo/7/", IsNullable = false)]
        public partial class imginfo
        {

            private imginfoRating ratingField;

            private imginfoFiles filesField;

            private imginfoResolution resolutionField;

            private string classField;

            private string visibilityField;

            private imginfoUploader uploaderField;

            private imginfoLinks linksField;

            private byte versionField;

            private uint timestampField;

            /// <remarks/>
            public imginfoRating rating
            {
                get
                {
                    return this.ratingField;
                }
                set
                {
                    this.ratingField = value;
                }
            }

            /// <remarks/>
            public imginfoFiles files
            {
                get
                {
                    return this.filesField;
                }
                set
                {
                    this.filesField = value;
                }
            }

            /// <remarks/>
            public imginfoResolution resolution
            {
                get
                {
                    return this.resolutionField;
                }
                set
                {
                    this.resolutionField = value;
                }
            }

            /// <remarks/>
            public string @class
            {
                get
                {
                    return this.classField;
                }
                set
                {
                    this.classField = value;
                }
            }

            /// <remarks/>
            public string visibility
            {
                get
                {
                    return this.visibilityField;
                }
                set
                {
                    this.visibilityField = value;
                }
            }

            /// <remarks/>
            public imginfoUploader uploader
            {
                get
                {
                    return this.uploaderField;
                }
                set
                {
                    this.uploaderField = value;
                }
            }

            /// <remarks/>
            public imginfoLinks links
            {
                get
                {
                    return this.linksField;
                }
                set
                {
                    this.linksField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint timestamp
            {
                get
                {
                    return this.timestampField;
                }
                set
                {
                    this.timestampField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoRating
        {

            private byte ratingsField;

            private decimal avgField;

            /// <remarks/>
            public byte ratings
            {
                get
                {
                    return this.ratingsField;
                }
                set
                {
                    this.ratingsField = value;
                }
            }

            /// <remarks/>
            public decimal avg
            {
                get
                {
                    return this.avgField;
                }
                set
                {
                    this.avgField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoFiles
        {

            private imginfoFilesImage imageField;

            private imginfoFilesThumb thumbField;

            private ushort serverField;

            private ushort bucketField;

            /// <remarks/>
            public imginfoFilesImage image
            {
                get
                {
                    return this.imageField;
                }
                set
                {
                    this.imageField = value;
                }
            }

            /// <remarks/>
            public imginfoFilesThumb thumb
            {
                get
                {
                    return this.thumbField;
                }
                set
                {
                    this.thumbField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort server
            {
                get
                {
                    return this.serverField;
                }
                set
                {
                    this.serverField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort bucket
            {
                get
                {
                    return this.bucketField;
                }
                set
                {
                    this.bucketField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoFilesImage
        {

            private ushort sizeField;

            private string contenttypeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort size
            {
                get
                {
                    return this.sizeField;
                }
                set
                {
                    this.sizeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute("content-type")]
            public string contenttype
            {
                get
                {
                    return this.contenttypeField;
                }
                set
                {
                    this.contenttypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoFilesThumb
        {

            private ushort sizeField;

            private string contenttypeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort size
            {
                get
                {
                    return this.sizeField;
                }
                set
                {
                    this.sizeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute("content-type")]
            public string contenttype
            {
                get
                {
                    return this.contenttypeField;
                }
                set
                {
                    this.contenttypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoResolution
        {

            private ushort widthField;

            private ushort heightField;

            /// <remarks/>
            public ushort width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public ushort height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoUploader
        {

            private string ipField;

            /// <remarks/>
            public string ip
            {
                get
                {
                    return this.ipField;
                }
                set
                {
                    this.ipField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ns.imageshack.us/imginfo/7/")]
        public partial class imginfoLinks
        {

            private string image_linkField;

            private string image_htmlField;

            private string image_bbField;

            private string image_bb2Field;

            private string thumb_linkField;

            private string thumb_htmlField;

            private string thumb_bbField;

            private string thumb_bb2Field;

            private string yfrog_linkField;

            private string yfrog_thumbField;

            private string ad_linkField;

            private string done_pageField;

            /// <remarks/>
            public string image_link
            {
                get
                {
                    return this.image_linkField;
                }
                set
                {
                    this.image_linkField = value;
                }
            }

            /// <remarks/>
            public string image_html
            {
                get
                {
                    return this.image_htmlField;
                }
                set
                {
                    this.image_htmlField = value;
                }
            }

            /// <remarks/>
            public string image_bb
            {
                get
                {
                    return this.image_bbField;
                }
                set
                {
                    this.image_bbField = value;
                }
            }

            /// <remarks/>
            public string image_bb2
            {
                get
                {
                    return this.image_bb2Field;
                }
                set
                {
                    this.image_bb2Field = value;
                }
            }

            /// <remarks/>
            public string thumb_link
            {
                get
                {
                    return this.thumb_linkField;
                }
                set
                {
                    this.thumb_linkField = value;
                }
            }

            /// <remarks/>
            public string thumb_html
            {
                get
                {
                    return this.thumb_htmlField;
                }
                set
                {
                    this.thumb_htmlField = value;
                }
            }

            /// <remarks/>
            public string thumb_bb
            {
                get
                {
                    return this.thumb_bbField;
                }
                set
                {
                    this.thumb_bbField = value;
                }
            }

            /// <remarks/>
            public string thumb_bb2
            {
                get
                {
                    return this.thumb_bb2Field;
                }
                set
                {
                    this.thumb_bb2Field = value;
                }
            }

            /// <remarks/>
            public string yfrog_link
            {
                get
                {
                    return this.yfrog_linkField;
                }
                set
                {
                    this.yfrog_linkField = value;
                }
            }

            /// <remarks/>
            public string yfrog_thumb
            {
                get
                {
                    return this.yfrog_thumbField;
                }
                set
                {
                    this.yfrog_thumbField = value;
                }
            }

            /// <remarks/>
            public string ad_link
            {
                get
                {
                    return this.ad_linkField;
                }
                set
                {
                    this.ad_linkField = value;
                }
            }

            /// <remarks/>
            public string done_page
            {
                get
                {
                    return this.done_pageField;
                }
                set
                {
                    this.done_pageField = value;
                }
            }
        }
    }
}