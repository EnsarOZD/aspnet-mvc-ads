using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ads.Data.Entities;

namespace Ads.Services.Services.Abstract
{
    public interface IAdvertImageService
    {
        IEnumerable<AdvertImageEntity> GetAllImages();
        AdvertImageEntity GetImageById(int id);
        void AddImage(AdvertImageEntity image);
        void UpdateImage(AdvertImageEntity image);
        void DeleteImage(int id);
        void GetAdvertTitle(int advertId);

    }
}
