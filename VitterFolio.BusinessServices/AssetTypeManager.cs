using System;
using System.Collections.Generic;
using System.Text;
using Vitter.Core;
using VitterFolio.DataServices;
using VitterFolio.DataServices.Models;

namespace VitterFolio.BusinessServices
{
    public class AssetManager
    {
        private AssetDB _AssetDB;

        public AssetManager(AssetDB AssetDB)
        {
            _AssetDB = AssetDB;
        }

        public Response<IEnumerable<Asset>> GetAssets()
        {
            var response = _AssetDB.GetAssets();

            if(response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred retrieving asset types.";
            }

            return response;
        }

        public Response<Asset> GetAsset(int id)
        {
            var response = _AssetDB.GetAsset(id);

            if (response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred retrieving asset type.";
            }

            return response;
        }

        public Response<bool> AssetExists(int id)
        {
            var response = _AssetDB.AssetExists(id);

            if (response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred checking if asset type exists.";
            }

            return response;
        }

        public Response<Asset> DeleteAsset(int id)
        {
            var response = _AssetDB.DeleteAsset(id);

            if (response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred deleting asset type.";
            }

            return response;
        }

        public Response<Asset> UpdateAsset(Asset Asset)
        {
            var response =  _AssetDB.UpdateAsset(Asset);

            if (response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred updating asset type.";
            }

            return response;
        }

        public Response<Asset> AddAsset(Asset Asset)
        {
            var response = _AssetDB.AddAsset(Asset);

            if (response.Status == Vitter.Core.ResponseStatus.Error)
            {
                response.Message = "Error occurred adding asset type.";
            }

            return response;
        }
    }
}
