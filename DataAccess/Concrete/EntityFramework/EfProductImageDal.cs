using Core.DataAccsess.EnitiyFrameWork;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductImageDal : EfEntityRepositoryBase<ProductImage, NorthwindContext>, IProductImageDal
    {
    }
}
