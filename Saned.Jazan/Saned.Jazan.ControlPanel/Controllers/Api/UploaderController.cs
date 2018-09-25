using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.ControlPanel.Controllers.Api
{
    public class UploaderController : ApiController
    {
        readonly IAdvertisementRepository _repo;
        private IAdvertisementImageRepository IAdvertisementImageRepository;

        public UploaderController()
        {
            this._repo =new AdvertisementRepository() ;
            this.IAdvertisementImageRepository =new  AdvertisementImageRepository();

        }


        [HttpPost]
        public IHttpActionResult SaveFile(string id)
        {
            string _Path = "";
            string path = "";

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    Guid imgeId = Guid.NewGuid();
                    var postedFile = httpRequest.Files[file];
                    string filename = imgeId + Path.GetExtension(postedFile.FileName);
                    _Path = "uploads/" + imgeId + Path.GetExtension(filename);
                    path = HttpContext.Current.Server.MapPath("~/uploads/") + filename;
                    postedFile.SaveAs(path);
                    docfiles.Add(path);
                    string st = ThumbnailUtilities.CreateThumbnail(path);
                }

            }

            else
                return Ok(_Path);

            return Ok(_Path);
        }


        [HttpPost]
        
        public IHttpActionResult SaveAdvertisementFiles(string id)
        {
            string _Path = "";
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        Guid imgeId = Guid.NewGuid();
                        var postedFile = httpRequest.Files[file];
                        string filename = imgeId + Path.GetExtension(postedFile.FileName);
                        _Path += "uploads/" + imgeId + Path.GetExtension(filename) + ",";
                        var path = HttpContext.Current.Server.MapPath("~/uploads/") + filename;
                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                        string st = ThumbnailUtilities.CreateThumbnail(path);
                        //save image in database
                        SaveAdvertisementImage(filename, id);

                    }
                    return Ok(_Path);
                }

                else
                    return BadRequest("no files");
            }
            catch (Exception e)
            {
                return BadRequest("no files");
            }




        }

        private void SaveAdvertisementImage(string path, string id)
        {
            var advertisement = _repo.SelectAdId(int.Parse(id));
            if (advertisement == null)
                return;
            advertisement.AdvertisementImages.Add(new AdvertisementImage() { ImageUrl = "#Back#"+path });
            _repo.Update(advertisement);




        }


        [HttpPost]

        public IHttpActionResult DeleteAdvertisementImage([FromBody] dynamic key)
        {

            //string key= "1";
            var advertisement = _repo.SelectAdId(int.Parse(key));
            if (advertisement == null)
                return BadRequest();



            AdvertisementImage image = IAdvertisementImageRepository.Find(key);
            if (image != null)
            {
                DeleteImage(image.ImageUrl);
                IAdvertisementImageRepository.Delete(image);
            }


            return Ok();
        }


        [HttpPost]

        public IHttpActionResult DeleteAdvertisementImage([FromBody] string key)
        {
            return Ok();
        }

        [HttpPost]

        public IHttpActionResult DeleteAdvertisementImagesHttpActionResult(dynamic key)
        {
            return Ok();
        }

        [HttpPost]

        public HttpResponseMessage DeleteImg([FromBody] objdata key)
        {
            return Request.CreateResponse();
        }



        private void DeleteImage(string imageUrl)
        {
            string slogn = "/Uploads/";
            slogn += imageUrl;
            string filePath = (HostingEnvironment.MapPath($"~{slogn}"));
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                    throw;
                }

            }

        }




    }

    public class objdata
    {
        public string Key { get; set; }
    }
}