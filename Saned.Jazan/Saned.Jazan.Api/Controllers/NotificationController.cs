using Microsoft.AspNet.Identity;
using Saned.Common.Notification.Model;
using Saned.Common.Notification.Repository;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Core;
using Saned.Jazan.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Notification")]
    public class NotificationController : BasicController
    {

        private INotificationsRepositoryAsync INotificationsRepositoryAsync;
        OneSignalLibrary.OneSignalClient client;
        public NotificationController()
        {
            INotificationsRepositoryAsync = new NotificationRepositoryAsync("Saned_Jazan");
            client = new OneSignalLibrary.OneSignalClient(System.Configuration.ConfigurationManager.AppSettings["appKey"],
                                                          System.Configuration.ConfigurationManager.AppSettings["resetKey"],
                                                          System.Configuration.ConfigurationManager.AppSettings["userAuth"]);
        }

        [Route("SelectNotificationByUserId/{userId}/{page?}/{pageSize?}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> SelectNotificationByUserId(string userId, int page = 1, int pageSize = 10)
        {
            try
            {
                
                var result = await INotificationsRepositoryAsync
                    .GetNotificationOfUser(userId, page, pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AddDevice/{deviceId}")]
        public async Task<IHttpActionResult> AddDevice(string deviceId)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                if (u != null)
                {
                    var userDevices = await INotificationsRepositoryAsync.GetDevicesBasedOnUserId(u.Id);
                    if (!userDevices.Any(x => x.DeviceId == deviceId))
                    {
                        await INotificationsRepositoryAsync.AddDevice(new Devices()
                        {
                            DeviceId = deviceId,
                            UserId = u.Id,
                        });
                    }
                   

                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SendNotification")]
        public async Task<IHttpActionResult> SendNotification(Notifications Notification)
        {
            try
            {
                var Devices = await INotificationsRepositoryAsync.GetDevices();
                var DevicesList = Devices.ToList().Select(x => x.DeviceId);
                var RecepientDevices = new OneSignalLibrary.Posting.Device(new HashSet<string>(DevicesList));

                Dictionary<string, string> NotificationCOntent = new Dictionary<string, string>();
                NotificationCOntent.Add("ar", Notification.ArabicMessage);
                NotificationCOntent.Add("en", Notification.EnglishMessage);
                var content = new OneSignalLibrary.Posting.ContentAndLanguage(NotificationCOntent);

                Notification.NotificationLogs =
                                (from device in Devices
                                 select new NotificationLog()
                                 {
                                     isSeen = false,
                                     RecieverId = device.UserId
                                 }).ToList();

                await INotificationsRepositoryAsync.AddNotification(Notification);

                try
                {
                    client.SendNotification(RecepientDevices, content, null, null);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
