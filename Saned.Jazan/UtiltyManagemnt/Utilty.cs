using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
//using Subgurim.Controles;
using System.Net.Mail;
using System.Diagnostics;
using System.Drawing;


namespace UtiltyManagemnt
{
    /// <summary>
    /// Summary description for UtiltyManagemnt
    /// </summary>
    public class Utilty
    {
       
        #region Public Functions
      

        public static string UploadMyIMG(FileUpload myupload, string oldIMG, string folderName)
        {
            string imgName = oldIMG;
            if (myupload.HasFile)
            {
                string ext = System.IO.Path.GetExtension(myupload.PostedFile.FileName);
                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".bmp" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpe" || ext.ToLower() == ".tif")
                {
                    if (myupload.PostedFile.ContentLength > 1048576 * double.Parse(ConfigurationManager.AppSettings["MaxImageSizeInMB"])) //(2 * 1048576)) // max 2MG file
                    {
                        return "";
                    }

                    #region DELETE...

                    if (!string.IsNullOrEmpty(oldIMG))
                    {
                        try
                        {
                            FileInfo mytha = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(folderName + "Thumbnails/" + oldIMG));
                            mytha.Delete();

                            FileInfo mypic = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(folderName + oldIMG));
                            mypic.Delete();
                        }
                        catch { }
                    }

                    #endregion

                    imgName = Guid.NewGuid().ToString().Substring(0, 10) + myupload.PostedFile.FileName.Remove(0, myupload.PostedFile.FileName.LastIndexOf("."));

                    myupload.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(folderName) + imgName);
                    ThumbnailUtilities.CreateThumbnail(HttpContext.Current.Server.MapPath(folderName) + imgName);

                }
                else
                {
                    return "";
                }
            }
            return imgName;
        }

        /// <summary>
        /// Check Image extension.
        /// </summary>
        /// <param name="imageName">String image file name.</param>
        /// <returns>bool if extension is valid, false otherwise.</returns>
        public static bool ChechImageExtension(string imageName)
        {
            //Get file extension.
            string fileExtension = Path.GetExtension(imageName);
            //Check file extension.
            switch (fileExtension.ToLower())
            {
                case ".gif":
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".tif":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check Image Size
        /// </summary>
        /// <param name="imgSize">Int image size</param>
        /// <returns></returns>
        public static bool CheckImageSize(int imgSize)
        {
            //if (imgSize > ApplicationConfiguration.MaxImgSize * 1024)
            //    return false;
            //else
            return true;
        }

        public static void TakeVideoSnapShoot(string videoName)
        {
            Process ffmpeg; // creating process
            string video;
            string thumb;
            video = HttpContext.Current.Server.MapPath("~/PlayerVideos/" + videoName);// setting video input name with path
            System.IO.FileInfo file = new System.IO.FileInfo(video);
            string imageName = file.Name.Replace(file.Extension, ".jpg");

            thumb = HttpContext.Current.Server.MapPath("~/PlayerVideos/images") + "/" + imageName; // thumb name with path !
            ffmpeg = new Process();

            ffmpeg.StartInfo.Arguments = " -i \"" + video + "\" -s 140*140  -vframes 5 -f image2 -t   0.02 -vcodec mjpeg \"" + thumb + "\""; // arguments !
            ffmpeg.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/PlayerVideos/ffmpeg/ffmpeg.exe");
            ffmpeg.Start(); // start !
        }

        public static void TakeFanVideoSnapShoot(string videoName)
        {
            string video;
            string thumb;
            video = HttpContext.Current.Server.MapPath("~/FanVideos/" + videoName);// setting video input name with path
            System.IO.FileInfo file = new System.IO.FileInfo(video);
            string imageName = file.Name.Replace(file.Extension, ".jpg");

            thumb = HttpContext.Current.Server.MapPath("~/FanVideos/images") + "/" + imageName; // thumb name with path !
            //ffmpeg = new Process();

            //ffmpeg.StartInfo.Arguments = " -i \"" + video + "\" -s 140*140  -vframes 5 -f image2 -t   0.02 -vcodec mjpeg \"" + thumb + "\""; // arguments !
            //ffmpeg.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/FanVideos/ffmpeg/ffmpeg.exe");
            //ffmpeg.Start(); // start !
        }

        public static string SaveFile(FileUpload fileUpload, string oldFileName, string folderUrl)
        {
            if (!fileUpload.HasFile)
                return "";
            string fileName = oldFileName;
            HttpPostedFile file = fileUpload.PostedFile;

            #region Delete
            if (!string.IsNullOrEmpty(oldFileName))
            {
                try
                {
                    FileInfo mypic = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(folderUrl + "/" + fileName));
                    mypic.Delete();
                }
                catch { }
            }
            #endregion

            fileName = Guid.NewGuid().ToString().Substring(0, 10) + file.FileName.Remove(0, file.FileName.LastIndexOf("."));
            fileUpload.SaveAs(HttpContext.Current.Server.MapPath(folderUrl) + "/" + fileName);
            return fileName;
        }

        public static string EditFirstLetter(string text)
        {
            if (text.Length > 0)
            {
                string x = text.Substring(0, 1);
                if (x == "Ã" || x == "Å" || x == "Â")
                    text = text.Replace(x, "Ç");
            }
            return text;
        }
        #endregion

        #region Convert
        public static byte ConvertToByte(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;

            try { return Convert.ToByte(obj); }
            catch { return 0; }
        }

        public static short ConvertToInt16(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;

            try { return Convert.ToInt16(obj); }
            catch { return 0; }
        }

        public static int ConvertToInt32(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;

            try { return Convert.ToInt32(obj); }
            catch { return 0; }
        }

        public static long ConvertToInt64(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;

            try { return Convert.ToInt64(obj); }
            catch { return 0; }
        }

        public static double ConvertToDouble(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return 0;

            try { return Convert.ToDouble(obj); }
            catch { return 0; }
        }

        public static bool ConvertToBoolean(object obj)
        {
            if (obj == null || obj.ToString() == "")
                return false;

            try { return Convert.ToBoolean(obj); }
            catch { return false; }
        }

        public static DateTime ConvertToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        #endregion

        #region Send Mail
        /// <summary>
        /// Generic method for sending emails.
        /// </summary>
        /// <param name="from">String from mail address.</param>
        /// <param name="to">String to mail address.</param>
        /// <param name="subject">String mail Subject.</param>
        /// <param name="body">String mail body.</param>
        public static string SendMail(string host, string from, string password, string to, string subject, string body, string bbc)
        {
            // Configure mail client (may need additional
            // code for authenticated SMTP servers)


            SmtpClient mailClient = new SmtpClient(host);
            mailClient.EnableSsl = false;// changed for // Server does not support secure connections.
            mailClient.Port = 25;
            mailClient.Credentials = new System.Net.NetworkCredential(from, password);
            // Create the mail message
            MailMessage mailMessage = new MailMessage(from, to, subject, body);

            //if (bbc.Length > 0)
            //    mailMessage.Bcc.Add(bbc);
            mailMessage.IsBodyHtml = true;

            try
            {
                mailClient.Send(mailMessage);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
                // throw ex;
            }

        }
        #endregion

        public static bool ThumbnailCallback()
        {
            return false;
        }
        public static string SaveImageFileAndReturnItsName(HttpPostedFile myFile, string sSavePath, string sThumbPath, int intThumbWidth, int intThumbHeight)
        {
            // Initialize variables
            string sFilename = string.Empty;


            // If file field isn’t empty
            if (myFile != null)
            {
                // Check file size (mustn’t be 0)


                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                {
                    //Span1.InnerHtml = "ÇÎÊÑ ÕæÑÉ";
                    return string.Empty;
                }

                // Check file extension (must be image)


                //if (!myFile.ContentType.Contains("image"))
                //{
                //    //Span1.InnerHtml = "ãáÝ ÎÇØÆ ÎÊÑ ÕæÑÉ";
                //    return string.Empty;
                //}

                // Read file into a data stream
                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);

                // Make sure a duplicate file doesn’t exist.  If it does, keep on appending an incremental numeric until it is unique
                sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + ".jpg";
                int file_append = 0;
                while (System.IO.File.Exists(HttpContext.Current.Server.MapPath(sSavePath + sFilename)))
                {
                    file_append++;
                    sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + ".jpg";
                }

                // Save the stream to disk
                System.IO.FileStream newFile = new System.IO.FileStream(HttpContext.Current.Server.MapPath(sSavePath + sFilename), System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                // Check whether the file is really a JPEG by opening it
                System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                Bitmap myBitmap;
                try
                {

                    System.IO.MemoryStream MS = new System.IO.MemoryStream(myData);
                    myBitmap = (Bitmap)Bitmap.FromStream(MS);
                    //myBitmap = new Bitmap(Server.MapPath(sSavePath + sFilename));

                    // If jpg file is a jpeg, create a thumbnail filename that is unique.
                    file_append = 0;

                    // Save thumbnail and output it onto the webpage
                    System.Drawing.Image myThumbnail = myBitmap.GetThumbnailImage(intThumbWidth, intThumbHeight, null, IntPtr.Zero);
                    myThumbnail.Save(HttpContext.Current.Server.MapPath(sThumbPath + sFilename));




                    // Destroy objects
                    myThumbnail.Dispose();
                    myBitmap.Dispose();
                }
                catch (ArgumentException)
                {
                    // The file wasn't a valid jpg file
                    //Span1.InnerHtml = "ÇÎÊÑ ÕæÑÉ";
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(sSavePath + sFilename));
                    return string.Empty;
                }
            }
            return sFilename;
        }
        public static bool DeleteImage(string ImageFileName, string sSavePath, string sThumbPath)
        {
            bool result = false;
            try
            {
                //path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                //string sSavePath = "~/PlayersImages/";
                // = "~/PlayersImages/Thumbnails/";
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(sSavePath + ImageFileName));
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(sThumbPath + ImageFileName));
                result = true;
            }
            catch (Exception) { }
            return result;
        }
        public static bool ValidateImage(FileUpload fileUpload, ref Literal LiteralErrorMessage)
        {
            if (!fileUpload.HasFile)
            {
                return false;
            }

            double length = Math.Ceiling((double)fileUpload.FileBytes.LongLength / (1000));
            if (length > 1024)
            {
                LiteralErrorMessage.Text = "Your image must be less than or equal 1 Megabytes";
                LiteralErrorMessage.Visible = true;
                return false;
            }
            return true;
        }
        public static bool ChechFileType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".gif":
                    return true;
                case ".png":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }


        //public  List<Item> GetItems(string tableName, string idField, string valueField, string Filter = "")
        //{
        //    DataAccessLayer.DataBaseUtil db = new DataAccessLayer.DataBaseUtil();
        //    DataTable dt = new DataTable();
        //    db.Fill(dt, "SELECT " + idField + "," + valueField + " FROM " + tableName + (string.IsNullOrEmpty(Filter) ? "" : " WHERE " + Filter));
        //    List<Item> items = new List<Item>();
        //    items = dt.AsEnumerable().Select(obj => new Item()
        //    {
        //        Id = obj[idField],
        //        Name = obj.Field<string>(valueField)
        //    }).ToList();
        //    return items;
        //}

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        //#region Settings Methods
        //public  string GetThemesURL()
        //{
        //    try
        //    {
        //        #region DataBaseAccessObject
        //        DataAccessLayer.DataBaseUtil _dataBaseUtil = new DataAccessLayer.DataBaseUtil();
        //        Parameters _parameters = new Parameters();
        //        #endregion

        //        _parameters = new Parameters();
        //        _parameters.Add("@SettingName", "ThemesURL");
        //        return _dataBaseUtil.ExecuteScaller(" SELECT SettingValue " +
        //                                            " FROM Settings " +
        //                                            " WHERE (SettingName = @SettingName)", _parameters).ToString();
        //    }
        //    catch
        //    {
        //        return String.Empty;
        //    }



        //}
        //public  string GetSettingValue(string settingName)
        //{
        //    try
        //    {
        //        #region DataBaseAccessObject
        //        DataAccessLayer.DataBaseUtil _dataBaseUtil = new DataAccessLayer.DataBaseUtil();
        //        Parameters _parameters = new Parameters();
        //        #endregion

        //        _parameters = new Parameters();
        //        _parameters.Add("@SettingName", settingName);
        //        return _dataBaseUtil.ExecuteScaller(" SELECT SettingValue " +
        //                                            " FROM Settings " +
        //                                            " WHERE (SettingName = @SettingName)", _parameters).ToString();
        //    }
        //    catch
        //    {
        //        return String.Empty;
        //    }



        //}
        //#endregion

    }
}
