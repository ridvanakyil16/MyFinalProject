using Business.Abstract;
using Core.Business;
using Core.Utilities.Helper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductImageManager : IProductImageService
    {
        IProductImageDal _ProductImageDal;
        public ProductImageManager(IProductImageDal ProductImageDal)
        {
            _ProductImageDal = ProductImageDal;
        }

        public IResult Add(IFormFile file, ProductImage ProductImage)
        {

            IResult result = BusinessRules.Run(CheckImageLimitExceeded(ProductImage.ProductId));
            if (result != null)
            {
                return result;
            }
            var result2 = FileHelper.Add(file);
            if (!result2.Succsess)
            {
                return new ErrorResult(result2.Message);
            }
            var deneme = ProductImage.ImagePath;
            ProductImage.ImagePath = result2.Message;
            ProductImage.ImageDate = DateTime.Now;
            _ProductImageDal.Add(ProductImage);
            return new SuccessResult();
        }


        public IResult Delete(ProductImage ProductImage)
        {
            FileHelper.Delete(ProductImage.ImagePath);
            _ProductImageDal.Delete(ProductImage);
            return new SuccessResult();
        }
        public IDataResult<List<ProductImage>> GetAll()
        {

            return new SuccessDataResult<List<ProductImage>>(_ProductImageDal.GetAll());
        }

        public IDataResult<ProductImage> Get(int ProductImageId)
        {
            return new SuccessDataResult<ProductImage>(_ProductImageDal.Get(p => p.Id == ProductImageId));
        }


        public IResult Update(IFormFile file, ProductImage ProductImage)
        {
            var isImage = _ProductImageDal.Get(c => c.Id == ProductImage.Id);
            if (isImage == null)
            {
                return new ErrorResult("Image not found");
            }

            var updatedFile = FileHelper.Update(file, isImage.ImagePath);
            if (!updatedFile.Succsess)
            {
                return new ErrorResult(updatedFile.Message);
            }
            ProductImage.ImagePath = updatedFile.Message;
            ProductImage.ImageDate = DateTime.Now;
            _ProductImageDal.Update(ProductImage);
            return new SuccessResult();
        }
        private IResult CheckImageLimitExceeded(int productId)
        {
            //1 arabanın en fazla 5 resmi olabilir dediğimiz kod bloğu.
            var ProductImageCount = _ProductImageDal.GetAll(p => p.ProductId == productId).Count;
            if (ProductImageCount >= 5)
            {
                return new ErrorResult();
            }

            return new SuccessResult();
        }
        private List<ProductImage> CheckIfProductImageNull(int id)
        {
            //Arabanın resmi yoksa default resim koyulması için yazdığımzı kod bloğu.
            string path = @"\Uploads\Images\Default.png";
            var result = _ProductImageDal.GetAll(c => c.ProductId == id).Any();
            if (!result)
            {
                return new List<ProductImage> { new ProductImage { ProductId = id, ImagePath = path, ImageDate = DateTime.Now } };
            }
            return _ProductImageDal.GetAll(p => p.ProductId == id);
        }

        public IDataResult<List<ProductImage>> GetImagesByCarId(int id)
        {
            return new SuccessDataResult<List<ProductImage>>(CheckIfProductImageNull(id));
        }
    }
}
