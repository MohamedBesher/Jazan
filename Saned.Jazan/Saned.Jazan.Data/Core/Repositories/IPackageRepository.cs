using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IPackageRepository : IBaseRepository<Package>, IDisposable
    {
        Task<List<Advertisements_Packages_SelectAll_Result>> SelectAllPackages();

        Task<List<PackageFeaturesDto>> SelectById(int id);

        Task<Package> UpdatePackage(Package package);

        Task<bool> DeletePackage(int id);

        Task<int> InsertPackage(Package package);

        Task<List<PackageDto>> SelectPagedPackages(PackageParamterDto packageParamterDto);
        List<Package> GetAll();



        Package SelectByIdwithFeatures(int id);

       Package SelectPackageById(int id);


    }
}
