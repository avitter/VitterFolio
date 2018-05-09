using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vitter.Core;
using Vitter.Core.Logging;
using VitterFolio.DataServices.Models;

namespace VitterFolio.DataServices
{
    public class AssetDB
    {
        private readonly VitterFolioContext _context;

        public AssetDB(VitterFolioContext context)
        {
            _context = context;
        }

        public AssetDB() {
         
        }

        [ExceptionHandler(AspectPriority = 1)]
        //[Logger(AspectPriority = 2, OnEntryMessage="VitterFolio.DataServices.GetAssets() Enter", OnExitMessage = "VitterFolio.DataServices.GetAssets() Exit")]
        public Response<IEnumerable<Asset>> GetAssets()
        {
            var response = new Response<IEnumerable<Asset>>
            {
                Data = _context.Asset,
                Status = ResponseStatus.Success
            };

            return response;
        }

        [ExceptionHandler(AspectPriority = 1)]
        public Response<Asset> GetAsset(int id)
        {
            var response = new Response<Asset>()
            {
                Data = _context.Asset.FirstOrDefault<Asset>(m => m.AssetId == id),
                Status = ResponseStatus.Success
            };

            return response;
        }

        [ExceptionHandler(AspectPriority = 1)]
        public Response<bool> AssetExists(int id)
        {
            var response = new Response<bool>()
            {
                Data = _context.Asset.Any(e => e.AssetId == id),
                Status = ResponseStatus.Success
            };
            return response;
        }

        [ExceptionHandler(AspectPriority = 1)]
        public Response<Asset> DeleteAsset(int id)
        {
            var response = new Response<Asset>()
            {
                Data = _context.Asset.SingleOrDefault(m => m.AssetId == id),
                Status = ResponseStatus.Success
            };
            
            _context.Asset.Remove(response.Data);
            _context.SaveChanges();

            return response;
        }

        [ExceptionHandler(AspectPriority = 1)]
        public Response<Asset> UpdateAsset(Asset Asset)
        {
            var response = new Response<Asset>()
            {
                Data = Asset,
                Status = ResponseStatus.Success
            };

            _context.Entry(Asset).State = EntityState.Modified;
            _context.SaveChanges();
           
            return response;
        }

        [ExceptionHandler(AspectPriority = 1)]
        public Response<Asset> AddAsset(Asset Asset)
        {
            _context.Asset.Add(Asset);
            _context.SaveChanges();

            var response = new Response<Asset>()
            {
                Data = Asset, 
                Status = ResponseStatus.Success
            };

            return response;
        }
    }
}
