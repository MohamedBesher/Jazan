using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Saned.Jazan.Data.Persistence.Repositories.Tests
{
    [TestClass()]
    public class NewsRepositoryTests
    {
        [TestMethod()]
        public void AddNewsTest()
        {
            using (var repo = new NewsRepository())
            { 
                int c = repo.AddNews(new Core.Dtos.InsertNewsDto { DefaultImageIndex = 0,
                    Description = "zkcnmksdncsdnkcvsd", ImageExtentions = new List<string> { "png" },
                    Title = "sdfsdfsdf", ImagesBase64s = new List<string>
                    { "iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAQAAABLCVATAAACH0lEQVR42q3WoZKrMBQGYGRkIpHEoY9DMrh1nUGtzxPcGV7gCsTaK3iBCqa2ipmrVqLrWrmytjL3nBwoEGD30ja/6JaSj/wp3SEIXjpUoB+Oeg0zpoR+NsyoDVOgi39cbYHAy4MQTc0wOYZepxRBUkn9UxxEiNnXxyYwd6w/438hSddHJilv1tqv664Shle1DeJaJihPV9uNQ+NWBRK2QVSr+GjtaFzOIpdjKFShnoY+Gv0N0u0OVLexY48NQ+68JchdpQu/o1piVMu6faJdwjNWIAYyl55bqGUtbndO53TzCIpUpCkdlEm+V3J3Ir8r3uops2+FkTmvx832IGJwN97xS/5Ti0LQ/WLwtbxMal2ueAwvc2c8CAgSJip5U4+tKHECMlUzq2UcA9EyROuJi6/71dtzWAfVcq0Jw1CsYh13kDDteVoirE+zWtLVinQ8ZAS5YlVlvRHWfi3pakUQL0OOwmp/W/vN6Gt5zBIkzEezxnCtMJsxDIECTYmhp3bej4HHzaalNMyAnzE0UBKp6Z1Do2pwd3JkAH6CxlTs/bZOZ661yMwhohDLQqREMWz8UAvWoUQleggehG5dSPUbv28GJlnKHGJsqPi7vuG/MGTyCGslOtkCOayrGOa/indajdudb6FUpXoepgiLHIIMriddyzrkMBhGAqlOH4U2hKCT2j0NdU8jFbzpZ3LQlh9srPqEQ1Y9lEP2CVa99KHvH8mnrGGdl9V9AAAAAElFTkSuQmCC" } }).Result;
                Assert.IsTrue(c > 0);
            }
        }

        [TestMethod()]
        public void GetNewsImagesTest()
        {
            using (var repo = new NewsRepository())
            {
                int c = repo.GetNewsImages(1).Result.Count;
                Assert.IsTrue(c > 0);
            }
        }


        [TestMethod()]
        public void GetNewsListTest()
        {
            using (var repo = new NewsRepository())
            {
                int c = repo.GetNewsList(new Core.Dtos.NewsRequestDto { }).Result.Count;
                Assert.IsTrue(c > 0);
            }
        }

        [TestMethod()]
        public void GetSingleNewsTest()
        {
            using (var repo = new NewsRepository())
            {
                var dd = repo.GetSingleNews(1).Result;
                Assert.IsTrue(dd != null);
            }
        }

        [TestMethod()]
        public void RemoveNewsTest()
        {
            using (var repo = new NewsRepository())
            {
                int c = repo.RemoveNews(2).Result;
                Assert.IsTrue(c > 0);
            }
        }

        [TestMethod()]
        public void SetImageAsDefaultTest()
        {
            using (var repo = new NewsRepository())
            {
                int c = repo.SetImageAsDefault(1).Result;
                Assert.IsTrue(c > 0);
            }
        }

        [TestMethod()]
        public void UpdateNewsTest()
        {
            using (var repo = new NewsRepository())
            {
                int c = repo.UpdateNews(new Core.Dtos.UpdateNewsDto { NewsId = 1, ImagesToDelete = null, Description = " New Updated Description Of The News ", DefaultImageIndex = 0, NewImageExtentions = null, NewImagesBase64s = null,Title="New Updated Title ."}).Result;
                Assert.IsTrue(c > -1);
            }
        }
    }
}