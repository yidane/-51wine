﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Travel.Infrastructure.DomainDataAccess.Coupon.Entitys;
using Travel.Infrastructure.DomainDataAccess.User;
using System.Transactions;

namespace Travel.Infrastructure.DomainDataAccess.Coupon
{
    public class CouponRepository
    {

        private static object obj = new object();
        public CouponRepository()
        {


        }

        public Entitys.CouponUsage GetCouponByUser(Guid couponUsageId, string openid)
        {
            Entitys.CouponUsage result = null;
            var db = new TravelDBContext();
            //using (var db = new TravelDBContext())
            //{



            var coupon = db.CouponUsage
              .FirstOrDefault(item => item.CouponUsageId == couponUsageId && item.OpenId == openid);
            result = coupon;

            //}


            return result;
        }

        public List<Entitys.CouponUsage> GetUnExpiredCoupons(UserInfo user)
        {
            List<Entitys.CouponUsage> result = null;
            var db = new TravelDBContext();
            //using (var db = new TravelDBContext())
            //{
            //   var typeId = Guid.Parse("63313E55-A213-4B38-AF64-E6F2ABF68E56");
            var nowTime = DateTime.Now;
            var coupons = db.CouponUsage.Where(item => item.State == CouponState.NotUsed
             && item.OpenId == user.openid &&
           item.Coupon.EndTime >= nowTime
          );
            result = coupons.ToList();
            //var coupons = db.Coupon.Where(item => item.State== CouponState.NotUsed &&
            //item.EndTime >= nowTime
            //&&item.Type.CouponTypeId== typeId)
            //.Join(db.CouponUsage.Where(item => item.OpenId == user.openid)
            //, c => c.CouponId, u => u.CouponId, (c, u) => c);
            //result = coupons.ToList();

            //}


            return result;
        }

        public List<Entitys.CouponUsage> GetExpiredCoupons(UserInfo user)
        {
            List<Entitys.CouponUsage> result = null;
            var db = new TravelDBContext();
            //using (var db = new TravelDBContext())
            //{
            //   var typeId = Guid.Parse("63313E55-A213-4B38-AF64-E6F2ABF68E56");
            var nowTime = DateTime.Now;
            var coupons = db.CouponUsage.Where(item => item.State == CouponState.NotUsed
             && item.OpenId == user.openid
            && item.Coupon.EndTime < nowTime
            );
            //var coupons = db.Coupon.Where(item => item.State == CouponState.NotUsed &&
            //    item.EndTime < nowTime
            //    && item.Type.CouponTypeId == typeId).Join(db.CouponUsage.Where(item => item.OpenId == user.openid)
            //    , c => c.CouponId, u => u.CouponId, (c, u) => c);
            result = coupons.ToList();

            //}

            return result;
        }

        public List<Entitys.CouponUsage> GetUsedCoupons(UserInfo user)
        {
            List<Entitys.CouponUsage> result = null;
            var db = new TravelDBContext();
            //using (var db = new TravelDBContext())
            //{
            // var typeId = Guid.Parse("63313E55-A213-4B38-AF64-E6F2ABF68E56");
            var nowTime = DateTime.Now;
            var coupons = db.CouponUsage.Where(item => item.State == CouponState.Used
            && item.OpenId == user.openid
         );
            //result = db.Coupon.Where(item => item.State == CouponState.Used && 
            //item.Type.CouponTypeId == typeId).Join(db.CouponUsage.Where(item => item.OpenId == user.openid)
            //    , c => c.CouponId, u => u.CouponId, (c, u) => c).ToList();

            result = coupons.ToList();
            //}

            return result;
        }

        /// <summary>
        /// 判断是否还有优惠券
        /// </summary>
        /// <returns></returns>
        public bool HasCoupon()
        {
            var result = false;
            using (var db = new TravelDBContext())
            {
                var nowTime = DateTime.Now;
                var coupons = GetAvailableCoupon();
                result = coupons.Any();

            }
            //var nowTime = DateTime.Now;
            //var coupons = _testCoupon.Where(item => item.BeginTime <= nowTime && item.EndTime >= nowTime);
            //result = coupons.Any();
            return result;
        }

        /// <summary>
        ///  判断是否参与过摇奖
        /// </summary>
        /// <returns></returns>
        public bool BeParticipatedInCoupon(string openId)
        {
            var result = false;
            using (var db = new TravelDBContext())
            {
                var nowTime = DateTime.Now;
                //获取现有优惠券

                var coupon = db.Coupon.FirstOrDefault(item =>
                item.BeginTime <= nowTime &&
                item.EndTime >= nowTime);

                if (coupon != null)
                {
                    var usage = db.CouponUsage.FirstOrDefault(item =>
                 item.ReceivedTime >= coupon.BeginTime &&
                 item.ReceivedTime <= coupon.EndTime && item.OpenId
                 == openId);
                    if (usage != null)
                    {
                        result = true;
                    }
                }




            }
            //var nowTime = DateTime.Now;
            //var coupons = _testCoupon.Where(item => item.BeginTime <= nowTime && item.EndTime >= nowTime);
            //result = coupons.Any();
            return result;
        }

        /// <summary>
        /// 获取现有可用的优惠券
        /// </summary>
        /// <returns></returns>
        public List<Entitys.Coupon> GetAvailableCoupon()
        {
            List<Entitys.Coupon> result = new List<Entitys.Coupon>();
            var db = new TravelDBContext();

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            //      new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
            //{

                //using (var db = new TravelDBContext())
                //{
                var nowTime = DateTime.Now;
                //获取现有优惠券

                var coupons = db.Coupon.Where(item =>
                item.BeginTime <= nowTime &&
                item.EndTime >= nowTime);


                //获取优惠券使用数量
                //todo:方法有问题，返回的是空值

                //var dt =  from couponUsage in db.CouponUsage
                //    join coupon in coupons on couponUsage.CouponId equals coupon.CouponId
                //        into cu
                //    from u in cu.DefaultIfEmpty()
                //    select new
                //    {
                //        CouponId = u.CouponId,
                //        OpenId = couponUsage.OpenId
                //    };



                var usageCount = db.CouponUsage
                .Join(coupons, u => u.CouponId,
                c => c.CouponId, (u, c) => u).GroupBy(u => u.CouponId)
                .Select(u => new
                {
                    CouponId = u.Key,
                    Count = u.Count() == null ? 0 : u.Count()
                });

                var da = from c in coupons
                         join u in usageCount on c.CouponId equals u.CouponId
                             into cu
                         from cu1 in cu.DefaultIfEmpty()
                         where c.StockQuantity > (cu1.Count == null ? 0 : cu1.Count)
                         select new CouponWithCount()
                         {
                             Count = c.StockQuantity - (cu1.Count == null ? 0 : cu1.Count),
                             Coupon = c
                         };
                foreach (var cc in da)
                {
                    for (int i = 0; i < cc.Count; i++)
                    {
                        result.Add(cc.Coupon);
                    }
                }
            //    scope.Complete();
            //}

            //result = da.ToList();

            //            result = coupons.Join(usageCount, c => c.CouponId,
            //                u => u.CouponId, (c, u) => new { c = c, u = u })
            //                .Where(item => item.c.StockQuantity > item.u.Count)
            //                .Select(item => item.c).ToList();
            //}
            //var nowTime = DateTime.Now;
            //result = _testCoupon.Where(item => item.BeginTime <= nowTime && item.EndTime >= nowTime).ToList();

            return result;
        }


        /// <summary>
        /// 随机优惠券
        /// </summary>
        /// <param name="availableCoupons"></param>
        /// <returns></returns>
        public Entitys.Coupon GetRandomCoupon(List<Entitys.Coupon> availableCoupons)
        {
            Entitys.Coupon result = null;


            //lock (obj)
            //{
            //取随机数
            Random r = new Random();
            int i = r.Next(0, availableCoupons.Count);


            //using (var db = new TravelDBContext())
            //{
            //    //得到随机的优惠券
            //    result = availableCoupons[i];
            //    result.Type = db.CouponType.Find(result.CouponTypeId);
            //}
            result = availableCoupons[i];
            //}




            return result;
        }

        /// <summary>
        /// 绑定优惠券到用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="coupon"></param>
        public void AddCouponToUser(string openid, Entitys.Coupon coupon)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                  new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                using (var db = new TravelDBContext())
                {
                    var usage = new CouponUsage()
                    {
                        CouponUsageId = Guid.NewGuid(),
                        CouponId = coupon.CouponId,
                        OpenId = openid,
                        ReceivedTime = DateTime.Now
                    };
                    db.CouponUsage.Add(usage);
                    db.SaveChanges();
                }
                scope.Complete();
            }

        }

        public void UseCoupon(string openId, Guid id)
        {
            using (var db = new TravelDBContext())
            {
                var usage = db.CouponUsage.FirstOrDefault(p => p.CouponUsageId == id && p.OpenId == openId);

                if (usage != null)
                {

                    usage.State = CouponState.Used;
                    usage.UsedTime = DateTime.Now;
                    db.Entry(usage).State = System.Data.Entity.EntityState.Modified;
                    // db.Coupon.Attach(coupon);
                    db.SaveChanges();
                }
            }
        }



        public Entitys.Coupon GetCoupon(Guid couponId)
        {
            Entitys.Coupon result = null;
            using (var db = new TravelDBContext())
            {
                result = db.Coupon.Find(couponId);
            }
            // result = _testCoupon.Where(item=>item.CouponId==couponId).FirstOrDefault();
            return result;
        }
    }
}