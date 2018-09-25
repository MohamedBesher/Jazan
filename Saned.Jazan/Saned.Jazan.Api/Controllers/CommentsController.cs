using Microsoft.AspNet.Identity;
using Saned.Common.Comments.ComplexType;
using Saned.Common.Comments.Model;
using Saned.Common.Comments.Repository;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Comments")]
    public class CommentController : BasicController
    {
        private readonly ICommentRepositoryAsync ICommentRepositoryAsync;

        public CommentController()
        {
            ICommentRepositoryAsync = new CommentRepositoryAsync("Saned_Jazan");
        }

        [Route("GetComments")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllComments()
        {
            return Ok(await ICommentRepositoryAsync.GetAllComments());
        }

        [Authorize]
        [Route("SaveComment")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveComment([FromBody] Comment comment)
        {
            try
            {

                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                comment.UserId = u.Id;
                comment.CreatedDate = DateTime.Now;
                var r = await ICommentRepositoryAsync.AddComment(comment);
                Saned.Jazan.Api.Models.CommentViewModel CommentViewModel = new Saned.Jazan.Api.Models.CommentViewModel()
                {
                    CommentText = r.CommentText,
                    CommentTypeId = r.CommentTypeId,
                    CreatedDate = r.CreatedDate,
                    FullName = r.FullName,
                    Id = r.Id,
                    OverallCount = r.OverallCount,
                    ParentId = r.ParentId,
                    PhotoUrl = r.PhotoUrl,
                    RelatedId = r.RelatedId,
                    UpdatedDate = r.UpdatedDate,
                    UserId = r.UserId,
                    UserName = r.UserName,
                    CommentPeriod = CalculateSinceDuration(r.CreatedDate, DateTime.Now)
                };
                return Ok(CommentViewModel);
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("SaveComment --- " + msg);
            }
        }

        [Authorize]
        [Route("UpdateComment")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateComment([FromBody] Comment comment)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                comment.UserId = u.Id;
                comment.UpdatedDate = DateTime.Now;
                return Ok(await ICommentRepositoryAsync.UpdateComment(comment));
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("UpdateComment --- " + msg);
            }
        }

        [Authorize]
        [Route("GetCommentsByParentId/{parentId?}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCommentsByParentId(int? parentId = null)
        {
            try
            {
                return Ok(await ICommentRepositoryAsync.GetCommentByParentId(parentId));
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("GetCommentsByParentId --- " + msg);
            }
        }

        [Route("GetPagedComments/{pageIndex}/{pageSize}/{relatedId?}/{commentTypeId?}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPagedComments(int pageIndex, int pageSize, string relatedId = null, int? commentTypeId = null)
        {
            try
            {
                var pagedCommentsParam = new PagedCommentsParam()
                {
                    CommentTypeId = commentTypeId,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    RelatedId = relatedId
                };
                var result = await ICommentRepositoryAsync.GetPagedComments(pagedCommentsParam);

                List<Saned.Jazan.Api.Models.CommentViewModel> CommentViewModelList = (from r in result
                                                                                      select new Saned.Jazan.Api.Models.CommentViewModel
                                                                                      {
                                                                                          CommentText = r.CommentText,
                                                                                          CommentTypeId = r.CommentTypeId,
                                                                                          CreatedDate = r.CreatedDate,
                                                                                          FullName = r.FullName,
                                                                                          Id = r.Id,
                                                                                          OverallCount = r.OverallCount,
                                                                                          ParentId = r.ParentId,
                                                                                          PhotoUrl = r.PhotoUrl,
                                                                                          RelatedId = r.RelatedId,
                                                                                          UpdatedDate = r.UpdatedDate,
                                                                                          UserId = r.UserId,
                                                                                          UserName = r.UserName,
                                                                                          CommentPeriod = CalculateSinceDuration(r.CreatedDate, DateTime.Now)

                                                                                      }).ToList();
                return Ok(CommentViewModelList);
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("GetPagedComments --- " + msg);
            }
        }

        [Authorize]
        [Route("DeleteComment/{commentId}")]
        [HttpPost]
        public async Task<IHttpActionResult> Delete(int commentId)
        {
            try
            {
                await ICommentRepositoryAsync.DeleteComment(commentId);
                return Ok();
            }
            catch (Exception e)
            {
                string msg = e.GetaAllMessages();
                return BadRequest("Delete --- " + msg);
            }
        }

        private string CalculateSinceDuration(DateTime firstDate, DateTime secondDate)
        {
            // Convert both dates to milliseconds
            var date1_ms = firstDate.Millisecond;
            var date2_ms = secondDate.Millisecond;

            // Calculate the difference in milliseconds
            double difference_ms = secondDate.Subtract(firstDate).TotalMilliseconds;
            //take out milliseconds
            difference_ms = difference_ms / 1000;
            var seconds = Math.Floor(difference_ms % 60);
            difference_ms = difference_ms / 60;
            var minutes = Math.Floor(difference_ms % 60);
            difference_ms = difference_ms / 60;
            var hours = Math.Floor(difference_ms % 24);
            var days = Math.Floor(difference_ms / 24);
            var txtDuration = "";

            if (days > 30)
            {
                txtDuration = "منذ أكثر من شهر مضي .";
            }
            else
            {
                if (days > 0)
                {
                    txtDuration = days + " يوم " + hours + " ساعة " + minutes + " دقيقة ";
                }
                else
                {
                    if (hours > 0)
                    {
                        txtDuration = hours + " ساعة " + minutes + " دقيقة ";
                    }
                    else
                    {
                        if (minutes > 0)
                        {
                            txtDuration = minutes + " دقيقة " + seconds + " ث ";
                        }
                        else
                        {
                            txtDuration = "منذ قليل .";
                        }
                    }
                }
            }

            return txtDuration;
        }
    }
}