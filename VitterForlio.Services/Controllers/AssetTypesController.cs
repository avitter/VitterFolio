using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vitter.Core;
using VitterFolio.BusinessServices;
using VitterFolio.DataServices.Models;

namespace VitterFolio.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AssetTypesController : Controller
    {
        private readonly AssetManager _assetManager;
        private readonly int _currentUser;

        public AssetTypesController(AssetManager assetManager, IHttpContextAccessor httpContextAccessor)
        {
            _currentUser = httpContextAccessor.CurrentUser();
            _assetManager = assetManager;
        }

        // GET: api/AssetTypes
        [HttpGet]
        public IActionResult GetAssetType()
        {
            return Ok(_assetManager.GetAssets());
        }

        // GET: api/AssetTypes/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetAssetType([FromRoute] int id)
        {
            // Test to pull claim from current user
            var test = _currentUser;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _assetManager.GetAsset(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/AssetTypes/5
        [HttpPut("{id}")]
        public IActionResult PutAssetType([FromRoute] int id, [FromBody] Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asset.AssetId)
            {
                return BadRequest();
            }

            var response = _assetManager.UpdateAsset(asset);

            if(response.Status != ResponseStatus.Success)
            {
                if(!AssetTypeExists(id).Data)
                {
                    return NotFound();
                }
            }

            return Ok(response);
        }

        // POST: api/AssetTypes
        [HttpPost]
        public IActionResult PostAssetType([FromBody] Asset assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _assetManager.AddAsset(assetType);

            return Ok(response);
        }

        // DELETE: api/AssetTypes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAssetType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _assetManager.DeleteAsset(id);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        private Response<bool> AssetTypeExists(int id)
        {
            return _assetManager.AssetExists(id);  
        }
    }

    public static class IHttpContextAccessorExtension
    {
        public static int CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}