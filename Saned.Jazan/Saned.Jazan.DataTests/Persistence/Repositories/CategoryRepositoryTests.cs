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
    public class CategoryRepositoryTests
    {
        [TestMethod()]
        public void IsCategoryExistReallyExistTest()
        {
            using (var repo = new CategoryRepository())
            {
                bool result = repo.IsCategoryExist("Fist").Result;
                Assert.IsTrue(result);
            }
        }

        [TestMethod()]
        public void IsCategoryExistReallyExistWithOwnerCategoryTest()
        {
            using (var repo = new CategoryRepository())
            {
                bool result = repo.IsCategoryExist("Fist", 2).Result;
                Assert.IsTrue(result);
            }
        }

        [TestMethod()]
        public void IsCategoryExistNotExistTest()
        {
            using (var repo = new CategoryRepository())
            {
                bool result = repo.IsCategoryExist("NewCategory").Result;
                Assert.IsFalse(result);
            }
        }

        [TestMethod()]
        public void GetParentCategoriesTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.GetParentCategories(new Core.Dtos.CategoriesRequest { }).Result;
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod()]
        public void GetCategoryChieldsTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.GetCategoryChields(1, new Core.Dtos.CategoriesRequest { }).Result;
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod()]
        public void RemoveCategoryTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.RemoveCategory(2).Result;
                Assert.IsTrue(result > 0);
            }
        }

        [TestMethod()]
        public void SingleTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.Single(1).Result;
                Assert.IsTrue(result != null);
            }
        }

        [TestMethod()]
        public void UpdateCategoryTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.UpdateCategory(new Core.Dtos.UpdateCategoryDto { CategoryId = 1, Name = "First Is Updated", Status = 0 }).Result;
                Assert.IsTrue(result > 0);
            }
        }

        [TestMethod()]
        public void AddCategoryTest()
        {
            using (var repo = new CategoryRepository())
            {
                var result = repo.AddCategory(new Core.Dtos.AddCategoryDto
                {
                    CategoryImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAQAAABLCVATAAACH0lEQVR42q3WoZKrMBQGYGRkIpHEoY9DMrh1nUGtzxPcGV7gCsTaK3iBCqa2ipmrVqLrWrmytjL3nBwoEGD30ja/6JaSj/wp3SEIXjpUoB+Oeg0zpoR+NsyoDVOgi39cbYHAy4MQTc0wOYZepxRBUkn9UxxEiNnXxyYwd6w/438hSddHJilv1tqv664Shle1DeJaJihPV9uNQ+NWBRK2QVSr+GjtaFzOIpdjKFShnoY+Gv0N0u0OVLexY48NQ+68JchdpQu/o1piVMu6faJdwjNWIAYyl55bqGUtbndO53TzCIpUpCkdlEm+V3J3Ir8r3uops2+FkTmvx832IGJwN97xS/5Ti0LQ/WLwtbxMal2ueAwvc2c8CAgSJip5U4+tKHECMlUzq2UcA9EyROuJi6/71dtzWAfVcq0Jw1CsYh13kDDteVoirE+zWtLVinQ8ZAS5YlVlvRHWfi3pakUQL0OOwmp/W/vN6Gt5zBIkzEezxnCtMJsxDIECTYmhp3bej4HHzaalNMyAnzE0UBKp6Z1Do2pwd3JkAH6CxlTs/bZOZ661yMwhohDLQqREMWz8UAvWoUQleggehG5dSPUbv28GJlnKHGJsqPi7vuG/MGTyCGslOtkCOayrGOa/indajdudb6FUpXoepgiLHIIMriddyzrkMBhGAqlOH4U2hKCT2j0NdU8jFbzpZ3LQlh9srPqEQ1Y9lEP2CVa99KHvH8mnrGGdl9V9AAAAAElFTkSuQmCC",
                    ImageExtention = "png",
                    LanguageId = 1,
                    Name = "New Category",
                    ParentId = 0
                }).Result;
                Assert.IsTrue(result > 0);
            }
        }
    }
}