using DataAccess.Abstract;
using System;
using System.Text;
using System.Linq;
using Core.DataAccsess.EnitiyFrameWork;
using Core.Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, NorthwindContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var result = from op in context.OperationClaims
                             join uop in context.UserOperationClaims
                             on op.Id equals uop.OperationClaimId
                             where uop.UserId == user.Id
                             select new OperationClaim { Id = op.Id, Name = op.Name };
                return result.ToList();
            }
        }
    }
}
